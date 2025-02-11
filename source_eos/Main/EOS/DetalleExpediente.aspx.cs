using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Logica;
using EOS.Web;
using EOS.Entidades.Datos;
using System.Web.UI.HtmlControls;
using EOS.Web.Enums;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace EOS
{
    public partial class DetalleExpediente : Page
    {
        bool bCreando = false;
        public DCabeceraExpedienteAmpliado entExp = new DCabeceraExpedienteAmpliado();
        public string DocTypes { get; set; }
        public int IdPeticionario { get; set; }
        public string DocumentNameTypeSubType { get; set; }
        public bool IsExpIndividual { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            ///////////////Controlar Si caduca la sesión en la aplicación y redireccionar al Login//////////////
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Session["rolesuser"] == null) Response.Redirect("~/Account/Login.aspx");
            //////////////////////////////////////////////////////////////////////////


            if (!Page.IsPostBack)
            {
                if (Page.Request.UrlReferrer != null)
                {
                    if (Page.Request.UrlReferrer.AbsoluteUri.Contains("NuevoExpedientePasoB.aspx"))
                    {
                        // Está terminando el proceso de creación
                        bCreando = true;
                        ViewState.Add("Creando", bCreando);
                    }
                }

                GetTipoDeServicio();

                string sIdExp = string.Empty;
                if (Session["idExpedienteSeleccionado"] != null && Session["idExpedienteSeleccionado"].ToString() != Request.QueryString["idexp"])
                {
                    Session.Remove("idExpedienteSeleccionado");

                    if (!string.IsNullOrWhiteSpace(Request.QueryString["idexp"]))
                    {
                        sIdExp = Request.QueryString["idexp"];
                        Session["idExpedienteSeleccionado"] = sIdExp;
                        Response.Redirect("DetalleExpediente.aspx?idexp=" + Request.QueryString["idexp"], false);
                    }
                    else
                    {
                        Response.Redirect("Expedientes.aspx", false);
                    }
                }
                else
                {
                    sIdExp = Request.QueryString["idexp"];
                    Session["idExpedienteSeleccionado"] = sIdExp;
                }

                Session.Remove("CurExp");

                if (!string.IsNullOrEmpty(sIdExp))
                {
                    // Aseguramos que el usuario tiene derechos ver estos expedientes
                    //Xavier Morell
                    //AgenteUsuarios agenteUsu = new AgenteUsuarios();
                    //DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
                    DVPeticionariosRoles datosRoles = (DVPeticionariosRoles) Session["rolesuser"];
                    IdPeticionario = datosRoles.IdPeticionario;
                    // Buscamos los datos básicos del expediente que se pasa por la url
                    AgenteExpedientes agenteExp = new Web.AgenteExpedientes();
                    entExp = agenteExp.ObtenerExpedientePorID(sIdExp, datosRoles);

                    plhParticipantesNoCalculadora.Visible = false;
                    plhParticipantesCalculadora.Visible = false;

                    if (entExp.TipoPagoFee == 3)
                    {
                        plhParticipantesCalculadora.Visible = true;
                        botoneraAcciones.Visible = false;
                        lstServicios.Visible = false;
                        lvServicioInscripcion.Visible = false;
                        lvServicioAlojamiento.Visible = false;
                        lvServicioTransporte.Visible = false;
                        lvServicioActividades.Visible = false;
                        divInscripciones.Visible = false;
                        divAlojamiento.Visible = false;
                        divTransporte.Visible = false;
                        divOtrosServicios.Visible = false;
                        divAnadirServicio.Visible = false;

                        var dExpediente = new Entidades.DetalleExpediente();

                        AgenteExpedientes agExp = new AgenteExpedientes();
                        DCabeceraExpedienteAmpliado dCabExp = agExp.ObtenerExpedientePorID(entExp.Idexpediente.ToString());

                        if (dCabExp != null)
                        {
                            dExpediente.nIDExpediente = dCabExp.Idexpediente;
                            dExpediente.nIDAMEC = dCabExp.Idamec.ToString();
                            dExpediente.AMEC = agExp.ObtenerEntidadAMECporID(Convert.ToString(dCabExp.Idamec));
                            dExpediente.nIDCogreso = (dCabExp.Idactividad.HasValue) ? dCabExp.Idactividad.Value : 1776;
                            dExpediente.sCongreso = dCabExp.Actividad;
                            dExpediente.esCongreso = dExpediente.AMEC.IdCongreso.HasValue ? 1 : 0;
                            dExpediente.sPedido = dCabExp.Pedido;
                            dExpediente.tipoPagoFee = dCabExp.TipoPagoFee;
                            dExpediente.bUrgente = (dCabExp.Urgente.HasValue) && Convert.ToBoolean(dCabExp.Urgente.Value);
                            dExpediente.dParticipantes = null;
                            dExpediente.nIDTipo = dCabExp.Idtiporeserva;

                            Session.Add("NuevoExpediente", dExpediente);
                        }
                    }
                    else {
                        plhParticipantesNoCalculadora.Visible = true;
                    }

                    if (entExp.Idtiporeserva == 2) {
                        divInscripciones.Visible = false;
                        divAlojamiento.Visible = false;
                        divTransporte.Visible = false;
                    }

                    var empleadoGP = agenteExp.ObtenerEmpleadoGpPorPetAmecExp(datosRoles.IdPeticionario, entExp.Amec, entExp.Idexpediente);
                    if (empleadoGP != null)
                    {
                        datosRoles.idempleadogp = empleadoGP.IdEmpleadoGp;
                    }


                    if (entExp == null)
                    {
                        // si no hay un expediente eliminamos los servicios
                        lstServicios.Items.Clear();
                        lvServicioInscripcion.DataSource = string.Empty;
                        lvServicioInscripcion.DataSourceID = null;

                        entExp = new DCabeceraExpedienteAmpliado();
                        entExp.Tiporeserva = "-1";

                        Alert.Show("Esta tratando de acceder a un expediente no válido", "Expedientes.aspx");
                    }
                    else
                    {
                        //Obtener Peticionario expediente
                        //Pau ferrer 23-06-2011
                        lblPeticionario.Text = entExp.Peticionario;


                        //Fin Pau

                        //AVA: Se ha modificado para que el botón modificar se vea solo cuando el Expediente está en borrador o En curso
                        if (entExp.Idestado == "AB" || entExp.Idestado == "NC")
                            this.lnkModificar.HRef = string.Format("NuevoExpedientePasoA.aspx?idexp={0}&tipo={1}", entExp.Idexpediente, entExp.Idtiporeserva == 1 ? entExp.TipoPagoFee == 3 ? "3" : "1" : "2");
                        else this.lnkModificar.Visible = false;
                        //añadido datos  Marco 21/02/2011--mostrar mas datos 
                        this.lblCongreso.Text = entExp.Actividad;//+" " + string.Format("{0:dd/MM/yyyy HH:mm:ss}", entExp.FechaDesde) + " " + string.Format("{0:dd/MM/yyyy HH:mm:ss}", entExp.FechaHasta);
                        this.lblFechaDesde.Text = string.Format("{0:dd/MM/yyyy }", entExp.FechaDesde);
                        this.LblFechaHasta.Text = string.Format("{0:dd/MM/yyyy }", entExp.FechaHasta);
                        this.LblPoblacion.Text = entExp.Poblacion;
                        //fin de añadido

                        this.lblFecha.Text = string.Format("{0:dd/MM/yyyy HH:mm:ss}", entExp.Fechacreacion);
                        this.lblPedido.Text = entExp.Pedido;
                        this.lblExpediente.Text = entExp.Idexpediente.ToString();
                        this.lblAMEC.Text = entExp.Amec;
                        this.lblTipoPago.Text = entExp.TipoPagoFee.HasValue ? entExp.TipoPagoFee.Value == 1 ? "Orden de compra" : entExp.TipoPagoFee.Value == 2 ? "Tarjeta de crédito" : entExp.TipoPagoFee.Value == 3 ? "Pago Ponente (Calculadoras)" : "" : "";
                        DAmec damec = agenteExp.ObtenerEntidadAMECporID(entExp.Idamec.ToString());
                        AgenteMaestros am = new AgenteMaestros();
                        if (damec.idconfempresa != null)
                        {
                            var empresaConf = am.ObtenerEmpresaConf(damec.idconfempresa.Value);
                            lblAgencia.Text = empresaConf.nombreagencia;                            
                        }
                        if (damec.newco != null) {
                            lblCompany.Text = damec.newco.Value ? "ORGANON" : "MSD";
                        }
                        this.eosAmecBusca.NavigateUrl = string.Format("DetalleAMEC.aspx?idamec={0}", entExp.Idamec);
                        this.eosAmecBusca.ToolTip = string.Format("Ver detalles del AMEC: {0}", entExp.Amec);
                        //this.lblFechas.Text = string.Format("{0:dd/MM/yyyy}-{1:dd/MM/yyyy}", );+96
                        //Ismael Ameller Vidal 23-03-2011 Mostramos los productos del expediente
                        this.lblProductos.Text = ProductosExpediente(sIdExp);
                        //FIN Ismael Ameller Vidal 23-03-2011 Mostramos los productos del expediente
                        CalculaEstado();
                        //Gestor
                        IsExpIndividual = true;
                        CargarCombosDocumentos();
                        CargarPassengers();
                        CargarRelacionVersionTipo();
                        //Gestor
                        int i = -1;
                        foreach (ListItem item in lstServicios.Items)
                        {
                            if (i > -1)
                            {
                                // Saltamos el primero porque es la carátula de selección
                                item.Value = string.Format("Inscripcion.aspx?idexp={0}&tab={1}", entExp.Idexpediente, i);
                            }
                            i++;
                        }
                        // si es un expediente colectivo elimina los 3 primeros servicios
                        if (entExp.Idtiporeserva == 2)
                        {
                            lstServicios.Items.RemoveAt(1);
                            lstServicios.Items.RemoveAt(1);
                            lstServicios.Items.RemoveAt(1);
                            lstServicios.Visible = false;
                            cboTipo.Visible = true;
                            IsExpIndividual = false;
                        }
                        else {
                            lstServicios.Visible = true;
                            cboTipo.Visible = false;
                        }
                        Session.Add("CurExp", entExp);
                    }
                }
            }

            if (Session["CurExp"] != null)
            {
                entExp = (DCabeceraExpedienteAmpliado)Session["CurExp"];
            }
            else
            {
                //Alert.Show("Esta tratando de acceder a un expediente no válido", "Expedientes.aspx");
                //Response.Redirect("Expedientes.aspx");
            }

            if (ViewState["Creando"] != null)
                bCreando = (bool)ViewState["Creando"];

            // Añadir clase eura al boton enviar en caso de que la petición sea para colectivos para mostrar mensaje 
            // (ticket Redmine Cambio #1000 NUEVO REQUISITO FCPA)
            // Cuando el botón tiene esta clase muestra la ventana de notificación que debe ser aceptada antes de enviar.
            btnEnviar.CssClass = (entExp != null && entExp.Tiporeserva == "COL") ? "eura" : string.Empty;
        }

        protected void CargarPassengers()
        {
            AgenteExpedientes ae = new AgenteExpedientes();
            List<DPassenger> list = new List<DPassenger>() {new DPassenger() {Id = -1,NombreCompleto = "COMUNES"} };
            list.AddRange(ae.ObtenerPassengers(entExp.Idexpediente));
            ddlAsistente.DataSource = list;
            ddlAsistente.DataValueField = "Id";
            ddlAsistente.DataTextField = "NombreCompleto";
            ddlAsistente.DataBind();
        }

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

        //JMM
        protected void odsParticipantesExpediente_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (entExp != null)
            {
                e.InputParameters["filtroIDExpediente"] = entExp.Idexpediente.ToString();                
            }
        }

        protected void lvServicioParticipantes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvServicioParticipantes.SelectedValue != null)
            {
            }
        }

        protected void cboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            HttpContext.Current.Session.Add("nuevoServicio", true);
            Response.Redirect(string.Format("/Inscripcion.aspx?idexp={0}&tab=3&tipo={1}", Request.QueryString["idexp"], HttpUtility.UrlEncode(cboTipo.SelectedValue)));
        }

        protected void CargarRelacionVersionTipo()
        {
            AgenteExpedientes ae = new AgenteExpedientes();
            List<DDocumentNameTypeSubType> list = ae.ObtenerRelacionTipoVersion(entExp.Idexpediente);
            if (list.Count > 0)
            {
                var jsonSerialiser = new JavaScriptSerializer();
                DocumentNameTypeSubType = jsonSerialiser.Serialize(list);
            }
            else
            {
                var jsonSerialiser = new JavaScriptSerializer();
                DocumentNameTypeSubType = jsonSerialiser.Serialize(new List<DDocumentNameTypeSubType>());
            }
        }
        protected void CargarCombosDocumentos()
        {
            
            AgenteMaestros am = new AgenteMaestros();
            List <DDocType> list = new List<DDocType>();
            if (entExp.Idtiporeserva == 2)
            {
                list.AddRange(am.GetDocType().Where(x => x.Inactivo == 0 && x.IdTipo == GestorTipoSeccion.ExpedienteColectivo.GetHashCode()).ToList());
                foreach (var dDocType in list)
                {
                    dDocType.SubTypes = new List<DDocSubType>();
                }
            }
            else
            {
                list.AddRange(am.GetDocType().Where(x => x.Inactivo == 0 && x.IdTipo == GestorTipoSeccion.ExpedienteIndividual.GetHashCode()).ToList());

                List<DDocSubType> subtipos = am.GetDocSubType().Where(x => x.Inactivo == 0).ToList();
                foreach (var dDocType in list)
                {
                    dDocType.SubTypes = new List<DDocSubType>();
                    dDocType.SubTypes.AddRange(subtipos.Where(x => x.IdTipodoc == dDocType.Id));
                }
            }
            var jsonSerialiser = new JavaScriptSerializer();
            DocTypes = jsonSerialiser.Serialize(list);
        }

        protected void CalculaEstado()
        {
            AgenteExpedientes agExp = new AgenteExpedientes();
            lblEstado.CssClass = string.Format("eosImagenEstado eosImagenLeyenda{0}", entExp.Idestado);            

            this.lstServicios.Enabled = true;
            if (entExp.Idestado == "AB")
            {
                string sYaAprobado = agExp.MsgYaAprobado(entExp.Idexpediente);
                //Ismael Ameller 08-03-2011 Sacamos los Labels de la BBDD
                this.lblEstado.Text = agExp.ObtenerLabelEstado("AB");
                eosFilterHeaderEstadoDetalle.Attributes["class"] = agExp.ObtenerLabelEstado("AB").ToLower().Replace(" ","");

                this.btnAprobar.OnClientClick = sYaAprobado;
                //this.btnCancelar.OnClientClick = "";
                this.btnEnviar.OnClientClick = (bCreando) ? "javascript:return confirm('¿Desea finalizar y enviar el expediente para ser procesado?')" : "javascript:return confirm('Este proceso cursa los servicios que estén sin enviar, ¿desea continuar?')";

                //this.lnkModificar.Attributes.Add("onclick", "");
                //this.lnkModificar.HRef = "javascript:;";
            }
            else if (entExp.Idestado == "NC")
            {
                string sYaEnviado = agExp.MsgYaEnviado(entExp.Idexpediente);
                string sYaAprobado = agExp.MsgYaAprobado(entExp.Idexpediente);
                //Ismael Ameller 08-03-2011 Sacamos los Labels de la BBDD
                this.lblEstado.Text = agExp.ObtenerLabelEstado("NC");
                eosFilterHeaderEstadoDetalle.Attributes["class"] = agExp.ObtenerLabelEstado("NC").ToLower().Replace(" ", "");

                if (!agExp.PuedeAprobar(entExp.Idexpediente))
                    this.btnAprobar.OnClientClick = sYaAprobado;
                //this.btnCancelar.OnClientClick = "";
                //Ismael Ameller 22/02/2011 Deja enviar aunque el estado del expediente sea en curso
                //this.btnEnviar.OnClientClick = sYaEnviado;
                //FIN Ismael Ameller 22/02/2011 Deja enviar aunque el estado del expediente sea en curso

                //this.lnkModificar.Attributes.Add("onclick", "");
                //this.lnkModificar.HRef = "javascript:;";
            }

            //else if (entExp.Idestado == "FZ")

            // Pau Ferrer  09-06-2011 #Añado estado TR como finalizado
            else if (entExp.Idestado == "FZ" || entExp.Idestado == "TR")
            {
                this.lstServicios.Enabled = false;

                string sYaCerrado = agExp.MsgYaCerrado(entExp.Idexpediente);
                //Ismael Ameller 08-03-2011 Sacamos los Labels de la BBDD
                this.lblEstado.Text = agExp.ObtenerLabelEstado("FZ");
                eosFilterHeaderEstadoDetalle.Attributes["class"] = agExp.ObtenerLabelEstado("FZ").ToLower().Replace(" ", "");

                this.btnAprobar.OnClientClick = sYaCerrado;
                //this.btnCancelar.OnClientClick = sYaCerrado;
                this.btnEnviar.OnClientClick = sYaCerrado;

                this.lnkModificar.Attributes.Add("onclick", sYaCerrado);
                this.lnkModificar.HRef = "javascript:;";
            }
            else
            {
                this.lstServicios.Enabled = false;

                string sYaCancelado = agExp.MsgYaCancelado(entExp.Idexpediente);
                //Ismael Ameller 08-03-2011 Sacamos los Labels de la BBDD
                this.lblEstado.Text = agExp.ObtenerLabelEstado("CN");
                eosFilterHeaderEstadoDetalle.Attributes["class"] = agExp.ObtenerLabelEstado("CN").ToLower().Replace(" ", "");

                this.btnAprobar.OnClientClick = sYaCancelado;
                this.btnEnviar.OnClientClick = sYaCancelado;
                this.btnCancelar.OnClientClick = sYaCancelado;

                this.lnkModificar.Attributes.Add("onclick", sYaCancelado);
                this.lnkModificar.HRef = "javascript:;";
            }
        }

        protected void CalculaEstadoIconos(ListViewItem item, String IdEstado)
        {
            ImageButton imgServicioModificar = (ImageButton)item.FindControl("imgServicioMod");
            ImageButton imgServicioAprobar = (ImageButton)item.FindControl("imgServicioAprobar");
            ImageButton imgServicioCancelar = (ImageButton)item.FindControl("imgServicioCancelar");
            Image imgServicioAprobado = (Image)item.FindControl("imgServicioAprobado");
            Image imgServicioCancelado = (Image)item.FindControl("imgServicioCancelado");

            imgServicioCancelar.OnClientClick = String.Format("return ConfirmaModificacionC('{0}');", IdEstado);

            AgenteExpedientes agExp = new AgenteExpedientes();

            if (IdEstado == "AB") // Sin Enviar
            {
                //imgServicioModificar.OnClientClick = agExp.MsgSeModificado();
                imgServicioAprobar.OnClientClick = agExp.MsgSeAprobado();
                //imgServicioCancelar.OnClientClick = agExp.MsgSeCancelado();
            }
            else if (IdEstado == "CR") // Enviado
            {
                //imgServicioModificar.OnClientClick = agExp.MsgSeModificado();
                imgServicioAprobar.OnClientClick = agExp.MsgSeAprobado();
                //imgServicioCancelar.OnClientClick = agExp.MsgSeCancelado();
            }
            else if (IdEstado == "CTZD") // Cotizando
            {
                imgServicioModificar.OnClientClick = agExp.MsgSeModificado();
                imgServicioAprobar.OnClientClick = agExp.MsgSeAprobado();
                //imgServicioCancelar.OnClientClick = agExp.MsgSeCancelado();
            }
            else if (IdEstado == "CTZ") // Cotizado
            {
                imgServicioModificar.OnClientClick = agExp.MsgSeModificado();
                //imgServicioAprobar.OnClientClick = agExp.MsgSeAprobado();
                //imgServicioCancelar.OnClientClick = agExp.MsgSeCancelado();
            }
            else if (IdEstado == "CFP") // Aceptado
            {
                imgServicioModificar.OnClientClick = agExp.MsgSeModificado();
                imgServicioAprobar.OnClientClick = agExp.MsgSeAprobado();
                //imgServicioCancelar.OnClientClick = agExp.MsgSeCancelado();
                imgServicioAprobar.Visible = false;
                imgServicioAprobado.Visible = true;
            }
            else if (IdEstado == "PTR") // Tramitando
            {
                imgServicioModificar.OnClientClick = agExp.MsgSeModificado();
                imgServicioAprobar.OnClientClick = agExp.MsgSeAprobado();
                imgServicioAprobar.Visible = false;
                imgServicioAprobado.Visible = true;
            }
            else if (IdEstado == "TR") // Tramitado
            {
                imgServicioModificar.OnClientClick = agExp.MsgSeModificado();
                imgServicioAprobar.OnClientClick = agExp.MsgSeAprobado();
                //imgServicioCancelar.OnClientClick = agExp.MsgSeCancelado();
                imgServicioAprobar.Visible = false;
                imgServicioAprobado.Visible = true;
            }
            else if (IdEstado == "CN" || IdEstado == "AN" || IdEstado == "CNTR") // Cancelado o Rechazado o "Cancelado con Gastos"
            {
                imgServicioModificar.OnClientClick = agExp.MsgSeModificado();
                imgServicioAprobar.OnClientClick = agExp.MsgSeAprobado();
                imgServicioCancelar.OnClientClick = agExp.MsgSeCancelado();
                imgServicioCancelar.Visible = false;
                imgServicioCancelado.Visible = true;
            }
        }

        #region "Botones de Modificación"

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            bool IsPonentes = false;
            IList<ReservasInfo> ListaReservasInfo = null;
            AgenteExpedientes agExp = null;
            AgenteAmecInfo agInfo = new AgenteAmecInfo();

            if (entExp.Idexpediente > 0)
            {
                string sNavegacion = "";
                ListaReservasInfo = new List<ReservasInfo>();
                agExp = new AgenteExpedientes();
                IEnumerable<DVServicioHotel> servicioHotels = agExp.ObtenerServiciosHoteles(entExp.Idexpediente);
                IEnumerable<DVServicioActividades> otroServicio = agExp.ObtenerOtrosServicios(entExp.Idexpediente);
                IEnumerable<DVServicioInscripciones> servicioInscripciones =
                    agExp.ObtenerServiciosInscripciones(entExp.Idexpediente);
                IEnumerable<DVServicioTransporte> servicioTransportes =
                    agExp.ObtenerServiciosTransportes(entExp.Idexpediente);

                if (!servicioHotels.Any() && !otroServicio.Any() && !servicioInscripciones.Any() && !servicioTransportes.Any())
                {
                    LogicaParticipantes log = new LogicaParticipantes();
                    if (log.ParticipantesPonentes(entExp.Idexpediente))
                    {
                        IsPonentes = true;
                    }
                }

                ///////////////////INFORME FCPA////////////////////
                if (entExp.Amec != null && entExp.Tiporeserva == "COL")
                {
                    DAmecInfo amec = new DAmecInfo();

                    amec = agInfo.CargarTodosValoresAmec(entExp.Amec);
                
                    
                    foreach (var dvservicioHotels in servicioHotels)
                    {
                        if (amec != null)
                        {
                            agInfo.guardarEnInformeFCPA(amec.idamecs, amec.idtipoactividad.ToString(), amec.descripcion,
                                amec.fechacomienzo, amec.fechafinalizacion, amec.idestado, 2, "", "", "", "Hoteles",
                                entExp.Idexpediente.ToString());
                        }
                        else
                        {
                            agInfo.guardarEnInformeFCPA(entExp.Amec, "", "", null, null, 1, 2, "", "", "", "Hoteles",
                                entExp.Idexpediente.ToString());
                        }
                    }
                    foreach (var dvServicioTransporte in servicioTransportes)
                    {
                        if (amec != null)
                        {
                            agInfo.guardarEnInformeFCPA(amec.idamecs, amec.idtipoactividad.ToString(), amec.descripcion,
                                amec.fechacomienzo, amec.fechafinalizacion, amec.idestado, 2, "", "", "",
                                "Transporte",
                                entExp.Idexpediente.ToString());
                        }
                        else
                        {
                            agInfo.guardarEnInformeFCPA(entExp.Amec, "", "", null, null, 1, 2, "", "", "", "Transporte", entExp.Idexpediente.ToString());
                        }
                    }
                    foreach (var dvServicioInscripciones in servicioInscripciones)
                    {
                        if (amec != null)
                        {
                            agInfo.guardarEnInformeFCPA(amec.idamecs, amec.idtipoactividad.ToString(), amec.descripcion,
                                amec.fechacomienzo, amec.fechafinalizacion, amec.idestado, 2, "", "", "",
                                "Inscripciones",
                                entExp.Idexpediente.ToString());
                        }
                        else
                        {

                            agInfo.guardarEnInformeFCPA(entExp.Amec, "", "", null, null, 1, 2, "", "", "",
                                "Inscripciones",
                                entExp.Idexpediente.ToString());
                        }
                    }
                    foreach (var dvOtroServicio in otroServicio)
                    {
                        if (amec != null)
                        {
                            agInfo.guardarEnInformeFCPA(amec.idamecs, amec.idtipoactividad.ToString(), amec.descripcion,
                                amec.fechacomienzo, amec.fechafinalizacion, amec.idestado, 2, "", "", "",
                                dvOtroServicio.tipo,
                                entExp.Idexpediente.ToString());
                        }
                        else
                        {
                            agInfo.guardarEnInformeFCPA(entExp.Amec, "", "", null, null, 1, 2, "", "", "",
                                dvOtroServicio.tipo,
                                entExp.Idexpediente.ToString());
                        }
                    }


                }
                //////////////////////////////////////////////////
                
                /*if (agExp.ValidarEstadoAMEC(entExp.Idexpediente))
                {*/
                    sNavegacion = agExp.MsgNavegacion(entExp.Idexpediente);
                    if (!IsPonentes)
                    {
                        if (CambiarEstadoExpediente_a_Enviado(ref ListaReservasInfo, ref agExp, sNavegacion))
                        {
                            //Inicio Generación de reserva asociada a los fees
                            AgenteUsuarios agenteUsuFee = new AgenteUsuarios();
                            AgenteExpedientes agenteExpFee = new AgenteExpedientes();
                            DVPeticionariosRoles datosRolesFee = agenteUsuFee.ObtenerDatosRolesPorLogin();
                            var datosExpFee = agenteExpFee.ObtenerExpedientePorID(entExp.Idexpediente.ToString());
                            DAmec damecFee = agenteExpFee.ObtenerEntidadAMECporID(datosExpFee.Idamec.ToString());
                            SqlConnection connFees = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
                            connFees.Open();
                            string idconfEmpresa = damecFee.idconfempresa == null ? "0" : damecFee.idconfempresa.Value.ToString();
                            string consulta = " exec dbo.sp_fee_generar_reserva " + entExp.Idexpediente.ToString() + ", " + datosRolesFee.IdPeticionario + ", " + idconfEmpresa + ";";
                            SqlCommand commFees = new SqlCommand(consulta, connFees);
                            commFees = new SqlCommand(consulta, connFees);
                            commFees.ExecuteNonQuery();
                            connFees.Close();
                            //Fin Generación de reserva asociada a los fees

                        EnviarMailExpedienteEnviado(ListaReservasInfo, sNavegacion);
                        }
                        else
                        {
                            Alert.Show(string.Format("El expediente {0} ya está enviado ", entExp.Idexpediente), sNavegacion);
                        }
                    }
                    else
                    {
                        EnviarMailExpedienteEnviado(ListaReservasInfo, sNavegacion);
                        int n = agExp.CambiaEstadoExpedienteFinalizado(entExp.Idexpediente);
                        if (n <= 0)
                        {
                            Alert.Show(string.Format("Error al finalizar el expediente {0}", entExp.Idexpediente));
                        }
                    }
                /*}
                else
                {
                    Alert.Show(string.Format("No se permite continuar con la reserva porque el AMEC no está aprobado"), sNavegacion);
                }*/

                // cambiar estado expediente finalizado si el expediente solo tiene honorarios.

            }
        }

        private bool CambiarEstadoExpediente_a_Enviado(ref IList<ReservasInfo> ListaReservasInfo, ref AgenteExpedientes agExp, string sNavegacion)
        {
            try
            {

                if (!(entExp.Idestado == "AB" || entExp.Idestado == "NC"))
                {
                    Alert.Show(string.Format("El expediente {0} no puede ser Enviado en el estado actual", entExp.Idexpediente), sNavegacion);
                    return false;
                }
                else
                {
                    AgenteMaestros agente = new AgenteMaestros();
                    ListaReservasInfo = agente.obtenerInfoReserva(entExp.Idexpediente);

                    int nEnviado = agExp.CambiaEstadoExpedienteEnviado(entExp.Idexpediente);

                    if (nEnviado == -1)
                    {
                        Alert.Show(string.Format("No se han podido enviar todas las reservas del expediente {0} porque el estado del Amec", entExp.Idexpediente), sNavegacion);
                        return false;
                    }
                    //LJM 12/01/2018 - Eliminar restricción
                    //else if (nEnviado == -2)
                    //{
                    //    Alert.Show(string.Format("No se han podido enviar todas las reservas del expediente {0}  porque su importe supera el presupuesto del AMEC", entExp.Idexpediente), sNavegacion);
                    //    return false;
                    //}
                    else
                    {
                        return nEnviado > 0;
                    }
                }

            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Alert.Show(string.Format("Se ha producido un error en la modificación del Expediente {0}", entExp.Idexpediente), sNavegacion);
                return false;

            }
        }

        private void EnviarMailExpedienteEnviado(IList<ReservasInfo> ListaReservasInfo, string sNavegacion)
        {
            try
            {
                //IAV ENVIO MAIL
                Mail mail = new Mail();

                string body = string.Empty;
                if (entExp.Tiporeserva == "IND")
                {
                    body = "EXPEDIENTE INDIVIDUAL número: " + entExp.Idexpediente + "\n\n";
                }
                else if (entExp.Tiporeserva == "COL")
                {
                    body = "EXPEDIENTE COLECTIVO número: " + entExp.Idexpediente + "\n\n";
                }

                foreach (ReservasInfo item in ListaReservasInfo)
                {
                    if (item.idestado == "AB")
                    {
                        string infoReserva = FormateaInfoReservaMensaje(item);
                        body = body + infoReserva + "\n";
                    }
                }
                //Xavier Morell
                //AgenteUsuarios agenteUsu = new AgenteUsuarios();
                //DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
                DVPeticionariosRoles datosRoles = (DVPeticionariosRoles)Session["rolesuser"];
                AgenteExpedientes agenteExp = new Web.AgenteExpedientes();
                DAmec damec = agenteExp.ObtenerEntidadAMECporID(entExp.Idamec.ToString());
                mail.EnvioReservaMail(body, entExp.Idexpediente.ToString(), datosRoles.idempleadogp, true, ConfigUtil.GetAppSetting(Constantes.AppParams.CopiaContacto), damec.idconfempresa == null ? 0 : damec.idconfempresa.Value);
                Alert.Show(string.Format("Se ha enviado correctamente el expediente {0}", entExp.Idexpediente), (bCreando) ? "Expedientes.aspx" : sNavegacion);


            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Alert.Show(string.Format("Se ha producido un error en el envío del correo electrónico del Expediente {0}", entExp.Idexpediente), sNavegacion);

            }
        }

        private string FormateaInfoReservaMensaje(ReservasInfo item)
        {
            string cadenaReserva = string.Empty;
            string tipoReserva = string.Empty;
            string idServiciostr = string.Empty;
            int? idServicio = null;
            switch (item.IdTipo)
            {
                case "INS":
                    tipoReserva = "INSCRIPCION";
                    idServicio = item.IdServicioInscripcion;
                    break;
                case "HOT":
                    tipoReserva = "ALOJAMIETO";
                    idServicio = item.IdServicioHotel;
                    break;
                case "DSP":
                    tipoReserva = "TRANSPORTE";
                    idServicio = item.IdServicioTransporte;
                    break;
                case "ACT":
                    tipoReserva = "OTROS SERVICIOS - ACTIVIDAD";
                    idServicio = item.IdServicioActividad;
                    break;
                default:
                    break;
            }
            if (idServicio != null)
            {
                idServiciostr = idServicio.ToString();
            }

            cadenaReserva = "Id reserva: " + item.IdReserva.ToString() + "\n" +
                              "Tipo Servicio: " + tipoReserva + " \n" +
                              "Id Servicio: " + idServiciostr.ToString() + "\n";

            return cadenaReserva;
        }

        public void AprobarMail_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //Ismael Ameller 21-04-2011 Envio mail al aprobar
                Mail mail = new Mail();
                AgenteMaestros agente = new AgenteMaestros();
                IList<ReservasInfo> ListaReservasInfo = new List<ReservasInfo>();
                ListaReservasInfo = agente.obtenerInfoReserva(entExp.Idexpediente);
                string body = string.Empty;
                if (entExp.Tiporeserva == "IND")
                {
                    body = "EXPEDIENTE INDIVIDUAL número: " + entExp.Idexpediente + "\n\n";
                }
                else if (entExp.Tiporeserva == "COL")
                {
                    body = "EXPEDIENTE COLECTIVO número: " + entExp.Idexpediente + "\n\n";
                }
                int idReserva = int.Parse(((ImageButton)sender).CommandArgument);
                foreach (ReservasInfo item in ListaReservasInfo)
                {
                    if (item.IdReserva == idReserva)
                    {
                        string infoReserva = FormateaInfoReservaMensaje(item);
                        body = body + infoReserva + "\n";
                    }
                }
                //Xavier Morell
                //AgenteUsuarios agenteUsu = new AgenteUsuarios();
                //DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
                DVPeticionariosRoles datosRoles = (DVPeticionariosRoles)Session["rolesuser"];
                AgenteExpedientes agenteExp = new Web.AgenteExpedientes();
                DAmec damec = agenteExp.ObtenerEntidadAMECporID(entExp.Idamec.ToString());
                mail.EnvioReservaMail(body, entExp.Idexpediente.ToString(), datosRoles.idempleadogp, false, ConfigUtil.GetAppSetting(Constantes.AppParams.CopiaContacto), damec.idconfempresa == null ? 0 : damec.idconfempresa.Value);
                //FIN Ismael Ameller 21-04-2011 Envio mail al aprobar
            }
            catch { }
        }
        protected void btnAprobar_Click(object sender, EventArgs e)
        {
            int nValidaEstadoAmec = 0;
            int nValidaPresupAmec = 0;

            if (entExp.Idexpediente > 0)
            {
                string sNavegacion = "";

                try
                {
                    AgenteExpedientes agExp = new AgenteExpedientes();
                    sNavegacion = agExp.MsgNavegacion(entExp.Idexpediente);

                    AgenteMaestros agente = new AgenteMaestros();
                    IList<ReservasInfo> ListaReservasInfo = new List<ReservasInfo>();
                    ListaReservasInfo = agente.obtenerInfoReserva(entExp.Idexpediente);

                    int nEnviado = agExp.CambiaEstadoExpedienteAprobado(entExp.Idexpediente, ref nValidaEstadoAmec, ref nValidaPresupAmec);

                    if (nEnviado > 0)
                    {
                        try
                        {
                            //Ismael Ameller 21-04-2011 Envio mail al aprobar
                            Mail mail = new Mail();

                            string body = string.Empty;
                            if (entExp.Tiporeserva == "IND")
                            {
                                body = "EXPEDIENTE INDIVIDUAL número: " + entExp.Idexpediente + "\n\n";
                            }
                            else if (entExp.Tiporeserva == "COL")
                            {
                                body = "EXPEDIENTE COLECTIVO número: " + entExp.Idexpediente + "\n\n";
                            }

                            foreach (ReservasInfo item in ListaReservasInfo)
                            {
                                if (item.idestado == "CTZ")
                                {
                                    string infoReserva = FormateaInfoReservaMensaje(item);
                                    body = body + infoReserva + "\n";
                                }
                            }
                            //AgenteUsuarios agenteUsu = new AgenteUsuarios();
                            //DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
                            DVPeticionariosRoles datosRoles = (DVPeticionariosRoles)Session["rolesuser"];
                            AgenteExpedientes agenteExp = new Web.AgenteExpedientes();
                            DAmec damec = agenteExp.ObtenerEntidadAMECporID(entExp.Idamec.ToString());
                            mail.EnvioReservaMail(body, entExp.Idexpediente.ToString(), datosRoles.idempleadogp, false, ConfigUtil.GetAppSetting(Constantes.AppParams.CopiaContacto), damec.idconfempresa == null ? 0 : damec.idconfempresa.Value);
                            //FIN Ismael Ameller 21-04-2011 Envio mail al aprobar
                        }
                        catch { }
                        if (nValidaEstadoAmec == 0 && nValidaPresupAmec == 0)
                        {
                            Alert.Show(string.Format("Se han aprobado correctamente {0} servicios para el expediente {1}", nEnviado, entExp.Idexpediente), sNavegacion);
                        }
                        else if (nValidaEstadoAmec > 0 && nValidaPresupAmec == 0)
                        {
                            Alert.Show(string.Format("Se han aprobado correctamente {0} servicios para el expediente {1} pero {2} servicios no se han podido aprobar porque su AMEC no está aprobado", nEnviado, entExp.Idexpediente, nValidaEstadoAmec), sNavegacion);
                        }
                        //LJM 12/01/2018 - Eliminar restricción
                        //else if (nValidaEstadoAmec == 0 && nValidaPresupAmec > 0)
                        //{
                        //    Alert.Show(string.Format("Se han aprobado correctamente {0} servicios para el expediente {1} pero {2} servicios no se han podido aprobar porque su importe supera el presupuesto del AMEC", nEnviado, entExp.Idexpediente, nValidaPresupAmec), sNavegacion);
                        //}
                        //else if (nValidaEstadoAmec > 0 && nValidaPresupAmec > 0)
                        //{
                        //    Alert.Show(string.Format("Se han aprobado correctamente {0} servicios para el expediente {1} pero {2} servicios no se han podido aprobar porque su AMEC no está aprobado y {3} servicios no se han podido aprobar porque su importe supera el presupuesto del AMEC", nEnviado, entExp.Idexpediente,nValidaEstadoAmec , nValidaPresupAmec), sNavegacion);
                        //}

                    }
                    else
                    {
                        //LJM 12/01/2018 - Eliminar restricción
                        //if (nValidaEstadoAmec > 0 && nValidaPresupAmec > 0)
                        //{
                        //    Alert.Show(string.Format("No se han podido aprobar ningún servicio para el expediente {0} porque en algunos su AMEC no está aprobado y en otros su importe supera el presupuesto del AMEC", entExp.Idexpediente), sNavegacion);
                        //}
                        if (nValidaEstadoAmec > 0)
                        {
                            Alert.Show(string.Format("No se han podido aprobar ningún servicio para el expediente {0} porque el AMEC no está aprobado", entExp.Idexpediente), sNavegacion);
                        }
                        //LJM 12/01/2018 - Eliminar restricción
                        //else if (nValidaPresupAmec > 0)
                        //{
                        //    Alert.Show(string.Format("No se ha podido aprobar ningún servicio para el expediente {0} porque su importe supera el presupuesto del AMEC", entExp.Idexpediente), sNavegacion);
                        //}
                        else
                        {
                            Alert.Show(string.Format("No se ha podido aprobar ningún servicio para el expediente {0}", entExp.Idexpediente), sNavegacion);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Ismael Ameller 09-03-2011 Envio de Mail
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                    //FIN Ismael Ameller 09-03-2011 Envio de Mail
                    Alert.Show(string.Format("Se ha producido un error en la modificación del Expediente {0}", entExp.Idexpediente), sNavegacion);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            string msg;

            if (entExp.Idexpediente > 0)
            {
                string sNavegacion = "";

                try
                {
                    AgenteExpedientes agExp = new AgenteExpedientes();
                    sNavegacion = agExp.MsgNavegacion(entExp.Idexpediente);

                    if (entExp.Idestado == "AN" || entExp.Idestado == "CN")
                    {
                        Alert.Show(string.Format("El expediente {0} ya está Cancelado", entExp.Idexpediente), sNavegacion);
                    }
                    else
                    {
                        int nCancelado = agExp.CambiaEstadoExpedienteCancelado(entExp.Idexpediente);
                        if (nCancelado > 0)
                        {
                            msg = (entExp.Idestado == "CN" ? "cancelado" : "rechazado");

                            Alert.Show(string.Format("Se ha {0} correctamente el expediente {1}", msg, entExp.Idexpediente), sNavegacion);
                        }
                        else
                        {
                            Alert.Show(string.Format("No se ha podido modificar el estado del expediente {0}", entExp.Idexpediente), sNavegacion);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Ismael Ameller 09-03-2011 Envio de Mail
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                    //FIN Ismael Ameller 09-03-2011 Envio de Mail
                    Alert.Show(string.Format("Se ha producido un error en la modificación del Expediente {0}", entExp.Idexpediente), sNavegacion);
                }
            }
        }

        protected void Listas_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            AgenteExpedientes agExp = new AgenteExpedientes();
            string sNavegacion = agExp.MsgNavegacion(entExp.Idexpediente);
            string sRedirect = "";
            int nCambio = 0;
            bool bOK = true;

            try
            {
                int nIDReserva = 0;
                if (e.CommandArgument != null)
                {
                    if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                    {
                        nIDReserva = int.Parse(e.CommandArgument.ToString());

                        if (e.CommandName.StartsWith("MOD"))
                        {
                            //Qurius (EAS) 26/01/2011
                            //nCambio = agExp.CambiaEstadoServicioSinEnviar(nIDReserva);
                            //if (nCambio > 0)
                            //{
                            // Le lleva a la ventana de Modificación
                            nCambio = 1;
                            int nTab = int.Parse(e.CommandName.Substring(3));
                            sRedirect = string.Format("Inscripcion.aspx?idexp={0}&tab={1}&idres={2}", entExp.Idexpediente, nTab, nIDReserva);
                            //}
                        }
                        else if (e.CommandName == "ACE")
                        {
                            // Modificar el Estado a Aceptado
                            //JLV ---- 110512 ---------------start
                            AgenteMaestros agente = new AgenteMaestros();
                            int idServicioReserva = agente.GetServicioByReserva(nIDReserva);
                            if (!agente.ValidarReservaEstadoAmec(idServicioReserva))
                                Alert.Show(string.Format("El estado del AMEC no permite realizar el envío del servicio"), sNavegacion);
                            //ShowMessage("El estado del AMEC no permite realizar el envío del servicio", "");
                            // LJM 20180124 - Eliminar restricción  
                            //else if (!agente.ValidarReservaImporteAmec(idServicioReserva))
                            //    //ShowMessage("El servicio no se ha podido enviar ya que el importe superaría el presupuesto máximo del AMEC.", "");
                            //    Alert.Show(string.Format("El servicio no se ha podido enviar ya que el importe superaría el presupuesto máximo del AMEC."), sNavegacion);
                            else
                            {
                                nCambio = agExp.CambiaEstadoServicioAceptado(nIDReserva);
                            }
                            //JLV ---- 110512 ---------------start
                        }
                        else if (e.CommandName == "CAN")
                        {
                            // Modificar el Estado a Cancelado
                            nCambio = agExp.CambiaEstadoServicioCancelado(nIDReserva);


                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                bOK = false;
                Alert.Show(string.Format("Se ha producido un error en la modificación de la Reserva"), sNavegacion);
            }

            if (bOK)
            {
                if (nCambio > 0)
                {
                    sRedirect = string.IsNullOrEmpty(sRedirect) ? sNavegacion : sRedirect;
                    this.lvServicioInscripcion.DataBind();
                    this.lvServicioAlojamiento.DataBind();
                    this.lvServicioTransporte.DataBind();
                    this.lvServicioActividades.DataBind();

                    

                    Server.Transfer(sRedirect);
                    //Response.Clear();
                    //Response.Expires = 0;
                    //Response.CacheControl = "Private";
                    //Response.AddHeader("PRAGMA", "NO-CACHE"); 
                    //Response.Redirect(sRedirect);


                }
                else
                {
                    Alert.Show(string.Format("No se ha podido modificar el estado de la Reserva"), sNavegacion);
                }
            }

        }

        #endregion

        #region "Grid Participantes"

        protected void lvResumenParticipantes_Sorting(object sender, ListViewSortEventArgs e)
        {
            string strCssClass = "";
            if (e.SortDirection == SortDirection.Ascending)
            {
                strCssClass = "eosSortAscending";
            }
            else
            {
                strCssClass = "eosSortDescending";
            }

            LinkButton lbNombre = (LinkButton)this.lvResumenParticipantes.FindControl("lbNombre");
            LinkButton lbApel1 = (LinkButton)this.lvResumenParticipantes.FindControl("lbApel1");
            LinkButton lbApel2 = (LinkButton)this.lvResumenParticipantes.FindControl("lbApel2");
            LinkButton lbTotal = (LinkButton)this.lvResumenParticipantes.FindControl("lbTotal");

            if (e.SortExpression == "Nombre")
            {
                lbNombre.CssClass = strCssClass;
                lbApel1.CssClass = "";
                lbApel2.CssClass = "";
                lbTotal.CssClass = "";
            }
            else if (e.SortExpression == "Apel1")
            {
                lbNombre.CssClass = "";
                lbApel1.CssClass = strCssClass;
                lbApel2.CssClass = "";
                lbTotal.CssClass = "";
            }
            else if (e.SortExpression == "Apel2")
            {
                lbNombre.CssClass = "";
                lbApel1.CssClass = "";
                lbApel2.CssClass = strCssClass;
                lbTotal.CssClass = "";
            }
            else if (e.SortExpression == "Total")
            {
                lbNombre.CssClass = "";
                lbApel1.CssClass = "";
                lbApel2.CssClass = "";
                lbTotal.CssClass = strCssClass;
            }
        }

        protected void odsParticipantes_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (entExp != null)
            {
                e.InputParameters["filtroIDExp"] = entExp.Idexpediente;
            }
        }

        public double dTotalIns = 0;
        public double dTotalAct = 0;
        public double dTotalHot = 0;
        public double dTotalDsp = 0;
        public double dTotalFee = 0;
        protected void lvResumenParticipantes_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DVServicioPassengerResumen dvPass = (DVServicioPassengerResumen)e.Item.DataItem;

            dTotalIns += (dvPass.ImporteINS.HasValue) ? dvPass.ImporteINS.Value : 0;
            dTotalAct += (dvPass.ImporteACT.HasValue) ? dvPass.ImporteACT.Value : 0;
            dTotalHot += (dvPass.ImporteHOT.HasValue) ? dvPass.ImporteHOT.Value : 0;
            dTotalDsp += (dvPass.ImporteDSP.HasValue) ? dvPass.ImporteDSP.Value : 0;
            dTotalFee = (dvPass.ImporteFee.HasValue) ? dvPass.ImporteFee.Value : 0;

            ((Label)this.lvResumenParticipantes.FindControl("TotalIns")).Text = dTotalIns.ToString("N2") + " €";
            ((Label)this.lvResumenParticipantes.FindControl("TotalAct")).Text = dTotalAct.ToString("N2") + " €";
            ((Label)this.lvResumenParticipantes.FindControl("TotalHot")).Text = dTotalHot.ToString("N2") + " €";
            ((Label)this.lvResumenParticipantes.FindControl("TotalDsp")).Text = dTotalDsp.ToString("N2") + " €";
            ((Label)this.lvResumenParticipantes.FindControl("TotalRes")).Text = (dTotalIns + dTotalAct + dTotalHot + dTotalDsp).ToString("N2") + " €";
            ((Label)this.lvResumenParticipantes.FindControl("lblFee")).Text = "Fee: " + dTotalFee.ToString("N2") + " €";
            ((Label)this.lvResumenParticipantes.FindControl("lblTotalFee")).Text = "Total + Fee: " + (dTotalIns + dTotalAct + dTotalHot + dTotalDsp + dTotalFee).ToString("N2") + " €";

        }

        #endregion

        #region "Grid Inscripciones"

        public double dInscripcionPax = 0;
        public double dInscripcionTotal = 0;
        string _participante = "";
        protected void lvServicioInscripcion_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DVServicioInscripciones dvIns = (DVServicioInscripciones)e.Item.DataItem;
            AgenteParticipantes agPar = new AgenteParticipantes();
            dInscripcionTotal += (dvIns.cotizado.HasValue) ? dvIns.cotizado.Value : 0;

            ((Label)this.lvServicioInscripcion.FindControl("InscripcionTotal")).Text = dInscripcionTotal.ToString("N2") + " €";

            //((Label)e.Item.FindControl("InscripcionPaxDetalle")).Text = String.Format("{0}", agPar.ObtenerParticipantesPorInscripcion(entExp.Idexpediente, dvIns.idservicioinscripcion));
            //((ImageButton)e.Item.FindControl("imgServicioMod")).OnClientClick = string.Format("javascript:document.location.href = 'Inscripcion.aspx?idexp={0}&tab={1}';return false;", entExp.Idexpediente, 0);
            //Qurius (EAS) 26/01/2011 
            string _participanteInscripcion = agPar.ObtenerParticipantesPorInscripcion(entExp.Idexpediente, dvIns.idservicioinscripcion).Replace("<ul>", "").Replace("</ul>", "");
            if (!_participante.Contains(_participanteInscripcion) && (dvIns.IdEstado != "CN" && dvIns.IdEstado != "AN" && dvIns.IdEstado != "CNTR"))
            {
                _participante += _participanteInscripcion;
                dInscripcionPax += (dvIns.pax.HasValue) ? dvIns.pax.Value : 0;
            }
            ((Label)e.Item.FindControl("InscripcionPaxDetalle")).Text = String.Format("{0}", _participanteInscripcion);
            ((Label)this.lvServicioInscripcion.FindControl("InscripcionPax")).Text = String.Format("{0}", dInscripcionPax);
            //Ismael Ameller 23-03-2011 Le paso el estado de la reserva
            ((ImageButton)e.Item.FindControl("imgServicioAlternativas")).OnClientClick = string.Format("javascript:document.location.href = 'Inscripcion.aspx?idexp={0}&tab={1}&idres={2}&view=alternativa&idEstado={3}';return false;", entExp.Idexpediente, 0, dvIns.idreserva, dvIns.IdEstado);

            CalculaEstadoIconos(e.Item, dvIns.IdEstado);
        }

        protected void lvServicioInscripcion_LayoutCreated(object sender, EventArgs e)
        {
            if (entExp != null)
            {
                AgenteParticipantes agPar = new AgenteParticipantes();
                ((Label)this.lvServicioInscripcion.FindControl("InscripcionPaxDetalleTotal")).Text = agPar.ObtenerParticipantesPorInscripcion(entExp.Idexpediente, null);
            }
        }

        protected void odsServiciosInscripciones_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (entExp != null)
            {
                e.InputParameters["idExpediente"] = entExp.Idexpediente;
            }
        }

        #endregion

        #region "Grid Alojamientos"

        public double dHotelPax = 0;
        public double dHotelTotal = 0;
        string _participanteAlojamiento = "";
        protected void lvServicioAlojamiento_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DVServicioHotel dvHot = (DVServicioHotel)e.Item.DataItem;
            AgenteParticipantes agPar = new AgenteParticipantes();

            dHotelTotal += (dvHot.cotizado.HasValue) ? dvHot.cotizado.Value : 0;


            ((Label)this.lvServicioAlojamiento.FindControl("HotelTotal")).Text = dHotelTotal.ToString("N2") + " €";

            ((ImageButton)e.Item.FindControl("imgServicioAlternativas")).OnClientClick = string.Format("javascript:document.location.href = 'Inscripcion.aspx?idexp={0}&tab={1}&idres={2}&view=alternativa&idEstado={3}';return false;", entExp.Idexpediente, 1, dvHot.idserviciohotel, dvHot.IdEstado);
            // Qurius (EAS) 27/01/2011
            //((Label)e.Item.FindControl("HotelPaxDetalle")).Text = String.Format("{0}", agPar.ObtenerParticipantesPorHotel(entExp.Idexpediente, dvHot.idserviciohotel));

            string _NombreparticipanteAlojamiento = agPar.ObtenerParticipantesPorHotel(entExp.Idexpediente, dvHot.idserviciohotel).Replace("<ul>", "").Replace("</ul>", "");
            if (!_participanteAlojamiento.Contains(_NombreparticipanteAlojamiento) && (dvHot.IdEstado != "CN" && dvHot.IdEstado != "AN" && dvHot.IdEstado != "CNTR"))
            {
                _participanteAlojamiento += _NombreparticipanteAlojamiento;
                dHotelPax += (dvHot.pax.HasValue) ? dvHot.pax.Value : 0;
            }
            ((Label)e.Item.FindControl("HotelPaxDetalle")).Text = String.Format("{0}", _NombreparticipanteAlojamiento);
            ((Label)this.lvServicioAlojamiento.FindControl("HotelPax")).Text = String.Format("{0}", dHotelPax);

            CalculaEstadoIconos(e.Item, dvHot.IdEstado);
        }

        protected void lvServicioAlojamiento_LayoutCreated(object sender, EventArgs e)
        {
            if (entExp != null)
            {
                AgenteParticipantes agPar = new AgenteParticipantes();
                ((Label)this.lvServicioAlojamiento.FindControl("HotelPaxDetalleTotal")).Text = agPar.ObtenerParticipantesPorHotel(entExp.Idexpediente, null);
            }
        }

        protected void odsServiciosAlojamientos_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (entExp != null)
            {
                e.InputParameters["idExpediente"] = entExp.Idexpediente;
            }
        }
        #endregion

        #region "Grid Transportes"

        public double dTransportePax = 0;
        public double dTransporteTotal = 0;

        public bool bIda1 = false;
        public bool bIda2 = false;
        public bool bReg1 = false;
        public bool bReg2 = false;

        string _participanteTransporte = "";

        protected void lvServicioTransporte_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DVServicioTransporte dvTra = (DVServicioTransporte)e.Item.DataItem;
            AgenteParticipantes agPar = new AgenteParticipantes();

            dTransporteTotal += (dvTra.cotizado.HasValue) ? dvTra.cotizado.Value : 0;


            ((Label)this.lvServicioTransporte.FindControl("TransporteTotal")).Text = dTransporteTotal.ToString("N2") + " €";

            ((ImageButton)e.Item.FindControl("imgServicioAlternativas")).OnClientClick = string.Format("javascript:document.location.href = 'Inscripcion.aspx?idexp={0}&tab={1}&idres={2}&view=alternativa&idEstado={3}';return false;", entExp.Idexpediente, 2, dvTra.idserviciotransporte, dvTra.IdEstado);

            // Qurius (EAS) 27/01/2011
            //((Label)e.Item.FindControl("TransportePaxDetalle")).Text = String.Format("{0}", agPar.ObtenerParticipantesPorDesplazamiento(entExp.Idexpediente, dvTra.idserviciotransporte));
            string _NombreparticipanteTransporte = agPar.ObtenerParticipantesPorDesplazamiento(entExp.Idexpediente, dvTra.idserviciotransporte).Replace("<ul>", "").Replace("</ul>", "");
            if (!_participanteTransporte.Contains(_NombreparticipanteTransporte) && (dvTra.IdEstado != "CN" && dvTra.IdEstado != "AN" && dvTra.IdEstado != "CNTR"))
            {
                _participanteTransporte += _NombreparticipanteTransporte;
                dTransportePax += (dvTra.pax.HasValue) ? dvTra.pax.Value : 0;
            }
            ((Label)e.Item.FindControl("TransportePaxDetalle")).Text = String.Format("{0}", _NombreparticipanteTransporte);
            ((Label)this.lvServicioTransporte.FindControl("TransportePax")).Text = String.Format("{0}", dTransportePax);

            CalculaEstadoIconos(e.Item, dvTra.IdEstado);

            ((System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("trIda1")).Visible = !string.IsNullOrEmpty(dvTra.ida1_transporte);
            ((System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("trIda2")).Visible = !string.IsNullOrEmpty(dvTra.ida2_transporte);
            ((System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("trReg1")).Visible = !string.IsNullOrEmpty(dvTra.reg1_transporte);
            ((System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("trReg2")).Visible = !string.IsNullOrEmpty(dvTra.reg2_transporte);
        }

        protected void lvServicioTransporte_LayoutCreated(object sender, EventArgs e)
        {
            AgenteParticipantes agPar = new AgenteParticipantes();
            ((Label)this.lvServicioTransporte.FindControl("TransportePaxDetalleTotal")).Text = agPar.ObtenerParticipantesPorDesplazamiento(entExp.Idexpediente, null);
        }

        protected void odsServiciosTransportes_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (entExp != null)
            {
                e.InputParameters["idExpediente"] = entExp.Idexpediente;
            }
        }

        #endregion

        #region "Grid Actividades"

        public double dActividadPax = 0;
        public double dActividadTotal = 0;
        string _participanteActividades = "";
        protected void lvServicioActividades_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            DVServicioActividades dvAct = (DVServicioActividades)e.Item.DataItem;
            AgenteParticipantes agPar = new AgenteParticipantes();

            dActividadTotal += (dvAct.cotizado.HasValue) ? dvAct.cotizado.Value : 0;

            ((Label)this.lvServicioActividades.FindControl("ActividadTotal")).Text = dActividadTotal.ToString("N2") + " €";

            //Qurius (EAS) 26/01/2011
            ((ImageButton)e.Item.FindControl("imgServicioAlternativas")).OnClientClick = string.Format("javascript:document.location.href = 'Inscripcion.aspx?idexp={0}&tab={1}&idres={2}&view=alternativa&idEstado={3}';return false;", entExp.Idexpediente, 3, dvAct.idreserva, dvAct.IdEstado);

            //((Label)e.Item.FindControl("ActividadPaxDetalle")).Text = String.Format("{0}", agPar.ObtenerParticipantesPorActividad(entExp.Idexpediente, dvAct.idservicioactividad));
            string _NombreparticipanteActividad = agPar.ObtenerParticipantesPorActividad(entExp.Idexpediente, dvAct.idservicioactividad).Replace("<ul>", "").Replace("</ul>", "");
            if (!_participanteActividades.Contains(_NombreparticipanteActividad) && (dvAct.IdEstado != "CN" && dvAct.IdEstado != "AN" && dvAct.IdEstado != "CNTR"))
            {
                _participanteActividades += _NombreparticipanteActividad;
                dActividadPax += (dvAct.pax.HasValue) ? dvAct.pax.Value : 0;
            }
            if (dActividadPax == 0)
            {
                ((Label)this.lvServicioActividades.FindControl("ActividadPax")).Visible = false;
                ((HtmlAnchor)this.lvServicioActividades.FindControl("ActividadPaxPlus")).Visible = false;
            }
            ((Label)e.Item.FindControl("ActividadPaxDetalle")).Text = String.Format("{0}", _NombreparticipanteActividad);
            ((Label)this.lvServicioActividades.FindControl("ActividadPax")).Text = String.Format("{0}", dActividadPax);


            CalculaEstadoIconos(e.Item, dvAct.IdEstado);
        }

        protected void lvServicioActividades_LayoutCreated(object sender, EventArgs e)
        {
            AgenteParticipantes agPar = new AgenteParticipantes();
            ((Label)this.lvServicioActividades.FindControl("ActividadPaxDetalleTotal")).Text = agPar.ObtenerParticipantesPorActividad(entExp.Idexpediente, null);
        }

        protected void odsOtrosServicios_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (entExp != null)
            {
                e.InputParameters["idExpediente"] = entExp.Idexpediente;
            }
        }

        #endregion


        public string GetEstadoTooltip(string IdEstado)
        {
            //Ismael Ameller 08-03-2011 Sacamos los Labels de la BBDD

            AgenteExpedientes agExp = new AgenteExpedientes();
            string sToolTip = "";

            if (IdEstado == "AB") // Sin Enviar
            {
                sToolTip = agExp.ObtenerLabelEstado("AB"); ;
            }
            else if (IdEstado == "CR") // Enviado
            {
                sToolTip = agExp.ObtenerLabelEstado("CR"); ;
            }
            else if (IdEstado == "CTZD") // Cotizando
            {
                sToolTip = agExp.ObtenerLabelEstado("CTZD"); ;
            }
            else if (IdEstado == "CTZ") // Cotizado
            {
                sToolTip = agExp.ObtenerLabelEstado("CTZ"); ;
            }
            else if (IdEstado == "CFP") // Aceptado
            {
                sToolTip = agExp.ObtenerLabelEstado("CFP"); ;
            }
            else if (IdEstado == "PTR") // Tramitando
            {
                sToolTip = agExp.ObtenerLabelEstado("PTR"); ;
            }
            else if (IdEstado == "TR") // Tramitado
            {
                sToolTip = agExp.ObtenerLabelEstado("TR"); ;
            }
            else if (IdEstado == "CN") // Cancelado
            {
                sToolTip = agExp.ObtenerLabelEstado("CN"); ;
            }
            else if (IdEstado == "CNTR") // Cancelado
            {
                sToolTip = agExp.ObtenerLabelEstado("CNTR"); ;
            }
            else if (IdEstado == "AN") // Rechazado
            {
                sToolTip = agExp.ObtenerLabelEstado("AN"); ;
            }
            return sToolTip;
        }

        public bool GetEstadoShow(string IdEstado)
        {
            bool sReturn = true;

            switch (IdEstado)
            {
                case "AB":
                case "CR":
                    sReturn = false;
                    break;
                default:
                    sReturn = true;
                    break;
            }
            return sReturn;
        }

        //Ismael Ameller Vidal 23-03-2011 Mostramos los productos del expediente
        private string ProductosExpediente(string sIdExp)
        {
            AgenteExpedientes agExp = new AgenteExpedientes();
            return agExp.ProductosExpediente(sIdExp);
        }
        //FIN Ismael Ameller Vidal 23-03-2011 Mostramos los productos del expediente

        #region "combo servicios"

        /// <summary>
        /// Pau Ferrer 20-06-2011. Nos aseguramos que el id de reserva no es null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstServicios_SelectedIndexChanged(object sender, EventArgs e)
        {
            AgenteExpedientes agenteExp = new Web.AgenteExpedientes();
            entExp = agenteExp.ObtenerExpedientePorID(Request.QueryString["idexp"]);

            if (entExp.Idtiporeserva == 2)
            {
                Session[Constantes.Session.idReserva] = null;
            }

            Response.Redirect(lstServicios.SelectedValue);

        }

        #endregion

        [System.Web.Services.WebMethod]
        public static bool CheckJustificanteInscripcion(int idExpediente, int idReserva, int idServicio, int idServicioAlojamiento, int idServicioTransporte)
        {
            AgenteExpedientes agExp = new AgenteExpedientes();

            //Se ha pinchado en el botón aprobar del expediente
            if (idReserva == 0 && idServicio == 0 && idServicioAlojamiento == 0 && idServicioTransporte == 0)
            {

                AgenteMaestros agente = new AgenteMaestros();
                IList<ReservasInfo> ListaReservasInfo = new List<ReservasInfo>();
                ListaReservasInfo = agente.obtenerInfoReserva(idExpediente);
                bool checkJustificante = true;
                foreach (ReservasInfo item in ListaReservasInfo)
                {
                    //Solo se comprueba si existe justificante para las reservas que tienen los servicios cotizados,
                    //ya que la acción de aprobar todo solo lo hace sobre las reservas en estado CTZ
                    if (item.idestado == "CTZ")
                    {
                        switch (item.IdTipo)
                        {
                            case "HOT":
                                var serviciosHoteles = agExp.ObtenerServiciosHoteles(idExpediente);
                                var servicioHotel = serviciosHoteles.Where(x => x.idserviciohotel == item.IdServicioHotel).FirstOrDefault();
                                checkJustificante = checkJustificante && agExp.CheckJustificanteInscripcion(idExpediente, item.IdReserva, servicioHotel.idservicio, item.IdServicioHotel.HasValue ? item.IdServicioHotel.Value : 0, item.IdServicioTransporte.HasValue ? item.IdServicioTransporte.Value : 0);
                                break;
                            case "DSP":
                                var serviciosTransporte = agExp.ObtenerServiciosTransportes(idExpediente);
                                var servicioTransporte = serviciosTransporte.Where(x => x.idserviciotransporte == item.IdServicioTransporte).FirstOrDefault();
                                checkJustificante = checkJustificante && agExp.CheckJustificanteInscripcion(idExpediente, item.IdReserva, servicioTransporte.idservicio, item.IdServicioHotel.HasValue ? item.IdServicioHotel.Value : 0, item.IdServicioTransporte.HasValue ? item.IdServicioTransporte.Value : 0);
                                break;
                            default:
                                break;
                        }
                    }
                    
                }
                return checkJustificante;
            }
            //Se ha pinchado en el botón aprobar del servicio
            else
            {                
                return agExp.CheckJustificanteInscripcion(idExpediente, idReserva, idServicio, idServicioAlojamiento, idServicioTransporte);
            }
        }
    }
}