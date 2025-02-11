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

    public partial class AlternativasListViewControl : System.Web.UI.UserControl
    {

        #region Properties

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

        private string idservicio = "";
        public string IdServicio
        {
            set { idservicio = value; }
            get { return idservicio; }
        }

        private string idtab = "";
        public string TipoServicio
        {
            set { idtab = value; }
            get { return idtab; }
        }

        private int idReserva = 0;
        public int IdReserva
        {
            set { idReserva = value; }
            get { return idReserva; }
        }

        public int NumAlternativas
        {
            get { return this.AlternativasListView.Items.Count; }
        }

        #endregion


        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                if (idservicio == "")
                    return;

                // Recupera las alternativas del servicio
                GetAlternatives();

            }
        }


        protected void GetAlternatives()
        {

            string currentidcongreso = HttpContext.Current.Session["currentFKIdCongreso"].ToString();
            
            string field = "";

            switch (idtab)
            {
                case "0":
                    field = "idservicioinscripcion";
                    break;
                case "1":
                    field = "idserviciohotel";
                    break;
                case "2":
                    field = "idserviciotransporte";
                    break;
                case "3":
                    field = "idservicioactividad";
                    break;
            };

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);

            try
            {
                conn.Open();


                string consultaIdConfEmpresa = string.Format("select idconfempresa from amec where idamec = (select top 1 idamec from expediente where idxpediente = {0})", expediente.Idexpediente);
                SqlCommand commandIdConfEmpresa = new SqlCommand(consultaIdConfEmpresa, conn);
                string idConfEmpresa = commandIdConfEmpresa.ExecuteScalar().ToString();

                string consulta = "SELECT TR.idtramitacion,  TR.okalternativa1,  TR.pvp1,  PR1.proveedor proveedor1,  TR.Validez1,  TR.gastoscancelacion1,  TR.alternativa1, " +
                  "TR.okalternativa2,  TR.pvp2,  PR2.proveedor proveedor2,  TR.Validez2,  TR.gastoscancelacion2,  TR.alternativa2,   " +
                  "TR.okalternativa3,  TR.pvp3,  PR3.proveedor proveedor3,  TR.Validez3,  TR.gastoscancelacion3,  TR.alternativa3,  TR.idestado   " +
                  "FROM tramitacionesserviciosreservas TR   " +
                  "INNER JOIN serviciosreservasviajes srv on srv.idservicio = TR.fkidservicio " +
                  "LEFT JOIN proveedores PR1 ON TR.idproveedor1 = PR1.idproveedor AND PR1.IdConfEmpresa = " + idConfEmpresa +  "  " +
                  "LEFT JOIN proveedores PR2 ON TR.idproveedor2 = PR2.idproveedor AND PR2.IdConfEmpresa = " + idConfEmpresa + "   " +
                  "LEFT JOIN proveedores PR3 ON TR.idproveedor3 = PR3.idproveedor AND PR3.IdConfEmpresa = " + idConfEmpresa + "   " +
                  "LEFT JOIN estadosreservas ER ON TR.idestado = ER.idestado   " +
                  "WHERE srv." + field + " = " + idservicio + " " +
                  "ORDER BY TR.linea ;";

                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("alternativa", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("idtramitacion", System.Type.GetType("System.Int32")));
                dt.Columns.Add(new DataColumn("selected", System.Type.GetType("System.Boolean")));
                dt.Columns.Add(new DataColumn("pvp", System.Type.GetType("System.Decimal")));
                dt.Columns.Add(new DataColumn("proveedor", System.Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("validez", System.Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("gastos", System.Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("observaciones", System.Type.GetType("System.String")));

                // Read rows from DataReader and populate the DataTable
                int selalternativa = 0;
                if (rea.Read())
                {
                    for (int i = 1; i <= 3; ++i)
                    {
                        // Qurius (EAS) 27/01/2010
                        if (!IsDbNull(rea, "alternativa" + i.ToString()))
                        {
                            DataRow dr = dt.NewRow();
                            dr["alternativa"] = i;
                            dr["idtramitacion"] = rea["idtramitacion"].ToString();
                            dr["Selected"] = (!IsDbNull(rea, "okalternativa" + i.ToString()) && rea["okalternativa" + i.ToString()].ToString() == "AC" ? true : false);
                            dr["PVP"] = (IsDbNull(rea, "pvp" + i.ToString()) ? 0 : double.Parse(rea["pvp" + i.ToString()].ToString()));
                            dr["Proveedor"] = (IsDbNull(rea, "proveedor" + i.ToString()) ? "" : rea["proveedor" + i.ToString()].ToString());
                            dr["Validez"] = (IsDbNull(rea, "validez" + i.ToString()) ? "" : rea["validez" + i.ToString()].ToString());
                            dr["Gastos"] = (IsDbNull(rea, "gastoscancelacion" + i.ToString()) ? "" : rea["gastoscancelacion" + i.ToString()].ToString());
                            dr["Observaciones"] = (IsDbNull(rea, "alternativa" + i.ToString()) ? "" : rea["alternativa" + i.ToString()].ToString());
                            dt.Rows.Add(dr);
                            if (!IsDbNull(rea, "okalternativa" + i.ToString()) && rea["okalternativa" + i.ToString()].ToString() == "AC")
                                selalternativa = i;
                        }
                    }
                }
                this.AlternativasListView.DataSource = dt;
                this.AlternativasListView.DataBind();

                if (selalternativa > 0)
                    ((ImageButton)this.AlternativasListView.Items[selalternativa - 1].Controls[1]).ImageUrl = "~/Styles/images/ic_amecselecc.png";

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

        protected void ImageButton_Command(object sender, CommandEventArgs e)
        {
            string idEstadoReserva = string.Empty;
            if (Request.QueryString["idEstado"] != null)
            {
                idEstadoReserva = Request.QueryString["idEstado"];
            }
            //Ismael Ameller 24-03-2011 Solo se modifican las alternativas si el estado de la reserva es CTZ
            if (idEstadoReserva.Trim() == "CTZ")
            {
                if (e.CommandName == "Selected")
                {

                    foreach (ListViewItem item in this.AlternativasListView.Items)
                    {
                        if (((ImageButton)item.Controls[1]).CommandArgument == e.CommandArgument.ToString())
                        {
                            ((ImageButton)item.Controls[1]).ImageUrl = "~/Styles/images/ic_amecselecc.png";
                            // Guarda alternativa como seleccionada
                            Label lblKey = (Label)item.FindControl("ID");
                            SetAlternative(Int32.Parse(e.CommandArgument.ToString()), true, lblKey.Text);
                        }
                        else
                        {
                            ((ImageButton)item.Controls[1]).ImageUrl = "~/Styles/images/ic_amec.png";
                        }
                    }

                    ShowMessage("Se ha actualizado la alternativa seleccionada. Puede realizar el envio de la nueva selección pulsando la opción Enviar", null);
                }
            }
        }

        #endregion


        #region Functions

        private bool IsDbNull(SqlDataReader data, string field)
        {
            return data.IsDBNull(data.GetOrdinal(field));
        }

        private void ShowMessage(string message, string navigation)
        {
            // Muestra mensaje y navega a una URL especificada
            navigation = (navigation == null ? "" : ";document.location='" + navigation + "'");
            string script = String.Format("alert('{0}');{1}", message, navigation);
            ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "dialog", script, true);
        }

        private void SetAlternative(int alternativa, bool selected, string idTramite)
        {
            string consulta = "UPDATE tramitacionesserviciosreservas SET okalternativa1 = 'NA', okalternativa2 = 'NA', okalternativa3 = 'NA' WHERE idtramitacion = " + idTramite + ";" +
                              "UPDATE tramitacionesserviciosreservas SET okalternativa" + alternativa + " = '" + (selected ? "AC" : "NA") + "' WHERE idtramitacion = " + idTramite + ";";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                comm.ExecuteNonQuery();
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

        #endregion

    }
}