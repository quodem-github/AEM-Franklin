using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

using EOS.Controls;

using EOS.ServiceLogic.BLL.Calculator;
using EOS.ServiceLogic.Data.DTO.Calculator;
using EOS.Web;
using EOS.Entidades.Datos;
//using NPOI.HSSF.Record.Formula.Functions;
using NPOI.SS.Formula.Functions;
using CheckBox = System.Web.UI.WebControls.CheckBox;
using EOS.ServiceLogic.Enums;

namespace EOS
{
    [System.Runtime.InteropServices.GuidAttribute("4605F5B0-5B4C-45E9-A351-EDF4261F612B")]
    public partial class NuevoExpedientePasoB : System.Web.UI.Page
    {
        private ListItem blankComboItem = new ListItem(" - Seleccione un valor - ", " ");
        private readonly CorrespondenciasService _correspondenciasService;
        private readonly CountryToCountryService _countryToCountryService;
        private readonly TipoContratoConsultoriaService _tipoContratoContratoConsultoriaService;
        private CountryToCountryDto _countryToCountryDto;
        private List<TipoContratoConsultoriaDto> _tiposContratosConsultoria;
        private List<CorrespondenciasDto> _correspondenciasList;

        public List<CorrespondenciasDto> CorrespondenciasList
        {
            get { return _correspondenciasList ?? (_correspondenciasList = _correspondenciasService.GetList()); }
            set { _correspondenciasList = value; }
        }

        public List<TipoContratoConsultoriaDto> TiposContratosConsultoria
        {
            get { return _tiposContratosConsultoria ?? (_tiposContratosConsultoria = _tipoContratoContratoConsultoriaService.GetList()); }
            set { _tiposContratosConsultoria = value; }
        }


        public CountryToCountryDto CountryToCountryObj
        {
            get { return _countryToCountryDto ?? (_countryToCountryDto = _countryToCountryService.Get()); }
            set { _countryToCountryDto = value; }
        }

        public NuevoExpedientePasoB()
        {
            _correspondenciasService = new CorrespondenciasService();
            _countryToCountryService = new CountryToCountryService();
            _tipoContratoContratoConsultoriaService = new TipoContratoConsultoriaService();
        }

        private const string ID_KEY_PART_REMOVED = "ParticipantesEliminados_";
        private const string ID_KEY_HONORARIOS = "HonorariosPassenger_";

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

        protected EOS.Entidades.DetalleExpediente dExpediente = null;
        private ListaPP ListaProductoPorcentaje = new ListaPP();

        protected void Page_Load(object sender, EventArgs e)
        {
            ///////////////Controlar Si caduca la sesión en la aplicación//////////////
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Session["rolesuser"] == null) Response.Redirect("~/Account/Login.aspx");
            //////////////////////////////////////////////////////////////////////////

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            // Sólo se puede llegar a esta página si tenemos una entidad Expediente ya creada. En caso contrario, redirigimos a NuevoExpPasoA
            if (Session["NuevoExpediente"] != null)
            {
                dExpediente = (EOS.Entidades.DetalleExpediente)Session["NuevoExpediente"];
                btnCancelar.OnClientClick = "javascript:document.location = 'NuevoExpedientePasoA.aspx?idexp=" + dExpediente.nIDExpediente + "&tipo=" + dExpediente.tipoPagoFee.ToString() + "';return false;";
            }
            else
            {
                Response.Redirect("NuevoExpedientePasoA.aspx");
            }
            if (Session["ListaProductosPorcentaje"] != null)
            {
                ListaProductoPorcentaje.Clear();
                ListaProductoPorcentaje = (ListaPP)Session["ListaProductosPorcentaje"];
            }

            //Al hacer un cargar página se pierden los titles del dropdownlist
            CargaTitleJustificaciones();

            hiddenPonentesNivelPSList.Value = GetNivelPonentePS();

            if (!Page.IsPostBack)
            {
                BindDataCalculadora();
                RellenaCombos();
                dtHonorariosPassangerCreados.Clear();
                if (dExpediente.nIDExpediente > 0)
                {
                    this.btnGuardar.OnClientClick = string.Format("javascript:return confirm('¿Desea guardar los datos modificados del expediente {0}')", dExpediente.nIDExpediente);
                }
                else
                {
                    this.btnGuardar.OnClientClick = "confirma();";
                }

                // Si no tiene participantes asignados, hace la asignación de participantes inicial del AMEC.
                if (dExpediente.dParticipantes == null)
                {
                    AgenteParticipantes agPar = new AgenteParticipantes();
                    dExpediente.dParticipantes = agPar.ObtenerParticipantesExpediente(dExpediente.nIDExpediente.ToString(), "", 0, 1000);
                }

                // Si proviene de la creación de un nuevo participante, lo añade al conjunto temporal
                if (Session["ParticipanteNuevo"] != null)
                {
                    string sIdPar = Session["ParticipanteNuevo"].ToString();
                    DataTable dt = (DataTable)Session["dataParticipanteNuevo"];
                    AgenteParticipantes agPar = new AgenteParticipantes();
                    DVParticipante dvAdd = agPar.ObtenerParticipantePorID(sIdPar);
                    dvAdd.tipoactividadpax = dt.Rows[0]["tipoactividadpax"].ToString();
                    dvAdd.tipoasistente = dt.Rows[0]["tipoasistente"].ToString();
                    dvAdd.honorarios = dt.Rows[0]["honorarios"].ToString() == "" ? 0 : decimal.Parse(dt.Rows[0]["honorarios"].ToString()); ;
                    dvAdd.nivelriesgo = dt.Rows[0]["nivelriesgo"].ToString();
                    dExpediente.dParticipantes.Add(dvAdd);
                    GuardarHonorariosDataSet(
                        dt.Rows[0]["idpassengerlist"].ToString(),
                        dt.Rows[0]["idtipoasistente"].ToString(),
                        dt.Rows[0]["ficherogenesis"].ToString(),
                        dt.Rows[0]["idtipoactividadpax"].ToString(),
                        dt.Rows[0]["idnivelriesgo"].ToString(),
                        dt.Rows[0]["justificaciones"].ToString(),
                        dt.Rows[0]["honorarios"].ToString(),
                        dt.Rows[0]["pagodirecto"].ToString(),
                        dt.Rows[0]["pagosociedad"].ToString(),
                        dt.Rows[0]["tiporeunion"].ToString(),
                        dt.Rows[0]["ponentes_duracionactividad"].ToString(),
                        dt.Rows[0]["ponentes_preparacion"].ToString(),
                        dt.Rows[0]["ponentes_nivelps"].ToString(),
                        dt.Rows[0]["ponentes_honorariosmaximos"].ToString(),
                        dt.Rows[0]["ponentes_idhonorariosmaximos"].ToString(),
                        dt.Rows[0]["abeif_preparacion"].ToString(),
                        dt.Rows[0]["abeif_duracionactividad"].ToString(),
                        dt.Rows[0]["abeif_nivelps"].ToString(),
                        dt.Rows[0]["abeif_honorariosmaximos"].ToString(),
                        dt.Rows[0]["ponencia_centro_salud"].ToString(),
                        dt.Rows[0]["talleres"].ToString(),
                        dt.Rows[0]["videoconferencia_repetida"].ToString(),
                        dt.Rows[0]["tipo_ponente"].ToString(),
                        dt.Rows[0]["justificacion"].ToString(),
                        dt.Rows[0]["idTipoContratoConsultoria"].ToString(),
                        dt.Rows[0]["numero_dias_consultoria"].ToString()
                        );
                    this.lvServicioParticipantes.DataBind();
                    Session.Remove("ParticipanteNuevo");
                    Session.Remove("dataParticipanteNuevo");
                }

                // Activa o desactiva el check de individual en función de si tiene participantes o no
                ControlaCheckIndividualColectivo();

                lvParticipantesVeeva.Visible = true;
                lvParticipantesVeeva.DataBind();
            }
        }

        
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            lvParticipantes.Visible = true;
            lvParticipantes.DataBind();
        }

        protected void btnFiltrarVeeva_Click(object sender, EventArgs e)
        {
            lvParticipantesVeeva.Visible = true;
            lvParticipantesVeeva.DataBind();
        }

        protected void odsParticipantes_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNombre.Text.Trim())) e.InputParameters["filtroNombre"] = "%" + txtNombre.Text.Trim() + "%";
            if (!string.IsNullOrEmpty(txtApel1.Text.Trim())) e.InputParameters["filtroApel1"] = "%" + txtApel1.Text.Trim() + "%";
            //if (!string.IsNullOrEmpty(txtApel2.Text.Trim())) e.InputParameters["filtroApel2"] = "%" + txtApel2.Text.Trim() + "%";
            if (!string.IsNullOrEmpty(txtHospital.Text.Trim())) e.InputParameters["filtroHospital"] = "%" + txtHospital.Text.Trim() + "%";

            if (dExpediente != null)
            {
                e.InputParameters["filtroIDExpediente"] = dExpediente.nIDExpediente.ToString();
            }
        }

        protected void odsParticipantesVeeva_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNombreVeeva.Text.Trim())) e.InputParameters["filtroNombre"] = "%" + txtNombreVeeva.Text.Trim() + "%";
            if (!string.IsNullOrEmpty(txtApel1Veeva.Text.Trim())) e.InputParameters["filtroApel1"] = "%" + txtApel1Veeva.Text.Trim() + "%";
            //if (!string.IsNullOrEmpty(txtApel2.Text.Trim())) e.InputParameters["filtroApel2"] = "%" + txtApel2.Text.Trim() + "%";
            //if (!string.IsNullOrEmpty(txtHospitalVeeva.Text.Trim())) e.InputParameters["filtroHospital"] = "%" + txtHospitalVeeva.Text.Trim() + "%";

            if (dExpediente != null)
            {
                e.InputParameters["filtroIDExpediente"] = dExpediente.nIDExpediente.ToString();
                e.InputParameters["filtroIDAmecsString"] = dExpediente.AMEC.amec;                
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            // Si se trata de un expediente nuevo lo insertamos. En cualquier otro caso debería ser una modificación
            if (dExpediente != null)
            {
                try
                {
                    if (!Validacion()) return;

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
                    int nIdTipoReserva = 1;

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
                        dExpediente.dParticipantes,
                        listparticipantesEliminados,
                        dtHonorariosPassangerCreados,
                        dExpediente.nIDExpediente, datosUsuario.iddepartament, datosUsuario.idsaleforce, datosUsuario.iddistrict);

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

        protected void odsParticipantesExpediente_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (dExpediente != null)
            {
                e.InputParameters["filtroIDExpediente"] = dExpediente.nIDExpediente.ToString();
            }

        }

        protected void lvServicioParticipantes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvServicioParticipantes.SelectedValue != null)
            {


                AgenteExpedientes agExp = new AgenteExpedientes();
                DCabeceraExpedienteAmpliado dCabExp = agExp.ObtenerExpedientePorID(dExpediente.nIDExpediente.ToString());
                //No es pot eliminar cap usuari si està en un estat "En curso"
                if (dCabExp != null && (dCabExp.Idestado == "NC" || dCabExp.Idestado == "AB"))
                {
                    DVParticipante dvRemove = dExpediente.dParticipantes.FirstOrDefault(
                        f => f.IdPassengerlist == (int)lvServicioParticipantes.SelectedValue);
                    if (dvRemove != null)
                    {
                        bool tieneServiciosAsignados = TieneServiciosAsignados(dvRemove, dExpediente.nIDExpediente);

                        if (!tieneServiciosAsignados)
                        {
                            DataRow foundRow = EstaEnTablaHonorariosCreados(dvRemove.IdPassengerlist);
                            if (foundRow != null)
                            {
                                //Si está en la tabla honorarios significará que el usuario/honorarios no están en base de datos.
                                dtHonorariosPassangerCreados.Rows.Remove(foundRow);
                            }
                            else
                            {
                                listparticipantesEliminados.Add(dvRemove.IdPassengerlist);
                            }

                            dExpediente.dParticipantes.Remove(dvRemove);
                            this.lvParticipantes.DataBind();
                        }
                        else
                        {
                            Alert.Show("No puede eliminar el participante porque tiene servicios asignados");
                        }
                    }
                }
                else
                {
                    DVParticipante dvRemove =
                        dExpediente.dParticipantes.FirstOrDefault(
                            f => f.IdPassengerlist == (int)lvServicioParticipantes.SelectedValue);

                    if (dvRemove != null)
                    {
                        dExpediente.dParticipantes.Remove(dvRemove);
                        DataRow foundRow = EstaEnTablaHonorariosCreados(dvRemove.IdPassengerlist);
                        if (foundRow != null)
                        {
                            //Si está en la tabla honorarios significará que el usuario/honorarios no están en base de datos.
                            dtHonorariosPassangerCreados.Rows.Remove(foundRow);
                        }
                        this.lvParticipantes.DataBind();
                    }
                }
                ControlaCheckIndividualColectivo();
            }
        }

        public string GetJsonObject(object data)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize((DVParticipante)data);
        }


        private bool GuardarHonorariosDataSet(string idpassengerlist, string idtipoasisitente, string ficheroGenesis, string idtipoactividadpax,
            string idnivelriesgo, string idjustificaciones, string honorarios, string pagodirecto, string pagosociedad,
            string tiporeunion, string ponentes_duracionactividad, string ponentes_preparacion, string ponentes_nivelps, string ponentes_honorariosmaximos, string ponentes_idhonorariosmaximos,
            string abeif_preparacion, string abeif_duracionactividad, string abeif_nivelps, string abeif_honorariosmaximos, string ponenciaCentroSalud, string talleres, string videoconferenciaRepetida, string tipo_ponente, string justificacion, string idTipoContratoConsultoria, string numero_dias_consultoria)
        {
            try
            {

                if (idtipoasisitente != "2" && idtipoasisitente != "1" && idtipoasisitente != "3" && idtipoasisitente != "9")
                    idjustificaciones = "";
                if (idtipoasisitente != "2" && idtipoasisitente != "3" && idtipoasisitente != "6" && idtipoasisitente != "7" && idtipoasisitente != "8" && idtipoasisitente != "9" && idtipoasisitente != "10")
                {
                    honorarios = "";
                    pagodirecto = "";
                    pagosociedad = "";
                }


                switch (ddlTipoAsistente.SelectedValue)
                {
                    case "2":
                    case "3":
                    case "6":
                    case "9":
                        idTipoContratoConsultoria = string.Empty;
                        numero_dias_consultoria = string.Empty;
                        abeif_preparacion = string.Empty;
                        abeif_duracionactividad = string.Empty;
                        abeif_nivelps = string.Empty;
                        abeif_honorariosmaximos = string.Empty;
                        break;
                    case "7":
                    case "8":
                        idTipoContratoConsultoria = string.Empty;
                        numero_dias_consultoria = string.Empty;
                        tiporeunion = string.Empty;
                        ponentes_duracionactividad = string.Empty;
                        ponentes_preparacion = string.Empty;
                        ponentes_nivelps = string.Empty;
                        ponentes_honorariosmaximos = string.Empty;
                        ponenciaCentroSalud = "0";
                        talleres = "0";
                        videoconferenciaRepetida = "0";
                        tipo_ponente = string.Empty;
                        break;
                    case "10":
                        tiporeunion = string.Empty;
                        ponentes_duracionactividad = string.Empty;
                        ponentes_preparacion = string.Empty;
                        ponentes_nivelps = string.Empty;
                        ponentes_honorariosmaximos = string.Empty;
                        ponenciaCentroSalud = "0";
                        talleres = "0";
                        videoconferenciaRepetida = "0";
                        tipo_ponente = string.Empty;
                        abeif_preparacion = string.Empty;
                        abeif_duracionactividad = string.Empty;
                        abeif_nivelps = string.Empty;
                        abeif_honorariosmaximos = string.Empty;
                        break;
                    default:
                        idTipoContratoConsultoria = string.Empty;
                        numero_dias_consultoria = string.Empty;
                        tiporeunion = string.Empty;
                        ponentes_duracionactividad = string.Empty;
                        ponentes_preparacion = string.Empty;
                        ponentes_nivelps = string.Empty;
                        ponentes_honorariosmaximos = string.Empty;
                        abeif_preparacion = string.Empty;
                        abeif_duracionactividad = string.Empty;
                        abeif_nivelps = string.Empty;
                        abeif_honorariosmaximos = string.Empty;
                        ponenciaCentroSalud = "0";
                        talleres = "0";
                        videoconferenciaRepetida = "0";
                        tipo_ponente = string.Empty;
                        break;

                }


                dtHonorariosPassangerCreados.Rows.Add(idpassengerlist, idtipoasisitente, ficheroGenesis,
                    idtipoactividadpax, idnivelriesgo, idjustificaciones, honorarios, pagodirecto, pagosociedad,
                    tiporeunion, ponentes_duracionactividad, ponentes_preparacion, ponentes_nivelps,
                    ponentes_honorariosmaximos, ponentes_idhonorariosmaximos,
                    abeif_preparacion, abeif_duracionactividad, abeif_nivelps, abeif_honorariosmaximos,
                    ponenciaCentroSalud, talleres, videoconferenciaRepetida, tipo_ponente, justificacion, idTipoContratoConsultoria, numero_dias_consultoria);
                return false;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        protected bool TieneServiciosAsignados(DVParticipante participante, int idexp)
        {
            AgenteParticipantes agParticipantes = new AgenteParticipantes();
            ICollection<DPassengerList> passengers = null;
            passengers = agParticipantes.getInscripcionList(idexp);

            foreach (var passenger in passengers)
            {
                if (passenger.idpassengerlist == participante.IdPassengerlist) return true;
            }

            passengers = agParticipantes.getAlojamientoList(idexp);

            foreach (var passenger in passengers)
            {
                if (passenger.idpassengerlist == participante.IdPassengerlist) return true;
            }

            passengers = agParticipantes.getTransporteList(idexp);

            foreach (var passenger in passengers)
            {
                if (passenger.idpassengerlist == participante.IdPassengerlist) return true;
            }

            passengers = agParticipantes.getActividadesList(idexp);

            foreach (var passenger in passengers)
            {
                if (passenger.idpassengerlist == participante.IdPassengerlist) return true;
            }

            return false;
        }

        protected void ControlaCheckIndividualColectivo()
        {
            if (dExpediente.nIDExpediente > 0 && dExpediente.nIDTipo == 1)
            {
                bool bParticipantes = (dExpediente.dParticipantes != null) ? (dExpediente.dParticipantes.Count > 0 ? true : false) : false;
                if (bParticipantes)
                {
                    //this.chkIndividuales.Enabled = false;
                    //this.lblIndividuales.ToolTip = "Antes de cambiar el tipo de Expediente debe eliminar todos sus participantes";
                }
                else
                {
                    //this.chkIndividuales.Enabled = true;
                    //this.chkIndividuales.ToolTip = "";
                }
            }
            if (dExpediente.nIDExpediente > 0 && dExpediente.nIDTipo == 2)
            {
                //this.chkColectivos.Enabled = false;
            }
        }

        protected Boolean Validacion()
        {
            //if (!chkIndividuales.Checked /*&& !chkColectivos.Checked*/)
            //{
            //    Alert.Show("Debe elegir si se trata de un expediente individual o colectivo", null);
            //    return false;
            //}

            //if (chkIndividuales.Checked && lvServicioParticipantes.Items.Count() == 0)
            //{
            //    Alert.Show("Debe seleccionar al menos un participante", null);
            //    return false;
            //}

            return true;
        }

        [System.Web.Services.WebMethod]
        public static string obtenerMensaje(string idrisk, string idtipoasis, string idtipoact)
        {

            AgenteExpedientes agExp = new AgenteExpedientes();
            string text = agExp.ObtenerMensajeAMostrar(idtipoact, idtipoasis, idrisk);
            return text;
        }


        protected void btnSeleccionarParticipante(object sender, EventArgs e)
        {
            try
            {
                if (Request.Form["Participante"] == "" || Request.Form["Participante"] == null) return;
                string idPassengerList = Request.Form["Participante"].Split(' ')[3];
                AgenteParticipantes agPar = new AgenteParticipantes();
                AgenteAmecInfo agInfo = new AgenteAmecInfo();
                if (ddlTipoAsistente.SelectedValue == "4")
                {
                    ddljustificaciones.SelectedValue = " ";
                    ddlTipoActividadPax.SelectedValue = " ";

                }
                string fichero = ddlTipoAsistente.SelectedValue == "4" ? "" : rbficheroGenesis.SelectedValue;
                DVParticipante dvAdd = agPar.ObtenerParticipantePorID(idPassengerList);

                dvAdd.tipoasistente = ddlTipoAsistente.SelectedItem.Text;
                dvAdd.idtipoasistente = uint.Parse(ddlTipoAsistente.SelectedValue);
                if (!string.IsNullOrWhiteSpace(rbficheroGenesis.SelectedValue))
                {
                    dvAdd.ficherogenesis = int.Parse(rbficheroGenesis.SelectedValue);
                }
                dvAdd.nivelriesgo = ddlRiskLevel.SelectedItem.Text;
                if (!string.IsNullOrWhiteSpace(ddlTipoActividadPax.SelectedValue))
                {
                    dvAdd.tipoactividadpax = ddlTipoActividadPax.SelectedItem.Text;
                    dvAdd.idtipoactividadpax = uint.Parse(ddlTipoActividadPax.SelectedValue);
                }
                dvAdd.pagosociedad = txtPagoSociedadOtros.Text;
                dvAdd.pagodirecto = string.IsNullOrWhiteSpace(rblpagodirecto.SelectedValue) ? new int() : int.Parse(rblpagodirecto.SelectedValue);
                
                dvAdd.tiporeunion = string.IsNullOrWhiteSpace(ddlSpeakerChairTipoReunion.SelectedValue) ? new int?() : int.Parse(ddlSpeakerChairTipoReunion.SelectedValue);
                dvAdd.ponentes_nivelps = string.IsNullOrWhiteSpace(ddlSpeakerChairTipoPS.SelectedValue) ? new int?() : int.Parse(ddlSpeakerChairTipoPS.SelectedValue);
                dvAdd.ponentes_duracionactividad = string.IsNullOrWhiteSpace(ddlSpeakerChairDuracionActividad.SelectedValue) ? new int?() : int.Parse(ddlSpeakerChairDuracionActividad.SelectedValue);
                dvAdd.ponentes_preparacion = string.IsNullOrWhiteSpace(ddlSpeakerChairTiempoPreparacion.SelectedValue) ? new int?() : int.Parse(ddlSpeakerChairTiempoPreparacion.SelectedValue);
                dvAdd.tipo_ponente = string.IsNullOrWhiteSpace(rblTipoPonente.SelectedValue) ? new int?() : int.Parse(rblTipoPonente.SelectedValue); 

                dvAdd.ponentes_honorariosmaximos = string.IsNullOrWhiteSpace(txtSpeakerChairHonorariosMaximos.Text) ? new float?() : float.Parse(txtSpeakerChairHonorariosMaximos.Text);
                dvAdd.ponentes_idhonorariosmaximos = string.IsNullOrWhiteSpace(hidFieldIdValueHonorariosMaximos.Value) ? new int?() : int.Parse(hidFieldIdValueHonorariosMaximos.Value);
                dvAdd.abeif_preparacion = string.IsNullOrWhiteSpace(ddlEifABTiempoPreparacion.SelectedValue) ? new int?() : int.Parse(ddlEifABTiempoPreparacion.SelectedValue); 
                dvAdd.abeif_duracionactividad = string.IsNullOrWhiteSpace(ddlEifABDuracionActividad.SelectedValue) ? new int?() : int.Parse(ddlEifABDuracionActividad.SelectedValue); 
                dvAdd.abeif_nivelps = string.IsNullOrWhiteSpace(ddlEifABTipoPS.SelectedValue) ? new int?() : int.Parse(ddlEifABTipoPS.SelectedValue);
                dvAdd.abeif_honorariosmaximos = string.IsNullOrWhiteSpace(txtEifABHonorariosMaximos.Text) ? new float?() : float.Parse(txtEifABHonorariosMaximos.Text);
                
                dvAdd.ponencia_centro_salud = chkBoxPonenciaCentroSalud.Checked ? int.Parse("1") : int.Parse("0");
                dvAdd.talleres = chkBoxTalleres.Checked ? int.Parse("1") : int.Parse("0");
                dvAdd.videoconferencia_repetida = chkBoxVideoconferenciasRepetidas.Checked ? int.Parse("1") : int.Parse("0");
                
                dvAdd.justificacion = txtJustificacion.Text;
                dvAdd.idTipoContratoConsultoria = string.IsNullOrWhiteSpace(rblConsultoriaTipoReunion.SelectedValue) ? new int?() : int.Parse(rblConsultoriaTipoReunion.SelectedValue); 
                dvAdd.numero_dias_consultoria = string.IsNullOrWhiteSpace(txtConsultoriaDiaConsultor.Text) ? new int?() : int.Parse(txtConsultoriaDiaConsultor.Text);

                if (ddlTipoAsistente.SelectedValue == "10")
                {
                    var correspondencia = CorrespondenciasList.Find(
                    x =>
                        x.IdTipoAsistente.GetValueOrDefault().ToString() ==
                        EAsistenteTypes.Consultor.GetHashCode().ToString() && x.IdTipoContratoConsultoria == dvAdd.idTipoContratoConsultoria);

                    dvAdd.FloatMinValue = honorariosMaximosList.Find(x => x.id == correspondencia.IdCalcHonorariosMaximos).FloatValue;
                }

                dvAdd.honorarios = txthonorarios.Text == "" ? 0 : decimal.Parse(txthonorarios.Text);

                dExpediente.dParticipantes.Add(dvAdd);
                lvServicioParticipantes.DataBind();
                lvParticipantes.DataBind();
                lvParticipantesVeeva.DataBind();
                //Honorarios//
                //Se tienen que guardar los honorarios que se crean en un dataset para cuando se ejecute el guardar general y guardarlo
                GuardarHonorariosDataSet(
                    idPassengerList,
                    ddlTipoAsistente.SelectedValue,
                    fichero,
                    ddlTipoActividadPax.SelectedValue,
                    inputRiskLevel.Value,
                    ddljustificaciones.SelectedValue,
                    txthonorarios.Text,
                    rblpagodirecto.SelectedValue,
                    txtPagoSociedadOtros.Text,
                    ddlSpeakerChairTipoReunion.SelectedValue,
                    ddlSpeakerChairDuracionActividad.SelectedValue,
                    ddlSpeakerChairTiempoPreparacion.SelectedValue,
                    ddlSpeakerChairTipoPS.SelectedValue,
                    string.IsNullOrWhiteSpace(txtSpeakerChairHonorariosMaximos.Text) ? "" : float.Parse(txtSpeakerChairHonorariosMaximos.Text).ToString(),
                    hidFieldIdValueHonorariosMaximos.Value,
                    ddlEifABTiempoPreparacion.SelectedValue,
                    ddlEifABDuracionActividad.SelectedValue,
                    ddlEifABTipoPS.SelectedValue,
                    string.IsNullOrWhiteSpace(txtEifABHonorariosMaximos.Text) ? "" : float.Parse(txtEifABHonorariosMaximos.Text).ToString(),
                    chkBoxPonenciaCentroSalud.Checked ? "1" : "0",
                    chkBoxTalleres.Checked ? "1" : "0",
                    chkBoxVideoconferenciasRepetidas.Checked ? "1" : "0",
                    rblTipoPonente.SelectedValue,
                    txtJustificacion.Text,
                    rblConsultoriaTipoReunion.SelectedValue,
                    txtConsultoriaDiaConsultor.Text
                    );
                /////////////

                //INFORME FCPA (Solo se insertará cuando salga el mensaje al insertar participante)//
                if (ddlTipoAsistente.SelectedValue != "4")
                {
                    string mensaje = obtenerMensaje(ddlRiskLevel.SelectedValue, ddlTipoAsistente.SelectedValue,
                        ddlTipoActividadPax.SelectedValue);
                    if (!string.IsNullOrEmpty(mensaje) && mensaje != "NULL")
                    {
                        DAmecInfo amec = new DAmecInfo();
                        amec = agInfo.CargarTodosValoresAmec(dExpediente.AMEC.amec);
                        if (amec != null)
                        {
                            agInfo.guardarEnInformeFCPA(amec.idamecs, amec.idtipoactividad.ToString(), amec.descripcion,
                                amec.fechacomienzo, amec.fechafinalizacion, amec.idestado, 3, dvAdd.Nombre, dvAdd.Apel1,
                                dvAdd.Msdid,
                                "", dExpediente.nIDExpediente.ToString());
                        }
                        else
                        {
                            agInfo.guardarEnInformeFCPA(dExpediente.AMEC.amec, "", "", null, null, 1, 3,
                                dvAdd.Nombre, dvAdd.Apel1, dvAdd.Msdid,
                                "", dExpediente.nIDExpediente.ToString());
                        }
                    }
                }
                ///////////////

                ControlaCheckIndividualColectivo();
                //RellenaCombos();
                LimpiarCamposCalculadora();
            }
            catch (Exception err)
            {
                Global.SendApplicationError(err, Request, Session, GetType().Name);
                Alert.Show("Message2:" + err.Message.ToString() + " / Target:" + err.TargetSite.ToString() + " / Source:" + err.Source);
            }
        }

        private void CargaTitleJustificaciones()
        {
            foreach (ListItem _listItem in this.ddljustificaciones.Items)
            {
                //_listItem.Attributes.AddAttributes("title"); = _listItem.Text;
                _listItem.Attributes.Add("Title", _listItem.Text);
            }

            // add a tooltip for the selected item also
            ddljustificaciones.Attributes.Add("onmouseover", "this.title=this.options[this.selectedIndex].title");
        }
        private void RellenaCombos()
        {
            try
            {
                AgenteExpedientes agExpedientes = new AgenteExpedientes();

                this.ddlRiskLevel.DataSource = agExpedientes.ObtenerRiskLevel();
                this.ddlRiskLevel.DataBind();
                ListItem blankRiskLevel = new ListItem("Selecciona Nivel de riesgo HCP en APPIAN", " ");
                ddlRiskLevel.Items.Insert(0, blankRiskLevel);
                this.ddlRiskLevel.SelectedValue = " ";

                this.ddlTipoActividadPax.DataSource = agExpedientes.ObtenerTiposActividadPax();
                //this.ddlTipoActividadPax.SelectedValue = "1";
                this.ddlTipoActividadPax.DataBind();
                ListItem blankTipoActividadPax = new ListItem("Selecciona el tipo de actividad que definirá el riesgo definitivo de FCPA", " ");
                ddlTipoActividadPax.Items.Insert(0, blankTipoActividadPax);
                this.ddlTipoActividadPax.SelectedValue = " ";

                if (dExpediente.tipoPagoFee == 3)
                {
                    this.ddlTipoAsistente.DataSource = agExpedientes.ObtenerTiposAsistenteConCalculadora();
                }
                else
                {
                    this.ddlTipoAsistente.DataSource = agExpedientes.ObtenerTiposAsistente();
                }
                this.ddlTipoAsistente.DataBind();
                ListItem blankTipoAsisitente = new ListItem("Selecciona Tipo de Participante", " ");
                ddlTipoAsistente.Items.Insert(0, blankTipoAsisitente);
                this.ddlTipoAsistente.SelectedValue = " ";

                //this.ddljustificaciones.SelectedValue = "";
                this.ddljustificaciones.DataSource = agExpedientes.ObtenerJustificaciones();
                this.ddljustificaciones.DataBind();

                ListItem blankJustificaciones = new ListItem("Selecciona una justificación", " ");
                ddljustificaciones.Items.Insert(0, blankJustificaciones);
                this.ddljustificaciones.SelectedValue = " ";
                if (dExpediente.sCongreso != null && EsEventoInternacional(dExpediente.nIDCogreso))
                {
                    hiddenInternacional.Value = "1";
                    rfdtxtjustificaciones.Enabled = true;
                }
                else
                { hiddenInternacional.Value = "0"; }


                txtPagoSociedadOtros.Text = "";
                txthonorarios.Text = "";
                rbficheroGenesis.SelectedValue = null;
                rblpagodirecto.SelectedValue = null;

                if (dtHonorariosPassangerCreados.Rows.Count == 0 && dtHonorariosPassangerCreados.Columns.Count == 0)
                {
                    dtHonorariosPassangerCreados.Columns.Add("idpassengerlist");
                    dtHonorariosPassangerCreados.Columns.Add("idtipoasistente");
                    dtHonorariosPassangerCreados.Columns.Add("ficherogenesis");
                    dtHonorariosPassangerCreados.Columns.Add("idtipoactividadpax");
                    dtHonorariosPassangerCreados.Columns.Add("idnivelriesgo");
                    dtHonorariosPassangerCreados.Columns.Add("justificaciones");
                    dtHonorariosPassangerCreados.Columns.Add("honorarios");
                    dtHonorariosPassangerCreados.Columns.Add("pagodirecto");
                    dtHonorariosPassangerCreados.Columns.Add("pagosociedad");
                    dtHonorariosPassangerCreados.Columns.Add("tiporeunion");
                    dtHonorariosPassangerCreados.Columns.Add("ponentes_duracionactividad");
                    dtHonorariosPassangerCreados.Columns.Add("ponentes_preparacion");
                    dtHonorariosPassangerCreados.Columns.Add("ponentes_nivelps");
                    dtHonorariosPassangerCreados.Columns.Add("ponentes_honorariosmaximos");
                    dtHonorariosPassangerCreados.Columns.Add("ponentes_idhonorariosmaximos");
                    dtHonorariosPassangerCreados.Columns.Add("abeif_preparacion");
                    dtHonorariosPassangerCreados.Columns.Add("abeif_duracionactividad");
                    dtHonorariosPassangerCreados.Columns.Add("abeif_nivelps");
                    dtHonorariosPassangerCreados.Columns.Add("abeif_honorariosmaximos");
                    dtHonorariosPassangerCreados.Columns.Add("ponencia_centro_salud");
                    dtHonorariosPassangerCreados.Columns.Add("talleres");
                    dtHonorariosPassangerCreados.Columns.Add("videoconferencia_repetida");
                    dtHonorariosPassangerCreados.Columns.Add("tipo_ponente");
                    dtHonorariosPassangerCreados.Columns.Add("justificacion");
                    dtHonorariosPassangerCreados.Columns.Add("idTipoContratoConsultoria");
                    dtHonorariosPassangerCreados.Columns.Add("numero_dias_consultoria");
                }

            }
            catch (Exception err)
            {
                Global.SendApplicationError(err, Request, Session, GetType().Name);
                Alert.Show("Message2:" + err.Message.ToString() + " / Target:" + err.TargetSite.ToString() + " / Source:" + err.Source);
            }
        }

        private DataRow EstaEnTablaHonorariosCreados(int idpassengerlist)
        {
            try
            {
                DataRow[] foundRow = dtHonorariosPassangerCreados.Select("idpassengerlist like '%" + idpassengerlist + "%'");
                if (foundRow.Any())
                    return foundRow[0];
                else return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        private bool EsEventoInternacional(int idevento)
        {
            try
            {
                AgenteExpedientes agExpedientes = new AgenteExpedientes();
                return agExpedientes.EsEventoInternacional(idevento);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al intentar comprobar un evento internacional: " + ex.Message);
            }
        }

        #region Nuevos desarrollos calculadora

        public List<HonorariosMaximosDto> honorariosMaximosList
        {
            get
            {
                List<HonorariosMaximosDto> sAux = new List<HonorariosMaximosDto>();
                if (ViewState["honorariosMaximosList_" + Session.SessionID] != null)
                {
                    sAux = (List<HonorariosMaximosDto>)ViewState["honorariosMaximosList_" + Session.SessionID];
                }

                ViewState["honorariosMaximosList_" + Session.SessionID] = sAux;

                return sAux;
            }
            set
            {
                ViewState["honorariosMaximosList_" + Session.SessionID] = value;
            }
        }

        public List<ABEifDuracionActividadDto> abEifDuracionActividadList
        {
            get
            {
                List<ABEifDuracionActividadDto> sAux = new List<ABEifDuracionActividadDto>();
                if (ViewState["abEifDuracionActividadList_" + Session.SessionID] != null)
                {
                    sAux = (List<ABEifDuracionActividadDto>)ViewState["abEifDuracionActividadList_" + Session.SessionID];
                }

                ViewState["abEifDuracionActividadList_" + Session.SessionID] = sAux;

                return sAux;
            }
            set
            {
                ViewState["abEifDuracionActividadList_" + Session.SessionID] = value;
            }
        }

        public List<ABEifNivelPSDto> abEifNivelPSList
        {
            get
            {
                List<ABEifNivelPSDto> sAux = new List<ABEifNivelPSDto>();
                if (ViewState["abEifNivelPSList_" + Session.SessionID] != null)
                {
                    sAux = (List<ABEifNivelPSDto>)ViewState["abEifNivelPSList_" + Session.SessionID];
                }

                ViewState["abEifNivelPSList_" + Session.SessionID] = sAux;

                return sAux;
            }
            set
            {
                ViewState["abEifNivelPSList_" + Session.SessionID] = value;
            }
        }

        public List<ABEifPreparacionDto> abEifPreparacionList
        {
            get
            {
                List<ABEifPreparacionDto> sAux = new List<ABEifPreparacionDto>();
                if (ViewState["abEifPreparacionList_" + Session.SessionID] != null)
                {
                    sAux = (List<ABEifPreparacionDto>)ViewState["abEifPreparacionList_" + Session.SessionID];
                }

                ViewState["abEifPreparacionList_" + Session.SessionID] = sAux;

                return sAux;
            }
            set
            {
                ViewState["abEifPreparacionList_" + Session.SessionID] = value;
            }
        }

        public List<PonentesDuracionActividadDto> ponentesDuracionActividadList
        {
            get
            {
                List<PonentesDuracionActividadDto> sAux = new List<PonentesDuracionActividadDto>();
                if (ViewState["ponentesDuracionActividadList_" + Session.SessionID] != null)
                {
                    sAux = (List<PonentesDuracionActividadDto>)ViewState["ponentesDuracionActividadList_" + Session.SessionID];
                }

                ViewState["ponentesDuracionActividadList_" + Session.SessionID] = sAux;

                return sAux;
            }
            set
            {
                ViewState["ponentesDuracionActividadList_" + Session.SessionID] = value;
            }
        }

        public List<PonentesNivelPSDto> ponentesNivelPSList
        {
            get
            {
                List<PonentesNivelPSDto> sAux = new List<PonentesNivelPSDto>();
                if (ViewState["ponentesNivelPSList_" + Session.SessionID] != null)
                {
                    sAux = (List<PonentesNivelPSDto>) ViewState["ponentesNivelPSList_" + Session.SessionID];
                }

                ViewState["ponentesNivelPSList_" + Session.SessionID] = sAux;
                
                return sAux;
            }
            set
            {
                ViewState["ponentesNivelPSList_" + Session.SessionID] = value;

                System.Web.Script.Serialization.JavaScriptSerializer serializer = new JavaScriptSerializer();
                hiddenPonentesNivelPSList.Value = serializer.Serialize(value);
            }
        }

        public List<PonentesPreparacionDto> ponentesPreparacionList
        {
            get
            {
                List<PonentesPreparacionDto> sAux = new List<PonentesPreparacionDto>();
                if (ViewState["ponentesPreparacionList_" + Session.SessionID] != null)
                {
                    sAux = (List<PonentesPreparacionDto>)ViewState["ponentesPreparacionList_" + Session.SessionID];
                }

                ViewState["ponentesPreparacionList_" + Session.SessionID] = sAux;

                return sAux;
            }
            set
            {
                ViewState["ponentesPreparacionList_" + Session.SessionID] = value;
            }
        }

        public List<TipoReunionDto> tipoReunionList
        {
            get
            {
                List<TipoReunionDto> sAux = new List<TipoReunionDto>();
                if (ViewState["tipoReunionList_" + Session.SessionID] != null)
                {
                    sAux = (List<TipoReunionDto>)ViewState["tipoReunionList_" + Session.SessionID];
                }

                ViewState["tipoReunionList_" + Session.SessionID] = sAux;

                return sAux;
            }
            set
            {
                ViewState["tipoReunionList_" + Session.SessionID] = value;
            }
        }

        public string GetNivelPonentePS()
        {
            PonentesNivelPSService ponentesNivelPS = new PonentesNivelPSService();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(ponentesNivelPS.GetList());
        }

        private void BindDataCalculadora()
        {
            HonorariosMaximosService serviceHonorariosMaximos = new HonorariosMaximosService();
            honorariosMaximosList = serviceHonorariosMaximos.GetList();

            rptHonorariosMaximos.DataSource = honorariosMaximosList.Where(x => x.SpecialCase == false && x.Visible == true).ToList();
            rptHonorariosMaximos.DataBind();

            rptHonorariosMaximosSpecialCase.DataSource = honorariosMaximosList.Where(x => x.SpecialCase == true && x.Visible == true).ToList();
            rptHonorariosMaximosSpecialCase.DataBind();

            rptMaximosNonGlobal.DataSource =
                CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.FindAll(
                    x =>
                        x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Standar.GetHashCode() &&
                        x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode())
                    .OrderBy(x => x.IdNumSpeaksType)
                    .ToList();

            rptMaximosNonGlobal.DataBind();

            rptMaximosGlobal.DataSource =
                CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.FindAll(
                    x =>
                        x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Standar.GetHashCode() &&
                        x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode()
                        )
                    .OrderBy(x => x.IdNumSpeaksType)
                    .ToList();

            rptMaximosGlobal.DataBind();

            rptStandarCountryToCountryNonGlobal.DataSource =
                CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Where(
                    x =>
                        x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Standar.GetHashCode() &&
                        x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                        x.IdNumSpeaksType != EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.Anual.GetHashCode()
                    )
                    .OrderBy(x => x.IdGlobalType).ToList();

            rptStandarCountryToCountryNonGlobal.DataBind();

            rptStandarCountryToCountryGlobal.DataSource =
                CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Where(
                    x =>
                        x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Standar.GetHashCode() &&
                        x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                        x.IdNumSpeaksType != EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.Anual.GetHashCode()
                    )
                    .OrderBy(x => x.IdGlobalType).ToList();

            rptStandarCountryToCountryGlobal.DataBind();

            rptCopCountryToCountryNonGlobal.DataSource =
                CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Where(
                    x =>
                        x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Cop.GetHashCode() &&
                        x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.NonGlobal.GetHashCode() &&
                        x.IdNumSpeaksType != EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.Anual.GetHashCode()
                    )
                    .OrderBy(x => x.IdGlobalType).ToList();

            rptCopCountryToCountryNonGlobal.DataBind();

            rptCopCountryToCountryGlobal.DataSource =
                CountryToCountryObj.CountryToCountryCorrespondenciasDtoList.Where(
                    x =>
                        x.IdType == EOS.ServiceLogic.Enums.ECountryToCountryTypes.Cop.GetHashCode() &&
                        x.IdGlobalType == EOS.ServiceLogic.Enums.ECountryToCountryGlobalTypes.Global.GetHashCode() &&
                        x.IdNumSpeaksType != EOS.ServiceLogic.Enums.ECountryToCountrySpeakersTypes.Anual.GetHashCode()
                    )
                    .OrderBy(x => x.IdGlobalType).ToList();

            rptCopCountryToCountryGlobal.DataBind();

            rptExceptionReasons.DataSource = CountryToCountryObj.CountryToCountryReasonsTypeDtoList;
            rptExceptionReasons.DataBind();

            ABEifDuracionActividadService abEifDuracionActividad = new ABEifDuracionActividadService();
            abEifDuracionActividadList = abEifDuracionActividad.GetList();
            ddlEifABDuracionActividad.DataSource = abEifDuracionActividadList;
            ddlEifABDuracionActividad.DataBind();

            ddlEifABDuracionActividad.Items.Insert(0, blankComboItem);
            ddlEifABDuracionActividad.SelectedValue = " ";

            ABEifNivelPSService abEifNivelPS = new ABEifNivelPSService();
            abEifNivelPSList = abEifNivelPS.GetList();
            ddlEifABTipoPS.DataSource = abEifNivelPSList;
            ddlEifABTipoPS.DataBind();
            var abEifNivelPSVal = abEifNivelPSList.FirstOrDefault();
            txtEifABFeeHora.Text = abEifNivelPSVal.Value.ToString("N2");

            ABEifPreparacionService abEifPreparacion = new ABEifPreparacionService();
            abEifPreparacionList = abEifPreparacion.GetList();
            ddlEifABTiempoPreparacion.DataSource = abEifPreparacionList;
            ddlEifABTiempoPreparacion.DataBind();

            ddlEifABTiempoPreparacion.Items.Insert(0, blankComboItem);
            ddlEifABTiempoPreparacion.SelectedValue = " ";

            PonentesDuracionActividadService ponentesDuracionActividad = new PonentesDuracionActividadService();
            ponentesDuracionActividadList = ponentesDuracionActividad.GetList();
            ddlSpeakerChairDuracionActividad.DataSource = ponentesDuracionActividadList;
            ddlSpeakerChairDuracionActividad.DataBind();

            ddlSpeakerChairDuracionActividad.Items.Insert(0, blankComboItem);
            ddlSpeakerChairDuracionActividad.SelectedValue = " ";

            PonentesNivelPSService ponentesNivelPS = new PonentesNivelPSService();
            ponentesNivelPSList = ponentesNivelPS.GetList();

            ddlSpeakerChairTipoPS.DataSource = ponentesNivelPSList;
            ddlSpeakerChairTipoPS.DataBind();

            ddlSpeakerChairTipoPS.Items.Insert(0, blankComboItem);
            ddlSpeakerChairTipoPS.SelectedValue = " ";

            PonentesPreparacionService ponentesPreparacion = new PonentesPreparacionService();
            ponentesPreparacionList = ponentesPreparacion.GetList();
            ddlSpeakerChairTiempoPreparacion.DataSource = ponentesPreparacionList;
            ddlSpeakerChairTiempoPreparacion.DataBind();

            ddlSpeakerChairTiempoPreparacion.Items.Insert(0, blankComboItem);
            ddlSpeakerChairTiempoPreparacion.SelectedValue = " ";

            TipoReunionService tipoReunion = new TipoReunionService();
            tipoReunionList = tipoReunion.GetList();
            ddlSpeakerChairTipoReunion.DataSource = tipoReunionList;
            ddlSpeakerChairTipoReunion.DataBind();

            ddlSpeakerChairTipoReunion.Items.Insert(0, blankComboItem);
            ddlSpeakerChairTipoReunion.SelectedValue = " ";

            rblConsultoriaTipoReunion.DataSource = TiposContratosConsultoria;
            rblConsultoriaTipoReunion.DataBind();

            TipoPonenteService tipoPonente = new TipoPonenteService();
            rblTipoPonente.DataSource = tipoPonente.GetList();
            rblTipoPonente.DataBind();
        }

        protected void rblTipoPonente_change(object sender, EventArgs e)
        {
            CalculateHonorarios();
        }

        protected void ddlSpeakerChairTipoReunion_Change(object sender, EventArgs e)
        {
            plhTipoPonente.Visible = false;
            long idTipoReunionRegional = 1;
            long idDuracionMenorTreinta = 1;
            PonentesDuracionActividadService ponentesDuracionActividad = new PonentesDuracionActividadService();
            ponentesDuracionActividadList = ponentesDuracionActividad.GetList();
            if (ddlSpeakerChairTipoReunion.SelectedValue == idTipoReunionRegional.ToString())
            {
                plhTipoPonente.Visible = true;
                ddlSpeakerChairDuracionActividad.DataSource =
                    ponentesDuracionActividadList.Where(x => x.id == idDuracionMenorTreinta).ToList();
            }
            else
            {
                ddlSpeakerChairDuracionActividad.DataSource = ponentesDuracionActividadList;
            }

            ddlSpeakerChairDuracionActividad.DataBind();

            ddlSpeakerChairDuracionActividad.Items.Insert(0, blankComboItem);
            ddlSpeakerChairDuracionActividad.SelectedValue = " ";
            
            CalculateHonorarios();
        }

        protected void ddlSpeakerChairTipoPS_Change(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddlSpeakerChairTipoPS.SelectedValue))
            {
                txtSpeakerChairFeeHora.Text =
                    ponentesNivelPSList.Find(x => x.id.ToString() == ddlSpeakerChairTipoPS.SelectedValue)
                        .Value.ToString();
            }
            else
            {
                txtSpeakerChairFeeHora.Text = string.Empty;
            }
            CalculateHonorarios();
        }

        protected void ddlSpeakerChairDuracionActividad_Change(object sender, EventArgs e)
        {
            CalculateHonorarios();
        }

        protected void ddlSpeakerChairTiempoPreparacion_Change(object sender, EventArgs e)
        {
            CalculateHonorarios();
        }

        protected void ddlEifABDuracionActividad_Change(object sender, EventArgs e)
        {
            CalculateHonorarios();
        }

        protected void ddlEifABTiempoPreparacion_Change(object sender, EventArgs e)
        {
            CalculateHonorarios();
        }

        protected void chkBoxPonenciaCentroSalud_Change(object sender, EventArgs e)
        {
            if (chkBoxPonenciaCentroSalud.Checked)
            {
                chkBoxTalleres.Checked = false;
                chkBoxVideoconferenciasRepetidas.Checked = false;
            }
            CalculateHonorarios();
        }

        protected void chkBoxTalleres_Change(object sender, EventArgs e)
        {
            if (chkBoxTalleres.Checked)
            {
                chkBoxPonenciaCentroSalud.Checked = false;
                chkBoxVideoconferenciasRepetidas.Checked = false;
            }
            CalculateHonorarios();
        }

        protected void chkBoxVideoconferenciasRepetidas_Change(object sender, EventArgs e)
        {
            if (chkBoxVideoconferenciasRepetidas.Checked)
            {
                chkBoxPonenciaCentroSalud.Checked = false;
                chkBoxTalleres.Checked = false;
            }
            CalculateHonorarios();
        }

        protected void rblConsultoriaTipoReunion_Change(object sender, EventArgs e)
        {
            txtConsultoriaDiaConsultor.Text = string.Empty;
            plhTxtConsultoriaDiaConsultor.Visible =
                TiposContratosConsultoria.Find(x => x.Value.ToString() == rblConsultoriaTipoReunion.SelectedItem.Value)
                    .CampoDependiente;
            if (plhTxtConsultoriaDiaConsultor.Visible)
            {
                var correspondencia = CorrespondenciasList.Find(
                    x =>
                        x.IdTipoAsistente.GetValueOrDefault().ToString() ==
                        EAsistenteTypes.Consultor.GetHashCode().ToString() && x.IdTipoContratoConsultoria == 1);
                hidFieldValueHonorariosMaximosConsultoria.Value = honorariosMaximosList.Find(x => x.id == correspondencia.IdCalcHonorariosMaximos).FloatValue.ToString();
                hidFieldIdValueHonorariosMaximosConsultoria.Value = honorariosMaximosList.Find(x => x.id == correspondencia.IdCalcHonorariosMaximos).id.ToString();
                hidFieldIdValueHonorariosMaximos.Value = honorariosMaximosList.Find(x => x.id == correspondencia.IdCalcHonorariosMaximos).id.ToString();
                txtConsultoriaHonorariosMaximos.Text = string.Empty;
                txthonorarios.Text = string.Empty;
            }
            else
            {
                var correspondencia = CorrespondenciasList.Find(
                    x =>
                        x.IdTipoAsistente.GetValueOrDefault().ToString() ==
                        EAsistenteTypes.Consultor.GetHashCode().ToString() && x.IdTipoContratoConsultoria == 2);
                hidFieldValueHonorariosMaximosConsultoria.Value = honorariosMaximosList.Find(x => x.id == correspondencia.IdCalcHonorariosMaximos).FloatValue.ToString();
                hidFieldIdValueHonorariosMaximosConsultoria.Value = honorariosMaximosList.Find(x => x.id == correspondencia.IdCalcHonorariosMaximos).id.ToString();
                hidFieldValueHonorariosMaximos.Value = honorariosMaximosList.Find(x => x.id == correspondencia.IdCalcHonorariosMaximos).FloatValue.ToString();
                hidFieldIdValueHonorariosMaximos.Value = honorariosMaximosList.Find(x => x.id == correspondencia.IdCalcHonorariosMaximos).id.ToString();
                txtConsultoriaHonorariosMaximos.Text = hidFieldValueHonorariosMaximosConsultoria.Value;
                txthonorarios.Text = honorariosMaximosList.Find(x => x.id == correspondencia.IdCalcHonorariosMaximos).FloatValue.ToString(CultureInfo.GetCultureInfo("en-GB"));
            }
            updPanelConsultoriaHonorariosMaximos.Update();
            updHonorarioMaximo.Update();
        }

        private void CalculateHonorarios()
        {
            double honorarios;
            long idcalchonorarios = -1;
            
            //var honorariosMaximosSorted = honorariosMaximosList.Where(x => x.FloatValue > 0).OrderBy(x => x.FloatValue);
            //var honorarioMaximo = honorariosMaximosSorted.Last().FloatValue;
            //var honorarioMinimo = honorariosMaximosSorted.First().FloatValue;

            plhCalculadoraSemaforoVerde.Visible = false;
            plhCalculadoraSemaforoNaranja.Visible = false;
            plhCalculadoraSemaforoRojo.Visible = false;

            switch (ddlTipoAsistente.SelectedValue)
            {
                case "2":
                case "3":
                case "6":
                case "9":
                    if (chkBoxPonenciaCentroSalud.Checked)
                    {
                        //Ponencia en centro de salud
                        idcalchonorarios = CorrespondenciasList.Find(
                                x =>
                                    x.IdTipoAsistente.GetValueOrDefault().ToString() == ddlTipoAsistente.SelectedValue &&
                                    x.PonenciaCentroSalud.GetValueOrDefault() == chkBoxPonenciaCentroSalud.Checked)
                                .IdCalcHonorariosMaximos;
                        var honorario = honorariosMaximosList.Find(x => x.id == idcalchonorarios);
                        txtSpeakerChairHonorariosMaximos.Text = honorario.FloatValue.ToString("N2");
                        txthonorarios.Text = honorario.FloatValue.ToString(CultureInfo.GetCultureInfo("en-GB"));
                        updPanelSpeakerHonorariosMaximos.Update();
                        break;
                    }
                    if (chkBoxTalleres.Checked)
                    {
                        //Talleres
                        idcalchonorarios = CorrespondenciasList.Find(
                                x =>
                                    x.IdTipoAsistente.GetValueOrDefault().ToString() == ddlTipoAsistente.SelectedValue &&
                                    x.TallerOCurso.GetValueOrDefault() == chkBoxTalleres.Checked)
                                .IdCalcHonorariosMaximos;
                        var honorario = honorariosMaximosList.Find(x => x.id == idcalchonorarios);
                        txtSpeakerChairHonorariosMaximos.Text = honorario.FloatValue.ToString("N2");
                        txthonorarios.Text = honorario.FloatValue.ToString(CultureInfo.GetCultureInfo("en-GB"));
                        updPanelSpeakerHonorariosMaximos.Update();
                        break;
                    }
                    if (chkBoxVideoconferenciasRepetidas.Checked)
                    {
                        //Videoconferencia repetida
                        idcalchonorarios = CorrespondenciasList.Find(
                                x =>
                                    x.IdTipoAsistente.GetValueOrDefault().ToString() == ddlTipoAsistente.SelectedValue &&
                                    x.VideoconferenciaRepetida.GetValueOrDefault() == chkBoxVideoconferenciasRepetidas.Checked)
                                .IdCalcHonorariosMaximos;
                        var honorario = honorariosMaximosList.Find(x => x.id == idcalchonorarios);
                        txtSpeakerChairHonorariosMaximos.Text = honorario.FloatValue.ToString("N2");
                        txthonorarios.Text = honorario.FloatValue.ToString(CultureInfo.GetCultureInfo("en-GB"));
                        updPanelSpeakerHonorariosMaximos.Update();
                        break;
                    }
                    if (ddlSpeakerChairTipoReunion.SelectedValue =="3" || ddlSpeakerChairTipoReunion.SelectedValue =="4")
                    {
                        idcalchonorarios = CorrespondenciasList.Find(
                                x =>
                                    x.IdTipoAsistente.GetValueOrDefault().ToString() == ddlTipoAsistente.SelectedValue &&
                                    x.IdTipoReunion.ToString() == ddlSpeakerChairTipoReunion.SelectedValue)
                                .IdCalcHonorariosMaximos;
                        var honorario = honorariosMaximosList.Find(x => x.id == idcalchonorarios);
                        txtSpeakerChairHonorariosMaximos.Text = honorario.FloatValue.ToString("N2");
                        txthonorarios.Text = honorario.FloatValue.ToString(CultureInfo.GetCultureInfo("en-GB"));
                        updPanelSpeakerHonorariosMaximos.Update();
                        break;
                    }
                    else if (
                        !String.IsNullOrWhiteSpace(ddlSpeakerChairTipoPS.SelectedValue) &&
                        !String.IsNullOrWhiteSpace(ddlSpeakerChairDuracionActividad.SelectedValue) &&
                        !String.IsNullOrWhiteSpace(ddlSpeakerChairTiempoPreparacion.SelectedValue)
                        )
                    {

                        var correspondencia = CorrespondenciasList.Find(x =>
                            (x.IdTipoAsistente.ToString() == ddlTipoAsistente.SelectedValue) &&
                            (x.IdTipoReunion.ToString() == ddlSpeakerChairTipoReunion.SelectedValue) &&
                            (!plhTipoPonente.Visible || (x.IdTipoNacionalLocal == null || x.IdTipoNacionalLocal.ToString() == rblTipoPonente.SelectedValue)) &&
                            (x.IdDuracionActividadPonentes == null || x.IdDuracionActividadPonentes.ToString() == ddlSpeakerChairDuracionActividad.SelectedValue)
                            //&& x.IdTiempoPreparacionPonentes.ToString() == ddlSpeakerChairTiempoPreparacion.SelectedValue
                            );

                        if (correspondencia != null)
                        {
                            idcalchonorarios = correspondencia.IdCalcHonorariosMaximos;

                            honorarios =
                                ponentesNivelPSList.Find(x => x.id.ToString() == ddlSpeakerChairTipoPS.SelectedValue)
                                    .Value*
                                (
                                    ponentesDuracionActividadList.Find(
                                        x => x.id.ToString() == ddlSpeakerChairDuracionActividad.SelectedValue).Value
                                    +
                                    ponentesPreparacionList.Find(
                                        x => x.id.ToString() == ddlSpeakerChairTiempoPreparacion.SelectedValue).Value
                                    );
                            txtSpeakerChairHonorariosMaximos.Text = honorarios.ToString("N2");
                            txthonorarios.Text = honorarios.ToString(CultureInfo.GetCultureInfo("en-GB"));
                            var honorarioMaximo =
                                honorariosMaximosList.Find(x => x.id == correspondencia.IdCalcHonorariosMaximos)
                                    .FloatValue;

                            if (honorarios <= honorarioMaximo)
                            {
                                plhCalculadoraSemaforoVerde.Visible = true;
                            }
                            else if (honorarios > honorarioMaximo)
                            {
                                plhCalculadoraSemaforoRojo.Visible = true;
                            }
                            else
                            {
                                plhCalculadoraSemaforoNaranja.Visible = true;
                            }
                        }
                        else
                        {
                            idcalchonorarios = -1;
                        }
                    }
                    else
                    {
                        idcalchonorarios = -1;
                        txthonorarios.Text = string.Empty;
                        txtSpeakerChairHonorariosMaximos.Text = string.Empty;
                    }
                    updPanelSpeakerHonorariosMaximos.Update();
                    break;
                case "10":
                    var correspondenciaConsultoria = CorrespondenciasList.Find(x =>
                            (x.IdTipoAsistente.ToString() == ddlTipoAsistente.SelectedValue) &&
                            (x.IdTipoContratoConsultoria.ToString() == rblConsultoriaTipoReunion.SelectedValue)
                            );

                    if (correspondenciaConsultoria != null)
                    {
                        idcalchonorarios = correspondenciaConsultoria.IdCalcHonorariosMaximos;
                    }
                    else
                    {
                        idcalchonorarios = -1;
                    }
                    break;
                case "7":
                case "8":
                    if (
                        !String.IsNullOrWhiteSpace(ddlEifABTipoPS.SelectedValue) &&
                        !String.IsNullOrWhiteSpace(ddlEifABDuracionActividad.SelectedValue) &&
                        !String.IsNullOrWhiteSpace(ddlEifABTiempoPreparacion.SelectedValue)
                        )
                    {

                        idcalchonorarios = CorrespondenciasList.Find(
                                x =>
                                    x.IdTipoAsistente.GetValueOrDefault().ToString() == ddlTipoAsistente.SelectedValue)
                                .IdCalcHonorariosMaximos;

                        honorarios = abEifNivelPSList.Find(x => x.id.ToString() == ddlEifABTipoPS.SelectedValue).Value *
                                     (
                                         abEifDuracionActividadList.Find(x => x.id.ToString() == ddlEifABDuracionActividad.SelectedValue).Value
                                         +
                                         abEifPreparacionList.Find(x => x.id.ToString() == ddlEifABTiempoPreparacion.SelectedValue).Value
                                     );

                        if (ddlTipoAsistente.SelectedValue == "7")
                        {
                            honorarios += float.Parse(litChairmanAditionalEuros.Text);
                        }
                        txtEifABHonorariosMaximos.Text = honorarios.ToString("N2");
                        txthonorarios.Text = honorarios.ToString(CultureInfo.GetCultureInfo("en-GB"));
                    }
                    else
                    {
                        txtEifABHonorariosMaximos.Text = string.Empty;
                        txthonorarios.Text = string.Empty;
                    }
                    updPanelEifAbHonorariosMaximos.Update();
                    break;
                default:
                    idcalchonorarios = -1;
                    txtSpeakerChairHonorariosMaximos.Text = string.Empty;
                    txtEifABHonorariosMaximos.Text = string.Empty;
                    txthonorarios.Text = string.Empty;
                    break;

            }

            hidFieldValueHonorariosMaximos.Value = string.Empty;

            if (idcalchonorarios > 0)
            {
                hidFieldValueHonorariosMaximos.Value = honorariosMaximosList.Find(x => x.id == idcalchonorarios).FloatValue.ToString();
                hidFieldIdValueHonorariosMaximos.Value = idcalchonorarios.ToString();
            }
            updHonorarioMaximo.Update();
        }

        public void LimpiarCamposCalculadora()
        {
            plhCalculadoraSemaforoVerde.Visible = false;
            plhCalculadoraSemaforoNaranja.Visible = false;
            plhCalculadoraSemaforoRojo.Visible = false;

            ddlEifABDuracionActividad.SelectedValue = " ";
            ddlEifABTiempoPreparacion.SelectedValue = " ";
            ddlSpeakerChairDuracionActividad.SelectedValue = " ";
            ddlSpeakerChairTipoPS.SelectedValue = " ";
            ddlSpeakerChairTiempoPreparacion.SelectedValue = " ";
            ddlSpeakerChairTipoReunion.SelectedValue = " ";
        }

        #endregion

    }
}
