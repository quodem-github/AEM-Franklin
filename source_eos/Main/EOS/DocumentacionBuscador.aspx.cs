using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Entidades.Datos;
using EOS.Entidades.Maestros;
using EOS.Web;
using NPOI.SS.Formula.Functions;
using Quodem.Utility.Json;

namespace EOS
{
    public partial class DocumentacionBuscador : System.Web.UI.Page
    {
        #region Clases

        protected class FiltroExpedientes
        {
            private string _importe;

            public string Actividad { get; set; }

            public string Asistente { get; set; }

            public string EstadoExpediente { get; set; }

            public string Amec { get; set; }

            public string EstadoReserva { get; set; }

            public string Tipo { get; set; }

            public string Anyo { get; set; }

            public string Mes { get; set; }

            public string FechaDesde { get; set; }

            public string FechaHasta { get; set; }

            public string Unidad { get; set; }

            public string Producto { get; set; }

            public string AreaNegocio { get; set; }

            public string Region { get; set; }

            public string Distrito { get; set; }

            public string Peticionario { get; set; }

            public string ProveedorServicio { get; set; }

            public string TipoImporteItem { get; set; }

            public string TipoImporteValue { get; set; }

            public string IdDistrict { get; set; }

            public string IdSaleForce { get; set; }

            public string IdDepartament { get; set; }

            public int IdPeticionarioSession { get; set; }

            public string TipoActividad { get; set; }

            public string Importe
            {
                get
                {
                    decimal dCantidad = 0;

                    decimal.TryParse(_importe, out dCantidad);

                    return dCantidad.ToString();
                }

                set
                {
                    _importe = value;
                }
            }

            public string NumExpediente { get; set; }

            public string sortParameter { get; set; }

            public int startRowIndex { get; set; }

            public Boolean filtroAvanzadoActivado { get; set; }

            public FiltroExpedientes()
            {
            }

            public FiltroExpedientes(string actividad, string asistente, string estado, string amec, string estadoReserva, string tipo, string anyo, string mes, string fechaDesde, string fechaHasta
                , string unidad, string producto, string areaNegocio, string region, string distrito, string peticionario, string proveedor, string tipoImporteItem, string tipoImporteValue,
                string importe, string numExpediente,string idDepartament,string idSaleForce, string idDistrict,int idPeticionarioSession,string tipoActividad)
            {
                Actividad = actividad;
                Asistente = asistente;
                EstadoExpediente = estado;
                Amec = amec;
                EstadoReserva = estadoReserva;
                Tipo = tipo;
                Anyo = anyo;
                Mes = mes;
                FechaDesde = fechaDesde;
                FechaHasta = fechaHasta;
                Unidad = unidad;
                Producto = producto;
                AreaNegocio = areaNegocio;
                Region = region;
                Distrito = distrito;
                Peticionario = peticionario;
                ProveedorServicio = proveedor;
                TipoImporteItem = tipoImporteItem;
                TipoImporteValue = tipoImporteValue;
                Importe = importe;
                NumExpediente = numExpediente;
                IdSaleForce = idSaleForce;
                IdDistrict = idDistrict;
                IdDepartament = idDepartament;
                IdPeticionarioSession = idPeticionarioSession;
                TipoActividad = tipoActividad;
                startRowIndex = 0;
            }

            public Boolean TieneFiltroTipo { get; set; }

            public Boolean TieneFiltroAnyo { get; set; }

            public Boolean TieneFiltroMes { get; set; }

            public Boolean TieneTipoImporte { get; set; }

            public Boolean TieneFechaDesde()
            {
                return tieneFecha(FechaDesde);
            }

            public Boolean TieneFechaHasta()
            {
                return tieneFecha(FechaHasta);
            }

            private Boolean tieneFecha(string txtFecha)
            {
                DateTime dt;
                return (DateTime.TryParse(txtFecha, out dt) ? true : false);
            }
        }

        #endregion Clases

        #region Variables

        private DDatosPersonalesUsuario _datosUsuario;
        public bool m_bSelect = false;
        public int nBack = 0;
        private List<DDepartament> OriginalDeptList;
        private List<DDistrict> OriginalDistList;
        private List<DSaleForce> OriginalSaleForceList;
        public Boolean blnMostrarFiltroAvanzado = false;

        #endregion Variables

        #region Eventos

        #region Page

        protected void Page_Load(object sender, EventArgs e)
        {
            ///////////////Controlar Si caduca la sesión en la aplicación//////////////
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Session["rolesuser"] == null) Response.Redirect("~/Account/Login.aspx");
            //////////////////////////////////////////////////////////////////////////
            _datosUsuario = (DDatosPersonalesUsuario)Session["datosUsuario"];
            FiltroExpedientes oFiltroExpedientes;
            this.Form.DefaultButton = this.ImageButton2.UniqueID;
            Session.Remove("idExpedienteSeleccionado");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            EOS.Repositorios.Constantes.ConsultaNumExpedientes = null; 
            EOS.Repositorios.Constantes.ConsultaTotalExpedientes = null;

            /* Activa unas áreas u otras si estamos en modo normal o en modo selección */
            m_bSelect = (Request.QueryString["idsel"] != null);

            if (!Page.IsPostBack)
            {
                //if (datosUsuario.admin.HasValue)
                //    ddlEstadoReserva.SelectedIndex = 0;
                nBack = -1;
                Session.Add("ExpHistoryBack", nBack);

                ContentPlaceHolder mpContentPlaceHolder;
                mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("eosContentStatus");
                if (mpContentPlaceHolder != null)
                {
                    mpContentPlaceHolder.Visible = !m_bSelect;
                }
                mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("eosHeaderBotonera");
                if (mpContentPlaceHolder != null)
                {
                    mpContentPlaceHolder.Visible = !m_bSelect;
                }
                this.btnCancelar.Visible = m_bSelect;
                this.eosContentStatusTitulo.Visible = m_bSelect;

                AgenteExpedientes agExp = new AgenteExpedientes();
                try
                {
                    ICollection<DVResumenEstados> lResEstados = agExp.ObtenerResumenEstados();

                    //Ismael Ameller 08-03-2011 Sacamos los Labels de la BBDD
                    this.lblSinEnviar.Text = string.Format("{0}|{1}", agExp.ObtenerLabelEstado("AB"), agExp.GetTotalEstado(lResEstados, "AB"));
                    this.lblEnviado.Text = string.Format("{0}|{1}", agExp.ObtenerLabelEstado("CR"), agExp.GetTotalEstado(lResEstados, "CR"));
                    this.lblCotizando.Text = string.Format("{0}|{1}", agExp.ObtenerLabelEstado("CTZD"), agExp.GetTotalEstado(lResEstados, "CTZD"));
                    this.lblCotizado.Text = string.Format("{0}|{1}", agExp.ObtenerLabelEstado("CTZ"), agExp.GetTotalEstado(lResEstados, "CTZ"));
                    this.lblAceptado.Text = string.Format("{0}|{1}", agExp.ObtenerLabelEstado("CFP"), agExp.GetTotalEstado(lResEstados, "CFP"));
                    this.lblTramitando.Text = string.Format("{0}|{1}", agExp.ObtenerLabelEstado("PTR"), agExp.GetTotalEstado(lResEstados, "PTR"));
                    this.lblTramitado.Text = string.Format("{0}|{1}", agExp.ObtenerLabelEstado("TR"), agExp.GetTotalEstado(lResEstados, "TR"));
                    this.lblRechazado.Text = string.Format("{0}|{1}", agExp.ObtenerLabelEstado("AN"), agExp.GetTotalEstado(lResEstados, "AN"));
                    long totalCancelados = agExp.GetTotalEstado(lResEstados, "CN") + agExp.GetTotalEstado(lResEstados, "CNTR");
                    this.lblCancelado.Text = string.Format("{0}|{1}", agExp.ObtenerLabelEstado("CN"), totalCancelados);      
                }
                catch (Exception ex)
                {
                    Global.SendApplicationError(ex, Request, Session, GetType().Name);

                    this.lblSinEnviar.Text = string.Format("{0}|{1}", agExp.ObtenerLabelEstado("AB"), "--");
                    this.lblEnviado.Text = string.Format("{0}|{1}", agExp.ObtenerLabelEstado("CR"), "--");
                    this.lblCotizando.Text = string.Format("{0}|{1}", agExp.ObtenerLabelEstado("CTZD"), "--");
                    this.lblCotizado.Text = string.Format("{0}|{1}", agExp.ObtenerLabelEstado("CTZ"), "--");
                    this.lblAceptado.Text = string.Format("{0}|{1}", agExp.ObtenerLabelEstado("CFP"), "--");
                    this.lblTramitando.Text = string.Format("{0}|{1}", agExp.ObtenerLabelEstado("PTR"), "--");
                    this.lblTramitado.Text = string.Format("{0}|{1}", agExp.ObtenerLabelEstado("TR"), "--");
                    this.lblRechazado.Text = string.Format("{0}|{1}", agExp.ObtenerLabelEstado("AN"), "--");
                    this.lblCancelado.Text = string.Format("{0}|{1}", agExp.ObtenerLabelEstado("CN"), "--");   
                }
        
                if (Session[Constantes.Session.oFiltroExpedientesDoc] == null)
                {
                    //Es la primera vez y gaurdamos el filtro en session
                    guardarFiltroExpedientesEnSesion();
                }

                oFiltroExpedientes = (FiltroExpedientes)Session[Constantes.Session.oFiltroExpedientesDoc];
                llenarFiltro(oFiltroExpedientes);

               
                
            }
            else
            {
                // OJO: Parece que si se deja este DataBind no funcionan los avances izquierda y derecha del ListView
                //lvExpedientes.DataBind();

                nBack = (int)Session["ExpHistoryBack"] - 1;
            }

            Session["ExpHistoryBack"] = nBack;
            this.btnCancelar.OnClientClick = string.Format("history.go({0});return false;", nBack);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
           AgenteUsuarios au = new AgenteUsuarios();

            int? idUserRegion = ((EOS.Entidades.Datos.DVPeticionariosRoles)Session["rolesuser"]).idregion;
            int? idUserDistrito = ((EOS.Entidades.Datos.DVPeticionariosRoles)Session["rolesuser"]).iddistrito;

            if ((au.IsAdmin() || (idUserRegion == null && idUserDistrito == null)) && !IsPostBack)
            {
                lvExpedientes.Visible = false;
            }
            else
            {
                if (!IsPostBack)
                {
                    this.lvExpedientes.DataBind();
                }
                lvExpedientes.Visible = true;
            }
            
        }

        #endregion Page

        #region lvExpedientes

        protected void lvExpedientes_Load(object sender, EventArgs e)
        {
            FiltroExpedientes oFiltroExp;
            DataPager oDataPager = getDataPager();

            if (oDataPager != null)
            {
                oFiltroExp = getFiltroExpedientes();
                oDataPager.SetPageProperties(oFiltroExp.startRowIndex, oDataPager.PageSize, true);
            }
        }

        protected void lvExpedientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nIDExpediente = 0;
            try
            {
                if (this.lvExpedientes.SelectedValue != null)
                {
                    nIDExpediente = int.Parse(lvExpedientes.SelectedValue.ToString());
                    AgenteExpedientes agExp = new AgenteExpedientes();
                    //Xavier Morell
                    DDatosPersonalesUsuario datos = (DDatosPersonalesUsuario)Session["datosUsuario"];
                    //int nIDNuevoExpediente = agExp.CrearCopiaExpediente(nIDExpediente, new AgenteUsuarios().ObtenerDatosPersonalesPorLogin());
                    int nIDNuevoExpediente = agExp.CrearCopiaExpediente(nIDExpediente, datos);
                    Alert.Show(string.Format("El Expediente {0} se ha copiado correctamente y se ha creado el nuevo expediente {1}", nIDExpediente, nIDNuevoExpediente), string.Format("NuevoExpedientePasoA.aspx?idexp={0}&back=det", nIDNuevoExpediente));
                }
            }
            catch (Exception ex)
            {
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
                //Ismael Ameller 09-03-2011 Envio de Mail
                //Mail mail = new Mail();
                //mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Alert.Show(string.Format("Se ha producido un error en la grabación del Expediente {0}. Error: {1}", nIDExpediente, ex.Message), null);
            }
        }

        protected void imgExpedienteInformes_Command(object sender, CommandEventArgs e)
        {
            string sufix = DateTime.Now.ToString("mmss");
            string query = "key=" + HttpUtility.UrlEncode(EOS.Web.Encriptacion.Encrypt(e.CommandName, "Documents" + sufix) + sufix);
            Response.Redirect("DocumentosExpediente.aspx?" + query);
        }

        protected void lvExpedientes_Sorting(object sender, ListViewSortEventArgs e)
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

            LinkButton lbNExp = (LinkButton)this.lvExpedientes.FindControl("lbNExp");
            LinkButton lbAmec = (LinkButton)this.lvExpedientes.FindControl("lbAmec");
            LinkButton lbFecha = (LinkButton)this.lvExpedientes.FindControl("lbFecha");
            LinkButton lbActividad = (LinkButton)this.lvExpedientes.FindControl("lbActividad");
            LinkButton lbTotal = (LinkButton)this.lvExpedientes.FindControl("lbTotal");

            lbNExp.CssClass = string.Empty;
            lbAmec.CssClass = string.Empty;
            lbFecha.CssClass = string.Empty;
            lbActividad.CssClass = string.Empty;
            lbTotal.CssClass = string.Empty;

            LinkButton target = null;
            switch (e.SortExpression)
            {
                case "IDEXPEDIENTE":
                    target = lbNExp;
                    break;
                case "AMEC":
                    target = lbAmec;
                    break;
                case "FECHACREACION":
                    target = lbFecha;
                    break;
                case "ACTIVIDAD":
                    target = lbActividad;
                    break;
                case "IMPORTE":
                    target = lbTotal;
                    break;
            }
            if (target != null)
            {
                target.CssClass = strCssClass;
            }
        }

        #endregion lvExpedientes

        #region Datasorce_Expedientes

        protected void odsExpediente_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            FiltroExpedientes oFiltroExp = getFiltroExpedientes();

            if (oFiltroExp != null)
            {
                if (!Page.IsPostBack)
                {
                    e.Arguments.StartRowIndex = oFiltroExp.startRowIndex;
                    e.Arguments.SortExpression = oFiltroExp.sortParameter;
                }
                else
                {
                    oFiltroExp.startRowIndex = e.Arguments.StartRowIndex;
                    oFiltroExp.sortParameter = e.Arguments.SortExpression;
                }

                insertarParametrosFiltro(ref e, oFiltroExp);
            }
        }

        #endregion Datasorce_Expedientes

        #region Botones

        /// <summary>
        /// Evento filtrar expedientes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButton2_Click(object sender, EventArgs e)
        {
            guardarFiltroExpedientesEnSesion();
            lvExpedientes.DataBind();
        }

        #endregion Botones

        #endregion Eventos

        #region metodos

        

        protected void guardarFiltroExpedientesEnSesion()
        {
            var numExpedienteCalc = txtNumExpediente.Text.Trim();
            AgenteUsuarios au = new AgenteUsuarios();
            if (au.IsAdmin() && !IsPostBack)
            {
                numExpedienteCalc = "-100";
            }
            FiltroExpedientes oFiltroExpedientes = new FiltroExpedientes("", txtAsistente.Text.Trim(), "", txtAMEC.Text.Trim(),
                "", "", "", "", "", "",
                "", "", "", "", "", "", "",
                "", "", "", numExpedienteCalc,"","","",_datosUsuario.IdPeticionario,txtTipoActividad.Text);

            oFiltroExpedientes.TieneFiltroTipo = false;
            oFiltroExpedientes.TieneFiltroAnyo = false;
            oFiltroExpedientes.TieneFiltroMes = false;
            oFiltroExpedientes.TieneTipoImporte = false;

            oFiltroExpedientes.filtroAvanzadoActivado = false;

            Session[Constantes.Session.oFiltroExpedientesDoc] = oFiltroExpedientes;
        }

        protected void insertarParametrosFiltro(ref ObjectDataSourceSelectingEventArgs e, FiltroExpedientes oFiltroExp)
        {
            AgenteUsuarios au = new AgenteUsuarios();

            int? idUserRegion = ((EOS.Entidades.Datos.DVPeticionariosRoles)Session["rolesuser"]).idregion;
            int? idUserDistrito = ((EOS.Entidades.Datos.DVPeticionariosRoles)Session["rolesuser"]).iddistrito;

            if ((au.IsAdmin() || (idUserRegion == null && idUserDistrito == null)) && !IsPostBack)
            {
                e.InputParameters["disableBinding"] = true;
            }
            else
            {
                e.InputParameters["disableBinding"] = false;
            }

            if (!string.IsNullOrEmpty(oFiltroExp.Asistente)) e.InputParameters["filtroAsistente"] = "%" + oFiltroExp.Asistente + "%";
            if (!string.IsNullOrEmpty(oFiltroExp.Amec)) e.InputParameters["filtroAmec"] = "%" + oFiltroExp.Amec + "%";
            if (!string.IsNullOrEmpty(oFiltroExp.TipoActividad)) e.InputParameters["filtroTipoActividad"] = "%" + oFiltroExp.TipoActividad + "%";
            if (_datosUsuario != null && _datosUsuario.IdPeticionario > 0) e.InputParameters["filtroIdPeticionarioSession"] = oFiltroExp.IdPeticionarioSession;
            if (!string.IsNullOrEmpty(txtNumExpediente.Text.Trim()))
            {
                e.InputParameters["filtroNumExpediente"] = txtNumExpediente.Text.Trim(); ;
            }
        }

   

        private void llenarFiltro(FiltroExpedientes oFiltroExp)
        {
           txtAsistente.Text = oFiltroExp.Asistente;
            txtAMEC.Text = oFiltroExp.Amec;
            txtTipoActividad.Text = oFiltroExp.TipoActividad;
            if (oFiltroExp.NumExpediente != "-100")
            {
                txtNumExpediente.Text = oFiltroExp.NumExpediente;
            }
            
        }

       

        private FiltroExpedientes getFiltroExpedientes()
        {
            return (FiltroExpedientes)Session[Constantes.Session.oFiltroExpedientesDoc];
        }

        private DataPager getDataPager()
        {
            return (DataPager)lvExpedientes.FindControl("DataPager3");
        }

        #endregion metodos

       
    }
}