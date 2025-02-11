using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Entidades.Datos;
using EOS.Web;
//using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using EOS.Entidades.Filtros;
using EOS.Entidades.Maestros;
using NPOI.SS.Formula.Functions;


namespace EOS
{
    public partial class ListadoAMECs : System.Web.UI.Page
    {
        public bool bAprobador = false;
        public string amecanterior = "";
        protected Peticionario peticionario;
        private BDManage extraccion;
        protected DataTable dt;
        public static ListView lvCongresos;
        public static ObjectDataSource ods;
        public static ListView listaCongresos;
        public static ObjectDataSource odsCongresos;
        public static string idamecstring;
        public static int em=0;

        private List<DDepartament> OriginalDeptList;
        private List<DDistrict> OriginalDistList;
        private List<DSaleForce> OriginalSaleForceList;

        public List<KeyValuePair<string, string>> Listanyo
        {
            get
            {
                var value = new List<KeyValuePair<string, string>>();
                value.Add(new KeyValuePair<string, string>("-1","(Todos)"));
                for (var i=2005;i<=DateTime.Now.Year;i++)
                {
                    value.Add(new KeyValuePair<string, string>(i.ToString(),i.ToString()));
                }
                return value;
            }
        }
        EOS.Entidades.Datos.DDatosPersonalesUsuario datosUsuario;

        protected void Page_Load(object sender, EventArgs e)
        {

            ///////////////Controlar Si caduca la sesión en la aplicación//////////////
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Session["rolesuser"] == null) Response.Redirect("~/Account/Login.aspx");
            //////////////////////////////////////////////////////////////////////////

            DVPeticionariosRoles datosRoles = (EOS.Entidades.Datos.DVPeticionariosRoles)Session["rolesuser"];
            
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            if(Session["datosUsuario"] == null)
            datosUsuario = agenteUsu.ObtenerDatosPersonalesPorIDPeticionario(datosRoles.IdPeticionario.ToString());
            else
            {
                datosUsuario = (DDatosPersonalesUsuario) Session["datosUsuario"];
            }
            //Xavier Morell
            //AgenteUsuarios agUsu = new AgenteUsuarios();
            //EOS.Entidades.Datos.DVPeticionariosRoles datosRoles =  agUsu.ObtenerDatosRolesPorLogin();
            if (!Page.IsPostBack)
            {
                Session["StartRowIndex"] = 0;
                Session["datosUsuario"] = datosUsuario;
                bAprobador = (datosRoles.aprobador.HasValue) ? datosRoles.aprobador.Value : false;
                string user = datosUsuario.login;
                string pass = datosUsuario.password;
                try
                {
                    extraccion = new BDManage(SQLSentence.GetInstancia().login(user, pass), 1);
                    dt = extraccion.getDataSet().Tables[0];

                    //if(Request.QueryString["id"]!="")
                    if (dt.Rows.Count == 1)
                        peticionario = new Peticionario(dt.Rows[0].ItemArray[0].ToString(), true);
                }
                catch { }

                SetBackString();
                CargarValoresIniciales(datosRoles);
                InicializaCombos();
                //rellenaUnidad();
                //rellenaRegion();
                //rellenaArea();
                //rellenaDistrito();
                permisosDDL(peticionario.IdCargo);
                txtAnyo.DataSource = Listanyo;
                txtAnyo.DataTextField = "Value";
                txtAnyo.DataValueField = "Key";
                txtAnyo.DataBind();
            }
         
        }


        public bool UnidadesOrganizativas()
        {
            if (chkUnidadesOrganizativas.Checked)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected void InicializaCombos()
        {
            RellenarCombos();
            BDManage vectArea;
            BDManage vectRegion;
            BDManage vectDistrito;
            if (datosUsuario.admin== true)
            {
                ListItem blankUnidad = new ListItem("Sin Unidad", "0");
                ddlUnidadMas.Items.Insert(0, blankUnidad);
                ddlUnidadMas.SelectedValue = "0";
                if (!datosUsuario.idunidad.HasValue)
                {
                    ListItem blankArea = new ListItem("Sin Area", "0");
                    ddlAreaMas.Items.Insert(0, blankArea);
                    ddlAreaMas.SelectedValue = "0";

                    ListItem blankRegion = new ListItem("Sin Region", "0");
                    ddlRegionMas.Items.Insert(0, blankRegion);
                    ddlRegionMas.SelectedValue = "0";

                    ListItem blankDistrito = new ListItem("Sin Distrito", "0");
                    ddlDistritoMas.Items.Insert(0, blankDistrito);
                    ddlDistritoMas.SelectedValue = "0";
                }

            }
            if (datosUsuario.idunidad.HasValue)
            {
                ddlUnidadMas.SelectedValue = datosUsuario.idunidad.ToString();
                vectArea = new BDManage(SQLSentenceAMEC.GetInstancia().getAreasUnidad(ddlUnidadMas.SelectedValue), 1);
                rellenaAreaMas(vectArea);
                vectRegion = new BDManage(SQLSentenceAMEC.GetInstancia().getRegionesUnidades(ddlUnidadMas.SelectedValue.ToString()), 1);
                rellenaRegionMas(vectRegion);
                if (!datosUsuario.Idarea.HasValue && !datosUsuario.idregion.HasValue)
                {
                    vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoUnidad(ddlUnidadMas.SelectedValue.ToString()), 1);
                    rellenaDistritoMas(vectDistrito);
                }
            }
            //El Usuario puede tener (Area y Region) (Solo Area) (Solo Región)
            if (datosUsuario.Idarea.HasValue && datosUsuario.idregion.HasValue)
            {
                ddlAreaMas.SelectedValue = datosUsuario.Idarea.ToString();
                ddlRegionMas.SelectedValue = datosUsuario.idregion.ToString();
                vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoRegionArea(ddlRegionMas.SelectedItem.Value, ddlAreaMas.SelectedItem.Value), 1);
                rellenaDistritoMas(vectDistrito);
            }
            // Solo Area
            if (datosUsuario.Idarea.HasValue && !datosUsuario.idregion.HasValue)
            {
                ddlAreaMas.SelectedValue = datosUsuario.Idarea.ToString();
                vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoArea(ddlAreaMas.SelectedItem.Value), 1);
                rellenaDistritoMas(vectDistrito);  
            }

            // Solo Región
            if (!datosUsuario.Idarea.HasValue && datosUsuario.idregion.HasValue)
            {
                ddlRegionMas.SelectedValue = datosUsuario.idregion.ToString();
                vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoRegion(ddlRegionMas.SelectedItem.Value), 1);
                rellenaDistritoMas(vectDistrito);
            }

            if (datosUsuario.iddistrito.HasValue)
            {
                ddlDistritoMas.SelectedValue = datosUsuario.iddistrito.ToString();
            }
        }


        private void RellenarCombos()
        {

            /* //cuando el combo de unidades se seleccion entonces se cambia el de areas y
             //se vuelve a bindar con los datos seleccionados en unidades.
             AgenteMaestros agente = new AgenteMaestros();
             ListaReservasInfo = agente.obtenerInfoReserva(entExp.Idexpediente);
             */
            AgenteMaestros agente = new AgenteMaestros();

            List<DDepartament> departamentList = agente.GetDepartaments();
            DDepartament deptEmptyItem = new DDepartament() { IdDepartament = -1, Departament = "Sin Especificar" };
            departamentList.Add(deptEmptyItem);
            OriginalDeptList = departamentList.OrderBy(x => x.IdDepartament).ToList();
            ddlDepartament.DataSource = OriginalDeptList;
            ddlDepartament.DataValueField = "iddepartament";
            ddlDepartament.DataTextField = "departament";
            ddlDepartament.DataBind();

            List<DDistrict> districtList = agente.GetDistrict();
            DDistrict disEmptyItem = new DDistrict() { IdDistrict = -1, District = "Sin Especificar" };
            districtList.Add(disEmptyItem);
            OriginalDistList = districtList.OrderBy(x => x.IdDistrict).ToList();
            ddlDistrict.DataSource = OriginalDistList;
            ddlDistrict.DataValueField = "iddistrict";
            ddlDistrict.DataTextField = "district";
            ddlDistrict.DataBind();

            List<DSaleForce> saleForceList = agente.GetSaleForce();
            DSaleForce saleForEmptyItem = new DSaleForce() { IdSaleForce = -1, SaleForce = "Sin Especificar" };
            saleForceList.Add(saleForEmptyItem);
            OriginalSaleForceList = saleForceList.OrderBy(x => x.IdSaleForce).ToList();
            ddlSaleForce.DataSource = OriginalSaleForceList;
            ddlSaleForce.DataValueField = "idsaleforce";
            ddlSaleForce.DataTextField = "saleforce";
            ddlSaleForce.DataBind();

            BDManage vectUnidadMas;
            vectUnidadMas = new BDManage(SQLSentenceAMEC.GetInstancia().getAllUnidades(), 1);
            ddlUnidadMas.DataSource = vectUnidadMas.getDataSet();
            ddlUnidadMas.DataValueField = "idUnidad";
            ddlUnidadMas.DataTextField = "unidad";
            ddlUnidadMas.DataBind();
            

            BDManage vectAreaMas;
            vectAreaMas = new BDManage(SQLSentenceAMEC.GetInstancia().getAllAreas(), 1);
            ddlAreaMas.DataSource = vectAreaMas.getDataSet();
            ddlAreaMas.DataValueField = "idarea";
            ddlAreaMas.DataTextField = "area";
            ddlAreaMas.DataBind();


            BDManage vectRegionMas;
            vectRegionMas = new BDManage(SQLSentenceAMEC.GetInstancia().getAllRegiones(), 1);
            ddlRegionMas.DataSource = vectRegionMas.getDataSet();
            ddlRegionMas.DataValueField = "idregion";
            ddlRegionMas.DataTextField = "region";
            ddlRegionMas.DataBind();


            BDManage vectDistritoMas;
            vectDistritoMas = new BDManage(SQLSentenceAMEC.GetInstancia().getAllDistritos(), 1);
            ddlDistritoMas.DataSource = vectDistritoMas.getDataSet();
            ddlDistritoMas.DataValueField = "iddistrito";
            ddlDistritoMas.DataTextField = "distrito";
            ddlDistritoMas.DataBind();

        }

        protected void CargarValoresIniciales(DVPeticionariosRoles datosRol)
        {
            AgenteAmecInfo AgAmecInf = new AgenteAmecInfo();
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            this.ddlUsuario.DataSource = agenteUsu.ObtenerTodosUsuariosCombo(string.Empty);
            this.ddlUsuario.DataBind();
            
            int i = agenteUsu.EsUsurioDelegado(datosUsuario.IdPeticionario);

            /*if (AgAmecInf.ObtenerNumPendienteSometer(datosUsuario.IdPeticionario.ToString()) > 0)
                ddlEstado.SelectedValue = "7";
            else
            {
                if ((i == 1 || datosRol.administrador.Value))
                {
                    ddlEstado.SelectedValue = "2";
                }
                else ddlEstado.SelectedValue = "6";
            }*/
            
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {

        }

        protected void btnVerExcel_Click(object sender, EventArgs e)
        {
            
            IEnumerable<ListadoAmecs> list = (IEnumerable<ListadoAmecs>)odsAMECs.Select();
            Session.Add("IEnumAmec", list);
            Response.Redirect("PlantillaExcelFiltroListadoAmecs.aspx");
            // IEnumerable<> ColeccionAMECs = odsAMECs.Select();

        }

        protected void btnVerExcelInforme_Click(object sender, EventArgs e)
        {
            AgenteAmecInfo agAmInfo = new AgenteAmecInfo();
            string year = null, mes = null, unidad = null, area = null, distrito = null, region = null, nombrePrograma = null, estado = null, fechaInicio = null, fechaFin = null, idamec = null;
            //Campos a filtrar:
            //IdUnidad, IdArea, Iddistrito, IdRegion, NombreProgramaActividad, idestado, FechaInicioActividad, FechaFinActividad, idamec 
            if (chkUnidadesOrganizativas.Checked == true)
            {
                //
            }
            if (txtAMEC.Text != "") idamec = txtAMEC.Text;
            if (txtActividad.Text != "") nombrePrograma = txtActividad.Text;
            if (ddlEstado.SelectedValue != "-1") estado = ddlEstado.SelectedItem.Text;
            if (txtFechaComienzo.Text != "") fechaInicio = txtFechaComienzo.Text;
            if (txtFechaFinalizacion.Text != "") fechaFin = txtFechaFinalizacion.Text;
            if (txtAnyo.SelectedIndex > 0) year = txtAnyo.SelectedValue.Trim();
            if (txtMes.SelectedIndex > 0) mes = txtMes.SelectedValue.Trim();
            string iddistrict = ddlDistrict.SelectedValue != "-1" ? ddlDistrict.SelectedValue : "";
            string idsaleforce = ddlSaleForce.SelectedValue != "-1" ? ddlSaleForce.SelectedValue : "";
            string iddepartament = ddlDepartament.SelectedValue != "-1" ? ddlDepartament.SelectedValue : "";

            DataSet ds = agAmInfo.ObtenerInformeFCPA(idamec, nombrePrograma, estado, fechaInicio, fechaFin, unidad, area, region, distrito, year, mes, iddistrict,idsaleforce,iddepartament);
            Session.Add("datasetFCPA", ds);
            Response.Redirect("PlantillaExcelInformeFCPA.aspx");
        }

        protected void CargarListView1(ListView listview, ObjectDataSource odsCongresos, string idamec)
        {

            listview.DataSourceID = odsCongresos.ID;
            listview.DataBind();
        }

        protected void CargarListView(Object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            if (lb.Text != "[-]") idamecstring = lb.CommandArgument.ToString();
        }


        protected void lvAMECs_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ImageButton imgAmecDetalle = (ImageButton) e.Item.FindControl("imgAmecDetalle");
            ListadoAmecs data = (ListadoAmecs) e.Item.DataItem;

            if (data.veeva)
            {
                imgAmecDetalle.PostBackUrl = Page.ResolveUrl(String.Format("DetalleAMECVeeva.aspx?idamec={0}", data.idamecs));
            }
            else if (data.newco)
            {
                imgAmecDetalle.PostBackUrl = Page.ResolveUrl(String.Format("DetalleAMECNewCo.aspx?idamec={0}", data.idamecs));
            }
            else if (data.nuevosFlujosAprobacion.HasValue && data.nuevosFlujosAprobacion.Value)
            {
                imgAmecDetalle.PostBackUrl = Page.ResolveUrl(String.Format("DetalleAMEC.aspx?idamec={0}", data.idamecs));
            }
            else
            {
                imgAmecDetalle.PostBackUrl = Page.ResolveUrl(String.Format("NuevoDetalleAMEC.aspx?idamec={0}", data.idamecs));
            }

            LinkButton lb = (LinkButton) ((ListView) sender).Parent.FindControl("imgEventos");
            Label lb2 = (Label) e.Item.FindControl("labelidamecs");
            if (idamecstring == lb2.Text)
            {
                odsCongresos = (ObjectDataSource) e.Item.FindControl("odsActividadesAsigAmec");
                ListView lv2 = (ListView) e.Item.FindControl("lvAmecsCongresos");
                odsCongresos.SelectParameters.Add("filtroIdAMEC", idamecstring);
                lv2.DataSourceID = odsCongresos.ID;
                lv2.DataBind();
            }

        }

        protected void imgExportListAmec_Command(object sender, CommandEventArgs e)
        {
            Response.Redirect(String.Format("PlantillaExportarExceListadoAmec.aspx?idamec={0}", e.CommandArgument.ToString()));
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {

            lvAMECs.DataBind();
        }

        protected void GuardarFiltrosEnSession()
        {
            decimal dCantidad = 0;
            decimal.TryParse(txtImporte.Text, out dCantidad);
            FiltroListadoAMECs filtro = new FiltroListadoAMECs()
            {
                AMECLike = txtAMEC.Text.Trim(),
                idsolicitante = string.IsNullOrEmpty(ddlUsuario.SelectedValue) ? new Nullable<int>() : int.Parse(ddlUsuario.SelectedValue),
                NombrePrograma = txtActividad.Text.Trim(),
                idestadoamec = string.IsNullOrEmpty(ddlEstado.SelectedValue) ? new Nullable<int>() : int.Parse(ddlEstado.SelectedValue.Trim()),
                paraguas = string.IsNullOrEmpty(ddlParaguas.SelectedValue) ? new Nullable<int>() : int.Parse(ddlParaguas.SelectedValue.Trim()),
                year = txtAnyo.SelectedIndex > 0 ? int.Parse(txtAnyo.SelectedValue.Trim()) : new Nullable<int>(),
                mes = txtMes.SelectedIndex > 0 ? int.Parse(txtMes.SelectedValue.Trim()) : new Nullable<int>(),
                Importe = dCantidad > 0 ?  decimal.Parse(dCantidad.ToString()) : new Nullable<decimal>(),

            };
        }

        protected void btnAprobar_Click(object sender, ImageClickEventArgs e)
        {
            AgenteExpedientes agExp = new AgenteExpedientes();
            try
            {
                string sFiltroTipoImporte = "";
                string sFiltroImporte = "";
                if (ddlTipoImporte.SelectedIndex > 0)
                {
                    decimal dCantidad = 0;
                    decimal.TryParse(txtImporte.Text, out dCantidad);
                    sFiltroTipoImporte = ddlTipoImporte.SelectedItem.ToString();
                    sFiltroImporte = dCantidad.ToString();
                }
                int nAprobados = 0;

                if (nAprobados > 0)
                {
                    Alert.Show(string.Format("Se han aprobado {0} AMECs", nAprobados), null);
                    lvAMECs.DataBind();
                }
                else
                {
                    Alert.Show(string.Format("No hay ningún AMEC pendiente de aprobar para este rol en el listado seleccionado"), null);
                }

            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Alert.Show("Se ha producido un error aprobando los AMECs seleccionados", null);
            }
        }

        protected void lvAMECs_Sorting(object sender, ListViewSortEventArgs e)
        {

        }

        protected void odsActividadesAsigAmec_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            //if (!string.IsNullOrEmpty(txtAMEC.Text.Trim())) e.InputParameters["filtroAMEC"] = "%" + txtAMEC.Text.Trim() + "%";        
        }
        protected void odsAMEC_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {

            if (!Page.IsPostBack)
            {
                e.Arguments.StartRowIndex = (int)Session["StartRowIndex"];
            }
            else
            {
                Session["StartRowIndex"] = e.Arguments.StartRowIndex;
            }

            if (!string.IsNullOrEmpty(txtAMEC.Text.Trim())) e.InputParameters["filtroAMEC"] = "%" + txtAMEC.Text.Trim() + "%";
            if (!string.IsNullOrEmpty(txtActividad.Text.Trim())) e.InputParameters["filtroActividad"] = "%" + txtActividad.Text.Trim() + "%";
            if (!string.IsNullOrEmpty(ddlUsuario.SelectedValue.ToString())) e.InputParameters["filtroSolicitante"] = ddlUsuario.SelectedValue;
            if (!string.IsNullOrEmpty(ddlEstado.SelectedValue.Trim())) e.InputParameters["filtroEstadoAmec"] = ddlEstado.SelectedValue.Trim();
            //if (!string.IsNullOrEmpty(ddlAprobacion.SelectedValue.Trim())) e.InputParameters["filtroAprobacion"] = ddlAprobacion.SelectedValue.Trim();
            if (!string.IsNullOrEmpty(ddlParaguas.SelectedValue.Trim())) e.InputParameters["filtroParaguas"] = ddlParaguas.SelectedValue.Trim();
            if (txtAnyo.SelectedIndex > 0) e.InputParameters["filtroAnyo"] = txtAnyo.SelectedValue.Trim();
            if (txtMes.SelectedIndex > 0) e.InputParameters["filtroMes"] = txtMes.SelectedValue.Trim();
            e.InputParameters["filtroIdAreaUsuarioConectado"] = datosUsuario.Idarea.ToString();
            e.InputParameters["filtroIdDistritoUsuarioConectado"] = datosUsuario.iddistrito.ToString();
            e.InputParameters["filtroIdUnidadUsuarioConectado"] = datosUsuario.idunidad.ToString();
            e.InputParameters["filtroIdRegionUsuarioConectado"] = datosUsuario.idregion.ToString();
            e.InputParameters["filtroIdArea"] = ddlAreaMas.SelectedValue.ToString();
            e.InputParameters["filtroIdDistrito"] = ddlDistritoMas.SelectedValue.ToString();
            e.InputParameters["filtroIdUnidad"] = ddlUnidadMas.SelectedValue.ToString();
            e.InputParameters["filtroIdRegion"] = ddlRegionMas.SelectedValue.ToString();
            e.InputParameters["filtroXecUnidadesOrganizativas"] = chkUnidadesOrganizativas.Checked.ToString();
            e.InputParameters["filtroProducto"] = txtImporteCargoProducto.Text.Trim();
            e.InputParameters["filtroFechaInicio"] = txtFechaComienzo.Text.ToString();
            e.InputParameters["filtroFechaFin"] = txtFechaFinalizacion.Text.ToString();
            if (int.Parse(ddlDepartament.SelectedValue) != -1) e.InputParameters["filtroIdDepartament"] = ddlDepartament.SelectedValue;
            if (int.Parse(ddlSaleForce.SelectedValue) != -1) e.InputParameters["filtroIdSaleForce"] = ddlSaleForce.SelectedValue;
            if (int.Parse(ddlDistrict.SelectedValue) != -1) e.InputParameters["filtroIdDistrict"] = ddlDistrict.SelectedValue;
            e.InputParameters["filtroIdPeticionarioSession"] = datosUsuario.IdPeticionario;
            if (ddlTipoImporte.SelectedIndex > 0)
            {
                decimal dCantidad = 0;
                decimal.TryParse(txtImporte.Text, out dCantidad);
                txtImporte.Text = string.Format("{0:0.00}", dCantidad);
                e.InputParameters["filtroTipoImporte"] = ddlTipoImporte.SelectedItem.ToString();
                e.InputParameters["filtroImporte"] = dCantidad.ToString();
            }
            else
            {
                txtImporte.Text = "";
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
            int nCaso = 3;
            if (Page.Request.UrlReferrer != null)
            {

                if (Page.Request.UrlReferrer.AbsoluteUri.Contains("NuevoDetalleAMEC.aspx"))
                {
                    // Caso 1: Viene de Listado de AMECs
                    nCaso = 1;
                }
                if (Page.Request.UrlReferrer.AbsoluteUri.Contains("Expediente.aspx"))
                {
                    // Caso 2: Ha pulsado el botón de nuevo o ha refrescado la página. No se debe perder el back original
                    nCaso = 2;
                }
            }
            else
            {
                // Caso 3:Qualquier otro caso.
                nCaso = 3;
            }
            return ComputeBackString(nCaso);
        }

        protected void rellenaUnidadMas(BDManage VectUnidad)
        {
            ddlUnidadMas.DataSource = VectUnidad.getDataSet();
            ddlUnidadMas.DataValueField = "idUnidad";
            ddlUnidadMas.DataTextField = "unidad";
            ddlUnidadMas.DataBind();
            ListItem blankUnidad = new ListItem("Sin Unidad", "0");
            ddlUnidadMas.Items.Insert(0, blankUnidad);
            ddlUnidadMas.SelectedValue = "0";
        }

        protected void rellenaRegionMas(BDManage VectRegion)
        {
            ddlRegionMas.DataSource = VectRegion.getDataSet();
            ddlRegionMas.DataValueField = "idregion";
            ddlRegionMas.DataTextField = "region";
            ddlRegionMas.DataBind();
            ListItem blankRegion = new ListItem("Sin Region", "0");
            ddlRegionMas.Items.Insert(0, blankRegion);
            ddlRegionMas.SelectedValue = "0";
        }

        protected void rellenaAreaMas(BDManage VectArea)
        {
            ddlAreaMas.DataSource = VectArea.getDataSet();
            ddlAreaMas.DataValueField = "idarea";
            ddlAreaMas.DataTextField = "area";
            ddlAreaMas.DataBind();
            ListItem blankArea = new ListItem("Sin Area", "0");
            ddlAreaMas.Items.Insert(0, blankArea);
            ddlAreaMas.SelectedValue = "0";
        }

        protected void rellenaDistritoMas(BDManage VectDistrito)
        {
            ddlDistritoMas.DataSource = VectDistrito.getDataSet();
            ddlDistritoMas.DataValueField = "iddistrito";
            ddlDistritoMas.DataTextField = "distrito";
            ddlDistritoMas.DataBind();
            ListItem blankDistrito = new ListItem("Sin Distrito", "0");
            ddlDistritoMas.Items.Insert(0, blankDistrito);
            ddlDistritoMas.SelectedValue = "0";
        }

        protected string ComputeBackString(int nCaso)
        {
            string sBackString = "";
            string sURL = "";

            switch (nCaso)
            {
                case 1:
                    sURL = "NuevoDetalleAMEC.aspx";
                    break;
                case 2:
                    sURL = "Expedientes.aspx";
                    break;
                case 3:
                    //Calquier otro caso
                    sURL = "Expedientes.aspx";
                    break;

            }

            Session.Add("DetalleAMEC2_URLBackHistory", sURL);
            sBackString = string.Format("javascript:location.href = '{0}';return false;", sURL);
            this.btnVolver.OnClientClick = sBackString;

            return sURL;
        }

        /* FIN DE CAMBIO DE JOSE LAGUNA 23-03-2012 16:50 */
        protected void permisosDDL(string cargo)
        {
            ddlUnidadMas.Enabled = false;
            ddlAreaMas.Enabled = false;
            ddlRegionMas.Enabled = false;
            ddlDistritoMas.Enabled = false;

            if (string.IsNullOrEmpty(datosUsuario.idunidad.ToString()))
                ddlUnidadMas.Enabled = true;

            if (string.IsNullOrEmpty(datosUsuario.Idarea.ToString()))
                ddlAreaMas.Enabled = true;
            else
            {
                ddlUnidadMas.Enabled = false;
            }

            if (string.IsNullOrEmpty(datosUsuario.idregion.ToString()))
                ddlRegionMas.Enabled = true;
            else
            {
                ddlUnidadMas.Enabled = false;
            }

            if (string.IsNullOrEmpty(datosUsuario.iddistrito.ToString()))
                ddlDistritoMas.Enabled = true;
            else
            {
                ddlUnidadMas.Enabled = false;
                ddlAreaMas.Enabled = false;
                ddlRegionMas.Enabled = false;
            }

            if (datosUsuario.admin == true)
            {
                ddlUnidadMas.Enabled = true;
                ddlAreaMas.Enabled = true;
                ddlRegionMas.Enabled = true;
                ddlDistritoMas.Enabled = true;
            }
        }

        protected void rellenaUnidad()
        {
            //peticionario = (Peticionario)Session["peticionario"];

            ddlUnidadMas.DataSource = peticionario.VectUnidad.getDataSet();
            ddlUnidadMas.DataValueField = "idUnidad";
            ddlUnidadMas.DataTextField = "unidad";
            ddlUnidadMas.DataBind();

            if (!string.IsNullOrEmpty(peticionario.IdUnidad))
            {
                ddlUnidadMas.SelectedValue = peticionario.IdUnidad;

                //Para que no se pueda cambiar
                ddlUnidadMas.Enabled = false;
            }
            else
            {
                ListItem blank = new ListItem("Cualquier Unidad", "0");
                ddlUnidadMas.Items.Insert(0, blank);
                ddlUnidadMas.SelectedValue = "0";
                //ddlUnidad.Enabled = false;
            }
        }

        protected void rellenaRegion()
        {
            ddlRegionMas.DataSource = peticionario.VectRegion.getDataSet();
            ddlRegionMas.DataValueField = "idregion";
            ddlRegionMas.DataTextField = "region";
            ddlRegionMas.DataBind();

            if (!string.IsNullOrEmpty(peticionario.IdRegion))
            {
                ddlRegionMas.SelectedValue = peticionario.IdRegion;

                ddlRegionMas.Enabled = false;
            }
            else
            {
                ListItem blank = new ListItem("Cualquier Region", "0");
                ddlRegionMas.Items.Insert(0, blank);
                ddlRegionMas.SelectedValue = "0";
            }
        }

        protected void rellenaArea()
        {
            ddlAreaMas.DataSource = peticionario.VectArea.getDataSet();
            ddlAreaMas.DataValueField = "idarea";
            ddlAreaMas.DataTextField = "area";
            ddlAreaMas.DataBind();

            if (!string.IsNullOrEmpty(peticionario.IdArea))
            {
                ddlAreaMas.SelectedValue = peticionario.IdArea;

                //Para que no se pueda cambiar
                ddlAreaMas.Enabled = false;
            }
            else
            {
                ListItem blank = new ListItem("Cualquier Area", "0");
                ddlAreaMas.Items.Insert(0, blank);
                ddlAreaMas.SelectedValue = "0";
                //ddlRegion.Enabled = false;
            }
        }

        protected void rellenaDistrito()
        {
            ddlDistritoMas.DataSource = peticionario.VectDistrito.getDataSet();
            ddlDistritoMas.DataValueField = "iddistrito";
            ddlDistritoMas.DataTextField = "distrito";
            ddlDistritoMas.DataBind();

            if (!string.IsNullOrEmpty(peticionario.IdDistrito))
            {
                ddlDistritoMas.SelectedValue = peticionario.IdDistrito;

                ddlDistritoMas.Enabled = false;
            }
            else
            {
                ListItem blank = new ListItem("Cualquier Distrito", "0");
                ddlDistritoMas.Items.Insert(0, blank);
                ddlDistritoMas.SelectedValue = "0";
                //ddlDistrito.Enabled = false;
            }
        }


        protected void ddlUnidadMas_SelectedIndexChanged(object sender, EventArgs e)
        {
            BDManage vectArea;
            BDManage vectRegion;
            BDManage vectDistrito;
            
            vectArea = new BDManage(SQLSentenceAMEC.GetInstancia().getAreasUnidad(ddlUnidadMas.SelectedValue.ToString()), 1);
            rellenaAreaMas(vectArea);
            vectRegion = new BDManage(SQLSentenceAMEC.GetInstancia().getRegionesUnidades(ddlUnidadMas.SelectedValue.ToString()), 1);
            rellenaRegionMas(vectRegion);
            vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoUnidad(ddlUnidadMas.SelectedValue.ToString()), 1);
            rellenaDistritoMas(vectDistrito);

        }

        protected void ddlRegionMas_SelectedIndexChanged(object sender, EventArgs e)
        {

            BDManage vectDistrito = new BDManage();

            if (!string.IsNullOrEmpty(ddlRegionMas.SelectedItem.Value) && !string.IsNullOrEmpty(ddlAreaMas.SelectedItem.Value))
            {
                vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoRegionArea(ddlRegionMas.SelectedItem.Value, ddlAreaMas.SelectedItem.Value), 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(ddlRegionMas.SelectedItem.Value) && string.IsNullOrEmpty(ddlAreaMas.SelectedItem.Value))
                    vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoRegion(ddlRegionMas.SelectedItem.Value), 1);
                if (string.IsNullOrEmpty(ddlRegionMas.SelectedItem.Value) && !string.IsNullOrEmpty(ddlAreaMas.SelectedItem.Value))
                    vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoArea(ddlAreaMas.SelectedItem.Value), 1);
            }
            rellenaDistritoMas(vectDistrito);

        }

        protected void ddlAreaMas_SelectedIndexChanged(object sender, EventArgs e)
        {

            BDManage vectDistrito = new BDManage();

            if (!string.IsNullOrEmpty(ddlRegionMas.SelectedItem.Value) && !string.IsNullOrEmpty(ddlAreaMas.SelectedItem.Value))
            {
                vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoRegionArea(ddlRegionMas.SelectedItem.Value, ddlAreaMas.SelectedItem.Value), 1);
            }
            else
            {
                if (!string.IsNullOrEmpty(ddlRegionMas.SelectedItem.Value) && string.IsNullOrEmpty(ddlAreaMas.SelectedItem.Value))
                    vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoRegion(ddlRegionMas.SelectedItem.Value), 1);
                if (string.IsNullOrEmpty(ddlRegionMas.SelectedItem.Value) && !string.IsNullOrEmpty(ddlAreaMas.SelectedItem.Value))
                    vectDistrito = new BDManage(SQLSentenceAMEC.GetInstancia().getDistritoArea(ddlAreaMas.SelectedItem.Value), 1);
            }

            rellenaDistritoMas(vectDistrito);

        }

        /* FIN DE CAMBIO DE JOSE LAGUNA 23-03-2012 16:50 */

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
            ddlDistrict.DataSource = dList.OrderBy(x => x.IdDistrict);
            ddlDistrict.DataValueField = "iddistrict";
            ddlDistrict.DataTextField = "district";
            ddlDistrict.DataBind();
        }

        protected void ddlDepartament_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            List<DDistrict> dList = new List<DDistrict>();
            List<DSaleForce> sfList = new List<DSaleForce>();
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
            ddlSaleForce.DataSource = sfList.OrderBy(x => x.IdSaleForce).ToList();
            ddlSaleForce.DataValueField = "idsaleforce";
            ddlSaleForce.DataTextField = "saleforce";
            ddlSaleForce.DataBind();


            DDistrict disEmptyItem = new DDistrict() { IdDistrict = -1, District = "Sin Especificar" };
            dList.Add(disEmptyItem);
            ddlDistrict.DataSource = dList.OrderBy(x => x.IdDistrict).ToList();
            ddlDistrict.DataValueField = "iddistrict";
            ddlDistrict.DataTextField = "district";
            ddlDistrict.DataBind();

        }

        //protected void lvAMECs_OnPagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        //{
           
        //}

        //protected void lvAMECs_OnLoad(object sender, EventArgs e)
        //{
        //    int StartRowIndex = 0;
        //    if (Session["StartRowIndex"] != null)
        //    {
        //        StartRowIndex = (int)Session["StartRowIndex"];
        //    }

        //    ListView lv = sender as ListView;
        //    DataPager pager = lv.FindControl("DataPager3") as DataPager;
        //    pager.SetPageProperties(StartRowIndex, pager.PageSize, false);
        //    lvAMECs.DataBind();
        //}
    }
}