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
    public partial class Expedientes : System.Web.UI.Page
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
                startRowIndex = 0;
                TipoActividad = tipoActividad;
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
                    //mpContentPlaceHolder.Visible = !m_bSelect;
                }
                mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("eosHeaderBotonera");
                if (mpContentPlaceHolder != null)
                {
                    //mpContentPlaceHolder.Visible = !m_bSelect;
                }
                //this.btnCancelar.Visible = m_bSelect;
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
        

                //FIN Ismael Ameller 08-03-2011 Sacamos los Labels de la BBDD
                RellenarCombos();

                /***************************************/
                /* Cargar filtro expdientes de session */
                /***************************************/

                if (Session[Constantes.Session.oFiltroExpedientes] == null)
                {
                    //Es la primera vez y gaurdamos el filtro en session
                    guardarFiltroExpedientesEnSesion();
                }

                oFiltroExpedientes = (FiltroExpedientes)Session[Constantes.Session.oFiltroExpedientes];
                llenarFiltro(oFiltroExpedientes);

                if (oFiltroExpedientes.filtroAvanzadoActivado)
                {
                    mostrarFiltroAvanzado();
                }
                
            }
            else
            {
                // OJO: Parece que si se deja este DataBind no funcionan los avances izquierda y derecha del ListView
                //lvExpedientes.DataBind();

                nBack = (int)Session["ExpHistoryBack"] - 1;
            }

            Session["ExpHistoryBack"] = nBack;
            //this.btnCancelar.OnClientClick = string.Format("history.go({0});return false;", nBack);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //if (Session["rolesuser"] == null) Response.Redirect("~/Account/Login.aspx");

            if (this.txtFiltroAvanzadoEstado.Value == "1")
                this.eosFiltroAvanzado.Style.Add("display", "inline");
            else this.eosFiltroAvanzado.Style.Add("display", "none");
            AgenteUsuarios au = new AgenteUsuarios();

            int? idUserRegion = (EOS.Entidades.Datos.DVPeticionariosRoles) Session["rolesuser"] != null ? ((EOS.Entidades.Datos.DVPeticionariosRoles) Session["rolesuser"]).idregion : (int?) null;
            int? idUserDistrito = (EOS.Entidades.Datos.DVPeticionariosRoles) Session["rolesuser"] != null ? ((EOS.Entidades.Datos.DVPeticionariosRoles) Session["rolesuser"]).iddistrito : (int?) null;

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

        protected bool CheckIdAmec(object amec)
        {
            return amec.ToString().ToCharArray()[0] == 4;
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



        protected void validarCampos()
        {
            DateTime dtTest;
            Decimal dCantidad = 0;

            if (!(string.IsNullOrEmpty(txtFechaDesde.Text.Trim())) && !(DateTime.TryParse(txtFechaDesde.Text.Trim(), out dtTest)))
            {
                txtFechaDesde.Text = string.Empty;
            }

            if (!string.IsNullOrEmpty(txtFechaHasta.Text.Trim()) && !(DateTime.TryParse(txtFechaHasta.Text.Trim(), out dtTest)))
            {
                txtFechaHasta.Text = string.Empty;
            }

            if (!(ddlTipoImporte.SelectedIndex > 0) && !(Decimal.TryParse(txtImporte.Text.Trim(), out dCantidad)))
            {
                txtImporte.Text = string.Empty;
            }
            else
            {
                txtImporte.Text = string.Format("{0:0.00}", dCantidad);
            }
        }

        protected void guardarFiltroExpedientesEnSesion()
        {
            var numExpedienteCalc = txtNumExpediente.Text.Trim();
            AgenteUsuarios au = new AgenteUsuarios();
            if (au.IsAdmin() && !IsPostBack)
            {
                numExpedienteCalc = "-100";
            }
            FiltroExpedientes oFiltroExpedientes = new FiltroExpedientes(txtActividad.Text.Trim(), txtAsistente.Text.Trim(), ddlEstado.SelectedValue.Trim(), txtAMEC.Text.Trim(),
                ddlEstadoReserva.SelectedValue.Trim(), ddlTipo.SelectedValue.Trim(), ddlAnyo.SelectedValue.Trim(), ddlMes.SelectedValue.Trim(), txtFechaDesde.Text.Trim(), txtFechaHasta.Text.Trim(),
                txtUnidad.Text.Trim(), ddltiposDeProductos.SelectedValue.Trim(), txtNegocio.Text.Trim(), txtRegion.Text.Trim(), txtDistrito.Text.Trim(), txtPeticionario.Text.Trim(), txtProveedorServicio.Text.Trim(),
                ddlTipoImporte.SelectedItem.ToString(), ddlTipoImporte.SelectedValue, txtImporte.Text.Trim(), numExpedienteCalc,ddlDepartament.SelectedValue,ddlSaleForce.SelectedValue,ddlDistrict.SelectedValue,_datosUsuario.IdPeticionario,"");

            oFiltroExpedientes.TieneFiltroTipo = ddlTipo.SelectedIndex > 0;
            oFiltroExpedientes.TieneFiltroAnyo = ddlAnyo.SelectedIndex > 0;
            oFiltroExpedientes.TieneFiltroMes = ddlMes.SelectedIndex > 0;
            oFiltroExpedientes.TieneTipoImporte = ddlTipoImporte.SelectedIndex > 0;

            oFiltroExpedientes.filtroAvanzadoActivado = txtFiltroAvanzadoEstado.Value == "1";

            Session[Constantes.Session.oFiltroExpedientes] = oFiltroExpedientes;
        }

        protected void insertarParametrosFiltro(ref ObjectDataSourceSelectingEventArgs e, FiltroExpedientes oFiltroExp)
        {
            if (((EOS.Entidades.Datos.DVPeticionariosRoles) Session["rolesuser"]) != null)
            {
                AgenteUsuarios au = new AgenteUsuarios();

                int? idUserRegion = ((EOS.Entidades.Datos.DVPeticionariosRoles) Session["rolesuser"]).idregion;
                int? idUserDistrito = ((EOS.Entidades.Datos.DVPeticionariosRoles) Session["rolesuser"]).iddistrito;

                if ((au.IsAdmin() || (idUserRegion == null && idUserDistrito == null)) && !IsPostBack)
                {
                    e.InputParameters["disableBinding"] = true;
                }
                else
                {
                    e.InputParameters["disableBinding"] = false;
                }

                if (!string.IsNullOrEmpty(oFiltroExp.Actividad))
                    e.InputParameters["filtroActividad"] = "%" + oFiltroExp.Actividad + "%";
                if (!string.IsNullOrEmpty(oFiltroExp.Asistente))
                    e.InputParameters["filtroAsistente"] = "%" + oFiltroExp.Asistente + "%";
                if (!string.IsNullOrEmpty(oFiltroExp.EstadoExpediente))
                    e.InputParameters["filtroEstadoExpediente"] = oFiltroExp.EstadoExpediente;
                if (!string.IsNullOrEmpty(oFiltroExp.Amec))
                    e.InputParameters["filtroAmec"] = "%" + oFiltroExp.Amec + "%";

                if (!string.IsNullOrEmpty(oFiltroExp.EstadoReserva))
                    e.InputParameters["filtroEstadoReserva"] = oFiltroExp.EstadoReserva;

                if (oFiltroExp.TieneFiltroTipo) e.InputParameters["filtroTipo"] = oFiltroExp.Tipo;
                if (oFiltroExp.TieneFiltroAnyo) e.InputParameters["filtroAnyo"] = oFiltroExp.Anyo;
                if (oFiltroExp.TieneFiltroMes) e.InputParameters["filtroMes"] = oFiltroExp.Mes;
                if (oFiltroExp.TieneFechaDesde()) e.InputParameters["filtroFechaDesde"] = oFiltroExp.FechaDesde;
                if (oFiltroExp.TieneFechaHasta()) e.InputParameters["filtroFechaHasta"] = oFiltroExp.FechaHasta;

                if (!string.IsNullOrEmpty(oFiltroExp.Unidad))
                    e.InputParameters["filtroUnidad"] = "%" + oFiltroExp.Unidad + "%";
                if (!string.IsNullOrEmpty(oFiltroExp.Producto))
                    e.InputParameters["filtroProducto"] = oFiltroExp.Producto;
                if (!string.IsNullOrEmpty(oFiltroExp.AreaNegocio))
                    e.InputParameters["filtroAreaNegocio"] = "%" + oFiltroExp.AreaNegocio + "%";
                if (!string.IsNullOrEmpty(oFiltroExp.Region))
                    e.InputParameters["filtroRegion"] = "%" + oFiltroExp.Region + "%";
                if (!string.IsNullOrEmpty(oFiltroExp.Distrito))
                    e.InputParameters["filtroDistrito"] = "%" + oFiltroExp.Distrito + "%";
                if (!string.IsNullOrEmpty(oFiltroExp.Peticionario))
                    e.InputParameters["filtroPeticionario"] = "%" + oFiltroExp.Peticionario + "%";
                if (!string.IsNullOrEmpty(oFiltroExp.ProveedorServicio))
                    e.InputParameters["filtroProveedor"] = "%" + oFiltroExp.ProveedorServicio + "%";

                if (int.Parse(oFiltroExp.IdDepartament) != -1)
                    e.InputParameters["filtroIdDepartament"] = oFiltroExp.IdDepartament;
                if (int.Parse(oFiltroExp.IdSaleForce) != -1)
                    e.InputParameters["filtroIdSaleForce"] = oFiltroExp.IdSaleForce;
                if (int.Parse(oFiltroExp.IdDistrict) != -1)
                    e.InputParameters["filtroIdDistrict"] = oFiltroExp.IdDistrict;
                if (_datosUsuario != null && _datosUsuario.IdPeticionario > 0)
                    e.InputParameters["filtroIdPeticionarioSession"] = oFiltroExp.IdPeticionarioSession;

                oFiltroExp.TipoActividad = "";
                if (oFiltroExp.TieneTipoImporte)
                {
                    e.InputParameters["filtroTipoImporte"] = oFiltroExp.TipoImporteItem;
                    e.InputParameters["filtroImporte"] = oFiltroExp.Importe;
                }

                if (!string.IsNullOrEmpty(txtNumExpediente.Text.Trim()))
                {
                    e.InputParameters["filtroNumExpediente"] = txtNumExpediente.Text.Trim();
                    ;
                }
            }
        }

        private void RellenarCombos()
        {
            //Ismael Ameller 08-03-2011 Carga el combo de estado reserva de BBDD
            AgenteMaestros agente = new AgenteMaestros();

            List<DDepartament> departamentList = agente.GetDepartaments();
            DDepartament deptEmptyItem = new DDepartament() {IdDepartament = -1,Departament = "Sin Especificar"};
            departamentList.Add(deptEmptyItem);
            OriginalDeptList = departamentList.OrderBy(x=>x.IdDepartament).ToList();
            ddlDepartament.DataSource = OriginalDeptList;
            ddlDepartament.DataValueField = "iddepartament";
            ddlDepartament.DataTextField = "departament";
            ddlDepartament.DataBind();

            List<DDistrict> districtList = agente.GetDistrict();
            DDistrict disEmptyItem = new DDistrict() { IdDistrict = -1, District = "Sin Especificar" };
            districtList.Add(disEmptyItem);
            OriginalDistList = districtList.OrderBy(x=>x.IdDistrict).ToList();
            ddlDistrict.DataSource = OriginalDistList;
            ddlDistrict.DataValueField = "iddistrict";
            ddlDistrict.DataTextField = "district";
            ddlDistrict.DataBind();

            List<DSaleForce> saleForceList = agente.GetSaleForce();
            DSaleForce saleForEmptyItem = new DSaleForce() { IdSaleForce = -1, SaleForce = "Sin Especificar" };
            saleForceList.Add(saleForEmptyItem);
            OriginalSaleForceList = saleForceList.OrderBy(x=>x.IdSaleForce).ToList();
            ddlSaleForce.DataSource = OriginalSaleForceList;
            ddlSaleForce.DataValueField = "idsaleforce";
            ddlSaleForce.DataTextField = "saleforce";
            ddlSaleForce.DataBind();

            BDManage extraccion = new BDManage(SQLSentence.GetInstancia().getEstadoReserva(), 1);
            ddlEstadoReserva.DataSource = extraccion.getDataSet();
            ddlEstadoReserva.DataValueField = "idestado";
            ddlEstadoReserva.DataTextField = "estado";
            ddlEstadoReserva.DataBind();
            ListItem todos = new ListItem("(Todos)", "");
            ddlEstadoReserva.Items.Insert(0, todos);

            ListItem itemCN = null;
            ListItem itemDel = null;
            foreach (ListItem item in ddlEstadoReserva.Items)
            {
                if (item.Value == "CN")
                {
                    if (itemCN != null)
                    {
                        itemCN.Value += ("' + '" + item.Value);
                        itemCN.Text = item.Text;
                    }
                    else
                    {
                        itemCN = item;
                    }
                }
                else if (item.Value == "CNTR")
                {
                    if (itemCN != null)
                    {
                        itemCN.Value += ("' + '" + item.Value);
                    }
                    else
                    {
                        itemCN = item;
                    }
                    itemDel = item;
                }
            }

            ddlEstadoReserva.Items.Remove(itemDel);

            //FIN Ismael Ameller 08-03-2011 Carga el combo de estado reserva de BBDD

            DDatosPersonalesUsuario datosUsuario = (DDatosPersonalesUsuario)Session["datosUsuario"];
            if (!datosUsuario.admin.HasValue)
            {
                this.ddltiposDeProductos.DataSource = agente.ObtenerProductos(null);
            }
            else
            {
                this.ddltiposDeProductos.DataSource = agente.ObtenerProductos(datosUsuario.Idarea);
                this.ddltiposDeProductos.DataTextField = "Producto";
                this.ddltiposDeProductos.DataValueField = "idareaProducto";
                this.ddltiposDeProductos.DataBind();
                ListItem blanco = new ListItem("", "");
                ddltiposDeProductos.Items.Insert(0, blanco);
            }
            //Ismael Ameller 23-03-2011 Carga Combo de productos
        }

        private void llenarFiltro(FiltroExpedientes oFiltroExp)
        {
            txtActividad.Text = oFiltroExp.Actividad;
            txtAsistente.Text = oFiltroExp.Asistente;
            ddlEstado.SelectedValue = oFiltroExp.EstadoExpediente;
            txtAMEC.Text = oFiltroExp.Amec;
            ddlEstadoReserva.SelectedValue = oFiltroExp.EstadoReserva;
            ddlTipo.SelectedValue = oFiltroExp.Tipo;
            ddlAnyo.SelectedValue = oFiltroExp.Anyo;
            ddlMes.SelectedValue = oFiltroExp.Mes;
            txtFechaDesde.Text = oFiltroExp.FechaDesde;
            txtFechaHasta.Text = oFiltroExp.FechaHasta;
            txtUnidad.Text = oFiltroExp.Unidad;
            ddltiposDeProductos.SelectedValue = oFiltroExp.Producto;
            txtNegocio.Text = oFiltroExp.AreaNegocio;
            txtRegion.Text = oFiltroExp.Region;
            txtDistrito.Text = oFiltroExp.Distrito;
            txtPeticionario.Text = oFiltroExp.Peticionario;
            txtProveedorServicio.Text = oFiltroExp.ProveedorServicio;
            ddlTipoImporte.SelectedValue = oFiltroExp.TipoImporteValue;
            txtImporte.Text = oFiltroExp.Importe;
            ddlSaleForce.SelectedValue = oFiltroExp.IdSaleForce;
            ddlDepartament.SelectedValue = oFiltroExp.IdDepartament;
            ddlDistrict.SelectedValue = oFiltroExp.IdDistrict;

            if (oFiltroExp.NumExpediente != "-100")
            {
                txtNumExpediente.Text = oFiltroExp.NumExpediente;
            }
            
        }

        private void mostrarFiltroAvanzado()
        {
            blnMostrarFiltroAvanzado = true;

            //borrar
            //string script = "<script type=\"text/javascript\">mostrarFiltroAvanzado(true);</script>";
            //this.ClientScript.RegisterClientScriptBlock(typeof(Alert), "back", script);
        }

        private FiltroExpedientes getFiltroExpedientes()
        {
            return (FiltroExpedientes)Session[Constantes.Session.oFiltroExpedientes];
        }

        private DataPager getDataPager()
        {
            return (DataPager)lvExpedientes.FindControl("DataPager3");
        }

        #endregion metodos

        protected void ddlSaleForce_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            AgenteMaestros agente = new AgenteMaestros();
            List<DDistrict> dList = new List<DDistrict>();
           
            if (int.Parse(ddlSaleForce.SelectedValue) == -1 && int.Parse(ddlDepartament.SelectedValue) == -1)
            {
                dList = agente.GetDistrict().ToList();
            }
            else
            {
                dList = agente.GetDistrict().Where(x => x.IdSaleForce == int.Parse(ddlSaleForce.SelectedValue)).ToList();
            }

            DDistrict disEmptyItem = new DDistrict() { IdDistrict = -1, District = "Sin Especificar" };
            dList.Add(disEmptyItem);
            ddlDistrict.DataSource = dList.OrderBy(x=>x.IdDistrict);
            ddlDistrict.DataValueField = "iddistrict";
            ddlDistrict.DataTextField = "district";
            ddlDistrict.DataBind();
        }

        protected void ddlDepartament_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            List<DDistrict> dList = new List<DDistrict>();
            List <DSaleForce> sfList = new List<DSaleForce>();
            AgenteMaestros agente = new AgenteMaestros();

            if (int.Parse(ddlDepartament.SelectedValue) == -1)
            {
                sfList = agente.GetSaleForce();
                dList = agente.GetDistrict();
            }
            else
            {
                sfList = agente.GetSaleForce().Where(x => x.IdDepartament == int.Parse(ddlDepartament.SelectedValue)).ToList();
                dList = agente.GetDistrict().Where(x => sfList.Select(y => y.IdSaleForce).Contains(x.IdSaleForce)).ToList();
            }
            
            
            DSaleForce saleForEmptyItem = new DSaleForce() { IdSaleForce = -1, SaleForce = "Sin Especificar" };
            sfList.Add(saleForEmptyItem);
            ddlSaleForce.DataSource = sfList.OrderBy(x=>x.IdSaleForce).ToList();
            ddlSaleForce.DataValueField = "idsaleforce";
            ddlSaleForce.DataTextField = "saleforce";
            ddlSaleForce.DataBind();

            
            DDistrict disEmptyItem = new DDistrict() { IdDistrict = -1, District = "Sin Especificar" };
            dList.Add(disEmptyItem);
            ddlDistrict.DataSource = dList.OrderBy(x=>x.IdDistrict).ToList();
            ddlDistrict.DataValueField = "iddistrict";
            ddlDistrict.DataTextField = "district";
            ddlDistrict.DataBind();

        }
    }
}