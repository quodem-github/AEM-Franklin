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

    public partial class DetalleAMEC : Page
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

        public bool AsociarEvento()
        {
            return chkAsociarEvento.Checked;
        }

        public bool DocAdicional()
        {
            return chkDocAdicional.Checked;
        }

        public bool VerCriterioEspecifico()
        {
            
            return ddlCriteriosSeleccion.Items.Count > 0 && ddlCriteriosSeleccion.SelectedItem.Text == "OTRO";
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
                    //Si entro por primera vez

                    LimpiarSession();
                    datosUsuarioAMEC = (DVPeticionariosRoles)Session["rolesuser"];
                    agAMEC = datosUsuarioAMEC != null ? new AgenteAMEC(datosUsuarioAMEC.IdPeticionario) : null;
                    //Comprobamos si es un nuevo AMEC o si es una visualizacion de AMEC.                 
                    string sIdAmec = Request.QueryString["idamec"];
                    if (sIdAmec != null)
                    {
                        string idamec = sIdAmec;
                        ChechPermisosDeVisibilidad(idamec);
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
                        rblFarmaindustria.Attributes.Add("onclick",
                            "MostrarDescDocumentacionFarmaIndustria('" + rblFarmaindustria.ClientID + "'," +
                            rblFarmaindustria.Items.Count + ")");
                        MostrarPorTipoActividad(ddlActividad.Text, true);
                    }
                    else
                    {
                        //NUEVO AMEC 
                        Response.Redirect("~/", true);
                        //Se Puede Crear Un Nuevo Amec Viniendo del Expediente o viniendo del Boton Nuevo Amec del Menú
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
                            HabilitaBotones();
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
                    if (!chkParaguas.Checked)
                    {
                        AgenteUnidadesOrgAMEC agenteUnidORg = new AgenteUnidadesOrgAMEC();
                        if (
                            agenteUnidORg.ObtenerNumeroUnidadesOrgAMEC(txtNAmec.Text.ToString(),
                                miAmec.idsolicitante.ToString(), "", "", "", "", "", "", "", "", "") > 1)
                        {
                            chkParaguas.Checked = true;
                            Alert.Show("Un Amec no paraguas NO puede tener mas de una unidad organizativa");

                        }
                    }

                    HabilitaBotones();
                }
                //btnAnadirAprobador.Enabled = /*chkParaguas.Checked ||*/ (miAmec.idestado != 5 && miAmec.idsolicitante == datosUsuarioAMEC.IdPeticionario);
                listManagers.DataSource = LstManagers;
                listManagers.DataBind();
                this.SetFocus("txtDescripcionAMEC");

                AgenteAmecInfo aAmec = new AgenteAmecInfo();


                //
                if (miAmec.idestado == 1 && miAmec.idcreadopor == datosUsuarioAMEC.IdPeticionario && miAmec.idconfempresa <= 0)
                {
                    btnGuardar.Visible = true;
                    plAMECDatos1.Enabled = true;
                    ddlSolicitante.Enabled = false;
                    ddlActividad.Enabled = false;
                    ddlAgencias.Enabled = true;
                    chkParaguas.Enabled = false;
                    txtDescripcionAMEC.Enabled = false;
                    rblPreaprobMedico.Enabled = false;
                    rblFarmaindustria.Enabled = false;
                    rblPreaprobNegocio.Enabled = false;
                    rblCasosClinicos.Enabled = false;
                    rblPreaprovLegal_Comp.Enabled = false;
                    rblpoliticaN20.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                EOSLogger.PrintError(this.GetType().Name, "Page_Load", ex.Message, ex);
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
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
            if (ddlCriteriosSeleccion.SelectedItem.Text == "OTRO") miAmec.criterioespecificado = txtEspecificarCriterioSeleccion.Text;
            if (!string.IsNullOrEmpty(txtHoras.Text)) { miAmec.duracionhoras = txtHoras.Text; }
            if (!string.IsNullOrEmpty(txtLugarSede.Text)) { miAmec.lugarsede = txtLugarSede.Text; }
            if (!string.IsNullOrEmpty(txtgastoDesglose.Text)) { miAmec.conceptogastos = txtgastoDesglose.Text; }
            if (!string.IsNullOrEmpty(txtCargoADaxas.Text)) { miAmec.cargoadaxas = txtCargoADaxas.Text; }

            miAmec.idcreadopor = int.Parse(txtIdCreadoPor.Text);
            miAmec.idconfempresa = Convert.ToInt32(ddlAgencias.SelectedValue);
            miAmec.nwein = txtWein.Text;
            miAmec.idcargo = datosUsuarioSolicitante.IdCargo;
            miAmec.idposition = datosUsuarioSolicitante.IdPosition;
            miAmec.idtipoactividad = Int32.Parse(ddlActividad.SelectedValue);
            miAmec.preaprobadaamed = rblPreaprobMedico.SelectedItem.Value == "1";
            miAmec.preaprobadaneg = rblPreaprobNegocio.SelectedItem.Value == "1";
            miAmec.preaprobadaleg = rblPreaprovLegal_Comp.SelectedItem.Value == "1";
            miAmec.farmaindustria = rblFarmaindustria.SelectedItem.Value == "1";
            miAmec.casosclinicos = rblCasosClinicos.SelectedItem.Value == "1";
            miAmec.politicaN20 = rblpoliticaN20.SelectedValue == "1";
            miAmec.detallecriterios = ddlCriteriosSeleccion.SelectedItem.Text;
            miAmec.idcriterioseleccion = Int32.Parse(ddlCriteriosSeleccion.SelectedValue);
            miAmec.medicosfichero = rbMedicosGenesys.SelectedItem.Value == "1";

            //Validar si están los campos correctamente//
            if (ValidarCampos())
            {
                if (!string.IsNullOrEmpty(txtTotalParticipantes.Text)) { miAmec.participantesmsd = uint.Parse(txtTotalParticipantes.Text); }
                if (!string.IsNullOrEmpty(txtNumPonentes.Text)) { miAmec.ponentespatrocinados = uint.Parse(txtNumPonentes.Text); }
                if (!string.IsNullOrEmpty(txtFechaComienzo.Text)) { miAmec.fechacomienzo = DateTime.Parse(txtFechaComienzo.Text); }
                if (!string.IsNullOrEmpty(txtFechaFinalizacion.Text)) { miAmec.fechafinalizacion = DateTime.Parse(txtFechaFinalizacion.Text); }
                if (!string.IsNullOrEmpty(txtProfesionalesSanitarios.Text)) { miAmec.profesionalessanitarios = uint.Parse(txtProfesionalesSanitarios.Text); }
                if (!string.IsNullOrEmpty(txtNumeroCartas.Text)) { miAmec.cartascontrato = uint.Parse(txtNumeroCartas.Text); }

                if (!string.IsNullOrEmpty(txtImporteTotalGasto.Text))
                {
                    txtImporteTotalGasto.Text = txtImporteTotalGasto.Text.Replace(".", "");
                    miAmec.importegasto = Decimal.Parse(txtImporteTotalGasto.Text.Replace(",", "."), CultureInfo.InvariantCulture);
                    txtImporteTotalGasto.Text = string.Format("{0:n2}", miAmec.importegasto);
                }
            }

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
            AgentePeticionarioManager agPeticionarioManager = new AgentePeticionarioManager();
            datosUsuarioSolicitante = agenteUsu.ObtenerDatosPersonalesPorIDPeticionario(datosUsuarioAMEC.IdPeticionario.ToString());
            datosPeticionarioManager = agPeticionarioManager.ObtenerPeticionarioManager(datosUsuarioAMEC.IdPeticionario);

            miAmec.idsolicitante = datosUsuarioSolicitante.IdPeticionario;
            miAmec.idcargo = datosUsuarioSolicitante.IdCargo;
            miAmec.idposition = datosUsuarioSolicitante.IdPosition;
            miAmec.idamecs = agAMECinfo.DameSiguienteIdAmecs();

            miAmec.idcreadopor = datosUsuarioAMEC.IdPeticionario; //OBLIGATORIO
            miAmec.fechaamecs = System.DateTime.Today; //OBLIGATORIO
            miAmec.nwein = datosUsuarioSolicitante.wein;  //miAmec.nwein = datosUsuarioAMEC.wein; //MODIFICADO 06/02/2012 
            miAmec.idestado = 5; //ESTADO BORRADOR porque es un nuevo AMEC.
            if (datosPeticionarioManager != null)
            {
                txtDepartamento.Text = datosPeticionarioManager.departament;
                txtDistrito.Text = datosPeticionarioManager.district;
                txtFuerzaVentas.Text = datosPeticionarioManager.saleforce;
            }

        }

        protected void InicializaCombos()
        {
            RellenarCombos();
        }

        /// <summary>
        /// Establece valores para algunos campos del AMEC en base al estado
        /// </summary>
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
                    }

                    if (!string.IsNullOrWhiteSpace(miAmec.idamecsrelacionado))
                    {
                        lbidAMECClonado.InnerText = string.Format(" (Clonado-resometido de {0})", miAmec.idamecsrelacionado);
                    }
                    else
                    {
                        lbidAMECClonado.InnerText = string.Empty;
                    }

                    ImagenEstadoAMEC();
                    lbidAMEC.InnerText = miAmec.idamecs.ToString();

                    rblPreaprobMedico.SelectedValue = "1";
                    rblPreaprobNegocio.SelectedValue = "1";
                    rblPreaprovLegal_Comp.SelectedValue = "0";
                    rblFarmaindustria.SelectedValue = "0";
                    rblCasosClinicos.SelectedValue = "0";
                    rblpoliticaN20.SelectedValue = "1";
                    ddlCriteriosSeleccion.SelectedValue = "3";
                    rblCasosClinicos.Attributes.Add("onclick", "checkControl('" + rblCasosClinicos.ClientID + "'," + rblCasosClinicos.Items.Count + ")");
                    rblFarmaindustria.Attributes.Add("onclick", "MostrarDescFarmaIndustria('" + rblFarmaindustria.ClientID + "'," + rblFarmaindustria.Items.Count + ")");
                }


                ddlSolicitante.SelectedValue = datosUsuarioSolicitante.IdPeticionario.ToString();
                //obtener cargo de los datosroles.
                txtCargo.Text = agAMEC.ObtenerPosition();

                txtWein.Text = datosUsuarioAMEC.wein;
                txtFecha.Text = String.Format("{0:dd/MM/yyyy}", miAmec.fechaamecs);
                txtCreadoPor.Text = agAMEC.ObtenerNombreUsuario();
                txtIdCreadoPor.Text = miAmec.idcreadopor.ToString();
                txtIdEstado.Text = miAmec.idestado.ToString();

            }
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

            rblPreaprobMedico.SelectedValue = miAmec.preaprobadaamed == true ? "1" : "0";
            rblPreaprobNegocio.SelectedValue = miAmec.preaprobadaneg == true ? "1" : "0";
            rblPreaprovLegal_Comp.SelectedValue = miAmec.preaprobadaleg == true ? "1" : "0";
            rblFarmaindustria.SelectedValue = miAmec.farmaindustria == true ? "1" : "0";
            rblCasosClinicos.SelectedValue = miAmec.casosclinicos == true ? "1" : "0";

            if (miAmec.participantesmsd.HasValue) { txtTotalParticipantes.Text = miAmec.participantesmsd.ToString(); }

            ddlCriteriosSeleccion.SelectedValue = miAmec.idcriterioseleccion.ToString();
            ddlCriteriosSeleccion.Text = miAmec.detallecriterios;
            if (miAmec.criterioespecificado != null || miAmec.criterioespecificado != "") txtEspecificarCriterioSeleccion.Text = miAmec.criterioespecificado;

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
            if (miAmec.importegasto.HasValue) { txtImporteTotalGasto.Text = string.Format("{0:n2}", miAmec.importegasto); }
            if (miAmec.cartascontrato.HasValue) { txtNumeroCartas.Text = miAmec.cartascontrato.ToString(); }

            //txtArchivo.Text = miAmec.programaamecs.ToString(); 
            txtURLProgAMEC.Text = miAmec.urlprograma.ToString();
            txtDescProgAMEC.Text = miAmec.descripcionobjetivo.ToString();

            //Departamento, fuerza de venta, distrito
            AgentePeticionarioManager agPeticionarioManager = new AgentePeticionarioManager();
            datosPeticionarioManager = agPeticionarioManager.ObtenerPeticionarioManager(miAmec.idsolicitante.GetHashCode());
            if (datosPeticionarioManager != null)
            {
                txtDepartamento.Text = datosPeticionarioManager.departament;
                txtDistrito.Text = datosPeticionarioManager.district;
                txtFuerzaVentas.Text = datosPeticionarioManager.saleforce;
            }

            //EVENTOS
            AgenteAmecInfo aginfo = new AgenteAmecInfo();
            if (aginfo.ObtenerNumeroActividadesAsigAmec(miAmec.idamecs) > 0) chkAsociarEvento.Checked = true;
            //DOCUMENTOS
            if (aginfo.ObtenerNumeroDocumentacionAMEC(miAmec.idamecs.ToString()) > 0) chkDocAdicional.Checked = true;
            //Aprobadores
            AgentePeticionarioManager agPetManager = new AgentePeticionarioManager();
            AgenteAprobadorAmec agAprobAmec = new AgenteAprobadorAmec();
            List<int> idAprobadores = agAprobAmec.ObernerIdsAprobadoresAmec(miAmec.idamecs.ToString());
            if (idAprobadores.Count > 0) LstManagers = agPetManager.ObtenerPeticionarioDesdeLista(idAprobadores);




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
                if (agAmecInfo.HayAmecRelacionExpediente(miAmec.idamecs) > 0)
                {
                    ddlAgencias.Enabled = false;
                }
                if (agAmecInfo.EstaGuardadoAmec(miAmec.idamecs))
                {
                    GestorPermisos gestor = new GestorPermisos();
                    IPermiso permisos = gestor.GetPermisos(miAmec.idestado.GetHashCode());
                    bool aprobarRechazar = permisos.PuedoAprobarYRechazar(miAmec.idamecs, datosUsuarioAMEC.IdPeticionario);
                    bool someter = permisos.PuedoSometer(miAmec.idamecs, datosUsuarioAMEC.IdPeticionario);
                    int usuarioDelegado = agDelegacion.ObtenerDelegacion(miAmec.idsolicitante.Value);
                    
                    btnAprobar.Visible = aprobarRechazar;
                    btnAprobarConCondicion.Visible = aprobarRechazar;
                    btnRechazar.Visible = aprobarRechazar;
                    btnSometer.Visible = someter;
                    btnCancelar.Visible = (miAmec.idsolicitante == datosUsuarioAMEC.IdPeticionario || usuarioDelegado == datosUsuarioAMEC.IdPeticionario) && miAmec.idestado != 4;
                    btnGuardar.Visible = (miAmec.idcreadopor == datosUsuarioAMEC.IdPeticionario && miAmec.idestado == 5) || (miAmec.idsolicitante == datosUsuarioAMEC.IdPeticionario || usuarioDelegado == datosUsuarioAMEC.IdPeticionario);

                    //ESTADO PENDIENTE SOMETER
                    if (miAmec != null && miAmec.idestado.ToString() == "35")
                    {
                        btnCancelar.Visible = false;
                        btnGuardar.Visible = false;
                        btnAprobar.Visible = false;
                        btnAprobarConCondicion.Visible = false;
                        btnRechazar.Visible = false;
                    }
                }
                else
                {
                    //Aquí entra cuando entras/creas el Amec por primera vez
                    btnCancelar.Visible = miAmec.idsolicitante == datosUsuarioAMEC.IdPeticionario;
                    btnGuardar.Visible = true;
                    if (miAmec.idsolicitante != datosUsuarioAMEC.IdPeticionario && miAmec.idcreadopor == datosUsuarioAMEC.IdPeticionario)
                    {
                        btnCancelar.Visible = true;
                        btnSometer.Visible = false;
                    }
                    else
                    {
                        btnSometer.Visible = true;
                    }
                    btnRechazar.Visible = false;
                    btnAprobar.Visible = false;
                    btnAprobarConCondicion.Visible = false;
                }

                //BOTON RESOMETER
                if (miAmec.idestado != 5 && !IsPostBack)
                {
                    AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
                    bool YaSeHaEnviadoAFarma = agAmecInf.ComprobarSiSeEnvioAFarma(miAmec.idamecs);
                    bool YaSeHaEnviadoACasosClinicos = agAmecInf.ComprobarSiSeEnvioACasosClinicos(miAmec.idamecs);
                    btnSometer.OnClientClick = "return ConfirmarResometer('" + YaSeHaEnviadoAFarma + "','" + YaSeHaEnviadoACasosClinicos + "')";
                }

                btnAdjuntarDoc.OnClientClick = miAmec.idestado != 5 ? "return ConfirmarEnviarFarmaIndustria()" : "return CheckFileSize()";

                //Visibility and Edit AMEC
                if (btnSometer.Visible)
                {
                    plAMEC_Comentarios.Visible = true;
                    btnGuardar.Visible = true;
                    plAMECDatos7.Enabled = true;
                    Panel8.Enabled = true;
                    plPaso1.Enabled = true;
                    plAMECDatos1.Enabled = true;
                    Panel9.Enabled = true;
                }
                else if (btnAprobar.Visible)
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
                        btnSometer.Visible = true;
                        plAMEC_Comentarios.Visible = true;
                        plAMEC_Comentarios.Enabled = true;
                    }
                    else if (miAmec.idestado == 5 && miAmec.idcreadopor == datosUsuarioAMEC.IdPeticionario)
                    {
                        btnSometer.Visible = false;
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
            }
            catch (Exception ex)
            {
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
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

            //MANAGER//
            if (miAmec.idsolicitante.HasValue && miAmec.idsolicitante.Value > 0)
            {
                ddlManager.DataSource = agPeticionarioManager.ObtenerManagersFullNameNoSolicitanteNoResponsable(miAmec.idsolicitante.Value);
            }
            else
            {
                ddlManager.DataSource = agPeticionarioManager.ObtenerManagersFullName();
            }
            ddlManager.DataValueField = "idpeticionario";
            ddlManager.DataTextField = "nombre_completo";
            ddlManager.DataBind();
            ddlManager.SelectedIndex = 0;

            //FIN MANAGER//

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
            datosFiltroActividad = agenteFlujo.ObtenerTipoActividad(miAmec != null ? miAmec.idtipoactividad.ToString() : "0", false);
            ddlActividad.DataSource = datosFiltroActividad;
            ddlActividad.DataBind();
        }

        #endregion

        #region Botones

        protected void btnAnadirAprobador_Click(object sender, EventArgs e)
        {
            try
            {
                int idmanager = int.Parse(ddlManager.SelectedValue);
                if (idmanager < 0)
                {
                    Alert.Show("No se ha seleccionado ningún aprobador");
                    return;
                }
                AgentePeticionarioManager agPetManager = new AgentePeticionarioManager();
                DPeticionarioManager manager = agPetManager.ObtenerPeticionarioManager(idmanager);
                bool estaInsertado = LstManagers.FirstOrDefault(x => x.IdPeticionario == idmanager) != null;
                if (!estaInsertado && chkParaguas.Checked)
                {
                    LstManagers.Add(manager);
                    listManagers.DataSource = LstManagers;
                    listManagers.DataBind();
                }
                else
                {
                    Alert.Show(!chkParaguas.Checked
                        ? "Para agregar aprobadores adicionales es necesario marcar la opción paraguas. En el caso de no marcarla, se añadirá el aprobador por defecto (tu superior jerárquico)"
                        : "No se puede añadir un aprobador ya existente en la lista");
                    return;
                }
                //Comprobar si el AMEC esta ya guardado/insertado.
                if (datosUsuarioAMEC.IdPeticionario == datosUsuarioSolicitante.IdPeticionario || miAmec.idestado == EstadosAmec.Borrador.GetHashCode())
                {
                    AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
                    AgenteComentariosAMEC agComAmec = new AgenteComentariosAMEC();
                    if (ValidarCampos())
                    {
                        if (agAmecInfo.EstaGuardadoAmec(miAmec.idamecs))
                        {
                            //AMEC ya insertado solo añadimos la unidad organizativa correspondiente.
                            AniadirAprobador(manager);
                            bool cambioAgencia = false;
                            ActualizarAmec(out cambioAgencia);
                            if (miAmec.idestado != EstadosAmec.Borrador.GetHashCode() && !estaInsertado)
                            {
                                Alert.Show("Es NECESARIO RESOMETER para que el flujo sea correcto, debido a la modificación de los aprobadores");
                                agAmecInfo.CambiarEstadoAmec("35", miAmec.idamecs.ToString(), datosUsuarioAMEC.IdPeticionario.ToString(), idmanager.ToString());
                                string comentario =string.Format("Se ha cambiado el estado del amec al INSERTAR un nuevo aprobador: {0} {1} {2}",manager.Nombre, manager.Apellido1, manager.Apellido2);
                                agComAmec.InsertComentarioAMEC(miAmec.idamecs.ToString(), datosUsuarioAMEC.IdPeticionario.ToString(), comentario);
                                btnCancelar.Visible = false;
                                btnVolver.Visible = false;
                                btnGuardar.Visible = false;
                                lvComentarios.DataBind();
                                AgenteAmecInfo agenteAMEC = new AgenteAmecInfo();
                                lbEstadoAMEC.Text = agenteAMEC.ObtenerNombreEstado(uint.Parse(EstadosAmec.Borrador.GetHashCode().ToString()));
                            }
                        }
                        else
                        {
                            //AMEC no insertado. Guardamos valores del amec e insertamos amec
                            GuardarNuevoAmec();
                            AniadirAprobador(manager);
                        }
                    }
                }
                else Alert.Show("Solo puede añadir aprobadores el solicitante del AMEC a un amec sometido");
            }
            catch (Exception ex)
            {
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
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
                HabilitaBotones();
                //Session.Add("estadoAMEC", estadoAMEC);

            }
            AgenteComentariosAMEC agComentario = new AgenteComentariosAMEC();
            if (txtComentario.Text != "")
            {
                int retorn = agComentario.InsertComentarioAMEC(miAmec.idamecs.ToString(), datosUsuarioAMEC.IdPeticionario.ToString(), txtComentario.Text);
                EnviaMailPublicarComentario();
                btnAprobarConCondicion.OnClientClick = "return confirm('Has Introducido un Comentario. ¿Desea Aprobar con la Condición del Comentario?');";
                lvComentarios.DataBind();
                txtComentario.Text = "";
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //Guardando = true;
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            try
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
                                bool YaSeHaEnviadoACasosClinicos =
                                    agAmecInf.ComprobarSiSeEnvioACasosClinicos(miAmec.idamecs);
                                if (!YaSeHaEnviadoAFarma && miAmec.farmaindustria == true)
                                {
                                    Alert.Show("Se ha enviado el correo de FarmaIndustria");
                                    YaSeHaEnviadoAFarma = true;
                                    EnviaMailFarmaIndustria(false);
                                }
                                if (!YaSeHaEnviadoACasosClinicos && miAmec.casosclinicos == true)
                                {
                                    EnviaMailCasosClinicos();
                                    YaSeHaEnviadoACasosClinicos = true;
                                    Alert.Show("Se ha enviado el correo de CasosClinicos");
                                }
                                this.btnSometer.OnClientClick = "return ConfirmarResometer('" + YaSeHaEnviadoAFarma +
                                                                "','" + YaSeHaEnviadoACasosClinicos + "',')";
                            }
                            bool cambioAgencia = false;
                            ActualizarAmec(out cambioAgencia);

                            if (cambioAgencia && miAmec.idestado != 5 && miAmec.farmaindustria == true)
                            {
                                EnviaMailFarmaIndustria(cambioAgencia);
                            }
                        }
                        else
                        {
                            GuardarNuevoAmec();

                        }
                        //Aprobador por defecto
                        //if (LstManagers.Count < 1) AniadirAprobadorPorDefecto();
                        lvPrograma.DataBind();
                        Alert.Show(string.Format("La información del AMEC: {0} ha sido guardada correctamente.", miAmec.idamecs));
                    }

                    else
                        Alert.Show("No se puede Guardar el Amec. Es Obligatorio Resometer debido a la modificación del Importe.");

                }
            }
            catch (Exception ex)
            {
                //TODO:Implementar log
            }
        }

        protected void btnAprobarConCondicion_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                estadoAmecAnterior = Convert.ToInt32(miAmec.idestado);
                AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
                if (agAmecInf.EstaGuardadoAmec(miAmec.idamecs))
                {
                    //AMEC ya insertado solo modificamos(UPDATE) el amec.
                    bool cambioAgencia = false;
                    ActualizarAmec(out cambioAgencia);
                }
                else
                {
                    //AMEC no insertado. Guardamos valores del amec e insertamos amec y la unidad organizativa por defecto.
                    GuardarNuevoAmec();
                }
                GestorPermisos gestor = new GestorPermisos();
                IPermiso permisos = gestor.GetPermisos(miAmec.idestado.GetHashCode());
                bool bienAprobado = permisos.Aprobar(miAmec.idamecs, datosUsuarioAMEC.IdPeticionario, true);
                if (bienAprobado)
                {
                    ObtenerAMEC(miAmec.idamecs);
                    permisos = gestor.GetPermisos(miAmec.idestado.GetHashCode());
                    if (miAmec.idestado == 1)
                    {
                        EnviaMailAprobacionFinal();
                    }
                    else
                    {
                        if (estadoAmecAnterior != miAmec.idestado)
                        {
                            List<string> collectCorreo = permisos.ObtenerListaDestinatarios(miAmec.idamecs, datosUsuarioAMEC.IdPeticionario);
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
            }
            catch (Exception ex)
            {
                //TODO:Implementar log
            }
        }

        protected void btnAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                estadoAmecAnterior = Convert.ToInt32(miAmec.idestado);
                AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
                if (agAmecInf.EstaGuardadoAmec(miAmec.idamecs))
                {
                    //AMEC ya insertado solo modificamos(UPDATE) el amec.
                    bool cambioAgencia = false;
                    ActualizarAmec(out cambioAgencia);
                }
                else
                {
                    //AMEC no insertado. Guardamos valores del amec e insertamos amec y la unidad organizativa por defecto.
                    GuardarNuevoAmec();
                }
                //estadoAMEC = new RepositorioFlujo(miAmec.idamecs, (Int32)datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
                GestorPermisos gestor = new GestorPermisos();
                IPermiso permisos = gestor.GetPermisos(miAmec.idestado.GetHashCode());

                if (permisos.GetType() == typeof(PermisoSupJer))
                {
                    string checkAprobadorNegocio = string.Format(
                "   select " +
                "       COUNT(*) " +
                "   from histasocamecs hist " +
                "   inner " +
                "   join agency_user_approval_structure aprob on " +
                "   hist.idaprobador = aprob.IdPeticionario " +
                "   where " +
                "       aprob.IdPeticionarioManager = {0} and " +
                "       idamecs = {1} and " +
                "       idestado = {2} and " +
                "       idnivelaprobacion = {3} AND " +
                "    " +
                "       hist.fechacreacion >= (SELECT TOP 1 fechacreacion FROM histasocamecs WHERE idamecs = {4}  AND idestado = {5} ORDER BY fechacreacion DESC) and " +
                "       exists " +
                "       ( " +
                        "   select " +
                "               * " +
                        "   from histasocamecs hista " +
                "           where " +
                            "   hista.idamecs = hist.idamecs and " +
                "               hista.idcreadopor = hist.idaprobador and " +
                "               hista.idestado = {6} and " +
                "               hista.idnivelaprobacion = {7} AND " +
                "               hista.fechacreacion > (SELECT TOP 1 fechacreacion FROM histasocamecs WHERE idamecs = {8}  AND idestado = {9} ORDER BY fechacreacion DESC) AND " +
                "               not exists " +
                "               (" +
                "                  select " +
                "                       * " +
                "                  from histasocamecs histaso " +
                "                  where " +
                "                      histaso.idamecs = hista.idamecs and " +
                "                      histaso.idcreadopor = {10} and " +
                "                      histaso.idestado = {11} and " +
                "                      histaso.idnivelaprobacion = {12} and " +
                "                      histaso.fechacreacion > (SELECT TOP 1 fechacreacion FROM histasocamecs WHERE idamecs = {13}  AND idestado = {14} ORDER BY fechacreacion DESC) " +
                "               )" +
                "   	) ", datosUsuarioAMEC.IdPeticionario, miAmec.idamecs, 8, 3, miAmec.idamecs, 29, 21, 3, miAmec.idamecs, 29, datosUsuarioAMEC.IdPeticionario, 22, 4, miAmec.idamecs, 29);

                    int aprobadorNegocio = int.Parse(Quodem.Sql.SqlServerClient.GetValue(checkAprobadorNegocio));
                    if (aprobadorNegocio > 0)
                    {
                        permisos = gestor.GetPermisos(EstadosAmec.PendienteNegocio.GetHashCode());
                    }
                }

                bool bienAprobado = permisos.Aprobar(miAmec.idamecs, datosUsuarioAMEC.IdPeticionario, false);
                if (bienAprobado)
                {
                    //Si se ha aprobado el Amec se enviarà un correo al Solicitante del Amec
                    //IDEstado Aprobado = 1
                    ObtenerAMEC(miAmec.idamecs);
                    permisos = gestor.GetPermisos(miAmec.idestado.GetHashCode());
                    if (miAmec.idestado == 1)
                    {
                        EnviaMailAprobacionFinal();
                    }
                    else
                    {
                        AgenteUsuarios agenteUs = new AgenteUsuarios();
                        if (estadoAmecAnterior != miAmec.idestado)
                        {
                            var peticionariosDestino = new List<string>();

                            var peticionariosTemp = permisos.ObtenerListaDestinatarios(miAmec.idamecs,datosUsuarioAMEC.IdPeticionario);

                            RepositorioAprobadorAMEC aprobador = new RepositorioAprobadorAMEC();
                            foreach (var peticionario in peticionariosTemp)
                            {
                                DDatosPersonalesUsuario usuarioLogged = agenteUs.ObtenerDatosPersonalesPorIDPeticionario(datosUsuarioAMEC.IdPeticionario.ToString());
                                DDatosPersonalesUsuario usuario = agenteUs.ObtenerDatosPersonalesPorIDPeticionario(peticionario);
                                if (!aprobador.HaRealizadoAprobacion(miAmec.idamecs, usuario.IdPeticionario) || usuario.IdPeticionario == usuarioLogged.IdPeticionarioManager)
                                {
                                    peticionariosDestino.Add(usuario.IdPeticionario.ToString());
                                }
                            }

                            EnviaMailPendienteAprobar(peticionariosDestino);
                        }
                        //Si no hay cambio de estado del amec notificamos solo al superior jerárquico del usuario que ha realizado la aprobación
                        else if (miAmec.idestado == EstadosAmec.PendienteSupJerarquico.GetHashCode())
                        {
                            DDatosPersonalesUsuario usuario = agenteUs.ObtenerDatosPersonalesPorIDPeticionario(datosUsuarioAMEC.IdPeticionario.ToString());
                            IPermiso permisosNegocio = gestor.GetPermisos(miAmec.idestado.GetHashCode());

                            if (usuario.IdPeticionarioManager != null && permisosNegocio.PuedoAprobarYRechazar(miAmec.idamecs, usuario.IdPeticionarioManager.Value))
                            {
                                EnviaMailPendienteAprobar(new List<string>(){usuario.IdPeticionarioManager.Value.ToString()});    
                            }
                        }
                    }

                    string sNavegacion = "ListadoAMECs.aspx";
                    Alert.Show(string.Format("Se ha Aprobado Correctamente el AMEC con Número: {0}", miAmec.idamecs), sNavegacion);

                }
                else Alert.Show("No se ha podido Aprobar el AMEC debido a problemas internos");
            }
            catch (Exception ex)
            {
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
            }

        }

        protected void btnRechazar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
                if (agAmecInf.EstaGuardadoAmec(miAmec.idamecs))
                {
                    //AMEC ya insertado solo modificamos(UPDATE) el amec.
                    bool cambioAgencia = false;
                    ActualizarAmec(out cambioAgencia);
                }
                else
                {
                    //AMEC no insertado. Guardamos valores del amec e insertamos amec y la unidad organizativa por defecto.
                    GuardarNuevoAmec();
                }

                if (!agAMEC.EsCancelable())
                    Alert.Show("No se puede Rechazar el Amec porque tiene reservas asociadas en curso");
                else
                {
                    GestorPermisos gestor = new GestorPermisos();
                    IPermiso permisos = gestor.GetPermisos(miAmec.idestado.GetHashCode());
                    bool bienRechazado = permisos.Rechazar(miAmec.idamecs, datosUsuarioAMEC.IdPeticionario);
                    if (bienRechazado)
                    {
                        EnviaMailRechazado();
                        string sNavegacion = "ListadoAMECs.aspx";
                        Alert.Show(string.Format("Se ha Rechazado Correctamente el AMEC con Número: {0}", miAmec.idamecs), sNavegacion);
                    }
                    else Alert.Show("No se ha podido Rechazar el AMEC debido a problemas internos");
                }
            }
            catch (Exception ex)
            {
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
            }

        }

        protected void btnVolver_Click(object sender, EventArgs e) { }

        protected void btnSometer_Click(object sender, ImageClickEventArgs e)
        {
            bool emailsent = false;
            Sometiendo = true;
            try
            {
                AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
                if (ValidarCamposObligatorios(miAmec))
                {
                    if (ValidarCampos() && ComprobarLibros())
                    {
                        bool cambioAgencia = false;
                        if (agAmecInf.EstaGuardadoAmec(miAmec.idamecs) == true)
                        {
                            //AMEC ya insertado solo modificamos(UPDATE) el amec.
                            ActualizarAmec(out cambioAgencia);
                        }
                        else
                        {
                            //AMEC no insertado. Guardamos valores del amec e insertamos amec y la unidad organizativa por defecto.
                            GuardarNuevoAmec();
                        }

                        if (LstManagers.Count < 1)
                        {
                            AniadirAprobadorPorDefecto(sender, e);
                        }
                        else
                        {
                            //Añado siempre el manager del solicitante si no está ya añadido
                            AgentePeticionarioManager agPetManager = new AgentePeticionarioManager();
                            DPeticionarioManager manager = agPetManager.ObtenerManagerDePeticionario(datosUsuarioSolicitante.IdPeticionario);
                            if (LstManagers.ToList().Find(x => x.IdPeticionario == manager.IdPeticionario) == null)
                            {
                                AniadirAprobador(manager);
                                LstManagers.Add(manager);
                                listManagers.DataSource = LstManagers;
                                listManagers.DataBind();
                            }
                        }

                        CondicionesFlujo condiciones = new CondicionesFlujo(miAmec.idcreadopor, miAmec.idsolicitante.Value)
                        {
                            ConParaguas = chkParaguas.Checked,
                            EsDifSolicitante = false, //datosUsuarioSolicitante.IdPeticionario != miAmec.idcreadopor,
                            PreNegocio = false, //rblPreaprobNegocio.SelectedValue == "1",
                            PreLegal = rblPreaprovLegal_Comp.SelectedValue == "1",
                            PreMedico = rblPreaprobMedico.SelectedValue == "1",
                            EstaSometido = miAmec.idestado != 5
                        };

                        GestorFlujo gestorFlujo = new GestorFlujo();
                        IFlujo flujo = gestorFlujo.ObtenerFlujo(condiciones);
                        bool bienSometido = flujo.SometerAmec(condiciones, miAmec.idamecs, miAmec.idsolicitante.Value ,miAmec.idcreadopor);

                        if (bienSometido)
                        {
                            //Reestablecemos los permisos de los aprobadores por si se resometió un amec pasados mas de 7 dias sin que un supJerarquico
                            //aprobase el amec
                            AgenteAprobadorAmec agAprobador = new AgenteAprobadorAmec();
                            agAprobador.ReestablecerAprobadores(miAmec.idamecs);

                            ObtenerAMEC(miAmec.idamecs);
                            if (miAmec.farmaindustria == true && Request.Form["TornarEnviarFarma"] == "true")
                            {
                                EnviaMailFarmaIndustria(cambioAgencia);
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
                                List<string> collectCorreo = new List<string>();
                                //Permisos superior Jerárquico
                                List<uint> lstEstadosSuperiorJerarquico = new List<uint>() { 8, 41, 45 };
                                if(lstEstadosSuperiorJerarquico.Contains(miAmec.idestado))
                                {
                                    foreach (var manager in LstManagers)
                                    {
                                        if (agAprobador.ObtenerPermisosSupJerAmec(miAmec.idamecs, manager.IdPeticionario))
                                        {
                                            collectCorreo.Add(manager.IdPeticionario.ToString());
                                        }
                                    }
                                }

                                //Permisos negocio
                                List<uint> lstEstadosNegocio = new List<uint>() { 10, 39, 43 };
                                if (lstEstadosNegocio.Contains(miAmec.idestado))
                                {
                                    foreach (var manager in LstManagers)
                                    {
                                        if (agAprobador.ObtenerPermisosNegocio(miAmec.idamecs, manager.IdPeticionario))
                                        {
                                            collectCorreo.Add(manager.IdPeticionario.ToString());
                                        }
                                    }
                                }

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
                                //Response.Redirect(string.Format(@"DetalleAMEC.aspx?idamec={0}", miAmec.idamecs));
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
            catch (Exception ex)
            {
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
            }
        }

        protected void btnCancelar_Click(object sender, ImageClickEventArgs e)
        {

            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            try
            {
                if (agAmecInf.EstaGuardadoAmec(miAmec.idamecs))
                {
                    //AMEC ya insertado solo modificamos(UPDATE) el amec.
                    bool cambioAgencia = false;
                    ActualizarAmec(out cambioAgencia);
                }
                else
                {
                    //AMEC no insertado. Guardamos valores del amec e insertamos amec y la unidad organizativa por defecto.
                    GuardarNuevoAmec();
                }
                //Jose Laguna 10-05-2012 10:00 cancelamos la forma de averiguar si tiene reservas asociadas o no porque no se relaciona con el expediente si no, con las reservas.
                //int NumExpedienteAsoc = agExp.NumeroExpedientesPorAmec(miAmec.idamecs);
                agAMEC.nIDAmec = miAmec.idamecs;
                if (!agAMEC.EsCancelable()) Alert.Show("No se puede Cancelar el Amec porque tiene reservas asociadas en curso");
                else
                {
                    AgenteUsuarios agUsuario = new AgenteUsuarios();
                    DDatosPersonalesUsuario usuario = agUsuario.ObtenerDatosPersonalesPorIDPeticionario(datosUsuarioAMEC.IdPeticionario.ToString());
                    agAmecInf.InsertarHistorialAmec(EstadosAmec.Cancelado.GetHashCode(), miAmec.idamecs, usuario, NivelesAprobacion.No.GetHashCode(), null);
                    agAmecInf.CambiarEstadoAmec(EstadosAmec.Cancelado.GetHashCode().ToString(), miAmec.idamecs.ToString(), NivelesAprobacion.No.GetHashCode());
                    EnviaMailCancelado();
                    var sNavegacion = sURL.Contains("NuevoExpedientePasoA.aspx") ? sURL : "ListadoAMECs.aspx";
                    Alert.Show(string.Format("Se ha Cancelado el AMEC con Número: {0}", miAmec.idamecs), sNavegacion);
                    //si es un amec cancelado no podrás utilizarlo para crear un Expediente No se tiene en cuenta
                }
            }
            catch (Exception ex)
            {
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
            }

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
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
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
                    if (ddlExpedientesAmec.Visible)
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
                    //AMEC no insertado. Guardamos valores del amec e insertamos amec y la unidad organizativa por defecto.
                    if (!agAmecInfo.EstaGuardadoAmec(miAmec.idamecs))
                        GuardarNuevoAmec();
                    else
                    {
                        bool cambioAgencia = false;
                        ActualizarAmec(out cambioAgencia);
                    }
                    lvPrograma.DataBind();
                }
                else Alert.Show("No se ha subido bien el programa");
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

        protected void imgAprobadorEliminar_Command(object sender, CommandEventArgs e)
        {
            //if (!chkParaguas.Checked) return;
            try
            {
                //Comprobar si el usuario es el Solicitante
                if (datosUsuarioAMEC.IdPeticionario == datosUsuarioSolicitante.IdPeticionario || miAmec.idestado == 5)
                {
                    int idmanager = Int32.Parse(e.CommandArgument.ToString());
                    DPeticionarioManager manager = LstManagers.FirstOrDefault(x => x.IdPeticionario == idmanager);
                    LstManagers.Remove(manager);
                    listManagers.DataSource = LstManagers;

                    AgenteAprobadorAmec agAprobador = new AgenteAprobadorAmec();
                    AgenteComentariosAMEC agComAmec = new AgenteComentariosAMEC();
                    AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
                    if (LstManagers.Count < 1)
                    {
                        Alert.Show("Si no se selecciona ningún aprobador se insertará el aprobador por defecto.");
                       
                    }
                    if (manager != null)
                    {
                        //Borramos en la base de datos
                        agAprobador.BorrarAprobador(miAmec.idamecs.ToString(), manager.IdPeticionario.ToString());
                        if (miAmec.idestado != 5)
                        {
                            Alert.Show("Es NECESARIO RESOMETER para que el flujo sea correcto, debido a la modificación de los aprobadores");
                            agAmecInfo.CambiarEstadoAmec("35", miAmec.idamecs.ToString(), datosUsuarioAMEC.IdPeticionario.ToString(), idmanager.ToString());
                            string comentario =
                                string.Format("Se ha cambiado el estado del amec al BORRAR un aprobador: {0} {1} {2}",
                                    manager.Nombre, manager.Apellido1, manager.Apellido2);
                            agComAmec.InsertComentarioAMEC(miAmec.idamecs.ToString(), datosUsuarioAMEC.IdPeticionario.ToString(), comentario);
                            btnCancelar.Visible = false;
                            btnVolver.Visible = false;
                            btnGuardar.Visible = false;
                            lvComentarios.DataBind();
                            AgenteAmecInfo agenteAMEC = new AgenteAmecInfo();
                            lbEstadoAMEC.Text = agenteAMEC.ObtenerNombreEstado(35);


                        }
                    }
                }
                else Alert.Show("Solo Puede Eliminar aprobadores el Solicitante del Amec");

                listManagers.DataBind();
            }
            catch (Exception ex)
            {
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
            }

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
                        FileInfo infofile = new FileInfo(savePathDocumentacion);

                        string extensionDocumento = System.IO.Path.GetExtension(savePathDocumentacion);

                        benGuardat = agAmecInfo.GuardarDocumentacion(miAmec.idamecs, datosUsuarioAMEC.IdPeticionario, infofile.Name, savePathDocumentacion, txtComentarioDoc.Text.Trim(), chkemail.Checked, extensionDocumento, Convert.ToInt32(ddlCategoriaDocumento.SelectedValue));
                        if (benGuardat == 1)
                        {
                            lvDocumentacion.DataBind();
                            txtComentarioDoc.Text = "";
                            if ((miAmec.farmaindustria == true || rblFarmaindustria.SelectedValue == "1") && chkemail.Checked && Request.Form["TornarEnviarFarma"] == "true" && miAmec.idestado != 5)
                            {
                                EnviaMailFarmaIndustria(false);
                            }

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
                            EnviaMailFarmaIndustria(false);
                        }
                        //else if (miAmec.farmaindustria == false && Request.Form["TornarEnviarFarma"] == "true") Alert.Show("No se ha enviado el correo porque tiene bien la documentación");

                    }
                    else Alert.Show("No se ha guardado bien la documentación");
                }

            }
            else Alert.Show("No se ha subido bien la documentación");
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
                bool cambioAgencia = false;
                ActualizarAmec(out cambioAgencia);
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
                agComentarios.EliminarComentarioAMEC(idcomentario);
                txtComentario.Text = "";
                lvComentarios.DataBind();
            }
            else Alert.Show("No se puede eliminar el Comentario");
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

        #region Eventos

        protected void chkParaguas_OnCheckedChanged(object sender, EventArgs e)
        {
            //btnAnadirAprobador.Enabled = chkParaguas.Checked;
            if (!chkParaguas.Checked && LstManagers.Count > 0)
            {
                Alert.Show("Por favor, elmine todos los aprobadores de la lista antes de cambiar el tipo de aprobación.");
                chkParaguas.Checked = true;
            }
        }

        protected void lvActividadAsigAmec_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Codi del lvActividadesAsigAmec
            lvActividadAsigAmec.DataBind();

        }

        protected void lvActivid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvActivid.SelectedValue != null) SeleccionaActividad(lvActivid.SelectedValue.ToString(), null);
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

            if (Page.IsPostBack) return;
            e.Arguments.MaximumRows = 20;
            e.Arguments.StartRowIndex = 0;
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
            AgentePeticionarioManager agPeticionarioManager = new AgentePeticionarioManager();
            datosPeticionarioManager = agPeticionarioManager.ObtenerPeticionarioManager(Convert.ToInt32(ddlSolicitante.SelectedValue));
            if (datosPeticionarioManager != null)
            {
                txtDepartamento.Text = datosPeticionarioManager.departament;
                txtDistrito.Text = datosPeticionarioManager.district;
                txtFuerzaVentas.Text = datosPeticionarioManager.saleforce;
            }
            /* cuando el combo de Solicitante se seleccione entonces se carga el campo CARGO (del solicitante). */
            ////OBTENEMOS LOS DATOS PERSONALES DEL SOLICITANTE/RESPONSABLE del usuario.
            AgenteUsuarios agenteSolicitante = new AgenteUsuarios();
            datosUsuarioSolicitante = agenteSolicitante.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());

            miAmec.idcargo = datosUsuarioSolicitante.IdCargo;
            miAmec.idposition = datosUsuarioSolicitante.IdPosition;
            AgenteAMEC agSol_AMEC = new AgenteAMEC(datosUsuarioSolicitante.IdPeticionario);
            txtCargo.Text = agSol_AMEC.ObtenerPosition();

            ////Cambio nwein
            miAmec.nwein = datosUsuarioSolicitante.wein;
            txtWein.Text = miAmec.nwein;

            ////TODO s'ha de recollir les dades del Formulari
            Session.Add("NuevoAMEC", miAmec);
            Session.Add("DatosSolicitante", datosUsuarioSolicitante);
            btnSometer.Visible = (miAmec.idsolicitante == datosUsuarioAMEC.IdPeticionario);// || miAmec.idestado == 5;

        }

        public void ddlActividad_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarPorTipoActividad(ddlActividad.Text, false);
            HabilitaBotones();
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
                string link = string.Format("{0}DetalleAMEC.aspx?idamec={1}", RutaLinkMail, miAmec.idamecs);
                DEmpresaConf datosEmpresa = agMaes.ObtenerEmpresaConf(1);
                DDatosPersonalesUsuario Creador = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idcreadopor.ToString());
                DDatosPersonalesUsuario Solicitante = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());
                List<string> lstMailCCHcp = Quodem.Utility.StringUtil.GetValoresList(ConfigUtil.GetAppSetting(Constantes.AppParams.MailCCHcp));
                string estado = agAMECinfo.ObtenerNombreEstado(miAmec.idestado);
                AgenteUnidadesOrgAMEC agUnid = new AgenteUnidadesOrgAMEC();
                string unidad = agUnid.ObtenerNombreUnidadxId(Convert.ToInt32(dSolicitante.idunidad));
                string area = agUnid.ObtenerNombreAreaxId(Convert.ToInt32(dSolicitante.Idarea));

                if (collectCorreo.Count != 0 || miAmec.idnivelaprobacion == 2 || miAmec.idnivelaprobacion == 6)
                {
                    foreach (string correo in collectCorreo)
                    {
                        datosUsuarioCorreo = agUsu.ObtenerDatosPersonalesPorIDPeticionario(correo);
                        try
                        {
                            if (!string.IsNullOrEmpty(datosUsuarioCorreo.Email))
                            {
                                message.To.Add(new MailAddress(datosUsuarioCorreo.Email));
                            }
                        }
                        catch (Exception ex)
                        {
                            Global.SendApplicationError(ex, Request, Session, GetType().Name);
                            Alert.Show(
                                "Se ha producido un error enviando el mail de Notificación Pendiente de Aprobar. Por favor revise la dirección de envío en la configuración del sistema.",
                                null);
                            GuardarMail = agAMECinfo.GuardaLogMail(miAmec.idamecs, "MailAmecPendienteAprobar",
                                message.To.ToString(), message.Subject, message.Body, null, false,
                                "Error añadiendo la dirección de Correo Electrónico");
                            return;
                        }

                    }
                    try
                    {
                        if (miAmec.idnivelaprobacion == 2)
                            message.To.Add(new MailAddress(Web.Variables.EmailMedico));
                        if (miAmec.idnivelaprobacion == 6)
                            message.To.Add(new MailAddress(Web.Variables.EmailLegal));

                        if (MailCopia.Contains('@'))
                        {
                            foreach (string s in MailCopia.Split(';'))
                            {
                                message.To.Add(new MailAddress(s));
                            }
                        }
                            
                        if (MailCopiaProgramador.Contains('@'))
                            foreach (string s in MailCopiaProgramador.Split(';'))
                            {
                                message.To.Add(new MailAddress(s));
                            }

                        if (lstMailCCHcp.Contains(Solicitante.Email))
                        {
                            message.CC.Add(new MailAddress(Solicitante.Email));
                        }
                    }
                    catch (Exception ex)
                    {
                        Global.SendApplicationError(ex, Request, Session, GetType().Name);
                        Alert.Show(
                            "Se ha producido un error enviando el mail de Notificación Pendiente de Aprobar. Por favor revise la dirección de envío en la configuración del sistema.",
                            null);
                        GuardarMail = agAMECinfo.GuardaLogMail(miAmec.idamecs, "MailAmecPendienteAprobar",
                            message.To.ToString(), message.Subject, message.Body, null, false,
                            "Error añadiendo la dirección de Correo Electrónico");
                        return;
                    }

                    string importe = string.Format("{0:C}", miAmec.importegasto);
                    message.Subject =
                        string.Format(
                            "Amec {0} pendiente de Aprobar: Importe {1}, Número de Ponentes {2} y Número de Personas {3} ",
                            miAmec.idamecs, importe, miAmec.ponentespatrocinados, miAmec.participantesmsd);
                    if (miAmec.paraguas == true)
                    {
                        string listUnidadesOrg = ObtenerUnidadesOrganizativasParaCorreo();
                        message.Body =
                            string.Format(
                                "El Amec número {0} para la actividad {1} está pendiente de tu aprobación. \r\n\r\nPara aprobar y consultar más detalles haz click en el link adjunto: {3}",
                                miAmec.idamecs, miAmec.descripcion, estado, link, listUnidadesOrg);
                    }
                    else
                    {

                        message.Body =
                            string.Format(
                                "El Amec número {0} para la actividad {1} está pendiente de tu aprobación. \r\n\r\nPara aprobar y consultar más detalles haz click en el link adjunto: {3}",
                                miAmec.idamecs, miAmec.descripcion, estado, link, unidad, area);
                    }

                    Mail.EnviaMail(message);
                    GuardarMail = agAMECinfo.GuardaLogMail(miAmec.idamecs, "MailAmecPendienteAprobar",
                        message.To.ToString(), message.Subject, message.Body, null, true, null);

                    System.Net.Mail.MailMessage messageSolicitante = new System.Net.Mail.MailMessage();
                    if (MailCopia.Contains('@'))
                    {
                        foreach (string s in MailCopia.Split(';'))
                        {
                            messageSolicitante.To.Add(new MailAddress(s));
                        }
                    }
                        
                    if (MailCopiaProgramador.Contains('@'))
                        foreach (string s in MailCopiaProgramador.Split(';'))
                        {
                            messageSolicitante.To.Add(new MailAddress(s));
                        }
                    messageSolicitante.To.Add(new MailAddress(Creador.Email));
                    if (Solicitante.Email != Creador.Email)
                    {
                        messageSolicitante.To.Add(new MailAddress(Solicitante.Email));
                    }
                    messageSolicitante.Subject = string.Format("Amec {0} pendiente de Aprobar: Importe {1}, Número de Ponentes {2} y Número de Personas {3} ", miAmec.idamecs, importe, miAmec.ponentespatrocinados, miAmec.participantesmsd);
                    messageSolicitante.Body = string.Format("El Amec número {0} para la actividad {1} está pendiente de aprobación. \r\n\r\nPara consultar más detalles haz click en el link adjunto: {2}", miAmec.idamecs, miAmec.descripcion, link);
                    Mail.EnviaMail(messageSolicitante);
                    GuardarMail = agAMECinfo.GuardaLogMail(miAmec.idamecs, "MailAmecPendienteAprobar", messageSolicitante.To.ToString(), messageSolicitante.Subject, messageSolicitante.Body, null, true, null);
                }
                else
                {
                    Alert.Show("No se han encontrado destinatarios para el envío del correo debido a que no exiten  superiores jerárquicos de los aprobadores.", null);
                }


            }
            catch (Exception ex)
            {
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
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

                string link = string.Format("{0}DetalleAMEC.aspx?idamec={1}", RutaLinkMail, miAmec.idamecs);
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
                catch(Exception ex)
                {
                    Global.SendApplicationError(ex, Request, Session, GetType().Name);
                    Alert.Show("Se ha producido un error enviando el mail de Cancelado del AMEC. Por favor revise la dirección de correo del Solicitante.", null);
                    GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailAmecCancelado", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico");

                    return;
                }
                string importe = string.Format("{0:C}", miAmec.importegasto);
                message.Subject = string.Format("AMEC {0} CANCELADO, Importe {1}, Número de Ponentes {2} y Número de Personas {3}", miAmec.idamecs, importe, miAmec.ponentespatrocinados, miAmec.participantesmsd);
                if (miAmec.paraguas == true)
                {
                    string listUnidadesOrg = ObtenerUnidadesOrganizativasParaCorreo();
                    message.Body = string.Format("Ha sido CANCELADO el Amec número {0} para la actividad {1}\r\n\r\nHaz click en el link adjunto para ver más detalles: {3}", miAmec.idamecs, miAmec.descripcion, listUnidadesOrg, link);
                }
                else
                {
                    message.Body = string.Format("El Amec número {0} para la actividad {1}, ha sido CANCELADO. \r\n\r\nHaz click en el link adjunto para ver más detalles: {4}", miAmec.idamecs, miAmec.descripcion, unidad, area, link);
                }
                Mail.EnviaMail(message);
                GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailAmecCancelado", message.To.ToString(), message.Subject, message.Body, null, true, null);


            }
            catch (Exception ex)
            {
                Alert.Show("Se ha producido un error enviando el mail de Cancelado del AMEC", null);
                GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailAmecCancelado", message.To.ToString(), message.Subject, message.Body, null, false, "Error enviando el Correo");
                Logger.Logger.PrintError(this.GetType().Name, "amecNotificaciones", ex.Message, ex);
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
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
                
                string link = string.Format("{0}DetalleAMEC.aspx?idamec={1}", RutaLinkMail, miAmec.idamecs);
                DDatosPersonalesUsuario dSolicitante = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());
                DDatosPersonalesUsuario dCreador = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idcreadopor.ToString());
                
                try
                {
                    //Buscamos todos los usuarios que han intervenido en el flujo de aprobación para informarles que se ha aprobado el amec. Los usuarios Dep Médico no se les envia confirmación
                    AgenteAprobadorAmec agAprobador = new AgenteAprobadorAmec();
                    List<string> emailList = agAprobador.ObtenerCorreosTodosParticipantes(miAmec.idamecs);

                    foreach (var email in emailList)
                    {
                        message.To.Add(email);
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
                }
                catch(Exception ex)
                {
                    Global.SendApplicationError(ex, Request, Session, GetType().Name);
                    Alert.Show("Se ha producido un error enviando el mail de Rechazo del AMEC. Por favor revise la dirección de correo del Solicitante.", null);
                    GuardarMail = agAmecInfo.GuardaLogMail(miAmec.idamecs, "MailAmecRechazado", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico");
                    return;
                }
                string importe = string.Format("{0:C}", miAmec.importegasto);
                message.Subject = string.Format("AMEC {0} DENEGADO, Importe {1}, Número de Ponentes {2} y Número de Personas {3}", miAmec.idamecs, importe, miAmec.ponentespatrocinados, miAmec.participantesmsd);
                if (miAmec.paraguas == true)
                {
                    message.Body = string.Format("Ha sido DENEGADO el Amec número {0} para la actividad {1} \r\n\r\nHaz click en el link adjunto para ver más detalles: {2}", miAmec.idamecs, miAmec.descripcion, link);
                }
                else
                {
                    message.Body = string.Format("El Amec número {0} para la actividad {1} , ha sido DENEGADO. \r\n\r\nHaz click en el link adjunto para ver más detalles: {2}", miAmec.idamecs, miAmec.descripcion, link);
                }
                Mail.EnviaMail(message);
                agAmecInfo.GuardaLogMail(miAmec.idamecs, "MailAmecRechazado", message.To.ToString(), message.Subject, message.Body, null, true, null);

            }
            catch (Exception ex)
            {
                Alert.Show("Se ha producido un error enviando el mail de Rechazo del AMEC", null);
                agAmecInfo.GuardaLogMail(miAmec.idamecs, "MailAmecRechazado", message.To.ToString(), message.Subject, message.Body, null, false, "Error enviando el Correo");
                Logger.Logger.PrintError(this.GetType().Name, "amecNotificaciones", ex.Message, ex);
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
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
                string link = string.Format("{0}DetalleAMEC.aspx?idamec={1}", RutaLinkMail, miAmec.idamecs);
                DDatosPersonalesUsuario dSolicitante = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());
                DDatosPersonalesUsuario dCreado = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idcreadopor.ToString());
                AgenteUnidadesOrgAMEC agUnid = new AgenteUnidadesOrgAMEC();

                string estado = agAMECinfo.ObtenerNombreEstado(miAmec.idestado);
                string unidad = agUnid.ObtenerNombreUnidadxId(Convert.ToInt32(dSolicitante.idunidad));
                string area = agUnid.ObtenerNombreAreaxId(Convert.ToInt32(dSolicitante.Idarea));

                try
                {
                    AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
                    if (miAmec.preaprobadaneg == true)
                    {

                        PermisoSupJer perSupJer = new PermisoSupJer(new AgenteAprobadorAmec());
                        List<string> collectUsuariosIntervenidoFlujoAprobacion = perSupJer.ObtenerListaDestinatarios(miAmec.idamecs, miAmec.idsolicitante.Value);
                        foreach (string correo in collectUsuariosIntervenidoFlujoAprobacion)
                        {
                            datosUsuarioCorreo = agUsu.ObtenerDatosPersonalesPorIDPeticionario(correo);
                            try
                            {
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
                catch (Exception ex)
                {
                    Alert.Show("Se ha producido un error enviando el mail de Notificación de Aprobado Condicionado del AMEC. Por favor revise la dirección de correo del Solicitante.", null);
                    GuardarMail = agAMECinfo.GuardaLogMail(miAmec.idamecs, "MailAmecAprobacionCondicionada", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico");
                    Global.SendApplicationError(ex, Request, Session, GetType().Name);
                    return;
                }
                string importe = string.Format("{0:C}", miAmec.importegasto);
                message.Subject = string.Format("AMEC {0} Aprobado con Condición: Importe {1}, Número de Ponentes {2} y Número de Personas {3}", miAmec.idamecs, importe, miAmec.ponentespatrocinados, miAmec.participantesmsd);
                if (miAmec.paraguas == true)
                {
                    string listUnidadesOrg = ObtenerUnidadesOrganizativasParaCorreo();
                    message.Body = string.Format("Ha sido Aprobado con Condición el Amec número {0} para la actividad {1}\r\n\r\nPor favor, revisa las condiciones antes de implementar la actividad. \r\n\r\nHaz click en el link adjunto para ver más detalles: {3}", miAmec.idamecs, miAmec.descripcion, listUnidadesOrg, link, estado);
                }
                else
                {
                    message.Body = string.Format("El Amec número {0} para la actividad {1} ha sido Aprobado con Condición y nivel de aprobación ({4}). Por favor, revisa las condiciones antes de implementar la actividad.\r\n\r\nHaz click en el link adjunto para ver más detalles: {5}", miAmec.idamecs, miAmec.descripcion, unidad, area, estado, link);
                }
                Mail.EnviaMail(message);
                GuardarMail = agAMECinfo.GuardaLogMail(miAmec.idamecs, "MailAmecAprobacionCondicionada", message.To.ToString(), message.Subject, message.Body, null, true, null);

            }
            catch (Exception ex)
            {
                Alert.Show("Se ha producido un error enviando el mail de Notificación de Aprobado Condicionado", null);
                GuardarMail = agAMECinfo.GuardaLogMail(miAmec.idamecs, "MailAmecAprobacionCondicionada", message.To.ToString(), message.Subject, message.Body, null, false, "Error enviando el Correo");
                Logger.Logger.PrintError(this.GetType().Name, "amecNotificaciones", ex.Message, ex);
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
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
                string link = string.Format("{0}DetalleAMEC.aspx?idamec={1}", RutaLinkMail, miAmec.idamecs);
                DDatosPersonalesUsuario dSolicitante = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());
                AgenteUnidadesOrgAMEC agUnid = new AgenteUnidadesOrgAMEC();
                string unidad = agUnid.ObtenerNombreUnidadxId(Convert.ToInt32(dSolicitante.idunidad));
                string area = agUnid.ObtenerNombreAreaxId(Convert.ToInt32(dSolicitante.Idarea));

                try
                {
                    //Buscamos todos los usuarios que han intervenido en el flujo de aprobación para informarles que se ha aprobado el amec. Los usuarios Dep Médico no se les envia confirmación
                    if (miAmec.preaprobadaneg == true)
                    {
                        PermisoSupJer perSupJer = new PermisoSupJer(new AgenteAprobadorAmec());
                        List<string> collectUsuariosIntervenidoFlujoAprobacion = perSupJer.ObtenerListaDestinatarios(miAmec.idamecs, miAmec.idsolicitante.Value);
                        foreach (string correo in collectUsuariosIntervenidoFlujoAprobacion)
                        {
                            datosUsuarioCorreo = agUsu.ObtenerDatosPersonalesPorIDPeticionario(correo);
                            try
                            {
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

                }
                catch (Exception ex)
                {
                    Alert.Show("Se ha producido un error enviando el mail de Notificación de APROBADO del AMEC. Por favor revise la dirección de correo del Solicitante.", null);
                    GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailAmecAprobacionFinal", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico - " + ex.InnerException.ToString());
                    Global.SendApplicationError(ex, Request, Session, GetType().Name);
                    return;
                }
                string importe = string.Format("{0:C}", miAmec.importegasto);
                message.Subject = string.Format("Amec {0} APROBADO: Importe {1}, Número de Ponentes {2} y Número de Personas {3}", miAmec.idamecs, importe, miAmec.ponentespatrocinados, miAmec.participantesmsd);
                if (miAmec.paraguas == true)
                {
                    string listUnidadesOrg = "";
                    //Antes

                    //Cambio por un tiempo Determinado para informar al usuario que el MD no está en el flujo
                    listUnidadesOrg = ObtenerUnidadesOrganizativasParaCorreoHtml();
                    message.Body = string.Format("El Amec número {0} para la actividad {1}<br><br>Haz click en el link adjunto para ver más detalles: {3}", miAmec.idamecs, miAmec.descripcion, listUnidadesOrg, link);
                    ////////////////////////////////////

                }
                else
                {
                    //Cambio por un tiempo Determinado para informar al usuario que el MD no está en el flujo
                    message.Body = string.Format("El Amec número {0} para la actividad {1} ha sido aprobado en su totalidad. <br><br>Haz click en el link adjunto para ver más detalles: {4}", miAmec.idamecs, miAmec.descripcion, unidad, area, link);
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
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
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
            MailMessage message = new MailMessage();
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            try
            {
                AgenteUsuarios agUsu = new AgenteUsuarios();
                //string estado;
                string link = string.Format("{0}DetalleAMEC.aspx?idamec={1}", RutaLinkMail, miAmec.idamecs);
                DDatosPersonalesUsuario dSolicitante = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idsolicitante.ToString());

                if (miAmec.idestado == 8)
                {
                    PermisoNegocio perNegocio = new PermisoNegocio(new AgenteAprobadorAmec());
                    List<string> collectCorreo = perNegocio.ObtenerListaDestinatarios(miAmec.idamecs, dSolicitante.IdPeticionario);
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
                                agAMECinfo.GuardaLogMail(miAmec.idamecs, "EnviaMailPublicarComentario", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico");
                                return;
                            }

                        }
                    }
                }
                try
                {
                    if (miAmec.idestado == 6) message.To.Add(new MailAddress(Web.Variables.EmailMedico));
                    if (miAmec.idestado == 14) message.To.Add(new MailAddress(Web.Variables.EmailLegal));
                 
                    //Cuando el Peticionario que hace el Comentario es el Solicitante
                    if (miAmec.idsolicitante != miAmec.idcreadopor)
                    {
                        DDatosPersonalesUsuario dCreado = agUsu.ObtenerDatosPersonalesPorIDPeticionario(miAmec.idcreadopor.ToString());
                        message.To.Add(new MailAddress(dCreado.Email));
                    }
                    message.To.Add(new MailAddress(dSolicitante.Email));


                    if (MailCopia.Contains('@'))
                    {
                        foreach (string s in MailCopia.Split(';') )
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
                catch (Exception ex)
                {
                    Alert.Show("Se ha producido un error enviando el mail de Publicación de Comentario del AMEC. Por favor revise la dirección de correo.", null);
                    agAmecInf.GuardaLogMail(miAmec.idamecs, "MailPublicarComentario", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico");
                    Global.SendApplicationError(ex, Request, Session, GetType().Name);
                    return;
                }
                string importe = string.Format("{0:C}", miAmec.importegasto);
                message.Subject = string.Format("Cuadro de dialogo en AMEC {0} con Importe {1}, Número de Personas {3} y Número de ponentes {2}", miAmec.idamecs, importe, miAmec.ponentespatrocinados, miAmec.participantesmsd);
                message.Body = string.Format("Tienes un comentario en el cuadro de diálogo para la actividad {1} del número de AMEC {0}. \r\n\r\nHaz click en el link adjunto para ver más detalles: {2}", miAmec.idamecs, miAmec.descripcion, link);

                Mail.EnviaMail(message);
                agAmecInf.GuardaLogMail(miAmec.idamecs, "MailPublicarComentario", message.To.ToString(), message.Subject, message.Body, null, true, null);
            }
            catch (Exception ex)
            {
                Alert.Show("Se ha producido un error enviando el mail Publicación de Comentario del AMEC", null);
                agAmecInf.GuardaLogMail(miAmec.idamecs, "MailPublicarComentario", message.To.ToString(), message.Subject, message.Body, null, false, "Error enviando el Correo");
                Logger.Logger.PrintError(this.GetType().Name, "amecNotificaciones", ex.Message, ex);
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
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
                string link = string.Format("{0}DetalleAMEC.aspx?idamec={1}", RutaLinkMail, miAmec.idamecs);

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
                catch (Exception ex)
                {
                    EOSLogger.PrintError("DetalleAMEC.aspx", "EnviaMailCasosClinicos", ex.Message, ex);
                    Alert.Show("Se ha producido un error enviando el mail Casos Clinicos. Por favor revise la dirección From en la configuración del sistema.", null);
                    GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailCasosClinicos", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico");
                    Global.SendApplicationError(ex, Request, Session, GetType().Name);
                    return;
                }

                message.Subject = string.Format("AMEC {0} con gestión de casos clínicos", miAmec.idamecs); //ASUNTO DEL MAIL [me lo he inventado]
                message.Body = string.Format("\r\n Les informamos que la actividad AMEC {0} conlleva gestión de casos clíncos.\r\n\r\n Nombre del la actividad: {1}, Tipo de actividad: {2} \r\n Solicitada por {3}, que se celebrará {6}.\r\n\r\nHaz click en el link adjunto para ver más detalles: {7}", miAmec.idamecs, miAmec.descripcion, ddlActividad.SelectedItem.Text, ddlSolicitante.SelectedItem.Text, unidad, area, miAmec.fechacomienzo, link);
                Mail.EnviaMail(message); //ENVIAMOS MAIL
                GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailCasosClinicos", message.To.ToString(), message.Subject, message.Body, null, true, null);

            }
            catch (Exception ex)
            {
                EOSLogger.PrintError("DetalleAMEC.aspx", "EnviaMailCasosClinicos", ex.Message, ex);
                Alert.Show("Se ha producido un error enviando el mail Casos Clínicos. Por favor revise la configuración del sistema", null);
                GuardarMail = agAmecInf.GuardaLogMail(miAmec.idamecs, "MailCasosClinicos", message.To.ToString(), message.Subject, message.Body, null, false, "Error enviando el Correo");
                Logger.Logger.PrintError(this.GetType().Name, "amecNotificaciones", ex.Message, ex);
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
                try
                {
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("contactoError"), ex);
                }
                catch { }
            }
        }

        public void EnviaMailFarmaIndustria(bool cambioAgencia = false)
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
                string link = string.Format("{0}DetalleAMEC.aspx?idamec={1}", RutaLinkMail, miAmec.idamecs);
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
                    catch (Exception ex)
                    {
                        Alert.Show("Se ha producido un error enviando el mail a Farmaindustria. Por favor revise la dirección From en la configuración del sistema.", null);
                        GuardarMail = agAmecInfo.GuardaLogMail(miAmec.idamecs, "MailFarmaIndustria", message.To.ToString(), message.Subject, message.Body, null, false, "Error añadiendo la dirección de Correo Electrónico");
                        Global.SendApplicationError(ex, Request, Session, GetType().Name);
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

                if (cambioAgencia)
                {
                    message.Subject = string.Format("Comunicación a Farmaindustria de la actividad {0}, Número de AMEC {1} y Solicitante {2}. Cambio de agencia realizado", miAmec.descripcion, miAmec.idamecs, datosUsuarioSolicitante.NombreCompleto); //ASUNTO DEL MAIL [me lo he inventado]
                }
                else
                {
                    message.Subject = string.Format( "Comunicación a Farmaindustria de  la actividad {0}, Número de AMEC {1} y Solicitante {2}", miAmec.descripcion, miAmec.idamecs, datosUsuarioSolicitante.NombreCompleto);//ASUNTO DEL MAIL [me lo he inventado]
                }

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
                        //string Documentacion = @"Documentacion\";
                        //string saf = savePathDocumentacion;
                        //savePath = ConfigUtil.GetAppSetting("AMECUpload");
                        //sFile = string.Format("{0}{1}\\{2}{3}", savePath, miAmec.idamecs.ToString(), Documentacion, DocAmec.nombredoc.Trim());
                        message.Body = string.Format("{0}\r\n\r\nAdjuntado Documentación {1}\r\n", message.Body, DocAmec.nombredoc.Trim());
                        //saveFile = sFile.Replace("\\", "/");
                        Attachment data = new Attachment(DocAmec.ubicaciondoc);
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
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
                EOSLogger.PrintError(this.GetType().Name, "EnviaMailCasosClinicos", ex.Message, ex);
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

        /// <summary>
        /// Comprobamos si el importe a sido modificado con un valor superior al que está en la base de datos. Si es así y el amec es diferente de borrador el solicitante tendrá que resometer obligatoriamente
        /// </summary>
        /// <returns></returns>
        public bool ComprobarImporte()
        {
            AgenteAmecInfo agAmecInf = new AgenteAmecInfo();
            decimal importeActual = 0;

            if (!string.IsNullOrEmpty(txtImporteTotalGasto.Text))
            {
                txtImporteTotalGasto.Text = txtImporteTotalGasto.Text.Replace(".", "");
                importeActual = Decimal.Parse(txtImporteTotalGasto.Text.Replace(",", "."), CultureInfo.InvariantCulture);
                txtImporteTotalGasto.Text = string.Format("{0:n2}", importeActual);
            }
            if (miAmec.idestado == 5) return true;
            DAmecInfo AmecGuardado = agAmecInf.CargarTodosValoresAmec(lbidAMEC.InnerText);
            if (AmecGuardado != null)
            {
                return importeActual <= AmecGuardado.importegasto;
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
                else if ((Int32.Parse(ddlActividad.SelectedValue) == 9 || Int32.Parse(ddlActividad.SelectedValue) == 17 || Int32.Parse(ddlActividad.SelectedValue) == 18) && chkParaguas.Checked)
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
                if ((Int32.Parse(ddlActividad.SelectedValue) == 9 || Int32.Parse(ddlActividad.SelectedValue) == 17 || Int32.Parse(ddlActividad.SelectedValue) == 18) && chkParaguas.Checked)
                {
                    Alert.Show("No se puede someter un Amec Paraguas con Tipo de Actividad LIBROS");
                    return false;
                }

                else if ((Int32.Parse(ddlActividad.SelectedValue) == 9 || Int32.Parse(ddlActividad.SelectedValue) == 17 || Int32.Parse(ddlActividad.SelectedValue) == 18) && rblPreaprobNegocio.SelectedItem.Value == "1")
                {
                    Alert.Show("No se puede someter un Amec con Tipo de Actividad LIBROS con Preaprobado Negocio");
                    return false;
                }

                else if (string.IsNullOrEmpty(txtImporteTotalGasto.Text))
                {
                    Alert.Show("El importe total del gasto tiene que tener un valor");
                    return false;
                }
                else return true;
            }

        }

        #endregion

        #region "Funciones Auxiliares"

        public void ChechPermisosDeVisibilidad(string idamec)
        {
            try
            {
                if (!idamec.StartsWith("4") && idamec.Length == 9)
                    return;
                //if (idamec < 400000000 && idamec >= 500000000) return;

                AgenteAprobadorAmec agAprobador = new AgenteAprobadorAmec();
                bool puedo = agAprobador.ObtenerPermisoVerAmec(idamec, datosUsuarioAMEC.IdPeticionario);

                if (puedo) return;

                Response.Redirect("ListadoAMECs.aspx", false);
            }
            catch (Exception ex)
            {
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
            }


        }

        
        public void AniadirAprobadorPorDefecto(object sender, ImageClickEventArgs e)
        {
            try
            {
                
                AgentePeticionarioManager agPetManager = new AgentePeticionarioManager();
                DPeticionarioManager manager = agPetManager.ObtenerManagerDePeticionario(datosUsuarioSolicitante.IdPeticionario);
                AniadirAprobador(manager);
                LstManagers = new List<DPeticionarioManager> { manager };
                listManagers.DataSource = LstManagers;
                listManagers.DataBind();
                
                //Alert.Show("No se ha especificado ningún aprobador. Se inserta el superior jerárquico actual del solicitante como aprobador.");
                if (miAmec.idestado != 5)
                {
                    AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
                    AgenteComentariosAMEC agComAmec = new AgenteComentariosAMEC();
                    Alert.Show("Es NECESARIO RESOMETER para que el flujo sea correcto, debido a la modificación de los aprobadores");
                    agAmecInfo.CambiarEstadoAmec("35", miAmec.idamecs.ToString(), datosUsuarioAMEC.IdPeticionario.ToString(), manager.IdPeticionario.ToString());
                    string comentario =string.Format("Se ha cambiado el estado del amec al INSERTAR un nuevo aprobador: {0} {1} {2}",manager.Nombre, manager.Apellido1, manager.Apellido2);
                    agComAmec.InsertComentarioAMEC(miAmec.idamecs.ToString(), datosUsuarioAMEC.IdPeticionario.ToString(), comentario);
                    btnCancelar.Visible = false;
                    btnVolver.Visible = false;
                    btnGuardar.Visible = false;
                    lvComentarios.DataBind();
                    AgenteAmecInfo agenteAmec = new AgenteAmecInfo();
                    lbEstadoAMEC.Text = agenteAmec.ObtenerNombreEstado(35);
                }
            }
            catch (Exception ex)
            {
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
            }
        }

        public void itemXDefecto(DropDownList ddlnombre)
        {
            ListItem itemXdefecto = new ListItem();
            itemXdefecto.Value = "-1";
            itemXdefecto.Text = "Seleccionar";
            ddlnombre.Items.Add(itemXdefecto);
        }

        protected void AniadirAprobador(DPeticionarioManager manager)
        {
            AgenteAprobadorAmec agAprobador = new AgenteAprobadorAmec();
            FiltroAprovadorAmec filtro = new FiltroAprovadorAmec()
            {
                IdAmecs = miAmec.idamecs,
                IdCreador = datosUsuarioSolicitante.IdPeticionario,
                IdAprobador = manager.IdPeticionario,
                AprobacionJefe = AprobacionJefe.NoPuedeAprobar.GetHashCode()
            };
            agAprobador.InsertarAprobadorAmec(filtro);
            
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
                if (TipoActividad == "9" || TipoActividad == "13" || TipoActividad == "14" || ddlActividad.Text == "17" || ddlActividad.Text == "18")
                {
                    rblPreaprobMedico.SelectedValue = "0";
                    rblPreaprobNegocio.SelectedValue = "0";
                    rblPreaprovLegal_Comp.SelectedValue = "0";
                    rblPreaprovLegal_Comp.Enabled = false;
                    rblPreaprobMedico.Enabled = false;
                    rblPreaprobNegocio.Enabled = false;
                    if (miAmec != null && miAmec.idestado != 5 && esCargaFormulario) ddlActividad.Enabled = false;
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

        protected int UploadButton(string tipodeCarga)
        {
            try
            {
                savePath = ConfigurationManager.AppSettings["AmecDocumentation"];
                DDocumentUpload docInfo = new DDocumentUpload();
                GestorDocumentalServiceManager docService = new GestorDocumentalServiceManager();

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
                                String fileName = fuploadDocumentacion.FileName.Replace(' ', '_');
                                
                                savePath = string.Format("{0}{1}", savePath, miAmec.idamecs);


                                string Documentacion = @"\Documentacion";
                                savePath = string.Format("{0}{1}", savePath, Documentacion);
                                savePath = string.Format("{0}\\{1}", savePath, ddlCategoriaDocumento.SelectedItem.Text).Replace(' ', '_');
                                if (!System.IO.Directory.Exists(savePath))
                                {
                                    System.IO.Directory.CreateDirectory(savePath);
                                }
                                fileName = RemoveDiacritics(fileName);

                                savePath = string.Format("{0}\\{1}", savePath, fileName);

                                docInfo.Amec = miAmec.idamecs.ToString();
                                docInfo.Asistente = "-1";
                                docInfo.DocumentoOriginal = fileName;
                                docInfo.IdAmecDocCategory = int.Parse(ddlCategoriaDocumento.SelectedValue);
                                docInfo.Tipo = GestorDocTipo.DocuAdicional.GetHashCode();
                                docInfo.Fichero = savePath;
                                docInfo.Peticionario = datosUsuarioAMEC.IdPeticionario;

                                fuploadDocumentacion.SaveAs(savePath);

                                docService.InsertDocAmecs(docInfo, savePath);
                                docService.SetMetadata(docInfo);

                                savePathDocumentacion = savePath.Replace("\\", "/");
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
                                String fileName = fUploadPrograma.FileName.Replace(' ', '_');
                              
                                savePath = string.Format("{0}{1}", savePath, miAmec.idamecs);


                                string Programa = @"\Programa";
                                savePath = string.Format("{0}{1}", savePath, Programa);
                                if (!System.IO.Directory.Exists(savePath))
                                {
                                    System.IO.Directory.CreateDirectory(savePath);
                                }
                                fileName = RemoveDiacritics(fileName);
                                savePath = string.Format("{0}\\{1}", savePath, fileName);

                                docInfo.Amec = miAmec.idamecs.ToString();
                                docInfo.Asistente = "-1";
                                docInfo.DocumentoOriginal = fileName;
                                docInfo.Tipo = GestorDocTipo.Programa.GetHashCode();
                                docInfo.Fichero = savePath;
                                docInfo.Peticionario = datosUsuarioAMEC.IdPeticionario;

                                fUploadPrograma.SaveAs(savePath);

                                docService.InsertDocAmecs(docInfo, savePath);
                                docService.SetMetadata(docInfo);

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
            estadoAMEC = new RepositorioFlujo(miAmec.idamecs, datosUsuarioAMEC.IdPeticionario, ConfigUtil.GetConnectionString(EOS.ServiceLogic.Variables.IdConnectionDatabase));
            EOS.Entidades.InformacionExpediente InfoExpediente = estadoAMEC.IncorporarExpediente(idexpediente);
            //Impoorte
            txtImporteTotalGasto.Text = InfoExpediente.importe != 0 ? InfoExpediente.importe.ToString() : "0";
            //Sede
            txtLugarSede.Text = !string.IsNullOrEmpty(InfoExpediente.LugarRealizacion) ? InfoExpediente.LugarRealizacion : "";
            //fecha Inicio
            txtFechaComienzo.Text = !string.IsNullOrEmpty(InfoExpediente.FechaComienzo.ToString()) ? InfoExpediente.FechaComienzo.ToShortDateString() : "";
            //fecha fin
            txtFechaFinalizacion.Text = !string.IsNullOrEmpty(InfoExpediente.FechaFin.ToString()) ? InfoExpediente.FechaFin.ToShortDateString() : "";

            //Gastos
            string GastosDesglose = "0";

            if (InfoExpediente.detalleservicios == null) return;

            foreach (Entidades.DetalleServicios detalleservicios in InfoExpediente.detalleservicios)
            {

                GastosDesglose = GastosDesglose + detalleservicios.descripcion;
            }

            txtgastoDesglose.Text = GastosDesglose;
        }

        /// <summary>
        /// Crea una relacion entre AMEC y un id Congreso y actualiza o guarda un nuevo AMEC en base al estado del AMEC
        /// </summary>
        /// <param name="idcongres"></param>
        protected void AsignarEventoAmec(int idcongres)
        {
            if (!ValidarCampos()) return;

            AgenteAmecInfo agAmecInfo = new AgenteAmecInfo();
            //AMEC no insertado. Guardamos valores del amec e insertamos amec y la unidad organizativa por defecto.
            if (!agAmecInfo.EstaGuardadoAmec(miAmec.idamecs))
            {
                GuardarNuevoAmec();
                
            }
            else
            {
                bool cambioAgencia = false;
                ActualizarAmec(out cambioAgencia);
            }
            
            agAmecInfo.CrearRelacionAmecCongreso(idcongres, miAmec.idamecs, datosUsuarioAMEC.IdPeticionario);
            eosContentResults.Visible = false;
            lvActividadAsigAmec.DataBind();
        }

        /// <summary>
        /// Muestra mensaje de que no hay registros cuando se eliminan todos los aprobadores de la lista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void listManagers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (listManagers.Items.Count < 1)
            {

                if (e.Item.ItemType == ListItemType.Footer)
                {
                    // Show the Error Label (if no data is present).
                    Label lblErrorMsg = e.Item.FindControl("lblErrorMsg") as Label;
                    if (lblErrorMsg != null)
                    {
                        lblErrorMsg.Visible = true;
                    }
                }

            }
        }

        protected void ActualizarAmec(out bool cambioAgencia)
        {
            //A parte del Importe tendremos que comprobar que alguien no ha cambiado el estado del Amec (Otro usuario haya aprobado o rechazado el amec mientras tu estabas dentro)
            AgenteAmecInfo agAMECinfo = new AgenteAmecInfo();
            CargarValoreAmecFormulario();
            cambioAgencia = false;
            if (ComprobarEstado(miAmec.idamecs))
            {
                if (miAmec.idestado != 5 && !Sometiendo && !ComprobarImporte())
                {
                    Alert.Show("No se puede Guardar el Amec. Es Obligatorio Resometer debido a la modificación del Importe.");
                }
                else
                {
                    agAMECinfo.ModificarAMEC(miAmec, out cambioAgencia, true);
                    plAMEC_Historial.Visible = true;
                    HabilitaBotones();
                }
            }
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
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
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
            switch (ddlArchivoPrograma.SelectedValue)
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
            }
            return true;
        }

        /// <summary>
        /// Inserta un amec y devuelve uno nuevo
        /// </summary>
        protected void GuardarNuevoAmec()
        {
            AgenteAmecInfo agAMECinfo = new AgenteAmecInfo();

            CargarValoreAmecFormulario();
            miAmec = agAMECinfo.NuevoAMEC(miAmec);
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

        protected void AlertaCeroAprobadores_OnClick(object sender, EventArgs e)
        {
            return;
        }
    }

}
