using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Web;
using EOS.Entidades.Datos;
using System.Data;
using EOS.Entidades;
//using NPOI.HSSF.Record.Formula.Functions;

namespace EOS
{
    public partial class BuscadorGestorInvitados : Page
    {
        private Entidades.DetalleExpediente dExpediente;
        private ListaPP ListaProductoPorcentaje = new ListaPP();
        private static IList<DVProductos> ListaTodosProductos = new List<DVProductos>();
        private static IList<DCentroCoste> ListaCentroCoste = new List<DCentroCoste>();
        private int contador = 1;
        private readonly string AmecReunionesInternas = ConfigUtil.GetAppSetting("AmecReunionesInternas");

        private const string ID_KEY_PART_REMOVED = "ParticipantesEliminados_";
        private const string ID_KEY_HONORARIOS = "HonorariosPassenger_";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ///////////////Controlar Si caduca la sesión en la aplicación//////////////
                Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));

                if (Session["rolesuser"] == null)
                    Response.Redirect("~/Account/Login.aspx");
                //////////////////////////////////////////////////////////////////////////

                string sIdExp = Request.QueryString["idexp"];
                string tipo = Request.QueryString["tipo"];

                //Si es 1 es individual, si es 2 es colectivo, si es calculadora 3
                if (tipo == "1")
                {
                    btnContinuar.Visible = true;
                    btnGuardar.Visible = false;

                    var calculadoraItem = tipoPagoDropDownList.Items.FindByValue("3");
                    calculadoraItem.Enabled = false;
                }
                else if (tipo == "2")
                {
                    btnGuardar.Visible = true;
                    btnContinuar.Visible = false;

                    var calculadoraItem = tipoPagoDropDownList.Items.FindByValue("3");
                    calculadoraItem.Enabled = false;
                }
                else if (tipo == "3")
                {
                    btnContinuar.Visible = true;
                    btnGuardar.Visible = false;

                    plPaso1.Visible = false;
                    plPaso2.Visible = false;
                    var defaultItem = tipoPagoDropDownList.Items.FindByValue("0");
                    defaultItem.Enabled = false;
                    var ordenItem = tipoPagoDropDownList.Items.FindByValue("1");
                    ordenItem.Enabled = false;
                    var tarjetaItem = tipoPagoDropDownList.Items.FindByValue("2");
                    tarjetaItem.Enabled = false;
                    var calculadoraItem = tipoPagoDropDownList.Items.FindByValue("3");
                    calculadoraItem.Enabled = true;
                    divTipoPago.Attributes["Style"] = "display:none";
                }

                Form.DefaultButton = lvActividades.SelectedValue == null ? btnFiltrar.UniqueID : btnContinuar.UniqueID;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);

                if (Session["NuevoExpediente"] != null)
                {
                    dExpediente = (Entidades.DetalleExpediente)Session["NuevoExpediente"];
                    if (dExpediente != null && dExpediente.AMEC != null && dExpediente.AMEC.idconfempresa.HasValue)
                    {
                        hdnIdConfEmpresa.Value = dExpediente.AMEC.idconfempresa.Value.ToString();
                    }
                }
                else
                {
                    AgenteExpedientes agenteExpediente = new AgenteExpedientes();
                    var expediente = agenteExpediente.ObtenerExpedientePorID(sIdExp);
                    if (expediente != null && expediente.Amec != null && expediente.idconfempresa.HasValue)
                    {
                        hdnIdConfEmpresa.Value = expediente.idconfempresa.Value.ToString();
                    }

                    dExpediente = new Entidades.DetalleExpediente();
                    if (tipo == "3")
                    {
                        SeleccionaActividad(EOS.ServiceLogic.Variables.CalculadoraIdPeticionActividadDefault, null, out int? idconfempresakk);
                    }
                    Session.Add("NuevoExpediente", dExpediente);
                }

                if (Session["contador"] != null)
                {
                    contador = (int)Session["contador"];
                }
                else
                {
                    contador = 1;
                    Session.Add("contador", contador);
                }

                DDatosPersonalesUsuario datosUsuario = (DDatosPersonalesUsuario)Session["datosUsuario"];
                

                /* Hay 5 formas de llegar a esta página:
                 * 
                 * 1- Desde el botón de Nuevo Expediente. IdExp=0
                 * 2- Desde el botón de Crear Nueva Actividad, después de crearla la selecciona
                 * 3- Desde el botón de Copiar Desde Expediente
                 * 4- Desde el botón de Cancelar en el PasoB de selección de participantes
                 * 5- Desde el botón de Modificar desde la pantalla de destalle de Expediente. IdExp=N, donde N es el Id válido del expediente
                 * 6- Desde NuevoDetalleAMEC donde se ha creado un nuevo amec.
                 * */

                //Jose Laguna 19-02-2013 a través del webconfig, 
                //parametrizamos la herramienta y miramos si esta aplicación requiere o no control de presupuestos "AMEC" 
                //o si simplemente se hace mediante un control de texto

                if (System.Configuration.ConfigurationManager.AppSettings["controlPresupuesto"] == "0")
                {
                    ddlAmecPorCongreso.Enabled = false;
                    ddlAmecPorCongreso.Visible = false;
                    searchAmec.Enabled = false;
                    searchAmec.Visible = false;
                    txtAreaNuevoAMEC.Visible = true;
                    //imgNuevoFlujoAprobacion.Visible = false;
                    DetalleAmec.Visible = false;
                }
                //FIN Jose Laguna 19-02-2013 parametrización

                if (!Page.IsPostBack)
                {
                    if (dExpediente != null && dExpediente.nIDExpediente != 0)
                    {
                        AgenteExpedientes agExp = new AgenteExpedientes();
                        DCabeceraExpedienteAmpliado dCabExp1 = agExp.ObtenerExpedientePorID(dExpediente.nIDExpediente.ToString());
                        if (dCabExp1.Idestado == "NC")
                            DeshabilitarCamposExpediente();

                    }
                    if (sIdExp != null)
                    {
                        int nIDExp;
                        int.TryParse(sIdExp, out nIDExp);

                        if (nIDExp <= 0)
                        {
                            // Se trata de un nuevo expediente, por lo que inicializamos todas las variables
                            dExpediente = new Entidades.DetalleExpediente();
                            Session.Add("NuevoExpediente", dExpediente);
                            //btnCancelar.OnClientClick = "javascript:document.location.href = 'Expedientes.aspx';return false";
                        }
                        else
                        {
                            // Se trata de un expediente ya existente. Habría que recuperar sus valores asociados de la BBDD
                            AgenteExpedientes agExp = new AgenteExpedientes();
                            DCabeceraExpedienteAmpliado dCabExp = agExp.ObtenerExpedientePorID(sIdExp);

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

                                RellenaDatosExpediente(dCabExp.idconfempresa);

                                //Si el expediente es Individual o colectivo y está en estado diferente de borrador solo se podrà modificar
                                //segun que campos
                                if (dCabExp.Idestado == "NC")
                                    DeshabilitarCamposExpediente();

                            }

                            //btnCancelar.OnClientClick = string.Format("javascript:document.location.href = 'DetalleExpediente.aspx?idexp={0}';return false", dExpediente.nIDExpediente);
                        }
                    }

                    // Caso 2: Si acaba de crear una nueva actividad, la debe poner como la actividad del expediente
                    string sIdAct = Request.QueryString["idact"];
                    string sIdAMEC = Request.QueryString["idamec"];

                    int? idconfempresa = null;
                    if (sIdAct != null && sIdAMEC == null)
                    {
                        SeleccionaActividad(sIdAct, sIdAMEC, out idconfempresa);
                    }

                    // Caso 3: Acaba de copiar desde otro expediente, hay que configurar el botón de cancelar para que vaya a detalle
                    if (Request.QueryString["back"] != null)
                    {
                        //btnCancelar.OnClientClick = "javascript:document.location.href = 'Expedientes.aspx';return false";
                    }

                    // Caso 4: Vuelve desde la pantalla de selección de participantes y por tanto debe eliminar éstos y repintarse
                    string sIdBack = Request.QueryString["idback"];

                    if (sIdBack != null)
                    {
                        dExpediente.dParticipantes = null;
                        RellenaDatosExpediente(null);
                    }

                    // Caso 6: Se ha Creado un nuevo AMEC, que está asignado a un evento, por lo tanto tendremos dos valores por querystring
                    idconfempresa = null;
                    if (sIdAct != null && sIdAMEC != null)
                    {

                        SeleccionaActividad(sIdAct, sIdAMEC, out idconfempresa);
                        RellenaDatosExpediente(idconfempresa);
                    }

                    // Otras tareas rutinarias
                    RellenarCombos(idconfempresa);

                    //Ismael Ameller 22-03-2011 Control de productos
                    if (sIdExp != null)
                        LoadListaProductoPorcentaje(sIdExp);
                    else if (dExpediente.nIDExpediente != null)
                        LoadListaProductoPorcentaje(dExpediente.nIDExpediente.ToString());
                    //CalculaPorcentajeRestante();
                    //FIN Ismael Ameller 22-03-2011 Control de productos



                    ControlDeProductos();
                }
                else
                {
                    ListaProductoPorcentaje = (ListaPP)Session["ListaProductosPorcentaje"];
                }

                eventosFormsCnt.Visible = false;
                //InicializaGridProductos();

                if (!IsPostBack)
                {
                    //AGENCIAS//
                    AgenteMaestros agente = new AgenteMaestros();
                    var dataSourceAgencias = agente.ObtenerEmpresasConf().ToList();
                    this.ddlAgencias.DataSource = dataSourceAgencias;
                    ddlAgencias.DataValueField = "idconfempresa";
                    ddlAgencias.DataTextField = "nombreagencia";
                    ddlAgencias.DataBind();
                    ddlAgencias.SelectedValue = dataSourceAgencias.First(x => x.idconfempresa != 0).idconfempresa.ToString();
                    //FIN AGENCIAS//
                }

                if (tipo == "3")
                {
                    SeleccionaActividad(EOS.ServiceLogic.Variables.CalculadoraIdPeticionActividadDefault, null, out int? idconfempresakk);
                    plPaso1.Visible = false;
                    plPaso2.Visible = false;
                }

            }
            catch (Exception ex)
            {
                EOSLogger.PrintError(this.GetType().Name, "Page_Load", ex.Message, ex);
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
            }
        }

        private bool EstaIdAreaProductoEmpresaInactivo(string idAreaProductoEmpresa)
        {
            try
            {
                AgenteExpedientes ag = new AgenteExpedientes();
                return ag.EstaIdAreaProductoEmpresaInactivo(idAreaProductoEmpresa);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        private void ControlDeProductos()
        {
            try
            {
                if (ListaProductoPorcentaje.Count > 0)
                {
                    if (EstaIdAreaProductoEmpresaInactivo(ListaProductoPorcentaje[0].IdAreaProductoEmpresa.ToString()))
                    {
                        ListItem listItemDefault1 = new ListItem();
                        listItemDefault1.Value = "-2";
                        listItemDefault1.Text = "Asociado a Producto Anterior";
                        ddltiposDeProductos.Items.Insert(0, listItemDefault1);
                        ddltiposDeProductos.SelectedValue = "-2";
                    }
                    else
                    {
                        ddltiposDeProductos.SelectedValue = ListaProductoPorcentaje[0].IdAreaProductoEmpresa.ToString();
                    }

                }
                else
                {
                    ListItem listItemDefault = new ListItem();
                    listItemDefault.Value = "-1";
                    listItemDefault.Text = "Selecciona";
                    ddltiposDeProductos.Items.Insert(0, listItemDefault);
                    ddltiposDeProductos.SelectedValue = "-1";
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        private void RellenarCombos(int? idconfempresa)
        {
            DDatosPersonalesUsuario datosUsuario = (DDatosPersonalesUsuario)Session["datosUsuario"];
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
            AgenteMaestros agente = new AgenteMaestros();
            ddlTipoActividad.DataSource = agente.ObtenerTiposActividad("(Todas)", datosRoles.administrador, datosRoles.newco);
            ddlTipoActividad.DataBind();

            ddlPoblacion.DataSource = agente.ObtenerPoblaciones("(Todas)", idconfempresa ?? datosUsuario.IdConfEmpresa ?? 1);
            ddlPoblacion.DataBind();

            //Ismael Ameller 22-03-2011 Control de productos
            //Xavier Morell
            //AgenteUsuarios agenteUsu = new AgenteUsuarios();
            //DDatosPersonalesUsuario datosUsuario = agenteUsu.ObtenerDatosPersonalesPorLogin();

            ListaTodosProductos.Clear();
            ListaTodosProductos = agente.ObtenerProductos(datosUsuario.admin.HasValue ? null : datosUsuario.Idarea);
            ddltiposDeProductos.DataSource = ListaTodosProductos;
            //ddltiposDeProductos.DataSource = agente.ObtenerProductos(datosUsuario.Idarea);
            ddltiposDeProductos.DataTextField = "Producto";
            ddltiposDeProductos.DataValueField = "idareaProducto";
            ddltiposDeProductos.DataBind();

            ListaCentroCoste.Clear();
            ListaCentroCoste = agente.ObtenerCentrosCoste("").ToList();
            if (!string.IsNullOrEmpty(dExpediente.sPedido))
            {
                var centroExists = ListaCentroCoste.ToList().Find(x => x.CentroCoste == dExpediente.sPedido);
                if (centroExists == null)
                {
                    ListaCentroCoste.Add(new DCentroCoste() { CentroCoste = dExpediente.sPedido });
                }
            }
            else
            {
                ddlPedido.SelectedValue = "Selecciona";
            }
            ListaCentroCoste = ListaCentroCoste.OrderBy(x => x.CentroCoste).ToList();
            ListaCentroCoste.Insert(0, new DCentroCoste() { IdCentroCoste = -1, CentroCoste = "Selecciona" });
            ddlPedido.DataSource = ListaCentroCoste;
            ddlPedido.DataBind();

            //FIN Ismael Ameller 22-03-2011 Control de productos
            txtFechaDesde.Text = DateTime.Now.ToShortDateString();
        }

        private void DeshabilitarCamposExpediente()
        {
            try
            {
                btnCambiarActividad.Enabled = false;
                DetalleAmec.Enabled = false;
                txtAreaNuevoAMEC.Enabled = false;
                ddlAmecPorCongreso.Enabled = false;
                searchAmec.Enabled = false;
                chkUrgente.Enabled = false;
                //imgNuevoFlujoAprobacion.Enabled = false;

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            eosContentResults.Visible = true;
            
            lvActividades.DataBind();
        }

        protected void odsActividades_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim())) e.InputParameters["filtroNombre"] = "%" + txtNombre.Text.Trim() + "%";
            if (!string.IsNullOrEmpty(ddlPoblacion.SelectedValue.Trim())) e.InputParameters["filtroPoblacion"] = ddlPoblacion.SelectedValue.Trim();
            //if (!string.IsNullOrEmpty(txtAMEC.Text.Trim())) e.InputParameters["filtroAMEC"] = "%" + txtAMEC.Text.Trim() + "%";
            if (!string.IsNullOrEmpty(ddlTipoActividad.SelectedValue.Trim())) e.InputParameters["filtroTipoActividad"] = ddlTipoActividad.SelectedValue.Trim();
            if (!string.IsNullOrEmpty(txtFechaDesde.Text.Trim()))
            {
                DateTime dtTest;
                if (DateTime.TryParse(txtFechaDesde.Text.Trim(), out dtTest))
                    e.InputParameters["filtroFechaDesde"] = txtFechaDesde.Text.Trim();
                else
                    txtFechaDesde.Text = "";
            }
            if (!string.IsNullOrEmpty(txtFechaHasta.Text.Trim()))
            {
                DateTime dtTest;
                if (DateTime.TryParse(txtFechaHasta.Text.Trim(), out dtTest))
                    e.InputParameters["filtroFechaHasta"] = txtFechaHasta.Text.Trim();
                else
                    txtFechaHasta.Text = "";

            }

            e.InputParameters["isGestorInvitados"] = true;
        }

        protected void lvActividades_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvActividades.SelectedValue != null)
            {
                int? confempresa = null;
                SeleccionaActividad(lvActividades.SelectedValue.ToString(), null, out confempresa);

            }
            //Ismael Ameller 08-03-2011 Cambio del foco una vez hemos elegido una actividad
            Form.DefaultButton = btnContinuar.UniqueID;
            //FIN Ismael Ameller 08-03-2011 Cambio del foco una vez hemos elegido una actividad
        }

        protected void SeleccionaActividad(string sIdActividad, string sIdAMEC, out int? idconfempresa)
        {
            idconfempresa = null;
            AgenteExpedientes agenteExp = new AgenteExpedientes();
            DCabeceraActividad miActividad = agenteExp.ObtenerActividadPorID(sIdActividad);

            //Ismael Ameller 22/02/2011 No se pueden crear expedientes de congresos ya pasados e inferior a dos días de la fecha actual     
            //DEQ 28/06/2018: Quitamos de manera temporal la restricción de crear expedientes de congresos ya pasados
            //DEQ 12/02/2019: Vuelve a ponerse la restricción de crear expedientes de congresos ya pasados

            if (DateTime.Now > miActividad.Desde)
            {
                Alert.Show("No se pueden crear expedientes de eventos ya pasados");
            }
            else
            {
                //LJM 09/01/2018: Quitamos la validación de no poder crear expedientes para eventos que queden menos de 2 días
                //LJM 06/04/2018: Volvemos a poner la validación de no poder crear expedientes para eventos que queden menos de 2 días
                //DEQ 26/06/2018: Volvemos a quitar la validación de no poder crear expedientes para eventos que queden menos de 2 días
                //DEQ 12/02/2019: Volvemos a poner la validación de no poder crear expedientes para eventos que queden menos de 2 días

                if (miActividad.Desde <= DateTime.Now.AddDays(2))
                {
                    Alert.Show("No se pueden crear expedientes para eventos que queden menos de 2 días");
                }
                else
                {
                    DAmec miAMEC = null;

                    if (!string.IsNullOrEmpty(sIdAMEC))
                    {
                        miAMEC = agenteExp.ObtenerEntidadAMECporID(sIdAMEC);
                    }

                    if (string.IsNullOrEmpty(sIdAMEC))
                    {
                        if (dExpediente.nIDExpediente == 0 && !String.IsNullOrEmpty(Request.QueryString["idexp"]) && Request.QueryString["idexp"] != "0")
                        {
                            Response.Redirect(Request.Url.ToString(), false);
                        }

                        idconfempresa = miActividad.idconfempresa;
                        hdnIdConfEmpresa.Value = miActividad.idconfempresa.ToString();
                        dExpediente.nIDCogreso = miActividad.IdCongreso;
                        dExpediente.esCongreso = int.Parse(miActividad.EsCongreso.ToString());
                        dExpediente.sCongreso = miActividad.Congreso;
                        dExpediente.AMEC = miAMEC;
                        dExpediente.nIDAMEC = (miAMEC != null && miAMEC.idamec > 0) ? miAMEC.idamec.ToString() : "0";
                        RellenaDatosExpediente(miActividad.idconfempresa);
                    }

                    btnContinuar.Focus();
                }
            }
            //FIN Ismael Ameller 22/02/2011 No se pueden crear expedientes de congresos ya pasados e inferior a dos días de la fecha actual 
        }

        protected void btnCambiarActividad_Click(object sender, EventArgs e)
        {
            AgenteExpedientes agExp = new AgenteExpedientes();
            ICollection<DVAmecCongresoConcatSolicitante> DVAmecCongreConcatSolicitante = agExp.ObtenerAMECPorCongresoConcatSolicitante(0, "0", null);
            ddlAmecPorCongreso.DataSource = DVAmecCongreConcatSolicitante;
            ddlAmecPorCongreso.DataBind();
            selectedIdAmecs.Value = "";
            selectedAmecs.Value = "";
            searchAmec.Text = "";

            dExpediente.nIDAMEC = "0";
            dExpediente.AMEC = null;
            dExpediente.nIDCogreso = 0;
            dExpediente.sCongreso = "";
            txtIdCongreso.Text = "";
            txtCongreso.Text = "";
            //Carlos Serra
            lnkbtnGRUPOS.Visible = false;

            plPaso1.Visible = true;
            plPaso2.Visible = false;
        }

        protected void btnNuevoAmec_Click(object sender, ImageClickEventArgs e)
        {
            if (dExpediente.nIDCogreso == 0)
            {
                ShowMessage("El Congreso es un campo requerido. Complete el paso 1. Seleccione Actividad antes de crear un Amec Nuevo", null);
            }
            else
            {
                Response.Redirect("DetalleAMEC.aspx?idcongresoactividad=" + dExpediente.nIDCogreso);
            }

        }

        private void crearNuevoAmec()
        {
            AgenteExpedientes agExp = new AgenteExpedientes();

            DAmec miAMEC = new DAmec { amec = Convert.ToString(dExpediente.nIDAMEC) };
            if (dExpediente.esCongreso == 0) { miAMEC.idpeticionactividad = dExpediente.nIDCogreso; }
            else { miAMEC.IdCongreso = dExpediente.nIDCogreso; }

            DDatosPersonalesUsuario datosUsuarioAMEC = (DDatosPersonalesUsuario)Session["datosUsuario"];
            miAMEC.IdEmpresa = datosUsuarioAMEC.FKIdEmpresa;

            AgenteAmecInfo agenteAMEC = new AgenteAmecInfo();
            var miAmecDDBB = agenteAMEC.CargarTodosValoresAmec(selectedIdAmecs.Value);
            if (miAmecDDBB != null)
            {
                miAMEC.newco = miAmecDDBB.newco;
                miAMEC.idconfempresa = miAmecDDBB.idconfempresa;
            }
            else
            {
                miAMEC.newco = ((DVPeticionariosRoles)Session["DatosRol"]).newco;
                miAMEC.idconfempresa = int.Parse(ddlAgencias.SelectedValue);
            }
            dExpediente.nIDAMEC = agExp.NuevoAMEC(miAMEC).ToString();
            dExpediente.AMEC = miAMEC;

        }

        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            bool bRedirect = false;
            AgenteExpedientes agExp = new AgenteExpedientes();

            //FIN Ismael Ameller 22-03-2011 Control de productos
            if (!ComprobarCamposOK()) return;

            if (ddltiposDeProductos.SelectedValue != "-2")
            {
                //Cambio Albert Ventura, ahora solo quieren un producto por expediente pero Mariló me comenta que dejamos la misma metodologia por si
                // en algún momento se vuelve a lo anterior.
                ListaPP ListaProductoPorcentaje1 = new ListaPP();
                ProductoPorcentaje itemPP = new ProductoPorcentaje
                {
                    IdAreaProductoEmpresa = Convert.ToInt32(ddltiposDeProductos.SelectedItem.Value),
                    Producto = ddltiposDeProductos.SelectedItem.Text,
                    Porcentaje = "100"
                };
                ListaProductoPorcentaje1.Add(itemPP);
                Session["ListaProductosPorcentaje"] = ListaProductoPorcentaje1;
            }
            else
                Session["ListaProductosPorcentaje"] = ListaProductoPorcentaje;

            if (dExpediente != null)
            {
                //Ya existe el expediente
                dExpediente.bUrgente = chkUrgente.Checked;
                dExpediente.sPedido = !string.IsNullOrEmpty(ddlPedido.SelectedValue) && ddlPedido.SelectedValue != "Selecciona" ? ddlPedido.SelectedValue : null; //!string.IsNullOrEmpty(txtPedido.Text.Trim()) ? txtPedido.Text.Trim() : null;
                dExpediente.tipoPagoFee = int.Parse(tipoPagoDropDownList.SelectedValue);
                bRedirect = (dExpediente.nIDAMEC != "0" && !string.IsNullOrWhiteSpace(dExpediente.nIDAMEC)) || (dExpediente.nIDAMEC != "0" && !string.IsNullOrWhiteSpace(dExpediente.nIDAMEC)) && dExpediente.AMEC != null && dExpediente.AMEC.amec == txtAreaNuevoAMEC.Text;
            }

            if (searchAmec.Enabled)
            {
                AgenteAmecInfo agAmecInf = new AgenteAmecInfo();

                if (dExpediente != null)
                    dExpediente.nIDAMEC = selectedIdAmecs.Value;

                string NumAmec = selectedIdAmecs.Value;
                bool estaGuardadoAmec = agAmecInf.EstaGuardadoAmec(NumAmec);

                if (estaGuardadoAmec)
                {
                    DAmecInfo miAmecNuevo = agAmecInf.CargarTodosValoresAmec(NumAmec);
                    if (dExpediente != null)
                    {
                        dExpediente.idAmecViejo = selectedIdAmecs.Value;
                        dExpediente.nIDAMEC = miAmecNuevo.idamecs;
                        dExpediente.AMECNuevo = miAmecNuevo;
                        dExpediente.idAmecNuevo = selectedIdAmecs.Value;
                        //Si este amec tiene una relación con el congreso no crearemos nada sino crearemos una Nuevo Amec (Tabla AMEC)
                        int IdAmecAsignadoCongreso = agAmecInf.IdAmecAsignadoCongreso(dExpediente.idAmecNuevo, dExpediente.nIDCogreso);

                        if (IdAmecAsignadoCongreso == 0)
                        {
                            crearNuevoAmec();
                        }
                        else
                        {
                            dExpediente.AMEC = agExp.ObtenerEntidadAMECporID(Convert.ToString(IdAmecAsignadoCongreso));
                            dExpediente.nIDAMEC = dExpediente.AMEC.idamec.ToString();
                        }
                    }
                }
                else
                {
                    if (dExpediente != null)
                    {
                        dExpediente.nIDAMEC = NumAmec;
                        int IdAmecAsignadoCongreso1 = agAmecInf.IdAmecAsignadoCongreso(NumAmec, dExpediente.nIDCogreso);
                        DAmec miAmec = new DAmec();
                        if (IdAmecAsignadoCongreso1 == 0)
                        {
                            crearNuevoAmec();
                            miAmec = agExp.ObtenerEntidadAMECporID(Convert.ToString(dExpediente.nIDAMEC));
                        }
                        else
                            miAmec = agExp.ObtenerEntidadAMECporID(Convert.ToString(IdAmecAsignadoCongreso1));

                        dExpediente.nIDAMEC = miAmec.idamec.ToString();
                        dExpediente.AMEC = miAmec;
                    }
                }

                bRedirect = true;
            }
            else if (txtAreaNuevoAMEC.Visible)
            {
                //TODO
                if (dExpediente != null)
                    dExpediente.nIDAMEC = txtAreaNuevoAMEC.Text;

                string NumAmec = txtAreaNuevoAMEC.Text;

                if (dExpediente != null)
                {
                    dExpediente.nIDAMEC = NumAmec;
                    crearNuevoAmec();
                    DAmec miAmec = agExp.ObtenerEntidadAMECporID(Convert.ToString(dExpediente.nIDAMEC));
                    dExpediente.nIDAMEC = miAmec.idamec.ToString();
                    dExpediente.AMEC = miAmec;
                }

                bRedirect = true;
            }

            //PASO B
            if (!bRedirect)
                return;

            Response.Redirect("NuevoExpedientePasoB.aspx");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            AgenteExpedientes agExp = new AgenteExpedientes();

            //FIN Ismael Ameller 22-03-2011 Control de productos
            if (!ComprobarCamposOK()) return;

            if (ddltiposDeProductos.SelectedValue != "-2")
            {
                //Cambio Albert Ventura, ahora solo quieren un producto por expediente pero Mariló me comenta que dejamos la misma metodologia por si
                // en algún momento se vuelve a lo anterior.
                ListaPP ListaProductoPorcentaje1 = new ListaPP();
                ProductoPorcentaje itemPP = new ProductoPorcentaje
                {
                    IdAreaProductoEmpresa = Convert.ToInt32(ddltiposDeProductos.SelectedItem.Value),
                    Producto = ddltiposDeProductos.SelectedItem.Text,
                    Porcentaje = "100"
                };
                ListaProductoPorcentaje1.Add(itemPP);
                Session["ListaProductosPorcentaje"] = ListaProductoPorcentaje1;
            }
            else
                Session["ListaProductosPorcentaje"] = ListaProductoPorcentaje;

            if (dExpediente != null)
            {
                //Ya existe el expediente
                dExpediente.bUrgente = chkUrgente.Checked;
                dExpediente.sPedido = !string.IsNullOrEmpty(ddlPedido.SelectedValue) && ddlPedido.SelectedValue != "Selecciona" ? ddlPedido.SelectedValue : null; //!string.IsNullOrEmpty(txtPedido.Text.Trim()) ? txtPedido.Text.Trim() : null;
                dExpediente.tipoPagoFee = int.Parse(tipoPagoDropDownList.SelectedValue);

            }

            if (searchAmec.Enabled)
            {
                AgenteAmecInfo agAmecInf = new AgenteAmecInfo();

                if (dExpediente != null)
                    dExpediente.nIDAMEC = selectedIdAmecs.Value;

                string NumAmec = selectedIdAmecs.Value;
                bool estaGuardadoAmec = agAmecInf.EstaGuardadoAmec(NumAmec);

                if (estaGuardadoAmec)
                {
                    DAmecInfo miAmecNuevo = agAmecInf.CargarTodosValoresAmec(NumAmec);
                    if (dExpediente != null)
                    {
                        dExpediente.idAmecViejo = selectedIdAmecs.Value;
                        dExpediente.nIDAMEC = miAmecNuevo.idamecs;
                        dExpediente.AMECNuevo = miAmecNuevo;
                        dExpediente.idAmecNuevo = selectedIdAmecs.Value;
                        //Si este amec tiene una relación con el congreso no crearemos nada sino crearemos una Nuevo Amec (Tabla AMEC)
                        int IdAmecAsignadoCongreso = agAmecInf.IdAmecAsignadoCongreso(dExpediente.idAmecNuevo, dExpediente.nIDCogreso);

                        if (IdAmecAsignadoCongreso == 0)
                        {
                            crearNuevoAmec();
                        }
                        else
                        {
                            dExpediente.AMEC = agExp.ObtenerEntidadAMECporID(Convert.ToString(IdAmecAsignadoCongreso));
                            dExpediente.nIDAMEC = dExpediente.AMEC.idamec.ToString();
                        }
                    }
                }
                else
                {
                    if (dExpediente != null)
                    {
                        dExpediente.nIDAMEC = NumAmec;
                        int IdAmecAsignadoCongreso1 = agAmecInf.IdAmecAsignadoCongreso(NumAmec, dExpediente.nIDCogreso);
                        DAmec miAmec = new DAmec();
                        if (IdAmecAsignadoCongreso1 == 0)
                        {
                            crearNuevoAmec();
                            miAmec = agExp.ObtenerEntidadAMECporID(Convert.ToString(dExpediente.nIDAMEC));
                        }
                        else
                            miAmec = agExp.ObtenerEntidadAMECporID(Convert.ToString(IdAmecAsignadoCongreso1));

                        dExpediente.nIDAMEC = miAmec.idamec.ToString();
                        dExpediente.AMEC = miAmec;
                    }
                }

            }
            else if (txtAreaNuevoAMEC.Visible)
            {
                //TODO
                if (dExpediente != null)
                    dExpediente.nIDAMEC = txtAreaNuevoAMEC.Text;

                string NumAmec = txtAreaNuevoAMEC.Text;

                if (dExpediente != null)
                {
                    dExpediente.nIDAMEC = NumAmec;
                    crearNuevoAmec();
                    DAmec miAmec = agExp.ObtenerEntidadAMECporID(Convert.ToString(dExpediente.nIDAMEC));
                    dExpediente.nIDAMEC = miAmec.idamec.ToString();
                    dExpediente.AMEC = miAmec;
                }

            }


            //Si se trata de un expediente nuevo lo insertamos. En cualquier otro caso debería ser una modificación
            if (dExpediente != null)
            {
                try
                {

                    DDatosPersonalesUsuario datosUsuario = (DDatosPersonalesUsuario)Session["datosUsuario"];

                    int nIDPeticionario = 0;
                    if (datosUsuario != null)
                    {
                        nIDPeticionario = datosUsuario.IdPeticionario;
                    }

                    AgenteExpedientes agente = new AgenteExpedientes();
                    AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
                    int idamec;
                    int idEmpresa;
                    if (dExpediente.AMEC != null)
                    {
                        idEmpresa = dExpediente.AMEC.IdEmpresa;
                        idamec = dExpediente.AMEC.idamec;
                    }
                    else
                    {
                        idEmpresa = 1257;//L'única empresa que hi ha i en aquest moment el Amec nou te cap idempresa a la seva taula
                        //TODO: esto peta si no es numérico el amec
                        idamec = int.Parse(dExpediente.idAmecViejo);
                    }
                    int nIdTipoReserva = 2;

                    //Antes cuando se modificaba un Expediente el Estado siempre era AB (Borrador) ahora también existen el estado NC (En curso)
                    string estado;
                    if (dExpediente.nIDExpediente == 0)
                        estado = "AB";
                    else
                        estado = agente.GetEstado(dExpediente.nIDExpediente);
                    int nNuevoExpediente = agente.NuevoExpediente(datosUsuario.idregion,
                        idamec,
                        datosUsuario.idunidad,
                        datosUsuario.Idarea,
                        datosUsuario.IdPeticionario,
                        nIdTipoReserva,
                        idEmpresa,
                        estado,
                        datosUsuario.iddistrito,
                        System.DateTime.Now,
                        datosUsuario.idempleadogp,
                        dExpediente.sPedido,
                        dExpediente.tipoPagoFee,
                        dExpediente.bUrgente,
                        new List<DVParticipante>(),
                        new List<int>(),
                        new DataTable(), dExpediente.nIDExpediente, datosUsuario.iddepartament, datosUsuario.idsaleforce, datosUsuario.iddistrict);

                    agente.ClearProductosExpediente(nNuevoExpediente);

                    //Ismael Ameller 22-03-2011 Agrega la lista de productos
                    foreach (ProductoPorcentaje itempp in ListaProductoPorcentaje)
                    {
                        ExpAreaProductoEmpresa entidad = new ExpAreaProductoEmpresa();
                        entidad.idExpediente = nNuevoExpediente;
                        entidad.idAreaProductoEmpresa = itempp.IdAreaProductoEmpresa;
                        entidad.porcentaje = Convert.ToInt32(itempp.Porcentaje);
                        entidad.locked = 0;
                        agente.AgregaProductoExpedienteArea(entidad);
                    }
                    //Honorarios
                    if (dExpediente.nIDExpediente != 0)
                    {
                        if (listparticipantesEliminados.Any())
                        {
                            //Tendremos que comprobar si el Expediente es nuevo entonces no habrá ningún honorario creado en la base de datos.
                            agente.EliminarHonorariosPassenger(dExpediente.nIDExpediente, listparticipantesEliminados);
                        }
                        agente.GuardarHonorariosPassenger(dExpediente.nIDExpediente, dtHonorariosPassangerCreados);
                    }
                    else
                    {
                        agente.GuardarHonorariosPassenger(nNuevoExpediente, dtHonorariosPassangerCreados);
                    }
                    //FIN Ismael Ameller 22-03-2011 Agrega la lista de productos
                    if (dExpediente.nIDExpediente == 0)
                    {
                        Alert.Show(string.Format("Se ha creado un nuevo expediente con Identificador: {0}", nNuevoExpediente), agente.MsgNavegacion(nNuevoExpediente));
                    }
                    else
                    {
                        Alert.Show(string.Format("Se han modificado correctamente los datos del expediente: {0}", dExpediente.nIDExpediente), agente.MsgNavegacion(dExpediente.nIDExpediente));
                    }
                    listparticipantesEliminados.Clear();
                    dtHonorariosPassangerCreados.Clear();
                }
                catch (Exception ex)
                {
                    //Ismael Ameller 09-03-2011 Envio de Mail
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                    //FIN Ismael Ameller 09-03-2011 Envio de Mail
                    Alert.Show("Se ha producido un error en la grabación del Expediente", null);
                }
            }
        }

        public List<int> listparticipantesEliminados
        {
            get
            {
                List<int> sAux = new List<int>();
                if (ViewState[ID_KEY_PART_REMOVED + Session.SessionID] != null)
                {
                    sAux = (List<int>)ViewState[ID_KEY_PART_REMOVED + Session.SessionID];
                }

                ViewState[ID_KEY_PART_REMOVED + Session.SessionID] = sAux;

                return sAux;
            }
            set
            {
                ViewState[ID_KEY_PART_REMOVED + Session.SessionID] = value;
            }
        }

        public DataTable dtHonorariosPassangerCreados
        {
            get
            {
                DataTable sAux = new DataTable();
                if (ViewState[ID_KEY_HONORARIOS + Session.SessionID] != null)
                {
                    sAux = (DataTable)ViewState[ID_KEY_HONORARIOS + Session.SessionID];
                }

                ViewState[ID_KEY_HONORARIOS + Session.SessionID] = sAux;

                return sAux;
            }
            set
            {
                ViewState[ID_KEY_HONORARIOS + Session.SessionID] = value;
            }
        }

        protected void RellenaDatosExpediente(int? idconfempresa)
        {
            ICollection<DVAmecCongresoConcatSolicitante> DVAmecCongreConcatSolicitante = null;
            if (dExpediente != null)
            {
                AgenteExpedientes agExp = new AgenteExpedientes();
                DVAmecCongreConcatSolicitante = agExp.ObtenerAMECPorCongresoConcatSolicitante(dExpediente.nIDCogreso, AmecReunionesInternas, idconfempresa);

                if (DVAmecCongreConcatSolicitante.Count != 0)
                {
                    ddlAmecPorCongreso.ClearSelection();
                    ddlAmecPorCongreso.Items.Clear();
                    ddlAmecPorCongreso.DataSource = DVAmecCongreConcatSolicitante;
                    ddlAmecPorCongreso.DataBind();
                    ListItem blankUnidad = new ListItem("Selecciona un Amec", "0");
                    ddlAmecPorCongreso.Items.Insert(0, blankUnidad);
                    ddlAmecPorCongreso.SelectedValue = "0";

                    searchAmec.Enabled = true;

                    if (Request.QueryString["idamec"] != null)
                    {
                        foreach (DVAmecCongresoConcatSolicitante dameccongr in DVAmecCongreConcatSolicitante.Where(dameccongr => dameccongr.idamecs == Request.QueryString["idamec"]))
                        {
                            selectedIdAmecs.Value = dameccongr.idamecs;
                            selectedAmecs.Value = dameccongr.AmecConcatSolicitante;
                            searchAmec.Text = dameccongr.AmecConcatSolicitante;
                        }
                    }
                }
            }

            if (dExpediente != null)
            {
                if (DVAmecCongreConcatSolicitante != null && dExpediente.AMEC != null)
                {
                    foreach (DVAmecCongresoConcatSolicitante dameccongr in DVAmecCongreConcatSolicitante.Where(dameccongr => dameccongr.idamecs == dExpediente.AMEC.amec))
                    {
                        selectedIdAmecs.Value = dameccongr.idamecs;
                        selectedAmecs.Value = dameccongr.AmecConcatSolicitante;
                        searchAmec.Text = dameccongr.AmecConcatSolicitante;
                    }
                }

                // Valores del Congreso
                if (dExpediente.nIDCogreso > 0)
                {
                    txtIdCongreso.Text = Convert.ToString(dExpediente.nIDCogreso);
                }

                if (!string.IsNullOrEmpty(dExpediente.sCongreso))
                {
                    txtCongreso.Text = dExpediente.sCongreso;
                }

                if (!string.IsNullOrEmpty(dExpediente.sPedido))
                {
                    //txtPedido.Text = dExpediente.sPedido;
                    ddlPedido.SelectedValue = dExpediente.sPedido;
                }
                else
                {
                    if (string.IsNullOrEmpty(ddlPedido.SelectedValue))
                    {
                        ddlPedido.SelectedValue = "Selecciona";
                    }
                }

                if (dExpediente.tipoPagoFee.HasValue)
                {
                    tipoPagoDropDownList.SelectedValue = dExpediente.tipoPagoFee.Value.ToString();
                }
                /*else
                {
                    tipoPagoDropDownList.SelectedValue = "0";
                }*/

                chkUrgente.Checked = dExpediente.bUrgente;
                plPaso1.Visible = (dExpediente.nIDCogreso <= 0);
                plPaso2.Visible = (dExpediente.nIDCogreso > 0);

                if (dExpediente.tipoPagoFee == 3 || Request.QueryString["tipo"] == "3")
                {
                    plPaso1.Visible = false;
                    plPaso2.Visible = false;
                }

                //encripta en MD5 IdUsuario y IdEvento y los guarda en el LinkButton "lnkbtnGRUPOS" para que posteriormente pueda redireccionar a GRUPOS via POST.
                EncryptParamsGRUPOS();
            }
        }

        private void EncryptParamsGRUPOS()
        {
            //encripta el idevento en MD5.
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(Convert.ToString(dExpediente.nIDCogreso)));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            string nIDCogresoEncrypt = sBuilder.ToString();

            //encripta el idusuario en MD5.
            md5Hash = MD5.Create();
            DDatosPersonalesUsuario datosUsuario = (DDatosPersonalesUsuario)Session["datosUsuario"];
            data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(Convert.ToString(datosUsuario.IdPeticionario)));
            sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            string IdPeticionarioEncrypt = sBuilder.ToString();

            //elimina previamente los atributos.
            lnkbtnGRUPOS.Attributes.Remove("IdUser");
            lnkbtnGRUPOS.Attributes.Remove("IdEvent");

            //guarda en el LinkButton "lnkbtnGRUPOS" el IdUsuario y IdEvento.
            lnkbtnGRUPOS.Attributes.Add("IdUser", IdPeticionarioEncrypt);
            lnkbtnGRUPOS.Attributes.Add("IdEvent", nIDCogresoEncrypt);

            lnkbtnGRUPOS.Visible = true;
        }

        // Carlos Serra
        protected void lnkbtnGRUPOS_Click(object sender, EventArgs e)
        {
            //guarda el IdUsuario y IdEvento para poder pasarlos via POST a GRUPOS.
            ImageButton button = sender as ImageButton;

            if (button != null)
            {
                List<string> lstparams = button.AlternateText.Split('|').ToList();
                Session["idCongresoEForms"] = lstparams[0];
                Session["idconfempresaEForms"] = lstparams[1];
            }

            eventosFormsCnt.Visible = true;
            lvEventosForms.DataBind();
        }

        protected void odsEventosForms_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["FiltroID"] = Session["idCongresoEForms"];
        }

        // OGP "26/09/2012"
        protected void EventosFormsHeader_Click(object sender, EventArgs e)
        {
            eventosFormsCnt.Visible = true;
        }

        // OGP "19/09/2012"
        protected void lvEventosForms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvEventosForms.SelectedValue != null)
            {
                DVPeticionariosRoles rolesUser = Session["rolesUser"] as DVPeticionariosRoles ?? new DVPeticionariosRoles();
                NameValueCollection data = new NameValueCollection
                                               {
                                                   {"IdEvento", lvEventosForms.SelectedValue.ToString()},
                                                   {"IdUser", rolesUser.login},
                                                   {"EosToken", rolesUser.password}
                                               };

                AgenteMaestros agente = new AgenteMaestros();
                DEmpresaConf empresa = agente.ObtenerEmpresasConf().ToList().Find(x => x.idconfempresa == dExpediente.AMEC.idconfempresa);

                RedirectAndPOST(Page, empresa.ruta_formulario_grupos, data);
            }
        }

        //Carlos Serra
        public static void RedirectAndPOST(Page page, string destinationUrl, NameValueCollection data)
        {
            string strForm = PreparePOSTForm(destinationUrl, data);

            page.Controls.Add(new LiteralControl(strForm));
        }

        //Carlos Serra Prepara un formulario HTML donde guarda los datos como campos ocultos y construye un javascript que realizará el POST.
        private static String PreparePOSTForm(string url, NameValueCollection data)
        {
            const string formID = "PostForm";
            StringBuilder strForm = new StringBuilder();
            strForm.AppendFormat("<form id=\"{0}\" name=\"{0}\" action=\"{1}\" method=\"POST\">", formID, url);

            foreach (string key in data)
            {
                strForm.AppendFormat("<input type=\"hidden\" name=\"{0}\" value=\"{1}\">", key, data[key]);
            }

            strForm.Append("</form>");

            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language=\"javascript\">");
            strScript.AppendFormat("var v{0} = document.{0};", formID);
            strScript.AppendFormat("v{0}.submit();", formID);
            strScript.Append("</script>");

            return strForm.Append(strScript).ToString();
        }


        private bool ComprobarCamposOK()
        {
            const bool ok = true;

            if (dExpediente.nIDCogreso == 0)
            {
                ShowMessage("El Congreso es un campo requerido. Complete el paso 1. Seleccione Actividad", null);
                return false;
            }

            //if (string.IsNullOrWhiteSpace(txtPedido.Text.Trim()))
            //{
            //    ShowMessage("El NetworkActivity/Centro de Coste es un campo requerido. Complete el paso 1. Introduzca NetworkActivity/Centro de Coste", null);
            //    return false;
            //}

            if (ddlPedido.SelectedItem.Value == "Selecciona")
            {
                ShowMessage("El NetworkActivity/Centro de Coste es un campo requerido. Complete el paso 1. Introduzca NetworkActivity/Centro de Coste", null);
                return false;
            }

            if (tipoPagoDropDownList.SelectedValue == "0")
            {
                ShowMessage("El Tipo de Pago es un campo requerido. Complete el paso 1. Introduzca Tipo de Pago", null);
                return false;
            }

            if ((selectedIdAmecs.Value == null || selectedIdAmecs.Value == ""/*ddlAmecPorCongreso.Items.Count == 0 || ddlAmecPorCongreso.SelectedItem.Value == "0"*/) && txtAreaNuevoAMEC.Text == "")
            {
                ShowMessage("El " + System.Configuration.ConfigurationManager.AppSettings["codigoPresupuesto"] + " es un campo requerido. Complete el paso 2", null);
                return false;
            }

            //Tiene que funcionar con el proceso antiguo y con el nuevo que solo funciona con 3 productos
            if (ddltiposDeProductos.SelectedItem.Value == "-1")
            {
                ShowMessage("Vinculado A es un campo requerido. Complete el paso 3", null);
                return false;
            }

            if (plhShowAgency.Visible && ddlAgencias.SelectedItem.Value == "0")
            {
                ShowMessage("La Agencia es un campo requerido. Complete el paso 2", null);
                return false;
            }

            return ok;
        }


        private void ShowMessage(string message, string navigation)
        {
            // Muestra mensaje y navega a una URL especificada
            navigation = (navigation == null ? "" : ";document.location='" + navigation + "'");
            string script = String.Format("alert('{0}');{1}", message, navigation);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "dialog", script, true);
        }

        protected void imgDetalleAmec_Click(object sender, EventArgs e)
        {
            if (selectedIdAmecs.Value != "" && selectedIdAmecs.Value != "0")
            {
                AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
                string[] vtxt = selectedAmecs.Value.Split(' ');
                string NumAmec = vtxt[0];
                dExpediente.nIDAMEC = NumAmec;
                bool estaGuardadoAmec = agAmecInf.EstaGuardadoAmec(NumAmec);
                if (!estaGuardadoAmec)
                {
                    Alert.Show("No Se puede mostrar el Detalle del Amec Seleccionado");
                }
                else
                {
                    var amec = agAmecInf.CargarTodosValoresAmec(NumAmec);
                    string link = string.Format("{0}.aspx?idcongresoactividad={1}&idamec={2}", amec.veeva ? "DetalleAMECVeeva" : vtxt[0].ToCharArray()[0] == '4' ? "DetalleAMEC" : "NuevoDetalleAMEC", dExpediente.nIDCogreso, NumAmec);
                    Response.Redirect(link);
                }
            }
            else
            {
                Alert.Show("No Se puede mostrar el Detalle del Amec Seleccionado");
            }
        }

        private IEnumerable<DProductoPorcentajeVista> ProductosExpedienteItems(string idExp)
        {
            AgenteExpedientes agExp = new AgenteExpedientes();
            return agExp.ProductosExpedienteItems(idExp);
        }

        private void LoadListaProductoPorcentaje(string idExp)
        {
            if (idExp != null && !idExp.Equals("0"))
            {
                var ppItems = ProductosExpedienteItems(idExp);
                ListaProductoPorcentaje.AddRange(ppItems.Select(item => new ProductoPorcentaje
                {
                    Producto = item.Producto,
                    Porcentaje = Convert.ToString(item.Porcentaje),
                    IdAreaProductoEmpresa = item.IdareaProductoempresa
                }));
            }

            Session["ListaProductosPorcentaje"] = ListaProductoPorcentaje;
        }

        protected void ddlAmecPorCongreso_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            AgenteAmecInfo agenteAMEC = new AgenteAmecInfo();
            var miAmec = agenteAMEC.CargarTodosValoresAmec(ddlAmecPorCongreso.SelectedValue);
            bool hasIdConfEmpresa = (dExpediente != null && dExpediente.AMEC != null &&
                                     dExpediente.AMEC.idconfempresa != null && dExpediente.AMEC.idconfempresa.Value > 0);

            bool hasIdConfEmpresaGeneric = false;
            int idconfempresaGeneric = -1;
            if (!hasIdConfEmpresa)
            {
                if (dExpediente.esCongreso == 0)
                {
                    AgenteExpedientes agExp = new AgenteExpedientes();
                    DCabeceraActividad activ = agExp.ObtenerActividadPorID(dExpediente.nIDCogreso.ToString());
                    if (activ.idconfempresa.HasValue && activ.idconfempresa.Value > 0)
                    {
                        hasIdConfEmpresaGeneric = true;
                        idconfempresaGeneric = activ.idconfempresa.Value;
                    }
                }

            }

            //plhShowAgency.Visible = miAmec == null || (hasIdConfEmpresa);
            plhShowAgency.Visible = false;
            string tipo = Request.QueryString["tipo"];

            //Si es 1 es individual, si es 2 es colectivo
            if (tipo == "1")
            {
                btnContinuar.Visible = true;
            }

            if (hasIdConfEmpresaGeneric)
            {
                ddlAgencias.SelectedValue = idconfempresaGeneric.ToString();
                ddlAgencias.Enabled = false;

                if (miAmec != null && dExpediente.AMEC != null && dExpediente.AMEC.idconfempresa.HasValue && miAmec.idconfempresa != dExpediente.AMEC.idconfempresa.Value)
                {
                    Alert.Show("No es posible asignar el expediente a dicho amec: El expediente tiene una agencia asignada distinta de la agencia que tiene asignada el amec seleccionado");
                    btnContinuar.Visible = false;
                }
            }
            else if (hasIdConfEmpresa)
            {
                ddlAgencias.SelectedValue = dExpediente.AMEC.idconfempresa.Value.ToString();
                ddlAgencias.Enabled = false;

                if (miAmec != null && miAmec.idconfempresa != dExpediente.AMEC.idconfempresa.Value)
                {
                    Alert.Show("No es posible asignar el expediente a dicho amec: El expediente tiene una agencia asignada distinta de la agencia que tiene asignada el amec seleccionado");
                    btnContinuar.Visible = false;
                }
            }
        }

        protected void selectedAmecs_Change(object sender, EventArgs e)
        {
            AgenteAmecInfo agenteAMEC = new AgenteAmecInfo();
            var miAmec = agenteAMEC.CargarTodosValoresAmec(selectedIdAmecs.Value);
            bool hasIdConfEmpresa = (dExpediente != null && dExpediente.AMEC != null &&
                                     dExpediente.AMEC.idconfempresa != null && dExpediente.AMEC.idconfempresa.Value > 0);

            bool hasIdConfEmpresaGeneric = false;
            int idconfempresaGeneric = -1;
            if (!hasIdConfEmpresa)
            {
                if (dExpediente.esCongreso == 0)
                {
                    AgenteExpedientes agExp = new AgenteExpedientes();
                    DCabeceraActividad activ = agExp.ObtenerActividadPorID(dExpediente.nIDCogreso.ToString());
                    if (activ.idconfempresa.HasValue && activ.idconfempresa.Value > 0)
                    {
                        hasIdConfEmpresaGeneric = true;
                        idconfempresaGeneric = activ.idconfempresa.Value;
                    }
                }

            }

            //plhShowAgency.Visible = miAmec == null || (hasIdConfEmpresa);
            plhShowAgency.Visible = false;

            string tipo = Request.QueryString["tipo"];

            //Si es 1 es individual, si es 2 es colectivo
            if (tipo == "1")
            {
                btnContinuar.Visible = true;
            }


            if (hasIdConfEmpresaGeneric)
            {
                ddlAgencias.SelectedValue = idconfempresaGeneric.ToString();
                ddlAgencias.Enabled = false;

                if (miAmec != null && dExpediente.AMEC != null && dExpediente.AMEC.idconfempresa.HasValue && miAmec.idconfempresa != dExpediente.AMEC.idconfempresa.Value)
                {
                    Alert.Show("No es posible asignar el expediente a dicho amec: El expediente tiene una agencia asignada distinta de la agencia que tiene asignada el amec seleccionado");
                    btnContinuar.Visible = false;
                }
            }
            else if (hasIdConfEmpresa)
            {
                ddlAgencias.SelectedValue = dExpediente.AMEC.idconfempresa.Value.ToString();
                ddlAgencias.Enabled = false;

                if (miAmec != null && miAmec.idconfempresa != dExpediente.AMEC.idconfempresa.Value)
                {
                    Alert.Show("No es posible asignar el expediente a dicho amec: El expediente tiene una agencia asignada distinta de la agencia que tiene asignada el amec seleccionado");
                    btnContinuar.Visible = false;
                }
            }
        }


        [System.Web.Services.WebMethod]
        public static IEnumerable<DVAmecCongresoConcatSolicitante> amecsSearch(string search, int idConfEmpresa, bool includeOldAmecs)
        {
            try
            {
                AgenteExpedientes agExp = new AgenteExpedientes();
                var resultList = (agExp.ObtenerAMECPorCongresoConcatSolicitante(0, "0", idConfEmpresa, includeOldAmecs, search).Where(x => x.AmecConcatSolicitante.ToLower().Contains(search)).OrderBy(x => x.AmecConcatSolicitante).ToList());
                return resultList;//.Where(x => x.AMEC.Contains(search) || x.idamecs.Contains(search)).ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}