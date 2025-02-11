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

    public partial class DetalleAMECNewCo : Page
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

        public enum Estado { Aprobado = 47, Pendiente = 2, Cancelado = 48, Denegado = 4, Borrador = 5 };
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
                        txtNAmec.ReadOnly = true;
                        ObtenerAMEC(idamec); //OBTENER DATOS DEL AMEC
                        InicializaSesion();
                        InicializaCombos();
                        CargarControles();
                        HabilitaBotones();
                        ImagenEstadoAMEC();
                        MostrarPorTipoActividad(ddlActividad.Text, true);
                    }
                    else {
                        txtNAmec.ReadOnly = false;
                        //NUEVO AMEC 
                        //Se Puede Crear Un Nuevo Amec Viniendo del Expediente o viniendo del Boton Nuevo Amec del Menú
                        AgenteMaestros agente = new AgenteMaestros();
                        EOS.Entidades.Datos.DVPeticionariosRoles datosRoles = (EOS.Entidades.Datos.DVPeticionariosRoles)Session["rolesuser"];
                        
                        //Vienes del Boton del Menú
                        InicializaAMEC();
                        InicializaControles();
                        InicializaSesion();
                        HabilitaBotones(estadoAMEC);
                        ImagenEstadoAMEC();

                        //this.eosContentResults.Visible = true;
                        //lvActivid.DataBind();
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
            var tieneExpedientesAsociados = agAmecInfo.TieneExpedientesAsociadosPorIdAmecs(miAmec.idamecs);
            try
            {
                //BOTON APROBAR
                if (miAmec.idestado == 5 && agAmecInfo.EstaGuardadoAmec(miAmec.idamecs) && !string.IsNullOrWhiteSpace(miAmec.idamecs) && !tieneExpedientesAsociados)
                {
                    btnAprobar.Visible = true;
                }
                else
                {
                    btnAprobar.Visible = false;
                }

                //BOTON CANCELAR
                if (miAmec.idestado != 4 && miAmec.idestado != 48 && agAmecInfo.EstaGuardadoAmec(miAmec.idamecs) && !string.IsNullOrWhiteSpace(miAmec.idamecs) && !tieneExpedientesAsociados)
                {
                    btnCancelar.Visible = true;
                }
                else
                {
                    btnCancelar.Visible = false;
                }

                btnVolver.Visible = true;
                if (!tieneExpedientesAsociados)
                {
                    btnGuardar.Visible = true;
                    ddlAgencias.Enabled = true;
                    ddlSolicitante.Enabled = true;
                    ddlTipoEvento.Enabled = true;
                    txtDescripcionAMEC.Enabled = true;
                    txtImporteTotalGasto.Enabled = true;
                    txtFecha.Enabled = true;
                    btnCalendar.Visible = true;
                }
                else
                {
                    btnGuardar.Visible = false;
                    ddlAgencias.Enabled = false;
                    ddlSolicitante.Enabled = false;
                    ddlTipoEvento.Enabled = false;
                    txtDescripcionAMEC.Enabled = false;
                    txtImporteTotalGasto.Enabled = false;
                    txtFecha.Enabled = false;
                    btnCalendar.Visible = false;
                }

                if (miAmec.idestado == 4 || miAmec.idestado == 48)
                {
                    btnGuardar.Visible = false;
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
            ICollection<DTipoRegistroActividadFlujo> datosFiltroTipoActividad = null;
            datosFiltroTipoActividad = agenteFlujo.ObtenerTipoRegistroActividadNewCo("");
            ddlTipoEvento.DataSource = datosFiltroTipoActividad;
            ddlTipoEvento.DataBind();

            if (miAmec != null && !string.IsNullOrWhiteSpace(miAmec.idtipoactividad.ToString()))
            {
                ICollection<DTipoActividadFlujo> datosFiltroActividad = null;
                datosFiltroActividad = agenteFlujo.ObtenerTipoActividadPorTipoEvento(ddlTipoEvento.SelectedValue);
                if (miAmec.idtipoactividad > 0)
                {
                    ddlTipoEvento.SelectedValue = datosFiltroActividad.ToList().FirstOrDefault(x => x.idtipoactividad == miAmec.idtipoactividad).idtiporegistroactividad.ToString();
                    datosFiltroActividad = agenteFlujo.ObtenerTipoActividadPorTipoEvento(ddlTipoEvento.SelectedValue);
                    ddlActividad.DataSource = datosFiltroActividad;
                    ddlActividad.DataBind();
                    ddlActividad.Enabled = true;
                }
            }
            else
            {
                ddlActividad.Enabled = false;
            }
        }

        #endregion

        #region Botones

        protected void btnAprobar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                estadoAmecAnterior = Convert.ToInt32(miAmec.idestado);
                AgenteUsuarios agenteUsua = new AgenteUsuarios();
                AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
                RepositorioFlujo estadoAMEC = null;
                if (agAmecInf.EstaGuardadoAmec(miAmec.idamecs) == true)
                {
                    //AMEC ya insertado solo modificamos(UPDATE) el amec.
                    ActualizarAmec(false);
                }
                else
                {
                    //AMEC no insertado. Guardamos valores del amec e insertamos amec y la unidad organizativa por defecto.
                    GuardarNuevoAmec();
                }
                estadoAMEC = new RepositorioFlujo(miAmec.idamecs, (Int32)datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString("EOS.CadenaConexionSql"));
                txtIdEstado.Text = Convert.ToString(Convert.ToUInt32(Estado.Aprobado));
                //miAmec.idestado = Convert.ToUInt32(Estado.Cancelado);
                ActualizarAmec(true);
                int benguardathistorial = agAmecInf.GuardarEstadoAHistorialAMEC(miAmec.idamecs, Convert.ToInt32(miAmec.idestado), datosUsuarioAMEC.IdPeticionario, miAmec);


                ObtenerAMEC(miAmec.idamecs);

                string sNavegacion = "ListadoAMECs.aspx";
                Alert.Show(string.Format("Se ha Aprobado Correctamente el AMEC con Número: {0}", miAmec.idamecs), sNavegacion);

            }
            catch (Exception ex)
            {
                Alert.Show("No se ha podido Aprobar el AMEC debido a problemas internos");
            }

        }

        protected void btnCancelar_Click(object sender, ImageClickEventArgs e)
        {
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            if (agAmecInf.EstaGuardadoAmec(miAmec.idamecs) == true)
            {
                //AMEC ya insertado solo modificamos(UPDATE) el amec.
                ActualizarAmec(false);
            }
            else
            {
                //AMEC no insertado. Guardamos valores del amec e insertamos amec y la unidad organizativa por defecto.
                GuardarNuevoAmec();
            }
            AgenteExpedientes agExp = new AgenteExpedientes();
            //Jose Laguna 10-05-2012 10:00 cancelamos la forma de averiguar si tiene reservas asociadas o no porque no se relaciona con el expediente si no, con las reservas.
            //int NumExpedienteAsoc = agExp.NumeroExpedientesPorAmec(miAmec.idamecs);
            agAMEC.nIDAmec = miAmec.idamecs;
            if (!agAMEC.EsCancelable())
                Alert.Show("No se puede Cancelar el Amec porque tiene reservas asociadas en curso");
            else
            {
                txtIdEstado.Text = Convert.ToString(Convert.ToUInt32(Estado.Cancelado));
                //miAmec.idestado = Convert.ToUInt32(Estado.Cancelado);
                ActualizarAmec(true);
                int benguardathistorial = agAmecInf.GuardarEstadoAHistorialAMEC(miAmec.idamecs, Convert.ToInt32(miAmec.idestado), datosUsuarioAMEC.IdPeticionario, miAmec);

                string sNavegacion;
                if (sURL.Contains("NuevoExpedientePasoA.aspx")) sNavegacion = sURL;
                else sNavegacion = "ListadoAMECs.aspx";

                Alert.Show(string.Format("Se ha Cancelado el AMEC con Número: {0}", miAmec.idamecs), sNavegacion);
                //si es un amec cancelado no podrás utilizarlo para crear un Expediente No se tiene en cuenta
            }

        }

        protected void ActualizarAmec(bool evitarCheckEstado)
        {
            //A parte del Importe tendremos que comprobar que alguien no ha cambiado el estado del Amec (Otro usuario haya aprobado o rechazado el amec mientras tu estabas dentro)
            AgenteAmecInfo agAMECinfo = new AgenteAmecInfo();
            RepositorioFlujo estadoAMEC = null;
            CargarValoreAmecFormulario();
            if (ComprobarEstado(miAmec.idamecs, evitarCheckEstado) || evitarCheckEstado)
            {
                if (miAmec.idestado != 5 && !Sometiendo && !ComprobarImporte())
                {
                    Alert.Show("No se puede Guardar el Amec. Es Obligatorio Resometer debido a la modificación del Importe.");
                }
                else
                {
                    DAmecInfo DAmecInf = new DAmecInfo();
                    bool cambioAgencia = false;
                    DAmecInf = agAMECinfo.ModificarAMEC(miAmec, out cambioAgencia, false);

                    estadoAMEC = new RepositorioFlujo(miAmec.idamecs, (Int32)datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString("EOS.CadenaConexionSql"));
                    HabilitaBotones(estadoAMEC);
                }
            }
        }

        protected void btnNuevoAMEC_Click(object sender, ImageClickEventArgs e)
        {
            GuardarNuevoAmec();
        }

        protected void btnGuardar_Click(object sender, ImageClickEventArgs e)
        {
            //Guardando = true;
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            try
            {
                if (ValidarCampos() && ValidarCamposObligatorios(miAmec))
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
            }
            catch (Exception ex)
            {
                EOSLogger.PrintError(this.GetType().Name, "btnGuardar_Click", ex.Message, ex);
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
                throw ex;
            }
        }

        protected void btnVolver_Click(object sender, ImageClickEventArgs e) { }

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

        #region "Validaciones y Comprobaciones"

        public bool ValidarCamposObligatorios(DAmecInfo miAmec)
        {
            if (string.IsNullOrWhiteSpace(txtNAmec.Text)) {
                Alert.Show("Campo N.AMEC obligatorio");
                return false;
            }
            if (string.IsNullOrWhiteSpace(ddlTipoEvento.SelectedValue))
            {
                Alert.Show("Campo Tipo de Evento obligatorio");
                return false;
            }
            if (string.IsNullOrWhiteSpace(ddlActividad.SelectedValue) || ddlActividad.SelectedValue == "0" || ddlActividad.SelectedValue == "-1")
            {
                Alert.Show("Campo Tipo de Actividad obligatorio");
                return false;
            }
            if (txtDescripcionAMEC.Text == null || txtDescripcionAMEC.Text == "")
            {
                Alert.Show("Campo Descripción del Programa obligatorio");
                return false;
            }
            if (txtImporteTotalGasto.Text == null || txtImporteTotalGasto.Text == "")
            {
                Alert.Show("Campo Importe Gasto obligatorio");
                return false;
            }

            if (string.IsNullOrWhiteSpace(ddlAgencias.SelectedValue))
            {
                Alert.Show("Campo Tipo de Evento obligatorio");
                return false;
            }

            return true;
        }

        public bool ValidarNombreArchivo(string NombreArchivo)
        {
            //Validar que el nombre del archivo (Documentación o Programa) que se adjunta cumpla unos requisitos
            //Requisito 1: El nombre no puede ser superior a 85 carácteres
            //Requisito 2: El nombre no puede contener según que carácteres extraños ("!@#$%^&*ªº+=-:<>?")
            if (NombreArchivo.Count() > 85)
            {
                Alert.Show("El Nombre del Archivo es demasiado largo");
                return false;
            }

            var iChars = "!@#$%^&*ªº+=-:<>?'";
            for (var i = 0; i < NombreArchivo.Length; i++)
            {
                if (iChars.IndexOf(NombreArchivo.ElementAt(i)) != -1)
                {
                    Alert.Show("El Nombre del Archivo contiene carácteres no Aceptados en la aplicación !@#$%^&*ªº+=-:<>?'");
                    return false;
                }
            }

            return true;

        }

        public bool ValidarCampos()
        {
            decimal numberdec;    

            if (txtImporteTotalGasto.Text != "")
            {
                if (!Decimal.TryParse(txtImporteTotalGasto.Text, out numberdec))
                {
                    Alert.Show("El formato del campo: 'Importe Total Gasto' debe ser el correcto (un número).");
                    return false;
                }
            }

            return true;
        }

        public bool ComprobarEstado(string idamecs, bool evitarCheckEstado)
        {
            if (evitarCheckEstado)
            {
                return true;
            }
            //Comprobaremos si ha cambiado el estado mientras estabamos dentro del Amec/Formulario.
            AgenteAmecInfo agAmIn = new AgenteAmecInfo();
            uint idestadoguardado = agAmIn.ObtenerEstadoAmec(idamecs);
            if (miAmec.idestado != 3)
            {
                if (idestadoguardado.ToString() != miAmec.idestado.ToString())
                {
                    Alert.Show("El estado del Amec ha sido modificado y no se puede guardar");
                    return false;
                }
                else return true;
            }
            else return true;
        }

        public bool ComprobarImporte()
        {
            //Comprobamos si el importe a sido modificado con un valor superior al que está en la base de datos
            //Si es así i el amec es diferente de borrador el solicitante tendrá que resometer obligatoriamente
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            decimal importeActual = 0;

            if (txtImporteTotalGasto.Text != null && txtImporteTotalGasto.Text != "")
            {
                txtImporteTotalGasto.Text = txtImporteTotalGasto.Text.Replace(".", "");
                importeActual = Decimal.Parse(txtImporteTotalGasto.Text.Replace(",", "."), CultureInfo.InvariantCulture);
                txtImporteTotalGasto.Text = string.Format("{0:n2}", importeActual);
            }

            if (miAmec.idestado != 5)
            {
                DAmecInfo AmecGuardado = agAmecInf.CargarTodosValoresAmec(lbidAMEC.InnerText);
                if ((AmecGuardado != null))
                {
                    if (importeActual <= AmecGuardado.importegasto) return true;
                    else return false;
                }
                else return true;
            }
            else return true;
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

            agAMECinfo.ModificarAMEC(miAmec, out cambioAgencia,false);
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
            miAmec = agAMECinfo.NuevoAMECNewCo(miAmec);
            ImagenEstadoAMEC();
            HabilitaTodo();
        }

        protected void InicializaAMEC()
        {
            miAmec = new DAmecInfo();
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            AgenteAmecInfo agAMECinfo = new AgenteAmecInfo();
            datosUsuarioSolicitante = agenteUsu.ObtenerDatosPersonalesPorIDPeticionario(datosUsuarioAMEC.IdPeticionario.ToString());

            miAmec.idsolicitante = datosUsuarioSolicitante.IdPeticionario;
            miAmec.idcargo = datosUsuarioSolicitante.IdCargo;
            miAmec.idposition = datosUsuarioSolicitante.IdPosition;
            miAmec.idamecs = "0";
            //miAmec.idamecs = agAMECinfo.DameSiguienteIdAmecs();
            unidOrgXDefecto = new DUnidadesOrganizativasAmec(); //guardamos la unidad organizativa xdefecto del amec (la del solicitante).
            unidOrgXDefecto.idcreadopor = datosUsuarioSolicitante.IdPeticionario;
            unidOrgXDefecto.idarea = datosUsuarioSolicitante.Idarea;
            unidOrgXDefecto.idunidad = datosUsuarioSolicitante.idunidad;
            unidOrgXDefecto.idregion = datosUsuarioSolicitante.idregion;
            unidOrgXDefecto.iddistrito = datosUsuarioSolicitante.iddistrito;

            miAmec.idcreadopor = datosUsuarioAMEC.IdPeticionario; //OBLIGATORIO
            miAmec.fechaamecs = System.DateTime.Today; //OBLIGATORIO
            miAmec.nwein = datosUsuarioSolicitante.wein;  //miAmec.nwein = datosUsuarioAMEC.wein; //MODIFICADO 06/02/2012 
            miAmec.idestado = 5; //ESTADO BORRADOR porque es un nuevo AMEC.

        }

        protected void InicializaControles()
        {
            if (miAmec != null)
            {
                InicializaCombos();
                if (miAmec.idestado == 5) //ESTADO=5=BORRADOR por lo tanto es un NUEVO AMEC.
                {
                    btnCalendar.Enabled = false;
                    //Ocultar los comentarios
                    //JMM plAMEC_Historial.Visible = false;
                    AgenteAmecInfo agAMECinfo = new AgenteAmecInfo();
                    if (agAMECinfo.EstaGuardadoAmec(miAmec.idamecs))
                    {
                        lbEstadoAMEC.Text = agAMECinfo.ObtenerNombreEstado(miAmec.idestado);
                        lbidAMEC.InnerText = miAmec.idamecs.ToString();
                        txtNAmec.Text = miAmec.idamecs.ToString();
                    }
                    else
                    {
                        lbEstadoAMEC.Text = "NUEVO AMEC";
                        txtNAmec.Text = "";
                        lbidAMEC.InnerText = "0";
                    }
                    ImagenEstadoAMEC();
                    lbidAMEC.InnerText = miAmec.idamecs.ToString();

                    if (!string.IsNullOrWhiteSpace(miAmec.idamecsrelacionado))
                    {
                        lbidAMECClonado.InnerText = string.Format(" (Clonado-resometido a {0})", miAmec.idamecsrelacionado);
                    }
                    else
                    {
                        lbidAMECClonado.InnerText = string.Empty;
                    }

                    //JMM rblPreaprobMedico.SelectedValue = "1";
                    //JMM rblPreaprobNegocio.SelectedValue = "0";
                    //JMM rblPreaprovLegal_Comp.SelectedValue = "0";
                    //JMM rblFarmaindustria.SelectedValue = "0";
                    //JMM rblCasosClinicos.SelectedValue = "0";
                    //JMM rblpoliticaN20.SelectedValue = "1";
                    //JMM //this.txtFechaComienzo.Text = DateTime.Now.ToShortDateString();
                    //JMM ddlCriteriosSeleccion.SelectedValue = "3";
                    //JMM rblCasosClinicos.Attributes.Add("onclick", "checkControl('" + rblCasosClinicos.ClientID + "'," + rblCasosClinicos.Items.Count + ")");
                    //JMM rblFarmaindustria.Attributes.Add("onclick", "MostrarDescFarmaIndustria('" + rblFarmaindustria.ClientID + "'," + rblFarmaindustria.Items.Count + ")");
                }

                ddlSolicitante.SelectedValue = datosUsuarioSolicitante.IdPeticionario.ToString();
                //obtener cargo de los datosroles.
                txtCargo.Text = agAMEC.ObtenerCargo();

                txtWein.Text = datosUsuarioAMEC.wein;
                txtFecha.Text = String.Format("{0:dd/MM/yyyy}", miAmec.fechaamecs);
                txtCreadoPor.Text = agAMEC.ObtenerNombreUsuario();
                txtIdCreadoPor.Text = miAmec.idcreadopor.ToString();
                txtIdEstado.Text = miAmec.idestado.ToString();

            }
        }

        public void HabilitaBotones(RepositorioFlujo estadoAmec)
        {
            AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
            if (estadoAmec != null && agAmecInfo.EstaGuardadoAmec(miAmec.idamecs))
            {

                //BOTON GUARDAR
                if ((miAmec.idcreadopor == datosUsuarioAMEC.IdPeticionario && miAmec.idestado == 5) || miAmec.idsolicitante == datosUsuarioAMEC.IdPeticionario) btnGuardar.Visible = true;
                else btnGuardar.Visible = false;
                //////////////////////////////////////////////
            }
            else
            {
                //Aquí entra cuando entras/creas el Amec por primera vez
                btnVolver.Visible = true;
                btnGuardar.Visible = true;
            }

            if (miAmec.idestado == 4 || miAmec.idestado == 48)
            {
                btnGuardar.Visible = false;
            }
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

        public void ddlTipoEvento_SelectedIndexChanged(object sender, EventArgs e)
        {
            AgenteMaestros agente = new AgenteMaestros();
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            AgenteFlujoAprobacion agenteFlujo = new AgenteFlujoAprobacion();
            AgenteAmecInfo agenteAmecInf = new AgenteAmecInfo();
            AgentePeticionarioManager agPeticionarioManager = new AgentePeticionarioManager();

            ddlActividad.Items.Clear();

            List<DTipoActividadFlujo> datosFiltroActividad = new List<DTipoActividadFlujo>();
            datosFiltroActividad.Add(new DTipoActividadFlujo() { tipoactividad = "Selecciona Tipo de Actividad" });
            datosFiltroActividad.AddRange(agenteFlujo.ObtenerTipoActividadPorTipoEvento(ddlTipoEvento.SelectedValue).ToList());
            ddlActividad.DataSource = datosFiltroActividad;
            ddlActividad.DataBind();

            if (string.IsNullOrWhiteSpace(ddlTipoEvento.SelectedValue))
            {
                ddlActividad.Enabled = false;
            }
            else
            {
                ddlActividad.Enabled = true;
            }
            ddlActividad.SelectedValue = "0";


        }

        public void ddlSolicitante_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* cuando el combo de Solicitante se seleccione entonces se carga el campo CARGO (del solicitante). */
            miAmec.idsolicitante = Convert.ToInt32(ddlSolicitante.SelectedValue);

            //OBTENEMOS LOS DATOS PERSONALES DEL SOLICITANTE/RESPONSABLE del usuario.
            AgenteUsuarios agenteSolicitante = new AgenteUsuarios();
            datosUsuarioSolicitante = agenteSolicitante.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());


            //CAMBIAMOS la unidad organizativa por defecto del solicitante al cambiar el solicitante.
            unidOrgXDefecto.idarea = datosUsuarioSolicitante.Idarea;
            unidOrgXDefecto.idcreadopor = datosUsuarioSolicitante.IdPeticionario;
            unidOrgXDefecto.idarea = datosUsuarioSolicitante.Idarea;
            unidOrgXDefecto.idunidad = datosUsuarioSolicitante.idunidad;
            unidOrgXDefecto.idregion = datosUsuarioSolicitante.idregion;
            unidOrgXDefecto.iddistrito = datosUsuarioSolicitante.iddistrito;
            miAmec.idcargo = datosUsuarioSolicitante.IdCargo;
            AgenteAMEC agSol_AMEC = new AgenteAMEC(datosUsuarioSolicitante.IdPeticionario);
            txtCargo.Text = agSol_AMEC.ObtenerCargo();

            //Cambio nwein
            miAmec.nwein = datosUsuarioSolicitante.wein;
            txtWein.Text = miAmec.nwein;

            //TODO s'ha de recollir les dades del Formulari
            //Session.Add("NuevoAMEC", miAmec);
            Session.Add("DatosSolicitante", datosUsuarioSolicitante);
            Session.Add("UnidOrgXDefecto", unidOrgXDefecto);

            // Cargar nuevamente drop down list y la listview de la unidad org por defecto.
            //Departamento, fuerza de venta, distrito
            AgentePeticionarioManager agPeticionarioManager = new AgentePeticionarioManager();
            datosPeticionarioManager = agPeticionarioManager.ObtenerPeticionarioManager(miAmec.idsolicitante.GetHashCode());
            if (datosPeticionarioManager != null)
            {
                txtDepartamento.Text = datosPeticionarioManager.departament;
                txtDistrito.Text = datosPeticionarioManager.district;
                txtFuerzaVentas.Text = datosPeticionarioManager.saleforce;
            }
            AccionUOCambioSolicitante = true;

        }

        protected void ActualizaCombosUnidadOrgXDefecto()
        {
            //if (datosUsuarioSolicitante.idunidad.HasValue)
            //{
            //    // Si tiene idunidad la seleccionamos en el combo.
            //    ddlUnidad.SelectedValue = datosUsuarioSolicitante.idunidad.ToString();
            //}

            //if (datosUsuarioSolicitante.Idarea.HasValue)
            //{
            //    ddlArea.SelectedValue = datosUsuarioSolicitante.Idarea.ToString();
            //}
            //else
            //{
            //    ddlArea.SelectedValue = "0";
            //}


            //if (datosUsuarioSolicitante.idregion.HasValue)
            //{
            //    ddlRegion.SelectedValue = datosUsuarioSolicitante.idregion.ToString();
            //}
            //else
            //{
            //    ddlRegion.SelectedValue = "0";
            //}

            //if (datosUsuarioSolicitante.iddistrito.HasValue)
            //{
            //    ddlDistrito.SelectedValue = datosUsuarioSolicitante.iddistrito.ToString();
            //}

            //else
            //{
            //    ddlDistrito.SelectedValue = "0";
            //}
        }

        public void UpdateUnidadOrganizativaXDefecto()
        {
            ////MARTA MESTRE UPDATE UNIDAD ORGANIZATIVA SI EXISTE
            //AgenteUnidadesOrgAMEC agUnidad = new AgenteUnidadesOrgAMEC();
            //AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            //if (agAmecInf.EstaGuardadoAmec(miAmec.idamecs))
            //{
            //    //UPDATE DE LA UNIDAD ORGANIZATIVA
            //    if (miAmec.paraguas != true || chkParaguas.Checked != true)
            //    {
            //        ICollection<DUnidadesOrganizativasAmec> unidadesOrgAMEC = agUnidad.ObtenerUnidadesOrgAmec(miAmec.idamecs.ToString(), miAmec.idsolicitante.ToString(), "", "", "", "", "true", "", "", "", "", "", 0, 10);
            //        DUnidadesOrganizativasAmec unidadxdefecto = unidadesOrgAMEC.ElementAt<DUnidadesOrganizativasAmec>(0);
            //        unidOrgXDefecto.idunidadamec = unidadxdefecto.idunidadamec;
            //        agUnidad.ModificarUnidadOrgId(unidOrgXDefecto);
            //    }
            //}
            //else
            //{
            //    //MARTA JUEVES2
            //    if (ddlArea.SelectedValue == "0" || ddlArea.SelectedValue == "-1")
            //    {
            //        unidOrgXDefecto.idarea = null;
            //    }
            //    else
            //    {
            //        unidOrgXDefecto.idarea = Int32.Parse(ddlArea.SelectedValue.ToString());
            //    }



            //    if (ddlRegion.SelectedValue == "0" || ddlRegion.SelectedValue == "-1")
            //    {
            //        unidOrgXDefecto.idregion = null;
            //    }
            //    else
            //    {
            //        unidOrgXDefecto.idregion = Int32.Parse(ddlRegion.SelectedValue.ToString());
            //    }



            //    if (ddlDistrito.SelectedValue == "0" || ddlDistrito.SelectedValue == "-1")
            //    {
            //        unidOrgXDefecto.iddistrito = null;
            //    }
            //    else
            //    {
            //        unidOrgXDefecto.iddistrito = Int32.Parse(ddlDistrito.SelectedValue.ToString());
            //    }

            //    unidOrgXDefecto.idcreadopor = Int32.Parse(ddlSolicitante.SelectedValue.ToString());
            //    unidOrgXDefecto.idunidad = Int32.Parse(ddlUnidad.SelectedValue.ToString());


            //    //unidOrgXDefecto.idcreadopor = Int32.Parse(ddlSolicitante.SelectedValue.ToString());
            //    //unidOrgXDefecto.idarea =  Int32.Parse(ddlArea.SelectedValue.ToString());
            //    //unidOrgXDefecto.idunidad = Int32.Parse(ddlUnidad.SelectedValue.ToString());
            //    //unidOrgXDefecto.idregion = Int32.Parse(ddlRegion.SelectedValue.ToString());
            //    //unidOrgXDefecto.iddistrito = Int32.Parse(ddlDistrito.SelectedValue.ToString());          
            //}

            //lvUnidadesOrg.DataBind();
        }
    }

}
