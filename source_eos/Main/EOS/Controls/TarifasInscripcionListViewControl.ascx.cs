using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Entidades.Datos;
using EOS.Logica;
using EOS.Web;

using System.Configuration;
using System.Data;

namespace EOS.Controls
{
    public class TarifaSelectedEventArgs : EventArgs
    {
        public TarifaSelectedEventArgs(String selectedItem)
        {
            this.selectedItem = selectedItem;
        }

        public String selectedItem;
    }

    public partial class TarifasInscripcionListViewControl : System.Web.UI.UserControl
    {
        public event EventHandler OnTarifaSelected;
        private DCabeceraExpedienteAmpliado expediente
        {
            get
            {
                AgenteExpedientes agenteExp = new AgenteExpedientes();
                DCabeceraExpedienteAmpliado value = agenteExp.ObtenerExpedientePorID(Request.QueryString["idexp"]);
                HttpContext.Current.Session["currentFKIdCongreso"] = value.Idactividad;

                return value;
            }
        }

        public bool existePregarga
        {
            get { return (this.TarifasListView.Items.Count > 0); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                if (HttpContext.Current.Session["currentFKIdCongreso"] != null) {
                    string currentidcongreso = HttpContext.Current.Session["currentFKIdCongreso"].ToString();
                    string consulta = "SELECT TarifasInscripcion.idtarifainscripcion, tiposinscripcion.tipoinscripcion, " +
                    "tarifasinscripcion.pvp, tarifasinscripcion.descripcion " +
                    "FROM TarifasInscripcion " +
                    "LEFT JOIN tiposinscripcion ON tiposinscripcion.idtipoinscripcion = TarifasInscripcion.idtipoinscripcion " +
                    "WHERE (fechainicio < GETDATE() OR fechainicio is null) and (fechafin >= GETDATE() OR fechafin is null) and fkidcongreso = " + HttpContext.Current.Session["currentFKIdCongreso"];
                    //3177";// + currentidcongreso;

                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);

                    try {
                        conn.Open();
                        SqlCommand comm = new SqlCommand(consulta, conn);

                        SqlDataReader rea = comm.ExecuteReader();
                        DataTable dtSchema = rea.GetSchemaTable();
                        DataTable dt = new DataTable();
                        // You can also use an ArrayList instead of List<>
                        List<DataColumn> listCols = new List<DataColumn>();

                        if (dtSchema != null)
                        {
                            foreach (DataRow drow in dtSchema.Rows)
                            {
                                string columnName = System.Convert.ToString(drow["ColumnName"]);
                                DataColumn column = new DataColumn(columnName, (Type)(drow["DataType"]));
                                column.Unique = (bool)drow["IsUnique"];
                                column.AllowDBNull = (bool)drow["AllowDBNull"];
                                column.AutoIncrement = (bool)drow["IsAutoIncrement"];
                                listCols.Add(column);
                                dt.Columns.Add(column);
                            }
                        }

                        // Read rows from DataReader and populate the DataTable
                        while (rea.Read())
                        {
                            DataRow dataRow = dt.NewRow();
                            for (int i = 0; i < listCols.Count; i++)
                            {
                                dataRow[((DataColumn)listCols[i])] = rea[i];
                            }
                            dt.Rows.Add(dataRow);
                        }

                        //DataRow dr = dt.NewRow();
                        //dr["idtarifainscripcion"] = 0;
                        //dr["tipoinscripcion"] = "OTROS";
                        //dr["PVP"] = 0;
                        //dt.Rows.Add(dr);

                        this.TarifasListView.DataSource = dt;
                        this.TarifasListView.DataBind();
                    } catch (Exception ex) 
                    {
                        //Ismael Ameller 09-03-2011 Envio de Mail
                        Mail mail = new Mail();
                        mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                        //FIN Ismael Ameller 09-03-2011 Envio de Mail
                        Console.WriteLine(ex.ToString());
                    }
                    conn.Close();
                }
            }
        }

        protected void ImageButton_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Selected")
            {
                foreach (ListViewItem item in this.TarifasListView.Items)
                {
                    if (((ImageButton)item.Controls[1]).CommandArgument == e.CommandArgument.ToString())
                    {
                        ((ImageButton)item.Controls[1]).ImageUrl = "~/Styles/images/ic_amecselecc.png";
                    }
                    else
                    {
                        ((ImageButton)item.Controls[1]).ImageUrl = "~/Styles/images/ic_amec.png";
                    }
                }
                if (OnTarifaSelected != null)
                {
                    OnTarifaSelected(this, new TarifaSelectedEventArgs(e.CommandArgument.ToString()));
                }
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in this.TarifasListView.Items)
            {
                ((ImageButton)item.Controls[1]).ImageUrl = "~/Styles/images/ic_amec.png";
            }
            OnTarifaSelected(this, new TarifaSelectedEventArgs("0"));
        }
    }
}