using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Entidades.Datos;
using EOS.Web;


namespace EOS.Controls
{
    public partial class TransportePanel : System.Web.UI.UserControl
    {
        #region Propiedades

        private string idExpediente = "";
        private int idReservaServicio = 0;
        private int idReserva = 0;
        private int idServicio = 0;
        private int idCongreso = 0;
        private string idUsuario = "";
        private string state = "";
        private string statename = "";

        private bool showAlternative = false;

        private DCabeceraExpedienteAmpliado expediente
        {
            get
            {
                if (HttpContext.Current.Session["currentExpediente"] != null)
                {
                    return (DCabeceraExpedienteAmpliado)HttpContext.Current.Session["currentExpediente"];
                }
                else
                {
                    AgenteExpedientes agenteExp = new AgenteExpedientes();
                    DCabeceraExpedienteAmpliado value = agenteExp.ObtenerExpedientePorID(idExpediente);

                    return value;
                }
            }
        }
        private DVCongresos congreso
        {
            get
            {
                AgenteExpedientes agenteExp = new AgenteExpedientes();

                DVCongresos value = agenteExp.ObtenerCongreso(expediente.Idactividad.Value);

                return value;
            }

            set
            {
                HttpContext.Current.Session["currentCongreso"] = value;
            }
        }
        #endregion

        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            //1702/2011 EAS.
            //TipoActividad tipoactividad = (TipoActividad)Session["tipoactividad"];
            if (Request.QueryString["tab"].ToString() != "2") return;

            //rellena propiedades
            showAlternative = (Request.QueryString["view"] != null && Request.QueryString["view"].ToString() == "alternativa");
            idExpediente = Request.QueryString.Get("idexp");
            idReservaServicio = Request.QueryString["idres"] == null ? 0 : Int32.Parse(Request.QueryString["idres"].ToString());
            idReserva = idReservaServicio > 0 ? GetReservaByServicio() : 0;
            idCongreso = HttpContext.Current.Session["currentFKIdCongreso"] == null ? 0 : Int32.Parse(HttpContext.Current.Session["currentFKIdCongreso"].ToString());
            idUsuario = ((DDatosPersonalesUsuario)HttpContext.Current.Session["DatosUsuario"]).IdPeticionario.ToString();


            this.Alternativas.IdServicio = idReservaServicio.ToString();

            this.Test.IdExpediente = idExpediente;
            this.Test.IdServicio = idReservaServicio.ToString();

            if (!IsPostBack)
            {
                if (idReservaServicio > 0)
                {
                    GetServicioTransporte();
                }
                else
                {
                    Inicializa();
                }
                //1702/2011 EAS.
                //if (tipoactividad == TipoActividad.Transporte) SetButtons();
                SetButtons();
            }
            showControlesAlternative();

            // Muestra el estado
            this.lblEstado.Text = statename;
            this.lblEstado.CssClass = string.Format("eosImagenEstado eosImagenLeyenda{0}", state);
        }

        #region Botones

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Guardar())
            {
                ShowMessage("El servicio se ha guardado correctamente.", "DetalleExpediente.aspx?idexp=" + idExpediente);
            }
            else
            {
                ShowMessage("Se ha producido un error al guardar el servicio.", "");
            }

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (idReserva > 0)
            {
                EOS.Entidades.Modelo.wintour_robotEntities db = new EOS.Entidades.Modelo.wintour_robotEntities();
                EOS.Entidades.Modelo.reservasviajes currExp = db.reservasviajes.Single(exp => exp.idreserva == idReserva);
                currExp.idestado = MaquinaEstados.Cancelado;
                db.SaveChanges();
                ShowMessage("Se ha cancelado el servicio correctamente.", "DetalleExpediente.aspx?idexp=" + idExpediente);

            }
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            if (idReserva > 0)
            {
                //EOS.Entidades.Modelo.wintour_robotEntities db = new EOS.Entidades.Modelo.wintour_robotEntities();
                //EOS.Entidades.Modelo.reservasviajes currExp = db.reservasviajes.Single(exp => exp.idreserva == idReserva);
                //currExp.idestado = MaquinaEstados.Enviado;
                //db.SaveChanges();

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

                        if (!Guardar())
                        {
                            ShowMessage("Se ha producido un error al actualizar los datos y no se puede enviar", null);
                            return;
                        }

                        this.btnEnviar.Visible = true;

                        //IAV ENVIAR MAIL

                        Mail mail = new Mail();
                        DCabeceraExpedienteAmpliado expediente = null;
                        AgenteExpedientes agenteExp = new AgenteExpedientes();
                        expediente = agenteExp.ObtenerExpedientePorID(this.idExpediente);

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
                                      "Tipo de Reserva: TRANSPORTE \n" +
                                      "Id de servicio: " + this.idServicio.ToString() + "\n\n" +
                                      "IDA \n" +
                                      "Fecha de salida: " + fechaSalidaIdaInputDatePickerControl1.Value + "\n" +
                                      "Locomoción: " + locomocionIdaDropDownList1.SelectedItem.Text + "\n" +
                                      "Nº Vuelo/Tren: " + numeroVueloIdaTrenTextBox1.Text + "\n" +
                                      "Origen: " + this.origenIda1.Text + "\n" +
                                      "Destino: " + this.destinoIda1.Text + "\n" +
                                      "Hora de salida: " + this.horaSalidaIdaTimeInputBox1.Value + "\n" +
                                      "Hora de llegada: " + this.horaLlegadaIdaTimeInputBox1.Value + "\n\n";
                        if (chbAltIDA.Checked)
                        {
                            body = body +
                                      "Fecha de salida: " + fechaSalidaIdaInputDatePickerControl2.Value + "\n" +
                                      "Locomoción: " + locomocionIdaDropDownList2.SelectedItem.Text + "\n" +
                                      "Nº Vuelo/Tren: " + numeroVueloIdaTrenTextBox2.Text + "\n" +
                                      "Origen: " + this.origenIda2.Text + "\n" +
                                      "Destino: " + this.destinoIda2.Text + "\n" +
                                      "Hora de salida: " + this.horaSalidaIdaTimeInputBox2.Value + "\n" +
                                      "Hora de llegada: " + this.horaLlegadaIdaTimeInputBox2.Value + "\n";
                        }
                        if (!string.IsNullOrEmpty(txtObservacionIda.Text))
                        {
                            body = body + "Observaciones Ida: " + this.txtObservacionIda.Text + "\n\n";
                        }
                        else
                        {
                            body = body + "\n";
                        }
                        body = body + "REGRESO \n" +
                            "Fecha de salida: " + fechaSalidaRegresoInputDatePickerControl1.Value + "\n" +
                            "Locomoción: " + locomocionRegresoDropDownList1.SelectedItem.Text + "\n" +
                            "Nº Vuelo/Tren: " + numeroVueloTrenRegresoTextBox1.Text + "\n" +
                            "Origen: " + this.origenRegreso1.Text + "\n" +
                            "Destino: " + this.destinoRegreso1.Text + "\n" +
                            "Hora de salida: " + this.horaSalidaRegresoTimeInputBox1.Value + "\n" +
                            "Hora de llegada: " + this.horaLlegadaRegresoTimeInputBox1.Value + "\n\n";

                        if (chbAltRegreso.Checked)
                        {
                            body = body +
                                "Fecha de salida: " + fechaSalidaRegresoInputDatePickerControl2.Value + "\n" +
                                "Locomoción: " + locomocionRegresoDropDownList2.SelectedItem.Text + "\n" +
                                "Nº Vuelo/Tren: " + numeroVueloTrenRegresoTextBox2.Text + "\n" +
                                "Origen: " + this.origenRegreso2.Text + "\n" +
                                "Destino: " + this.destinoRegreso2.Text + "\n" +
                                "Hora de salida: " + this.horaSalidaRegresoTimeInputBox2.Value + "\n" +
                                "Hora de llegada: " + this.horaLlegadaRegresoTimeInputBox2.Value + "\n";
                        }
                        if (!string.IsNullOrEmpty(txtObservacionRegreso.Text))
                        {
                            body = body + "Observaciones Regreso: " + this.txtObservacionRegreso.Text + "\n\n";
                        }
                        else
                        {
                            body = body + "\n";
                        }
                        if (!string.IsNullOrEmpty(importeMaximoTextBox.Text))
                        {
                            body = body + "Importe máximo: " + importeMaximoTextBox.Text + " euros \n";
                        }

                        string pasajeros = "";
                        if (Test.NumPassengers > 0)
                        {
                            pasajeros = Test.ListaPasajeros;
                            body = body + "\n" + "Pasajeros: " + pasajeros + "\n";
                        }
                        else
                        {
                            body = body + "\n" + "Pasajeros: No hay ninguno seleccionado";
                        }
                        if (!string.IsNullOrEmpty(this.Test.Observaciones))
                        {
                            body = body + "Observaciones pasajeros: " + this.Test.Observaciones + "\n";
                        }
                        AgenteUsuarios agenteUsu = new AgenteUsuarios();
                        DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
                        var datosExp = agenteExp.ObtenerExpedientePorID(expediente.Idexpediente.ToString());

                        datosRoles.idempleadogp = agenteExp.ObtenerEmpleadoGpPorPetAmecExp(datosRoles.IdPeticionario, datosExp.Amec, datosExp.Idexpediente).IdEmpleadoGp;
                        DAmec damec = agenteExp.ObtenerEntidadAMECporID(datosExp.Idamec.ToString());
                        mail.EnvioReservaMail(body, this.idExpediente.ToString(), datosRoles.idempleadogp, true, ConfigUtil.GetAppSetting(Constantes.AppParams.CopiaContacto), damec.idconfempresa == null ? 0 : damec.idconfempresa.Value);
                        //FIN IAV ENVIAR MAIL

                        ShowMessage("Se ha enviado el servicio correctamente.", "DetalleExpediente.aspx?idexp=" + idExpediente);

                    }
                }
                catch { }
            }
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
                    ShowMessage("Se ha aprobado el servicio correctamente.", "DetalleExpediente.aspx?idexp=" + idExpediente);
                    //Ismael Ameller 21-04-2011 Envio mail al aprobar
                    Mail mail = new Mail();
                    DCabeceraExpedienteAmpliado expediente = null;
                    AgenteExpedientes agenteExp = new AgenteExpedientes();
                    expediente = agenteExp.ObtenerExpedientePorID(this.idExpediente);

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
                                  "Tipo de Reserva: TRANSPORTE \n" +
                                  "Id de servicio: " + this.idServicio.ToString() + "\n\n" +
                                  "IDA \n" +
                                  "Fecha de salida: " + fechaSalidaIdaInputDatePickerControl1.Value + "\n" +
                                  "Locomoción: " + locomocionIdaDropDownList1.SelectedItem.Text + "\n" +
                                  "Nº Vuelo/Tren: " + numeroVueloIdaTrenTextBox1.Text + "\n" +
                                  "Origen: " + this.origenIda1.Text + "\n" +
                                  "Destino: " + this.destinoIda1.Text + "\n" +
                                  "Hora de salida: " + this.horaSalidaIdaTimeInputBox1.Value + "\n" +
                                  "Hora de llegada: " + this.horaLlegadaIdaTimeInputBox1.Value + "\n\n";
                    if (chbAltIDA.Checked)
                    {
                        body = body +
                                  "Fecha de salida: " + fechaSalidaIdaInputDatePickerControl2.Value + "\n" +
                                  "Locomoción: " + locomocionIdaDropDownList2.SelectedItem.Text + "\n" +
                                  "Nº Vuelo/Tren: " + numeroVueloIdaTrenTextBox2.Text + "\n" +
                                  "Origen: " + this.origenIda2.Text + "\n" +
                                  "Destino: " + this.destinoIda2.Text + "\n" +
                                  "Hora de salida: " + this.horaSalidaIdaTimeInputBox2.Value + "\n" +
                                  "Hora de llegada: " + this.horaLlegadaIdaTimeInputBox2.Value + "\n";
                    }
                    if (!string.IsNullOrEmpty(txtObservacionIda.Text))
                    {
                        body = body + "Observaciones Ida: " + this.txtObservacionIda.Text + "\n\n";
                    }
                    else
                    {
                        body = body + "\n";
                    }
                    body = body + "REGRESO \n" +
                        "Fecha de salida: " + fechaSalidaRegresoInputDatePickerControl1.Value + "\n" +
                        "Locomoción: " + locomocionRegresoDropDownList1.SelectedItem.Text + "\n" +
                        "Nº Vuelo/Tren: " + numeroVueloTrenRegresoTextBox1.Text + "\n" +
                        "Origen: " + this.origenRegreso1.Text + "\n" +
                        "Destino: " + this.destinoRegreso1.Text + "\n" +
                        "Hora de salida: " + this.horaSalidaRegresoTimeInputBox1.Value + "\n" +
                        "Hora de llegada: " + this.horaLlegadaRegresoTimeInputBox1.Value + "\n\n";

                    if (chbAltRegreso.Checked)
                    {
                        body = body +
                            "Fecha de salida: " + fechaSalidaRegresoInputDatePickerControl2.Value + "\n" +
                            "Locomoción: " + locomocionRegresoDropDownList2.SelectedItem.Text + "\n" +
                            "Nº Vuelo/Tren: " + numeroVueloTrenRegresoTextBox2.Text + "\n" +
                            "Origen: " + this.origenRegreso2.Text + "\n" +
                            "Destino: " + this.destinoRegreso2.Text + "\n" +
                            "Hora de salida: " + this.horaSalidaRegresoTimeInputBox2.Value + "\n" +
                            "Hora de llegada: " + this.horaLlegadaRegresoTimeInputBox2.Value + "\n";
                    }
                    if (!string.IsNullOrEmpty(txtObservacionRegreso.Text))
                    {
                        body = body + "Observaciones Regreso: " + this.txtObservacionRegreso.Text + "\n\n";
                    }
                    else
                    {
                        body = body + "\n";
                    }
                    if (!string.IsNullOrEmpty(importeMaximoTextBox.Text))
                    {
                        body = body + "Importe máximo: " + importeMaximoTextBox.Text + " euros \n";
                    }

                    string pasajeros = "";
                    if (Test.NumPassengers > 0)
                    {
                        pasajeros = Test.ListaPasajeros;
                        body = body + "\n" + "Pasajeros: " + pasajeros + "\n";
                    }
                    else
                    {
                        body = body + "\n" + "Pasajeros: No hay ninguno seleccionado";
                    }
                    if (!string.IsNullOrEmpty(this.Test.Observaciones))
                    {
                        body = body + "Observaciones pasajeros: " + this.Test.Observaciones + "\n";
                    }
                    AgenteUsuarios agenteUsu = new AgenteUsuarios();
                    DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
                    var datosExp = agenteExp.ObtenerExpedientePorID(expediente.Idexpediente.ToString());

                    datosRoles.idempleadogp = agenteExp.ObtenerEmpleadoGpPorPetAmecExp(datosRoles.IdPeticionario, datosExp.Amec, datosExp.Idexpediente).IdEmpleadoGp;
                    DAmec damec = agenteExp.ObtenerEntidadAMECporID(datosExp.Idamec.ToString());
                    mail.EnvioReservaMail(body, this.idExpediente.ToString(), datosRoles.idempleadogp, false, ConfigUtil.GetAppSetting(Constantes.AppParams.CopiaContacto), damec.idconfempresa == null ? 0 : damec.idconfempresa.Value);
                    //FIN Ismael Ameller 21-04-2011 Envio mail al aprobar
                }
            }
            catch { }
        }

        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            SetState("AN");
            ShowMessage("Se ha rechazado el servicio correctamente.", "DetalleExpediente.aspx?idexp=" + idExpediente);
        }


        #endregion

        #region checks

        protected void chbAltIDA_CheckedChanged(object sender, EventArgs e)
        {
            int nIdPoblacion = congreso.IdPoblacion;

            AgenteMaestros agente = new AgenteMaestros();
            DVPoblacion dpSelect = agente.ObtenerPoblacionesTodas(null, expediente.idconfempresa ?? -1).FirstOrDefault(f => f.IdPoblacion == nIdPoblacion);

            if (!((CheckBox)sender).Checked)
            {
                this.hdCombinacionIda.Value = "false";
                this.destinoIda1.Text = dpSelect.Poblacion;
                this.origenIda2.Text = string.Empty;
                this.destinoIda2.Text = string.Empty;

            }
            else
            {
                this.hdCombinacionIda.Value = "true";
                this.origenIda2.Text = destinoIda1.Text;
                this.destinoIda2.Text = dpSelect.Poblacion;
            }
            this.fechaSalidaIdaInputDatePickerControl2.Enabled = this.chbAltIDA.Checked;
            this.fechaSalidaIdaInputDatePickerControl2.InputTextBox.Enabled = this.chbAltRegreso.Checked;
            this.fechaSalidaIdaInputDatePickerControl2.ClickImageButton.Enabled = this.chbAltRegreso.Checked;
            this.locomocionIdaDropDownList2.Enabled = this.chbAltIDA.Checked;
            this.numeroVueloIdaTrenTextBox2.Enabled = this.chbAltIDA.Checked;
            this.origenIda2.Enabled = this.chbAltIDA.Checked;
            this.destinoIda2.Enabled = this.chbAltIDA.Checked;
            this.horaSalidaIdaTimeInputBox2.Enabled = this.chbAltIDA.Checked;
            this.horaSalidaIdaTimeInputBox2.InputTextBox.Enabled = this.chbAltRegreso.Checked;
            this.horaLlegadaIdaTimeInputBox2.Enabled = this.chbAltIDA.Checked;
            this.horaLlegadaIdaTimeInputBox2.InputTextBox.Enabled = this.chbAltRegreso.Checked;
        }

        protected void chbAltREGRESO_CheckedChanged(object sender, EventArgs e)
        {
            if (!((CheckBox)sender).Checked)
            {
                this.hdCombinacionRegreso.Value = "false";
                this.origenRegreso2.Text = string.Empty;
                this.destinoRegreso2.Text = string.Empty;
            }
            else
            {
                this.hdCombinacionRegreso.Value = "true";
                this.origenIda2.Text = destinoIda1.Text;
                this.origenRegreso2.Text = this.destinoRegreso1.Text;
            }
            this.fechaSalidaRegresoInputDatePickerControl2.Enabled = this.chbAltRegreso.Checked;
            this.fechaSalidaRegresoInputDatePickerControl2.InputTextBox.Enabled = this.chbAltRegreso.Checked;
            this.fechaSalidaRegresoInputDatePickerControl2.ClickImageButton.Enabled = this.chbAltRegreso.Checked;
            this.locomocionRegresoDropDownList2.Enabled = this.chbAltRegreso.Checked;
            this.numeroVueloTrenRegresoTextBox2.Enabled = this.chbAltRegreso.Checked;
            this.origenRegreso2.Enabled = this.chbAltRegreso.Checked;
            this.destinoRegreso2.Enabled = this.chbAltRegreso.Checked;
            this.horaSalidaRegresoTimeInputBox2.Enabled = this.chbAltRegreso.Checked;
            this.horaSalidaRegresoTimeInputBox2.InputTextBox.Enabled = this.chbAltRegreso.Checked;
            this.horaLlegadaRegresoTimeInputBox2.Enabled = this.chbAltRegreso.Checked;
            this.horaLlegadaRegresoTimeInputBox2.InputTextBox.Enabled = this.chbAltRegreso.Checked;
        }


        #endregion


        #endregion

        #region metodos

        public Boolean validarCampos()
        {

            if (this.fechaSalidaRegresoInputDatePickerControl1.Value != "" && this.fechaSalidaIdaInputDatePickerControl1.Value != "")
            {
                if (DateTime.Parse(this.fechaSalidaRegresoInputDatePickerControl1.Value) < DateTime.Parse(this.fechaSalidaIdaInputDatePickerControl1.Value))
                {
                    ShowMessage("La fecha de regreso no puede ser anterior a la fecha de ida.", null);
                    return false;
                }
            }
            if (this.fechaSalidaIdaInputDatePickerControl2.Value != "" && this.fechaSalidaRegresoInputDatePickerControl2.Value != "")
            {
                if (DateTime.Parse(this.fechaSalidaRegresoInputDatePickerControl2.Value) < DateTime.Parse(this.fechaSalidaIdaInputDatePickerControl2.Value))
                {
                    ShowMessage("La fecha de regreso 2 no puede ser anterior a la fecha de ida 2.", null);
                    return false;
                }
            }

            if (this.Test.NumPassengers == 0)
            {
                ShowMessage("Debe introducir participantes", null);
                return false;
            }

            return true;

        }

        /// <summary>
        ///  Ismael Ameller 21-04-2011 Rehecho el modificar transporte
        /// </summary>
        /// <returns></returns>
        public Boolean modificarServicio()
        {

            string Ida = locomocionIdaDropDownList1.SelectedValue;
            string IdTipoBono_ida1 = locomocionIdaDropDownList1.SelectedValue;
            DateTime ida1_fechasalida = fechaSalidaIdaInputDatePickerControl1.Value != "" ? Convert.ToDateTime(fechaSalidaIdaInputDatePickerControl1.Value) : DateTime.Now;
            string ida1_origen = origenIda1.Text.ToUpper().Replace("'", "");
            string ida1_destino = destinoIda1.Text.ToUpper().Replace("'", "");
            string ida1_numvuelo_tren = numeroVueloIdaTrenTextBox1.Text;
            string ida1_horasalida = horaSalidaIdaTimeInputBox1.Value;
            string ida1_horallegada = horaLlegadaIdaTimeInputBox1.Value;

            string IdTipoBono_ida2 = "";
            DateTime? ida2_fechasalida = null;
            string ida2_origen = "";
            string ida2_destino = "";
            string ida2_numvuelo_tren = "";
            string ida2_horasalida = "";
            string ida2_horallegada = "";

            if (hdCombinacionIda.Value == "true")
            {
                IdTipoBono_ida2 = "'" + locomocionIdaDropDownList2.SelectedValue + "'";
                ida2_fechasalida = fechaSalidaIdaInputDatePickerControl2.Value != "" ? Convert.ToDateTime(fechaSalidaIdaInputDatePickerControl2.Value) : DateTime.Now;
                ida2_origen = origenIda2.Text.ToUpper().Replace("'", "");
                ida2_destino = destinoIda2.Text.ToUpper().Replace("'", "");
                ida2_numvuelo_tren = numeroVueloIdaTrenTextBox2.Text;
                ida2_horasalida = horaSalidaIdaTimeInputBox2.Value;
                ida2_horallegada = horaLlegadaIdaTimeInputBox2.Value;
            }
            else
            {
                IdTipoBono_ida2 = "null";
                ida2_fechasalida = null;
                ida2_origen = null;
                ida2_destino = null;
                ida2_numvuelo_tren = null;
                ida2_horasalida = null;
                ida2_horallegada = null;
            }
            string IdTipoBono_reg1 = locomocionRegresoDropDownList1.SelectedValue;
            DateTime reg1_fechasalida = fechaSalidaRegresoInputDatePickerControl1.Value != string.Empty ? Convert.ToDateTime(fechaSalidaRegresoInputDatePickerControl1.Value) : DateTime.Now;
            string reg1_origen = origenRegreso1.Text.ToUpper().Replace("'", "");
            string reg1_destino = destinoRegreso1.Text.ToUpper().Replace("'", "");
            string reg1_numvuelo_tren = numeroVueloTrenRegresoTextBox1.Text;
            string reg1_horasalida = horaSalidaRegresoTimeInputBox1.Value;
            string reg1_horallegada = horaLlegadaRegresoTimeInputBox1.Value;

            string IdTipoBono_reg2 = "";
            DateTime? reg2_fechasalida = null;
            string reg2_origen = "";
            string reg2_destino = "";
            string reg2_numvuelo_tren = "";
            string reg2_horasalida = "";
            string reg2_horallegada = "";

            if (hdCombinacionRegreso.Value == "true")
            {
                IdTipoBono_reg2 = "'" + locomocionRegresoDropDownList2.SelectedValue + "'";
                reg2_fechasalida = fechaSalidaRegresoInputDatePickerControl2.Value != "" ? Convert.ToDateTime(fechaSalidaRegresoInputDatePickerControl2.Value) : DateTime.Now;
                reg2_origen = origenRegreso2.Text.ToUpper().Replace("'", "");
                reg2_destino = destinoRegreso2.Text.ToUpper().Replace("'", "");
                reg2_numvuelo_tren = numeroVueloTrenRegresoTextBox2.Text;
                reg2_horasalida = horaSalidaRegresoTimeInputBox2.Value;
                reg2_horallegada = horaLlegadaRegresoTimeInputBox2.Value;
            }
            else
            {
                IdTipoBono_reg2 = "null";
                reg2_fechasalida = null;
                reg2_origen = null;
                reg2_destino = null;
                reg2_numvuelo_tren = null;
                reg2_horasalida = null;
                reg2_horallegada = null;
            }
            string observaciones_ida = this.txtObservacionIda.Text;
            string observaciones_reg = this.txtObservacionRegreso.Text;
            string observaciones = this.Test.Observaciones.Replace("'", "");
            double val;
            double importe_max = Double.TryParse(importeMaximoTextBox.Text, NumberStyles.Number, CultureInfo.GetCultureInfo("es-ES").NumberFormat, out val) ? val : 0; 


            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                string consulta = "UPDATE reservasviajes SET idestado = '" + MaquinaEstados.SinEnviar + "', LastUpd = getdate() WHERE idreserva = " + idReserva;
                SqlCommand comm = new SqlCommand(consulta, conn);
                comm.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {

                ShowMessage("Se ha producido un error al actualizar el servicio", null);
                return false;
            }
            string strida1_fechasalida = null;
            if (ida1_fechasalida != null)
            {
                strida1_fechasalida = Convert.ToString(ida1_fechasalida).Substring(6, 4) + Convert.ToString(ida1_fechasalida).Substring(3, 2) + Convert.ToString(ida1_fechasalida).Substring(0, 2);
            }
            string strida2_fechasalida = "null";
            if (ida2_fechasalida != null)
            {
                strida2_fechasalida = "'" + Convert.ToString(ida2_fechasalida).Substring(6, 4) + Convert.ToString(ida2_fechasalida).Substring(3, 2) + Convert.ToString(ida2_fechasalida).Substring(0, 2) + "'";
            }
            string strreg1_fechasalida = null;
            if (reg1_fechasalida != null)
            {
                strreg1_fechasalida = Convert.ToString(reg1_fechasalida).Substring(6, 4) + Convert.ToString(reg1_fechasalida).Substring(3, 2) + Convert.ToString(reg1_fechasalida).Substring(0, 2);
            }
            string strreg2_fechasalida = "null";
            if (reg2_fechasalida != null)
            {
                strreg2_fechasalida = "'" + Convert.ToString(reg2_fechasalida).Substring(6, 4) + Convert.ToString(reg2_fechasalida).Substring(3, 2) + Convert.ToString(reg2_fechasalida).Substring(0, 2) + "'";
            }
            SqlConnection conn2 = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn2.Open();
                string consulta2 = "UPDATE serviciosreservastransporte SET " +
                    "IdTipoBono_ida1 = '" + IdTipoBono_ida1 + "'," +
                    "ida1_fechasalida= '" + strida1_fechasalida + "'," +
                    "ida1_origen= '" + ida1_origen + "'," +
                    "ida1_destino= '" + ida1_destino + "'," +
                    "ida1_numvuelo_tren= '" + ida1_numvuelo_tren + "'," +
                    "ida1_horasalida= '" + ida1_horasalida + "'," +
                    "ida1_horallegada= '" + ida1_horallegada + "'," +
                    "IdTipoBono_ida2 = " + IdTipoBono_ida2 + "," +
                    "ida2_fechasalida= " + strida2_fechasalida + "," +
                    "ida2_origen= '" + ida2_origen + "'," +
                    "ida2_destino= '" + ida2_destino + "'," +
                    "ida2_numvuelo_tren= '" + ida2_numvuelo_tren + "'," +
                    "ida2_horasalida= '" + ida2_horasalida + "'," +
                    "ida2_horallegada= '" + ida2_horallegada + "'," +
                    //
                    "IdTipoBono_reg1 = '" + IdTipoBono_reg1 + "'," +
                    "reg1_fechasalida= '" + strreg1_fechasalida + "'," +
                    "reg1_origen= '" + reg1_origen + "'," +
                    "reg1_destino= '" + reg1_destino + "'," +
                    "reg1_numvuelo_tren= '" + reg1_numvuelo_tren + "'," +
                    "reg1_horasalida= '" + reg1_horasalida + "'," +
                    "reg1_horallegada= '" + reg1_horallegada + "'," +
                    "IdTipoBono_reg2 = " + IdTipoBono_reg2 + "," +
                    "reg2_fechasalida= " + strreg2_fechasalida + "," +
                    "reg2_origen= '" + reg2_origen + "'," +
                    "reg2_destino= '" + reg2_destino + "'," +
                    "reg2_numvuelo_tren= '" + reg2_numvuelo_tren + "'," +
                    "reg2_horasalida= '" + reg2_horasalida + "'," +
                    "reg2_horallegada= '" + reg2_horallegada + "'," +
                    //
                    "observaciones_ida= '" + observaciones_ida + "'," +
                    "observaciones_reg= '" + observaciones_reg + "'," +
                    "observaciones= '" + observaciones + "'," +
                    "importe_max= " + importe_max +
                    " WHERE idServicioTransporte = " + idReservaServicio;
                SqlCommand comm2 = new SqlCommand(consulta2, conn2);
                comm2.ExecuteNonQuery();
                this.Test.IdServicio = idReservaServicio.ToString();
                this.Test.Save();
                conn2.Close();

            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public Boolean crearServicio()
        {
            //Instancia de Agentes
            AgenteServicio oServicio = new AgenteServicio();
            AgenteUsuarios oUsuarios = new AgenteUsuarios();
            DDatosPersonalesUsuario datos = (DDatosPersonalesUsuario)Session["datosUsuario"];

            try
            {
                //Inserta Reserva reservasviajes
                DReservasViajes oReservas = new DReservasViajes();
                oReservas.Reserva = "Servicio DSP";
                oReservas.Fkidexpediente = expediente.Idexpediente;
                oReservas.Idestado = MaquinaEstados.SinEnviar;
                oReservas.Fechapeticion = DateTime.Now;
                //Xavier Morell
                //oReservas.IdPeticionario = oUsuarios.ObtenerDatosPersonalesPorLogin().IdPeticionario;
                oReservas.IdPeticionario = datos.IdPeticionario;
                oReservas.Locked = 0;
                oReservas.Observaciones = this.Test.Observaciones.Replace("'", "");

                oServicio.InsertarReserva(oReservas);


                //Inserta Servicios Transportes serviciosreservastransporte
                DVServicioTransporte oServicioTransporte = new DVServicioTransporte();
                oServicioTransporte.idserviciotransporte = 0;    //Autonúmerico
                oServicioTransporte.ida1_transporte = locomocionIdaDropDownList1.SelectedValue;
                oServicioTransporte.ida1_fechasalida = fechaSalidaIdaInputDatePickerControl1.Value != "" ? Convert.ToDateTime(fechaSalidaIdaInputDatePickerControl1.Value) : DateTime.Now;
                oServicioTransporte.ida1_origen = origenIda1.Text.ToUpper().Replace("'", "");
                oServicioTransporte.ida1_destino = destinoIda1.Text.ToUpper().Replace("'", "");
                oServicioTransporte.ida1_numvuelo_tren = numeroVueloIdaTrenTextBox1.Text;
                oServicioTransporte.ida1_horasalida = horaSalidaIdaTimeInputBox1.Value;
                oServicioTransporte.ida1_horallegada = horaLlegadaIdaTimeInputBox1.Value;

                if (hdCombinacionIda.Value == "true")
                {
                    oServicioTransporte.ida2_transporte = locomocionIdaDropDownList2.SelectedValue;
                    oServicioTransporte.ida2_fechasalida = fechaSalidaIdaInputDatePickerControl2.Value != string.Empty ? Convert.ToDateTime(fechaSalidaIdaInputDatePickerControl2.Value) : DateTime.Now;
                    oServicioTransporte.ida2_origen = origenIda2.Text.ToUpper().Replace("'", "");
                    oServicioTransporte.ida2_destino = destinoIda2.Text.ToUpper().Replace("'", "");
                    oServicioTransporte.ida2_numvuelo_tren = numeroVueloIdaTrenTextBox2.Text;
                    oServicioTransporte.ida2_horasalida = horaSalidaIdaTimeInputBox2.Value;
                    oServicioTransporte.ida2_horallegada = horaLlegadaIdaTimeInputBox2.Value;
                }
                else
                {
                    oServicioTransporte.ida2_transporte = null;
                    oServicioTransporte.ida2_fechasalida = null;
                    oServicioTransporte.ida2_origen = null;
                    oServicioTransporte.ida2_destino = null;
                    oServicioTransporte.ida2_numvuelo_tren = null;
                    oServicioTransporte.ida2_horasalida = null;
                    oServicioTransporte.ida2_horallegada = null;

                }
                oServicioTransporte.reg1_transporte = locomocionRegresoDropDownList1.SelectedValue;
                oServicioTransporte.reg1_fechasalida = fechaSalidaRegresoInputDatePickerControl1.Value != string.Empty ? Convert.ToDateTime(fechaSalidaRegresoInputDatePickerControl1.Value) : DateTime.Now;

                oServicioTransporte.reg1_origen = origenRegreso1.Text.ToUpper().Replace("'", "");
                oServicioTransporte.reg1_destino = destinoRegreso1.Text.ToUpper().Replace("'", "");
                oServicioTransporte.reg1_numvuelo_tren = numeroVueloTrenRegresoTextBox1.Text;
                oServicioTransporte.reg1_horasalida = horaSalidaRegresoTimeInputBox1.Value;
                oServicioTransporte.reg1_horallegada = horaLlegadaRegresoTimeInputBox1.Value;

                if (hdCombinacionRegreso.Value == "true")
                {
                    oServicioTransporte.reg2_transporte = locomocionRegresoDropDownList2.SelectedValue;
                    oServicioTransporte.reg2_fechasalida = fechaSalidaRegresoInputDatePickerControl2.Value != "" ? Convert.ToDateTime(fechaSalidaRegresoInputDatePickerControl2.Value) : DateTime.Now;
                    oServicioTransporte.reg2_origen = origenRegreso2.Text.ToUpper().Replace("'", "");
                    oServicioTransporte.reg2_destino = destinoRegreso2.Text.ToUpper().Replace("'", "");
                    oServicioTransporte.reg2_numvuelo_tren = numeroVueloTrenRegresoTextBox2.Text;
                    oServicioTransporte.reg2_horasalida = horaSalidaRegresoTimeInputBox2.Value;
                    oServicioTransporte.reg2_horallegada = horaLlegadaRegresoTimeInputBox2.Value;
                }
                else
                {
                    oServicioTransporte.reg2_transporte = null;
                    oServicioTransporte.reg2_fechasalida = null;
                    oServicioTransporte.reg2_origen = null;
                    oServicioTransporte.reg2_destino = null;
                    oServicioTransporte.reg2_numvuelo_tren = null;
                    oServicioTransporte.reg2_horasalida = null;
                    oServicioTransporte.reg2_horallegada = null;
                }
                float val;
                oServicioTransporte.cotizado = float.TryParse(importeMaximoTextBox.Text, NumberStyles.Number, CultureInfo.GetCultureInfo("es-ES").NumberFormat, out val) ? val : 0; 
                oServicioTransporte.observaciones_ida = this.txtObservacionIda.Text;
                oServicioTransporte.observaciones_reg = this.txtObservacionRegreso.Text;
                oServicioTransporte.observaciones = this.Test.Observaciones.Replace("'", "");


                oServicio.InsertarServicioTransporte(oServicioTransporte, idExpediente);

                //Inserta Servicios Reservas serviciosreservasviajes
                DServiciosReservas oServicioReserva = new DServiciosReservas();
                oServicioReserva.Idservicio = 0;    //Autonúmerico
                oServicioReserva.Idreserva = oReservas.Idreserva;
                oServicioReserva.IdTipoBono = hdTipoBono.Value;
                oServicioReserva.Fechapeticion = DateTime.Now;
                oServicioReserva.Resumenservicio = "resumen desplazamiento";
                oServicioReserva.Cotizado = 0;
                oServicioReserva.Idserviciotransporte = oServicioTransporte.idserviciotransporte; //Llave
                oServicioReserva.Locked = 0;

                oServicio.InsertarServicioReserva(oServicioReserva);

                idReserva = oServicioReserva.Idreserva;

                //Passengers
                this.Test.IdServicio = oServicioTransporte.idserviciotransporte.ToString(); //Llave de trasnporte
                this.Test.Save();


            }
            catch (Exception e)
            {

                return false;
            }

            return true;

        }

        public Boolean Guardar()
        {
            Boolean blnResultado;

            // Control de errores y validación
            if (!validarCampos()) return false;


            if (idReservaServicio > 0)
            {
                blnResultado = modificarServicio();
            }
            else
            {
                blnResultado = crearServicio();
            }


            return blnResultado;
        }

        private void SetState(string state)
        {
            // Actualizar el estado del servicio de alojamiento
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                string consulta = "UPDATE reservasviajes SET idestado = '" + state + "', LastUpd = getdate() WHERE idreserva = " + idReserva.ToString() + ";";
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
                    if (idReservaServicio == 0 && idReserva == 0)
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
                    if (idReserva == 0 || idReserva == null)
                        this.btnGuardar.Visible = true;
                    break;
            }


        }

        private void showControlesAlternative()
        {
            // Muestra alternativas

            this.Alternativas.Visible = showAlternative;


            this.fechaSalidaIdaInputDatePickerControl1.Enabled = !showAlternative;
            this.fechaSalidaIdaInputDatePickerControl1.ClickImageButton.Enabled = !showAlternative;
            this.fechaSalidaIdaInputDatePickerControl1.InputTextBox.Enabled = !showAlternative;
            this.locomocionIdaDropDownList1.Enabled = !showAlternative;
            this.numeroVueloIdaTrenTextBox1.Enabled = !showAlternative;
            this.origenIda1.Enabled = !showAlternative;
            this.destinoIda1.Enabled = !showAlternative;
            this.horaSalidaIdaTimeInputBox1.Enabled = !showAlternative;
            this.horaSalidaIdaTimeInputBox1.InputTextBox.Enabled = !showAlternative;
            this.horaLlegadaIdaTimeInputBox1.Enabled = !showAlternative;
            this.horaLlegadaIdaTimeInputBox1.InputTextBox.Enabled = !showAlternative;
            this.importeMaximoTextBox.Enabled = !showAlternative;

            this.fechaSalidaIdaInputDatePickerControl2.Enabled = !showAlternative;
            this.fechaSalidaIdaInputDatePickerControl2.ClickImageButton.Enabled = !showAlternative;
            this.fechaSalidaIdaInputDatePickerControl2.InputTextBox.Enabled = !showAlternative;
            this.locomocionIdaDropDownList2.Enabled = !showAlternative;
            this.numeroVueloIdaTrenTextBox2.Enabled = !showAlternative;
            this.origenIda2.Enabled = !showAlternative;
            this.destinoIda2.Enabled = !showAlternative;
            this.horaSalidaIdaTimeInputBox2.Enabled = !showAlternative;
            this.horaSalidaIdaTimeInputBox2.InputTextBox.Enabled = !showAlternative;
            this.horaLlegadaIdaTimeInputBox2.Enabled = !showAlternative;
            this.horaLlegadaIdaTimeInputBox2.InputTextBox.Enabled = !showAlternative;
            this.chbAltIDA.Enabled = !showAlternative;


            this.fechaSalidaRegresoInputDatePickerControl1.Enabled = !showAlternative;
            this.fechaSalidaRegresoInputDatePickerControl1.ClickImageButton.Enabled = !showAlternative;
            this.fechaSalidaRegresoInputDatePickerControl1.InputTextBox.Enabled = !showAlternative;
            this.locomocionRegresoDropDownList1.Enabled = !showAlternative;
            this.numeroVueloTrenRegresoTextBox1.Enabled = !showAlternative;
            this.origenRegreso1.Enabled = !showAlternative;
            this.destinoRegreso1.Enabled = !showAlternative;
            this.horaSalidaRegresoTimeInputBox1.Enabled = !showAlternative;
            this.horaSalidaRegresoTimeInputBox1.InputTextBox.Enabled = !showAlternative;
            this.horaLlegadaRegresoTimeInputBox1.Enabled = !showAlternative;
            this.horaLlegadaRegresoTimeInputBox1.InputTextBox.Enabled = !showAlternative;


            this.fechaSalidaRegresoInputDatePickerControl2.Enabled = !showAlternative;
            this.fechaSalidaRegresoInputDatePickerControl2.ClickImageButton.Enabled = !showAlternative;
            this.fechaSalidaRegresoInputDatePickerControl2.InputTextBox.Enabled = !showAlternative;
            this.locomocionRegresoDropDownList2.Enabled = !showAlternative;
            this.numeroVueloTrenRegresoTextBox2.Enabled = !showAlternative;
            this.origenRegreso2.Enabled = !showAlternative;
            this.destinoRegreso2.Enabled = !showAlternative;
            this.horaSalidaRegresoTimeInputBox2.Enabled = !showAlternative;
            this.horaSalidaRegresoTimeInputBox2.InputTextBox.Enabled = !showAlternative;
            this.horaLlegadaRegresoTimeInputBox2.Enabled = !showAlternative;
            this.horaLlegadaRegresoTimeInputBox2.InputTextBox.Enabled = !showAlternative;
            this.chbAltRegreso.Enabled = !showAlternative;

            this.Test.Enabled = !showAlternative;

            if (hdCombinacionIda.Value == "false" && !showAlternative)
            {
                this.fechaSalidaIdaInputDatePickerControl2.Enabled = showAlternative;
                this.fechaSalidaIdaInputDatePickerControl2.ClickImageButton.Enabled = !showAlternative;
                this.fechaSalidaIdaInputDatePickerControl2.InputTextBox.Enabled = !showAlternative;
                this.locomocionIdaDropDownList2.Enabled = showAlternative;
                this.numeroVueloIdaTrenTextBox2.Enabled = showAlternative;
                this.origenIda2.Enabled = showAlternative;
                this.destinoIda2.Enabled = showAlternative;
                this.horaSalidaIdaTimeInputBox2.Enabled = showAlternative;
                this.horaSalidaIdaTimeInputBox2.InputTextBox.Enabled = !showAlternative;
                this.horaLlegadaIdaTimeInputBox2.Enabled = showAlternative;
                this.horaLlegadaIdaTimeInputBox2.InputTextBox.Enabled = !showAlternative;
            }
            if (hdCombinacionRegreso.Value == "false" && !showAlternative)
            {
                this.fechaSalidaRegresoInputDatePickerControl2.Enabled = showAlternative;
                this.fechaSalidaRegresoInputDatePickerControl2.ClickImageButton.Enabled = !showAlternative;
                this.fechaSalidaRegresoInputDatePickerControl2.InputTextBox.Enabled = !showAlternative;
                this.locomocionRegresoDropDownList2.Enabled = showAlternative;
                this.numeroVueloTrenRegresoTextBox2.Enabled = showAlternative;
                this.origenRegreso2.Enabled = showAlternative;
                this.destinoRegreso2.Enabled = showAlternative;
                this.horaSalidaRegresoTimeInputBox2.Enabled = showAlternative;
                this.horaSalidaRegresoTimeInputBox2.InputTextBox.Enabled = !showAlternative;
                this.horaLlegadaRegresoTimeInputBox2.Enabled = showAlternative;
                this.horaLlegadaRegresoTimeInputBox2.InputTextBox.Enabled = !showAlternative;
            }
        }

        private string GetServicioByReserva()
        {
            // Recupera el id de servicio a partir del id de reserva
            string id = "";
            string consulta = "SELECT * FROM serviciosreservasviajes WHERE idreserva = " + idReserva;
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

        private int GetReservaByServicio()
        {
            // Recupera el id de servicio a partir del id de reserva
            int id = 0;
            string consulta = "SELECT SV.idreserva, idestado FROM serviciosreservasviajes SV LEFT JOIN reservasviajes RV ON SV.idreserva = RV.idreserva WHERE idserviciotransporte = " + idReservaServicio;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();
                if (rea.Read())
                {
                    id = int.Parse(rea["idreserva"].ToString());
                    //Jose Laguna 06-04-2010 Esto lo añadimos para que no pierda el estado cuando modificas la alternativa
                    this.state = rea["idEstado"] != DBNull.Value ? rea["idEstado"].ToString() : "";
                    this.statename = GetStateName();
                }
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

        private void Inicializa()
        {
            int nIdPoblacion = congreso.IdPoblacion;

            AgenteMaestros agente = new AgenteMaestros();
            DVPoblacion dpSelect = agente.ObtenerPoblacionesTodasActivasInactivas(null, expediente.idconfempresa ?? 1).FirstOrDefault(f => f.IdPoblacion == nIdPoblacion);

            if (dpSelect != null)
            {
                destinoIda1.Text = dpSelect.Poblacion;
                origenRegreso1.Text = dpSelect.Poblacion;
            }
            fechaSalidaIdaInputDatePickerControl1.Value = congreso.Desde.Value.ToShortDateString();
            fechaSalidaIdaInputDatePickerControl2.Value = congreso.Desde.Value.ToShortDateString();
            fechaSalidaRegresoInputDatePickerControl1.Value = congreso.Hasta.Value.ToShortDateString();
            fechaSalidaRegresoInputDatePickerControl2.Value = congreso.Hasta.Value.ToShortDateString();
        }

        private void GetServicioTransporte()
        {
            AgenteExpedientes agenteExp = new AgenteExpedientes();
            DVServicioTransporte oTransporte = agenteExp.ObtenerServiciosTransportes(expediente.Idexpediente).FirstOrDefault(f => (f.idreserva == Convert.ToInt32(idReserva)));
            if (oTransporte != null)
            {
                //Cargar Datos
                state = oTransporte.IdEstado;
                statename = GetStateName();

                if (oTransporte.ida1_fechasalida != null) fechaSalidaIdaInputDatePickerControl1.Value = oTransporte.ida1_fechasalida.Value.ToShortDateString();
                locomocionIdaDropDownList1.SelectedValue = oTransporte.ida1_transporte;
                numeroVueloIdaTrenTextBox1.Text = oTransporte.ida1_numvuelo_tren;
                origenIda1.Text = oTransporte.ida1_origen;
                destinoIda1.Text = oTransporte.ida1_destino;
                horaSalidaIdaTimeInputBox1.Value = oTransporte.ida1_horasalida;
                horaLlegadaIdaTimeInputBox1.Value = oTransporte.ida1_horallegada;

                if (oTransporte.ida2_fechasalida != null)
                {
                    fechaSalidaIdaInputDatePickerControl2.Value = oTransporte.ida2_fechasalida.Value.ToShortDateString();
                    hdCombinacionIda.Value = "true";
                    this.chbAltIDA.Checked = true;
                }
                locomocionIdaDropDownList2.SelectedValue = oTransporte.ida2_transporte;
                numeroVueloIdaTrenTextBox2.Text = oTransporte.ida2_numvuelo_tren;
                origenIda2.Text = oTransporte.ida2_origen;
                destinoIda2.Text = oTransporte.ida2_destino;
                horaSalidaIdaTimeInputBox2.Value = oTransporte.ida2_horasalida;
                horaLlegadaIdaTimeInputBox2.Value = oTransporte.ida2_horallegada;


                if (oTransporte.reg1_fechasalida != null)
                    fechaSalidaRegresoInputDatePickerControl1.Value = oTransporte.reg1_fechasalida.Value.ToShortDateString();
                locomocionRegresoDropDownList1.SelectedValue = oTransporte.reg1_transporte;
                numeroVueloTrenRegresoTextBox1.Text = oTransporte.reg1_numvuelo_tren;
                origenRegreso1.Text = oTransporte.reg1_origen;
                destinoRegreso1.Text = oTransporte.reg1_destino;
                horaSalidaRegresoTimeInputBox1.Value = oTransporte.reg1_horasalida;
                horaLlegadaRegresoTimeInputBox1.Value = oTransporte.reg1_horallegada;


                if (oTransporte.reg2_fechasalida != null)
                {
                    fechaSalidaRegresoInputDatePickerControl2.Value = oTransporte.reg2_fechasalida.Value.ToShortDateString();
                    hdCombinacionRegreso.Value = "true";
                    this.chbAltRegreso.Checked = true;

                }
                locomocionRegresoDropDownList2.SelectedValue = oTransporte.reg2_transporte;
                numeroVueloTrenRegresoTextBox2.Text = oTransporte.reg2_numvuelo_tren;
                origenRegreso2.Text = oTransporte.reg2_origen;
                destinoRegreso2.Text = oTransporte.reg2_destino;
                horaSalidaRegresoTimeInputBox2.Value = oTransporte.reg2_horasalida;
                horaLlegadaRegresoTimeInputBox2.Value = oTransporte.reg2_horallegada;

                if (oTransporte.importe_max != null) importeMaximoTextBox.Text = oTransporte.importe_max.ToString();
                this.txtObservacionIda.Text = oTransporte.observaciones_ida;
                this.txtObservacionRegreso.Text = oTransporte.observaciones_reg;
                this.Test.Observaciones = oTransporte.observaciones;
            }
        }

        private void ShowMessage(string message, string navigation)
        {
            // Muestra mensaje y navega a una URL especificada
            navigation = (string.IsNullOrEmpty(navigation) ? "closeLoading()" : "document.location='" + navigation + "'");
            string script = String.Format("alert('{0}');{1};", message, navigation);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "dialog", script, true);
        }


        #endregion


    }
}