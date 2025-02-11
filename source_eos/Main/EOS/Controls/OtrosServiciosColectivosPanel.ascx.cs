//Qurius (EAS) 24/01/2011
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI;
using EOS.Entidades.Datos;
using EOS.Web;

using System.Web.UI.WebControls;
using System.Globalization;

namespace EOS.Controls
{
    public partial class OtrosServiciosColectivosPanel : UserControl
    {
        protected int? currentidreserva = null;
        protected int? CurrentIdReserva
        {
            get
            {
                if (Request.QueryString["idres"] != null)
                {
                    currentidreserva = int.Parse(Request.QueryString["idres"]);
                }
                else if (HttpContext.Current.Session["idreserva"] != null)
                {
                    currentidreserva = int.Parse(HttpContext.Current.Session["idreserva"].ToString());
                }

                return currentidreserva;
            }
            set { currentidreserva = value; }
        }

        protected int currentidservicio = 0;
        protected int CurrentIdServicio
        {
            get { return currentidservicio; }
            set { currentidservicio = value; }
        }

        private bool _NuevoServicio;
        protected bool NuevoServicio
        {
            get
            {
                if (HttpContext.Current.Session["nuevoServicio"] != null)
                {
                    _NuevoServicio = Convert.ToBoolean(HttpContext.Current.Session["nuevoServicio"].ToString());
                }
                return _NuevoServicio;
            }
            set { _NuevoServicio = value; }
        }

        private DCabeceraExpedienteAmpliado expediente
        {
            get
            {
                if (HttpContext.Current.Session["currentExpediente"] != null)
                {
                    return (DCabeceraExpedienteAmpliado) HttpContext.Current.Session["currentExpediente"];
                }

                AgenteExpedientes agenteExp = new AgenteExpedientes();
                DCabeceraExpedienteAmpliado value = agenteExp.ObtenerExpedientePorID(Request.QueryString["idexp"]);
                HttpContext.Current.Session["currentFKIdCongreso"] = value.Idactividad;

                return value;
            }
        }
        private bool showAlternative;
        private string state = string.Empty;
        private string statename = string.Empty;
        private string desde = string.Empty;
        private string hasta = string.Empty;
        private string desdehora = string.Empty;
        private string hastahora = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //TipoActividad tipoactividad = (TipoActividad)Session["tipoactividad"];
            if (Request.QueryString["tab"] == "3" && expediente.Idtiporeserva == 2)
            {
                if (!IsPostBack)
                {
                    //Ismael Ameller 24/02/2011 Recuperamos el combo tipo de servicio de la base de datos
                    GetTipoDeServicio();
                    //FIN Ismael Ameller 24/02/2011 Recuperamos el combo tipo de servicio de la base de datos
                    showAlternative = (Request.QueryString["view"] != null && Request.QueryString["view"] == "alternativa");

                    if (Request.QueryString["idres"] == null)
                    {
                        if (expediente.Idactividad.HasValue)
                        {
                            GetFechasReservas(Convert.ToString(expediente.Idactividad.Value));
                        }

                        setFechas();
                    }
                    else
                    {
                        CurrentIdReserva = Convert.ToInt32(Request.QueryString["idres"]);

                        GetServicioReserva();

                        Alternativas.Visible = showAlternative;
                        Alternativas.IdServicio = Convert.ToString(CurrentIdServicio);
                        //Alternativas.IdServicio = !showAlternative ? CurrentIdReserva.ToString() : currentidservicio.ToString();

                        // Buscamos los datos básicos del expediente que se pasa por la url
                        LoadReserva(CurrentIdReserva);
                    }


                }

                //if (tipoactividad == TipoActividad.OtrosServicios) 
                SetButtons();

                // Muestra el estado
                lblEstado.Text = statename;
                lblEstado.CssClass = string.Format("eosImagenEstado eosImagenLeyenda{0}", state);


            }

        }
        //Ismael Ameller 24/02/2011 Recuperamos el combo tipo de servicio de la base de datos
        private void GetTipoDeServicio()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            const string consulta = "SELECT descripcion FROM cv_tipo_servicio";
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();
                cboTipo.DataSource = rea;
                cboTipo.DataTextField = "descripcion";
                cboTipo.DataValueField = "descripcion";
                cboTipo.DataBind();
                cboTipo.Items.Insert(0, "Seleccione una opción");

                if (!string.IsNullOrWhiteSpace(Request.QueryString["tipo"])) {
                    cboTipo.SelectedValue = Request.QueryString["tipo"];
                }
               // cboTipo.Items.Add("Otro");
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
        }
        //FIN Ismael Ameller 24/02/2011 Recuperamos el combo tipo de servicio de la base de datos
        private void GetFechasReservas(string idCongreso)
        {
            string consulta = "SELECT idCongreso, Desde, Hasta FROM tiposcongreso tc";
            consulta += " RIGHT JOIN congresos c ON c.idtipocongreso = tc.idtipocongreso";
            consulta += " where idCongreso = " + idCongreso + ";";

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();
                if (rea.Read())
                {
                    DateTime desdedt = Convert.ToDateTime(rea["Desde"].ToString());
                    desde = desdedt.ToShortDateString();
                    desdehora = desdedt.ToShortTimeString();
                    if (desdehora.IndexOf(":", StringComparison.InvariantCulture) == 1) desdehora = "0" + desdehora;

                    DateTime hastadt = Convert.ToDateTime(rea["Hasta"].ToString());
                    hasta = hastadt.ToShortDateString();
                    hastahora = hastadt.ToShortTimeString();
                    if (hastahora.IndexOf(":", StringComparison.InvariantCulture) == 1) hastahora = "0" + hastahora;
                }
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                //you need some kind of error logging...
            }
            finally
            {
                conn.Close();
            }

        }

        private void SetState(string _state)
        {
            // Actualizar el estado del servicio de alojamiento
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                string consulta = "UPDATE reservasviajes SET idestado = '" + _state + "', LastUpd = getdate() WHERE idreserva = " + CurrentIdReserva + ";";
                SqlCommand comm = new SqlCommand(consulta, conn);
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
        }

        public void SetButtons()
        {
            btnNuevo.Visible = false;
            btnGuardar.Visible = false;
            btnEnviar.Visible = false;
            btnRechazar.Visible = false;
            btnAprobar.Visible = false;
            btnCancelar.Visible = false;
            switch (state)
            {
                case "AB": //sin enviar
                    btnGuardar.Visible = true;
                    //Ismael Ameller 08-03-2011 Controles enable segun si sale el boton guardar o no
                    ControlesEnable(true);
                    //si es nuevo (no guardado) no hay que mostrar el boton enviar
                    if (currentidservicio == 0 && (currentidreserva == 0 || currentidreserva == null))
                        btnEnviar.Visible = false;
                    else
                        btnEnviar.Visible = true;
                    btnRechazar.Visible = true;
                    btnAprobar.Visible = false;
                    btnCancelar.Visible = false;
                    btnNuevo.Visible = true;
                    break;
                case "ABI": //abierto
                    break;
                case "AC": //aceptada
                case "ACP": //aceptada
                case "CFP": //aceptado
                    btnRechazar.Visible = true;
                    btnGuardar.Visible = false;
                    //Ismael Ameller 08-03-2011 Controles enable segun si sale el boton guardar o no
                    ControlesEnable(false);
                    btnEnviar.Visible = false;
                    btnAprobar.Visible = false;
                    btnCancelar.Visible = false;
                    break;
                case "AN": //rechazado
                case "CTZD": //cotizando
                case "PTR": //tramitando    
                    btnGuardar.Visible = false;
                    //Ismael Ameller 08-03-2011 Controles enable segun si sale el boton guardar o no
                    ControlesEnable(false);
                    btnEnviar.Visible = false;
                    btnRechazar.Visible = false;
                    btnAprobar.Visible = false;
                    btnCancelar.Visible = false;
                    break;
                case "CER": //cerrado
                    break;
                case "CN": //cancelado
                case "CNTR": //cancelado con Gastos
                    break;
                case "CR": //enviado
                    btnGuardar.Visible = true;
                    //Ismael Ameller 08-03-2011 Controles enable segun si sale el boton guardar o no
                    ControlesEnable(true);
                    btnEnviar.Visible = false;
                    btnRechazar.Visible = true;
                    btnAprobar.Visible = false;
                    btnCancelar.Visible = false;
                    btnNuevo.Visible = false;
                    break;
                case "FZ": //finalizado
                    break;
                case "MDF": //MDF
                    break;
                case "NC": // en curso
                    break;
                case "CTZ": //cotizado
                    btnGuardar.Visible = false;
                    //Ismael Ameller 08-03-2011 Controles enable segun si sale el boton guardar o no
                    ControlesEnable(false);
                    btnEnviar.Visible = false;
                    btnRechazar.Visible = true;
                    btnAprobar.Visible = true;
                    btnCancelar.Visible = false;
                    break;
                case "TR": //tramitado
                    btnGuardar.Visible = false;
                    ControlesEnable(false);
                    btnEnviar.Visible = false;
                    btnRechazar.Visible = false;
                    btnAprobar.Visible = false;
                    btnCancelar.Visible = true;
                    break;
                case "":
                    if (currentidreserva == 0 || currentidreserva == null)
                    {
                        btnGuardar.Visible = true;
                        //Ismael Ameller 08-03-2011 Controles enable segun si sale el boton guardar o no
                        ControlesEnable(true);
                    }
                    break;
            }
        }

        private void GetServicioReserva()
        {
            // Recupera el id de servicio a partir del id de reserva
            //string consulta = "SELECT * FROM serviciosreservasactividades SH LEFT JOIN serviciosreservasviajes SR ON SH.idservicioactividad = SR.idservicioactividad WHERE SR.idreserva = " + CurrentIdReserva;
            string consulta = "SELECT * FROM serviciosreservasactividades SH LEFT JOIN serviciosreservasviajes SR ON SH.idservicioactividad = SR.idservicioactividad LEFT JOIN reservasviajes RV ON SR.idreserva = RV.idreserva WHERE SR.idreserva = " + CurrentIdReserva;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();
                if (rea.Read())
                {
                    CurrentIdServicio = int.Parse(rea["idservicioactividad"].ToString());
                    //Jose Laguna 06-04-2010 Esto lo añadimos para que no pierda el estado cuando modificas la alternativa
                    state = rea["idEstado"] != DBNull.Value ? rea["idEstado"].ToString() : "";
                    statename = GetStateName();
                }
                else
                {
                    state = "";
                    CurrentIdServicio = 0;
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                CurrentIdServicio = 0;
                Console.WriteLine(ex.ToString());
                conn.Close();
            }
        }

        private int GetNextIdServicioReserva()
        {
            int id = 0;
            const string consulta = "SELECT MAX(idservicio) FROM serviciosreservasviajes";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);

            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();
                if (rea.Read())
                {
                    id = rea.GetInt32(0);
                }
                conn.Close();
                return id + 1;
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
                conn.Close();
                return 0;
            }
        }

        private void LoadReserva(int? _currentIdReserva)
        {
            if (!_currentIdReserva.HasValue)
                return;

            string consulta = "SELECT " +
                              "serviciosreservasactividades.idservicioactividad, " +
                              "serviciosreservasactividades.idtarifaactividad, " +
                              "serviciosreservasactividades.observaciones, " +
                              "serviciosreservasactividades.pvp, " +
                              "serviciosreservasactividades.locked, " +
                              "serviciosreservasactividades.sede, " +
                              "serviciosreservasactividades.Descripcion, " +
                              "serviciosreservasactividades.tipo, " +
                              "serviciosreservasactividades.fechainicio, " +
                              "serviciosreservasactividades.horainicio, " +
                              "serviciosreservasactividades.minutosinicio, " +
                              "serviciosreservasactividades.fechafin, " +
                              "serviciosreservasactividades.horafin, " +
                              "serviciosreservasactividades.minutosfin, " +
                              "reservasviajes.idestado, " +
                              "serviciosreservasactividades.pax, " +
                              "reservasviajes.fkidexpediente as idexpediente " +
                              "FROM serviciosreservasactividades " +
                              "LEFT JOIN serviciosreservasviajes ON serviciosreservasactividades.idservicioactividad = serviciosreservasviajes.idservicioactividad " +
                              "LEFT JOIN reservasviajes ON reservasviajes.idreserva = serviciosreservasviajes.idreserva " +
                              "WHERE reservasviajes.idreserva = " + _currentIdReserva;

            using(SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand(consulta, conn);
                    SqlDataReader rea = comm.ExecuteReader();

                    if(!rea.HasRows)
                        return;

                    while (rea.Read())
                    {
                        
                        string idexped = rea[16] != DBNull.Value ? rea[16].ToString() : string.Empty;
                        if (Convert.ToInt32(idexped) != expediente.Idexpediente)
                        {
                            //no hay que pasar esto... hay que mostrar un error
                            Alert.Show("Esta tratando de acceder a una reserva no válido", "Expedientes.aspx");
                        }

                        CurrentIdServicio = rea[0] != DBNull.Value ? rea.GetInt32(0) : 0;
                        state = rea["idestado"].ToString();
                        statename = GetStateName();
                        descripcionTextBox.Text = rea[6] != DBNull.Value ? rea.GetString(6) : string.Empty;
                        string sValor = rea[7] != DBNull.Value ? rea.GetString(7) : string.Empty;
                        cboTipo.SelectedItem.Value = (cboTipo.Items.FindByValue(sValor) != null) ? sValor : cboTipo.Items[0].Value;
                        cboTipo.SelectedItem.Text = (cboTipo.Items.FindByValue(sValor) != null) ? sValor : cboTipo.Items[0].Value;

                        if (sValor.ToLower() == "otro")
                            otroTipoTextBox.Text = rea[7] != DBNull.Value ? rea.GetString(7) : string.Empty;

                        sedeTextBox.Text = rea[5] != DBNull.Value ? rea.GetString(5) : string.Empty;
                        importeTextBox.Text = rea[3] != DBNull.Value ? Convert.ToString(rea.GetDouble(3)) : string.Empty;
                        fechaInicioInputDatePickerControl.Value = rea[8] != DBNull.Value ? rea.GetDateTime(8).ToShortDateString() : string.Empty;
                        string horaIni = rea[9] != DBNull.Value ? rea.GetString(9) : string.Empty;
                        string minutosIni = rea[10] != DBNull.Value ? rea.GetString(10) : string.Empty;
                        horaInicioTimeInputBox.Value = horaIni + ":" + minutosIni;
                        fechaFinalInputDatePickerControl.Value = rea[11] != DBNull.Value ? rea.GetDateTime(11).ToShortDateString() : string.Empty;
                        string horaFin = rea[12] != DBNull.Value ? rea.GetString(12) : string.Empty;
                        string minutosFin = rea[13] != DBNull.Value ? rea.GetString(13) : string.Empty;
                        horaFinalTimeInputBox.Value = horaFin + ":" + minutosFin;
                        personasTextBox.Text = rea[15] != DBNull.Value ? rea.GetInt32(15).ToString() : string.Empty;
                        txtObservaciones.Text = rea[2] != DBNull.Value ? rea.GetString(2) : string.Empty;
                    }

                    rea.Close();
                    rea.Dispose();
                }
                catch (Exception ex)
                {
                    //Ismael Ameller 09-03-2011 Envio de Mail
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                    //FIN Ismael Ameller 09-03-2011 Envio de Mail
                    Console.WriteLine(ex.ToString());
                } 
            }
        }


        private void setFechas()
        {
            fechaInicioInputDatePickerControl.Enabled = true;
            fechaInicioInputDatePickerControl.ClickImageButton.Enabled = true;
            fechaInicioInputDatePickerControl.InputTextBox.Enabled = true;
            fechaInicioInputDatePickerControl.InputTextBox.Text = desde;
            //fechaInicioInputDatePickerControl.Value = string.Empty;
            horaInicioTimeInputBox.Enabled = true;
            horaInicioTimeInputBox.InputTextBox.Enabled = true;
            //horaInicioTimeInputBox.Value = string.Empty;
            horaInicioTimeInputBox.InputTextBox.Text = desdehora;

            fechaFinalInputDatePickerControl.Enabled = true;
            fechaFinalInputDatePickerControl.ClickImageButton.Enabled = true;
            fechaFinalInputDatePickerControl.InputTextBox.Enabled = true;
            //fechaFinalInputDatePickerControl.Value = string.Empty;
            fechaFinalInputDatePickerControl.InputTextBox.Text = hasta;
            horaFinalTimeInputBox.Enabled = true;
            horaFinalTimeInputBox.InputTextBox.Enabled = true;
            //horaFinalTimeInputBox.Value = string.Empty;
            horaFinalTimeInputBox.InputTextBox.Text = hastahora;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        public void Guardar()
        {
            int nextid = GetNextIdServicio();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            SqlTransaction trans = null;

            if (!ComprobarCamposOK())
            {
                return;
            }

            string fechaIni = fechaInicioInputDatePickerControl.Value == "" ? "NULL" : fechaInicioInputDatePickerControl.Value;

            if (fechaIni != "NULL")
            {
                DateTime inidt = Convert.ToDateTime(fechaIni);
                fechaIni = String.Format("{0:yyyyMMdd}", inidt);
            }
            string horaIni = horaInicioTimeInputBox.Value == "" ? "NULL" : GetHour(horaInicioTimeInputBox.Value);
            string minutosIni = horaInicioTimeInputBox.Value == "" ? "NULL" : GetMinutes(horaInicioTimeInputBox.Value);
            string fechaFin = fechaFinalInputDatePickerControl.Value == "" ? "NULL" : fechaFinalInputDatePickerControl.Value;
            if (fechaFin != "NULL")
            {
                DateTime findt = Convert.ToDateTime(fechaFin);
                fechaFin = String.Format("{0:yyyyMMdd}", findt);
            }
            string horaFin = horaFinalTimeInputBox.Value == "" ? "NULL" : GetHour(horaFinalTimeInputBox.Value);
            string minutosFin = horaFinalTimeInputBox.Value == "" ? "NULL" : GetMinutes(horaFinalTimeInputBox.Value);
            string consulta;

            string sTipo = string.Empty;
            if (cboTipo.SelectedIndex > 0)
                sTipo = cboTipo.SelectedValue.ToLower() == "otro" ? otroTipoTextBox.Text : cboTipo.SelectedValue;

            if(string.IsNullOrEmpty(sTipo) && !string.IsNullOrEmpty(cboTipo.SelectedValue.ToLower()))
            {
                sTipo = cboTipo.SelectedValue.ToLower() == "otro" ? otroTipoTextBox.Text : cboTipo.SelectedValue;
            }
            double val;
            double dImporte = Double.TryParse(importeTextBox.Text, NumberStyles.Number, CultureInfo.GetCultureInfo("es-ES").NumberFormat, out val) ? val: 0; 
            string sImporte = Convert.ToString(dImporte).Replace(",", "@");


            if (CurrentIdReserva == null || NuevoServicio)
            {
                conn.Open();
                string consultaIdConfEmpresa = string.Format("select idconfempresa from amec where idamec = (select top 1 idamec from expediente where idxpediente = {0})", expediente.Idexpediente);
                SqlCommand commandIdConfEmpresa = new SqlCommand(consultaIdConfEmpresa, conn);
                string idConfEmpresa = commandIdConfEmpresa.ExecuteScalar().ToString();

                //INSERT 
                consulta = string.Format("INSERT INTO serviciosreservasactividades VALUES ({0},{1},'{2}',{3},{4},'{5}','{6}','{7}',{8},{9},{10},{11},{12},{13},{14},{15})", +
                                                                                                                                                                              nextid,
                                         "null",
                                         txtObservaciones.Text.Replace("'", ""),
                                         sImporte.Replace("@", "."),
                                         0,
                                         (sedeTextBox.Text.Length > 60 ? sedeTextBox.Text.Substring(0, 59).Replace("'", "") : sedeTextBox.Text.Replace("'", "")),
                                         descripcionTextBox.Text.Replace("'", ""),
                                         sTipo,
                                         fechaIni == "NULL" ? "null" : "'" + fechaIni + "'",
                                         horaIni == "NULL" ? "null" : "'" + horaIni + "'",
                                         minutosIni == "NULL" ? "null" : "'" + minutosIni + "'",
                                         fechaFin == "NULL" ? "null" : "'" + fechaFin + "'",
                                         horaFin == "NULL" ? "null" : "'" + horaFin + "'",
                                         minutosFin == "NULL" ? "null" : "'" + minutosFin + "'",
                                         personasTextBox.Text == "" ? 0 : Convert.ToInt32(personasTextBox.Text), idConfEmpresa);


                try
                {
                    trans = conn.BeginTransaction(System.Data.IsolationLevel.Serializable);
                    SqlCommand comm = new SqlCommand(consulta, conn, trans);
                    comm.ExecuteNonQuery();
                    int nextreserva = GetNextIdReserva();
                    CurrentIdReserva = nextreserva;
                    HttpContext.Current.Session.Add("idreserva", nextreserva);
                    HttpContext.Current.Session.Add("nuevoServicio", false);
                    InsertReserva(conn, trans);
                    InsertServiciosReservasViajes(nextid, nextreserva, conn, trans);

                    trans.Commit();

                    //InsertPassengerList();  
                    CurrentIdReserva = nextreserva;
                    lvServicioActividades.DataBind();
                    state = "AB";
                    statename = "Sin Enviar";
                    //SetButtons();

                    ShowMessage("El servicio se ha creado correctamente.", "DetalleExpediente.aspx?idexp=" + Convert.ToString(expediente.Idexpediente));

                    //Pau Ferrer 16-06-2011
                    //Se tiene que cargar la reserva que acabamos de crear.
                    LoadReserva(CurrentIdReserva);

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                       

                    //Ismael Ameller 09-03-2011 Envio de Mail
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                    //FIN Ismael Ameller 09-03-2011 Envio de Mail
                    Console.WriteLine(ex.ToString());
                    ShowMessage("Se ha producido un error al crear el servicio.", "");
                }
                conn.Close();
            }
                //UPDATE
            else
            {
                consulta = "UPDATE serviciosreservasactividades  " +
                           "SET idtarifaactividad = NULL, " +
                           "observaciones = '" + txtObservaciones.Text.Replace("'", "") + "'," +
                           "pvp = " + sImporte.Replace("@", ".") + "," +
                           "serviciosreservasactividades.locked = 0, " +
                           "sede = '" + (sedeTextBox.Text.Length > 60 ? sedeTextBox.Text.Substring(0, 59).Replace("'", "") : sedeTextBox.Text.Replace("'", "")) + "', " +
                           "descripcion = '" + descripcionTextBox.Text.Replace("'", "") + "', " +
                           "tipo = '" + sTipo + "',";
                consulta += fechaIni == "NULL" ? "fechainicio = NULL, " : "fechainicio = '" + fechaIni + "', ";
                consulta += horaIni == "NULL" ? "horainicio = NULL, " : "horainicio = '" + horaIni + "', ";
                consulta += minutosIni == "NULL" ? "minutosinicio = NULL, " : "minutosinicio = '" + minutosIni + "', ";
                consulta += fechaFin == "NULL" ? "fechafin = NULL, " : "fechafin = '" + fechaFin + "', ";
                consulta += horaFin == "NULL" ? "horafin = NULL, " : "horafin = '" + horaFin + "', ";
                consulta += minutosFin == "NULL" ? "minutosfin = NULL, " : "minutosfin = '" + minutosFin + "', ";
                consulta += personasTextBox.Text == "" ? "pax=0" : "pax=" + int.Parse(personasTextBox.Text);
                consulta += " FROM serviciosreservasactividades LEFT JOIN serviciosreservasviajes ON serviciosreservasviajes.idservicioactividad = serviciosreservasactividades.idservicioactividad ";
                consulta += "  WHERE serviciosreservasviajes.idreserva = " + CurrentIdReserva;

                try
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand(consulta, conn);
                    comm.ExecuteNonQuery();

                    if (state == "" && !string.IsNullOrWhiteSpace(Request.QueryString["idres"])) {
                        CurrentIdReserva = Convert.ToInt32(Request.QueryString["idres"]);

                        GetServicioReserva();
                    }

                    if (state == "CR")
                    {
                        SetState("AB");
                        state = "AB";
                    }
                    ShowMessage("El servicio se ha actualizado correctamente.", "DetalleExpediente.aspx?idexp=" + Convert.ToString(expediente.Idexpediente));

                    LoadReserva(CurrentIdReserva);
                    lvServicioActividades.DataBind();
                    //SetButtons();

                }
                catch (Exception ex)
                {
                    //Ismael Ameller 09-03-2011 Envio de Mail
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                    //FIN Ismael Ameller 09-03-2011 Envio de Mail
                    Console.WriteLine(ex.ToString());
                    ShowMessage("Se ha producido un error al actualizar el servicio.", "");
                }
                conn.Close();
            }
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                //Jose Laguna 11-05-2012 Añadimos las validaciones del estado y del presupuesto del AMEC para saber si la reserva se puede ENVIAR/APROBAR
                AgenteMaestros agente = new AgenteMaestros();
                if (!agente.ValidarReservaEstadoAmec(int.Parse(GetServicioByReserva())))
                    ShowMessage("El estado del AMEC no permite realizar el envío del servicio", "");
                // LJM 20180124 - Eliminar restricción  
                //else if (!agente.ValidarReservaImporteAmec(int.Parse(GetServicioByReserva())))
                //    ShowMessage("El servicio no se ha podido enviar ya que el importe superaría el presupuesto máximo del AMEC.", "");
                else
                {

                    Guardar();
                    SetState("CR");

                    //Inicio Generación de reserva asociada a los fees
                    AgenteUsuarios agenteUsuFee = new AgenteUsuarios();
                    AgenteExpedientes agenteExpFee = new AgenteExpedientes();
                    DVPeticionariosRoles datosRolesFee = agenteUsuFee.ObtenerDatosRolesPorLogin();
                    var datosExpFee = agenteExpFee.ObtenerExpedientePorID(expediente.Idexpediente.ToString());
                    DAmec damecFee = agenteExpFee.ObtenerEntidadAMECporID(datosExpFee.Idamec.ToString());
                    SqlConnection connFees = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
                    connFees.Open();
                    string idconfEmpresa = damecFee.idconfempresa == null ? "0" : damecFee.idconfempresa.Value.ToString();
                    string consulta = " exec dbo.sp_fee_generar_reserva " + expediente.Idexpediente.ToString() + ", " + datosRolesFee.IdPeticionario + ", " + idconfEmpresa + ";";
                    SqlCommand commFees = new SqlCommand(consulta, connFees);
                    commFees = new SqlCommand(consulta, connFees);
                    commFees.ExecuteNonQuery();
                    connFees.Close();
                    //Fin Generación de reserva asociada a los fees

                    Session["idreserva"] = null;
                    ShowMessage("Se ha enviado el servicio correctamente.", "DetalleExpediente.aspx?idexp=" + int.Parse(Request.QueryString["idexp"]));

                    //IAV ENVIAR MAIL

                    Mail mail = new Mail();
                    string body = "";
                    if (expediente.Tiporeserva == "IND")
                    {
                        body = "EXPEDIENTE INDIVIDUAL número: " + expediente.Idexpediente + "\n\n";
                    }
                    else if (expediente.Tiporeserva == "COL")
                    {
                        body = "EXPEDIENTE COLECTIVO número: " + expediente.Idexpediente + "\n\n";
                    }
                    body = body +
                                  "Tipo de Reserva: OTROS SERVICIOS - ACTIVIDAD \n" +
                                  "Id de servicio: " + Convert.ToString(CurrentIdServicio) + "\n" +
                                  "Descripción: " + descripcionTextBox.Text + "\n" +
                                  "Tipo: " + cboTipo.SelectedItem.Text + " " + otroTipoTextBox.Text + "\n" +
                                  "Número de personas: " + personasTextBox.Text + "\n" +
                                  "Sede: " + sedeTextBox.Text + "\n" +
                                  "Importe máximo: " + importeTextBox.Text + "\n" +
                                  "Fecha inicio: " + fechaInicioInputDatePickerControl.Value + "\n" +
                                  "Hora inicio: " + horaInicioTimeInputBox.Value + "\n" +
                                  "Fecha final: " + fechaFinalInputDatePickerControl.Value + "\n" +
                                  "Hora final: " + horaFinalTimeInputBox.Value + "\n";

                    AgenteUsuarios agenteUsu = new AgenteUsuarios();
                    DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
                    AgenteExpedientes agenteExp = new AgenteExpedientes();
                    var datosExp = agenteExp.ObtenerExpedientePorID(expediente.Idexpediente.ToString());

                    datosRoles.idempleadogp = agenteExp.ObtenerEmpleadoGpPorPetAmecExp(datosRoles.IdPeticionario, datosExp.Amec, datosExp.Idexpediente).IdEmpleadoGp;
                    DAmec damec = agenteExp.ObtenerEntidadAMECporID(datosExp.Idamec.ToString());
                    mail.EnvioReservaMail(body, Convert.ToString(expediente.Idexpediente), datosRoles.idempleadogp, true, ConfigUtil.GetAppSetting(Constantes.AppParams.CopiaContacto), damec.idconfempresa == null ? 0 : damec.idconfempresa.Value);
                }
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
            }
        }

        protected void btnCancelar_Click(object sender, ImageClickEventArgs e)
        {
            SetState("CN");
            Session["idreserva"] = null;
            ShowMessage("Se ha cancelado el servicio correctamente.", "DetalleExpediente.aspx?idexp=" + int.Parse(Request.QueryString["idexp"]));
        }

        protected void btnAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                //Jose Laguna 11-05-2012 Añadimos las validaciones del estado y del presupuesto del AMEC para saber si la reserva se puede ENVIAR/APROBAR
                AgenteMaestros agente = new AgenteMaestros();
                if (!agente.ValidarReservaEstadoAmec(int.Parse(GetServicioByReserva())))
                    ShowMessage("El estado del AMEC no permite realizar el envío del servicio", "");
                // LJM 20180124 - Eliminar restricción  
                //else if (!agente.ValidarReservaImporteAmec(int.Parse(GetServicioByReserva())))
                //    ShowMessage("El servicio no se ha podido enviar ya que el importe superaría el presupuesto máximo del AMEC.", "");
                else
                {

                    SetState("CFP");
                    Session["idreserva"] = null;

                    ShowMessage("Se ha aprobado el servicio correctamente.", "DetalleExpediente.aspx?idexp=" + int.Parse(Request.QueryString["idexp"]));

                    //Ismael Ameller 21-04-2011 Envio mail al aprobar
                    Mail mail = new Mail();
                    string body = "";
                    if (expediente.Tiporeserva == "IND")
                    {
                        body = "EXPEDIENTE INDIVIDUAL número: " + expediente.Idexpediente + "\n\n";
                    }
                    else if (expediente.Tiporeserva == "COL")
                    {
                        body = "EXPEDIENTE COLECTIVO número: " + expediente.Idexpediente + "\n\n";
                    }
                    body = body +
                                  "Tipo de Reserva: OTROS SERVICIOS - ACTIVIDAD \n" +
                                  "Id de servicio: " + Convert.ToString(CurrentIdServicio) + "\n" +
                                  "Descripción: " + descripcionTextBox.Text + "\n" +
                                  "Tipo: " + cboTipo.SelectedItem.Text + " " + otroTipoTextBox.Text + "\n" +
                                  "Número de personas: " + personasTextBox.Text + "\n" +
                                  "Sede: " + sedeTextBox.Text + "\n" +
                                  "Importe máximo: " + importeTextBox.Text + "\n" +
                                  "Fecha inicio: " + fechaInicioInputDatePickerControl.Value + "\n" +
                                  "Hora inicio: " + horaInicioTimeInputBox.Value + "\n" +
                                  "Fecha final: " + fechaFinalInputDatePickerControl.Value + "\n" +
                                  "Hora final: " + horaFinalTimeInputBox.Value + "\n";

                    AgenteUsuarios agenteUsu = new AgenteUsuarios();
                    DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
                    AgenteExpedientes agenteExp = new AgenteExpedientes();
                    var datosExp = agenteExp.ObtenerExpedientePorID(expediente.Idexpediente.ToString());

                    datosRoles.idempleadogp = agenteExp.ObtenerEmpleadoGpPorPetAmecExp(datosRoles.IdPeticionario, datosExp.Amec, datosExp.Idexpediente).IdEmpleadoGp;
                    DAmec damec = agenteExp.ObtenerEntidadAMECporID(datosExp.Idamec.ToString());
                    mail.EnvioReservaMail(body, Convert.ToString(expediente.Idexpediente), datosRoles.idempleadogp, false, ConfigUtil.GetAppSetting(Constantes.AppParams.CopiaContacto), damec.idconfempresa == null ? 0 : damec.idconfempresa.Value);
                    //FIN Ismael Ameller 21-04-2011 Envio mail al aprobar
                }
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
            }
        }

        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            SetState("AN");
            Session["idreserva"] = null;
            ShowMessage("Se ha rechazado el servicio correctamente.", "DetalleExpediente.aspx?idexp=" + int.Parse(Request.QueryString["idexp"]));

        }

        private void ShowMessage(string message, string navigation)
        {
            // Muestra mensaje y navega a una URL especificada
            navigation = (string.IsNullOrEmpty(navigation) ? "closeLoading()" : "document.location='" + navigation + "'");
            string script = String.Format("alert('{0}');{1};", message, navigation);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "dialog", script, true);
        }
        
        private string GetStateName()
        {
            // Recupera el id de servicio a partir del id de reserva
            string id = "--";
            string consulta = "SELECT estado FROM estadosreservas WHERE idestado = '" + state + "';";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();
                if (rea.Read())
                    id = rea["estado"].ToString();
                conn.Close();
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
                conn.Close();
            }
            return id;
        }

        //private void GetHistorico()
        //{
        //    string consulta = string.Format("select * from cv_servicioactividades where idexpediente = {0}", expediente.Idexpediente);
        //    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);

        //    try
        //    {
        //        conn.Open();
        //        SqlCommand comm = new SqlCommand(consulta, conn);
        //        SqlDataReader rea = comm.ExecuteReader();

        //        DataTable dt = new DataTable();                
        //        dt.Columns.Add(new DataColumn("idservicioactividad", System.Type.GetType("System.Int32")));                
        //        dt.Columns.Add(new DataColumn("pvp", System.Type.GetType("System.Decimal")));
        //        dt.Columns.Add(new DataColumn("descripcion", System.Type.GetType("System.String")));
        //        dt.Columns.Add(new DataColumn("tipo", System.Type.GetType("System.String")));
        //        dt.Columns.Add(new DataColumn("fechainicio", System.Type.GetType("System.String")));
        //        dt.Columns.Add(new DataColumn("fechafin", System.Type.GetType("System.String")));
        //        dt.Columns.Add(new DataColumn("observaciones", System.Type.GetType("System.String")));

        //        // Read rows from DataReader and populate the DataTable

        //        if (rea.Read())
        //        {
        //            for (int i = 1; i <= 3; ++i)
        //            {
        //                DataRow dr = dt.NewRow();                       
        //                dr["idservicioactividad"] = rea.GetInt32("idservicioactividad");                         
        //                dr["PVP"] = (IsDbNull(rea, "pvp" + i.ToString()) ? 0 : rea.GetDouble("pvp" + i.ToString()));
        //                dr["Descripcion"] = (IsDbNull(rea, "descripcion" + i.ToString()) ? "" : rea.GetString("descripcion" + i.ToString()));
        //                dr["Tipo"] = (IsDbNull(rea, "tipo" + i.ToString()) ? "" : rea.GetString("tipo" + i.ToString()));
        //                dr["FechaInicio"] = (IsDbNull(rea, "fechainicio" + i.ToString()) ? "" : rea.GetString("fechainicio" + i.ToString()));
        //                dr["Observaciones"] = (IsDbNull(rea, "alternativa" + i.ToString()) ? "" : rea.GetString("alternativa" + i.ToString()));
        //                dt.Rows.Add(dr);

        //            }
        //        }
        //        grdServiciosColectivos.DataSource = dt;
        //        grdServiciosColectivos.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //    }

        //    conn.Close();

        //}

        protected void odsOtrosServicios_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (expediente != null)
            {
                e.InputParameters["idExpediente"] = expediente.Idexpediente;
            }
        }

        protected void lvServicioActividades_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            //DVServicioActividades dvAct = (DVServicioActividades)e.Item.DataItem; 
        }

        #region Inserts
        private void InsertReserva(SqlConnection conn, SqlTransaction trans)
        {
            DDatosPersonalesUsuario datos = (DDatosPersonalesUsuario)HttpContext.Current.Session["DatosUsuario"];
            string consulta = "INSERT INTO reservasviajes (idreserva, reserva, fechapeticion, idestado, idpeticionario, observaciones, fkidexpediente, LastUpd) VALUES " +
                              "(" + GetNextIdReserva() + ", '', '" + DateTime.Now.ToString("yyyy/MM/dd") + "', 'AB'," + datos.IdPeticionario + ", '" + txtObservaciones.Text.Replace("'", "") + "', " + int.Parse(Request.QueryString["idexp"]) + ", '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "')";

            SqlCommand comm = new SqlCommand(consulta, conn, trans);
            comm.ExecuteNonQuery();
        }

        private void InsertServiciosReservasViajes(int idactividadservicio, int idreserva, SqlConnection conn, SqlTransaction trans)
        {

            string consulta = "INSERT INTO serviciosreservasviajes (idservicio, idreserva, idtipobono, fechapeticion, resumenservicio, idservicioactividad, idservicioinscripcion, idserviciohotel, idserviciotransporte) VALUES " +
                              "(" + GetNextIdServicioReserva() + ", " + idreserva + ", 'ACT', '" + DateTime.Now.ToString("yyyy/MM/dd") + "', 'resumen'," + idactividadservicio + ", NULL, NULL, NULL)";
            SqlCommand comm = new SqlCommand(consulta, conn, trans);
            comm.ExecuteNonQuery();

        }

        #endregion

        #region Auxiliares
        private bool ComprobarCamposOK()
        {
            bool ok = true;

            if (descripcionTextBox.Text == "")
            {
                ShowMessage("La descripción es un campo requerido", null);
                descripcionTextBox.Focus();
                return false;
            }
            //Ismael Ameller 24/02/2011 Validación para que el usuario indique una opción en tipo de servicio colectivo
            if (this.cboTipo.SelectedItem.Text == "Seleccione una opción")
            {
                ShowMessage("No ha seleccionado ningún tipo de servicio colectivo", null);
                this.cboTipo.Focus();
                return false;
            }
            //FIN Ismael Ameller 24/02/2011 Validación para que el usuario indique una opción en tipo de servicio colectivo
            if (importeTextBox.Text != "")
            {
                if (IsNumeric(importeTextBox.Text))
                {
                    ok = true;
                }
                else
                {
                    ShowMessage("El importe máximo debe ser un valor numérico.", null);
                    importeTextBox.Focus();
                    return false;
                }
            }
            if (personasTextBox.Text != "")
            {
                if (IsNumeric(personasTextBox.Text))
                {
                    ok = true;
                }
                else
                {
                    ShowMessage("El -No. Personas debe ser un valor numérico.", null);
                    personasTextBox.Focus();
                    return false;
                }
            }
            if (fechaInicioInputDatePickerControl.Value != "" && fechaFinalInputDatePickerControl.Value != "")
            {
                if (DateTime.Parse(this.fechaInicioInputDatePickerControl.Value) > DateTime.Parse(this.fechaFinalInputDatePickerControl.Value))
                {
                    ShowMessage("La fecha de fin no puede ser anterior a la  fecha de inicio.", null);
                    fechaFinalInputDatePickerControl.Focus();
                    return false;
                }
            }

            if (horaInicioTimeInputBox.Value != "" && horaFinalTimeInputBox.Value != "")
            {
                if (DateTime.Parse(this.horaInicioTimeInputBox.Value) > DateTime.Parse(this.horaFinalTimeInputBox.Value))
                {
                    ShowMessage("La hora de fin no puede ser anterior a la hora de inicio.", null);
                    horaFinalTimeInputBox.Focus();
                    return false;
                }
            }
            return ok;
        }

        private int GetNextIdServicio()
        {
            int id = 0;
            const string consulta = "SELECT MAX(idservicioactividad) FROM serviciosreservasactividades";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);

            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();
                if (rea.Read())
                {
                    id = rea.GetInt32(0);
                }
                conn.Close();
                return id + 1;
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
                conn.Close();
                return 0;
            }
        }

        private int GetNextIdReserva()
        {
            int id = 0;
            const string consulta = "SELECT MAX(idreserva) FROM reservasviajes";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);

            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();
                if (rea.Read())
                {
                    id = rea.GetInt32(0);
                }
                conn.Close();
                return id + 1;
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
                conn.Close();
                return 0;
            }
        }

        private string GetHour(string time)
        {
            return time.Substring(0, 2);
        }

        private string GetMinutes(string time)
        {
            return time.Substring(3, 2);
        }

        private bool IsNumeric(string value)
        {
            double d;
            return Double.TryParse(value, out d);
        }

        #endregion

        private string GetServicioByReserva()
        {
            // Recupera el id de servicio a partir del id de reserva
            string id;
            string consulta = "SELECT * FROM serviciosreservasviajes WHERE idreserva = " + CurrentIdReserva;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();
                if (rea.Read())
                {
                    id = Convert.ToString(int.Parse(rea["idservicio"].ToString()));
                    statename = GetStateName();
                }
                else
                    id = "";

                conn.Close();
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                id = "";
                Console.WriteLine(ex.ToString());
                conn.Close();
            }
            return id;
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            if (expediente.Idactividad.HasValue)
            {
                GetFechasReservas(Convert.ToString(expediente.Idactividad.Value));    
            }

            descripcionTextBox.Text = string.Empty;
            cboTipo.SelectedIndex = -1;
            otroTipoTextBox.Text = string.Empty;
            personasTextBox.Text = string.Empty;
            sedeTextBox.Text = string.Empty;
            importeTextBox.Text = string.Empty;
            setFechas();
            txtObservaciones.Text = string.Empty;
            state = "AB";
            statename = "Sin Enviar";
            HttpContext.Current.Session.Add("nuevoServicio", true);
        }

        protected void cboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTipo.SelectedValue.ToLower() == "otro")
            {
                otroTipoTextBox.Enabled = true;
            }
            else
            {
                otroTipoTextBox.Enabled = false;
                otroTipoTextBox.Text = string.Empty;
            }

        }
        //Ismael Ameller 08-03-2011 Controles enable segun si sale el boton guardar o no
        public void ControlesEnable(bool permitir)
        {
            descripcionTextBox.Enabled = permitir;
            cboTipo.Enabled = permitir;
            otroTipoTextBox.Enabled = permitir;
            personasTextBox.Enabled = permitir;
            sedeTextBox.Enabled = permitir;
            importeTextBox.Enabled = permitir;
            txtObservaciones.Enabled = permitir;
            fechaInicioInputDatePickerControl.Enabled = permitir;
            fechaFinalInputDatePickerControl.Enabled = permitir;
            horaInicioTimeInputBox.Enabled = permitir;
            horaFinalTimeInputBox.Enabled = permitir;
        }
        //FIN Ismael Ameller 08-03-2011 Controles enable segun si sale el boton guardar o no
    }
}