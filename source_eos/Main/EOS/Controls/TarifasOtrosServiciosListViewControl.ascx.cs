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

using System.Data;
using System.Configuration;

namespace EOS.Controls
{
    public class TarifaOtrosServiciosSelectedEventArgs : EventArgs
    {
        public TarifaOtrosServiciosSelectedEventArgs(String selectedItem)
        {
            this.selectedItem = selectedItem;
        }

        public String selectedItem;
    }

    public partial class TarifasOtrosServiciosListViewControl: UserControl
    {
        public event EventHandler OnTarifaSelected;
        private DCabeceraExpedienteAmpliado expediente
        {
            get //Qurius (EAS) 24/01/2011
            {
                if (HttpContext.Current.Session["currentExpediente"] != null)
                {
                    return (DCabeceraExpedienteAmpliado)HttpContext.Current.Session["currentExpediente"];
                }
                else
                {
                    AgenteExpedientes agenteExp = new AgenteExpedientes();
                    DCabeceraExpedienteAmpliado value = agenteExp.ObtenerExpedientePorID(Request.QueryString["idexp"]);
                    HttpContext.Current.Session["currentFKIdCongreso"] = value.Idactividad;

                    return value;
                }
            }
        }

        public bool existePregarga
        {
            get { return (this.TarifasListView.Items.Count > 0); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && expediente.Idtiporeserva!=2)   //Qurius (EAS) 24/01/2011
            {
                string currentidcongreso = HttpContext.Current.Session["currentFKIdCongreso"].ToString();
                string consulta = "SELECT tarifasactividad.idtarifaactividad, tarifasactividad.actividad, tipos_actividad_congreso.tipoactividadcongreso, " +
                "proveedores.proveedor, tarifasactividad.pvp, tarifasactividad.cancelacion,tarifasactividad.fechainicio,tarifasactividad.fechafin " +
                "FROM tarifasactividad " +
                "LEFT JOIN proveedores ON tarifasactividad.idproveedor = proveedores.idproveedor " +
                "LEFT JOIN tipos_actividad_congreso ON tarifasactividad.idtipoactividadcongreso = tipos_actividad_congreso.idtipoactividadcongreso " +
                "WHERE fkidcongreso = " + currentidcongreso;

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);

                try
                {
                    conn.Open();

                    string consultaIdConfEmpresa = string.Format("select idconfempresa from amec where idamec = (select top 1 idamec from expediente where idxpediente = {0})", expediente.Idexpediente);
                    SqlCommand commandIdConfEmpresa = new SqlCommand(consultaIdConfEmpresa, conn);
                    string idConfEmpresa = commandIdConfEmpresa.ExecuteScalar().ToString();

                    consulta += " AND proveedores.IdConfEmpresa = " + idConfEmpresa;

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
                    //dr["IdTarifaActividad"] = 0;
                    //dr["proveedor"] = "";
                    //dr["actividad"] = "OTROS";
                    //dr["tipoactividadcongreso"] = "";
                    //dr["PVP"] = 0;
                    //dt.Rows.Add(dr);

                    this.TarifasListView.DataSource = dt;
                    this.TarifasListView.DataBind();
                }
                catch (Exception ex)
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

        protected override void OnPreRender(EventArgs e)
        {
            lnkBtnOtraInscripcion.Enabled = true;
            base.OnPreRender(e);
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
            OnTarifaSelected(this, new TarifaSelectedEventArgs("0"));
            foreach (ListViewItem item in this.TarifasListView.Items)
            {
                ((ImageButton)item.Controls[1]).ImageUrl = "~/Styles/images/ic_amec.png";
            }
        }
    }
}