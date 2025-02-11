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
    public partial class Calculadora : System.Web.UI.Page
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

        public Calculadora()
        {
            _correspondenciasService = new CorrespondenciasService();
            _countryToCountryService = new CountryToCountryService();
            _tipoContratoContratoConsultoriaService = new TipoContratoConsultoriaService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ///////////////Controlar Si caduca la sesión en la aplicación//////////////
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Session["rolesuser"] == null) Response.Redirect("~/Account/Login.aspx");
            //////////////////////////////////////////////////////////////////////////

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            hiddenPonentesNivelPSList.Value = GetNivelPonentePS();

            if (!Page.IsPostBack)
            {
                BindDataCalculadora();
                RellenaCombos();
            }
        }

        public string GetJsonObject(object data)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize((DVParticipante)data);
        }    
        
        private void RellenaCombos()
        {
            try
            {
                AgenteExpedientes agExpedientes = new AgenteExpedientes();

                this.ddlTipoAsistente.DataSource = agExpedientes.ObtenerTiposAsistenteConCalculadora();
                this.ddlTipoAsistente.DataBind();
                ListItem blankTipoAsisitente = new ListItem("Selecciona Tipo de Participante", " ");
                ddlTipoAsistente.Items.Insert(0, blankTipoAsisitente);
                this.ddlTipoAsistente.SelectedValue = " ";
            }
            catch (Exception err)
            {
                Global.SendApplicationError(err, Request, Session, GetType().Name);
                Alert.Show("Message2:" + err.Message.ToString() + " / Target:" + err.TargetSite.ToString() + " / Source:" + err.Source);
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
                    }
                    else
                    {
                        txtEifABHonorariosMaximos.Text = string.Empty;
                    }
                    updPanelEifAbHonorariosMaximos.Update();
                    break;
                default:
                    idcalchonorarios = -1;
                    txtSpeakerChairHonorariosMaximos.Text = string.Empty;
                    txtEifABHonorariosMaximos.Text = string.Empty;
                    break;

            }
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
