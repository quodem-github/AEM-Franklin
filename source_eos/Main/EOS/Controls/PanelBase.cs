using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using System.Configuration;
using EOS.Web;

namespace EOS.Controls
{
    public class PanelBase
    {
        public static string GetEstado()
        {
            // Recupera el id de servicio a partir del id de reserva
            string consulta = "SELECT idestado From reservasviajes";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
            try {
                conn.Open();
                SqlCommand comm = new SqlCommand(consulta, conn);
                SqlDataReader rea = comm.ExecuteReader();
                if (rea.Read())
                {
                    return rea["idestado"].ToString();
                }
            } catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                //do nothing
                return "AB";
            } finally {
                conn.Close();
            }
            return "AB";
        }
    }
}