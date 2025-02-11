using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net.Mail;
using System.Globalization;
using EOS.Web.BLL.GestorPermisos;
using EOS.Web.BLL.GestorPermisos.Permisos;
using EOS.Web;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Web.Enums;
using EOS.Repositorios;
using EOS.Web.BLL.GestorFlujo;
using EOS.ServiceLogic.BLL.GestorDocumental;


namespace EOS
{

    public partial class DetalleAMECVeeva : Page
    {
        DAmecInfo miAmec = new DAmecInfo();
        AgenteAMEC agAMEC = null;
        AgenteAmecInfo agAMECinfo = null;

        public static string savePath;
        public static string savePathPrograma;
        public static string savePathDocumentacion;

        public static int estadoAmecAnterior;
        public static string sURL = "";

        public int idcongreso;
        public bool estaAnulandoPrograma;
        public bool Sometiendo = false;
        public string MailCopia = ConfigUtil.GetAppSetting("MailCopia");
        public string MailCopiaProgramador = ConfigUtil.GetAppSetting("MailCopiaProgramador");
        public string RutaLinkMail = ConfigUtil.GetAppSetting("RutaLinkMailAmec");
        public string mailGestorArchivos = ConfigUtil.GetAppSetting("MailGestorArchivos");
        public string mailCasosClinicos = ConfigUtil.GetAppSetting("MailCasosClinicos");

        public enum Estado { Aprobado = 1, Pendiente, Cancelado, Denegado, Borrador };
        EOS.Entidades.Datos.DVPeticionariosRoles datosUsuarioAMEC;
        EOS.Entidades.Datos.DDatosPersonalesUsuario datosUsuarioSolicitante;
        EOS.Entidades.Datos.DPeticionarioManager datosPeticionarioManager;
        EOS.Entidades.Datos.DDatosPersonalesUsuario datosUsuarioCorreo;
        EOS.Entidades.Datos.DUnidadesOrganizativasAmec unidOrgXDefecto;

        bool AccionUOCambioSolicitante = true;

        public ICollection<DPeticionarioManager> _listManagers;

        public ICollection<DPeticionarioManager> LstManagers
        {
            set
            {
                Session["LstManagers_" + miAmec.idamecs] = value;
            }
            get
            {
                if (Session["LstManagers_" + miAmec.idamecs] == null)
                {
                    _listManagers = new List<DPeticionarioManager>();
                    Session["LstManagers_" + miAmec.idamecs] = _listManagers;
                }
                return (ICollection<DPeticionarioManager>)Session["LstManagers_" + miAmec.idamecs];
            }
        }

        #region CargadoPagina

        protected void Page_Load(object sender, EventArgs e)
        {
            ///////////////Controlar Si caduca la sesión en la aplicación//////////////
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Session["rolesuser"] == null) Response.Redirect("~/Account/Login.aspx");
            //////////////////////////////////////////////////////////////////////////
            RepositorioFlujo estadoAMEC = null;
            try
            {

                if (!Page.IsPostBack)
                {
                    LimpiarSession();
                    datosUsuarioAMEC = (DVPeticionariosRoles)Session["rolesuser"];
                    agAMEC = datosUsuarioAMEC != null ? new AgenteAMEC(datosUsuarioAMEC.IdPeticionario) : null;
                    //Comprobamos si es un nuevo AMEC o si es una visualizacion de AMEC.                 
                    string sIdAmec = Request.QueryString["idamec"];
                    if (sIdAmec != null)
                    {
                        string idamec = sIdAmec;
                        if (Request.QueryString["idcongresoactividad"] != null)
                        {
                            idcongreso = Int32.Parse(Request.QueryString["idcongresoactividad"]);
                        }

                        ObtenerAMEC(idamec); //OBTENER DATOS DEL AMEC
                        InicializaSesion();
                        InicializaCombos();
                        CargarControles();
                        HabilitaBotones();
                        ImagenEstadoAMEC();
                        MostrarPorTipoActividad(ddlActividad.Text, true);
                    }
                }
                else
                {
                    RecuperaSesion();
                    CargarValoreAmecFormulario();
                    ImagenEstadoAMEC();
                    
                    HabilitaBotones();
                }

                this.SetFocus("txtDescripcionAMEC");

                AgenteAmecInfo aAmec = new AgenteAmecInfo();
            }
            catch (Exception ex)
            {
                EOSLogger.PrintError(this.GetType().Name, "Page_Load", ex.Message, ex);
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
                throw ex;
            }

        }

        protected void CargarValoreAmecFormulario()
        {
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            miAmec.idsolicitante = Convert.ToInt32(ddlSolicitante.SelectedValue);
            datosUsuarioSolicitante = agenteUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());

            if (txtNAmec.Text != null && txtNAmec.Text != "0") miAmec.idamecs = txtNAmec.Text;
            if (txtNAmec.Text == "0" && lbidAMEC.InnerText != "") miAmec.idamecs = lbidAMEC.InnerText;

            if (!string.IsNullOrEmpty(txtIdEstado.Text)) miAmec.idestado = Convert.ToUInt32(txtIdEstado.Text);
            if (!string.IsNullOrEmpty(txtDescripcionAMEC.Text)) { miAmec.descripcion = txtDescripcionAMEC.Text; }

            miAmec.idcreadopor = int.Parse(txtIdCreadoPor.Text);
            miAmec.idconfempresa = Convert.ToInt32(ddlAgencias.SelectedValue);
            miAmec.nwein = txtWein.Text;
            miAmec.idcargo = datosUsuarioSolicitante.IdCargo;
            miAmec.idposition = datosUsuarioSolicitante.IdPosition;
            miAmec.idtipoactividad = Int32.Parse(ddlActividad.SelectedValue);

            if (!string.IsNullOrEmpty(txtImporteTotalGasto.Text))
            {
                txtImporteTotalGasto.Text = txtImporteTotalGasto.Text.Replace(".", "");
                miAmec.importegasto = Decimal.Parse(txtImporteTotalGasto.Text.Replace(",", "."), CultureInfo.InvariantCulture);
                txtImporteTotalGasto.Text = string.Format("{0:n2}", miAmec.importegasto);
            }
        }

        protected void InicializaCombos()
        {
            RellenarCombos();
        }

        /// <summary>
        /// Guarda las variables agAMEC datosUsuarioAMEC datosUsuarioSolicitante unidOrgXDefecto AccionUOCambioSolicitante
        /// </summary>
        protected void InicializaSesion()
        {
            Session.Add("NuevoAgente", agAMEC);
            Session.Add("DatosUsuarioAMEC", datosUsuarioAMEC);
            Session.Add("DatosSolicitante", datosUsuarioSolicitante);
            Session.Add("UnidOrgXDefecto", unidOrgXDefecto);
            Session.Add("AccionUO", AccionUOCambioSolicitante);

            // Guarda el String de retorno la primera vez que carga la página
            SetBackString();
        }

        protected void CargarControles()
        {
            if (miAmec.idsolicitante.HasValue) ddlSolicitante.SelectedValue = miAmec.idsolicitante.ToString();
            AgenteAMEC agSol_AMEC = new AgenteAMEC(Int32.Parse(miAmec.idsolicitante.ToString()));
            txtCargo.Text = agSol_AMEC.ObtenerPosition();
            txtIdEstado.Text = miAmec.idestado.ToString();
            txtWein.Text = miAmec.nwein.ToString();
            txtNAmec.Text = miAmec.idamecs.ToString();

            lbidAMEC.InnerText = miAmec.idamecs.ToString();

            if (!string.IsNullOrWhiteSpace(miAmec.idamecsrelacionado))
            {
                lbidAMECClonado.InnerText = string.Format(" (Clonado-resometido de {0})", miAmec.idamecsrelacionado);
            }
            else
            {
                lbidAMECClonado.InnerText = string.Empty;
            }

            agAMECinfo = new AgenteAmecInfo();
            lbEstadoAMEC.Text = agAMECinfo.ObtenerNombreEstado(miAmec.idestado);
            if (miAmec.fechaamecs != null) { txtFecha.Text = String.Format("{0:dd/MM/yyyy}", miAmec.fechaamecs); }

            AgenteAMEC agUSU_AMEC = new AgenteAMEC(Int32.Parse(miAmec.idcreadopor.ToString()));
            txtCreadoPor.Text = agUSU_AMEC.ObtenerNombreUsuario();
            txtIdCreadoPor.Text = miAmec.idcreadopor.ToString();

            ddlActividad.SelectedValue = miAmec.idtipoactividad.ToString();
            ddlAgencias.SelectedValue = miAmec.idconfempresa.ToString();
            txtDescripcionAMEC.Text = miAmec.descripcion.ToString();

            // Obtiene un NumberFormatInfo asociado con la cultura es-ES (Español de España)
            if (miAmec.importegasto.HasValue) { txtImporteTotalGasto.Text = string.Format("{0:n2}", miAmec.importegasto); }

            //Departamento, fuerza de venta, distrito
            AgentePeticionarioManager agPeticionarioManager = new AgentePeticionarioManager();
            datosPeticionarioManager = agPeticionarioManager.ObtenerPeticionarioManager(miAmec.idsolicitante.GetHashCode());
            if (datosPeticionarioManager != null)
            {
                txtDepartamento.Text = datosPeticionarioManager.departament;
                txtDistrito.Text = datosPeticionarioManager.district;
                txtFuerzaVentas.Text = datosPeticionarioManager.saleforce;
            }

            //Aprobadores
            AgentePeticionarioManager agPetManager = new AgentePeticionarioManager();
            AgenteAprobadorAmec agAprobAmec = new AgenteAprobadorAmec();
            List<int> idAprobadores = agAprobAmec.ObernerIdsAprobadoresAmec(miAmec.idamecs.ToString());
            if (idAprobadores.Count > 0) LstManagers = agPetManager.ObtenerPeticionarioDesdeLista(idAprobadores);
        }

        public void HabilitaTodo()
        {
            AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
            if (miAmec.idestado == 5 && agAmecInfo.EstaGuardadoAmec(miAmec.idamecs))
            {
                //Los siguientes valores no se cargan la primera vez que se crea un amec, porque aún están en estado Nuevo Amec
                txtNAmec.Text = miAmec.idamecs.ToString();
                lbidAMEC.InnerText = miAmec.idamecs.ToString();

                if (!string.IsNullOrWhiteSpace(miAmec.idamecsrelacionado))
                {
                    lbidAMECClonado.InnerText = string.Format(" (Clonado-resometido de {0})", miAmec.idamecsrelacionado);
                }
                else
                {
                    lbidAMECClonado.InnerText = string.Empty;
                }

                agAMECinfo = new AgenteAmecInfo();
                lbEstadoAMEC.Text = agAMECinfo.ObtenerNombreEstado(miAmec.idestado);
            }
            HabilitaBotones();
        }

        public void HabilitaBotones()
        {
            AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
            AgenteDelegacion agDelegacion = new AgenteDelegacion();
            try
            {
                btnVolver.Visible = true;
                btnGuardar.Visible = false;
                ddlAgencias.Enabled = false;
                if (miAmec.idsolicitante == datosUsuarioAMEC.IdPeticionario && agAmecInfo.HayAmecRelacionExpediente(miAmec.idamecs) <= 0)
                {
                    btnGuardar.Visible = true;
                    ddlAgencias.Enabled = true;
                }                
            }
            catch (Exception ex)
            {
                EOSLogger.PrintError(this.GetType().Name, "HabilitaBotones", ex.Message, ex);
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
                throw ex;
            }
        }

        private void RellenarCombos()
        {
            AgenteMaestros agente = new AgenteMaestros();
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            AgenteFlujoAprobacion agenteFlujo = new AgenteFlujoAprobacion();
            AgenteAmecInfo agenteAmecInf = new AgenteAmecInfo();
            AgentePeticionarioManager agPeticionarioManager = new AgentePeticionarioManager();

            //AGENCIAS//
            this.ddlAgencias.DataSource = agente.ObtenerEmpresasConf();
            ddlAgencias.DataValueField = "idconfempresa";
            ddlAgencias.DataTextField = "nombreagencia";
            ddlAgencias.DataBind();
            ddlAgencias.SelectedIndex = 0;
            //FIN AGENCIAS//

            this.ddlSolicitante.DataSource = agenteUsu.ObtenerTodosUsuariosCombo(miAmec.idamecs);
            this.ddlSolicitante.DataBind();

            lvActividadAsigAmec.DataBind();
            ///////////////////////////////////////////////////////

            ICollection<DTipoActividadFlujo> datosFiltroActividad = null;
            datosFiltroActividad = agenteFlujo.ObtenerTipoActividad(miAmec != null ? miAmec.idtipoactividad.ToString() : "0", true);
            ddlActividad.DataSource = datosFiltroActividad;
            ddlActividad.DataBind();

            ICollection<DTipoRegistroActividadFlujo> datosFiltroTipoActividad = null;
            datosFiltroTipoActividad = agenteFlujo.ObtenerTipoRegistroActividad(miAmec != null ? miAmec.idtipoactividad.ToString() : "0", true);
            ddlTipoEvento.DataSource = datosFiltroTipoActividad;
            ddlTipoEvento.DataBind();
        }

        #endregion

        #region Botones

        protected void btnNuevoAMEC_Click(object sender, ImageClickEventArgs e)
        {
            GuardarNuevoAmec();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Guardando = true;
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            try
            {
                CargarValoreAmecFormulario();

                if (agAmecInf.EstaGuardadoAmec(miAmec.idamecs) == true)
                {
                    bool cambioAgencia = false;
                    ActualizarAmec(out cambioAgencia);
                }
                else
                {
                    GuardarNuevoAmec();

                }
                Alert.Show(string.Format("La información del AMEC: {0} ha sido guardada correctamente.", miAmec.idamecs));
            }
            catch (Exception ex)
            {
                EOSLogger.PrintError(this.GetType().Name, "btnGuardar_Click", ex.Message, ex);
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
                throw ex;
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e) { }

        protected void btnVerExcel_Click(object sender, ImageClickEventArgs e)
        {
            //S'ha de comprovar que l'Amec s'hagi guardat sinó s'ha guardat li preguntarem si vol guardar
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            if (agAmecInf.EstaGuardadoAmec(miAmec.idamecs))
            {
                Response.Redirect("PlantillaExportarAmecExcel.aspx?idamec=" + miAmec.idamecs);
            }
            else
            {
                Alert.Show("Tienes que Guardar el AMEC antes de Exportar a Excel");
            }
        }

        public void btnInformeAdicional_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
                if (agAmecInf.EstaGuardadoAmec(miAmec.idamecs))
                {
                    Response.Redirect("PlantillaExportarInformeAdicionalAmec.aspx?idamec=" + miAmec.idamecs);
                }
                else
                {
                    Alert.Show("Tienes que Guardar el AMEC antes de Exportar a Excel");

                }
            }
            catch (Exception ex)
            {
                EOSLogger.PrintError(this.GetType().Name, "btnInformeAdicional_Click", ex.Message, ex);
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
                throw ex;
            }
        }

        public void btnExportarInfoExpediente_Click(object sender, ImageClickEventArgs e)
        {
            AgenteAmecInfo agAMECinfo = new AgenteAmecInfo();
            int AmecRelacionadoConExpediente = agAMECinfo.HayAmecRelacionExpediente(miAmec.idamecs);
            if (AmecRelacionadoConExpediente != 0)
            {
                if (AmecRelacionadoConExpediente == 1)
                {
                    ImportarDatosExpediente(0);
                }
                else
                {
                }
            }
            else
            {
                Alert.Show("El AMEC no puede recuperar la información ya que no esta asociado a ningun expediente.");
            }

        }

        protected void imgEliminarAsignacionEvento_Command(object sender, CommandEventArgs e)
        {
            AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
            if (agAmecInfo.TieneExpedientesAsociados(Int32.Parse(e.CommandArgument.ToString())))
            {
                Alert.Show("No se puede eliminar el evento de un amec cuando ya tiene Expedientes aosciados.");
            }
            else
            {
            //Eliminar el Evento assignado al amec, asi que eliminar la fila dentro del dbo_iw_amec
            agAmecInfo.EliminarRelacionAmecCongreso(Int32.Parse(e.CommandArgument.ToString()));    
            }
            
        }

        #endregion

        #region Eventos

        protected void lvActividadAsigAmec_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Codi del lvActividadesAsigAmec
            lvActividadAsigAmec.DataBind();

        }

        protected void odsActividadesAsigAmec_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            RecuperaSesion();
            if (Page.IsPostBack) miAmec.idamecs = lbidAMEC.InnerText;
            if (!string.IsNullOrEmpty(miAmec.idamecs.ToString())) e.InputParameters["filtroIdAMEC"] = miAmec.idamecs;
        }

        #endregion

        #region "Funciones Auxiliares"

        public void itemXDefecto(DropDownList ddlnombre)
        {
            ListItem itemXdefecto = new ListItem();
            itemXdefecto.Value = "-1";
            itemXdefecto.Text = "Seleccionar";
            ddlnombre.Items.Add(itemXdefecto);
        }

        public void LimpiarSession()
        {
            Session.Remove("NuevoAMEC");
            Session.Remove("NuevoAgente");
            Session.Remove("DatosUsuarioAMEC");
            Session.Remove("DatosSolicitante");
            Session.Remove("UnidOrgXDefecto");
            Session.Remove("AccionUO");
        }

        public void MostrarPorTipoActividad(string TipoActividad, bool esCargaFormulario)
        {
            try
            {
                if (ddlActividad.Text == "9" || ddlActividad.Text == "17" || ddlActividad.Text == "18")
                {
                    plPaso1.Visible = false;

                }
                else
                {
                    plPaso1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                EOSLogger.PrintError(this.GetType().Name, "MostrarPorTipoActividad", ex.Message, ex);
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
                throw ex;
            }          

        }

        public void ImportarDatosExpediente(int idexpediente)
        {
            RepositorioFlujo estadoAMEC = null;
            estadoAMEC = new RepositorioFlujo(miAmec.idamecs, datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
            EOS.Entidades.InformacionExpediente InfoExpediente = estadoAMEC.IncorporarExpediente(idexpediente);
            //Impoorte
            txtImporteTotalGasto.Text = InfoExpediente.importe != 0 ? InfoExpediente.importe.ToString() : "0";

            //Gastos
            string GastosDesglose = "0";

            if (InfoExpediente.detalleservicios == null) return;

            foreach (Entidades.DetalleServicios detalleservicios in InfoExpediente.detalleservicios)
            {

                GastosDesglose = GastosDesglose + detalleservicios.descripcion;
            }
        }


        protected void ActualizarAmec(out bool cambioAgencia)
        {
            //A parte del Importe tendremos que comprobar que alguien no ha cambiado el estado del Amec (Otro usuario haya aprobado o rechazado el amec mientras tu estabas dentro)
            AgenteAmecInfo agAMECinfo = new AgenteAmecInfo();
            CargarValoreAmecFormulario();
            cambioAgencia = false;

            agAMECinfo.ModificarAMEC(miAmec, out cambioAgencia, false);
            HabilitaBotones();
        }

        protected void ObtenerAMEC(string idamec)
        {
            try
            {
                AgenteAmecInfo agenteAMEC = new AgenteAmecInfo();
                miAmec = agenteAMEC.CargarTodosValoresAmec(idamec);
            }
            catch (Exception ex)
            {
                EOSLogger.PrintError(this.GetType().Name, "ObtenerAMEC", ex.Message, ex);
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
                throw ex;
            }
        }

        protected void SeleccionaActividad(string sIdActividad, string sIdAMEC)
        {
            AgenteExpedientes agenteExp = new AgenteExpedientes();
            DCabeceraActividad miActividad = agenteExp.ObtenerActividadPorID(sIdActividad);
            //Ismael Ameller 22/02/2011 No se pueden crear expedientes de congresos ya pasados e inferior a dos días de la fecha actual          
            if (DateTime.Now > miActividad.Hasta)
            {
                Alert.Show("No se puede asignar el Amec a un eventos ya pasados");
            }
            else
            {
                if (miActividad.Hasta <= DateTime.Now.AddDays(2))
                {
                    Alert.Show("No se puede asignar el Amec a eventos que queden menos de 2 días");
                }
            }
        }

        protected void ImagenEstadoAMEC()
        {
            //TODO poner las imagenes correctas segun el Estado
            // Muestra el estado                             
            switch (miAmec.idestado)
            {
                case 1:
                    imgIconoEstado.ImageUrl = "~/Styles/images/ic_aceptado.png";
                    break;
                case 3:
                    imgIconoEstado.ImageUrl = "~/Styles/images/ic_estado_cancelado.png";
                    break;
                case 4:
                    imgIconoEstado.ImageUrl = "~/Styles/images/ic_estado_cancelado.png";
                    break;
                case 5:
                    imgIconoEstado.ImageUrl = "~/Styles/images/ic_enviado.png";
                    break;
                default:
                    imgIconoEstado.ImageUrl = "~/Styles/images/ic_en_proceso.png";
                    break;
            }

        }

        protected void RecuperaSesion()
        {
            if (Session["NuevoAgente"] != null) agAMEC = (AgenteAMEC)Session["NuevoAgente"];
            if (Session["NuevoAgente"] != null) agAMEC = (AgenteAMEC)Session["NuevoAgente"];
            if (Session["DatosUsuarioAMEC"] != null) datosUsuarioAMEC = (EOS.Entidades.Datos.DVPeticionariosRoles)Session["DatosUsuarioAMEC"];
            if (Session["DatosSolicitante"] != null) datosUsuarioSolicitante = (EOS.Entidades.Datos.DDatosPersonalesUsuario)Session["DatosSolicitante"];
            if (Session["UnidOrgXDefecto"] != null) unidOrgXDefecto = (DUnidadesOrganizativasAmec)Session["UnidOrgXDefecto"];
            if (Session["AccionUO"] != null) AccionUOCambioSolicitante = (bool)Session["AccionUO"];
        }

        /// <summary>
        /// Inserta un amec y devuelve uno nuevo
        /// </summary>
        protected void GuardarNuevoAmec()
        {
            AgenteAmecInfo agAMECinfo = new AgenteAmecInfo();

            CargarValoreAmecFormulario();
            miAmec = agAMECinfo.NuevoAMEC(miAmec);
            ImagenEstadoAMEC();
            HabilitaTodo();
        }

        protected string SetBackString()
        {
            /* La primera vez que entra en la página debe determinar la dirección de vuelta   
             * 1- Si viene del listado de AMECs tendrá en el querystring idamec 
             * 2- Si se ha pulsado el botón de Nuevo o se refresca la página no se debería perder el back original
             * 3- Si viene del expediente tendrá un querystring idcongresoactividad
             * 4- En cualquier otro caso redirigimos a listado de AMECs.
             * **/

            int nCaso = 4;
            if (Page.Request.UrlReferrer != null)
            {
                if (Page.Request.UrlReferrer.AbsoluteUri.Contains("ListadoAMECs.aspx"))
                {
                    // Caso 1: Viene de Listado de AMECs
                    nCaso = 1;
                }
                if (Page.Request.UrlReferrer.AbsoluteUri.Contains("DetalleAMEC.aspx"))
                {
                    // Caso 2: Ha pulsado el botón de nuevo o ha refrescado la página. No se debe perder el back original
                    nCaso = 2;
                }
                if (Page.Request.UrlReferrer.AbsoluteUri.Contains("NuevoExpedientePasoA.aspx"))
                {
                    // Caso 2: Ha pulsado el botón de nuevo o ha refrescado la página. No se debe perder el back original
                    nCaso = 3;
                }
            }
            else
            {
                // Caso 3:Qualquier otro caso.
                nCaso = 4;
            }
            return ComputeBackString(nCaso);
        }

        protected string ComputeBackString(int nCaso)
        {
            string sBackString = "";

            switch (nCaso)
            {
                case 1:
                    sURL = "ListadoAMECs.aspx";
                    break;
                case 2:
                    sURL = Page.Request.UrlReferrer.AbsoluteUri;
                    break;
                case 3:
                    sURL = string.Format("NuevoExpedientePasoA.aspx?idact={0}&idamec={1}", idcongreso, miAmec.idamecs);
                    break;
                case 4:
                    //Calquier otro caso
                    sURL = "ListadoAMECs.aspx";
                    break;

            }

            Session.Add("DetalleAMEC2_URLBackHistory", sURL);
            sBackString = string.Format("javascript:location.href = '{0}';return false;", sURL);
            this.btnVolver.OnClientClick = sBackString;
            return sURL;
        }

        #endregion
    }

}
