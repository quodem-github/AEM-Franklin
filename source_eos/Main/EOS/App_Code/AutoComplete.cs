using System;
using System.Data.SqlClient;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;

using System.Configuration;
using EOS.Web;
using EOS;

/// <summary>
/// Summary description for AutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class AutoComplete : System.Web.Services.WebService
{
    [WebMethod]
    public string[] GetCompletionListEspecialidades(string prefixText)
    {
        ArrayList cntName = new ArrayList();
        try {
            DataSet dtst = new DataSet();
            SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DNS_conn"].ToString());
            string strSql = "SELECT * FROM especialidades WHERE especialidad like '%" + prefixText + "%'";
            SqlCommand sqlComd = new SqlCommand(strSql, sqlCon);
            sqlCon.Open();
            SqlDataAdapter sqlAdpt = new SqlDataAdapter();
            sqlAdpt.SelectCommand = sqlComd;
            sqlAdpt.Fill(dtst);
            //cntName = new string[dtst.Tables[0].Rows.Count];

            try {
                foreach (DataRow rdr in dtst.Tables[0].Rows) {
                    if (rdr["especialidad"].ToString().StartsWith(prefixText)) {
                        cntName.Add(rdr["especialidad"].ToString());
                    }
                }

            } catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
            } 
            finally {

                if (sqlCon.State == ConnectionState.Open)
                    sqlCon.Close();
            }
        } catch (Exception err) 
        {
            //Ismael Ameller 09-03-2011 Envio de Mail
            Mail mail = new Mail();
            mail.Send(ConfigUtil.GetAppSetting("ContactoError"), err);
            //FIN Ismael Ameller 09-03-2011 Envio de Mail
            Alert.Show(err.Message); 
        }

        if (cntName.Count == 0) {
            cntName.Add("No hay coincidencias");
        }
        string[] strNames = new string[cntName.Count];
        cntName.CopyTo(strNames, 0);
        return strNames;
    }

    [WebMethod]
    public string[] GetCompletionListAsistentes(string prefixText)
    {
        ArrayList cntName = new ArrayList();
        try {
            DataSet dtst = new DataSet();
            SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DNS_conn"].ToString());
            string strSql = "SELECT distinct CONCAT(pl.apel1,' ',pl.apel2,', ', pl.nombre) as nombreCompleto FROM expediente exp"
                + " INNER JOIN reservas_passengers_list rpl ON exp.idxpediente=rpl.idxpediente"
                + " INNER JOIN passengers_list pl ON pl.idpassengerlist=rpl.idpassengerlist"
                + " WHERE exp.idamec in (SELECT idamec FROM amec)";

            //+ " and ( pl.apel1 like '%" + prefixText + "%'"
            //+ " or pl.apel2 like '%" + prefixText + "%' or or pl.nombre like '%" + prefixText + "%')";

            SqlCommand sqlComd = new SqlCommand(strSql, sqlCon);
            sqlCon.Open();
            SqlDataAdapter sqlAdpt = new SqlDataAdapter();
            sqlAdpt.SelectCommand = sqlComd;
            sqlAdpt.Fill(dtst);

            try {
                foreach (DataRow rdr in dtst.Tables[0].Rows) {
                    if (rdr["nombreCompleto"].ToString().StartsWith(prefixText)) {
                        cntName.Add(rdr["nombreCompleto"].ToString());
                    }
                }
            } catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
            } 
            finally {
                if (sqlCon.State == ConnectionState.Open)
                    sqlCon.Close();
            }
        } catch (Exception err) 
        {
            //Ismael Ameller 09-03-2011 Envio de Mail
            Mail mail = new Mail();
            mail.Send(ConfigUtil.GetAppSetting("ContactoError"), err);
            //FIN Ismael Ameller 09-03-2011 Envio de Mail
            Alert.Show(err.Message); 
        }

        if (cntName.Count == 0) {
            cntName.Add("No hay coincidencias");
        }
        string[] strNames = new string[cntName.Count];
        cntName.CopyTo(strNames, 0);
        return strNames;
    }
}