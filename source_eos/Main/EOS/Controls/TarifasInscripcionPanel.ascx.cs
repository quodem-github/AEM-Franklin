using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using EOS.Entidades.Datos;
using EOS.Web;


namespace EOS.Controls
{
    public partial class TarifasInscripcionPanel : UserControl
    {
        protected int currentidservicio = 0;
        protected int CurrentIdServicio
        {
            get { return this.currentidservicio; }
            set { this.currentidservicio = value; }
        }

        protected int? currentidreserva = null;
        protected int? CurrentIdReserva
        {
            get { return this.currentidreserva; }
            set { this.currentidreserva = value; }
        }

        protected int idServicioReserva = 0;
        protected int IdServicioReserva
        {
            get { return this.idServicioReserva; }
            set { this.idServicioReserva = value; }
        }

        protected String idLastSelectedTarifaInscripcion
        {
            get
            {
                return (String)ViewState["idLastSelectedTarifaInscripcion"];
            }
            set
            {
                ViewState["idLastSelectedTarifaInscripcion"] = value;
            }
        }
        private string state = "";
        private string statename = "";
        private bool showAlternative = false;

        private DCabeceraExpedienteAmpliado expediente
        {
            get
            {
                AgenteExpedientes agenteExp = new AgenteExpedientes();
                DCabeceraExpedienteAmpliado value = agenteExp.ObtenerExpedientePorID(Request.QueryString["idexp"]);
                HttpContext.Current.Session["currentFKIdCongreso"] = value.Idactividad;

                return value;
            }
        }

        public bool existePregarga
        {
            get { return this.eoscTarifasInscripcionListView.existePregarga; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //1702/2011 EAS.
            //TipoActividad tipoactividad = (TipoActividad)Session["tipoactividad"];


            if (Request.QueryString["tab"].ToString() == "0")
            {
                showAlternative = (Request.QueryString["view"] != null && Request.QueryString["view"].ToString() == "alternativa");
                this.Test.IdExpediente = this.expediente.Idexpediente.ToString();

                if (Request.QueryString["idres"] != null)
                {
                    this.CurrentIdReserva = int.Parse(Request.QueryString["idres"].ToString());
                    GetServicioReserva();
                    this.Test.IdServicio = currentidservicio.ToString();
                    this.Alternativas.Visible = showAlternative;
                    this.Alternativas.IdServicio = this.CurrentIdServicio.ToString();
                    this.Test.Enabled = !showAlternative;

                    if (!this.IsPostBack)
                    {
                        LoadReserva(CurrentIdReserva);
                    }
                }
                //1702/2011 EAS.
                //if (tipoactividad == TipoActividad.Inscripción) 
                SetButtons();

                // Muestra el estado
                if (!string.IsNullOrEmpty(statename))
                {
                    this.lblEstado.Text = statename;
                }
                this.lblEstado.CssClass = string.Format("eosImagenEstado eosImagenLeyenda{0}", state);
            }
        }

        private void GetServicioReserva()
        {
            // Recupera el id de servicio a partir del id de reserva
            string consulta = "SELECT * FROM serviciosreservasinscripciones SH LEFT JOIN serviciosreservasviajes SR ON SH.idservicioinscripcion = SR.idservicioinscripcion LEFT JOIN reservasviajes RV ON SR.idreserva = RV.idreserva WHERE SR.idreserva = " + this.CurrentIdReserva;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();
                if (rea.Read())
                {
                    this.CurrentIdServicio = int.Parse(rea["idservicioinscripcion"].ToString());
                    //Jose Laguna 06-04-2010 Esto lo añadimos para que no pierda el estado cuando modificas la alternativa
                    this.state = rea["idEstado"] != DBNull.Value ? rea["idEstado"].ToString() : "";
                    this.statename = GetStateName();

                    //Xavier Morell (GP)
                    //this.eoscTarifasManualInputPanel.tipoTextBox.Text = rea.GetString("tipoinscripcion");
                    //this.eoscTarifasManualInputPanel.importeTextBox.Text = rea.GetString("pvp");
                }
                else
                {
                    this.CurrentIdServicio = 0;
                }
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                this.CurrentIdServicio = 0;
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        private void LoadReserva(int? CurrentIdReserva)
        {
            string consulta = "SELECT " +
                             "serviciosreservasinscripciones.idservicioinscripcion, " +
                             "serviciosreservasinscripciones.idtarifainscripcion, " +
                             "serviciosreservasinscripciones.observaciones, " +
                             "serviciosreservasinscripciones.pvp, " +
                             "serviciosreservasinscripciones.tipoinscripcion, " +
                             "idestado " +
                             "FROM serviciosreservasinscripciones " +
                             "LEFT JOIN serviciosreservasviajes ON serviciosreservasinscripciones.idservicioinscripcion = serviciosreservasviajes.idservicioinscripcion " +
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
                    this.idLastSelectedTarifaInscripcion = rea[1] != DBNull.Value ? rea.GetInt32(1).ToString() : "0";
                    this.Test.Observaciones = rea[2] != DBNull.Value ? rea.GetString(2) : "";
                    this.eoscTarifasManualInputPanel.Importe = rea[3] != DBNull.Value ? rea.GetDouble(3) : 0;
                    this.eoscTarifasManualInputPanel.Tipo = rea[4] != DBNull.Value ? rea.GetString(4) : "";
                    state = rea[5] != DBNull.Value ? rea.GetString(5) : "";
                    statename = GetStateName();

                    if (this.idLastSelectedTarifaInscripcion == "0")
                    {
                        this.eoscTarifasManualInputPanel.importeTextBox.Enabled = !showAlternative;
                        this.eoscTarifasManualInputPanel.tipoTextBox.Enabled = !showAlternative;
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
            this.eoscTarifasInscripcionListView.OnTarifaSelected += new EventHandler(eoscTarifasInscripcionListView_OnTarifaSelected);

        }

        void eoscTarifasInscripcionListView_OnTarifaSelected(object sender, EventArgs e)
        {
            TarifaSelectedEventArgs args = (TarifaSelectedEventArgs)e;
            this.idLastSelectedTarifaInscripcion = args.selectedItem;
            /*
            if (this.idLastSelectedTarifaInscripcion != null)
            {
                this.inscripcionGestorParticipantesPanel.Observaciones = this.idLastSelectedTarifaInscripcion.cancelacion;
            } */
            if (this.idLastSelectedTarifaInscripcion == "0")
            {
                eoscTarifasManualInputPanel.tipoTextBox.Enabled = true;
                eoscTarifasManualInputPanel.Tipo = "";
                eoscTarifasManualInputPanel.importeTextBox.Enabled = true;
                eoscTarifasManualInputPanel.Importe = 0;
                this.Test.Observaciones = "";
            }
            else
            {
                eoscTarifasManualInputPanel.tipoTextBox.Enabled = false;
                eoscTarifasManualInputPanel.importeTextBox.Enabled = false;

                string currentidcongreso = HttpContext.Current.Session["currentFKIdCongreso"].ToString();
                //Ismael Ameller 30-03-2011
                //string consulta = "SELECT TarifasInscripcion.idtarifainscripcion, tiposinscripcion.tipoinscripcion, " +
                //"tarifasinscripcion.pvp, TarifasInscripcion.cancelacion " +
                //"FROM TarifasInscripcion " +
                //"LEFT JOIN tiposinscripcion ON tiposinscripcion.idtipoinscripcion = TarifasInscripcion.idtipoinscripcion " +
                //"WHERE fkidcongreso = " + currentidcongreso + " AND TarifasInscripcion.idtarifainscripcion = " + this.idLastSelectedTarifaInscripcion;// + currentidcongreso;
                //FIN Ismael Ameller 30-03-2011
                //Ismael Ameller 30-03-2011
                string consulta = "SELECT TarifasInscripcion.idtarifainscripcion, TarifasInscripcion.descripcion, " +
                "tarifasinscripcion.pvp, TarifasInscripcion.cancelacion " +
                "FROM TarifasInscripcion " +
                "LEFT JOIN tiposinscripcion ON tiposinscripcion.idtipoinscripcion = TarifasInscripcion.idtipoinscripcion " +
                "WHERE fkidcongreso = " + currentidcongreso + " AND TarifasInscripcion.idtarifainscripcion = " + this.idLastSelectedTarifaInscripcion;
                //FIN Ismael Ameller 30-03-2011
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);

                try
                {
                    conn.Open();
                    SqlCommand comm = new SqlCommand(consulta, conn);
                    SqlDataReader rea = comm.ExecuteReader();
                    if (rea.Read())
                    {
                        eoscTarifasManualInputPanel.Tipo = rea[1] != DBNull.Value ? rea.GetString(1) : "";
                        eoscTarifasManualInputPanel.Importe = rea[2] != DBNull.Value ? rea.GetDouble(2) : 0;
                        this.Test.Observaciones = rea[3] != DBNull.Value ? rea.GetString(3) : "";
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



        //private void InsertPassengerList()
        //{
        //    wintour_robotEntities db = new Entidades.Modelo.wintour_robotEntities();
        //    passengers_list.updateInscripcionList(db, this.test.ParticipatesSeleccionados, this.expediente.Idexpediente, this.CurrentIdReserva.Value);
        //}

        #region Inserts
        private void InsertReserva(SqlTransaction trans, SqlConnection conn)
        {
            DDatosPersonalesUsuario datos = (DDatosPersonalesUsuario)HttpContext.Current.Session["DatosUsuario"];

            string consulta = "INSERT INTO reservasviajes (idreserva, reserva, fechapeticion, idestado, idpeticionario, observaciones, fkidexpediente, LastUpd) VALUES " +
                              "(" + GetNextIdReserva() + ", '', '" + DateTime.Now.ToString("yyyyMMdd") + "', 'AB'," + datos.IdPeticionario + ", '" + this.Test.Observaciones + "', " + int.Parse(Request.QueryString["idexp"]) + ", '" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "')";
            SqlCommand comm = new SqlCommand(consulta, conn, trans);
            comm.ExecuteNonQuery();
        }

        private void InsertServiciosReservasViajes(int idinscripcion, int idreserva, SqlTransaction trans, SqlConnection conn)
        {
            string consulta = "INSERT INTO serviciosreservasviajes (idservicio, idreserva, idtipobono, fechapeticion, resumenservicio, idservicioactividad, idservicioinscripcion, idserviciohotel, idserviciotransporte) VALUES " +
                   "(" + GetNextIdServicioReserva() + ", " + idreserva + ", 'INS', '" + DateTime.Now.ToString("yyyyMMdd") + "', 'resumen', NULL, " + idinscripcion + ", NULL, NULL)";
            SqlCommand comm = new SqlCommand(consulta, conn, trans);
            comm.ExecuteNonQuery();
        }
        #endregion

        #region Auxiliares
        private int GetNextIdTarifa()
        {
            int id = 0;
            string consulta = "SELECT MAX(idservicioinscripcion) FROM serviciosreservasinscripciones";
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

        private bool ComprobarCamposOK()
        {
            bool ok = false;            

            if (eoscTarifasManualInputPanel.importeTextBox.Text != "")
            {
                if (IsNumeric(eoscTarifasManualInputPanel.importeTextBox.Text))
                {
                    ok = true;
                }
                else
                {
                    eoscTarifasManualInputPanel.importeTextBox.Focus();
                    ShowMessage("El importe debe ser un valor numérico.", null);
                    return false;
                }
            }

            if (eoscTarifasManualInputPanel.tipoTextBox.Text != "")
            {
                ok = true;
            }
            else
            {
                eoscTarifasManualInputPanel.tipoTextBox.Focus();
                ShowMessage("El tipo es un campo requerido", null);
                return false;
            }

            if (this.Test.NumPassengers == 0)
            {
                ShowMessage("Debe introducir participantes", null);
                return false;
            }            

            return ok;
        }

        private bool IsNumeric(string value)
        {
            double d = 0;
            return Double.TryParse(value, out d);
        }
        #endregion

        public void SetButtons()
        {
            this.btnGuardar.Visible = false;
            this.btnEnviar.Visible = false;
            this.btnRechazar.Visible = false;
            this.btnAprobar.Visible = false;
            this.btnCancelar.Visible = false;
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
                case "CNTR": //cancelado con gastos
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
                    else  //Caso exceppcional es como si el estado fuera AB
                    {
                        this.btnGuardar.Visible = true;
                        //si es nuevo (no guardado) no hay que mostrar el boton enviar
                        if (currentidservicio == 0 && (currentidreserva == 0 || currentidreserva == null))
                            this.btnEnviar.Visible = false;
                        else
                            this.btnEnviar.Visible = true;
                        this.btnRechazar.Visible = true;
                        this.btnAprobar.Visible = false;
                        this.btnCancelar.Visible = false;
                    }
                    break;
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
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
        }

        private void ShowMessage(string message, string navigation)
        {
            // Muestra mensaje y navega a una URL especificada
            navigation = (string.IsNullOrEmpty(navigation) ? "closeLoading()" : "document.location='" + navigation + "'");
            string script = String.Format("alert('{0}');{1};", message, navigation);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "dialog", script, true);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Guardar();
        }
        public Boolean Guardar()
        {
            if (!ComprobarCamposOK())
            {
                return false;
            }

            if (this.CurrentIdReserva == null)
            {
                ////INSERT
                CreateRecord();
            }
            //UPDATE
            else
            {
                UpdateRecord();
            }

            return true;

        }

        private void CreateRecord()
        {
            int nextid = GetNextIdTarifa();
            SqlTransaction trans = null;
            // Guardar el servicio de alojamiento
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            string consulta = string.Empty;
            string sImporte = this.eoscTarifasManualInputPanel.Importe.ToString().Replace(",", "@");
            try
            {
                conn.Open();

                string consultaIdConfEmpresa = string.Format("select idconfempresa from amec where idamec = (select top 1 idamec from expediente where idxpediente = {0})", expediente.Idexpediente);
                SqlCommand commandIdConfEmpresa = new SqlCommand(consultaIdConfEmpresa, conn);
                string idConfEmpresa = commandIdConfEmpresa.ExecuteScalar().ToString();

                trans = conn.BeginTransaction(System.Data.IsolationLevel.Serializable);

                //INSERT
                if (this.idLastSelectedTarifaInscripcion == "0")
                {
                    consulta = "INSERT INTO serviciosreservasinscripciones (envioboletin, idservicioinscripcion, idtarifainscripcion, observaciones, inscripcion, tipoinscripcion, pvp, iddatosentrega, idconfempresa) VALUES " +
                                    "('', " + nextid.ToString() + ",  NULL , '" + this.Test.Observaciones.Replace("'", "") + "', '" + eoscTarifasManualInputPanel.tipoTextBox.Text.Replace("'", "") + "', '" + eoscTarifasManualInputPanel.Tipo.Replace("'", "") + "', " + sImporte.Replace("@", ".") + ", NULL, " + idConfEmpresa + ")";
                }
                else
                {
                    consulta = "INSERT INTO serviciosreservasinscripciones (envioboletin, idservicioinscripcion, idtarifainscripcion, observaciones,inscripcion, tipoinscripcion, pvp, iddatosentrega, idconfempresa) VALUES " +
                                "('', " + nextid.ToString() + ",  '" + this.idLastSelectedTarifaInscripcion + "' , '" + this.Test.Observaciones.Replace("'", "") + "', '" + eoscTarifasManualInputPanel.tipoTextBox.Text.Replace("'", "") + "', '" + eoscTarifasManualInputPanel.Tipo.Replace("'", "") + "', " + sImporte.Replace("@", ".") + ", NULL, " + idConfEmpresa + ")";
                }

                SqlCommand comm = new SqlCommand(consulta, conn, trans);
                comm.ExecuteNonQuery();
                int nextreserva = GetNextIdReserva();
                this.CurrentIdReserva = nextreserva;
                InsertReserva(trans, conn);
                this.IdServicioReserva = GetNextIdServicioReserva();
                InsertServiciosReservasViajes(nextid, nextreserva, trans, conn);
                trans.Commit();
                this.Test.IdServicio = nextid.ToString();
                this.Test.Save();

                //InsertPassengerList();

                //Pau Ferrer 09-06-2011 no se tiene que mostrar el boton enviar al guardar
                //this.btnEnviar.Visible = true;
                //FIN Pau Ferrer

                ShowMessage("El servicio se ha creado correctamente.", "DetalleExpediente.aspx?idexp=" + this.expediente.Idexpediente.ToString());

                //añadimos esta llamada al procedure para actualizar los importes de las reservas, expediente y AMEC
                consulta = " exec dbo.actualizar_importes_totales " + this.IdServicioReserva + ";";
                comm = new SqlCommand(consulta, conn);
                comm.ExecuteNonQuery();

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
            }
            conn.Close();
            SetState("AB");
        }

        private void UpdateRecord()
        {
            SqlTransaction trans = null;
            // Guardar el servicio de alojamiento
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            string consulta = string.Empty;
            string sImporte = this.eoscTarifasManualInputPanel.Importe.ToString().Replace(",", "@");

            try
            {
                conn.Open();
                trans = conn.BeginTransaction(System.Data.IsolationLevel.Serializable);
                //UPDATE

                if (this.idLastSelectedTarifaInscripcion == "0")
                {
                    consulta = " UPDATE serviciosreservasinscripciones  " +
                                " SET IdTarifaInscripcion = NULL, " +
                                " observaciones = '" + this.Test.Observaciones.Replace("'", "") + "'," +
                                " pvp = " + sImporte.Replace("@", ".") + "," +
                                " tipoinscripcion = '" + this.eoscTarifasManualInputPanel.Tipo + "', " +
                                " inscripcion = '" + this.eoscTarifasManualInputPanel.Tipo + "' " +
                                " from serviciosreservasinscripciones LEFT JOIN serviciosreservasviajes ON serviciosreservasviajes.idservicioinscripcion = serviciosreservasinscripciones.idservicioinscripcion " +
                                " WHERE serviciosreservasviajes.idreserva = " + CurrentIdReserva.ToString();
                }
                else
                {
                    if (this.idLastSelectedTarifaInscripcion != "0")
                    {
                        consulta = " UPDATE serviciosreservasinscripciones " +
                                    "SET IdTarifaInscripcion = " + int.Parse(this.idLastSelectedTarifaInscripcion) + ", " +
                                    "observaciones = '" + this.Test.Observaciones.Replace("'", "") + "', " +
                                    "pvp = " + sImporte.Replace("@", ".") + "," +
                                    "tipoinscripcion = '" + this.eoscTarifasManualInputPanel.Tipo + "', " +
                                    "inscripcion = '" + eoscTarifasManualInputPanel.tipoTextBox.Text + "' " +
                                    " from serviciosreservasinscripciones LEFT JOIN serviciosreservasviajes ON serviciosreservasviajes.idservicioinscripcion = serviciosreservasinscripciones.idservicioinscripcion " +
                                    "WHERE serviciosreservasviajes.idreserva = " + CurrentIdReserva.ToString();
                    }
                    else
                    {
                        consulta = "UPDATE serviciosreservasactividades " +
                                    "SET observaciones = '" + this.Test.Observaciones.Replace("'", "") + "', " +
                                    "pvp = " + sImporte.Replace("@", ".") + ", " +
                                    "tipoinscripcion = '" + this.eoscTarifasManualInputPanel.Tipo + "', " +
                                    "inscripcion = '" + eoscTarifasManualInputPanel.tipoTextBox.Text + "' " +
                                    " from serviciosreservasactividades LEFT JOIN serviciosreservasviajes ON serviciosreservasviajes.idservicioinscripcion = serviciosreservasinscripciones.idservicioinscripcion" + 
                                    "WHERE serviciosreservasviajes.idreserva = " + CurrentIdReserva.ToString();
                    }

                }
                SqlCommand comm = new SqlCommand(consulta, conn, trans);
                comm.ExecuteNonQuery();
                trans.Commit();
                this.Test.Save();

                ShowMessage("El servicio se ha actualizado correctamente.", "DetalleExpediente.aspx?idexp=" + this.expediente.Idexpediente.ToString());

                //añadimos esta llamada al procedure para actualizar los importes de las reservas, expediente y AMEC
                consulta = " exec dbo.actualizar_importes_totales " + GetServicioByReserva() + ";";
                comm = new SqlCommand(consulta, conn);
                comm.ExecuteNonQuery();

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
            SetState("AB");
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

                    if (Guardar())
                    {
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
                                      "Tipo de Reserva: INSCRIPCION \n" +
                                      "Id de servicio: " + this.CurrentIdServicio.ToString() + "\n" +
                                      "Descripción del servicio: " + this.eoscTarifasManualInputPanel.tipoTextBox.Text + "\n" +
                                      "Id reserva: " + this.currentidreserva.ToString() + "\n" +
                                      "Precio: " + this.eoscTarifasManualInputPanel.importeTextBox.Text + " euros";

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

        protected void btnCancelar_Click(object sender, EventArgs e)
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
                                  "Tipo de Reserva: INSCRIPCION \n" +
                                  "Id de servicio: " + this.CurrentIdServicio.ToString() + "\n" +
                                  "Descripción del servicio: " + this.eoscTarifasManualInputPanel.tipoTextBox.Text + "\n" +
                                  "Id reserva: " + this.currentidreserva.ToString() + "\n" +
                                  "Precio: " + this.eoscTarifasManualInputPanel.importeTextBox.Text + " euros";

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

    }
}