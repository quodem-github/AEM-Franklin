using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net.Mail;
using System.Globalization;
using EOS.Web;
using EOS.Logica;
using EOS.Entidades.Datos;
using EOS.Repositorios;
//using NPOI.HSSF.Record.Formula.Functions;

namespace EOS
{

    public partial class NuevoDetalleAMEC : System.Web.UI.Page
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
        EOS.Entidades.Datos.DDatosPersonalesUsuario datosUsuarioCorreo;
        EOS.Entidades.Datos.DUnidadesOrganizativasAmec unidOrgXDefecto;
        bool AccionUOCambioSolicitante = true;


        public bool hasParaguas()
        {
            if (chkParaguas.Checked)
            {
                ddlUnidad.Enabled = false;
                ddlArea.Enabled = false;
                ddlRegion.Enabled = false;
                ddlDistrito.Enabled = false;
                plAMECDatos3.Enabled = false;
                return true;
            }
            AgenteUnidadesOrgAMEC agenteUnidORg = new AgenteUnidadesOrgAMEC();
            if (agenteUnidORg.ObtenerNumeroUnidadesOrgAMEC(miAmec.idamecs.ToString(), miAmec.idsolicitante.ToString(), "", "", "", "", "", "", "", "", "") > 1)
            {
                chkParaguas.Checked = true;
                //Alert.Show("Un Amec no paraguas NO puede tener mas de una unidad organizativa");
                return false;
            }
            ddlUnidad.Enabled = false;
            ddlArea.Enabled = false;
            ddlRegion.Enabled = false;
            ddlDistrito.Enabled = false;
            plAMECDatos3.Enabled = false;
            return false;
        }

        public bool AsociarEvento()
        {
            if (chkAsociarEvento.Checked)
            {
                return true;
            }
            return false;
        }

        public bool DocAdicional()
        {
            if (chkDocAdicional.Checked)
            {
                return true;
            }
            return false;
        }

        public bool VerCriterioEspecifico()
        {
            if (ddlCriteriosSeleccion.SelectedItem.Text == "OTRO")
            {
                return true;
            }
            return false;
        }

        #region CargadoPagina

        protected void Page_Load(object sender, EventArgs e)
        {
            ///////////////Controlar Si caduca la sesión en la aplicación//////////////
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Session["rolesuser"] == null) Response.Redirect("~/Account/Login.aspx");
            //////////////////////////////////////////////////////////////////////////
            RepositorioFlujo estadoAMEC = null;

            string sIdAmec = Request.QueryString["idamec"];

            if (sIdAmec == null)
            {
                Response.Redirect("~/ListadoAMECs.aspx");
            }
            else
            {
                if (sIdAmec.ToCharArray()[0] == '4')
                {
                    Response.Redirect("~/ListadoAMECs.aspx");
                }
            }

            if (!Page.IsPostBack)
            {
                Session.Remove("NuevoAMEC");
                Session.Remove("NuevoAgente");
                Session.Remove("DatosUsuarioAMEC");
                Session.Remove("DatosSolicitante");
                Session.Remove("UnidOrgXDefecto");
                Session.Remove("AccionUO");

                datosUsuarioAMEC = (EOS.Entidades.Datos.DVPeticionariosRoles)Session["rolesuser"];
                agAMEC = new AgenteAMEC(datosUsuarioAMEC.IdPeticionario);
                //Comprobamos si es un nuevo AMEC o si es una visualizacion de AMEC.                 
                

                if (sIdAmec != null)
                {

                    if (Request.QueryString["idcongresoactividad"] != null)
                    {
                        idcongreso = Int32.Parse(Request.QueryString["idcongresoactividad"]);
                    }
                    string idamec = sIdAmec;
                    ObtenerAMEC(idamec); //OBTENER DATOS DEL AMEC
                    InicializaSesion();
                    InicializaCombos();
                    CargarControles();
                    estadoAMEC = new RepositorioFlujo(sIdAmec, (Int32)datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
                    HabilitaBotones(estadoAMEC);
                    ImagenEstadoAMEC();
                    rblFarmaindustria.Attributes.Add("onclick", "MostrarDescDocumentacionFarmaIndustria('" + rblFarmaindustria.ClientID + "'," + rblFarmaindustria.Items.Count + ")");
                    MostrarPorTipoActividad(ddlActividad.Text, true);
                    //El desplegable de la agencia solo será modificable si el amec no esta aprobado
                    ddlAgencias.Enabled = miAmec.idestado != 1;
                }
                else
                {
                    //NUEVO AMEC 
                    Response.Redirect("~/", true);
                    //Se Puede Crear Un Nuevo Amec Viniendo del Expediente o viniendo del Boton Nuevo Amec del Menú
                    AgenteMaestros agente = new AgenteMaestros();
                    EOS.Entidades.Datos.DVPeticionariosRoles datosRoles = (EOS.Entidades.Datos.DVPeticionariosRoles)Session["rolesuser"];

                    InicializaAMEC();
                    InicializaControles();
                    InicializaSesion();

                    if (Request.QueryString["idcongresoactividad"] != null)
                    {
                        idcongreso = Int32.Parse(Request.QueryString["idcongresoactividad"]);
                        //Vienes de Expediente por tanto tienes que asignarle un Congreso
                        GuardarNuevoAmec();
                        AsignarEventoAmec(idcongreso);
                        chkAsociarEvento.Checked = true;

                        RellenarDatosDelCongreso(idcongreso);

                    }
                    else
                    {
                        //Vienes del Boton del Menú
                        HabilitaBotones(estadoAMEC);
                    }
                    ImagenEstadoAMEC();
                    //this.eosContentResults.Visible = true;
                    lvActivid.DataBind();



                }
            }
            else
            {
                RecuperaSesion();
                CargarValoreAmecFormulario();
                ImagenEstadoAMEC();
                CombosUO(chkParaguas.Checked);
                ddlArchivoPrograma.SelectedValue = ddlArchivoPrograma.SelectedValue;

                if (!chkParaguas.Checked)
                {
                    AgenteUnidadesOrgAMEC agenteUnidORg = new AgenteUnidadesOrgAMEC();
                    if (agenteUnidORg.ObtenerNumeroUnidadesOrgAMEC(txtNAmec.Text.ToString(), miAmec.idsolicitante.ToString(), "", "", "", "", "", "", "", "", "") > 1)
                    {
                        chkParaguas.Checked = true;
                        Alert.Show("Un Amec no paraguas NO puede tener mas de una unidad organizativa");

                    }
                }
            }
            ActualizaCombosUnidadOrgXDefecto();
            this.SetFocus("txtDescripcionAMEC");

        }

        protected void CargarValoreAmecFormulario()
        {
            miAmec.idcreadopor = int.Parse(txtIdCreadoPor.Text);
            if (txtNAmec.Text != null && txtNAmec.Text != "0") miAmec.idamecs = txtNAmec.Text;
            if (txtNAmec.Text == "0" && lbidAMEC.InnerText != "") miAmec.idamecs = lbidAMEC.InnerText;

            miAmec.idsolicitante = Convert.ToInt32(ddlSolicitante.SelectedValue);
            miAmec.idconfempresa = Convert.ToInt32(ddlAgencias.SelectedValue);
            miAmec.nwein = txtWein.Text;
            if (!string.IsNullOrEmpty(txtIdEstado.Text)) miAmec.idestado = Convert.ToUInt32(txtIdEstado.Text);

            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            datosUsuarioSolicitante = agenteUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());
            miAmec.idcargo = datosUsuarioSolicitante.IdCargo;
            miAmec.idposition = datosUsuarioSolicitante.IdPosition;

            if (!string.IsNullOrEmpty(txtDescripcionAMEC.Text)) { miAmec.descripcion = txtDescripcionAMEC.Text; }
            miAmec.idtipoactividad = Int32.Parse(ddlActividad.SelectedValue);

            miAmec.preaprobadaamed = rblPreaprobMedico.SelectedItem.Value == "1";
            miAmec.preaprobadaneg = rblPreaprobNegocio.SelectedItem.Value == "1";
            miAmec.preaprobadaleg = rblPreaprovLegal_Comp.SelectedItem.Value == "1";
            miAmec.farmaindustria = rblFarmaindustria.SelectedItem.Value == "1";
            miAmec.casosclinicos = rblCasosClinicos.SelectedItem.Value == "1";
            miAmec.politicaN20 = rblpoliticaN20.SelectedValue == "1";

            miAmec.detallecriterios = ddlCriteriosSeleccion.SelectedItem.Text;
            miAmec.idcriterioseleccion = Int32.Parse(ddlCriteriosSeleccion.SelectedValue);

            if (ddlCriteriosSeleccion.SelectedItem.Text == "OTRO")
            {
                miAmec.criterioespecificado = txtEspecificarCriterioSeleccion.Text;
            }

            miAmec.medicosfichero = rbMedicosGenesys.SelectedItem.Value == "1";


            if (!string.IsNullOrEmpty(txtHoras.Text)) { miAmec.duracionhoras = txtHoras.Text; }

            //Validar si están los campos correctamente//
            if (ValidarCampos())
            {

                if (!string.IsNullOrEmpty(txtTotalParticipantes.Text)) { miAmec.participantesmsd = uint.Parse(txtTotalParticipantes.Text); }
                if (!string.IsNullOrEmpty(txtNumPonentes.Text)) { miAmec.ponentespatrocinados = uint.Parse(txtNumPonentes.Text); }
                if (!string.IsNullOrEmpty(txtFechaComienzo.Text)) { miAmec.fechacomienzo = DateTime.Parse(txtFechaComienzo.Text); }
                if (!string.IsNullOrEmpty(txtFechaFinalizacion.Text)) { miAmec.fechafinalizacion = DateTime.Parse(txtFechaFinalizacion.Text); }
                if (!string.IsNullOrEmpty(txtProfesionalesSanitarios.Text)) { miAmec.profesionalessanitarios = uint.Parse(txtProfesionalesSanitarios.Text); }

                if (!string.IsNullOrEmpty(txtImporteTotalGasto.Text))
                {
                    txtImporteTotalGasto.Text = txtImporteTotalGasto.Text.Replace(".", "");
                    miAmec.importegasto = Decimal.Parse(txtImporteTotalGasto.Text.Replace(",", "."), CultureInfo.InvariantCulture);
                    txtImporteTotalGasto.Text = string.Format("{0:n2}", miAmec.importegasto);
                }
                if (!string.IsNullOrEmpty(txtNumeroCartas.Text)) { miAmec.cartascontrato = uint.Parse(txtNumeroCartas.Text); }
            }
            if (!string.IsNullOrEmpty(txtLugarSede.Text)) { miAmec.lugarsede = txtLugarSede.Text; }
            if (!string.IsNullOrEmpty(txtgastoDesglose.Text)) { miAmec.conceptogastos = txtgastoDesglose.Text; }
            if (!string.IsNullOrEmpty(txtCargoADaxas.Text)) { miAmec.cargoadaxas = txtCargoADaxas.Text; }
            miAmec.paraguas = chkParaguas.Checked;



            if (estaAnulandoPrograma)
            {
                miAmec.programaamecs = "";
                miAmec.urlprograma = "";
                miAmec.descripcionobjetivo = "";
            }
            else
            {
                if (odsProgramaAMEC.Select() != null)
                {
                    Object[] valores = (Object[])odsProgramaAMEC.Select();

                    DAmecInfo dato = (DAmecInfo)valores[0];
                    miAmec.programaamecs = dato.programaamecs;
                    miAmec.urlprograma = dato.urlprograma;
                    miAmec.descripcionobjetivo = dato.descripcionobjetivo;
                }
                else
                {
                    switch (ddlArchivoPrograma.SelectedValue)
                    {
                        case "ARCHIVO":
                            //miAmec.programaamecs ja ho tenim guardat
                            miAmec.urlprograma = "";
                            miAmec.descripcionobjetivo = "";
                            break;

                        case "ENLACE":
                            miAmec.programaamecs = "";
                            miAmec.urlprograma = txtURLProgAMEC.Text;
                            miAmec.descripcionobjetivo = "";
                            break;

                        case "DESCRIPCION":
                            //Calquier otro caso
                            miAmec.programaamecs = "";
                            miAmec.urlprograma = "";
                            miAmec.descripcionobjetivo = txtDescProgAMEC.Text;

                            break;
                    }
                }
            }
        }

        protected void RellenarDatosDelCongreso(int idcongreso)
        {
            AgenteAmecInfo agamecinf = new AgenteAmecInfo();
            DVCongresoAmec congresoactividad = agamecinf.ObtenerCongreso(idcongreso);
            txtLugarSede.Text = congresoactividad.Poblacion;
            txtFechaComienzo.Text = String.Format("{0:dd/MM/yyyy}", congresoactividad.Desde);
            txtFechaFinalizacion.Text = String.Format("{0:dd/MM/yyyy}", congresoactividad.Hasta);
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
            miAmec.idamecs = agAMECinfo.DameSiguienteIdAmecs();
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

        protected void InicializaCombos()
        {
            RellenarCombos();
            if (miAmec.idestado == 8)
            {
                if (datosUsuarioSolicitante.idunidad.HasValue)
                {
                    // Si tiene idunidad la seleccionamos en el combo.
                    ddlUnidad.SelectedValue = datosUsuarioSolicitante.idunidad.ToString();
                }
                if (datosUsuarioSolicitante.Idarea.HasValue)
                {
                    ddlArea.SelectedValue = datosUsuarioSolicitante.Idarea.ToString();
                }

                if (datosUsuarioSolicitante.idregion.HasValue)
                {
                    ddlRegion.SelectedValue = datosUsuarioSolicitante.idregion.ToString();
                }

                if (datosUsuarioSolicitante.iddistrito.HasValue)
                {
                    ddlDistrito.SelectedValue = datosUsuarioSolicitante.iddistrito.ToString();
                }
            }
        }

        protected void ActualizaCombosUnidadOrgXDefecto()
        {
            if (datosUsuarioSolicitante.idunidad.HasValue)
            {
                // Si tiene idunidad la seleccionamos en el combo.
                ddlUnidad.SelectedValue = datosUsuarioSolicitante.idunidad.ToString();
            }

            ddlArea.SelectedValue = datosUsuarioSolicitante.Idarea.HasValue ? datosUsuarioSolicitante.Idarea.ToString() : "0";

            ddlRegion.SelectedValue = datosUsuarioSolicitante.idregion.HasValue ? datosUsuarioSolicitante.idregion.ToString() : "0";

            ddlDistrito.SelectedValue = datosUsuarioSolicitante.iddistrito.HasValue ? datosUsuarioSolicitante.iddistrito.ToString() : "0";
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
                    plAMEC_Historial.Visible = false;
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
                        txtNAmec.Text = "0";
                        //lbidAMEC.InnerText = "0";
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

                    rblPreaprobMedico.SelectedValue = "1";
                    rblPreaprobNegocio.SelectedValue = "0";
                    rblPreaprovLegal_Comp.SelectedValue = "0";
                    rblFarmaindustria.SelectedValue = "0";
                    rblCasosClinicos.SelectedValue = "0";
                    rblpoliticaN20.SelectedValue = "1";
                    //this.txtFechaComienzo.Text = DateTime.Now.ToShortDateString();
                    ddlCriteriosSeleccion.SelectedValue = "3";
                    rblCasosClinicos.Attributes.Add("onclick", "checkControl('" + rblCasosClinicos.ClientID + "'," + rblCasosClinicos.Items.Count + ")");
                    rblFarmaindustria.Attributes.Add("onclick", "MostrarDescFarmaIndustria('" + rblFarmaindustria.ClientID + "'," + rblFarmaindustria.Items.Count + ")");
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
            txtCargo.Text = agSol_AMEC.ObtenerCargo();
            txtIdEstado.Text = miAmec.idestado.ToString();
            txtWein.Text = miAmec.nwein.ToString();
            txtNAmec.Text = miAmec.idamecs.ToString();

            lbidAMEC.InnerText = miAmec.idamecs.ToString();

            if (!string.IsNullOrWhiteSpace(miAmec.idamecsrelacionado))
            {
                lbidAMECClonado.InnerText = string.Format(" (Clonado-resometido a {0})", miAmec.idamecsrelacionado);
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

            rblPreaprobMedico.SelectedValue = miAmec.preaprobadaamed == true ? "1" : "0";

            rblPreaprobNegocio.SelectedValue = miAmec.preaprobadaneg == true ? "1" : "0";

            rblPreaprovLegal_Comp.SelectedValue = miAmec.preaprobadaleg == true ? "1" : "0";

            rblFarmaindustria.SelectedValue = miAmec.farmaindustria == true ? "1" : "0";

            rblCasosClinicos.SelectedValue = miAmec.casosclinicos == true ? "1" : "0";

            if (miAmec.participantesmsd.HasValue) { txtTotalParticipantes.Text = miAmec.participantesmsd.ToString(); }

            ddlCriteriosSeleccion.SelectedValue = miAmec.idcriterioseleccion.ToString();
            ddlCriteriosSeleccion.Text = miAmec.detallecriterios.ToString();
            if (miAmec.criterioespecificado != null || miAmec.criterioespecificado != "")
            {
                txtEspecificarCriterioSeleccion.Text = miAmec.criterioespecificado;
            }

            rbMedicosGenesys.SelectedValue = miAmec.medicosfichero == true ? "1" : "0";

            if (miAmec.duracionhoras != null) { txtHoras.Text = miAmec.duracionhoras.ToString(); }
            if (miAmec.ponentespatrocinados.HasValue) { txtNumPonentes.Text = miAmec.ponentespatrocinados.ToString(); }
            if (miAmec.profesionalessanitarios.HasValue) { txtProfesionalesSanitarios.Text = miAmec.profesionalessanitarios.ToString(); } //TODO

            if (!string.IsNullOrEmpty(miAmec.conceptogastos)) { txtgastoDesglose.Text = miAmec.conceptogastos; }

            if (!string.IsNullOrEmpty(miAmec.cargoadaxas)) { txtCargoADaxas.Text = miAmec.cargoadaxas.ToString(); }

            chkParaguas.Checked = miAmec.paraguas == true;

            rblpoliticaN20.SelectedValue = miAmec.politicaN20 == true ? "1" : "0";


            if (!string.IsNullOrEmpty(miAmec.lugarsede)) { txtLugarSede.Text = miAmec.lugarsede.ToString(); }

            if (miAmec.fechafinalizacion != null) { txtFechaFinalizacion.Text = String.Format("{0:dd/MM/yyyy}", miAmec.fechafinalizacion); }
            if (miAmec.fechacomienzo != null) { txtFechaComienzo.Text = String.Format("{0:dd/MM/yyyy}", miAmec.fechacomienzo); }

            // Obtiene un NumberFormatInfo asociado con la cultura es-ES (Español de España)
            NumberFormatInfo nfi = new CultureInfo("es-ES", false).NumberFormat;
            if (miAmec.importegasto.HasValue) { txtImporteTotalGasto.Text = string.Format("{0:n2}", miAmec.importegasto); }
            if (miAmec.cartascontrato.HasValue) { txtNumeroCartas.Text = miAmec.cartascontrato.ToString(); }

            //txtArchivo.Text = miAmec.programaamecs.ToString(); 
            txtURLProgAMEC.Text = miAmec.urlprograma.ToString();
            txtDescProgAMEC.Text = miAmec.descripcionobjetivo.ToString();

            //EVENTOS
            AgenteAmecInfo aginfo = new AgenteAmecInfo();
            if (aginfo.ObtenerNumeroActividadesAsigAmec(miAmec.idamecs) > 0)
            {
                chkAsociarEvento.Checked = true;
            }


            //DOCUMENTOS
            if (aginfo.ObtenerNumeroDocumentacionAMEC(miAmec.idamecs.ToString()) > 0)
            {
                chkDocAdicional.Checked = true;
            }
        }

        public static String RemoveDiacritics(String s)
        {
            String normalizedString = s.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                Char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        public void HabilitaTodo()
        {
            AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
            RepositorioFlujo estadoAMEC = null;
            if (miAmec.idestado == 5 && agAmecInfo.EstaGuardadoAmec(miAmec.idamecs))
            {
                //Los siguientes valores no se cargan la primera vez que se crea un amec, porque aún están en estado Nuevo Amec
                txtNAmec.Text = miAmec.idamecs.ToString();
                lbidAMEC.InnerText = miAmec.idamecs.ToString();
                if (!string.IsNullOrWhiteSpace(miAmec.idamecsrelacionado))
                {
                    lbidAMECClonado.InnerText = string.Format(" (Clonado-resometido a {0})", miAmec.idamecsrelacionado);
                }
                else
                {
                    lbidAMECClonado.InnerText = string.Empty;
                }
                agAMECinfo = new AgenteAmecInfo();
                lbEstadoAMEC.Text = agAMECinfo.ObtenerNombreEstado(miAmec.idestado);
            }
            estadoAMEC = new RepositorioFlujo(miAmec.idamecs, (Int32)datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
            HabilitaBotones(estadoAMEC);
        }

        //public void HabilitaBotones(RepositorioFlujo estadoAmec)
        //{
            //AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
            //if (estadoAmec != null && agAmecInfo.EstaGuardadoAmec(miAmec.idamecs))
            //{

            //    //BOTON APROBAR
            //    btnAprobar.Visible = estadoAmec.isAceptar;
            //    btnAprobarConCondicion.Visible = estadoAmec.isAceptar;
            //    //////////////////////////////////////////////

            //    //BOTON RECHAZAR
            //    btnRechazar.Visible = estadoAmec.isRechazar;
            //    //////////////////////////////////////////////

            //    //BOTON CANCELAR
            //    btnVolver.Visible = true;
            //    btnCancelar.Visible = miAmec.idsolicitante == datosUsuarioAMEC.IdPeticionario && miAmec.idestado != 4;
            //    //////////////////////////////////////////////

            //    //BOTON GUARDAR
            //    btnGuardar.Visible = (miAmec.idcreadopor == datosUsuarioAMEC.IdPeticionario && miAmec.idestado == 5) || miAmec.idsolicitante == datosUsuarioAMEC.IdPeticionario;
            //    //////////////////////////////////////////////

            //    //ESTADO PENDIENTE SOMETER
            //    if (miAmec != null && miAmec.idestado.ToString() == "35")
            //    {
            //        btnCancelar.Visible = false;
            //        btnGuardar.Visible = false;
            //        btnAprobar.Visible = false;
            //        btnAprobarConCondicion.Visible = false;
            //        btnRechazar.Visible = false;
            //    }

            //    //BOTON SOMETER
            //    btnSometer.Visible = estadoAmec.isSometer;
            //    //////////////////////////////////////////////

            //}
            //else
            //{
            //    //Aquí entra cuando entras/creas el Amec por primera vez
            //    btnVolver.Visible = true;
            //    btnCancelar.Visible = miAmec.idsolicitante == datosUsuarioAMEC.IdPeticionario;
            //    btnGuardar.Visible = true;
            //    if (miAmec.idsolicitante != datosUsuarioAMEC.IdPeticionario && miAmec.idcreadopor == datosUsuarioAMEC.IdPeticionario) btnCancelar.Visible = true;
            //    else btnSometer.Visible = true;
            //    btnRechazar.Visible = false;
            //    btnAprobar.Visible = false;
            //    btnAprobarConCondicion.Visible = false;
            //}

            ////BOTON RESOMETER
            //if (miAmec.idestado != 5 && !IsPostBack)
            //{
            //    AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            //    bool YaSeHaEnviadoAFarma = agAmecInf.ComprobarSiSeEnvioAFarma(miAmec.idamecs);
            //    bool YaSeHaEnviadoACasosClinicos = agAmecInf.ComprobarSiSeEnvioACasosClinicos(miAmec.idamecs);
            //    this.btnSometer.OnClientClick = "return ConfirmarResometer('" + YaSeHaEnviadoAFarma + "','" + YaSeHaEnviadoACasosClinicos + "')";
            //}
            ////////////////////////////////////////////////

            //this.btnAdjuntarDoc.OnClientClick = miAmec.idestado != 5 ? "return ConfirmarEnviarFarmaIndustria()" : "return CheckFileSize()";

            ////Visibility and Edit AMEC
            //if (btnSometer.Visible == true)
            //{
            //    plAMEC_Comentarios.Visible = true;
            //    btnGuardar.Visible = true;
            //    plAMECDatos7.Enabled = true;
            //    Panel8.Enabled = true;
            //    plPaso1.Enabled = true;
            //    plAMECDatos1.Enabled = true;
            //    Panel9.Enabled = true;
            //}
            //else if (btnAprobar.Visible == true)
            //{
            //    plAMEC_Comentarios.Visible = true;
            //    plAMECDatos7.Enabled = false;
            //    btnExportarInfoExpediente.Visible = false;
            //    Panel8.Enabled = true;

            //    plPaso1.Enabled = false;
            //    plAMECDatos1.Enabled = false;
            //    Panel9.Enabled = true;
            //}
            //else
            //{
            //    if (miAmec.idestado == 1 && miAmec.idsolicitante == datosUsuarioAMEC.IdPeticionario)
            //    {
            //        btnSometer.Visible = true;
            //        plAMEC_Comentarios.Visible = true;
            //        plAMEC_Comentarios.Enabled = true;
            //    }
            //    else if (miAmec.idestado == 5 && miAmec.idcreadopor == datosUsuarioAMEC.IdPeticionario)
            //    {
            //        btnSometer.Visible = false;
            //        plAMEC_Comentarios.Visible = true;
            //        plAMEC_Comentarios.Enabled = false;
            //        btnPublicarComentario.Visible = false;
            //        Panel9.Enabled = true;
            //    }
            //    else
            //    {
            //        //entrarà cuando esté aprobado o rechazado o cancelado 
            //        plAMEC_Comentarios.Visible = true;

            //        plAMEC_Comentarios.Enabled = false;
            //        btnPublicarComentario.Visible = false;
            //        btnGuardar.Visible = false;
            //        plAMECDatos7.Enabled = false;
            //        btnExportarInfoExpediente.Visible = false;
            //        Panel8.Enabled = true;
            //        plPaso1.Enabled = false;
            //        plAMECDatos1.Enabled = false;
            //        Panel9.Enabled = true;
            //    }
            //}
            /////////////////////////////////////////////
        //}

        public void HabilitaBotones(RepositorioFlujo estadoAmec)
        {
            AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
            if (estadoAmec != null && agAmecInfo.EstaGuardadoAmec(miAmec.idamecs))
            {
                //BOTON CANCELAR
                btnVolver.Visible = true;

                //BOTON GUARDAR
                btnGuardar.Visible = (miAmec.idcreadopor == datosUsuarioAMEC.IdPeticionario && miAmec.idestado == 5) || miAmec.idsolicitante == datosUsuarioAMEC.IdPeticionario;
                //////////////////////////////////////////////

                //ESTADO PENDIENTE SOMETER
                if (miAmec != null && miAmec.idestado.ToString() == "35")
                {
                    btnGuardar.Visible = false;
                }
            }
            else
            {
                //Aquí entra cuando entras/creas el Amec por primera vez
                btnVolver.Visible = true;
                btnGuardar.Visible = true;
            }

            if (miAmec.idestado != 5 && !IsPostBack)
            {
                AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
                bool YaSeHaEnviadoAFarma = agAmecInf.ComprobarSiSeEnvioAFarma(miAmec.idamecs);
                bool YaSeHaEnviadoACasosClinicos = agAmecInf.ComprobarSiSeEnvioACasosClinicos(miAmec.idamecs);
                this.btnClonar.OnClientClick = "return ConfirmarResometer('" + YaSeHaEnviadoAFarma + "','" + YaSeHaEnviadoACasosClinicos + "')";
            }

            this.btnAdjuntarDoc.OnClientClick = miAmec.idestado != 5 ? "return ConfirmarEnviarFarmaIndustria()" : "return CheckFileSize()";

            //Visibility and Edit AMEC
            if (estadoAmec.isSometer)
            {
                plAMEC_Comentarios.Visible = true;
                btnGuardar.Visible = true;
                plAMECDatos7.Enabled = true;
                Panel8.Enabled = true;
                plPaso1.Enabled = true;
                plAMECDatos1.Enabled = true;
                Panel9.Enabled = true;
            }
            else if (estadoAmec.isAceptar)
            {
                plAMEC_Comentarios.Visible = true;
                plAMECDatos7.Enabled = false;
                btnExportarInfoExpediente.Visible = false;
                Panel8.Enabled = true;

                plPaso1.Enabled = false;
                plAMECDatos1.Enabled = false;
                Panel9.Enabled = true;
            }
            else
            {
                if (miAmec.idestado == 1 && miAmec.idsolicitante == datosUsuarioAMEC.IdPeticionario)
                {
                    plAMEC_Comentarios.Visible = true;
                    plAMEC_Comentarios.Enabled = true;
                }
                else if (miAmec.idestado == 5 && miAmec.idcreadopor == datosUsuarioAMEC.IdPeticionario)
                {
                    plAMEC_Comentarios.Visible = true;
                    plAMEC_Comentarios.Enabled = false;
                    btnPublicarComentario.Visible = false;
                    Panel9.Enabled = true;
                }
                else
                {
                    //entrarà cuando esté aprobado o rechazado o cancelado 
                    plAMEC_Comentarios.Visible = true;
                    plAMEC_Comentarios.Enabled = false;
                    btnPublicarComentario.Visible = false;
                    btnGuardar.Visible = false;
                    plAMECDatos7.Enabled = false;
                    btnExportarInfoExpediente.Visible = false;
                    Panel8.Enabled = true;
                    plPaso1.Enabled = false;
                    plAMECDatos1.Enabled = false;
                    Panel9.Enabled = true;
                }
            }

            //plAMECDatos1.Enabled = false;
            plAMECDatos3.Enabled = false;
            //plAMECDatos7.Enabled = false;
            btnClonar.Visible = (string.IsNullOrWhiteSpace(miAmec.idamecsrelacionado) && (miAmec.idsolicitante == datosUsuarioAMEC.IdPeticionario));
            btnClonar.OnClientClick = "return confirm('VAS A CLONAR-SOMETER EL AMEC\\n\\nSe generará otro AMEC en estado borrador con una nueva numeración.\\n\\nEste AMEC se cancelará. Toda la documentación y expedientes asociados a él serán traspasados al nuevo AMEC.\\n\\n¿Está seguro que desea continuar?');";
            ///////////////////////////////////////////
        }

        private void RellenarCombos()
        {

            /* //cuando el combo de unidades se seleccion entonces se cambia el de areas y
             //se vuelve a bindar con los datos seleccionados en unidades.
             AgenteMaestros agente = new AgenteMaestros();
             ListaReservasInfo = agente.obtenerInfoReserva(entExp.Idexpediente);
             */

            AgenteMaestros agente = new AgenteMaestros();
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            AgenteFlujoAprobacion agenteFlujo = new AgenteFlujoAprobacion();
            AgenteAmecInfo agenteAmecInf = new AgenteAmecInfo();

            //AGENCIAS//
            this.ddlAgencias.DataSource = agente.ObtenerEmpresasConf();
            ddlAgencias.DataValueField = "idconfempresa";
            ddlAgencias.DataTextField = "nombreagencia";
            ddlAgencias.DataBind();
            ddlAgencias.SelectedIndex = 0;
            //FIN AGENCIAS//

            //AREA//
            this.ddlArea.DataSource = agente.ObtenerAreas(null);
            ddlArea.DataValueField = "idarea";
            ddlArea.DataTextField = "area";
            this.ddlArea.DataBind();
            ListItem blankArea = new ListItem("Cualquier Area", "0");
            ddlArea.Items.Insert(0, blankArea);
            ddlArea.SelectedValue = "0";

            BDManage vectAreaMas;
            vectAreaMas = new BDManage(SQLSentenceAMEC.GetInstancia().getAllAreas(), 1);
            rellenaAreaMas(vectAreaMas);
            //FIN AREA//

            //UNIDAD//
            this.ddlUnidad.DataSource = agente.ObtenerUnidades(null);
            ddlUnidad.DataValueField = "idUnidad";
            ddlUnidad.DataTextField = "unidad";
            this.ddlUnidad.DataBind();
            ListItem blankUnidad = new ListItem("Cualquier Unidad", "0");
            ddlUnidad.Items.Insert(0, blankUnidad);
            ddlUnidad.SelectedValue = "0";

            BDManage vectUnidadMas;
            vectUnidadMas = new BDManage(SQLSentenceAMEC.GetInstancia().getAllUnidades(), 1);
            rellenaUnidadMas(vectUnidadMas);
            //FIN UNIDAD

            //REGION//
            this.ddlRegion.DataSource = agente.ObtenerRegiones(null);
            ddlRegion.DataValueField = "idregion";
            ddlRegion.DataTextField = "region";
            this.ddlRegion.DataBind();
            ListItem blankRegion = new ListItem("Cualquier Region", "0");
            ddlRegion.Items.Insert(0, blankRegion);
            ddlRegion.SelectedValue = "0";

            BDManage vectRegionMas;
            vectRegionMas = new BDManage(SQLSentenceAMEC.GetInstancia().getAllRegiones(), 1);
            rellenaRegionMas(vectRegionMas);
            //FIN REGION//

            //DISTRITO//
            this.ddlDistrito.DataSource = agente.ObtenerDistritos(null);
            ddlDistrito.DataValueField = "iddistrito";
            ddlDistrito.DataTextField = "distrito";
            this.ddlDistrito.DataBind();
            ListItem blankDistrito = new ListItem("Cualquier Distrito", "0");
            ddlDistrito.Items.Insert(0, blankDistrito);
            ddlDistrito.SelectedValue = "0";

            //BDManage vectDistritoMas;
            //vectDistritoMas = new BDManage(SQLSentenceAMEC.GetInstancia().getAllDistritos(), 1);
            //rellenaDistritoMas(vectDistritoMas);
            ddlDistritoMas.DataValueField = "iddistrito";
            ddlDistritoMas.DataTextField = "distrito";
            ddlDistritoMas.DataBind();
            ListItem blankDistritoMas = new ListItem("Cualquier Distrito", "0");
            ddlDistritoMas.Items.Insert(0, blankDistritoMas);
            ddlDistritoMas.SelectedValue = "0";
            //FIN DISTRITO//

            this.ddlSolicitante.DataSource = agenteUsu.ObtenerTodosUsuariosCombo(miAmec.idamecs);
            this.ddlSolicitante.DataBind();

            this.ddlCriteriosSeleccion.DataSource = agente.ObtenerCriteriosSeleccion(null);
            this.ddlCriteriosSeleccion.DataBind();

            this.ddlCategoriaDocumento.DataSource = agenteAmecInf.ObtenerCategoriasDocumento();
            this.ddlCategoriaDocumento.DataBind();
            this.ddlCategoriaDocumento.SelectedValue = "1";

            /////////////////////////////EVENTO////////////////////////////////////////

            DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
            this.ddlTipoActividad.DataSource = agente.ObtenerTiposActividad("(Todas)", datosRoles.administrador, datosRoles.newco);
            this.ddlTipoActividad.DataBind();

            this.ddlPoblacion.DataSource = agente.ObtenerPoblaciones("(Todas)", miAmec.idconfempresa);
            this.ddlPoblacion.DataBind();

            this.txtFechaDesde.Text = DateTime.Now.ToShortDateString();
            RangeValidator1.MinimumValue = DateTime.Now.ToShortDateString();
            lvActividadAsigAmec.DataBind();
            ///////////////////////////////////////////////////////

            ICollection<DTipoActividadFlujo> datosFiltroActividad = null;
            if (miAmec != null)
                datosFiltroActividad = agenteFlujo.ObtenerTipoActividad(miAmec.idtipoactividad.ToString(), false);
            else
                datosFiltroActividad = agenteFlujo.ObtenerTipoActividad("0", false);
            ddlActividad.DataSource = datosFiltroActividad;
            ddlActividad.DataBind();
        }

        #endregion

        #region Imagenes


        protected void imgAdjuntarPrograma_Command(object sender, CommandEventArgs e)
        {
            int bienSubidoProgram = 0;
            AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
            if (ValidarCampos())
            {
                if (ddlArchivoPrograma.SelectedValue == "ARCHIVO")
                {
                    String fileName = fUploadPrograma.FileName;
                    if (ValidarNombreArchivo(fileName)) bienSubidoProgram = UploadButton("Programa");
                }
                if (bienSubidoProgram == 1)
                {
                    if (!agAmecInfo.EstaGuardadoAmec(miAmec.idamecs))
                    {
                        //AMEC no insertado. Guardamos valores del amec e insertamos amec y la unidad organizativa por defecto.
                        GuardarNuevoAmec();
                    }
                    else ActualizarAmec();
                    //ddlArchivoPrograma.Enabled = false;
                    //btnAdjuntarPrograma.Enabled = false;
                    //VisualitzarPrograma = false;
                    //Response.Write("<script language='javascript'> {window.document.getElementById('Div6').style.display = 'none'}</script>");

                    lvPrograma.DataBind();
                }
                else Alert.Show("No se ha subido bien el programa");
            }

        }


        protected void imgEliminarAsignacionEvento_Command(object sender, CommandEventArgs e)
        {
            AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
            //Eliminar el Evento assignado al amec, asi que eliminar la fila dentro del dbo_iw_amec
            int idameccongreso = Int32.Parse(e.CommandArgument.ToString());
            int benEliminat = agAmecInfo.EliminarRelacionAmecCongreso(idameccongreso);
            if (benEliminat == 1) { }//treure text que s'ha eliminat correctament TODO
        }


        protected void imgAsignarEvento_Command(object sender, CommandEventArgs e)
        {
            AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
            if (agAmecInfo.EventoYaEstaAsignadoAmec(miAmec.idamecs, Int32.Parse(e.CommandArgument.ToString())) == 0)
            {

                int acividadesAsigAmec = agAmecInfo.ObtenerNumeroActividadesAsigAmec(miAmec.idamecs);
                if (acividadesAsigAmec == 0)
                {
                    //Si s'ha creat un amec no el crearem de nou
                    AsignarEventoAmec(Int32.Parse(e.CommandArgument.ToString()));
                }
                else if (chkParaguas.Checked == true)
                {
                    //Si s'ha creat un amec no el crearem de nou
                    AsignarEventoAmec(Int32.Parse(e.CommandArgument.ToString()));
                }
                else Alert.Show("No se puede Asignar más de un evento a un AMEC no Paraguas");
            }
            else Alert.Show("Este Evento ya está Asignado al AMEC");
        }


        protected void imgGuardarFichero_Command(object sender, CommandEventArgs e)
        {
            //agAMECinfo.GuardarInfoFichero();
        }


        protected void imgGuardarDocumento_Command(object sender, CommandEventArgs e)
        {
            //agAMECinfo.GuardarDocumento();
        }


        protected void imgUnidadEliminar_Command(object sender, CommandEventArgs e)
        {
            //Cojer el valor del comandArgument el idunidadamec
            AgenteUnidadesOrgAMEC agUnidad = new AgenteUnidadesOrgAMEC();
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            AgenteComentariosAMEC agComAmec = new AgenteComentariosAMEC();
            long idunidadamec = Int64.Parse(e.CommandArgument.ToString());

            //Comprobar si el usuario es el Solicitante
            if (datosUsuarioAMEC.IdPeticionario == datosUsuarioSolicitante.IdPeticionario)
            {
                IEnumerable<DUnidadesOrganizativasAmec> list = (IEnumerable<DUnidadesOrganizativasAmec>)odsUnidadesOrganizativas.Select();
                if (list.Count() == 0 || list.Count() == 1)
                {
                    Alert.Show("Debe existir una unidad organizativa como mínimo para la creacion del amec.");
                }
                else
                {
                    int resultado = agUnidad.EliminarUnidadOrganizativaAMEC(idunidadamec);
                    if (miAmec.idestado != 5)
                    {
                        Alert.Show("Es NECESARIO RESOMETER para que el flujo sea correcto, debido a la modificación de las unidades Organizativas, ");
                        //agAmecInf.CambiarEstadoAmec("35", miAmec.idamecs.ToString(),
                        //datosUsuarioAMEC.IdPeticionario.ToString() != null ? datosUsuarioAMEC.IdPeticionario.ToString() : null,
                        //datosUsuarioAMEC.idunidad != null ? datosUsuarioAMEC.idunidad.Value.ToString() : "null",
                        //datosUsuarioAMEC.Idarea != null ? datosUsuarioAMEC.Idarea.Value.ToString() : "null",
                        //datosUsuarioAMEC.idregion != null ? datosUsuarioAMEC.idregion.Value.ToString() : "null",
                        //datosUsuarioAMEC.iddistrito != null ? datosUsuarioAMEC.iddistrito.Value.ToString() : "null");
                        var unidadOrganizEliminar = list.Where(DUnidadesOrganizativasAmec => DUnidadesOrganizativasAmec.idunidadamec == idunidadamec);
                        string comentario = string.Format("Se ha cambiado el estado del amec al ELIMINAR una Unidad Organizativa: Unidad: {0}, Area: {1}, Region: {2}, Distrito: {3}", unidadOrganizEliminar.First().unidad, unidadOrganizEliminar.First().area, unidadOrganizEliminar.First().region, unidadOrganizEliminar.First().distrito);
                        agComAmec.InsertComentarioAMEC(miAmec.idamecs.ToString(), datosUsuarioAMEC.IdPeticionario.ToString(), comentario);
                        //Los comentarios por acciones de eliminar unidades no se envían por correo -> Eso que sepamos, ya que hubo un bug que se comentaron líneas de código de envío de emails.
                        //EnviaMailPublicarComentario();
                        btnVolver.Visible = false;

                        //btnCancelar.Visible = false;
                        //btnGuardar.Visible = false;

                        lvComentarios.DataBind();
                        AgenteAmecInfo agenteAMEC = new AgenteAmecInfo();
                        lbEstadoAMEC.Text = agenteAMEC.ObtenerNombreEstado(35);


                    }
                    lvUnidadesOrg.DataBind();
                }

            }
            else Alert.Show("Solo Puede Eliminar las Unidades Organizativas el Solicitante del Amec");

        }


        protected void imgAdjuntarDocumentacion_Command(object sender, CommandEventArgs e)
        {
            AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
            int bienSubidaDoc = 0;
            String fileName = fuploadDocumentacion.FileName;
            if (ValidarNombreArchivo(fileName))
            {
                bienSubidaDoc = UploadButton("Documentacion");
            }
            if (bienSubidaDoc == 1)
            {
                if (agAmecInfo.EstaGuardadoAmec(miAmec.idamecs) != true)
                {
                    if (ValidarCampos())
                    {
                        //AMEC no insertado. Guardamos valores del amec e insertamos amec y la unidad organizativa por defecto.
                        GuardarNuevoAmec();
                        int benGuardat = 0;
                        //string[] nombreDocumento = savePathDocumentacion.Split('/');
                        FileInfo infofile = new FileInfo(savePathDocumentacion);

                        string extensionDocumento = System.IO.Path.GetExtension(savePathDocumentacion);

                        benGuardat = agAmecInfo.GuardarDocumentacion(miAmec.idamecs, datosUsuarioAMEC.IdPeticionario, infofile.Name, savePathDocumentacion, txtComentarioDoc.Text.Trim(), chkemail.Checked, extensionDocumento, Convert.ToInt32(ddlCategoriaDocumento.SelectedValue));
                        if (benGuardat == 1)
                        {
                            lvDocumentacion.DataBind();
                            //txtArchivoDocumento.Text = "";
                            txtComentarioDoc.Text = "";
                            if ((miAmec.farmaindustria == true || rblFarmaindustria.SelectedValue == "1") && chkemail.Checked && Request.Form["TornarEnviarFarma"] == "true" && miAmec.idestado != 5)
                            {
                                EnviaMailFarmaIndustria();
                            }
                            //else if (miAmec.farmaindustria == false && Request.Form["TornarEnviarFarma"] == "true") Alert.Show("No se ha enviado el correo porque tiene bien la documentación");

                        }
                        else Alert.Show("No se ha guardado bien la documentación");
                    }
                }
                else
                {
                    int benGuardat = 0;
                    //string[] nombreDocumento = savePathDocumentacion.Split('/');
                    FileInfo infofile = new FileInfo(savePathDocumentacion);

                    string extensionDocumento = System.IO.Path.GetExtension(savePathDocumentacion);

                    benGuardat = agAmecInfo.GuardarDocumentacion(miAmec.idamecs, datosUsuarioAMEC.IdPeticionario, infofile.Name, savePathDocumentacion, txtComentarioDoc.Text.Trim(), chkemail.Checked, extensionDocumento, Convert.ToInt32(ddlCategoriaDocumento.SelectedValue));
                    if (benGuardat == 1)
                    {
                        lvDocumentacion.DataBind();
                        txtComentarioDoc.Text = "";
                        if ((miAmec.farmaindustria == true || rblFarmaindustria.SelectedValue == "1") && chkemail.Checked && Request.Form["TornarEnviarFarma"] == "true" && miAmec.idestado != 5)
                        {
                            EnviaMailFarmaIndustria();
                        }
                        //else if (miAmec.farmaindustria == false && Request.Form["TornarEnviarFarma"] == "true") Alert.Show("No se ha enviado el correo porque tiene bien la documentación");

                    }
                    else Alert.Show("No se ha guardado bien la documentación");
                }

            }
            else Alert.Show("No se ha subido bien la documentación");
            //}
            //else Alert.Show("Solo puede adjuntar Documentación el Solicitante y Creador del AMEC y el Gestor de Archivos");

        }


        protected void imgAnularDocumentacion_Command(object sender, CommandEventArgs e)
        {
            if (datosUsuarioAMEC.IdPeticionario == datosUsuarioSolicitante.IdPeticionario)
            {
                AgenteAmecInfo agenteAmecInf = new AgenteAmecInfo();
                agenteAmecInf.EliminarDocumentacion(Convert.ToInt32(e.CommandArgument));
                lvDocumentacion.DataBind();
            }
            else Alert.Show("Solo Puede Eliminar los Documentos el Solicitante del Amec");
        }


        protected void imgAnularPrograma_Command(object sender, CommandEventArgs e)
        {
            if (datosUsuarioAMEC.IdPeticionario == datosUsuarioSolicitante.IdPeticionario)
            {
                estaAnulandoPrograma = true;
                ActualizarAmec();
                lvPrograma.DataBind();
                estaAnulandoPrograma = false;
                ddlArchivoPrograma.Enabled = true;
                btnAdjuntarPrograma.Enabled = true;
            }
            else Alert.Show("Solo Puede Eliminar el Programa el Solicitante del Amec");
        }


        protected void imgComentarioEliminar_Command(object sender, CommandEventArgs e)
        {
            long idcomentario = Int64.Parse(e.CommandArgument.ToString());
            AgenteComentariosAMEC agCom = new AgenteComentariosAMEC();
            int i = agCom.ComprobarComentarioEsTuyo(Convert.ToInt32(idcomentario), datosUsuarioAMEC.IdPeticionario.ToString());
            if (i != 0 || datosUsuarioAMEC.IdPeticionario == datosUsuarioSolicitante.IdPeticionario)
            {
                AgenteComentariosAMEC agComentarios = new AgenteComentariosAMEC();
                int resultado = agComentarios.EliminarComentarioAMEC(idcomentario);
                txtComentario.Text = "";
                lvComentarios.DataBind();
            }
            else Alert.Show("No se puede eliminar el Comentario");
        }

        #endregion

        #region Excel

        protected int UploadButton(string tipodeCarga)
        {
            try
            {
                savePath = ConfigUtil.GetAppSetting("AMECUpload");
                int bienSubido = 0;

                byte[] input = new byte[6291456];
                if ((tipodeCarga == "Documentacion" && fuploadDocumentacion.FileBytes.Length >= input.Length)
                    || (tipodeCarga == "Programa" && fUploadPrograma.FileBytes.Length >= input.Length))
                {
                    Alert.Show("El tamaño del documento debe ser menor a 6MB");
                    bienSubido = 0;
                    return bienSubido;
                }

                if (tipodeCarga == "Documentacion")
                {
                    if (fuploadDocumentacion.HasFile)
                    {
                        if (miAmec != null)
                        {
                            try
                            {
                                String fileName = fuploadDocumentacion.FileName;

                                savePath = string.Format("{0}{1}", savePath, miAmec.idamecs);


                                string Documentacion = @"\Documentacion";
                                savePath = string.Format("{0}{1}", savePath, Documentacion);
                                if (!System.IO.Directory.Exists(savePath))
                                {
                                    System.IO.Directory.CreateDirectory(savePath);
                                }
                                fileName = RemoveDiacritics(fileName);
                                savePath = string.Format("{0}\\{1}", savePath, fileName);

                                fuploadDocumentacion.SaveAs(savePath);

                                savePathDocumentacion = savePath.Replace("/", @"\");
                                bienSubido = 1;
                            }
                            catch (Exception ex)
                            {
                                //Ismael Ameller 09-03-2011 Envio de Mail
                                Mail mail = new Mail();
                                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                                Alert.Show("Se ha producido un error subiendo el fichero", null);
                                bienSubido = 0;
                            }
                        }

                    }
                    else
                    {
                        Alert.Show("Debe Adjuntar un fichero válido al AMEC", null);
                        bienSubido = 0;
                    }

                }

                else
                {
                    if (fUploadPrograma.HasFile)
                    {
                        if (miAmec != null)
                        {
                            try
                            {
                                String fileName = fUploadPrograma.FileName;

                                savePath = string.Format("{0}{1}", savePath, miAmec.idamecs);


                                string Programa = @"\Programa";
                                savePath = string.Format("{0}{1}", savePath, Programa);
                                if (!System.IO.Directory.Exists(savePath))
                                {
                                    System.IO.Directory.CreateDirectory(savePath);
                                }
                                fileName = RemoveDiacritics(fileName);
                                savePath = string.Format("{0}\\{1}", savePath, fileName);

                                fUploadPrograma.SaveAs(savePath);

                                savePathPrograma = savePath.Replace("\\", "/");
                                miAmec.programaamecs = fileName;
                                bienSubido = 1;
                            }
                            catch (Exception ex)
                            {
                                //Ismael Ameller 09-03-2011 Envio de Mail
                                Mail mail = new Mail();
                                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                                Alert.Show("Se ha producido un error subiendo el Programa", null);
                                bienSubido = 0;
                            }
                        }

                    }
                    else
                    {
                        Alert.Show("Debe Adjuntar el Programa válido al AMEC", null);
                        bienSubido = 0;
                    }

                }
                return bienSubido;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void imgDownloadDocumentacion_Command(object sender, CommandEventArgs e)
        {
            Response.Redirect("DownloadProgramDoc.aspx?Tipodoc=Documentacion" + "&idamec=" + miAmec.idamecs + "&file=" + e.CommandName);
        }

        protected void imgDownloadPrograma_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName != "")
            {
                Response.Redirect("DownloadProgramDoc.aspx?Tipodoc=Programa" + "&idamec=" + miAmec.idamecs + "&file=" + e.CommandName);
            }
            else Alert.Show("No Existe ningun Fichero del Programa Adjuntado");
        }

        #endregion

        #region Botones

        protected void btnAnadirUnidadesOrgAMEC_Click(object sender, EventArgs e)
        {
            LogicaUnidadesOrgAMEC aprob = new LogicaUnidadesOrgAMEC();
            if (!aprob.ExisteAprobador(int.Parse(ddlUnidadMas.SelectedValue), int.Parse(ddlAreaMas.SelectedValue), int.Parse(ddlRegionMas.SelectedValue), int.Parse(ddlDistritoMas.SelectedValue)))
            {
                Alert.Show("No existen aprobadores para esta combinación. Por favor, seleccione otra");
            }
            else
            {
                //Comprobar si el AMEC esta ya guardado/insertado.
                if (datosUsuarioAMEC.IdPeticionario == datosUsuarioSolicitante.IdPeticionario)
                {
                    AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
                    AgenteComentariosAMEC agComAmec = new AgenteComentariosAMEC();
                    string idamec = miAmec.idamecs;
                    if (ValidarCampos())
                    {
                        if (agAmecInfo.EstaGuardadoAmec(miAmec.idamecs) == true)
                        {
                            //AMEC ya insertado solo añadimos la unidad organizativa correspondiente.
                            ActualizarAmec();
                            int resultado = AnadirUnidadOrgAMEC();
                            lvUnidadesOrg.DataBind();
                            if (miAmec.idestado != 5 && resultado != 0)
                            {
                                Alert.Show("Es NECESARIO RESOMETER para que el flujo sea correcto, debido a la modificación de las unidades Organizativas");
                                //agAmecInfo.CambiarEstadoAmec("35", miAmec.idamecs.ToString(),
                                //datosUsuarioAMEC.IdPeticionario.ToString() != null ? datosUsuarioAMEC.IdPeticionario.ToString() : null,
                                //datosUsuarioAMEC.idunidad != null ? datosUsuarioAMEC.idunidad.Value.ToString() : "null",
                                //datosUsuarioAMEC.Idarea != null ? datosUsuarioAMEC.Idarea.Value.ToString() : "null",
                                //datosUsuarioAMEC.idregion != null ? datosUsuarioAMEC.idregion.Value.ToString() : "null",
                                //datosUsuarioAMEC.iddistrito != null ? datosUsuarioAMEC.iddistrito.Value.ToString() : "null");
                                string comentario = string.Format("Se ha cambiado el estado del amec al INSERTAR una Unidad Organizativa: Unidad: {0}, Area: {1}, Región: {2}, Distrito: {3}", ddlUnidadMas.SelectedItem.Text, ddlAreaMas.SelectedItem.Text, ddlRegionMas.SelectedItem.Text, ddlDistritoMas.SelectedItem.Text);
                                agComAmec.InsertComentarioAMEC(miAmec.idamecs.ToString(), datosUsuarioAMEC.IdPeticionario.ToString(), comentario);
                                
                                btnVolver.Visible = false;
                                //btnGuardar.Visible = false;
                                //btnCancelar.Visible = false;
                                lvComentarios.DataBind();
                                AgenteAmecInfo agenteAMEC = new AgenteAmecInfo();
                                lbEstadoAMEC.Text = agenteAMEC.ObtenerNombreEstado(35);
                            }
                        }
                        else
                        {
                            //AMEC no insertado. Guardamos valores del amec e insertamos amec y la unidad organizativa por defecto.
                            GuardarNuevoAmec();
                            AnadirUnidadOrgAMEC();
                            lvUnidadesOrg.DataBind();
                        }
                    }
                }
                else Alert.Show("Solo puede Adjuntar las Unidades Organizativas el Solicitante del Amec");
            }

        }

        protected void btnNuevoAMEC_Click(object sender, ImageClickEventArgs e)
        {
            GuardarNuevoAmec();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (ValidateFechaFiltro())
            {
                this.eosContentResults.Visible = true;
                lvActivid.DataBind();
            }

        }

        protected void btnPublicarComentario_Click(object sender, ImageClickEventArgs e)
        {
            //codigo PUBLICAR COMENTARIO
            AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
            RepositorioFlujo estadoAMEC = null;
            CargarValoreAmecFormulario();
            if (agAmecInfo.EstaGuardadoAmec(miAmec.idamecs) != true)
            {
                if (ValidarCampos())
                {
                    //AMEC no insertado. Guardamos valores del amec e insertamos amec y la unidad organizativa por defecto.
                    GuardarNuevoAmec();
                }
            }
            else
            {
                estadoAMEC = new RepositorioFlujo(miAmec.idamecs, (Int32)datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
                //Session.Add("estadoAMEC", estadoAMEC);
                HabilitaBotones(estadoAMEC);
            }
            AgenteComentariosAMEC agComentario = new AgenteComentariosAMEC();
            if (txtComentario.Text != "")
            {
                int retorn = agComentario.InsertComentarioAMEC(miAmec.idamecs.ToString(), datosUsuarioAMEC.IdPeticionario.ToString(), txtComentario.Text);
                EnviaMailPublicarComentario();
                //btnAprobarConCondicion.OnClientClick = "return confirm('Has Introducido un Comentario. ¿Desea Aprobar con la Condición del Comentario?');";
                lvComentarios.DataBind();
                txtComentario.Text = "";
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Guardando = true;
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            if (ComprobarUnidadOrganizativa())
            {
                if (ValidarCampos() && ValidarCamposObligatorios(miAmec))
                {
                    if (ComprobarImporte())
                    {

                        CargarValoreAmecFormulario();

                        if (agAmecInf.EstaGuardadoAmec(miAmec.idamecs) == true)
                        {
                            if (miAmec.idestado != 5)
                            {

                                bool YaSeHaEnviadoAFarma = agAmecInf.ComprobarSiSeEnvioAFarma(miAmec.idamecs);
                                bool YaSeHaEnviadoACasosClinicos = agAmecInf.ComprobarSiSeEnvioACasosClinicos(miAmec.idamecs);
                                if (!YaSeHaEnviadoAFarma && miAmec.farmaindustria == true)
                                {
                                    Alert.Show("Se ha enviado el correo de FarmaIndustria");
                                    YaSeHaEnviadoAFarma = true;
                                    EnviaMailFarmaIndustria();
                                }
                                if (!YaSeHaEnviadoACasosClinicos && miAmec.casosclinicos == true)
                                {
                                    EnviaMailCasosClinicos();
                                    YaSeHaEnviadoACasosClinicos = true;
                                    Alert.Show("Se ha enviado el correo de CasosClinicos");
                                }
                                this.btnClonar.OnClientClick = "return ConfirmarResometer('" + YaSeHaEnviadoAFarma + "','" + YaSeHaEnviadoACasosClinicos + "')";
                            }
                            ActualizarAmec();
                        }
                        else
                        {
                            GuardarNuevoAmec();
                        }
                        lvPrograma.DataBind();
                        Alert.Show(string.Format("Se ha creado correctament el AMEC con Número: {0}", miAmec.idamecs));
                    }

                    else Alert.Show("No se puede Guardar el Amec. Es Obligatorio Resometer debido a la modificación del Importe.");
                }
            }
        }

        protected void btnClonar_Click(object sender, EventArgs e)
        {
            string idAmecOriginal = miAmec.idamecs;
            AgenteAmecInfo agAMECinfo = new AgenteAmecInfo();
            CargarValoreAmecFormulario();

            //El amec anterior se cancela
            miAmec.idestado = Convert.ToUInt32(Estado.Cancelado);
            DAmecInfo DAmecInf = new DAmecInfo();
            bool cambioAgencia = false;
            DAmecInf = agAMECinfo.ModificarAMEC(miAmec, out cambioAgencia, true);

            //El nuevo se genera en estado borrador
            miAmec.idamecs = agAMECinfo.DameSiguienteIdAmecs();
            miAmec.idestado = Convert.ToUInt32(Estado.Borrador);
            
            //El creador es el creador
            miAmec.idcreadopor = miAmec.idcreadopor;
            
            //El solicitante es el solicitante original
            miAmec.idsolicitante = miAmec.idsolicitante;

            //Se traspasa toda la información relativa a expedientes... junto con sus documentaciones
            //Se añade un campo con un comentario en amec original y amec destino
            miAmec = agAMECinfo.ClonarAMEC(miAmec, idAmecOriginal);

            //Documentación del amec
            string SourcePath = ConfigUtil.GetAppSetting("AMECUpload");
            SourcePath = string.Format("{0}{1}", SourcePath, idAmecOriginal);
            string DestinationPath = ConfigUtil.GetAppSetting("AMECUpload");
            DestinationPath = string.Format("{0}{1}", DestinationPath, miAmec.idamecs);

            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);

            EnviaMailClonado(idAmecOriginal.ToString(), miAmec.idamecs.ToString(), datosUsuarioAMEC.Nombre + " " + datosUsuarioAMEC.Apellido1 + "(IdPeticionario: " + datosUsuarioAMEC.IdPeticionario.ToString() + ")");
            Alert.Show(string.Format("Se ha clonado-sometido correctamente el AMEC con Número {0} sobre el AMEC con Número {1}", idAmecOriginal, miAmec.idamecs), "DetalleAMEC.aspx?idamec=" + miAmec.idamecs);
        }


        public void EnviaMailClonado(string idamecOriginal, string idamecDestino, string usuario)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            int GuardarMail;
            RepositorioFlujo estadoAMEC = null;
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            try
            {
                AgenteUsuarios agUsu = new AgenteUsuarios();
                DDatosPersonalesUsuario dSolicitante = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());
                message.To.Add(new MailAddress(dSolicitante.Email));

                if (MailCopia.Contains('@'))
                {
                    foreach (string s in MailCopia.Split(';'))
                    {
                        message.To.Add(new MailAddress(s));
                    }
                }
                    
                if (MailCopiaProgramador.Contains('@'))
                {
                    foreach (string s in MailCopiaProgramador.Split(';'))
                    {
                        message.To.Add(new MailAddress(s));
                    }
                }

                message.Subject = string.Format("Amec Clonado-sometido: {0} -> {1}", idamecOriginal, idamecDestino);
                message.Body = string.Format("El usuario {2} ha clonado-sometido correctamente el AMEC con Número {0} sobre el AMEC con Número {1}", idamecOriginal, idamecDestino, usuario);
                
                message.IsBodyHtml = true;

                Mail.EnviaMail(message);
                GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "AmecClonado", message.To.ToString(), message.Subject, message.Body, null, true, null);
            }
            catch (Exception ex)
            {
                Alert.Show("Se ha producido un error enviando el mail de Clonado de Amec", null);
                GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "AmecClonado", message.To.ToString(), message.Subject, message.Body, null, false, "Error enviando el Correo");
                Logger.Logger.PrintError(this.GetType().Name, "amecNotificaciones", ex.Message, ex);
                try
                {
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("contactoError"), ex);
                }
                catch { }
            }
        }

        protected void btnAprobarConCondicion_Click(object sender, ImageClickEventArgs e)
        {
            estadoAmecAnterior = Convert.ToInt32(miAmec.idestado);
            RepositorioFlujo estadoAMEC = null;
            AgenteUsuarios agenteUsua = new AgenteUsuarios();
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            if (agAmecInf.EstaGuardadoAmec(miAmec.idamecs) == true)
            {
                //AMEC ya insertado solo modificamos(UPDATE) el amec.
                ActualizarAmec();
            }
            else
            {
                //AMEC no insertado. Guardamos valores del amec e insertamos amec y la unidad organizativa por defecto.
                GuardarNuevoAmec();
            }
            //estadoAMEC = (RepositorioFlujo)Session["estadoAMEC"];
            estadoAMEC = new RepositorioFlujo(miAmec.idamecs, (Int32)datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
            bool BienAprobado = estadoAMEC.Aceptar(1);
            if (BienAprobado)
            {
                ObtenerAMEC(miAmec.idamecs);
                if (miAmec.idestado == 1)
                {
                    EnviaMailAprobacionFinal();
                }
                else
                {
                    if (estadoAmecAnterior != miAmec.idestado)
                    {
                        List<string> collectCorreo = estadoAMEC.ObtenerCorreoParaEnviar(miAmec.idamecs, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
                        if (collectCorreo.Count != 0)
                        {
                            EnviaMailPendienteAprobar(collectCorreo);
                        }
                    }
                }
                EnviaMailAprobacionCondicionada();
                string sNavegacion = "ListadoAMECs.aspx";
                Alert.Show(string.Format("Se ha Aprobado Correctamente el AMEC con Número: {0}", miAmec.idamecs), sNavegacion);

            }
            else Alert.Show("No se ha podido Aprobar el AMEC debido a problemas internos");
            //estadoAMEC.Aceptar("Aquí hi hem de posar un comentari");
        }

        protected void btnAprobar_Click(object sender, ImageClickEventArgs e)
        {
            estadoAmecAnterior = Convert.ToInt32(miAmec.idestado);
            AgenteUsuarios agenteUsua = new AgenteUsuarios();
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            RepositorioFlujo estadoAMEC = null;
            if (agAmecInf.EstaGuardadoAmec(miAmec.idamecs) == true)
            {
                //AMEC ya insertado solo modificamos(UPDATE) el amec.
                ActualizarAmec();
            }
            else
            {
                //AMEC no insertado. Guardamos valores del amec e insertamos amec y la unidad organizativa por defecto.
                GuardarNuevoAmec();
            }
            estadoAMEC = new RepositorioFlujo(miAmec.idamecs, (Int32)datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
            bool BienAprobado = estadoAMEC.Aceptar(0);
            if (BienAprobado)
            {

                //Si se ha aprobado el Amec se enviarà un correo al Solicitante del Amec
                //IDEstado Aprobado = 1
                ObtenerAMEC(miAmec.idamecs);
                if (miAmec.idestado == 1)
                {
                    EnviaMailAprobacionFinal();
                }
                else
                {
                    if (estadoAmecAnterior != miAmec.idestado)
                    {
                        //estadoAMEC = new RepositorioFlujo(miAmec.idamecs, (Int32)datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
                        List<string> collectCorreo = estadoAMEC.ObtenerCorreoParaEnviar(miAmec.idamecs, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
                        if (collectCorreo.Count != 0)
                        {
                            EnviaMailPendienteAprobar(collectCorreo);
                        }
                    }
                }

                string sNavegacion = "ListadoAMECs.aspx";
                Alert.Show(string.Format("Se ha Aprobado Correctamente el AMEC con Número: {0}", miAmec.idamecs), sNavegacion);

            }
            else Alert.Show("No se ha podido Aprobar el AMEC debido a problemas internos");

        }

        protected void btnRechazar_Click(object sender, ImageClickEventArgs e)
        {
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            RepositorioFlujo estadoAMEC = null;
            if (agAmecInf.EstaGuardadoAmec(miAmec.idamecs) == true)
            {
                //AMEC ya insertado solo modificamos(UPDATE) el amec.
                ActualizarAmec();
            }
            else
            {
                //AMEC no insertado. Guardamos valores del amec e insertamos amec y la unidad organizativa por defecto.
                GuardarNuevoAmec();
            }

            //estadoAMEC = (RepositorioFlujo)Session["estadoAMEC"];
            estadoAMEC = new RepositorioFlujo(miAmec.idamecs, (Int32)datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
            //agAMEC.nIDAmec = miAmec.idamecs;
            if (!agAMEC.EsCancelable())
                Alert.Show("No se puede Rechazar el Amec porque tiene reservas asociadas en curso");
            else
            {
                bool BienRechazado = estadoAMEC.Rechazar(txtComentario.Text);
                if (BienRechazado)
                {
                    EnviaMailRechazado();
                    string sNavegacion = "ListadoAMECs.aspx";
                    Alert.Show(string.Format("Se ha Rechazado Correctamente el AMEC con Número: {0}", miAmec.idamecs), sNavegacion);
                    //string UrlVolver = PaginaRetorno();
                }
                else Alert.Show("No se ha podido Rechazar el AMEC debido a problemas internos");
            }

        }

        protected void btnSometer_Click(object sender, ImageClickEventArgs e)
        {
            bool emailsent = false;
            Sometiendo = true;

            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            RepositorioFlujo estadoAMEC = null;
            if (ValidarCamposObligatorios(miAmec))
            {
                if (ValidarCampos() && ComprobarLibros() && ComprobarUnidadOrganizativa())
                {
                    if (agAmecInf.EstaGuardadoAmec(miAmec.idamecs) == true)
                    {
                        //AMEC ya insertado solo modificamos(UPDATE) el amec.
                        ActualizarAmec();
                    }
                    else
                    {
                        //AMEC no insertado. Guardamos valores del amec e insertamos amec y la unidad organizativa por defecto.
                        GuardarNuevoAmec();
                    }

                    estadoAMEC = new RepositorioFlujo(miAmec.idamecs, (Int32)datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
                    //estadoAMEC = (RepositorioFlujo)Session["estadoAMEC"];
                    bool BienSometido = estadoAMEC.Someter();

                    if (BienSometido)
                    {
                        ObtenerAMEC(miAmec.idamecs);
                        if (miAmec.farmaindustria == true && Request.Form["TornarEnviarFarma"] == "true")
                        {
                            EnviaMailFarmaIndustria();
                        }
                        if (miAmec.casosclinicos == true && Request.Form["TornarEnviarCasosClinicos"] == "true")
                        {
                            EnviaMailCasosClinicos();
                        }
                        if (miAmec.idestado == 1)
                        {
                            EnviaMailAprobacionFinal();
                        }
                        else
                        {
                            List<string> collectCorreo = estadoAMEC.ObtenerCorreoParaEnviar(miAmec.idamecs, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
                            if (collectCorreo.Count != 0)
                            {
                                EnviaMailPendienteAprobar(collectCorreo);
                                emailsent = true;
                            }

                        }
                        if (emailsent)
                        {
                            //Nuevo Requisito Informe FCPA
                            agAmecInf.guardarEnInformeFCPA(miAmec.idamecs, miAmec.idtipoactividad.ToString(), miAmec.descripcion, miAmec.fechacomienzo,
                                miAmec.fechafinalizacion, miAmec.idestado, 1, "", "", "", "", "0");
                            AgenteComentariosAMEC agenteComentarios = new AgenteComentariosAMEC();
                            //string comentario = "Certifico la revisión del nivel de riesgo de los profesionales sanitarios incluidos en esta actividad, con respecto a la misma según lo establecido en Pol. Corp. Nº 20/FCPA";
                            string comentario = "Certifico la revisión del nivel de riesgo de los profesionales sanitarios incluidos en esta actividad, con respecto a la misma según lo establecido en C.Pol 5/FCPA";
                            agenteComentarios.InsertComentarioAMEC(
                                miAmec.idamecs.ToString(), datosUsuarioAMEC.IdPeticionario.ToString(), comentario);
                            string sNavegacion;
                            if (sURL.Contains("NuevoExpedientePasoA.aspx")) sNavegacion = sURL;
                            else sNavegacion = "ListadoAMECs.aspx";

                            Alert.Show(string.Format("Se ha Sometido Correctamente el AMEC con Número: {0}", miAmec.idamecs), sNavegacion);
                        }
                        else
                        {
                            Alert.Show("No existen aprobadores para esa combinación de Areas de empresa. Por favor seleccione otra");
                        }
                    }
                    else Alert.Show("No se ha podido Someter el AMEC debido a problemas internos");

                    AgenteAmecInfo agenteAMEC = new AgenteAmecInfo();
                    miAmec = agenteAMEC.CargarTodosValoresAmec(miAmec.idamecs);
                    ImagenEstadoAMEC();
                    lbEstadoAMEC.Text = agenteAMEC.ObtenerNombreEstado(miAmec.idestado);
                    //if (miAmec.idestado != 5) this.btnSometer.OnClientClick = "return confirm('Este Amec Ya está Sometido. ¿Desea Resometer el AMEC?');";
                }
            }
        }

        protected void btnCancelar_Click(object sender, ImageClickEventArgs e)
        {
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            if (agAmecInf.EstaGuardadoAmec(miAmec.idamecs) == true)
            {
                //AMEC ya insertado solo modificamos(UPDATE) el amec.
                ActualizarAmec();
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
                ActualizarAmec();
                int benguardathistorial = agAmecInf.GuardarEstadoAHistorialAMEC(miAmec.idamecs, Convert.ToInt32(miAmec.idestado), datosUsuarioAMEC.IdPeticionario, miAmec);
                EnviaMailCancelado();
                string sNavegacion;
                if (sURL.Contains("NuevoExpedientePasoA.aspx")) sNavegacion = sURL;
                else sNavegacion = "ListadoAMECs.aspx";

                Alert.Show(string.Format("Se ha Cancelado el AMEC con Número: {0}", miAmec.idamecs), sNavegacion);
                //si es un amec cancelado no podrás utilizarlo para crear un Expediente No se tiene en cuenta
            }

        }

        protected void btnVolver_Click(object sender, ImageClickEventArgs e)
        {

        }

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
                throw ex;
            }
        }

        public void btnExportarInfoExpediente_Click(object sender, ImageClickEventArgs e)
        {

            AgenteAmecInfo agAMECinfo = new AgenteAmecInfo();
            int numcongreso = agAMECinfo.ObtenerNumeroActividadesAsigAmec(miAmec.idamecs);
            int AmecRelacionadoConExpediente = agAMECinfo.HayAmecRelacionExpediente(miAmec.idamecs);
            if (AmecRelacionadoConExpediente != 0)
            {
                if (AmecRelacionadoConExpediente == 1)
                {
                    ImportarDatosExpediente(0);
                }
                else
                {
                    if (ddlExpedientesAmec.Visible == true)
                    {
                        ImportarDatosExpediente(Int32.Parse(ddlExpedientesAmec.SelectedItem.Value));
                        ddlExpedientesAmec.Visible = false;
                    }
                    else
                    {
                        AgenteExpedientes agExp = new AgenteExpedientes();
                        Alert.Show("El AMEC es paraguas y tienes que identificar de que Expediente quieres Importar los datos");
                        ddlExpedientesAmec.Visible = true;
                        ddlExpedientesAmec.DataSource = agExp.ExpedientesPorAmec(miAmec.idamecs);
                        ddlExpedientesAmec.DataBind();
                    }
                }
            }
            else
            {
                Alert.Show("El AMEC no puede recuperar la información ya que no esta asociado a ningun expediente.");
            }

        }




        #endregion

        #region Eventos

        protected void odsUnidadesOrganizativas_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            //AQUÍ S'HAN D'ADJUNTAR ELS VALORS DEL (DATA SOURCE) AMB ELS DELS (COMBOBOX, TEXTBOX,...)

            if (Session["UnidOrgXDefecto"] != null)
                unidOrgXDefecto = (DUnidadesOrganizativasAmec)Session["UnidOrgXDefecto"];

            if (!string.IsNullOrEmpty(miAmec.idamecs.ToString())) e.InputParameters["filtroidamec"] = miAmec.idamecs.ToString();
            if (!string.IsNullOrEmpty(miAmec.idsolicitante.ToString())) e.InputParameters["filtroidsolicitante"] = miAmec.idsolicitante.ToString();

            if (!string.IsNullOrEmpty(unidOrgXDefecto.idunidad.ToString())) e.InputParameters["filtroidUnidad"] = unidOrgXDefecto.idunidad.ToString();
            if (!string.IsNullOrEmpty(unidOrgXDefecto.idarea.ToString())) e.InputParameters["filtroidArea"] = unidOrgXDefecto.idarea.ToString();
            if (!string.IsNullOrEmpty(unidOrgXDefecto.idregion.ToString())) e.InputParameters["filtroidRegion"] = unidOrgXDefecto.idregion.ToString();
            if (!string.IsNullOrEmpty(unidOrgXDefecto.iddistrito.ToString())) e.InputParameters["filtroidDistrito"] = unidOrgXDefecto.iddistrito.ToString();

            if (!string.IsNullOrEmpty(AccionUOCambioSolicitante.ToString())) e.InputParameters["filtroAccion"] = AccionUOCambioSolicitante.ToString();


            if (!string.IsNullOrEmpty(unidOrgXDefecto.idunidad.ToString())) e.InputParameters["filtroUnidad"] = "";
            if (!string.IsNullOrEmpty(unidOrgXDefecto.idarea.ToString())) e.InputParameters["filtroArea"] = "";
            if (!string.IsNullOrEmpty(unidOrgXDefecto.idregion.ToString())) e.InputParameters["filtroRegion"] = "";
            if (!string.IsNullOrEmpty(unidOrgXDefecto.iddistrito.ToString())) e.InputParameters["filtroDistrito"] = "";

            if (!Page.IsPostBack)
            {
                e.Arguments.MaximumRows = 10;
                e.Arguments.StartRowIndex = 0;
                //e.InputParameters[1] = "2";
                //e.Arguments.SortExpression.
            }
            else
            {
                //oFiltroExp.startRowIndex = e.Arguments.StartRowIndex;
                //oFiltroExp.sortParameter = e.Arguments.SortExpression;
            }
            //e.InputParameters["mes"] = "hola";

        }

        protected void lvActividadAsigAmec_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Codi del lvActividadesAsigAmec
            lvActividadAsigAmec.DataBind();

        }

        protected void lvActivid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvActivid.SelectedValue != null)
            {
                SeleccionaActividad(lvActivid.SelectedValue.ToString(), null);

            }
        }

        protected void odsComentariosAMEC_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            //AQUÍ S'HAN D'ADJUNTAR ELS VALORS DEL (DATA SOURCE) AMB ELS DELS (COMBOBOX, TEXTBOX,...)
            if (!string.IsNullOrEmpty(miAmec.idamecs.ToString())) e.InputParameters["filtroidamec"] = miAmec.idamecs.ToString();

            if (!Page.IsPostBack)
            {
                e.Arguments.MaximumRows = 10;
                e.Arguments.StartRowIndex = 0;
            }

        }

        protected void lvUnidadesOrg_Load(object sender, EventArgs e)
        {
            //lvDelegaciones.DataBind();
        }

        public void itemXDefecto(DropDownList ddlnombre)
        {
            ListItem itemXdefecto = new ListItem();
            itemXdefecto.Value = "-1";
            itemXdefecto.Text = "Seleccionar";
            ddlnombre.Items.Add(itemXdefecto);
        }

        protected void odsActividadesAsigAmec_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            RecuperaSesion();
            if (Page.IsPostBack) miAmec.idamecs = lbidAMEC.InnerText;
            if (!string.IsNullOrEmpty(miAmec.idamecs.ToString())) e.InputParameters["filtroIdAMEC"] = miAmec.idamecs;
        }

        protected void odsActividades_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            RecuperaSesion();
            if (Page.IsPostBack) miAmec.idamecs = lbidAMEC.InnerText;
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim())) e.InputParameters["filtroNombre"] = "%" + txtNombre.Text.Trim() + "%";
            if (!string.IsNullOrEmpty(ddlPoblacion.SelectedValue.Trim())) e.InputParameters["filtroPoblacion"] = ddlPoblacion.SelectedValue.Trim();
            if (!string.IsNullOrEmpty(txtAMEC.Text.Trim())) e.InputParameters["filtroAMEC"] = "%" + txtAMEC.Text.Trim() + "%";
            if (!string.IsNullOrEmpty(miAmec.idamecs.ToString().Trim())) e.InputParameters["filtroIdAMEC"] = miAmec.idamecs.ToString();
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
        }

        protected void odsDocumentacionAMEC_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            RecuperaSesion();
            if (Page.IsPostBack) miAmec.idamecs = lbidAMEC.InnerText;
            if (!string.IsNullOrEmpty(miAmec.idamecs.ToString())) e.InputParameters["filtroidamec"] = miAmec.idamecs.ToString();

        }

        protected void odsProgramaAMEC_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            RecuperaSesion();
            if (Page.IsPostBack) miAmec.idamecs = lbidAMEC.InnerText;
            if (!string.IsNullOrEmpty(miAmec.idamecs.ToString())) e.InputParameters["filtroidamec"] = miAmec.idamecs.ToString();

        }

        protected void odsHistEstadosAMEC_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            RecuperaSesion();
            if (Page.IsPostBack) miAmec.idamecs = lbidAMEC.InnerText;
            if (!string.IsNullOrEmpty(miAmec.idamecs.ToString())) e.InputParameters["filtroidamec"] = miAmec.idamecs.ToString();

            if (!Page.IsPostBack)
            {

                e.Arguments.MaximumRows = 20;
                e.Arguments.StartRowIndex = 0;
            }
        }

        protected void txtDescProgAMEC_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtLugarSede_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region Filtrar Combos


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
            miAmec.idposition = datosUsuarioSolicitante.IdPosition;
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
            ActualizaCombosUnidadOrgXDefecto();

            UpdateUnidadOrganizativaXDefecto();
            AccionUOCambioSolicitante = true;
            //btnSometer.Visible = miAmec.idsolicitante == datosUsuarioAMEC.IdPeticionario;

        }

        public void UpdateUnidadOrganizativaXDefecto()
        {
            //MARTA MESTRE UPDATE UNIDAD ORGANIZATIVA SI EXISTE
            AgenteUnidadesOrgAMEC agUnidad = new AgenteUnidadesOrgAMEC();
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            if (agAmecInf.EstaGuardadoAmec(miAmec.idamecs))
            {
                //UPDATE DE LA UNIDAD ORGANIZATIVA
                if (miAmec.paraguas != true || chkParaguas.Checked != true)
                {
                    ICollection<DUnidadesOrganizativasAmec> unidadesOrgAMEC = agUnidad.ObtenerUnidadesOrgAmec(miAmec.idamecs.ToString(), miAmec.idsolicitante.ToString(), "", "", "", "", "true", "", "", "", "", "", 0, 10);
                    DUnidadesOrganizativasAmec unidadxdefecto = unidadesOrgAMEC.ElementAt<DUnidadesOrganizativasAmec>(0);
                    unidOrgXDefecto.idunidadamec = unidadxdefecto.idunidadamec;
                    agUnidad.ModificarUnidadOrgId(unidOrgXDefecto);
                }
            }
            else
            {
                //MARTA JUEVES2
                if (ddlArea.SelectedValue == "0" || ddlArea.SelectedValue == "-1")
                {
                    unidOrgXDefecto.idarea = null;
                }
                else
                {
                    unidOrgXDefecto.idarea = Int32.Parse(ddlArea.SelectedValue.ToString());
                }

                if (ddlRegion.SelectedValue == "0" || ddlRegion.SelectedValue == "-1")
                {
                    unidOrgXDefecto.idregion = null;
                }
                else
                {
                    unidOrgXDefecto.idregion = Int32.Parse(ddlRegion.SelectedValue.ToString());
                }

                if (ddlDistrito.SelectedValue == "0" || ddlDistrito.SelectedValue == "-1")
                {
                    unidOrgXDefecto.iddistrito = null;
                }
                else
                {
                    unidOrgXDefecto.iddistrito = Int32.Parse(ddlDistrito.SelectedValue.ToString());
                }

                unidOrgXDefecto.idcreadopor = Int32.Parse(ddlSolicitante.SelectedValue.ToString());
                unidOrgXDefecto.idunidad = Int32.Parse(ddlUnidad.SelectedValue.ToString());


                //unidOrgXDefecto.idcreadopor = Int32.Parse(ddlSolicitante.SelectedValue.ToString());
                //unidOrgXDefecto.idarea =  Int32.Parse(ddlArea.SelectedValue.ToString());
                //unidOrgXDefecto.idunidad = Int32.Parse(ddlUnidad.SelectedValue.ToString());
                //unidOrgXDefecto.idregion = Int32.Parse(ddlRegion.SelectedValue.ToString());
                //unidOrgXDefecto.iddistrito = Int32.Parse(ddlDistrito.SelectedValue.ToString());          
            }

            lvUnidadesOrg.DataBind();
        }

        public void ddlAgencias_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void ddlUnidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cuando el combo de Unidades se seleccione entonces se filtran los combos de area,region e distrito. 
            AccionUOCambioSolicitante = false;

            BDManage vectArea;
            BDManage vectRegion;
            BDManage vectDistrito;
            lvPrograma.DataBind();

            vectArea = new BDManage(SQLSentenceAMEC.GetInstancia().getAreasUnidad(ddlUnidad.SelectedValue.ToString()), 1);
            rellenaArea(vectArea);
            vectRegion = new BDManage(SQLSentenceAMEC.GetInstancia().getRegionesUnidades(ddlUnidad.SelectedValue.ToString()), 1);
            rellenaRegion(vectRegion);
            vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoUnidad(ddlUnidad.SelectedValue.ToString()), 1);
            rellenaDistrito(vectDistrito);
            UpdateUnidadOrganizativaXDefecto();
        }

        public void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SEGUN LA PAGINA DE LISTADO E INFORMES AL SELECCIONAR AREA NOSE FILTRA NADA MAS.   
            AccionUOCambioSolicitante = false;
            Session.Add("AccionUO", AccionUOCambioSolicitante);
            lvPrograma.DataBind();
            UpdateUnidadOrganizativaXDefecto();

        }

        public void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cuando el combo de Region se seleccione entonces se filtran los distritos
            AccionUOCambioSolicitante = false;
            Session.Add("AccionUO", AccionUOCambioSolicitante);
            BDManage vectDistrito = new BDManage();
            lvPrograma.DataBind();


            if (!string.IsNullOrEmpty(ddlRegion.SelectedItem.Value) && !string.IsNullOrEmpty(ddlArea.SelectedItem.Value))
            {
                vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoRegionArea(ddlRegion.SelectedItem.Value, ddlArea.SelectedItem.Value), 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(ddlRegion.SelectedItem.Value) && string.IsNullOrEmpty(ddlArea.SelectedItem.Value))
                    vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoRegion(ddlRegion.SelectedItem.Value), 1);
                if (string.IsNullOrEmpty(ddlRegion.SelectedItem.Value) && !string.IsNullOrEmpty(ddlArea.SelectedItem.Value))
                    vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoArea(ddlArea.SelectedItem.Value), 1);
            }

            rellenaDistrito(vectDistrito);
            UpdateUnidadOrganizativaXDefecto();
        }

        public void ddlDistrito_SelectedIndexChanged(object sender, EventArgs e)
        {

            AccionUOCambioSolicitante = false;
            Session.Add("AccionUO", AccionUOCambioSolicitante);
            UpdateUnidadOrganizativaXDefecto();
        }

        public void ddlUnidadMas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Cuando el combo de Unidades se seleccione entonces se filtran los combos de area,region e distrito.           
            BDManage vectArea;
            BDManage vectRegion;
            BDManage vectDistrito;
            lvPrograma.DataBind();

            vectArea = new BDManage(SQLSentenceAMEC.GetInstancia().getAreasUnidad(ddlUnidadMas.SelectedValue.ToString()), 1);
            rellenaAreaMas(vectArea);
            vectRegion = new BDManage(SQLSentenceAMEC.GetInstancia().getRegionesUnidades(ddlUnidadMas.SelectedValue.ToString()), 1);
            rellenaRegionMas(vectRegion);

            /////////// ANTIGUO/////////////
            //vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoUnidad(ddlUnidadMas.SelectedValue.ToString()), 1);
            //rellenaDistritoMas(vectDistrito);

            ////////// NUEVO ///////////
            vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoRegionAreaNuevo(ddlRegionMas.SelectedItem.Value, ddlAreaMas.SelectedItem.Value), 1);
            rellenaDistritoMas(vectDistrito);
        }

        public void ddlAreaMas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SEGUN LA PAGINA DE LISTADO E INFORMES AL SELECCIONAR AREA NOSE FILTRA NADA.   
            lvPrograma.DataBind();
            BDManage vectDistrito = new BDManage();

            /////////// ANTIGUO/////////////
            //if (!string.IsNullOrEmpty(ddlRegionMas.SelectedItem.Value) && !string.IsNullOrEmpty(ddlAreaMas.SelectedItem.Value))
            //{
            //    vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoRegionArea(ddlRegionMas.SelectedItem.Value, ddlAreaMas.SelectedItem.Value), 1);
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(ddlRegionMas.SelectedItem.Value) && string.IsNullOrEmpty(ddlAreaMas.SelectedItem.Value))
            //        vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoRegion(ddlRegionMas.SelectedItem.Value), 1);
            //    if (string.IsNullOrEmpty(ddlRegionMas.SelectedItem.Value) && !string.IsNullOrEmpty(ddlAreaMas.SelectedItem.Value))
            //        vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoArea(ddlAreaMas.SelectedItem.Value), 1);
            //}

            ////////// NUEVO ///////////
            vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoRegionAreaNuevo(ddlRegionMas.SelectedItem.Value, ddlAreaMas.SelectedItem.Value), 1);
            rellenaDistritoMas(vectDistrito);

        }

        public void ddlRegionMas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cuando el combo de Region se seleccione entonces se filtra distritos

            lvPrograma.DataBind();

            BDManage vectDistrito = new BDManage();
            /////////// ANTIGUO/////////////
            //if (!string.IsNullOrEmpty(ddlRegionMas.SelectedItem.Value) && !string.IsNullOrEmpty(ddlAreaMas.SelectedItem.Value))
            //{
            //    vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoRegionArea(ddlRegionMas.SelectedItem.Value, ddlAreaMas.SelectedItem.Value), 1);
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(ddlRegionMas.SelectedItem.Value) && string.IsNullOrEmpty(ddlAreaMas.SelectedItem.Value))
            //        vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoRegion(ddlRegionMas.SelectedItem.Value), 1);
            //    if (string.IsNullOrEmpty(ddlRegionMas.SelectedItem.Value) && !string.IsNullOrEmpty(ddlAreaMas.SelectedItem.Value))
            //        vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoArea(ddlAreaMas.SelectedItem.Value), 1);
            //}

            ////////// NUEVO ///////////
            vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoRegionAreaNuevo(ddlRegionMas.SelectedItem.Value, ddlAreaMas.SelectedItem.Value), 1);
            rellenaDistritoMas(vectDistrito);
        }

        public void ddlActividad_SelectedIndexChanged(object sender, EventArgs e)
        {
            RepositorioFlujo estadoAMEC = null;
            MostrarPorTipoActividad(ddlActividad.Text, false);
            estadoAMEC = new RepositorioFlujo(miAmec.idamecs, (Int32)datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
            HabilitaBotones(estadoAMEC);
            //NoCargarPrograma = true;
        }

        public void filtrarAreaRegion(int idunidad)
        {
            AgenteMaestros agente = new AgenteMaestros();
            this.ddlArea.DataSource = agente.ObtenerAreasXidunidad(null, idunidad);
            this.ddlArea.DataBind();

            AgenteMaestros agente1 = new AgenteMaestros();
            this.ddlRegion.DataSource = agente1.ObtenerRegionesXidunidad(null, idunidad);
            this.ddlRegion.DataBind();
        }

        public void filtrarDistrito(int idarea, int idregion)
        {
            AgenteMaestros agente = new AgenteMaestros();
            this.ddlDistrito.DataSource = agente.ObtenerDistritosFiltrado(null, idarea, idregion);
            this.ddlDistrito.DataBind();
        }

        protected void rellenaUnidadMas(BDManage VectUnidad)
        {
            ddlUnidadMas.DataSource = VectUnidad.getDataSet();
            ddlUnidadMas.DataValueField = "idUnidad";
            ddlUnidadMas.DataTextField = "unidad";
            ddlUnidadMas.DataBind();
            ListItem blankUnidad = new ListItem("Cualquier Unidad", "0");
            ddlUnidadMas.Items.Insert(0, blankUnidad);
            ddlUnidadMas.SelectedValue = "0";
        }

        protected void rellenaRegionMas(BDManage VectRegion)
        {
            ddlRegionMas.DataSource = VectRegion.getDataSet();
            ddlRegionMas.DataValueField = "idregion";
            ddlRegionMas.DataTextField = "region";
            ddlRegionMas.DataBind();
            ListItem blankRegion = new ListItem("Cualquier Region", "0");
            ddlRegionMas.Items.Insert(0, blankRegion);
            ddlRegionMas.SelectedValue = "0";
        }

        protected void rellenaAreaMas(BDManage VectArea)
        {
            ddlAreaMas.DataSource = VectArea.getDataSet();
            ddlAreaMas.DataValueField = "idarea";
            ddlAreaMas.DataTextField = "area";
            ddlAreaMas.DataBind();
            ListItem blankArea = new ListItem("Cualquier Area", "0");
            ddlAreaMas.Items.Insert(0, blankArea);
            ddlAreaMas.SelectedValue = "0";
        }

        protected void rellenaDistritoMas(BDManage VectDistrito)
        {
            ddlDistritoMas.DataSource = VectDistrito.getDataSet();
            ddlDistritoMas.DataValueField = "iddistrito";
            ddlDistritoMas.DataTextField = "distrito";
            ddlDistritoMas.DataBind();
            ListItem blankDistrito = new ListItem("Cualquier Distrito", "0");
            ddlDistritoMas.Items.Insert(0, blankDistrito);
            ddlDistritoMas.SelectedValue = "0";
        }

        protected void rellenaUnidad(BDManage VectUnidad)
        {
            ddlUnidad.DataSource = VectUnidad.getDataSet();
            ddlUnidad.DataValueField = "idUnidad";
            ddlUnidad.DataTextField = "unidad";
            ddlUnidad.DataBind();
        }

        protected void rellenaRegion(BDManage VectRegion)
        {
            ddlRegion.DataSource = VectRegion.getDataSet();
            ddlRegion.DataValueField = "idregion";
            ddlRegion.DataTextField = "region";
            ddlRegion.DataBind();
            ListItem blankRegion = new ListItem("Cualquier Region", "0");
            ddlRegion.Items.Insert(0, blankRegion);
            ddlRegion.SelectedValue = "0";
        }

        protected void rellenaArea(BDManage VectArea)
        {
            ddlArea.DataSource = VectArea.getDataSet();
            ddlArea.DataValueField = "idarea";
            ddlArea.DataTextField = "area";
            ddlArea.DataBind();
            ListItem blankArea = new ListItem("Cualquier Area", "0");
            ddlArea.Items.Insert(0, blankArea);
            ddlArea.SelectedValue = "0";
        }

        protected void rellenaDistrito(BDManage VectDistrito)
        {
            ddlDistrito.DataSource = VectDistrito.getDataSet();
            ddlDistrito.DataValueField = "iddistrito";
            ddlDistrito.DataTextField = "distrito";
            ddlDistrito.DataBind();
            ListItem blankDistrito = new ListItem("Cualquier Distrito", "0");
            ddlDistrito.Items.Insert(0, blankDistrito);
            ddlDistrito.SelectedValue = "0";
        }

        public void CombosUO(bool habilita)
        {
            if (habilita)
            {
                ddlArea.Enabled = false;
                ddlUnidad.Enabled = false;
                ddlRegion.Enabled = false;
                ddlDistrito.Enabled = false;
            }
            else
            {
                ddlArea.Enabled = false;
                ddlUnidad.Enabled = false;
                ddlRegion.Enabled = false;
                ddlDistrito.Enabled = false;
            }

        }

        #endregion

        #region "Enviar Mail"

        public void EnviaMailPendienteAprobar(List<string> collectCorreo)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            AgenteAmecInfo agAMECinfo = new AgenteAmecInfo();
            int GuardarMail;
            try
            {
                AgenteUsuarios agUsu = new AgenteUsuarios();
                AgenteMaestros agMaes = new AgenteMaestros();

                DDatosPersonalesUsuario dSolicitante = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());
                string link = string.Format("{0}NuevoDetalleAMEC.aspx?idamec={1}", RutaLinkMail, miAmec.idamecs);
                DEmpresaConf datosEmpresa = agMaes.ObtenerEmpresaConf(1);
                DDatosPersonalesUsuario Creador = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idcreadopor.ToString());
                DDatosPersonalesUsuario Solicitante = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());
                List<string> lstMailCCHcp = Quodem.Utility.StringUtil.GetValoresList(ConfigUtil.GetAppSetting(Constantes.AppParams.MailCCHcp));
                string estado = agAMECinfo.ObtenerNombreEstado(miAmec.idestado);
                AgenteUnidadesOrgAMEC agUnid = new AgenteUnidadesOrgAMEC();
                string unidad = agUnid.ObtenerNombreUnidadxId(Convert.ToInt32(dSolicitante.idunidad));
                string area = agUnid.ObtenerNombreAreaxId(Convert.ToInt32(dSolicitante.Idarea));

                if (collectCorreo.Count != 0)
                {
                    foreach (string correo in collectCorreo)
                    {
                        datosUsuarioCorreo = agUsu.ObtenerDatosPersonalesPorIDPeticionario(correo);
                        try
                        {
                            //string sMailResponsable = MailInmediatoSuperior();
                            if (!string.IsNullOrEmpty(datosUsuarioCorreo.Email))
                            {
                                message.To.Add(new MailAddress(datosUsuarioCorreo.Email));
                            }
                        }
                        catch
                        {

                            Alert.Show("Se ha producido un error enviando el mail de Notificación Pendiente de Aprobar. Por favor revise la dirección de envío en la configuración del sistema.", null);
                            GuardarMail = agAMECinfo.GuardaLogMail(miAmec.idamecs, "MailAmecPendienteAprobar", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico");
                            return;
                        }

                    }
                    try
                    {
                        message.To.Add(new MailAddress(Creador.Email));
                        if (MailCopia.Contains('@'))
                        {
                            foreach (string s in MailCopia.Split(';'))
                            {
                                message.To.Add(new MailAddress(s));
                            }
                        }
                           
                        if (MailCopiaProgramador.Contains('@'))
                        {
                            foreach (string s in MailCopiaProgramador.Split(';'))
                            {
                                message.To.Add(new MailAddress(s));
                            }
                        }

                        if (lstMailCCHcp.Contains(Solicitante.Email))
                        {
                            message.CC.Add(new MailAddress(Solicitante.Email));
                        }
                    }
                    catch
                    {

                        Alert.Show("Se ha producido un error enviando el mail de Notificación Pendiente de Aprobar. Por favor revise la dirección de envío en la configuración del sistema.", null);
                        GuardarMail = agAMECinfo.GuardaLogMail(miAmec.idamecs, "MailAmecPendienteAprobar", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico");
                        return;
                    }

                }
                string importe = string.Format("{0:C}", miAmec.importegasto);
                message.Subject = string.Format("Amec {0} pendiente de Aprobar: Importe {1}, Número de Ponentes {2} y Número de Personas {3} ", miAmec.idamecs, importe, miAmec.ponentespatrocinados, miAmec.participantesmsd);
                if (miAmec.paraguas == true)
                {
                    string listUnidadesOrg = ObtenerUnidadesOrganizativasParaCorreo();
                    message.Body = string.Format("El Amec número {0} para la actividad {1} está en su nivel de aprobación ({2}). \r\n Unidades Organizativas del Amec Paraguas:\r\n{4} \r\n\r\nHaz click en el link adjunto para ver más detalles: {3}", miAmec.idamecs, miAmec.descripcion, estado, link, listUnidadesOrg);
                }
                else
                {

                    message.Body = string.Format("El Amec número {0} para la actividad {1} con unidad {4} y area {5} está en su nivel de aprobación ({2}). \r\n\r\nHaz click en el link adjunto para ver más detalles: {3}", miAmec.idamecs, miAmec.descripcion, estado, link, unidad, area);
                }

                Mail.EnviaMail(message);
                GuardarMail = agAMECinfo.GuardaLogMail(miAmec.idamecs, "MailAmecPendienteAprobar", message.To.ToString(), message.Subject, message.Body, null, true, null);

            }
            catch (Exception ex)
            {
                Alert.Show("Se ha producido un error enviando el mail de Notificación de Aprobación. Por favor revise la dirección de envío en la configuración del sistema.", null);
                GuardarMail = agAMECinfo.GuardaLogMail(miAmec.idamecs, "MailAmecPendienteAprobar", message.To.ToString(), message.Subject, message.Body, null, false, "Error enviando el Correo");
                Logger.Logger.PrintError(this.GetType().Name, "amecNotificaciones", ex.Message, ex);
                try
                {
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("contactoError"), ex);
                }
                catch { }
            }
        }

        public void EnviaMailCancelado()
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            int GuardarMail;
            try
            {
                AgenteUsuarios agUsu = new AgenteUsuarios();

                string link = string.Format("{0}NuevoDetalleAMEC.aspx?idamec={1}", RutaLinkMail, miAmec.idamecs);
                DDatosPersonalesUsuario dSolicitante = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());
                DDatosPersonalesUsuario dCreador = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idcreadopor.ToString());
                AgenteUnidadesOrgAMEC agUnid = new AgenteUnidadesOrgAMEC();
                string unidad = agUnid.ObtenerNombreUnidadxId(Convert.ToInt32(dSolicitante.idunidad));
                string area = agUnid.ObtenerNombreAreaxId(Convert.ToInt32(dSolicitante.Idarea));

                try
                {
                    //Buscamos todos los usuarios que han intervenido en el flujo de aprobación para informarles que se ha aprobado el amec. Los usuarios Dep Médico no se les envia confirmación
                    DataSet usuariosIntervenidoFlujoAprobacion = agAmecInf.MailsAEnviarCuandoAprobado(miAmec.idamecs);
                    int i;
                    for (i = 0; i < usuariosIntervenidoFlujoAprobacion.Tables[0].Rows.Count; i++)
                    {
                        message.To.Add(new MailAddress(usuariosIntervenidoFlujoAprobacion.Tables[0].Rows[i].ItemArray[0].ToString())); //PONEMOS LA DIRECCION DE ENVIO AL MAIL.
                    }
                    message.To.Add(new MailAddress(dSolicitante.Email));
                    //if (miAmec.idsolicitante != miAmec.idcreadopor) message.To.Add(new MailAddress(dCreador.Email));
                    message.To.Add(new MailAddress(mailGestorArchivos));
                    if (MailCopia.Contains('@'))
                    {
                        foreach (string s in MailCopia.Split(';'))
                        {
                            message.To.Add(new MailAddress(s));
                        }
                    }
                        
                    if (MailCopiaProgramador.Contains('@'))
                    {
                        foreach (string s in MailCopiaProgramador.Split(';'))
                        {
                            message.To.Add(new MailAddress(s));
                        }
                    }

                }
                catch
                {
                    Alert.Show("Se ha producido un error enviando el mail de Cancelado del AMEC. Por favor revise la dirección de correo del Solicitante.", null);
                    GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailAmecCancelado", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico");

                    return;
                }
                string importe = string.Format("{0:C}", miAmec.importegasto);
                message.Subject = string.Format("AMEC {0} CANCELADO, Importe {1}, Número de Ponentes {2} y Número de Personas {3}", miAmec.idamecs, importe, miAmec.ponentespatrocinados, miAmec.participantesmsd);
                if (miAmec.paraguas == true)
                {
                    string listUnidadesOrg = ObtenerUnidadesOrganizativasParaCorreo();
                    message.Body = string.Format("Ha sido CANCELADO el Amec número {0} para la actividad {1} con las siguientes Unidades Organizativas:\r\n{2} \r\n\r\nHaz click en el link adjunto para ver más detalles: {3}", miAmec.idamecs, miAmec.descripcion, listUnidadesOrg, link);
                }
                else
                {
                    message.Body = string.Format("El Amec número {0} para la actividad {1} de la unidad {2} area {3}, ha sido CANCELADO. \r\n\r\nHaz click en el link adjunto para ver más detalles: {4}", miAmec.idamecs, miAmec.descripcion, unidad, area, link);
                }
                Mail.EnviaMail(message);
                GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailAmecCancelado", message.To.ToString(), message.Subject, message.Body, null, true, null);


            }
            catch (Exception ex)
            {
                Alert.Show("Se ha producido un error enviando el mail de Cancelado del AMEC", null);
                GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailAmecCancelado", message.To.ToString(), message.Subject, message.Body, null, false, "Error enviando el Correo");
                Logger.Logger.PrintError(this.GetType().Name, "amecNotificaciones", ex.Message, ex);
                try
                {
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("contactoError"), ex);
                }
                catch { }
            }
        }

        public void EnviaMailRechazado()
        {
            AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            int GuardarMail;
            try
            {
                AgenteUsuarios agUsu = new AgenteUsuarios();

                string estado;
                string link = string.Format("{0}NuevoDetalleAMEC.aspx?idamec={1}", RutaLinkMail, miAmec.idamecs);
                DDatosPersonalesUsuario dSolicitante = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());
                DDatosPersonalesUsuario dCreador = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idcreadopor.ToString());
                AgenteUnidadesOrgAMEC agUnid = new AgenteUnidadesOrgAMEC();
                string unidad = agUnid.ObtenerNombreUnidadxId(Convert.ToInt32(dSolicitante.idunidad));
                string area = agUnid.ObtenerNombreAreaxId(Convert.ToInt32(dSolicitante.Idarea));

                //message.From = new MailAddress("sender@foo.bar.com");
                try
                {
                    AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
                    //Buscamos todos los usuarios que han intervenido en el flujo de aprobación para informarles que se ha aprobado el amec. Los usuarios Dep Médico no se les envia confirmación
                    DataSet usuariosIntervenidoFlujoAprobacion = agAmecInf.MailsAEnviarCuandoAprobado(miAmec.idamecs);
                    int i;
                    for (i = 0; i < usuariosIntervenidoFlujoAprobacion.Tables[0].Rows.Count; i++)
                    {
                        message.To.Add(new MailAddress(usuariosIntervenidoFlujoAprobacion.Tables[0].Rows[i].ItemArray[0].ToString())); //PONEMOS LA DIRECCION DE ENVIO AL MAIL.
                    }
                    message.To.Add(new MailAddress(dSolicitante.Email));
                    message.To.Add(new MailAddress(mailGestorArchivos));

                    if (MailCopia.Contains('@'))
                    {
                        foreach (string s in MailCopia.Split(';'))
                        {
                            message.To.Add(new MailAddress(s));
                        }
                    }
                        
                    if (MailCopiaProgramador.Contains('@'))
                    {
                        foreach (string s in MailCopiaProgramador.Split(';'))
                        {
                            message.To.Add(new MailAddress(s));
                        }
                    }

                    if (miAmec.idcreadopor != miAmec.idsolicitante) message.To.Add(new MailAddress(dCreador.Email));
                    estado = agAmecInfo.ObtenerNombreEstado(miAmec.idestado);
                    DataSet BudUnidadAmec = agAmecInf.BUDdelaUnidad(miAmec.idamecs);
                    message.To.Add(new MailAddress(BudUnidadAmec.Tables[0].Rows[0].ItemArray[0].ToString())); //PONEMOS LA DIRECCION DE ENVIO AL MAIL.                   
                }
                catch
                {
                    Alert.Show("Se ha producido un error enviando el mail de Rechazo del AMEC. Por favor revise la dirección de correo del Solicitante.", null);
                    GuardarMail = agAmecInfo.GuardaLogMail(miAmec.idamecs, "MailAmecRechazado", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico");
                    return;
                }
                string importe = string.Format("{0:C}", miAmec.importegasto);
                message.Subject = string.Format("AMEC {0} DENEGADO, Importe {1}, Número de Ponentes {2} y Número de Personas {3}", miAmec.idamecs, importe, miAmec.ponentespatrocinados, miAmec.participantesmsd);
                if (miAmec.paraguas == true)
                {
                    string listUnidadesOrg = ObtenerUnidadesOrganizativasParaCorreo();
                    message.Body = string.Format("Ha sido DENEGADO el Amec número {0} para la actividad {1} con las siguientes Unidades Organizativas:\r\n{2} \r\n\r\nHaz click en el link adjunto para ver más detalles: {3}", miAmec.idamecs, miAmec.descripcion, listUnidadesOrg, link);
                }
                else
                {
                    message.Body = string.Format("El Amec número {0} para la actividad {1} de la unidad {2} area {3}, ha sido DENEGADO. \r\n\r\nHaz click en el link adjunto para ver más detalles: {4}", miAmec.idamecs, miAmec.descripcion, unidad, area, link);
                }
                Mail.EnviaMail(message);
                GuardarMail = agAmecInfo.GuardaLogMail(miAmec.idamecs, "MailAmecRechazado", message.To.ToString(), message.Subject, message.Body, null, true, null);

            }
            catch (Exception ex)
            {
                Alert.Show("Se ha producido un error enviando el mail de Rechazo del AMEC", null);
                GuardarMail = agAmecInfo.GuardaLogMail(miAmec.idamecs, "MailAmecRechazado", message.To.ToString(), message.Subject, message.Body, null, false, "Error enviando el Correo");
                Logger.Logger.PrintError(this.GetType().Name, "amecNotificaciones", ex.Message, ex);
                try
                {
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("contactoError"), ex);
                }
                catch { }
            }
        }

        public void EnviaMailAprobacionCondicionada()
        {

            AgenteAmecInfo agAMECinfo = new AgenteAmecInfo();
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            int GuardarMail;
            RepositorioFlujo estadoAMEC = null;
            try
            {
                AgenteUsuarios agUsu = new AgenteUsuarios();
                string link = string.Format("{0}NuevoDetalleAMEC.aspx?idamec={1}", RutaLinkMail, miAmec.idamecs);
                DDatosPersonalesUsuario dSolicitante = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());
                DDatosPersonalesUsuario dCreado = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idcreadopor.ToString());
                AgenteUnidadesOrgAMEC agUnid = new AgenteUnidadesOrgAMEC();

                string estado = agAMECinfo.ObtenerNombreEstado(miAmec.idestado);
                string unidad = agUnid.ObtenerNombreUnidadxId(Convert.ToInt32(dSolicitante.idunidad));
                string area = agUnid.ObtenerNombreAreaxId(Convert.ToInt32(dSolicitante.Idarea));

                //message.From = new MailAddress("sender@foo.bar.com");
                try
                {
                    AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
                    if (miAmec.preaprobadaneg == true)
                    {
                        estadoAMEC = new RepositorioFlujo(miAmec.idamecs, (Int32)datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
                        List<string> collectUsuariosIntervenidoFlujoAprobacion = estadoAMEC.ObtenerCorreoNegocioParaEnviar(miAmec.idamecs, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
                        //DataSet collectUsuariosIntervenidoFlujoAprobacion = agAmecInf.ObtenerCorreoNegocioParaEnviar(miAmec.idamecs);
                        foreach (string correo in collectUsuariosIntervenidoFlujoAprobacion)
                        {
                            datosUsuarioCorreo = agUsu.ObtenerDatosPersonalesPorIDPeticionario(correo);
                            try
                            {
                                //string sMailResponsable = MailInmediatoSuperior();
                                if (!string.IsNullOrEmpty(datosUsuarioCorreo.Email))
                                {
                                    message.To.Add(new MailAddress(datosUsuarioCorreo.Email));
                                }
                            }
                            catch
                            {
                                Alert.Show("Se ha producido un error enviando el mail de Notificación de Aprobación. Por favor revise la dirección de envío en la configuración del sistema.", null);
                                GuardarMail = agAMECinfo.GuardaLogMail(miAmec.idamecs, "MailAmecAprobacionCondicionada", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico");
                                return;
                            }

                        }
                    }

                    if (miAmec.idsolicitante != miAmec.idcreadopor)
                    {
                        message.To.Add(new MailAddress(dCreado.Email));
                    }

                    message.To.Add(new MailAddress(mailGestorArchivos));
                    message.To.Add(new MailAddress(dSolicitante.Email));

                    if (MailCopia.Contains('@'))
                    {
                        foreach (string s in MailCopia.Split(';'))
                        {
                            message.To.Add(new MailAddress(s));
                        }
                    }
                        
                    if (MailCopiaProgramador.Contains('@'))
                    {
                        foreach (string s in MailCopiaProgramador.Split(';'))
                        {
                            message.To.Add(new MailAddress(s));
                        }
                    }

                }
                catch
                {
                    Alert.Show("Se ha producido un error enviando el mail de Notificación de Aprobado Condicionado del AMEC. Por favor revise la dirección de correo del Solicitante.", null);
                    GuardarMail = agAMECinfo.GuardaLogMail(miAmec.idamecs, "MailAmecAprobacionCondicionada", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico");
                    return;
                }
                string importe = string.Format("{0:C}", miAmec.importegasto);
                message.Subject = string.Format("AMEC {0} Aprobado con Condición: Importe {1}, Número de Ponentes {2} y Número de Personas {3}", miAmec.idamecs, importe, miAmec.ponentespatrocinados, miAmec.participantesmsd);
                if (miAmec.paraguas == true)
                {
                    string listUnidadesOrg = ObtenerUnidadesOrganizativasParaCorreo();
                    message.Body = string.Format("Ha sido Aprobado con Condición el Amec número {0} para la actividad {1}, nivel de aprobación ({4}) y con las siguientes Unidades Organizativas:\r\n{2}\r\nPor favor, revisa las condiciones antes de implementar la actividad. \r\n\r\nHaz click en el link adjunto para ver más detalles: {3}", miAmec.idamecs, miAmec.descripcion, listUnidadesOrg, link, estado);
                }
                else
                {
                    message.Body = string.Format("El Amec número {0} para la actividad {1} de la unidad {2} area {3} ha sido Aprobado con Condición y nivel de aprobación ({4}). Por favor, revisa las condiciones antes de implementar la actividad.\r\n\r\nHaz click en el link adjunto para ver más detalles: {5}", miAmec.idamecs, miAmec.descripcion, unidad, area, estado, link);
                }
                Mail.EnviaMail(message);
                GuardarMail = agAMECinfo.GuardaLogMail(miAmec.idamecs, "MailAmecAprobacionCondicionada", message.To.ToString(), message.Subject, message.Body, null, true, null);

            }
            catch (Exception ex)
            {
                Alert.Show("Se ha producido un error enviando el mail de Notificación de Aprobado Condicionado", null);
                GuardarMail = agAMECinfo.GuardaLogMail(miAmec.idamecs, "MailAmecAprobacionCondicionada", message.To.ToString(), message.Subject, message.Body, null, false, "Error enviando el Correo");
                Logger.Logger.PrintError(this.GetType().Name, "amecNotificaciones", ex.Message, ex);
                try
                {
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("contactoError"), ex);
                }
                catch { }
            }
        }

        public void EnviaMailAprobacionFinal()
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            int GuardarMail;
            RepositorioFlujo estadoAMEC = null;
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            try
            {
                //Haura de rebre-ho el solicitant i el Creador                
                AgenteUsuarios agUsu = new AgenteUsuarios();
                //string estado;
                string link = string.Format("{0}NuevoDetalleAMEC.aspx?idamec={1}", RutaLinkMail, miAmec.idamecs);
                DDatosPersonalesUsuario dSolicitante = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());
                AgenteUnidadesOrgAMEC agUnid = new AgenteUnidadesOrgAMEC();
                string unidad = agUnid.ObtenerNombreUnidadxId(Convert.ToInt32(dSolicitante.idunidad));
                string area = agUnid.ObtenerNombreAreaxId(Convert.ToInt32(dSolicitante.Idarea));


                //message.From = new MailAddress("sender@foo.bar.com");
                try
                {
                    //Buscamos todos los usuarios que han intervenido en el flujo de aprobación para informarles que se ha aprobado el amec. Los usuarios Dep Médico no se les envia confirmación
                    if (miAmec.preaprobadaneg == true)
                    {
                        estadoAMEC = new RepositorioFlujo(miAmec.idamecs, (Int32)datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
                        List<string> collectUsuariosIntervenidoFlujoAprobacion = estadoAMEC.ObtenerCorreoNegocioParaEnviar(miAmec.idamecs, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
                        //DataSet collectUsuariosIntervenidoFlujoAprobacion = agAmecInf.ObtenerCorreoNegocioParaEnviar(miAmec.idamecs);
                        foreach (string correo in collectUsuariosIntervenidoFlujoAprobacion)
                        {
                            datosUsuarioCorreo = agUsu.ObtenerDatosPersonalesPorIDPeticionario(correo);
                            try
                            {
                                //string sMailResponsable = MailInmediatoSuperior();
                                if (!string.IsNullOrEmpty(datosUsuarioCorreo.Email))
                                {
                                    message.To.Add(new MailAddress(datosUsuarioCorreo.Email));
                                }
                            }
                            catch
                            {
                                Alert.Show("Se ha producido un error enviando el mail de Notificación de Aprobación. Por favor revise la dirección de envío en la configuración del sistema.", null);
                                GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailAmecAprobacionFinal", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico de los Usuarios que intervienen en el Flujo");

                                return;
                            }

                        }
                    }

                    if (miAmec.idsolicitante != miAmec.idcreadopor)
                    {
                        DDatosPersonalesUsuario dCreadoPor = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idcreadopor.ToString());
                        message.To.Add(new MailAddress(dCreadoPor.Email));
                    }
                    message.To.Add(new MailAddress(dSolicitante.Email));
                    message.To.Add(new MailAddress(mailGestorArchivos));

                    if (MailCopia.Contains('@'))
                    {
                        foreach (string s in MailCopia.Split(';'))
                        {
                            message.To.Add(new MailAddress(s));
                        }
                    }
                        
                    if (MailCopiaProgramador.Contains('@'))
                    {
                        foreach (string s in MailCopiaProgramador.Split(';'))
                        {
                            message.To.Add(new MailAddress(s));
                        }
                    }

                    //estado = agAmecInf.ObtenerNombreEstado(miAmec.idestado);
                    DataSet BudUnidadAmec = agAmecInf.BUDdelaUnidad(miAmec.idamecs);
                    if (BudUnidadAmec != null && BudUnidadAmec.Tables[0] != null &&
                        BudUnidadAmec.Tables[0].Rows[0] != null)
                    {
                        message.To.Add(new MailAddress(BudUnidadAmec.Tables[0].Rows[0].ItemArray[0].ToString()));
                        //PONEMOS LA DIRECCION DE ENVIO AL MAIL.                   
                    }
                }
                catch (Exception ex)
                {
                    Alert.Show("Se ha producido un error enviando el mail de Notificación de APROBADO del AMEC. Por favor revise la dirección de correo del Solicitante.", null);
                    GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailAmecAprobacionFinal", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico - " + ex.InnerException.ToString());

                    return;
                }
                string importe = string.Format("{0:C}", miAmec.importegasto);
                message.Subject = string.Format("Amec {0} APROBADO: Importe {1}, Número de Ponentes {2} y Número de Personas {3}", miAmec.idamecs, importe, miAmec.ponentespatrocinados, miAmec.participantesmsd);
                if (miAmec.paraguas == true)
                {
                    string listUnidadesOrg = "";
                    //Antes
                    //listUnidadesOrg = ObtenerUnidadesOrganizativasParaCorreo();
                    //message.Body = string.Format("El Amec número {0} para la actividad {1} con las siguientes Unidades Organizativas:\r\n{2} \r\n\r\nHaz click en el link adjunto para ver más detalles: {3}", miAmec.idamecs, miAmec.descripcion, listUnidadesOrg, link);

                    //Cambio por un tiempo Determinado para informar al usuario que el MD no está en el flujo
                    listUnidadesOrg = ObtenerUnidadesOrganizativasParaCorreoHtml();
                    message.Body = string.Format("El Amec número {0} para la actividad {1} con las siguientes Unidades Organizativas:<br>{2} <br><br>Haz click en el link adjunto para ver más detalles: {3}", miAmec.idamecs, miAmec.descripcion, listUnidadesOrg, link);
                    ////////////////////////////////////

                }
                else
                {
                    //message.Body = string.Format("El Amec número {0} para la actividad {1} de la unidad {2} area {3} ha sido APROBADO. \r\n\r\nHaz click en el link adjunto para ver más detalles: {4}", miAmec.idamecs, miAmec.descripcion, unidad, area, link);

                    //Cambio por un tiempo Determinado para informar al usuario que el MD no está en el flujo
                    message.Body = string.Format("El Amec número {0} para la actividad {1} de la unidad {2} area {3} ha sido APROBADO. <br><br>Haz click en el link adjunto para ver más detalles: {4}", miAmec.idamecs, miAmec.descripcion, unidad, area, link);
                    ////////////////////////////////////
                }

                //Cambio por un tiempo Determinado para informar al usuario que el MD no está en el flujo
                string text1 = "<b>Se ha modificado el proceso de aprobación. El director general aprueba las actividades vía ICBP (fuera de la herramienta) y no actividad a actividad vía AMEC online</b>";
                message.Body = string.Format("{0}<br><br><br> * {1} *", message.Body, text1);
                message.IsBodyHtml = true;
                //////////////////////////////////

                Mail.EnviaMail(message);
                GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailAmecAprobacionFinal", message.To.ToString(), message.Subject, message.Body, null, true, null);
            }
            catch (Exception ex)
            {
                Alert.Show("Se ha producido un error enviando el mail de Notificación de APROBADO del AMEC", null);
                GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailAmecAprobacionFinal", message.To.ToString(), message.Subject, message.Body, null, false, "Error enviando el Correo");
                Logger.Logger.PrintError(this.GetType().Name, "amecNotificaciones", ex.Message, ex);
                try
                {
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("contactoError"), ex);
                }
                catch { }
            }
        }

        public void EnviaMailPublicarComentario()
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            int GuardarMail;
            RepositorioFlujo estadoAMEC = null;
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            try
            {
                AgenteUsuarios agUsu = new AgenteUsuarios();
                //string estado;
                string link = string.Format("{0}NuevoDetalleAMEC.aspx?idamec={1}", RutaLinkMail, miAmec.idamecs);
                DDatosPersonalesUsuario dSolicitante = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());
                AgenteUnidadesOrgAMEC agUnid = new AgenteUnidadesOrgAMEC();
                string unidad = agUnid.ObtenerNombreUnidadxId(Convert.ToInt32(dSolicitante.idunidad));
                string area = agUnid.ObtenerNombreAreaxId(Convert.ToInt32(dSolicitante.Idarea));

                estadoAMEC = new RepositorioFlujo(miAmec.idamecs, (Int32)datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
                List<string> collectCorreo = estadoAMEC.ObtenerCorreoParaEnviar(miAmec.idamecs, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
                if (collectCorreo.Count != 0)
                {
                    foreach (string correo in collectCorreo)
                    {
                        datosUsuarioCorreo = agUsu.ObtenerDatosPersonalesPorIDPeticionario(correo);
                        try
                        {
                            //string sMailResponsable = MailInmediatoSuperior();
                            if (!string.IsNullOrEmpty(datosUsuarioCorreo.Email))
                            {
                                message.To.Add(new MailAddress(datosUsuarioCorreo.Email));
                            }
                        }
                        catch
                        {

                            Alert.Show("Se ha producido un error enviando el mail de Publicacion de Comentario. Por favor revise la dirección de envío en la configuración del sistema.", null);
                            GuardarMail = agAMECinfo.GuardaLogMail(miAmec.idamecs, "EnviaMailPublicarComentario", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico");
                            return;
                        }

                    }
                }
                //message.From = new MailAddress("sender@foo.bar.com");
                try
                {
                    //Si el usuario es el solicitante el correo irá destinado al Usuario que tiene que aprobar
                    //Si el usuario es el que tiene que aprobar el correo irá destinado al solicitante y al Creador

                    //Cuando el Peticionario que hace el Comentario es el Solicitante
                    if (miAmec.idsolicitante != miAmec.idcreadopor)
                    {
                        DDatosPersonalesUsuario dCreado = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idcreadopor.ToString());
                        message.To.Add(new MailAddress(dCreado.Email));
                    }
                    message.To.Add(new MailAddress(dSolicitante.Email));


                    if (MailCopia.Contains('@'))
                    {
                        foreach (string s in MailCopia.Split(';'))
                        {
                            message.To.Add(new MailAddress(s));
                        }
                    }
                        
                    if (MailCopiaProgramador.Contains('@'))
                    {
                        foreach (string s in MailCopiaProgramador.Split(';'))
                        {
                            message.To.Add(new MailAddress(s));
                        }
                    }

                    //estado = agAmecInf.ObtenerNombreEstado(miAmec.idestado);
                    //DataSet BudUnidadAmec = agAmecInf.BUDdelaUnidad(miAmec.idamecs);
                    //message.To.Add(new MailAddress(BudUnidadAmec.Tables[0].Rows[0].ItemArray[0].ToString())); //PONEMOS LA DIRECCION DE ENVIO AL MAIL.                   

                }
                catch
                {
                    Alert.Show("Se ha producido un error enviando el mail de Publicación de Comentario del AMEC. Por favor revise la dirección de correo.", null);
                    GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailPublicarComentario", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico");

                    return;
                }
                string importe = string.Format("{0:C}", miAmec.importegasto);
                message.Subject = string.Format("Cuadro de dialogo en AMEC {0} con Importe {1}, Número de Personas {3} y Número de ponentes {2}", miAmec.idamecs, importe, miAmec.ponentespatrocinados, miAmec.participantesmsd);
                message.Body = string.Format("Tienes un comentario en el cuadro de diálogo para la actividad {1} del número de AMEC {0}. \r\n\r\nHaz click en el link adjunto para ver más detalles: {2}", miAmec.idamecs, miAmec.descripcion, link);

                Mail.EnviaMail(message);
                GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailPublicarComentario", message.To.ToString(), message.Subject, message.Body, null, true, null);
            }
            catch (Exception ex)
            {
                Alert.Show("Se ha producido un error enviando el mail Publicación de Comentario del AMEC", null);
                GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailPublicarComentario", message.To.ToString(), message.Subject, message.Body, null, false, "Error enviando el Correo");
                Logger.Logger.PrintError(this.GetType().Name, "amecNotificaciones", ex.Message, ex);
                try
                {
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("contactoError"), ex);
                }
                catch { }
            }
        }

        public void EnviaMailCasosClinicos()
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            int GuardarMail;
            try
            {
                AgenteUsuarios agUsu = new AgenteUsuarios();
                DDatosPersonalesUsuario dSolicitante = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());
                AgenteUnidadesOrgAMEC agUnid = new AgenteUnidadesOrgAMEC();
                string unidad = agUnid.ObtenerNombreUnidadxId(Convert.ToInt32(dSolicitante.idunidad));
                string area = agUnid.ObtenerNombreAreaxId(Convert.ToInt32(dSolicitante.Idarea));
                string link = string.Format("{0}NuevoDetalleAMEC.aspx?idamec={1}", RutaLinkMail, miAmec.idamecs);

                try
                {
                    message.To.Add(new MailAddress(mailCasosClinicos)); //PONEMOS LA DIRECCION DE ENVIO AL MAIL.

                    if (MailCopia.Contains('@'))
                    {
                        foreach (string s in MailCopia.Split(';'))
                        {
                            message.To.Add(new MailAddress(s));
                        }
                    }
                        
                    if (MailCopiaProgramador.Contains('@'))
                    {
                        foreach (string s in MailCopiaProgramador.Split(';'))
                        {
                            message.To.Add(new MailAddress(s));
                        }
                    }

                }
                catch
                {
                    Alert.Show("Se ha producido un error enviando el mail Casos Clinicos. Por favor revise la dirección From en la configuración del sistema.", null);
                    GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailCasosClinicos", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico");
                    return;
                }

                message.Subject = string.Format("AMEC {0} con gestión de casos clínicos", miAmec.idamecs); //ASUNTO DEL MAIL [me lo he inventado]
                message.Body = string.Format("\r\n Les informamos que la actividad AMEC {0} conlleva gestión de casos clíncos.\r\n\r\n Nombre del la actividad: {1}, Tipo de actividad: {2} \r\n Solicitada por {3} de la Unidad {4} y Area {5}, que se celebrará {6}.\r\n\r\nHaz click en el link adjunto para ver más detalles: {7}", miAmec.idamecs, miAmec.descripcion, ddlActividad.SelectedItem.Text, ddlSolicitante.SelectedItem.Text, unidad, area, miAmec.fechacomienzo, link);
                Mail.EnviaMail(message); //ENVIAMOS MAIL
                GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailCasosClinicos", message.To.ToString(), message.Subject, message.Body, null, true, null);

            }
            catch (Exception ex)
            {
                Alert.Show("Se ha producido un error enviando el mail Casos Clínicos. Por favor revise la configuración del sistema", null);
                GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailCasosClinicos", message.To.ToString(), message.Subject, message.Body, null, false, "Error enviando el Correo");
                Logger.Logger.PrintError(this.GetType().Name, "amecNotificaciones", ex.Message, ex);
                try
                {
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("contactoError"), ex);
                }
                catch { }
            }
        }

        public void EnviaMailFarmaIndustria()
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.BodyEncoding = Encoding.UTF8;
            AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
            int GuardarMail;
            string Attachments = "";
            try
            {
                string saveFile = "";
                String sFile = "";
                string link = string.Format("{0}NuevoDetalleAMEC.aspx?idamec={1}", RutaLinkMail, miAmec.idamecs);
                AgenteUsuarios agenteUsua = new AgenteUsuarios();
                int i;

                DDatosPersonalesUsuario Solicitante = agenteUsua.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());
                //DataSet usuariosCompliance = agenteUsua.ObtenerUsuariosPorCargo(555);
                DataSet usuariosComplianceAprobado = agenteUsua.ObtenerUsuariosComplianceAprobador();
                for (i = 0; i < usuariosComplianceAprobado.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        message.To.Add(new MailAddress(usuariosComplianceAprobado.Tables[0].Rows[i].ItemArray[16].ToString())); //PONEMOS LA DIRECCION DE ENVIO AL MAIL.
                    }
                    catch
                    {
                        Alert.Show("Se ha producido un error enviando el mail a Farmaindustria. Por favor revise la dirección From en la configuración del sistema.", null);
                        GuardarMail = agAmecInfo.GuardaLogMail(miAmec.idamecs, "MailFarmaIndustria", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico");
                        return;

                    }
                }
                message.To.Add(new MailAddress(Solicitante.Email));

                if (MailCopia.Contains('@'))
                {
                    foreach (string s in MailCopia.Split(';'))
                    {
                        message.To.Add(new MailAddress(s));
                    }
                }
                    
                if (MailCopiaProgramador.Contains('@'))
                {
                    foreach (string s in MailCopiaProgramador.Split(';'))
                    {
                        message.To.Add(new MailAddress(s));
                    }
                }

                //Comunicaciones de FarmaIndustria a Agencias -> Solo si tiene agencia asignada -> ¿Si no?
                AgenteMaestros agente = new AgenteMaestros();
                var confEmpresas = agente.ObtenerEmpresasConf();
                string mailAgenciaFi = string.Empty;
                if (miAmec.idconfempresa > 0)
                {
                    var firstOrDefault = confEmpresas.FirstOrDefault(x => x.idconfempresa == miAmec.idconfempresa);
                    if (firstOrDefault != null)
                    {
                        mailAgenciaFi = firstOrDefault.mailtocomunicfi;
                    }
                }

                if (!string.IsNullOrWhiteSpace(mailAgenciaFi))
                {
                    message.To.Add(new MailAddress(mailAgenciaFi));
                }
                //Fin comunicaciones de FarmaIndustria a Agencias

                message.Subject = string.Format("Comunicación a Farmaindustria de  la actividad {0}, Número de AMEC {1} y Solicitante {2}", miAmec.descripcion, miAmec.idamecs, datosUsuarioSolicitante.NombreCompleto); //ASUNTO DEL MAIL [me lo he inventado]
                message.Body = "Adjunto documentación para su comunicación a Farmaindustria";
                //message.Body = string.Format("Podrá encontrar la documentación del Amec en el siguiente link: {0}",link);


                if (!string.IsNullOrEmpty(miAmec.programaamecs.ToString()))
                {
                    string Programa = @"Programa\";
                    savePath = ConfigUtil.GetAppSetting("AMECUpload");
                    sFile = string.Format("{0}{1}\\{2}{3}", savePath, miAmec.idamecs.ToString(), Programa, miAmec.programaamecs.Trim());
                    message.Body = string.Format("{0}\r\n\r\nAdjuntado Programa {1}\r\n", message.Body, miAmec.programaamecs.Trim());
                    saveFile = sFile.Replace("\\", "/");
                    Attachment data = new Attachment(saveFile);
                    data.NameEncoding = System.Text.Encoding.UTF8;
                    message.Attachments.Add(data);
                }
                else
                {
                    if (!string.IsNullOrEmpty(miAmec.descripcionobjetivo.ToString()))
                    {
                        message.Body = string.Format("{0}\r\n\r\nDescripción del Programa: {1}\r\n", message.Body, miAmec.descripcionobjetivo.ToString());
                    }

                    if (!string.IsNullOrEmpty(miAmec.urlprograma.ToString()))
                    {
                        message.Body = string.Format("{0}\r\n\r\nUrl del Programa: {1}\r\n", message.Body, miAmec.urlprograma.ToString());
                    }
                }

                ICollection<DDocumentacionAmec> collectDocument = agAmecInfo.ObtenerDocumentacionAMEC(miAmec.idamecs.ToString(), "", 0, 0);
                foreach (DDocumentacionAmec DocAmec in collectDocument)
                {
                    if (DocAmec.adjuntaraemail == true)
                    {
                        string Documentacion = @"Documentacion\";
                        string saf = savePathDocumentacion;
                        savePath = ConfigUtil.GetAppSetting("AMECUpload");
                        sFile = string.Format("{0}{1}\\{2}{3}", savePath, miAmec.idamecs.ToString(), Documentacion, DocAmec.nombredoc.Trim());
                        message.Body = string.Format("{0}\r\n\r\nAdjuntado Documentación {1}\r\n", message.Body, DocAmec.nombredoc.Trim());
                        saveFile = sFile.Replace("\\", "/");
                        Attachment data = new Attachment(saveFile);
                        data.NameEncoding = System.Text.Encoding.UTF8;
                        message.Attachments.Add(data);
                    }

                }
                if (message.Attachments.Count > 0)
                {
                    for (int x = 0; x < message.Attachments.Count; x++)
                    {
                        if (x != 0) Attachments = Attachments + ";";
                        Attachments = Attachments + message.Attachments[x].Name;
                    }

                }

                Mail.EnviaMail(message); //ENVIAMOS MAIL
                if (message.Attachments.Count > 0) GuardarMail = agAmecInfo.GuardaLogMail(miAmec.idamecs, "MailFarmaIndustria", message.To.ToString(), message.Subject, message.Body, Attachments, true, null);
                else GuardarMail = agAmecInfo.GuardaLogMail(miAmec.idamecs, "MailFarmaIndustria", message.To.ToString(), message.Subject, message.Body, null, true, null);

            }
            catch (Exception ex)
            {
                Alert.Show("Se ha producido un error enviando el mail a FarmaIndustria. Por favor revise la configuración del sistema", null);
                if (message.Attachments.Count > 0)
                {
                    GuardarMail = agAmecInfo.GuardaLogMail(miAmec.idamecs, "MailFarmaIndustria", message.To.ToString(), message.Subject, message.Body, Attachments, false, "Error enviando el Correo");
                }
                else
                {
                    GuardarMail = agAmecInfo.GuardaLogMail(miAmec.idamecs, "MailFarmaIndustria", message.To.ToString(), message.Subject, message.Body, null, false, "Error enviando el Correo");

                }
                Logger.Logger.PrintError(this.GetType().Name, "amecNotificaciones", ex.Message, ex);
                try
                {
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("contactoError"), ex);
                }
                catch { }
            }
        }



        #endregion

        #region "Validaciones y Comprobaciones"

        public bool ValidarCamposObligatorios(DAmecInfo miAmec)
        {

            //Vamos a controlar los campos obligatorios cuando:
            //1.Resometa un Amec
            //2.Guarde un amec cuando el estado sea diferente de borrador
            if (miAmec.idestado != 5)
            {
                if (string.IsNullOrEmpty(txtDescripcionAMEC.Text))
                {
                    Alert.Show("Campo Descripción del Programa obligatorio");
                    return false;
                }
                if (string.IsNullOrEmpty(txtgastoDesglose.Text))
                {
                    Alert.Show("Campo Gasto/Desgloce obligatorio");
                    return false;
                }
                if (string.IsNullOrEmpty(txtImporteTotalGasto.Text))
                {
                    Alert.Show("Campo Importe Gasto obligatorio");
                    return false;
                }

                if (odsProgramaAMEC.Select() == null)
                {
                    if ((ddlArchivoPrograma.SelectedValue.ToString() == "ARCHIVO") || ((txtDescProgAMEC.Text == null || txtDescProgAMEC.Text == "") && ddlArchivoPrograma.SelectedValue.ToString() == "DESCRIPCION") || ((txtURLProgAMEC.Text == null || txtURLProgAMEC.Text == "") && ddlArchivoPrograma.SelectedValue.ToString() == "ENLACE"))
                    {
                        Alert.Show("Campo Programa obligatorio");
                        return false;
                    }
                }

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
            //Validar los campos Enteros, Decimales y Fechas.

            int number;
            decimal numberdec;

            if (txtHoras.Text != "")
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtHoras.Text, "^([0-9.]){1,9}$"))
                {
                    Alert.Show("El formato del campo 'Duración Horas' incorrecto. Formato correcto: hora 'punto' minutos (Ejemplo 8.30)");
                    return false;
                }
            }

            if (txtProfesionalesSanitarios.Text != "")
            {
                if (!Int32.TryParse(txtProfesionalesSanitarios.Text, out number))
                {
                    Alert.Show("El formato del campo: 'N. Profesionales Sanitarios percibiendo honorarios de MSD' debe ser el correcto (un número).");
                    return false;
                }
            }

            if (txtNumPonentes.Text != "")
            {
                if (!Int32.TryParse(txtNumPonentes.Text, out number))
                {
                    Alert.Show("El formato del campo: 'N. de ponentes patrocinados por MSD' debe ser el correcto (un número).");
                    return false;
                }
            }

            if (txtTotalParticipantes.Text != "")
            {
                if (!Int32.TryParse(txtTotalParticipantes.Text, out number))
                {
                    Alert.Show("El formato del campo: 'N.total de participantes por parte de MSD' debe ser el correcto (un número).");
                    return false;
                }
            }


            if (txtImporteTotalGasto.Text != "")
            {
                if (!Decimal.TryParse(txtImporteTotalGasto.Text, out numberdec))
                {
                    Alert.Show("El formato del campo: 'Importe Total Gasto' debe ser el correcto (un número).");
                    return false;
                }
            }

            if (txtNumeroCartas.Text != "")
            {
                if (!Int32.TryParse(txtNumeroCartas.Text, out number))
                {
                    Alert.Show("El formato del campo: 'N.cartas contrato esperadas' debe ser el correcto (un número).");
                    return false;
                }
            }

            if (!ValidateFechaAmec()) return false;

            return true;
        }

        public bool ComprobarUnidadOrganizativa()
        {
            AgenteUnidadesOrgAMEC agenteUnidades = new AgenteUnidadesOrgAMEC();

            int numUnidades = agenteUnidades.ObtenerNumeroUnidadesOrgAMEC(miAmec.idamecs.ToString(), miAmec.idsolicitante.ToString(), unidOrgXDefecto.idunidad.ToString(), unidOrgXDefecto.idarea.ToString(), unidOrgXDefecto.idregion.ToString(), unidOrgXDefecto.iddistrito.ToString(), AccionUOCambioSolicitante.ToString(), "", "", "", "");
            if (ddlUnidad.SelectedValue == "0" && ddlRegion.SelectedValue == "0" && ddlDistrito.SelectedValue == "0" && ddlArea.SelectedValue == "0" && numUnidades == 0)
            {
                Alert.Show("No se Puede Someter o Guardar un Amec sin Unidad Organizativa");
                return false;
            }
            return true;

        }

        public bool ValidateFechaAmec()
        {
            //Validamos la fecha del Amec
            DateTime result;
            if (txtFechaComienzo.Text != "" && txtFechaFinalizacion.Text != "")
            {
                if (!DateTime.TryParse(txtFechaComienzo.Text, out result))
                {
                    Alert.Show("El formato del campo: 'Fecha de Comienzo Prevista' debe ser el correcto, una fecha en formato dd/mm/yyyy dd=dia mm=mes yyyy=año.");
                    return false;
                }
                if (!DateTime.TryParse(txtFechaFinalizacion.Text, out result))
                {
                    Alert.Show("El formato del campo: 'Fecha Finalizacion Prevista' debe ser el correcto una fecha en formato dd/mm/yyyy dd=dia mm=mes yyyy=año.");
                    return false;
                }

                if (miAmec.idestado == 5 || Sometiendo)
                {
                    if (DateTime.Compare(DateTime.Parse(txtFechaComienzo.Text), DateTime.Parse(DateTime.Now.ToShortDateString())) < 0)
                    {
                        Alert.Show("La Fecha de Comienzo Prevista del AMEC, NO puede ser anterior a hoy");
                        return false;
                    }
                    if (DateTime.Compare(DateTime.Parse(txtFechaFinalizacion.Text), DateTime.Parse(txtFechaComienzo.Text)) < 0)
                    {
                        Alert.Show("La Fecha de Finalización Prevista del AMEC, NO puede ser anterior a la Fecha de Comienzo Prevista");
                        return false;
                    }
                }
                else return true;
            }

            else if (txtFechaComienzo.Text != "" && txtFechaFinalizacion.Text == "")
            {
                if (miAmec.idestado == 5 || Sometiendo == true)
                {
                    if (DateTime.Compare(DateTime.Parse(txtFechaComienzo.Text), DateTime.Parse(DateTime.Now.ToShortDateString())) < 0)
                    {
                        Alert.Show("La Fecha de Comienzo Prevista del AMEC, NO puede ser anterior a hoy");
                        return false;
                    }
                }
            }
            return true;
        }

        public bool ValidateFechaFiltro()
        {
            //Validamos las fechas del filtro de Eventos o Actividades
            if (txtFechaDesde.Text != "" && txtFechaHasta.Text != "")
            {
                DateTime result;

                if (!DateTime.TryParse(txtFechaDesde.Text, out result))
                {
                    Alert.Show("El formato del campo: 'Fecha de Comienzo Prevista' debe ser el correcto, una fecha en formato dd/mm/yyyy dd=dia mm=mes yyyy=año.");
                    return false;
                }
                if (!DateTime.TryParse(txtFechaHasta.Text, out result))
                {
                    Alert.Show("El formato del campo: 'Fecha Finalizacion Prevista' debe ser el correcto una fecha en formato dd/mm/yyyy dd=dia mm=mes yyyy=año.");
                    return false;
                }
                if (DateTime.Compare(DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(DateTime.Now.ToShortDateString())) < 0)
                {
                    Alert.Show("La Fecha DESDE NO puede ser anterior a hoy");
                    return false;
                }
                if (DateTime.Compare(DateTime.Parse(txtFechaHasta.Text), DateTime.Parse(txtFechaDesde.Text)) < 0)
                {
                    Alert.Show("La Fecha HASTA NO puede ser anterior a la Fecha DESDE");
                    return false;
                }
                return true;
            }
            return true;
        }

        public bool ComprobarEstado(string idamecs)
        {
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
                return true;
            }
            return true;
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
                    return false;
                }
                return true;
            }
            return true;
        }

        public bool ComprobarLibros()
        {
            //Cuando el Tipo de Actividad del Amec es Libros se tiene que comprobar todas las opciones que permite la opción libro
            if (!string.IsNullOrEmpty(txtImporteTotalGasto.Text))
            {
                txtImporteTotalGasto.Text = txtImporteTotalGasto.Text.Replace(".", "");
                decimal importeLibro = Decimal.Parse(txtImporteTotalGasto.Text.Replace(",", "."), CultureInfo.InvariantCulture);
                txtImporteTotalGasto.Text = string.Format("{0:n2}", importeLibro);
                if (Int32.Parse(ddlActividad.SelectedValue) == 9 && importeLibro > 5000)
                {
                    Alert.Show("No se puede someter un Amec con Tipo de Actividad 'Material Formativo/Informativo a Instituciones Sanitarias' con un Importe superior a 5000€");
                    return false;
                }
                if (Int32.Parse(ddlActividad.SelectedValue) == 17 && importeLibro > 60)
                {
                    Alert.Show("No se puede someter un Amec con Tipo de Actividad 'Material Formativo/Informativo a Profesionales Sanitarios' con un Importe superior a 60€");
                    return false;
                }
                if (Int32.Parse(ddlActividad.SelectedValue) == 18 && importeLibro > 60)
                {
                    Alert.Show("No se puede someter un Amec con Tipo de Actividad 'Artículos de Utilidad Médica' con un Importe superior a 60€");
                    return false;
                }
                else if ((Int32.Parse(ddlActividad.SelectedValue) == 9 || Int32.Parse(ddlActividad.SelectedValue) == 17 || Int32.Parse(ddlActividad.SelectedValue) == 18) && chkParaguas.Checked == true)
                {
                    Alert.Show("No se puede someter un Amec Paraguas con Tipo de Actividad LIBROS");
                    return false;
                }

                else if ((Int32.Parse(ddlActividad.SelectedValue) == 9 || Int32.Parse(ddlActividad.SelectedValue) == 17 || Int32.Parse(ddlActividad.SelectedValue) == 18) && rblPreaprobNegocio.SelectedItem.Value == "1")
                {
                    Alert.Show("No se puede someter un Amec con Tipo de Actividad LIBROS con Preaprobado Negocio");
                    return false;
                }
                else return true;
            }

            else
            {
                if ((Int32.Parse(ddlActividad.SelectedValue) == 9 || Int32.Parse(ddlActividad.SelectedValue) == 17 || Int32.Parse(ddlActividad.SelectedValue) == 18) && chkParaguas.Checked == true)
                {
                    Alert.Show("No se puede someter un Amec Paraguas con Tipo de Actividad LIBROS");
                    return false;
                }

                else if ((Int32.Parse(ddlActividad.SelectedValue) == 9 || Int32.Parse(ddlActividad.SelectedValue) == 17 || Int32.Parse(ddlActividad.SelectedValue) == 18) && rblPreaprobNegocio.SelectedItem.Value == "1")
                {
                    Alert.Show("No se puede someter un Amec con Tipo de Actividad LIBROS con Preaprobado Negocio");
                    return false;
                }

                else if (txtImporteTotalGasto.Text == null || txtImporteTotalGasto.Text == "")
                {
                    Alert.Show("El importe total del gasto tiene que tener un valor");
                    return false;
                }
                else return true;
            }

        }

        #endregion

        #region "Funciones Auxiliares"

        public void MostrarPorTipoActividad(string TipoActividad, bool esCargaFormulario)
        {
            try
            {
                if (TipoActividad == "9" || TipoActividad == "13" || TipoActividad == "14" || ddlActividad.Text == "17" || ddlActividad.Text == "18")
                {
                    rblPreaprobMedico.SelectedValue = "0";
                    rblPreaprobNegocio.SelectedValue = "0";
                    rblPreaprovLegal_Comp.SelectedValue = "0";
                    rblPreaprovLegal_Comp.Enabled = false;
                    rblPreaprobMedico.Enabled = false;
                    rblPreaprobNegocio.Enabled = false;
                    if (miAmec != null && miAmec.idestado != 5 && esCargaFormulario)
                        ddlActividad.Enabled = false;
                }
                else
                {
                    rblPreaprobMedico.Enabled = true;
                    rblPreaprobNegocio.Enabled = true;
                    rblPreaprovLegal_Comp.Enabled = true;
                }
                if (ddlActividad.Text == "9" || ddlActividad.Text == "17" || ddlActividad.Text == "18")
                {
                    plAMEC_Historial.Visible = true;
                    plAMEC_Comentarios.Visible = true;
                    plAMECDatos7.Visible = true;
                    Panel8.Visible = true;
                    plPaso1.Visible = false;
                    plAMECDatos1.Visible = true;
                    Panel9.Visible = true;
                    rblPreaprobMedico.SelectedValue = "1";
                    rblPreaprobNegocio.SelectedValue = "0";
                    rblPreaprovLegal_Comp.SelectedValue = "0";
                }
                else
                {
                    if (miAmec.idestado == 9) plAMEC_Historial.Visible = false;
                    plAMEC_Comentarios.Visible = true;
                    plAMECDatos7.Visible = true;
                    Panel8.Visible = true;
                    plPaso1.Visible = true;
                    plAMECDatos1.Visible = true;
                    Panel9.Visible = true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public string ObtenerUnidadesOrganizativasParaCorreo()
        {
            string listUnidadesOrg = "";
            AgenteUnidadesOrgAMEC agUnidad = new AgenteUnidadesOrgAMEC();
            ICollection<DUnidadesOrganizativasAmec> UnidadOrgAmec = agUnidad.ObtenerUnidadesOrgAmec(miAmec.idamecs.ToString(), miAmec.idsolicitante.ToString(), null, null, null, null, null, null, null, null, null, null, 0, 10);
            foreach (DUnidadesOrganizativasAmec unidorg in UnidadOrgAmec)
            {
                listUnidadesOrg = string.Format("{4} Unidad: {0}, Area: {1}, Región: {2}, Distrito: {3} \r\n", unidorg.unidad, unidorg.area, unidorg.region, unidorg.distrito, listUnidadesOrg);
            }
            return listUnidadesOrg;
        }

        public string ObtenerUnidadesOrganizativasParaCorreoHtml()
        {
            string listUnidadesOrg = "";
            AgenteUnidadesOrgAMEC agUnidad = new AgenteUnidadesOrgAMEC();
            ICollection<DUnidadesOrganizativasAmec> UnidadOrgAmec = agUnidad.ObtenerUnidadesOrgAmec(miAmec.idamecs.ToString(), miAmec.idsolicitante.ToString(), null, null, null, null, null, null, null, null, null, null, 0, 10);
            foreach (DUnidadesOrganizativasAmec unidorg in UnidadOrgAmec)
            {
                listUnidadesOrg = string.Format("{4} Unidad: {0}, Area: {1}, Región: {2}, Distrito: {3} <br>", unidorg.unidad, unidorg.area, unidorg.region, unidorg.distrito, listUnidadesOrg);
            }
            return listUnidadesOrg;
        }

        public void ImportarDatosExpediente(int idexpediente)
        {
            RepositorioFlujo estadoAMEC = null;
            estadoAMEC = new RepositorioFlujo(miAmec.idamecs, (Int32)datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
            EOS.Entidades.InformacionExpediente InfoExpediente = estadoAMEC.IncorporarExpediente(idexpediente);
            //Impoorte
            txtImporteTotalGasto.Text = InfoExpediente.importe != 0 ? InfoExpediente.importe.ToString() : "0";

            //Sede
            txtLugarSede.Text = !string.IsNullOrEmpty(InfoExpediente.LugarRealizacion) ? InfoExpediente.LugarRealizacion.ToString() : "";

            //fecha Inicio
            txtFechaComienzo.Text = !string.IsNullOrEmpty(InfoExpediente.FechaComienzo.ToString()) ? InfoExpediente.FechaComienzo.ToShortDateString() : "";

            //fecha fin
            txtFechaFinalizacion.Text = !string.IsNullOrEmpty(InfoExpediente.FechaFin.ToString()) ? InfoExpediente.FechaFin.ToShortDateString() : "";

            //Gastos
            string GastosDesglose = "0";
            if (InfoExpediente.detalleservicios != null)
            {
                foreach (Entidades.DetalleServicios detalleservicios in InfoExpediente.detalleservicios)
                {

                    GastosDesglose = GastosDesglose + detalleservicios.descripcion.ToString();
                }

                txtgastoDesglose.Text = GastosDesglose;
            }
        }

        protected void AsignarEventoAmec(int idcongres)
        {
            AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
            string idamec = miAmec.idamecs;
            if (ValidarCampos())
            {
                if (agAmecInfo.EstaGuardadoAmec(miAmec.idamecs) != true)
                {
                    //AMEC no insertado. Guardamos valores del amec e insertamos amec y la unidad organizativa por defecto.
                    GuardarNuevoAmec();
                }
                else ActualizarAmec();
                int EstaAsig = agAmecInfo.CrearRelacionAmecCongreso(idcongres, miAmec.idamecs, datosUsuarioAMEC.IdPeticionario);
                if (EstaAsig == 1) { }//S'ha de treure un text que s'ha assignat correctament TODO 
                this.eosContentResults.Visible = false;
                lvActividadAsigAmec.DataBind();
            }
        }

        protected void ActualizarAmec()
        {
            //A parte del Importe tendremos que comprobar que alguien no ha cambiado el estado del Amec (Otro usuario haya aprobado o rechazado el amec mientras tu estabas dentro)
            AgenteAmecInfo agAMECinfo = new AgenteAmecInfo();
            RepositorioFlujo estadoAMEC = null;
            CargarValoreAmecFormulario();
            if (ComprobarEstado(miAmec.idamecs))
            {
                if (miAmec.idestado != 5 && !Sometiendo && !ComprobarImporte())
                {
                    Alert.Show("No se puede Guardar el Amec. Es Obligatorio Resometer debido a la modificación del Importe.");
                }
                else
                {
                    DAmecInfo DAmecInf = new DAmecInfo();
                    bool cambioAgencia = false;
                    DAmecInf = agAMECinfo.ModificarAMEC(miAmec, out cambioAgencia, true);

                    btnAnadirUnidadesOrgAMEC.Enabled = true;
                    btnAnadirUnidadesOrgAMEC.Visible = true;
                    plAMEC_Historial.Visible = true;
                    estadoAMEC = new RepositorioFlujo(miAmec.idamecs, (Int32)datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
                    HabilitaBotones(estadoAMEC);
                }
            }
        }

        protected void ObtenerAMEC(string idamec)
        {
            //Clase encargada de recuperar todos los datos relativos a un AMEC y datos relacionados
            AgenteAmecInfo agenteAMEC = new AgenteAmecInfo();
            miAmec = new DAmecInfo();
            //Carga un amec dado un idamec
            miAmec = agenteAMEC.CargarTodosValoresAmec(idamec);
            if (miAmec.idconfempresa == 3)
            {
                string query = "SELECT idpeticionario from peticionarios where login in (select REPLACE(login, 'AMEX_', '') from peticionarios where idpeticionario = " + miAmec.idsolicitante + ")";
                string value = Quodem.Sql.SqlServerClient.GetValue(query);
                if (!String.IsNullOrWhiteSpace(value))
                {
                    int outValue = miAmec.idsolicitante.Value;
                    int.TryParse(value, out outValue);
                    miAmec.idsolicitante = outValue;
                }
            }

            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            //Carga peticionarios dado un id de peticionario desde la tabla dbo_iw_peticionarios
            datosUsuarioSolicitante = agenteUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());

            //obetener uidad organizativa por defecto
            unidOrgXDefecto = new DUnidadesOrganizativasAmec
            {
                idunidad = datosUsuarioSolicitante.idunidad,
                idarea = datosUsuarioSolicitante.Idarea,
                idregion = datosUsuarioSolicitante.idregion,
                iddistrito = datosUsuarioSolicitante.iddistrito
            };
        }

        protected int AnadirUnidadOrgAMEC()
        {
            //Insertar una nueva UNIDAD ORGANIZATIVA en el AMEC.
            //Y Volver a bindar en la listview de unidades organizativas.

            string idarea = null;
            string idunidad = null;
            string idregion = null;
            string iddistrito = null;
            int resultado = 0;
            AgenteUnidadesOrgAMEC agUnidad = new AgenteUnidadesOrgAMEC();
            if (ddlUnidadMas.SelectedValue == "-1" || ddlUnidadMas.SelectedValue == "0")
            {
                Alert.Show("Es obligatorio informar la unidad para crear una nueva unidad organizativa asociada al amec.");
            }
            else
            {
                idunidad = ddlUnidadMas.SelectedValue;

                if (ddlAreaMas.SelectedValue != "-1" && ddlAreaMas.SelectedValue != "0")
                {
                    idarea = ddlAreaMas.SelectedValue;
                }
                else
                {
                    idarea = null;
                }


                if (ddlRegionMas.SelectedValue != "-1" && ddlRegionMas.SelectedValue != "0")
                {
                    idregion = ddlRegionMas.SelectedValue;
                }
                else
                {
                    idregion = null;
                }


                if (ddlDistritoMas.SelectedValue != "-1" && ddlDistritoMas.SelectedValue != "0")
                {
                    iddistrito = ddlDistritoMas.SelectedValue;
                }
                else
                {
                    iddistrito = null;
                }
                bool duplicado = agUnidad.ComprobarDuplicidadUnidadOrgAMEC(miAmec.idamecs.ToString(), idunidad, idarea, idregion, iddistrito);

                if (!duplicado) resultado = agUnidad.InsertUnidadOrgAMEC(miAmec.idamecs.ToString(), miAmec.idcreadopor.ToString(), idunidad, idarea, idregion, iddistrito);
                else Alert.Show("Ya EXISTE la misma estructura en el Amec a Nivel Inferior o Superior. No es posible volver a incluirla en el amec ya que duplicaría el flujo de aprobación.");

            }
            return resultado;
        }

        protected void InsertarUnidOrgXDefecto()
        {
            //Insertar la UNIDAD ORGANIZATIVA x defecto en el AMEC.
            unidOrgXDefecto.idamecs = miAmec.idamecs;

            //MARTA JUEVES2
            if (unidOrgXDefecto.idarea.ToString() == "0" || unidOrgXDefecto.idarea.ToString() == "-1")
            {
                unidOrgXDefecto.idarea = null;
            }


            if (unidOrgXDefecto.idregion.ToString() == "0" || unidOrgXDefecto.idregion.ToString() == "-1")
            {
                unidOrgXDefecto.idregion = null;
            }


            if (unidOrgXDefecto.iddistrito.ToString() == "0" || unidOrgXDefecto.iddistrito.ToString() == "-1")
            {
                unidOrgXDefecto.iddistrito = null;
            }


            AgenteUnidadesOrgAMEC agUnidad = new AgenteUnidadesOrgAMEC();
            int resultado = agUnidad.InsertUnidadOrgAMEC(miAmec.idamecs.ToString(), miAmec.idcreadopor.ToString(), unidOrgXDefecto.idunidad.ToString(), unidOrgXDefecto.idarea.ToString(), unidOrgXDefecto.idregion.ToString(), unidOrgXDefecto.iddistrito.ToString());
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
                else
                {
                    DAmec miAMEC = null;

                    if (!string.IsNullOrEmpty(sIdAMEC))
                    {
                        miAMEC = agenteExp.ObtenerEntidadAMECporID(sIdAMEC);
                    }
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
                    imgEstadoAMEC.ImageUrl = "~/Styles/images/ic_aceptado.png";
                    break;
                case 3:
                    imgEstadoAMEC.ImageUrl = "~/Styles/images/ic_estado_cancelado.png";
                    break;
                case 4:
                    imgEstadoAMEC.ImageUrl = "~/Styles/images/ic_estado_cancelado.png";
                    break;
                case 5:
                    imgEstadoAMEC.ImageUrl = "~/Styles/images/ic_enviado.png";
                    break;
                default:
                    imgEstadoAMEC.ImageUrl = "~/Styles/images/ic_en_proceso.png";
                    break;
            }

        }

        protected void RecuperaSesion()
        {

            if (Session["NuevoAgente"] != null)
                agAMEC = (AgenteAMEC)Session["NuevoAgente"];

            if (Session["DatosUsuarioAMEC"] != null)
                datosUsuarioAMEC = (EOS.Entidades.Datos.DVPeticionariosRoles)Session["DatosUsuarioAMEC"];

            if (Session["DatosSolicitante"] != null)
                datosUsuarioSolicitante = (EOS.Entidades.Datos.DDatosPersonalesUsuario)Session["DatosSolicitante"];

            if (Session["UnidOrgXDefecto"] != null)
                unidOrgXDefecto = (DUnidadesOrganizativasAmec)Session["UnidOrgXDefecto"];

            if (Session["AccionUO"] != null)
                AccionUOCambioSolicitante = (bool)Session["AccionUO"];
        }

        public bool VisualitzarPrograma()
        {

            if (odsProgramaAMEC.Select() != null)
            {
                rfvUploadPrograma.Enabled = false;
                rfvURLProgAMEC.Enabled = false;
                rfvDescProgAMEC.Enabled = false;
                rfvUploadPrograma.EnableClientScript = false;
                rfvURLProgAMEC.EnableClientScript = false;
                rfvDescProgAMEC.EnableClientScript = false;
                return false;
            }
            switch (ddlArchivoPrograma.SelectedValue.ToString())
            {
                case "ARCHIVO":
                    rfvUploadPrograma.Enabled = true;
                    break;
                case "ENLACE":
                    rfvURLProgAMEC.Enabled = true;
                    break;
                case "DESCRIPCION":
                    rfvDescProgAMEC.Enabled = true;
                    break;
                default:
                    break;
            }
            return true;
        }

        protected void GuardarNuevoAmec()
        {
            AgenteAmecInfo agAMECinfo = new AgenteAmecInfo();

            CargarValoreAmecFormulario();
            miAmec = agAMECinfo.NuevoAMEC(miAmec);
            InsertarUnidOrgXDefecto();

            btnAnadirUnidadesOrgAMEC.Enabled = true;
            btnAnadirUnidadesOrgAMEC.Visible = true;
            plAMEC_Historial.Visible = true;
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
                if (Page.Request.UrlReferrer.AbsoluteUri.Contains("NuevoDetalleAMEC.aspx"))
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
