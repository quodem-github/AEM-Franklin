using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Reflection;

using System.Text;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.Util;
using EOS.Entidades.Datos;
using EOS.Web;
using EOS;
using EOS.Entidades.Maestros;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace EOS
{
    public partial class FiltroInforme : Page
    {
        protected Peticionario peticionario;
        private BDManage extraccion;
        private BDManage extraccion2;
        protected DataTable dt;
        protected DataTable dt2;
        protected DataTable dtAmecs;
        protected Informe informe;
        protected String[] listAmecs;
        protected String[] listActividad;
        public string filename = string.Empty;
        static XSSFWorkbook hssfworkbook;
        private EOS.Entidades.DetalleExpediente dExpediente = null;
        public bool sePuedeMostrarNuevoAmec = true;
        private const string ID_LANGUAGE_EXPORT_NUMBER = "en-US";
        private List<DDepartament> OriginalDeptList;
        private List<DDistrict> OriginalDistList;
        private List<DSaleForce> OriginalSaleForceList;

        protected void Page_Load(object sender, EventArgs e)
        {
            ///////////////Controlar Si caduca la sesión en la aplicación//////////////
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Session["rolesuser"] == null) Response.Redirect("~/Account/Login.aspx");
            //////////////////////////////////////////////////////////////////////////



            //make sure these are always disabled, also when using IE6
            this.txtIdCongreso.Enabled = false;
            this.txtCongreso.Enabled = false;

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            // Primero se recupera el dato de sesión del expediente y en función de los siguientes casos se elimina o se usa
            // Si ya existe el Expediente en la sesión recupera los valores
            if (Session["NuevoExpediente"] != null)
            {
                dExpediente = (EOS.Entidades.DetalleExpediente)Session["NuevoExpediente"];
            }
            else
            {
                dExpediente = new Entidades.DetalleExpediente();
                Session.Add("NuevoExpediente", dExpediente);
            }

            if (ConfigUtil.GetAppSetting("informesGrupos") == "1")
            {
                rbHonorarios.Visible = true;
                rbGrupos.Visible = true;
            }
            //Carga Inicial
            if (!Page.IsPostBack)
            {
                // Si no tiene participantes asignados, hace la asignación de participantes inicial del AMEC.
                if (dExpediente.dParticipantes == null)
                {
                    AgenteParticipantes agPar = new AgenteParticipantes();
                    dExpediente.dParticipantes = agPar.ObtenerParticipantesExpediente(dExpediente.nIDExpediente.ToString(), string.Empty, 0, 1000);
                }

                DDatosPersonalesUsuario dPeticionario = (DDatosPersonalesUsuario)Session["datosUsuario"];

                string user = dPeticionario.login;
                string pass = dPeticionario.password;

                extraccion = new BDManage(SQLSentence.GetInstancia().login(user, pass), 1);
                dt = extraccion.getDataSet().Tables[0];

                //if(Request.QueryString["id"]!="")
                if (dt.Rows.Count == 1)
                {
                    informe = new Informe();
                    Session["informe"] = informe;
                    peticionario = new Peticionario(dt.Rows[0].ItemArray[0].ToString(), true);
                    //peticionario = new Peticionario(Request.QueryString["id"].ToString());
                    Session["peticionario"] = peticionario;

                    //check administrador

                    rellenaUnidad();
                    rellenaRegion();
                    rellenaArea();
                    rellenaDistrito();
                    rellenaPeticionario();
                    rellenaProducto();
                    rellenarDeptSaleForceDistrictCombos();

                    permisosDDL(peticionario.IdCargo);

                    ListItem blank = new ListItem("Cualquier Tipo", "0");

                    //Asistentes - provisional - busqueda
                    extraccion = new BDManage(SQLSentence.GetInstancia().getAsistentes(), 1);

                    ListItem blank3 = new ListItem("Cualquier Asistente", "0");

                    //Especialidad Asistentes
                    extraccion2 = new BDManage(SQLSentence.GetInstancia().getEspecialidadesAsistentes(), 1);

                    ListItem blank2 = new ListItem("Cualquier Especialidad", "0");

                    ListItem blankFarma = new ListItem("Cualquier Valoración", "0");

                    //Estado reservas
                    extraccion = new BDManage(SQLSentence.GetInstancia().getEstadoReserva(), 1);
                    ddlEstadoReserva.DataSource = extraccion.getDataSet();
                    ddlEstadoReserva.DataValueField = "idestado";
                    ddlEstadoReserva.DataTextField = "estado";
                    ddlEstadoReserva.DataBind();

                    ListItem blank4 = new ListItem("Cualquier Estado", "0");
                    ddlEstadoReserva.Items.Insert(0, blank4);
                    ddlEstadoReserva.SelectedValue = "0";

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



                    //Estado expediente
                    //A la espera de tener una tabla de estados Expediente
                    extraccion = new BDManage(SQLSentence.GetInstancia().getEstadoExpediente(), 1);
                    ddlEstadoExpediente.DataSource = extraccion.getDataSet();
                    ddlEstadoExpediente.DataValueField = "idestado";
                    ddlEstadoExpediente.DataTextField = "estado";
                    ddlEstadoExpediente.DataBind();

                    //ListItem blank2 = new ListItem("Cualquier Estado", "0");
                    ddlEstadoExpediente.Items.Insert(0, blank4);
                    ddlEstadoExpediente.SelectedValue = "0";

                    //Estado valoracion FarmaIndustria
                    extraccion = new BDManage(SQLSentence.GetInstancia().getValoracionFI(), 1);
                    ddlValFarma.DataSource = extraccion.getDataSet();
                    ddlValFarma.DataValueField = "idvaloracionfi";
                    ddlValFarma.DataTextField = "descripcion";
                    ddlValFarma.DataBind();

                    ListItem blank5 = new ListItem(" ", "0");
                    ddlValFarma.Items.Insert(0, blankFarma);
                    ddlValFarma.SelectedValue = "0";
                    
                    // Otras tareas rutinarias
                    RellenarCombos();
                }
                else
                {
                    Response.Redirect("GenericErrorPage.htm");
                }

            }//Segundas recargas
            else
            {
                peticionario = (Peticionario)Session["peticionario"];
                informe = (Informe)Session["informe"];

                //ddlUnidad.SelectedIndex = (int.Parse)(Session["unidad"].ToString());
            }

            // Si no estamos en una pantalla como la de Login en la que no hay parte de cabecera.
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            DDatosPersonalesUsuario datosUsuario = (DDatosPersonalesUsuario)Session["datosUsuario"];
            if (datosUsuario == null)
            {
                datosUsuario = agenteUsu.ObtenerDatosPersonalesPorLogin();
                Session["datosUsuario"] = datosUsuario;
            }

            if (datosUsuario != null)
            {
                ICollection<DEmpleadosGP> datosEmpleados = null;
                if (HttpContext.Current.Session["DatosEmpleados"] != null)
                {
                    datosEmpleados = HttpContext.Current.Session["DatosEmpleados"] as List<DEmpleadosGP>;
                }
                if (datosEmpleados == null)
                {
                    AgenteEmpleados agenteEmp = new AgenteEmpleados();
                    datosEmpleados = agenteEmp.ObtenerDatosEmpleadosPorID(datosUsuario.IdPeticionario);
                    HttpContext.Current.Session["DatosEmpleados"] = datosEmpleados;
                }

                rptEmpleadosGP.DataSource = datosEmpleados;
                rptEmpleadosGP.DataBind();

                if (Page.Request.Url.AbsolutePath.Contains("CambiarDatosPersonales.aspx") || Page.Request.Url.AbsolutePath.Contains("CambiarDatosCompliance.aspx") || Page.Request.Url.AbsolutePath.Contains("NuevoExpedientePasoA.aspx") || System.Configuration.ConfigurationManager.AppSettings["controlPresupuesto"] == "0") { sePuedeMostrarNuevoAmec = false; }

            }
        }

        protected void permisosDDL(string cargo)
        {
            ddlUnidad.Enabled = false;
            ddlArea.Enabled = false;
            ddlRegion.Enabled = false;
            ddlDistrito.Enabled = false;
            ddlDepartament.Enabled = false;
            ddlSaleForce.Enabled = false;
            ddlDistrict.Enabled = false;

            if (string.IsNullOrEmpty(peticionario.IdUnidad))
                ddlUnidad.Enabled = true;

            if (string.IsNullOrEmpty(peticionario.IdArea))
                ddlArea.Enabled = true;
            else
            {
                ddlUnidad.Enabled = false;
            }

            if (string.IsNullOrEmpty(peticionario.IdRegion))
                ddlRegion.Enabled = true;
            else
            {
                ddlUnidad.Enabled = false;
            }

            if (string.IsNullOrEmpty(peticionario.IdDistrito))
                ddlDistrito.Enabled = true;
            else
            {
                ddlUnidad.Enabled = false;
                ddlArea.Enabled = false;
                ddlRegion.Enabled = false;
            }

            if (string.IsNullOrEmpty(peticionario.IdDepartament))
                ddlDepartament.Enabled = true;

            if (string.IsNullOrEmpty(peticionario.IdFuerzaVentas))
                ddlSaleForce.Enabled = true;

            if (string.IsNullOrEmpty(peticionario.IdDistrict))
                ddlDistrict.Enabled = true;

            if (isAdministrador())
            {
                ddlUnidad.Enabled = true;
                ddlArea.Enabled = true;
                ddlRegion.Enabled = true;
                ddlDistrito.Enabled = true;
                ddlDepartament.Enabled = true;
                ddlSaleForce.Enabled = true;
                ddlDistrict.Enabled = true;
            }
        }

        protected void rellenaUnidad()
        {
            //peticionario = (Peticionario)Session["peticionario"];

            ddlUnidad.DataSource = peticionario.VectUnidad.getDataSet();
            ddlUnidad.DataValueField = "idUnidad";
            ddlUnidad.DataTextField = "unidad";
            ddlUnidad.DataBind();

            if (!string.IsNullOrEmpty(peticionario.IdUnidad))
            {
                ddlUnidad.SelectedValue = peticionario.IdUnidad;

                //Para que no se pueda cambiar
                ddlUnidad.Enabled = false;
            }
            else
            {
                ListItem blank = new ListItem("Cualquier Unidad", "0");
                ddlUnidad.Items.Insert(0, blank);
                ddlUnidad.SelectedValue = "0";
                //ddlUnidad.Enabled = false;
            }
        }

        protected void rellenaRegion()
        {
            ddlRegion.DataSource = peticionario.VectRegion.getDataSet();
            ddlRegion.DataValueField = "idregion";
            ddlRegion.DataTextField = "region";
            ddlRegion.DataBind();

            if (!string.IsNullOrEmpty(peticionario.IdRegion))
            {
                ddlRegion.SelectedValue = peticionario.IdRegion;

                ddlRegion.Enabled = false;
            }
            else
            {
                ListItem blank = new ListItem("Cualquier Region", "0");
                ddlRegion.Items.Insert(0, blank);
                ddlRegion.SelectedValue = "0";
            }
        }

        protected void rellenaArea()
        {
            ddlArea.DataSource = peticionario.VectArea.getDataSet();
            ddlArea.DataValueField = "idarea";
            ddlArea.DataTextField = "area";
            ddlArea.DataBind();

            if (!string.IsNullOrEmpty(peticionario.IdArea))
            {
                ddlArea.SelectedValue = peticionario.IdArea;

                //Para que no se pueda cambiar
                ddlArea.Enabled = false;
            }
            else
            {
                ListItem blank = new ListItem("Cualquier Area", "0");
                ddlArea.Items.Insert(0, blank);
                ddlArea.SelectedValue = "0";
                //ddlRegion.Enabled = false;
            }
        }

        protected void rellenaDistrito()
        {
            ddlDistrito.DataSource = peticionario.VectDistrito.getDataSet();
            ddlDistrito.DataValueField = "iddistrito";
            ddlDistrito.DataTextField = "distrito";
            ddlDistrito.DataBind();

            if (!string.IsNullOrEmpty(peticionario.IdDistrito))
            {
                ddlDistrito.SelectedValue = peticionario.IdDistrito;

                ddlDistrito.Enabled = false;
            }
            else
            {
                ListItem blank = new ListItem("Cualquier Distrito", "0");
                ddlDistrito.Items.Insert(0, blank);
                ddlDistrito.SelectedValue = "0";
                //ddlDistrito.Enabled = false;
            }
        }

        protected void rellenaPeticionario()
        {
            ddlPeticionario.DataSource = peticionario.VectPeticionarios.getDataSet();
            ddlPeticionario.DataValueField = "idPeticionario";
            ddlPeticionario.DataTextField = "nombreCompleto";
            ddlPeticionario.DataBind();

            if (!string.IsNullOrEmpty(peticionario.IdPeticionario) && peticionario.Delegado == "True")
            {
                ddlPeticionario.SelectedValue = peticionario.IdPeticionario;

                ddlPeticionario.Enabled = false;
            }
            else
            {
                ListItem blank = new ListItem("Cualquier Peticionario", "0");
                ddlPeticionario.Items.Insert(0, blank);
                ddlPeticionario.SelectedValue = "0";
            }
        }

        private void rellenarDeptSaleForceDistrictCombos()
        {
            AgenteMaestros agente = new AgenteMaestros();

            List<DDepartament> departamentList = agente.GetDepartaments();
            DDepartament deptEmptyItem = new DDepartament() { IdDepartament = -1, Departament = "Sin Especificar" };
            departamentList.Add(deptEmptyItem);
            OriginalDeptList = departamentList.OrderBy(x => x.IdDepartament).ToList();
            ddlDepartament.DataSource = OriginalDeptList;
            ddlDepartament.DataValueField = "iddepartament";
            ddlDepartament.DataTextField = "departament";
            ddlDepartament.DataBind();

            bool isUserAdministrator = isAdministrador();

            if (!string.IsNullOrEmpty(peticionario.IdDepartament))
            {
                if (!isUserAdministrator)
                {
                    ddlDepartament.SelectedValue = peticionario.IdDepartament;
                }
                ddlDepartament.Enabled = false;
            }

            List<DDistrict> districtList = agente.GetDistrict();
            DDistrict disEmptyItem = new DDistrict() { IdDistrict = -1, District = "Sin Especificar" };
            districtList.Add(disEmptyItem);
            OriginalDistList = districtList.OrderBy(x => x.IdDistrict).ToList();
            ddlDistrict.DataSource = OriginalDistList;
            ddlDistrict.DataValueField = "iddistrict";
            ddlDistrict.DataTextField = "district";
            ddlDistrict.DataBind();

            if (!string.IsNullOrEmpty(peticionario.IdDistrict))
            {
                if (!isUserAdministrator)
                {
                    ddlDistrict.SelectedValue = peticionario.IdDistrict;
                }
                ddlDistrict.Enabled = false;
            }

            List<DSaleForce> saleForceList = agente.GetSaleForce();
            DSaleForce saleForEmptyItem = new DSaleForce() { IdSaleForce = -1, SaleForce = "Sin Especificar" };
            saleForceList.Add(saleForEmptyItem);
            OriginalSaleForceList = saleForceList.OrderBy(x => x.IdSaleForce).ToList();
            ddlSaleForce.DataSource = OriginalSaleForceList;
            ddlSaleForce.DataValueField = "idsaleforce";
            ddlSaleForce.DataTextField = "saleforce";
            ddlSaleForce.DataBind();

            if (!string.IsNullOrEmpty(peticionario.IdFuerzaVentas))
            {
                if (!isUserAdministrator)
                {
                    ddlSaleForce.SelectedValue = peticionario.IdFuerzaVentas;
                }
                ddlSaleForce.Enabled = false;
            }
        }

        protected void rellenaProducto()
        {
            getValoresInforme();
            if (peticionario.Marketing == "1")
            {
                extraccion = new BDManage(SQLSentence.GetInstancia().getProductosJefeProducto(peticionario.IdPeticionario), 1);
                dt = extraccion.getDataSet().Tables[0];
                if (dt.Rows.Count == 0) extraccion = new BDManage(SQLSentence.GetInstancia().getParametrosProducto2(informe), 1);
            }

            else extraccion = new BDManage(SQLSentence.GetInstancia().getParametrosProducto2(informe), 1);
        }

        protected void ddlUnidad_Changed(object sender, EventArgs e)
        {
            getValoresInforme();

            peticionario.getVectAreaUnidad(informe.IdUnidad);
            rellenaArea();
            peticionario.getVectRegionUnidad(informe.IdUnidad);
            rellenaRegion();
            peticionario.getVectDistritoUnidad(informe.IdUnidad);
            rellenaDistrito();
            rellenaProducto();
        }

        protected void ddlRegion_Changed(object sender, EventArgs e)
        {
            getValoresInforme();

            peticionario.getVectDistritoRegionUnidad(informe.IdRegion, informe.IdArea);
            peticionario.getVectPeticionarioChangeDISTR_REG(ddlDistrito.SelectedIndex.ToString(), (int.Parse(informe.IdRegion)).ToString());
            rellenaDistrito();
            rellenaPeticionario();
            rellenaProducto();
        }

        protected void ddlArea_Changed(object sender, EventArgs e)
        {
            getValoresInforme();
            peticionario.getVectDistritoRegionUnidad(informe.IdRegion, informe.IdArea);
            rellenaDistrito();
            rellenaProducto();
        }

        protected void ddlDistrito_Changed(object sender, EventArgs e)
        {
            getValoresInforme();

            peticionario.getVectPeticionario((int.Parse(informe.IdDistrito)).ToString());
            rellenaPeticionario();
            rellenaProducto();
        }

        protected void ddlPeticionario_Changed(object sender, EventArgs e)
        {
            getValoresInforme();
            rellenaProducto();
        }

        protected void ddlProducto_Changed(object sender, EventArgs e)
        {
            getValoresInforme();

            rellenaProducto();
        }

        private void AsignarDeptSaleDistAInforme()
        {
            informe.IdDepartament = ddlDepartament.SelectedValue;
            informe.Departament = ddlDepartament.SelectedItem.ToString();
            informe.IdSaleForce = ddlSaleForce.SelectedValue;
            informe.SaleForce = ddlSaleForce.SelectedItem.ToString();
            informe.IdDistrict = ddlDistrict.SelectedValue;
            informe.District = ddlDistrict.SelectedItem.ToString();
        }

        protected void btnInforme_Click(object sender, EventArgs e)
        {
            filename = string.Empty;
            getValoresInforme();

            if (rbExpediente.Checked) filename = "InformeExpediente.xlsx";
            if (rbReserva.Checked) filename = "InformePeticion.xlsx";
            if (rbGrupos.Checked) filename = "InformeGrupos.xlsx";
            if (rbHonorarios.Checked) filename = "InformeHonorarios.xlsx";

            string currenLanguage = CultureInfo.CurrentCulture.Name;
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(ID_LANGUAGE_EXPORT_NUMBER);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            try
            {
                //Asignamos los valores del departamento fuerza de venta y distrito para el filtro de informe.
                AsignarDeptSaleDistAInforme();

                InitializeWorkbook(filename, ConfigUtil.GetAppSetting("PathFicherosExcel"));
                if (rbReserva.Checked || rbExpediente.Checked || rbGrupos.Checked || rbHonorarios.Checked)
                {
                    try
                    {
                        //Parametros Filtro Informe
                        ISheet sheet1 = hssfworkbook.GetSheet("Datos Informe");
                        sheet1.GetRow(1).Cells[1].SetCellValue(informe.NomUnidad);
                        sheet1.GetRow(2).Cells[1].SetCellValue(informe.NomArea);
                        sheet1.GetRow(3).Cells[1].SetCellValue(informe.NomRegion);
                        sheet1.GetRow(4).Cells[1].SetCellValue(informe.NomDistrito);

                        sheet1.GetRow(5).Cells[1].SetCellValue(informe.Departament);
                        sheet1.GetRow(6).Cells[1].SetCellValue(informe.SaleForce);
                        sheet1.GetRow(7).Cells[1].SetCellValue(informe.District);

                        sheet1.GetRow(8).Cells[1].SetCellValue(informe.NomPeticionario);
                        sheet1.GetRow(9).Cells[1].SetCellValue(informe.NomProducto);
                        sheet1.GetRow(10).Cells[1].SetCellValue(informe.NombreActividad);
                        sheet1.GetRow(11).Cells[1].SetCellValue(informe.TipoActividad);
                        sheet1.GetRow(12).Cells[1].SetCellValue(informe.FechaActDesde);
                        sheet1.GetRow(13).Cells[1].SetCellValue(informe.FechaActHasta);

                        if (informe.Asistente != null && informe.Asistente.Count > 0)
                        {
                            string asistentname = string.Empty;
                            string especialityname = string.Empty;
                            foreach (Asistente asistente in informe.Asistente)
                            {
                                if (!string.IsNullOrEmpty(asistente.NombreAsistente)) asistentname += asistente.NombreAsistente + " " + asistente.ApellidoAsistente + ";";
                                if (!string.IsNullOrEmpty(asistente.EspecialidadAsistente)) especialityname += asistente.EspecialidadAsistente + ";";
                            }
                            if (!string.IsNullOrEmpty(asistentname))
                            {
                                asistentname = asistentname.Substring(0, asistentname.LastIndexOf(@";"));
                                sheet1.GetRow(14).Cells[1].SetCellValue(asistentname);
                            }
                            if (!string.IsNullOrEmpty(especialityname))
                            {
                                especialityname = especialityname.Substring(0, especialityname.LastIndexOf(@";"));
                                sheet1.GetRow(15).Cells[1].SetCellValue(especialityname);
                            }
                        }
                        sheet1.GetRow(16).Cells[1].SetCellValue(informe.CodigoAmec);
                        sheet1.GetRow(17).Cells[1].SetCellValue(informe.NumPedido);

                        sheet1.GetRow(18).Cells[1].SetCellValue(informe.NumExpediente);
                        sheet1.GetRow(19).Cells[1].SetCellValue(informe.NumReserva);
                        sheet1.GetRow(20).Cells[1].SetCellValue(informe.EstadoReserva);
                        sheet1.GetRow(21).Cells[1].SetCellValue(informe.EstadoExpediente);
                        sheet1.GetRow(22).Cells[1].SetCellValue(informe.ValoradoFI);
                        sheet1.GetRow(23).Cells[1].SetCellValue(informe.PendientePedido ? "Si" : "No");
                        sheet1.GetRow(24).Cells[1].SetCellValue(informe.FechaDesde);
                        sheet1.GetRow(25).Cells[1].SetCellValue(informe.FechaHasta);
                    }
                    catch (Exception err)
                    {
                        Global.SendApplicationError(err, Request, Session, GetType().Name);
                        Alert.Show("Message2:" + err.Message.ToString() + " / Target:" + err.TargetSite.ToString() + " / Source:" + err.Source);
                    }
                }

                //first check administrator: se puede ver todas las areas, unidades, regiones y distritos
                if (peticionario == null)
                    peticionario = (Peticionario)Session["peticionario"];

                //Código TASIO
                if (rbGrupos.Checked)
                {
                    extraccion = new BDManage(SQLSentence.GetInstancia().getInformeGrupos(informe), 1);
                    dt = extraccion.getDataSet().Tables[0];
                    ISheet sheet2 = hssfworkbook.GetSheet("InformeGrupos");
                    int y = 1;
                    int rowInforme = 0;
                    double porcentajeProducto;
                    string valor;

                    //Columna eliminada por petición del cliente
                    dt.Columns.Remove("Apellido2");
           
                    while (y <= dt.Rows.Count)
                    {
                        IRow row = sheet2.CreateRow(++rowInforme);

                        for (int x = 0; x < 30; x++)
                        {
                            if (string.IsNullOrWhiteSpace(dt.Rows[y - 1].ItemArray[x].ToString()))
                            {
                                valor = string.Empty;
                            }
                            else
                            {
                                valor = dt.Columns[x].DataType == typeof(DateTime) ? 
                                    Convert.ToDateTime(dt.Rows[y - 1].ItemArray[x]).ToString("dd/MM/yy") : 
                                    EOS.Logica.Utility.CleanString(dt.Rows[y - 1].ItemArray[x].ToString());
                            }

                            //Redondear importe a dos decimales
                            if (x == 15 && !string.IsNullOrEmpty(valor))
                                valor = decimal.Round(Convert.ToDecimal(valor), 2).ToString();

                            if (x == 23 && !string.IsNullOrEmpty(valor))
                            {
                                if (valor == "0") valor = "No";
                                if (valor == "1") valor = "Si";

                            }
                            //Mostrar si es colectivo o individual
                            if (x == 18 && !string.IsNullOrEmpty(valor))
                            {
                                if (valor == "2") valor = "Colectivo";
                                if (valor == "1") valor = "Individual";
                                if (valor == "3") valor = "Honorarios";

                            }
                         
                            row.CreateCell(x);

                            if (x == 15 && !string.IsNullOrEmpty(valor) &&  Double.TryParse(valor, NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out porcentajeProducto))
                                row.GetCell(x).SetCellValue(Convert.ToDouble(valor));
                            else                                
                                row.GetCell(x).SetCellValue(valor.Replace("\0", ""));
                        }

                        y++;
                    }

                    MemoryStream ms = new MemoryStream();
                    hssfworkbook.Write(ms);
                    GetFile(ms);
                }
                //FIN Código TASIO

                if (rbHonorarios.Checked)
                {
                    extraccion = new BDManage(SQLSentence.GetInstancia().getInformeDatosAdicionalesPassenger(informe), 1);
                    dt = extraccion.getDataSet().Tables[0];
                    ISheet sheet2 = hssfworkbook.GetSheet("InformeGrupos");
                    int y = 1;
                    int rowInforme = 0;
                    string valor = string.Empty;

                    //Columna eliminada por petición del cliente
                    dt.Columns.Remove("Apellido2");
                    dt.Columns.Remove("iddistrict");
                    dt.Columns.Remove("iddepartament");
                    dt.Columns.Remove("idsaleforce");

                    while (y <= dt.Rows.Count)
                    {
                        IRow row = sheet2.CreateRow(++rowInforme);

                        for (int x = 0; x < dt.Columns.Count; x++)
                        {
                            if (x >= 0 && x < 23 || x > 45)
                            {
                                var position = x;

                                if (x > 45)
                                {
                                    position = x - 24;
                                }

                                if (string.IsNullOrWhiteSpace(dt.Rows[y - 1].ItemArray[x].ToString()))
                                {
                                    valor = string.Empty;
                                }
                                else
                                {
                                    if (dt.Columns[x].DataType == typeof(DateTime))
                                    {
                                        valor = !string.IsNullOrWhiteSpace(dt.Rows[y - 1].ItemArray[x].ToString()) ?
                                            Convert.ToDateTime(dt.Rows[y - 1].ItemArray[x]).ToString("dd/MM/yy") :
                                            string.Empty;
                                    }
                                    else
                                    {
                                        valor = EOS.Logica.Utility.CleanString(dt.Rows[y - 1].ItemArray[x].ToString());
                                    }
                                }


                                //Redondear importe a dos decimales
                                if (x == 15 && !string.IsNullOrEmpty(valor))
                                    valor = decimal.Round(Convert.ToDecimal(valor), 2).ToString();

                                //Mostrar si es colectivo o individual
                                if (x == 14 && !string.IsNullOrEmpty(valor))
                                {
                                    if (valor == "2") valor = "Colectivo";
                                    if (valor == "1") valor = "Individual";
                                    if (valor == "3") valor = "Honorarios";

                                }
                                if ((x == 19 || x == 21) && !string.IsNullOrEmpty(valor))
                                {
                                    if (valor == "0") valor = "No";
                                    if (valor == "1") valor = "Si";
                                }
                                
                                row.CreateCell(position);

                                if (x == 15 && !string.IsNullOrEmpty(valor))
                                    row.GetCell(position).SetCellValue(Convert.ToDouble(valor));
                                else
                                    row.GetCell(position).SetCellValue(valor);
                            }
                        }

                        y++;
                    }

                    MemoryStream ms = new MemoryStream();
                    hssfworkbook.Write(ms);
                    GetFile(ms);
                }


                if (rbExpediente.Checked)
                {
                    extraccion = new BDManage(SQLSentence.GetInstancia().getInfoExpediente(informe), 1);
                    dt = extraccion.getDataSet().Tables[0];
                    ISheet sheet2 = hssfworkbook.GetSheet("InformeExpediente");
                    int y = 1;
                    int rowInforme = 0;
                    double porcentajeProducto;
                    string valor;


                    dt.Columns.Remove("iddistrict");
                    dt.Columns.Remove("iddepartament");
                    dt.Columns.Remove("idsaleforce");

                    while (y <= dt.Rows.Count)
                    {
                        IRow row = sheet2.CreateRow(++rowInforme);

                        for (int x = 0; x < 29; x++)
                        {
                            
                            if (string.IsNullOrWhiteSpace(dt.Rows[y - 1].ItemArray[x].ToString()))
                            {
                                valor = string.Empty;
                            }
                            else
                            {
                                valor = dt.Columns[x].DataType == typeof(DateTime) ? 
                                      Convert.ToDateTime(dt.Rows[y - 1].ItemArray[x]).ToString("dd/MM/yyyy") 
                                    : EOS.Logica.Utility.CleanString(dt.Rows[y - 1].ItemArray[x].ToString());
                            }

                            row.CreateCell(x);

                            if ((x == 18 || x == 19 || x == 20 || x == 21 || x == 22) && !string.IsNullOrEmpty(valor) && Double.TryParse(valor, NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out porcentajeProducto))
                            {
                                //row.GetCell(x).SetCellValue(Convert.ToDouble(valor));
                                valor = decimal.Round(Convert.ToDecimal(valor.Replace(",",".")), 2).ToString();
                                row.GetCell(x).SetCellValue(Convert.ToDouble(valor));                                
                            }
                            else
                            {
                                row.GetCell(x).SetCellValue(valor);
                            }
                        }

                        y++;
                    }

                    MemoryStream ms = new MemoryStream();
                    hssfworkbook.Write(ms);
                    GetFile(ms);
                }

                //Esta opcion en principio nunca esta disponible
                if (rbReserva.Checked)
                {
                    extraccion = new BDManage(SQLSentence.GetInstancia().getInformeReserva(informe), 1);
                    dt = extraccion.getDataSet().Tables[0];
                    //Ismael Ameller 03-02-2011 Cambio de nombre
                    ISheet sheet2 = hssfworkbook.GetSheet("InformePeticion");
                    int y = 1;
                    int rowInforme = 0;
                    double porcentajeProducto;
                    string valor;

                    while (y <= dt.Rows.Count)
                    {
                        IRow row = sheet2.CreateRow(++rowInforme);

                        for (int x = 0; x < 25; x++)
                        {
                            
                            if (string.IsNullOrWhiteSpace(dt.Rows[y - 1].ItemArray[x].ToString()))
                            {
                                valor = string.Empty;
                            }
                            else
                            {
                                valor = dt.Columns[x].DataType == typeof(DateTime) ? 
                                    Convert.ToDateTime(dt.Rows[y - 1].ItemArray[x]).ToString("dd/MM/yy") : 
                                    EOS.Logica.Utility.CleanString(dt.Rows[y - 1].ItemArray[x].ToString());

                            }
                            row.CreateCell(x);

                            if ((x == 20) && !string.IsNullOrEmpty(valor) && Double.TryParse(valor, NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out porcentajeProducto))
                            {
                                row.GetCell(x).SetCellValue(Convert.ToDouble(valor));
                            }
                            else
                            {
                                row.GetCell(x).SetCellValue(valor);
                            }
                        }

                        y++;
                    }

                    MemoryStream ms = new MemoryStream();
                    hssfworkbook.Write(ms);
                    GetFile(ms);
                }
            }
            catch (Exception err)
            {
                Global.SendApplicationError(err, Request, Session, GetType().Name);
                Alert.Show("Message4:" + err.Message + " / Target:" + err.TargetSite + " / Source:" + err.Source);
            }

            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(currenLanguage);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
        }

        private void ConvertPercentCellType(string valor, int column, HSSFRow row, double porcentajeProducto)
        {
            if (!string.IsNullOrEmpty(valor))
            {
                if (dt.Columns[column].DataType == typeof(System.Int32))
                {
                    row.GetCell(column).SetCellValue(TreatPercent(column, Convert.ToInt32(valor), porcentajeProducto));
                }
                else if (dt.Columns[column].DataType == typeof(System.Int64))
                {
                    row.GetCell(column).SetCellValue(TreatPercent(column, Convert.ToInt64(valor), porcentajeProducto));
                }
                else if (dt.Columns[column].DataType == typeof(System.Double))
                {
                    Decimal vdecimal = Convert.ToDecimal(valor);
                    vdecimal = (Math.Round(vdecimal, 2));
                    row.GetCell(column).SetCellValue(TreatPercent(column, Convert.ToDouble(vdecimal), porcentajeProducto));
                }
                else if (dt.Columns[column].DataType == typeof(System.Single))
                {
                    row.GetCell(column).SetCellValue(TreatPercent(column, Convert.ToDouble(valor), porcentajeProducto));
                }
                else
                {
                    row.GetCell(column).SetCellValue(valor);
                }
            }
            else
            {
                row.GetCell(column).SetCellValue(valor);
            }
        }

        private double TreatPercent(int column, double valor, double porcentajeProducto)
        {
            if (Constantes.NAMESIMPORTES.Contains(dt.Columns[column].ColumnName))
                return (valor * porcentajeProducto) / 100;
            else
                return valor;
        }

        protected bool isAdministrador()
        {
            BDManage adminextract = new BDManage(SQLSentence.GetInstancia().getAdministratorSentencia(peticionario.IdPeticionario), 1);
            if (adminextract.getDataSet() != null && adminextract.getDataSet().Tables[0] != null)
            {
                DataTable dt = adminextract.getDataSet().Tables[0];
                if (dt.Rows.Count == 1)
                {
                    DataRow row = dt.Rows[0];
                    if (row["administrador"].ToString() == "1" || row["administrador"].ToString().ToLower() == "true")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected void btn_BuscarActividad(object sender, EventArgs e)
        {
            getValoresInforme();
            //lbErrorFindActividad.Visible = false;
            extraccion = new BDManage(SQLSentence.GetInstancia().buscaActividades(informe), 1);
            dt = extraccion.getDataSet().Tables[0];
            Session["DTActividades"] = dt;
            if (dt.Rows.Count != 0)
            {
                ListItem blank = new ListItem("Actividades", "0");
            }
        }

        protected void btn_BuscarAsistente(object sender, EventArgs e)
        {
            getValoresInforme();
            //lbErrorFindActividad.Visible = false;
            extraccion = new BDManage(SQLSentence.GetInstancia().buscaAsistentes(informe), 1);
            dt = extraccion.getDataSet().Tables[0];
            Session["DTAsistentes"] = dt;
            if (dt.Rows.Count != 0)
            {
                ListItem blank = new ListItem("Asistentes", "0");
            }
            else
            {
                //lbErrorFindAsistente.Visible = true;
            }
        }

        protected void getValoresInforme()
        {
            try
            {
                string clientID = Request.Params[ddlUnidad.ClientID];
                if (clientID == null)
                {
                    informe.IdUnidad = ddlUnidad.SelectedValue;
                    if (ddlUnidad.SelectedItem != null) informe.NomUnidad = ddlUnidad.SelectedItem.ToString();
                }
                else
                {
                    //if (((int.Parse)(Request.Params[ddlUnidad.ClientID])) != null)
                    if ((int.Parse(clientID)) != 0)
                    {
                        informe.IdUnidad = ((int.Parse(clientID))).ToString();
                        informe.NomUnidad = ddlUnidad.SelectedItem.ToString();
                    }
                }
            }
            catch (Exception err)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), err);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                informe.IdUnidad = ddlUnidad.SelectedValue.ToString();
                Alert.Show(err.Message);
            }

            try
            {
                string clientID = Request.Params[ddlArea.ClientID];
                if (clientID == null)
                {
                    informe.IdArea = ddlArea.SelectedValue;
                    if (ddlArea.SelectedItem != null) informe.NomArea = ddlArea.SelectedItem.ToString();
                }
                else
                {
                    if ((int.Parse(clientID)) != 0)
                    {
                        informe.IdArea = (int.Parse(clientID)).ToString();
                        informe.NomArea = ddlArea.SelectedItem.ToString();
                    }
                }
            }
            catch (Exception err)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), err);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                informe.IdArea = ddlArea.SelectedValue;
                Alert.Show(err.Message);
            }

            try
            {
                string clientID = Request.Params[ddlRegion.ClientID];
                if (clientID == null)
                {
                    informe.IdRegion = ddlRegion.SelectedValue;
                    if (ddlRegion.SelectedItem != null) informe.NomRegion = ddlRegion.SelectedItem.ToString();
                }
                else
                {
                    if ((int.Parse(clientID)) != 0)
                    {
                        informe.IdRegion = (int.Parse(clientID)).ToString();
                        informe.NomRegion = ddlRegion.SelectedItem.ToString();
                    }
                }
            }
            catch (Exception err)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), err);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                informe.IdRegion = ddlRegion.SelectedValue.ToString();
                Alert.Show(err.Message);
            }

            try
            {
                string clientID = Request.Params[ddlDistrito.ClientID];
                if (clientID == null)
                {
                    informe.IdDistrito = ddlDistrito.SelectedValue;
                    if (ddlDistrito.SelectedItem != null) informe.NomDistrito = ddlDistrito.SelectedItem.ToString();
                }
                else
                {
                    if ((int.Parse(clientID)) != 0)
                    {
                        informe.IdDistrito = (int.Parse(clientID)).ToString();
                        informe.NomDistrito = ddlDistrito.SelectedItem.ToString();
                    }
                }
            }
            catch (Exception err)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), err);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                informe.IdDistrito = ddlDistrito.SelectedValue;
                Alert.Show(err.Message);
            }

            try
            {
                string clientID = Request.Params[ddlPeticionario.ClientID];
                if (clientID == null)
                {
                    informe.IdPeticionario = ddlPeticionario.SelectedValue;
                    if (ddlPeticionario.SelectedItem != null) informe.NomPeticionario = ddlPeticionario.SelectedItem.ToString();
                }
                else
                {
                    if ((int.Parse(clientID)) != 0)
                    {
                        informe.IdPeticionario = (int.Parse(clientID)).ToString();
                        informe.NomPeticionario = ddlPeticionario.SelectedItem.ToString();
                    }
                }
            }
            catch (Exception err)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), err);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                informe.IdPeticionario = ddlPeticionario.SelectedValue;
                Alert.Show(err.Message);
            }

            try
            {
                string clientID = Request.Params[ddltiposDeProductos.ClientID];
                if (clientID == null)
                {
                    informe.IdProducto = ddltiposDeProductos.SelectedValue;
                    if (ddltiposDeProductos.SelectedItem != null) informe.NomProducto = ddltiposDeProductos.SelectedItem.ToString();
                }
                else
                {
                    if ((int.Parse(clientID)) != 0)
                    {
                        informe.IdProducto = (int.Parse(clientID)).ToString();
                        informe.NomProducto = ddltiposDeProductos.SelectedItem.ToString();
                    }
                }
            }
            catch (Exception err)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), err);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                informe.IdProducto = ddltiposDeProductos.SelectedValue;
                Alert.Show(err.Message);
            }

            if (Page.IsPostBack)
            {
                informe.IdCongreso = txtIdCongreso.Text;
                informe.NombreActividad = txtCongreso.Text;
                informe.IdTipoActividad = ddlTipoActividad.SelectedValue.ToString();
                informe.TipoActividad = ddlTipoActividad.SelectedItem.ToString();
                //informe.IdEspecialidadCongreso = ddlEspecialidad.SelectedValue.ToString();
                //informe.EspecialidadCongreso = ddlEspecialidad.SelectedItem.ToString();
                informe.FechaActDesde = txtFechaActDesde.InputTextBox.Text.ToString();
                informe.FechaActHasta = txtFechaActHasta.InputTextBox.Text.ToString();

                //informe.IdAsistente = ddlAsistente.SelectedValue.ToString();
                //Ismael Ameller  Vidal 04-03-2011 Limpia la lista de asistentes
                if (informe.Asistente != null)
                {
                    informe.Asistente.Clear();
                }
                //FIN Ismael Ameller  Vidal 04-03-2011 Limpia la lista de asistentes
                if (dExpediente.dParticipantes != null && dExpediente.dParticipantes.Count > 0)
                {
                    informe.Asistente = new List<Asistente>();
                    foreach (DVParticipante participante in dExpediente.dParticipantes)
                    {
                        Asistente asistente = new Asistente();
                        asistente.IdAsistente = participante.IdPassengerlist.ToString();
                        asistente.NombreAsistente = participante.Nombre;
                        //Ismael Ameller 28-02-2011 Agregamos el apellido del asistente a la información del asistente
                        asistente.ApellidoAsistente = participante.Apel1;
                        //FIN Ismael Ameller 28-02-2011 Agregamos el apellido del asistente a la información del asistente
                        asistente.IdEspecialidadAsistente = participante.CodEspecialidad;
                        asistente.EspecialidadAsistente = participante.Especialidad;
                        informe.Asistente.Add(asistente);
                    }
                }
                if (this.lblAMEC.Text != "(Seleccione un AMEC válido)")
                    informe.CodigoAmec = lblAMEC.Text.ToString();
                informe.NumPedido = txtNPedido.Text.ToString();
                informe.IdEstadoReserva = ddlEstadoReserva.SelectedValue.ToString();
                informe.EstadoReserva = ddlEstadoReserva.SelectedItem.ToString();
                //Ismael Ameller 01-03-2011 Añado Numero de expediente y numero de reserva
                informe.NumExpediente = txtNumExpediente.Text.ToString();
                informe.CodigoAmec = txtNumAmec.Text.ToString();
                //FIN Ismael Ameller 01-03-2011 Añado Numero de expediente y numero de reserva
                informe.IdEstadoExpediente = ddlEstadoExpediente.SelectedValue.ToString();
                informe.EstadoExpediente = ddlEstadoExpediente.SelectedItem.ToString();
                informe.IdValoradoFI = ddlValFarma.SelectedIndex.ToString();
                informe.ValoradoFI = ddlValFarma.SelectedItem.ToString();
                //Ismael Ameller 28-02-2011 Elimino Estado del Amec
                //informe.IdEstadoAmec = ddlEstadoAmec.SelectedValue.ToString();
                //FIN Ismael Ameller 28-02-2011 Elimino Estado del Amec
                informe.FechaDesde = txtFechaDesde.InputTextBox.Text.ToString();
                informe.FechaHasta = txtFechaHasta.InputTextBox.Text.ToString();

                //Ismael Ameller 01-03-2011 Nueva propiedad en formato Datetime
                informe.FechaDesdeDt = null;
                informe.FechaHastaDt = null;
                if (!String.IsNullOrEmpty(txtFechaDesde.InputTextBox.Text)) informe.FechaDesdeDt = Convert.ToDateTime(txtFechaDesde.InputTextBox.Text);
                if (!String.IsNullOrEmpty(txtFechaHasta.InputTextBox.Text)) informe.FechaHastaDt = Convert.ToDateTime(txtFechaHasta.InputTextBox.Text);
                //FIN Ismael Ameller 01-03-2011 Nueva propiedad en formato Datetime

                if (PendienteSi.Checked && !PendienteNo.Checked)
                    informe.PendientePedido = true;
                else
                    informe.PendientePedido = false;

                //check whether the option has been selected at all
                if (!PendienteSi.Checked && !PendienteNo.Checked)
                    informe.PendientePedidoSeleccionado = false;
                else
                    informe.PendientePedidoSeleccionado = true;

                if (chkDatosHojas.Checked) informe.HojasDato = true;
                else informe.HojasDato = false;
            }
            Session["informe"] = informe;
        }

        public string getLetra(int z)
        {
            switch (z)
            {
                case 0: return "A";
                case 1: return "B";
                case 2: return "C";
                case 3: return "D";
                case 4: return "E";
                case 5: return "F";
                case 6: return "G";
                case 7: return "H";
                case 8: return "I";
                case 9: return "J";
                case 10: return "K";
                case 11: return "L";
                case 12: return "M";
                case 13: return "N";
                case 14: return "O";
                case 15: return "P";
                case 16: return "Q";
                case 17: return "R";
                case 18: return "S";
                case 19: return "T";
                case 20: return "U";
                case 21: return "V";
                case 22: return "W";
                case 23: return "X";
                case 24: return "Y";
                case 25: return "Z";
                case 26: return "AA";
                case 27: return "AB";
                case 28: return "AC";
                case 29: return "AD";
                case 30: return "AF";
                case 31: return "AG";
            }
            return " ";
        }

        public String[] dt2ArrayString(DataTable tabla, int ncampo)
        {
            int i = 0;
            String[] lista = new String[tabla.Rows.Count];

            lista[i] = tabla.Rows[i].ItemArray[ncampo].ToString();
            i++;

            while (i < tabla.Rows.Count)
            {
                if (tabla.Rows[i].ItemArray[ncampo].ToString() != tabla.Rows[i - 1].ItemArray[ncampo].ToString())
                {
                    lista[i] = tabla.Rows[i].ItemArray[ncampo].ToString();
                }
                i++;
            }
            return lista;
        }

        static void InitializeWorkbook(string nombreplantilla, string path)
        {
            if (!path.EndsWith(@"\")) path += @"\";

            string file = nombreplantilla;
            FileStream plantilla = new FileStream(path + file, FileMode.Open, FileAccess.Read);

            hssfworkbook = new XSSFWorkbook(plantilla);
        }

        private void GetFile(MemoryStream mstream)
        {
            //Create and populate a memorystream with the contents of the
            //database table
            //System.IO.MemoryStream mstream = GetData();
            //Convert the memorystream to an array of bytes.
            byte[] byteArray = mstream.ToArray();
            //Clean up the memory stream
            mstream.Flush();
            mstream.Close();
            // Clear all content output from the buffer stream
            Response.ClearHeaders();
            Response.Clear();
            // Add a HTTP header to the output stream that specifies the default filename
            // for the browser's download dialog
            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename.Replace(' ', '_'));
            // Add a HTTP header to the output stream that contains the
            // content length(File Size). This lets the browser know how much data is being transfered
            Response.AddHeader("Content-Length", byteArray.Length.ToString());
            // Set the HTTP MIME type of the output stream
            //Response.ContentType = "application/vnd.ms-excel";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            // Write the data out to the client.
            Response.BinaryWrite(byteArray);
            Response.End();
        }

        protected void clickDesbloquea(object sender, EventArgs e)
        {
            permisosDDL(peticionario.IdCargo);
        }

        private void RellenarCombos()
        {
            AgenteMaestros agente = new AgenteMaestros();
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
            this.ddlTipoActividad.DataSource = agente.ObtenerTiposActividad("(Todas)", datosRoles.administrador, datosRoles.newco);
            this.ddlTipoActividad.DataBind();

            this.ddlPoblacion.DataSource = agente.ObtenerPoblaciones("(Todas)", 1);
            this.ddlPoblacion.DataBind();

            //Ismael Ameller 23-03-2011 Carga Combo de productos
            //Xavier Morell
            //AgenteUsuarios agenteUsu = new AgenteUsuarios();
            //DDatosPersonalesUsuario datosUsuario = agenteUsu.ObtenerDatosPersonalesPorLogin();
            DDatosPersonalesUsuario datosUsuario = (DDatosPersonalesUsuario)Session["datosUsuario"];
            if (datosUsuario.admin.HasValue)
                this.ddltiposDeProductos.DataSource = agente.ObtenerProductos(null);
            else
                this.ddltiposDeProductos.DataSource = agente.ObtenerProductos(datosUsuario.Idarea);
            //this.ddltiposDeProductos.DataSource = agente.ObtenerProductos(datosUsuario.Idarea);
            this.ddltiposDeProductos.DataTextField = "Producto";
            this.ddltiposDeProductos.DataValueField = "idareaProducto";
            this.ddltiposDeProductos.DataBind();
            ListItem blanco = new ListItem("Cualquier Producto", "0");
            ddltiposDeProductos.Items.Insert(0, blanco);
            //Ismael Ameller 23-03-2011 Carga Combo de productos
        }

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

        #region Actividades
        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            //vaciar Congreso, eligen nuevo...
            txtIdCongreso.Text = string.Empty;
            txtCongreso.Text = string.Empty;

            this.eosContentResults.Visible = true;
            lvActividades.Visible = true;
            lvActividades.DataSourceID = "odsActividades";
            lvActividades.DataBind();
        }

        protected void lvActividades_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvActividades.SelectedValue != null)
            {
                SeleccionaActividad(lvActividades.SelectedValue.ToString(), null);
                //lvActividades.DataSourceID = string.Empty;
                //lvActividades.DataSource = null;
                //lvActividades.DataBind();
                lvActividades.Visible = false;
            }
        }

        protected void SeleccionaActividad(string sIdActividad, string sIdAMEC)
        {
            AgenteExpedientes agenteExp = new AgenteExpedientes();
            DCabeceraActividad miActividad = agenteExp.ObtenerActividadPorID(sIdActividad);
            DAmec miAMEC = null;

            if (!string.IsNullOrEmpty(sIdAMEC))
            {
                miAMEC = agenteExp.ObtenerEntidadAMECporID(sIdAMEC);
            }

            if (miActividad != null)
            {
                //dExpediente.nIDCogreso = miActividad.IdCongreso;
                //dExpediente.sCongreso = miActividad.Congreso;
                //dExpediente.AMEC = miAMEC;
                //dExpediente.nIDAMEC = (miAMEC != null) ? miAMEC.idamec : 0;
                this.txtIdCongreso.Text = miActividad.IdCongreso.ToString();
                this.txtCongreso.Text = miActividad.Congreso;

                //RellenaDatosExpediente();
            }
        }

        protected void odsActividades_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNombre1.Text.Trim())) e.InputParameters["filtroNombre1"] = "%" + txtNombre1.Text.Trim() + "%";
            if (!string.IsNullOrEmpty(ddlPoblacion.SelectedValue.Trim())) e.InputParameters["filtroPoblacion"] = ddlPoblacion.SelectedValue.Trim();
            if (!string.IsNullOrEmpty(txtAMEC.Text.Trim())) e.InputParameters["filtroAMEC"] = "%" + txtAMEC.Text.Trim() + "%";
            if (!string.IsNullOrEmpty(ddlTipoActividad.SelectedValue.Trim())) e.InputParameters["filtroTipoActividad"] = ddlTipoActividad.SelectedValue.Trim();
            if (!string.IsNullOrEmpty(txtFechaActDesde.InputTextBox.Text.Trim()))
            {
                DateTime dtTest;
                if (DateTime.TryParse(txtFechaActDesde.InputTextBox.Text.Trim(), out dtTest))
                    e.InputParameters["filtroFechaDesde"] = txtFechaActDesde.InputTextBox.Text.Trim();
                else
                    txtFechaActDesde.InputTextBox.Text = string.Empty;
            }
            if (!string.IsNullOrEmpty(txtFechaActHasta.InputTextBox.Text.Trim()))
            {
                DateTime dtTest;
                if (DateTime.TryParse(txtFechaActHasta.InputTextBox.Text.Trim(), out dtTest))
                    e.InputParameters["filtroFechaHasta"] = txtFechaActHasta.InputTextBox.Text.Trim();
                else
                    txtFechaActHasta.InputTextBox.Text = string.Empty;

            }
        }
        #endregion

        #region AMEC
        protected void lvAmecs_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sSelectedIdAmec = null;
            if (lvAmecs.SelectedValue != null)
            {
                sSelectedIdAmec = lvAmecs.SelectedValue.ToString();
            }
            AgenteExpedientes agenteExp = new AgenteExpedientes();

            DVAmecCongreso miAmecCongreso = agenteExp.ObtenerAMECPorActividad(txtIdCongreso.Text).FirstOrDefault();

            DAmec miAmec = agenteExp.ObtenerEntidadAMECporID(sSelectedIdAmec.ToString());
            if (miAmec != null)
            {
                this.lblAMEC.Text = miAmec.amec;
            }
            else { this.lblAMEC.Text = "(Seleccione un AMEC válido)"; }
        }
        #endregion

        #region Asistente
        protected void btnFiltrar2_Click(object sender, EventArgs e)
        {
            //this.eosContentResults.Visible = true;
            lvParticipantes2.Visible = true;
            emptyTable.Visible = true;
            lvParticipantes2.DataSourceID = "odsParticipantes";
            lvParticipantes2.DataBind();
        }

        protected void lvServicioParticipantes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvServicioParticipantes.SelectedValue != null)
            {
                DVParticipante dvRemove = dExpediente.dParticipantes.FirstOrDefault(f => f.IdPassengerlist == (int)lvServicioParticipantes.SelectedValue);
                dExpediente.dParticipantes.Remove(dvRemove);
                this.lvParticipantes2.DataBind();

                //ControlaCheckIndividual();
            }
        }

        protected void lvParticipantes2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvParticipantes2.SelectedValue != null)
            {
                AgenteParticipantes agPar = new AgenteParticipantes();
                DVParticipante dvAdd = agPar.ObtenerParticipantePorID(lvParticipantes2.SelectedValue.ToString());
                dExpediente.dParticipantes.Add(dvAdd);

                lvServicioParticipantes.Visible = true;
                lvServicioParticipantes.DataSourceID = "odsParticipantesExpediente";
                lvServicioParticipantes.DataBind();
            }
        }

        protected void odsParticipantes_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNombre2.Text.Trim())) e.InputParameters["filtroNombre2"] = "%" + txtNombre2.Text.Trim() + "%";
            if (!string.IsNullOrEmpty(txtApel1.Text.Trim())) e.InputParameters["filtroApel1"] = "%" + txtApel1.Text.Trim() + "%";
            //if (!string.IsNullOrEmpty(txtApel2.Text.Trim())) e.InputParameters["filtroApel2"] = "%" + txtApel2.Text.Trim() + "%";
            if (!string.IsNullOrEmpty(txtHospital.Text.Trim())) e.InputParameters["filtroHospital"] = "%" + txtHospital.Text.Trim() + "%";

            if (dExpediente != null)
            {
                e.InputParameters["filtroIDExpediente"] = dExpediente.nIDExpediente.ToString();
            }

            //AgenteUsuarios agenteUsu = new AgenteUsuarios();
            //DDatosPersonalesUsuario datosUsuario = agenteUsu.ObtenerDatosPersonalesPorLogin();
            DDatosPersonalesUsuario datosUsuario = (DDatosPersonalesUsuario)Session["datosUsuario"];
            if (datosUsuario.iddistrito.HasValue) { e.InputParameters["filtroIDDistrito"] = datosUsuario.iddistrito.Value.ToString(); }
        }

        protected void odsParticipantesExpediente_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (dExpediente != null)
            {
                e.InputParameters["filtroIDExpediente"] = dExpediente.nIDExpediente.ToString();
            }
        }
        #endregion

        #region AñadirParticipantesMarilo

        protected void btnNuevoParticipante_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect((String.Format("~/NuevoParticipante.aspx")));
        }
        #endregion
    }
}
