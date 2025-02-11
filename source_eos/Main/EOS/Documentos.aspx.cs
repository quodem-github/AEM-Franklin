using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Entidades.Datos;
using EOS.Web;

namespace EOS
{
    public partial class Documentos : System.Web.UI.Page
    {
        public DCabeceraExpedienteAmpliado expediente
        {
            get
            {
                return (DCabeceraExpedienteAmpliado)HttpContext.Current.Session["currentExpediente"];
            }

            set
            {
                HttpContext.Current.Session["currentExpediente"] = value;
                HttpContext.Current.Session["currentFKIdCongreso"] = value.Idactividad;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["key"] == null)
                    throw new ArgumentNullException("key", "El argumento key con la clave de documento no ha sido facilitado");

                AgenteExpedientes agenteExp = new AgenteExpedientes();
                string key = Request.QueryString["key"];
                string idexp = EOS.Web.Encriptacion.Decrypt(key.Substring(0, key.Length - 4), "Documents" + key.Substring(key.Length - 4));
                this.expediente = agenteExp.ObtenerExpedientePorID(idexp);

                this.btnSalir.HRef = "Expedientes.aspx";

                // Información de cabecera
                this.initInfoPanel();

                CargaPanel("Name", SortDirection.Ascending);
                //Ismael Ameller 09-03-2011 Añado dos labels
                this.lblFechaDesde.Text = string.Format("{0:dd/MM/yyyy }", expediente.FechaDesde);
                this.LblFechaHasta.Text = string.Format("{0:dd/MM/yyyy }", expediente.FechaHasta);
                this.LblPoblacion.Text = expediente.Poblacion;
                CalculaEstados();
                //FIN Ismael Ameller 09-03-2011 Añado dos labels
            }
        }

        //Ismael Ameller 09-03-2011 Añado Label Estado
        private void CalculaEstados()
        {
            AgenteExpedientes agExp = new AgenteExpedientes();
            lblEstado.CssClass = string.Format("eosImagenEstado eosImagenLeyenda{0}", expediente.Idestado);
            if (expediente.Idestado == "AB")
            {
                this.lblEstado.Text = agExp.ObtenerLabelEstado("AB");
            }
            else if (expediente.Idestado == "NC")
            {
                this.lblEstado.Text = agExp.ObtenerLabelEstado("NC");
            }
            else if (expediente.Idestado == "FZ")
            {
                this.lblEstado.Text = agExp.ObtenerLabelEstado("FZ");
            }
            else if (expediente.Idestado == "CR")
            {
                this.lblEstado.Text = agExp.ObtenerLabelEstado("CR");
            }
            else if (expediente.Idestado == "CN")
            {
                this.lblEstado.Text = agExp.ObtenerLabelEstado("CN");
            }
            else if (expediente.Idestado == "CNTR")
            {
                this.lblEstado.Text = agExp.ObtenerLabelEstado("CNTR");
            }
            else if (expediente.Idestado == "CFP")
            {
                this.lblEstado.Text = agExp.ObtenerLabelEstado("CFP");
            }
            else if (expediente.Idestado == "ACP")
            {
                this.lblEstado.Text = agExp.ObtenerLabelEstado("ACP");
            }
            else if (expediente.Idestado == "AC")
            {
                this.lblEstado.Text = agExp.ObtenerLabelEstado("AC");
            }
            else if (expediente.Idestado == "TR")
            {
                this.lblEstado.Text = agExp.ObtenerLabelEstado("TR");
            }
            else
            {
                this.lblEstado.Text = agExp.ObtenerLabelEstado("CN");
            }
        }

        //FIN Ismael Ameller 09-03-2011 Añado Label Estado
        public void CargaPanel(string sSort, SortDirection sDirection)
        {
            // Crea tabla de archivos
            DataTable dt = new DataTable("Documents");
            dt.Columns.Add("iddoc", System.Type.GetType("System.String"));
            dt.Columns.Add("tipo", System.Type.GetType("System.String"));
            dt.Columns.Add("documento", System.Type.GetType("System.String"));
            dt.Columns.Add("fecha", System.Type.GetType("System.DateTime"));

            // Recupera archivos
            string dir = ConfigUtil.GetAppSetting("PathDocuments") + this.expediente.Idexpediente.ToString();

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            DirectoryInfo di = new DirectoryInfo(dir);
            FileSystemInfo[] files = di.GetFiles();

            var orderedFiles = files.OrderBy(f => f.Name);

            foreach (FileSystemInfo fsFile in orderedFiles)
            {
                DataRow dr = dt.NewRow();
                switch (fsFile.Extension)
                {
                    case ".doc":
                    case ".docx": dr["iddoc"] = "docx.png"; break;
                    case ".xls":
                    case ".xlsx": dr["iddoc"] = "xlsx.png"; break;
                    case ".ppt":
                    case ".pptx": dr["iddoc"] = "pptx.png"; break;
                    case ".pdf": dr["iddoc"] = "pdf.png"; break;
                    case ".png":
                    case ".jpg":
                    case ".gif":
                    case ".bmp": dr["iddoc"] = "image.png"; break;
                    default: dr["iddoc"] = "otherdoc.png"; break;
                }
                dr["tipo"] = fsFile.Extension;
                dr["documento"] = fsFile.Name;
                dr["fecha"] = fsFile.LastWriteTime;
                dt.Rows.Add(dr);
            }
            this.lvDocumentos.DataSource = dt;
            this.lvDocumentos.DataBind();
        }

        private void initInfoPanel()
        {
            this.pedidoLabelValue.Text = this.expediente.Pedido;
            //Xavier Morell (GP) 16-01-11
            this.pedidoLabelExpediente.Text = this.expediente.Idexpediente.ToString();
            this.fechaExpedienteLabelValue.Text = this.expediente.Fechacreacion.ToString();
            this.amecLabelValue.Text = this.expediente.Amec;
            this.congresoLabelValue.Text = this.expediente.Actividad;
            this.imgExpedienteDetalle.ToolTip = String.Format("Ver detalles del AMEC {0}", this.expediente.Idamec);
            this.imgExpedienteDetalle.PostBackUrl = Page.ResolveUrl(String.Format("DetalleAMEC.aspx?idamec={0}", this.expediente.Idamec));
        }

        protected void imgDownload_Command(object sender, CommandEventArgs e)
        {
            Response.Redirect("Download.aspx?" + Request.QueryString.ToString() + "&file=" + EOS.Logica.Utility.ValueEncrypt(e.CommandName));
        }

        protected void lvDocumentos_Sorting(object sender, ListViewSortEventArgs e)
        {
            string strCssClass = "";
            if (e.SortDirection == SortDirection.Ascending) strCssClass = "eosSortAscending";
            else strCssClass = "eosSortDescending";

            LinkButton lbTipo = (LinkButton)this.lvDocumentos.FindControl("lbTipo");
            LinkButton lbDocumento = (LinkButton)this.lvDocumentos.FindControl("lbDocumento");
            LinkButton lbFecha = (LinkButton)this.lvDocumentos.FindControl("lbFecha");

            if (e.SortExpression == "Extension")
            {
                lbTipo.CssClass = strCssClass;
                lbDocumento.CssClass = "";
                lbFecha.CssClass = "";
            }
            else if (e.SortExpression == "Name")
            {
                lbTipo.CssClass = "";
                lbDocumento.CssClass = strCssClass;
                lbFecha.CssClass = "";
            }
            else if (e.SortExpression == "LastWriteTime")
            {
                lbTipo.CssClass = "";
                lbDocumento.CssClass = "";
                lbFecha.CssClass = strCssClass;
            }

            CargaPanel(e.SortExpression.ToString(), e.SortDirection);
        }

        protected void lvDocumentos_Sorted(object sender, EventArgs e)
        {
        }

        protected void odsDirectorio_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            //if (!string.IsNullOrEmpty(txtNombre.Text.Trim())) e.InputParameters["filtroNombre"] = "%" + txtNombre.Text.Trim() + "%";
            //if (!string.IsNullOrEmpty(ddlPoblacion.SelectedValue.Trim())) e.InputParameters["filtroPoblacion"] = ddlPoblacion.SelectedValue.Trim();
            //if (!string.IsNullOrEmpty(txtAMEC.Text.Trim())) e.InputParameters["filtroAMEC"] = "%" + txtAMEC.Text.Trim() + "%";
            //if (!string.IsNullOrEmpty(ddlTipoActividad.SelectedValue.Trim())) e.InputParameters["filtroTipoActividad"] = ddlTipoActividad.SelectedValue.Trim();
        }
    }
}