using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Entidades.Datos;
using EOS.Logica;
using EOS.Web;

namespace EOS
{
    //1702/2011 EAS.
    //public enum TipoActividad
    //{
    //    Inscripción,
    //    Transporte,
    //    Alojamiento,
    //    OtrosServicios,
    //    Ninguna
    //}

    public partial class Inscripcion : Page
    {
        public DCabeceraExpedienteAmpliado expediente
        {
            get
            {
                return (DCabeceraExpedienteAmpliado)HttpContext.Current.Session["currentExpediente"];
            }
            set
            {
                if (value != null) {
                    HttpContext.Current.Session["currentExpediente"] = value;
                    if (value.Idactividad != null) HttpContext.Current.Session["currentFKIdCongreso"] = value.Idactividad;
                }
            }
        }

        protected List<Control> AlojamientoControls
        {
            get
            {
                if (HttpContext.Current.Session["AlojamientoControls"] == null)
                    HttpContext.Current.Session["AlojamientoControls"] = new List<Control>();
                return (List<Control>)HttpContext.Current.Session["AlojamientoControls"];
            }
            set
            {
                HttpContext.Current.Session["AlojamientoControls"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) {
                if (Request.QueryString["idexp"] == null) {
                    throw new ArgumentNullException("idexp", "El argumento idexp con el número de expediente no a sido facilitado");
                }
                MainScriptManager.RegisterAsyncPostBackControl(this.InscripcionTabContainer);

                //aseguramos que el usuario tiene derechos ver estos expedientes
                //Xavier Morell
                //AgenteUsuarios agenteUsu = new AgenteUsuarios();
                //DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
                DVPeticionariosRoles datosRoles = (DVPeticionariosRoles)Session["rolesuser"];

                // Buscamos los datos básicos del expediente que se pasa por la url
                AgenteExpedientes agenteExp = new AgenteExpedientes();
                DCabeceraExpedienteAmpliado exp = agenteExp.ObtenerExpedientePorID(Request.QueryString["idexp"], datosRoles);


                if (exp == null) {
                    //no tiene derechos de ver esta inscripción
                    Alert.Show("Esta tratando de acceder a un expediente no válido", "Expedientes.aspx");
                } else {
                    this.expediente = exp;
                    this.btnSalir.HRef = string.Format("DetalleExpediente.aspx?idexp={0}", this.expediente.Idexpediente);

                    this.initInfoPanel();

                    //Xavier Morell (GP) 16-01-11
                    InscripcionTabContainer.Tabs[0].Enabled = (Request.QueryString["tab"] == "0" && this.expediente.Idtiporeserva != 2);
                    InscripcionTabContainer.Tabs[1].Enabled = (Request.QueryString["tab"] == "1" && this.expediente.Idtiporeserva != 2);
                    InscripcionTabContainer.Tabs[2].Enabled = (Request.QueryString["tab"] == "2" && this.expediente.Idtiporeserva != 2);
                    InscripcionTabContainer.Tabs[3].Enabled = (Request.QueryString["tab"] == "3");
                    /*
                     * erik bloem: InscripcionTabContainer.Tabs[0] no es buena implementación por razones de seguridad. No hay que 
                     * utilizar Request.QueryString, sino utilizar session parametros. Para ahora vale, pero se lo deberia cambiar cuando hay tiempo.
                     * //1702/2011 EAS. Codigo repetitivo  para eso esisten los InscripcionTabContainer.Tabs[0]
                        //erik bloem: saber que tipo no hemos elegido, tipo inscripción, transporte, alojamiento o otros servicios
                        TipoActividad tipoactividad = TipoActividad.Ninguna;
                        if (InscripcionTabContainer.Tabs[0].Enabled)
                            tipoactividad = TipoActividad.Inscripción;
                        else if (InscripcionTabContainer.Tabs[1].Enabled)
                            tipoactividad = TipoActividad.Alojamiento;
                        else if (InscripcionTabContainer.Tabs[2].Enabled)
                            tipoactividad = TipoActividad.Transporte;
                        else if (InscripcionTabContainer.Tabs[3].Enabled)
                            tipoactividad = TipoActividad.OtrosServicios;
                        Session["tipoactividad"] = tipoactividad;
                     */

                    #region  Otros Servicios Colectivos Qurius (EAS) 24/01/2011 y 01/02/2011
                    if (this.expediente.Idtiporeserva == 2 && (Request.QueryString["tab"] == "3")) {
                        InscripcionTabContainer.Tabs[4].Enabled = true;
                        InscripcionTabContainer.Tabs[4].Visible = true;
                        InscripcionTabContainer.ActiveTabIndex = 4;

                        InscripcionTabContainer.Tabs[0].Visible = false;
                        InscripcionTabContainer.Tabs[1].Visible = false;
                        InscripcionTabContainer.Tabs[2].Visible = false;
                        InscripcionTabContainer.Tabs[3].Visible = false;
                    } else {
                        InscripcionTabContainer.Tabs[4].Visible = false;
                        InscripcionTabContainer.Tabs[4].Enabled = false;
                        InscripcionTabContainer.ActiveTabIndex = 0;
                    }
                }
                #endregion
            }
        }

        /**
         * Inicializa el Panel de Información de la cabecera */
        private void initInfoPanel()
        {
            this.pedidoLabelValue.Text = this.expediente.Pedido;
            //Xavier Morell (GP) 16-01-11
            this.pedidoLabelExpediente.Text = this.expediente.Idexpediente.ToString();
            this.fechaExpedienteLabelValue.Text = this.expediente.Fechacreacion.ToString();

            this.lblFechaDesde.Text = string.Format("{0:dd/MM/yyyy }", expediente.FechaDesde);
            this.LblFechaHasta.Text = string.Format("{0:dd/MM/yyyy }", expediente.FechaHasta);
            this.LblPoblacion.Text = expediente.Poblacion;
 
            this.amecLabelValue.Text = this.expediente.Amec;
            this.congresoLabelValue.Text = this.expediente.Actividad;
            this.imgExpedienteDetalle.ToolTip = String.Format("Ver detalles del AMEC {0}", this.expediente.Idamec);
            this.imgExpedienteDetalle.PostBackUrl = Page.ResolveUrl(String.Format("DetalleAMEC.aspx?idamec={0}", this.expediente.Idamec));

            AgenteExpedientes agenteExp = new AgenteExpedientes();
            DAmec damec = agenteExp.ObtenerEntidadAMECporID(this.expediente.Idamec.ToString());
            AgenteMaestros am = new AgenteMaestros();
            if (damec.idconfempresa != null)
            {
                var empresaConf = am.ObtenerEmpresaConf(damec.idconfempresa.Value);
                lblAgencia.Text = empresaConf.nombreagencia;
            }
        }

        protected void InscripcionTabContainer_ActiveTabChanged(object sender, EventArgs e)
        {
            //this.adjustComponentsVisibility();
        }
    }
}