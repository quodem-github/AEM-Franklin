using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using EOS.Entidades.Datos;
using EOS.Web;

using System.Globalization;
using System.Drawing;

namespace EOS.Controls
{
    public partial class OtrosServiciosPanel : UserControl
    {
        protected String SelectedTarifaOtrosServicios
        {
            get
            {
                return (String)ViewState["SelectedTarifaOtrosServicios"];
            }
            set
            {
                ViewState["SelectedTarifaOtrosServicios"] = value;
            }
        }

        protected int? currentidreserva = null;
        protected int? CurrentIdReserva
        {
            get { return this.currentidreserva; }
            set { this.currentidreserva = value; }
        }

        protected int currentidservicio = 0;
        protected int CurrentIdServicio
        {
            get { return this.currentidservicio; }
            set { this.currentidservicio = value; }
        }

        protected int currentidServicioReserva = 0;
        protected int CurrentIdServicioReserva
        {
            get { return this.currentidServicioReserva; }
            set { this.currentidServicioReserva = value; }
        }

        private DCabeceraExpedienteAmpliado expediente
        {
            get //Qurius (EAS) 24/01/2011
            {
                if (HttpContext.Current.Session["currentExpediente"] != null)
                {
                    return (DCabeceraExpedienteAmpliado)HttpContext.Current.Session["currentExpediente"];
                }
                else
                {
                    AgenteExpedientes agenteExp = new AgenteExpedientes();
                    DCabeceraExpedienteAmpliado value = agenteExp.ObtenerExpedientePorID(Request.QueryString["idexp"]);
                    HttpContext.Current.Session["currentFKIdCongreso"] = value.Idactividad;

                    return value;
                }
            }
        }

        public bool existePregarga
        {
            get { return this.eoscTarifasOtrosServiciosListView.existePregarga; }
        }
        private bool showAlternative = false;
        private string state = string.Empty;
        private string statename = string.Empty;
        private string desde = string.Empty;
        private string hasta = string.Empty;
        private string desdehora = string.Empty;
        private string hastahora = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Qurius (EAS) 17/02/2011
            //TipoActividad tipoactividad = (TipoActividad)Session["tipoactividad"];
            if (Request.QueryString["tab"].ToString() == "3" && this.expediente.Idtiporeserva != 2)  //Qurius (EAS) 24/01/2011
            {
                showAlternative = (Request.QueryString["view"] != null && Request.QueryString["view"].ToString() == "alternativa");
                this.Test.IdExpediente = this.expediente.Idexpediente.ToString();

                if (Request.QueryString["idres"] != null)
                {
                    this.CurrentIdReserva = int.Parse(Request.QueryString["idres"].ToString());
                    GetServicioReserva();
                    //Ismael Ameller 25-02-2011 Descomento la línea para que me cargue las personas seleccionadas
                    this.Test.IdServicio = this.CurrentIdServicio.ToString();
                    //this.Test.Enabled = !showAlternative;
                    //FIN Ismael Ameller 25-02-2011 Descomento la línea para que me cargue las personas seleccionadas
                    this.Alternativas.Visible = showAlternative;

                    this.Alternativas.IdServicio = this.CurrentIdServicio.ToString();
                    if (!this.IsPostBack)
                    {
                        //this.Alternativas.IdServicio = !showAlternative ? this.CurrentIdReserva.ToString() : currentidservicio.ToString();
                        LoadReserva(CurrentIdReserva);
                    }
                }
                //Qurius (EAS) 17/02/2011
                //if (tipoactividad == TipoActividad.OtrosServicios) SetButtons();
                SetButtons();

                // Muestra el estado
                this.lblEstado.Text = statename;
                this.lblEstado.CssClass = string.Format("eosImagenEstado eosImagenLeyenda{0}", state);
            }
        }

        private void GetFechasReservas(string idCongreso)
        {
            string consulta = "SELECT idCongreso, Desde, Hasta FROM tiposcongreso tc";
            consulta += " RIGHT JOIN congresos c ON c.idtipocongreso = tc.idtipocongreso";
            consulta += " where idCongreso = " + idCongreso + ";";

            string test = string.Empty;
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
                    if (desdehora.IndexOf(":") == 1) desdehora = "0" + desdehora;

                    DateTime hastadt = Convert.ToDateTime(rea["Hasta"].ToString());
                    hasta = hastadt.ToShortDateString();
                    hastahora = hastadt.ToShortTimeString();
                    if (hastahora.IndexOf(":") == 1) hastahora = "0" + hastahora;
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

        private void SetState(string state)
        {
            // Actualizar el estado del servicio de alojamiento
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                string consulta = "UPDATE reservasviajes SET idestado = '" + state + "', LastUpd = getdate() WHERE idreserva = " + Request.QueryString["idres"] + ";";
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
            this.btnGuardar.Visible = false;
            this.btnEnviar.Visible = false;
            this.btnRechazar.Visible = false;
            this.btnAprobar.Visible = false;
            this.btnCancelar.Visible = false;
            //Ismael Ameller 25-02-2011 Mantiene bloqueada la fecha y hora cuando traspasamos una persona
            bool permite = this.tipoTextBox.Enabled;
            this.fechaFinalInputDatePickerControl.Enabled = permite;
            this.fechaInicioInputDatePickerControl.Enabled = permite;
            this.horaInicioTimeInputBox.Enabled = permite;
            this.horaFinalTimeInputBox.Enabled = permite;
            //FIN Ismael Ameller 25-02-2011 Mantiene bloqueada la fecha y hora cuando traspasamos una persona
            switch (state)
            {
                case "AB": //sin enviar
                    this.btnGuardar.Visible = true;
                    //si es nuevo (no guardado) no hay que mostrar el boton enviar
                    if (currentidservicio == 0 && (currentidreserva == 0 || currentidreserva == null))
                        this.btnEnviar.Visible = false;
                    else
                        this.btnEnviar.Visible = true;
                    this.btnRechazar.Visible = true;
                    this.btnAprobar.Visible = false;
                    this.btnCancelar.Visible = false;
                    break;
                case "ABI": //abierto
                    break;
                case "AC": //aceptada
                case "ACP": //aceptada
                case "CFP": //aceptado
                    this.btnRechazar.Visible = true;
                    this.btnGuardar.Visible = false;
                    this.btnEnviar.Visible = false;
                    this.btnAprobar.Visible = false;
                    this.btnCancelar.Visible = false;
                    break;
                case "AN": //rechazado
                case "CTZD": //cotizando
                case "PTR": //tramitando    
                    this.btnGuardar.Visible = false;
                    this.btnEnviar.Visible = false;
                    this.btnRechazar.Visible = false;
                    this.btnAprobar.Visible = false;
                    this.btnCancelar.Visible = false;
                    break;
                case "CER": //cerrado
                    break;
                case "CN": //cancelado
                case "CNTR": //cancelado con Gastos
                    break;
                case "CR": //enviado
                    this.btnGuardar.Visible = true;
                    this.btnEnviar.Visible = false;
                    this.btnRechazar.Visible = true;
                    this.btnAprobar.Visible = false;
                    this.btnCancelar.Visible = false;
                    break;
                case "FZ": //finalizado
                    break;
                case "MDF": //MDF
                    break;
                case "NC": // en curso
                    break;
                case "CTZ": //cotizado
                    this.btnGuardar.Visible = false;
                    this.btnEnviar.Visible = false;
                    this.btnRechazar.Visible = true;
                    this.btnAprobar.Visible = true;
                    this.btnCancelar.Visible = false;
                    break;
                case "TR": //tramitado
                    this.btnGuardar.Visible = false;
                    this.btnEnviar.Visible = false;
                    this.btnRechazar.Visible = false;
                    this.btnAprobar.Visible = false;
                    this.btnCancelar.Visible = true;
                    break;
                case "":
                    if (currentidreserva == 0 || currentidreserva == null)
                    {
                        this.btnGuardar.Visible = true;
                    }
                    break;
            }
        }

        private void GetServicioReserva()
        {
            // Recupera el id de servicio a partir del id de reserva
            string consulta = "SELECT * FROM serviciosreservasactividades SH LEFT JOIN serviciosreservasviajes SR ON SH.idservicioactividad = SR.idservicioactividad LEFT JOIN reservasviajes RV ON SR.idreserva = RV.idreserva WHERE SR.idreserva = " + this.CurrentIdReserva;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();
                if (rea.Read())
                {
                    this.CurrentIdServicio = int.Parse(rea["idservicioactividad"].ToString());
                    //Jose Laguna 06-04-2010 Esto lo añadimos para que no pierda el estado cuando modificas la alternativa
                    this.state = rea["idEstado"] != DBNull.Value ? rea["idEstado"].ToString() : "";
                    this.statename = GetStateName();
                }
                else
                    this.CurrentIdServicio = 0;

                conn.Close();
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                this.CurrentIdServicio = 0;
                Console.WriteLine(ex.ToString());
                conn.Close();
            }
        }

        private string GetServicioByReserva()
        {
            // Recupera el id de servicio a partir del id de reserva
            string id = "";
            string consulta = "SELECT * FROM serviciosreservasviajes WHERE idreserva = " + CurrentIdReserva;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();
                if (rea.Read())
                {
                    id = rea["idservicio"].ToString();
                    this.statename = GetStateName();
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

        private void LoadReserva(int? CurrentIdReserva)
        {
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
                              "reservasviajes.idestado " +
                              "FROM serviciosreservasactividades " +
                              "LEFT JOIN serviciosreservasviajes ON serviciosreservasactividades.idservicioactividad = serviciosreservasviajes.idservicioactividad " +
                              "LEFT JOIN reservasviajes ON reservasviajes.idreserva = serviciosreservasviajes.idreserva " +
                              "WHERE reservasviajes.idreserva = " + CurrentIdReserva;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);

            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();
                if (rea.Read())
                {
                    this.CurrentIdServicio = rea[0] != DBNull.Value ? rea.GetInt32(0) : 0;
                    this.SelectedTarifaOtrosServicios = rea[1] != DBNull.Value ? rea.GetInt32(1).ToString() : "0";
                    state = rea["idestado"].ToString();
                    statename = GetStateName();
                    if (SelectedTarifaOtrosServicios != "0")
                    {
                        this.Test.Observaciones = rea[2] != DBNull.Value ? rea.GetString(2) : string.Empty;
                        descripcionTextBox.Text = rea[6] != DBNull.Value ? rea.GetString(6) : string.Empty;
                        tipoTextBox.Text = rea[7] != DBNull.Value ? rea.GetString(7) : string.Empty;
                        sedeTextBox.Text = rea[5] != DBNull.Value ? rea.GetString(5) : string.Empty;
                        importeTextBox.Text = rea[3] != DBNull.Value ? rea.GetDouble(3).ToString() : string.Empty;
                        //Ismael Ameller 25-02-2011 Carga controles de fechas y horas cuando entramos por edición
                        desde = rea[8] != DBNull.Value ? rea.GetDateTime(8).ToShortDateString() : string.Empty;
                        fechaInicioInputDatePickerControl.Value = desde;
                        fechaInicioInputDatePickerControl.InputTextBox.Text = desde;
                        string horaIni = rea[9] != DBNull.Value ? rea.GetString(9) : string.Empty;
                        string minutosIni = rea[10] != DBNull.Value ? rea.GetString(10) : string.Empty;
                        desdehora = horaIni + ":" + minutosIni;
                        horaInicioTimeInputBox.Value = desdehora;
                        horaInicioTimeInputBox.InputTextBox.Text = desdehora;
                        hasta = rea[11] != DBNull.Value ? rea.GetDateTime(11).ToShortDateString() : string.Empty;
                        fechaFinalInputDatePickerControl.Value = hasta;
                        fechaFinalInputDatePickerControl.InputTextBox.Text = hasta;
                        string horaFin = rea[12] != DBNull.Value ? rea.GetString(12) : string.Empty;
                        string minutosFin = rea[13] != DBNull.Value ? rea.GetString(13) : string.Empty;
                        hastahora = horaFin + ":" + minutosFin;
                        horaFinalTimeInputBox.Value = hastahora;
                        horaFinalTimeInputBox.InputTextBox.Text = hastahora;
                        //FIN Ismael Ameller 25-02-2011 Carga controles de fechas y horas cuando entramos por edición
                    }
                    else
                    {
                        this.Test.Observaciones = rea[2] != DBNull.Value ? rea.GetString(2) : string.Empty;
                        descripcionTextBox.Text = rea[6] != DBNull.Value ? rea.GetString(6) : string.Empty;
                        descripcionTextBox.Enabled = !showAlternative;
                        //descripcionTextBox.BackColor = showAlternative? Color.Transparent: Color.Silver;
                        tipoTextBox.Text = rea[7] != DBNull.Value ? rea.GetString(7) : string.Empty;
                        tipoTextBox.Enabled = !showAlternative;

                        sedeTextBox.Text = rea[5] != DBNull.Value ? rea.GetString(5) : string.Empty;

                        sedeTextBox.Enabled = !showAlternative;
                        importeTextBox.Text = rea[3] != DBNull.Value ? rea.GetDouble(3).ToString() : string.Empty;
                        importeTextBox.Enabled = !showAlternative;
                        desde = rea[8] != DBNull.Value ? rea.GetDateTime(8).ToShortDateString() : string.Empty;

                        fechaInicioInputDatePickerControl.Value = desde;
                        fechaInicioInputDatePickerControl.InputTextBox.Text = desde;

                        fechaInicioInputDatePickerControl.Enabled = !showAlternative;
                        fechaInicioInputDatePickerControl.ClickImageButton.Enabled = !showAlternative;
                        fechaInicioInputDatePickerControl.InputTextBox.Enabled = !showAlternative;

                        string horaIni = rea[9] != DBNull.Value ? rea.GetString(9) : string.Empty;
                        string minutosIni = rea[10] != DBNull.Value ? rea.GetString(10) : string.Empty;
                        desdehora = horaIni + ":" + minutosIni;

                        horaInicioTimeInputBox.Value = desdehora;
                        horaInicioTimeInputBox.Enabled = !showAlternative;
                        horaInicioTimeInputBox.InputTextBox.Enabled = !showAlternative;
                        horaInicioTimeInputBox.InputTextBox.Text = desdehora;

                        hasta = rea[11] != DBNull.Value ? rea.GetDateTime(11).ToShortDateString() : string.Empty;
                        fechaFinalInputDatePickerControl.Value = hasta;
                        fechaFinalInputDatePickerControl.InputTextBox.Text = hasta;

                        fechaFinalInputDatePickerControl.Enabled = !showAlternative;
                        fechaFinalInputDatePickerControl.ClickImageButton.Enabled = !showAlternative;
                        fechaFinalInputDatePickerControl.InputTextBox.Enabled = !showAlternative;

                        string horaFin = rea[12] != DBNull.Value ? rea.GetString(12) : string.Empty;
                        string minutosFin = rea[13] != DBNull.Value ? rea.GetString(13) : string.Empty;
                        hastahora = horaFin + ":" + minutosFin;

                        horaFinalTimeInputBox.Value = hastahora;
                        horaFinalTimeInputBox.Enabled = !showAlternative;
                        horaFinalTimeInputBox.InputTextBox.Enabled = !showAlternative;
                        horaFinalTimeInputBox.InputTextBox.Text = hastahora;
                    }
                }
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(ex.ToString());
            } conn.Close();
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            eoscTarifasOtrosServiciosListView.OnTarifaSelected += new EventHandler(eoscTarifasOtrosServiciosListView_OnTarifaSelected);
        }

        void eoscTarifasOtrosServiciosListView_OnTarifaSelected(object sender, EventArgs e)
        {
            TarifaSelectedEventArgs args = (TarifaSelectedEventArgs)e;
            this.SelectedTarifaOtrosServicios = args.selectedItem;

            if (this.SelectedTarifaOtrosServicios == "0")
            {
                GetFechasReservas(this.expediente.Idactividad.Value.ToString());

                descripcionTextBox.Enabled = true;
                descripcionTextBox.Text = string.Empty;
                tipoTextBox.Enabled = true;
                tipoTextBox.Text = string.Empty;
                sedeTextBox.Enabled = true;
                sedeTextBox.Text = string.Empty;
                importeTextBox.Enabled = true;
                importeTextBox.Text = string.Empty;
                fechaInicioInputDatePickerControl.Enabled = true;
                fechaInicioInputDatePickerControl.ClickImageButton.Enabled = true;
                fechaInicioInputDatePickerControl.InputTextBox.Enabled = true;
                //Ismael Ameller 25-02-2011 Error en la fecha
                fechaInicioInputDatePickerControl.InputTextBox.Text = desde;
                //FIN Ismael Ameller 25-02-2011 Error en la fecha
                //fechaInicioInputDatePickerControl.Value = string.Empty;
                horaInicioTimeInputBox.Enabled = true;
                horaInicioTimeInputBox.InputTextBox.Enabled = true;
                //horaInicioTimeInputBox.Value = string.Empty;
                //Ismael Ameller 25-02-2011 Error en la hora
                horaInicioTimeInputBox.InputTextBox.Text = desdehora;
                //FIN Ismael Ameller 25-02-2011 Error en la hora
                fechaFinalInputDatePickerControl.Enabled = true;
                fechaFinalInputDatePickerControl.ClickImageButton.Enabled = true;
                fechaFinalInputDatePickerControl.InputTextBox.Enabled = true;
                //fechaFinalInputDatePickerControl.Value = string.Empty;
                //Ismael Ameller 25-02-2011 Error en la fecha
                fechaFinalInputDatePickerControl.InputTextBox.Text = hasta;
                //FIN Ismael Ameller 25-02-2011 Error en la fecha
                horaFinalTimeInputBox.Enabled = true;
                horaFinalTimeInputBox.InputTextBox.Enabled = true;
                //horaFinalTimeInputBox.Value = string.Empty;
                //Ismael Ameller 25-02-2011 Error en la hora
                horaFinalTimeInputBox.InputTextBox.Text = hastahora;
                //FIN Ismael Ameller 25-02-2011 Error en la hora
            }
            else
            {
                descripcionTextBox.Enabled = false;
                tipoTextBox.Enabled = false;
                sedeTextBox.Enabled = false;
                importeTextBox.Enabled = false;
                fechaInicioInputDatePickerControl.Enabled = false;
                fechaInicioInputDatePickerControl.ClickImageButton.Enabled = false;
                fechaInicioInputDatePickerControl.InputTextBox.Enabled = false;
                horaInicioTimeInputBox.Enabled = false;
                horaInicioTimeInputBox.InputTextBox.Enabled = false;
                fechaFinalInputDatePickerControl.Enabled = false;
                fechaFinalInputDatePickerControl.ClickImageButton.Enabled = false;
                fechaFinalInputDatePickerControl.InputTextBox.Enabled = false;
                horaFinalTimeInputBox.Enabled = false;
                horaFinalTimeInputBox.InputTextBox.Enabled = false;
                //25-02-2011 Ismael Ameller Carga de las fechas y las horas recuperadas de la BBDD
                string currentidcongreso = HttpContext.Current.Session["currentFKIdCongreso"].ToString();
                string consulta = "SELECT tarifasactividad.idtarifaactividad, tarifasactividad.actividad, tipos_actividad_congreso.tipoactividadcongreso, " +
                "proveedores.proveedor, tarifasactividad.pvp, tarifasactividad.fechainicio, tarifasactividad.fechafin " +
                "FROM tarifasactividad " +
                "LEFT JOIN proveedores ON tarifasactividad.idproveedor = proveedores.idproveedor " +
                "LEFT JOIN tipos_actividad_congreso ON tarifasactividad.idtipoactividadcongreso = tipos_actividad_congreso.idtipoactividadcongreso " +
                "WHERE fkidcongreso = " + currentidcongreso + " AND tarifasactividad.idtarifaactividad = " + this.SelectedTarifaOtrosServicios;

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);

                try
                {
                    conn.Open();

                    string consultaIdConfEmpresa = string.Format("select idconfempresa from amec where idamec = (select top 1 idamec from expediente where idxpediente = {0})", expediente.Idexpediente);
                    SqlCommand commandIdConfEmpresa = new SqlCommand(consultaIdConfEmpresa, conn);
                    string idConfEmpresa = commandIdConfEmpresa.ExecuteScalar().ToString();

                    consulta += " AND proveedores.IdConfEmpresa = " + idConfEmpresa;

                    SqlCommand comm = new SqlCommand(consulta, conn);
                    SqlDataReader rea = comm.ExecuteReader();
                    if (rea.Read())
                    {
                        descripcionTextBox.Text = rea[1] != DBNull.Value ? rea.GetString(1) : "";
                        tipoTextBox.Text = rea[2] != DBNull.Value ? rea.GetString(2) : "";
                        sedeTextBox.Text = rea[3] != DBNull.Value ? rea.GetString(3) : "";
                        importeTextBox.Text = rea[4] != DBNull.Value ? rea.GetInt32(4).ToString() : "";
                        fechaInicioInputDatePickerControl.Value = rea[5] != DBNull.Value ? rea.GetDateTime(5).ToString() : "";
                        fechaFinalInputDatePickerControl.Value = rea[6] != DBNull.Value ? rea.GetDateTime(6).ToString() : "";
                        if (fechaInicioInputDatePickerControl.Value != "")
                        {
                            if (rea.GetDateTime(5).ToString().Contains("0:00:00"))
                            {
                                horaInicioTimeInputBox.Value = "00:00";
                            }
                            else
                            {
                                horaInicioTimeInputBox.Value = rea[5] != DBNull.Value ? rea.GetDateTime(5).ToString().Substring(11, 5) : "";
                            }
                        }
                        else
                        {
                            horaInicioTimeInputBox.Value = "";
                        }
                        if (fechaFinalInputDatePickerControl.Value != "")
                        {
                            if (rea.GetDateTime(6).ToString().Contains("0:00:00"))
                            {
                                horaFinalTimeInputBox.Value = "00:00";
                            }
                            else
                            {
                                horaFinalTimeInputBox.Value = rea[6] != DBNull.Value ? rea.GetDateTime(6).ToString().Substring(11, 5) : "";
                            }
                        }
                        else
                        {
                            horaFinalTimeInputBox.Value = "";
                        }
                        //FIN 25-02-2011 Ismael Ameller Carga de las fechas y las horas recuperadas de la BBDD
                    }
                }
                catch (Exception ex)
                {
                    //Ismael Ameller 09-03-2011 Envio de Mail
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                    //FIN Ismael Ameller 09-03-2011 Envio de Mail
                    Console.WriteLine(ex.ToString());
                } conn.Close();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Guardar();
        }
        public Boolean Guardar()
        {
            int nextid = GetNextIdServicio();

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            conn.Open();

            SqlTransaction trans = null;

            if (!ComprobarCamposOK()) return false;


            string fechaIni = this.fechaInicioInputDatePickerControl.Value == "" ? "NULL" : this.fechaInicioInputDatePickerControl.Value;
            if (fechaIni != "NULL")
            {
                DateTime inidt = Convert.ToDateTime(fechaIni);
                fechaIni = String.Format("{0:yyyyMMdd}", inidt);
            }
            string horaIni = this.horaInicioTimeInputBox.Value == "" ? "NULL" : GetHour(this.horaInicioTimeInputBox.Value);
            string minutosIni = this.horaInicioTimeInputBox.Value == "" ? "NULL" : GetMinutes(this.horaInicioTimeInputBox.Value);
            string fechaFin = this.fechaFinalInputDatePickerControl.Value == "" ? "NULL" : this.fechaFinalInputDatePickerControl.Value;
            if (fechaFin != "NULL")
            {
                DateTime findt = Convert.ToDateTime(fechaFin);
                fechaFin = String.Format("{0:yyyyMMdd}", findt);
            }
            string horaFin = this.horaFinalTimeInputBox.Value == "" ? "NULL" : GetHour(this.horaFinalTimeInputBox.Value);
            string minutosFin = this.horaFinalTimeInputBox.Value == "" ? "NULL" : GetMinutes(this.horaFinalTimeInputBox.Value);
            double val;
            double dImporte = Double.TryParse(importeTextBox.Text, NumberStyles.Number, CultureInfo.GetCultureInfo("es-ES").NumberFormat, out val) ? val : 0; 
            string sImporte = dImporte.ToString().Replace(",", "@");

            string consulta = string.Empty;
            
            if (this.CurrentIdReserva == null)
            {
                //INSERT
                if (this.SelectedTarifaOtrosServicios == "0")
                {
                    /*
                    Qurius (EAS) 24/01/2011
                    consulta = "INSERT INTO serviciosreservasactividades (idservicioactividad, idtarifaactividad, observaciones, pvp, locked, sede, descripcion, tipo, fechainicio, horainicio, minutosinicio, fechafin, horafin, minutosfin) VALUES " +
                                       "(" + nextid + ",  NULL , '" + this.Test.Observaciones + "', " + int.Parse(this.importeTextBox.Text) + ", 0, '" + this.sedeTextBox.Text + "', '" + this.descripcionTextBox.Text + "', '" + this.tipoTextBox.Text + "', '" +
                                        "" + fechaIni + "', '" +
                                        "" + horaIni + "', '" +
                                        "" + minutosIni + "', '" +
                                        "" + fechaFin + "', '" +
                                        "" + horaFin + "','" +
                                        "" + minutosFin + "')";
                     */

                    string consultaIdConfEmpresa = string.Format("select idconfempresa from amec where idamec = (select top 1 idamec from expediente where idxpediente = {0})", expediente.Idexpediente);
                    SqlCommand commandIdConfEmpresa = new SqlCommand(consultaIdConfEmpresa, conn);
                    string idConfEmpresa = commandIdConfEmpresa.ExecuteScalar().ToString();

                    consulta = string.Format("INSERT INTO serviciosreservasactividades VALUES ({0},{1},'{2}',{3},{4},'{5}','{6}','{7}',{8},{9},{10},{11},{12},{13}, {14}, {15})", +
                    nextid,
                    "null",
                    this.Test.Observaciones.Replace("'", ""),
                    sImporte.Replace("@", "."),
                    0,
                    this.sedeTextBox.Text.Replace("'", ""),
                    this.descripcionTextBox.Text.Replace("'", ""),
                    this.tipoTextBox.Text.Replace("'", ""),
                    fechaIni == "NULL" ? "null" : "'" + fechaIni + "'",
                    horaIni == "NULL" ? "null" : "'" + horaIni + "'",
                    minutosIni == "NULL" ? "null" : "'" + minutosIni + "'",
                    fechaFin == "NULL" ? "null" : "'" + fechaFin + "'",
                    horaFin == "NULL" ? "null" : "'" + horaFin + "'",
                    minutosFin == "NULL" ? "null" : "'" + minutosFin + "'", 0, idConfEmpresa);
                }
                else
                {
                    //Ismael Ameller 25-02-2011 Guardamos las fechas y horas en BBDD
                    if (fechaIni == "NULL")
                    {
                        fechaIni = "null";
                    }
                    else
                    {
                        fechaIni = "'" + fechaIni + "'";
                    }

                    if (fechaFin == "NULL")
                    {
                        fechaFin = "null";
                    }
                    else
                    {
                        fechaFin = "'" + fechaFin + "'";
                    }

                    string consultaIdConfEmpresa = string.Format("select idconfempresa from amec where idamec = (select top 1 idamec from expediente where idxpediente = {0})", expediente.Idexpediente);
                    SqlCommand commandIdConfEmpresa = new SqlCommand(consultaIdConfEmpresa, conn);
                    string idConfEmpresa = commandIdConfEmpresa.ExecuteScalar().ToString();

                    consulta = "INSERT INTO serviciosreservasactividades (idservicioactividad, idtarifaactividad, observaciones, pvp, locked, sede, descripcion, tipo, fechainicio, horainicio, minutosinicio, fechafin, horafin, minutosfin, idconfempresa) VALUES " +
                                       "(" + nextid + ", " + int.Parse(this.SelectedTarifaOtrosServicios) + ", '" + this.Test.Observaciones.Replace("'", "") + "', " + sImporte.Replace("@", ".") + ", 0, '" + this.sedeTextBox.Text.Replace("'", "") + "', '" + this.descripcionTextBox.Text.Replace("'", "") + "', '"
                                       + this.tipoTextBox.Text.Replace("'", "") + "' ," + fechaIni + "," + horaIni + "," + minutosIni + ","
                                       + fechaFin + "," + horaFin + "," + minutosFin + "," + idConfEmpresa +
                                       ")";
                    //FIN Ismael Ameller 25-02-2011 Guardamos las fechas y horas en BBDD
                }
                try
                {
                    trans = conn.BeginTransaction(System.Data.IsolationLevel.Serializable);
                    SqlCommand comm = new SqlCommand(consulta, conn, trans);
                    comm.ExecuteNonQuery();
                    int nextreserva = GetNextIdReserva();
                    this.CurrentIdReserva = nextreserva;
                    InsertReserva(conn, trans);
                    InsertServiciosReservasViajes(nextid, nextreserva, conn, trans);
                    trans.Commit();

                    this.CurrentIdReserva = nextreserva;
                    this.Test.IdServicio = nextid.ToString();
                    this.Test.Save();
                    //InsertPassengerList();

                    ShowMessage("El servicio se ha creado correctamente.", "DetalleExpediente.aspx?idexp=" + this.expediente.Idexpediente.ToString());

                    //añadimos esta llamada al procedure para actualizar los importes de las reservas, expediente y AMEC
                    consulta = " exec dbo.actualizar_importes_totales " + GetServicioByReserva() + ";";
                    comm = new SqlCommand(consulta, conn);
                    comm.ExecuteNonQuery();

                    //this.btnEnviar.Visible = true;
                    //this.Response.Redirect("DetalleExpediente.aspx?idexp=" + this.expediente.Idexpediente, false);
                }
                catch (Exception ex)
                {
                    try
                    {
                        trans.Rollback();
                    }
                    catch { }
                    //Ismael Ameller 09-03-2011 Envio de Mail
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                    //FIN Ismael Ameller 09-03-2011 Envio de Mail
                    Console.WriteLine(ex.ToString());
                    ShowMessage("Se ha producido un error al actualizar el servicio.", "");
                    trans.Rollback();
                    return false;
                }
                conn.Close();
            }
            //UPDATE
            else
            {
                if (this.SelectedTarifaOtrosServicios == "0")
                {
                    consulta = "UPDATE serviciosreservasactividades "+
                               "SET idtarifaactividad = NULL, " +
                               "observaciones = '" + this.Test.Observaciones.Replace("'", "") + "'," +
                               "pvp = " + sImporte.Replace("@", ".") + "," +
                               "serviciosreservasactividades.locked = 0, " +
                               "sede = '" + this.sedeTextBox.Text.Replace("'", "") + "', " +
                               "descripcion = '" + descripcionTextBox.Text.Replace("'", "") + "', " +
                               "tipo = '" + this.tipoTextBox.Text.Replace("'", "") + "',";
                    consulta += fechaIni == "NULL" ? "fechainicio = NULL, " : "fechainicio = '" + fechaIni + "', ";
                    consulta += horaIni == "NULL" ? "horainicio = NULL, " : "horainicio = '" + horaIni + "', ";
                    consulta += minutosIni == "NULL" ? "minutosinicio = NULL, " : "minutosinicio = '" + minutosIni + "', ";
                    consulta += fechaFin == "NULL" ? "fechafin = NULL, " : "fechafin = '" + fechaFin + "', ";
                    consulta += horaFin == "NULL" ? "horafin = NULL, " : "horafin = '" + horaFin + "', ";
                    consulta += minutosFin == "NULL" ? "minutosfin = NULL, " : "minutosfin = '" + minutosFin + "' " +
                        " from serviciosreservasactividades LEFT JOIN serviciosreservasviajes ON serviciosreservasviajes.idservicioactividad = serviciosreservasactividades.idservicioactividad " +
                    "   WHERE serviciosreservasviajes.idreserva = " + CurrentIdReserva;
                }
                else
                {
                    if (this.SelectedTarifaOtrosServicios != "0")
                    {
                        consulta = "UPDATE serviciosreservasactividades " +
                                   "SET idtarifaactividad = " + int.Parse(this.SelectedTarifaOtrosServicios) + ", " +
                                   "observaciones = '" + this.Test.Observaciones.Replace("'", "") + "', " +
                                   "pvp = " + sImporte.Replace("@", ".") + ", " +
                                   "serviciosreservasactividades.locked = 0, " +
                                   "sede = '" + this.sedeTextBox.Text.Replace("'", "") + "', " +
                                   "descripcion = '" + this.descripcionTextBox.Text.Replace("'", "") + "', " +
                                   "tipo = '" + this.tipoTextBox.Text.Replace("'", "") + "' " +
                                   " from serviciosreservasactividades LEFT JOIN serviciosreservasviajes ON serviciosreservasviajes.idservicioactividad = serviciosreservasactividades.idservicioactividad " +
                                   "WHERE serviciosreservasviajes.idreserva = " + CurrentIdReserva;
                    }
                    else
                    {
                        consulta = "UPDATE serviciosreservasactividades " +
                                   "SET observaciones = '" + this.Test.Observaciones.Replace("'", "") + "', " +
                                   "pvp = " + sImporte.Replace("@", ".") + ", " +
                                   "serviciosreservasactividades.locked = 0, " +
                                   "sede = '" + this.sedeTextBox.Text.Replace("'", "") + "', " +
                                   "descripcion = '" + this.descripcionTextBox.Text.Replace("'", "") + "', " +
                                   "tipo = '" + this.tipoTextBox.Text.Replace("'", "") + "' " +
                                   " from serviciosreservasactividades LEFT JOIN serviciosreservasviajes ON serviciosreservasviajes.idservicioactividad = serviciosreservasactividades.idservicioactividad " + 
                                   "WHERE serviciosreservasviajes.idreserva = " + CurrentIdReserva;
                    }
                }

                try
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Open();
                    SqlCommand comm = new SqlCommand(consulta, conn);
                    comm.ExecuteNonQuery();
                    if (state == "CR")
                    {
                        SetState("AB");
                        state = "AB";
                    }
                    //this.Test.IdServicio = CurrentIdReserva.ToString();
                    this.Test.Save();
                    //InsertPassengerList();

                    ShowMessage("El servicio se ha actualizado correctamente.", "DetalleExpediente.aspx?idexp=" + this.expediente.Idexpediente.ToString());

                    //añadimos esta llamada al procedure para actualizar los importes de las reservas, expediente y AMEC
                    consulta = " exec dbo.actualizar_importes_totales " + GetServicioByReserva() + ";";
                    comm = new SqlCommand(consulta, conn);
                    comm.ExecuteNonQuery();

                    this.Response.Redirect("DetalleExpediente.aspx?idexp=" + this.expediente.Idexpediente, false);
                }
                catch (Exception ex)
                {
                    //Ismael Ameller 09-03-2011 Envio de Mail
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                    //FIN Ismael Ameller 09-03-2011 Envio de Mail
                    Console.WriteLine(ex.ToString());
                    ShowMessage("Se ha producido un error al actualizar el servicio.", "");
                    return false;
                }
                conn.Close();
            }

            return true;

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

                    if (!Guardar()) return;

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
                    string consulta = " exec dbo.sp_fee_generar_reserva " + expediente.Idexpediente + ", " + datosRolesFee.IdPeticionario + ", " + idconfEmpresa + ";";
                    SqlCommand commFees = new SqlCommand(consulta, connFees);
                    commFees = new SqlCommand(consulta, connFees);
                    commFees.ExecuteNonQuery();
                    connFees.Close();
                    //Fin Generación de reserva asociada a los fees

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
                                  "Id de servicio: " + this.CurrentIdServicio.ToString() + "\n" +
                                  "Descripción: " + this.descripcionTextBox.Text + "\n" +
                                  "Tipo: " + this.tipoTextBox.Text + "\n" +
                                  "Sede: " + this.sedeTextBox.Text + "\n" +
                                  "Importe máximo: " + this.importeTextBox.Text + "\n" +
                                  "Fecha inicio: " + this.fechaInicioInputDatePickerControl.Value + "\n" +
                                  "Hora inicio: " + this.horaInicioTimeInputBox.Value + "\n" +
                                  "Fecha final: " + this.fechaFinalInputDatePickerControl.Value + "\n" +
                                  "Hora final: " + this.horaFinalTimeInputBox.Value + "\n";

                    string pasajeros = "";
                    if (Test.NumPassengers > 0)
                    {
                        pasajeros = Test.ListaPasajeros;
                        body = body + "\n" + "Pasajeros: " + pasajeros + "\n";
                    }
                    else
                    {
                        body = body + "\n" + "Pasajeros: No hay ninguno seleccionado \n";
                    }
                    if (!string.IsNullOrEmpty(this.Test.Observaciones))
                    {
                        body = body + "Observaciones pasajeros: " + this.Test.Observaciones;
                    }
                    AgenteUsuarios agenteUsu = new AgenteUsuarios();
                    DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
                    AgenteExpedientes agenteExp = new AgenteExpedientes();
                    var datosExp = agenteExp.ObtenerExpedientePorID(expediente.Idexpediente.ToString());

                    datosRoles.idempleadogp = agenteExp.ObtenerEmpleadoGpPorPetAmecExp(datosRoles.IdPeticionario, datosExp.Amec, datosExp.Idexpediente).IdEmpleadoGp;
                    DAmec damec = agenteExp.ObtenerEntidadAMECporID(datosExp.Idamec.ToString());
                    mail.EnvioReservaMail(body, this.expediente.Idexpediente.ToString(), datosRoles.idempleadogp, true, ConfigUtil.GetAppSetting(Constantes.AppParams.CopiaContacto), damec.idconfempresa == null ? 0 : damec.idconfempresa.Value);
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
                                  "Id de servicio: " + this.CurrentIdServicio.ToString() + "\n" +
                                  "Descripción: " + this.descripcionTextBox.Text + "\n" +
                                  "Tipo: " + this.tipoTextBox.Text + "\n" +
                                  "Sede: " + this.sedeTextBox.Text + "\n" +
                                  "Importe máximo: " + this.importeTextBox.Text + "\n" +
                                  "Fecha inicio: " + this.fechaInicioInputDatePickerControl.Value + "\n" +
                                  "Hora inicio: " + this.horaInicioTimeInputBox.Value + "\n" +
                                  "Fecha final: " + this.fechaFinalInputDatePickerControl.Value + "\n" +
                                  "Hora final: " + this.horaFinalTimeInputBox.Value + "\n";

                    string pasajeros = "";
                    if (Test.NumPassengers > 0)
                    {
                        pasajeros = Test.ListaPasajeros;
                        body = body + "\n" + "Pasajeros: " + pasajeros + "\n";
                    }
                    else
                    {
                        body = body + "\n" + "Pasajeros: No hay ninguno seleccionado \n";
                    }
                    if (!string.IsNullOrEmpty(this.Test.Observaciones))
                    {
                        body = body + "Observaciones pasajeros: " + this.Test.Observaciones;
                    }
                    AgenteUsuarios agenteUsu = new AgenteUsuarios();
                    DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
                    AgenteExpedientes agenteExp = new AgenteExpedientes();
                    var datosExp = agenteExp.ObtenerExpedientePorID(expediente.Idexpediente.ToString());

                    datosRoles.idempleadogp = agenteExp.ObtenerEmpleadoGpPorPetAmecExp(datosRoles.IdPeticionario, datosExp.Amec, datosExp.Idexpediente).IdEmpleadoGp;
                    DAmec damec = agenteExp.ObtenerEntidadAMECporID(datosExp.Idamec.ToString());
                    mail.EnvioReservaMail(body, this.expediente.Idexpediente.ToString(), datosRoles.idempleadogp, false, ConfigUtil.GetAppSetting(Constantes.AppParams.CopiaContacto), damec.idconfempresa == null ? 0 : damec.idconfempresa.Value);
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

        #region Inserts
        private void InsertReserva(SqlConnection conn, SqlTransaction trans)
        {
            DDatosPersonalesUsuario datos = (DDatosPersonalesUsuario)HttpContext.Current.Session["DatosUsuario"];

            string consulta = "INSERT INTO reservasviajes (idreserva, reserva, fechapeticion, idestado, idpeticionario, observaciones, fkidexpediente, LastUpd) VALUES " +
                              "(" + GetNextIdReserva() + ", '', '" + DateTime.Now.ToString("yyyy/MM/dd") + "', 'AB'," + datos.IdPeticionario + ", '" + this.Test.Observaciones.Replace("'", "") + "', " + int.Parse(Request.QueryString["idexp"]) + ", '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "')";
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

            if (descripcionTextBox.Text == "")
            {
                ShowMessage("La descripción es un campo requerido", null);
                descripcionTextBox.Focus();
                return false;
            }

            if (fechaInicioInputDatePickerControl.Value != "" && fechaFinalInputDatePickerControl.Value != "")
            {
                if (DateTime.Parse(this.fechaInicioInputDatePickerControl.Value) > DateTime.Parse(this.fechaFinalInputDatePickerControl.Value))
                {
                    ShowMessage("La fecha de fin no puede ser anterior a la  fecha de inicio.", null);
                    fechaFinalInputDatePickerControl.Focus();
                    return false;
                }
                else if ((DateTime.Parse(this.fechaInicioInputDatePickerControl.Value) == DateTime.Parse(this.fechaFinalInputDatePickerControl.Value)))
                {
                    if (!String.IsNullOrEmpty(horaInicioTimeInputBox.Value) && !String.IsNullOrEmpty(horaFinalTimeInputBox.Value))
                    {
                        if (DateTime.Parse(this.horaInicioTimeInputBox.Value) > DateTime.Parse(this.horaFinalTimeInputBox.Value))
                        {
                            ShowMessage("La hora de fin no puede ser anterior a la hora de inicio.", null);
                            horaFinalTimeInputBox.Focus();
                            return false;
                        }
                    }

                }
            }

            if (this.Test.NumPassengers == 0)
            {
                ShowMessage("Debe introducir participantes", null);
                return false;
            }

            return ok;
        }

        private int GetNextIdServicio()
        {
            int id = 0;
            string consulta = "SELECT MAX(idservicioactividad) FROM serviciosreservasactividades";
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
            string consulta = "SELECT MAX(idreserva) FROM reservasviajes";
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

        private int GetNextIdServicioReserva()
        {
            int id = 0;
            string consulta = "SELECT MAX(idservicio) FROM serviciosreservasviajes";
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

        private int GetNextIdActividadPassengerList()
        {
            int id = 0;
            string consulta = "SELECT MAX(idactividadpassengerlist) FROM actividades_passengers_list";
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
            double d = 0;
            return Double.TryParse(value, out d);
        }
        #endregion
    }
}