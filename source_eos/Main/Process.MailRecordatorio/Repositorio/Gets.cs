using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using System.Configuration;

namespace Repositorio
{
    public class Gets
    {

        public static DataTable GetAmecsRecordatorio()
        {
            DataSet ds = new DataSet();
            try
            {
                DateTime Data = new DateTime();
                Data = DateTime.Now.AddDays(-15);
                string query = "select ams.idamecs, pet.email as solicitante, pet1.email as creador from dbo_iw_amecs ams inner join dbo_iw_peticionarios pet on ams.idsolicitante = pet.idpeticionario " +
                    "inner join dbo_iw_peticionarios pet1 on pet1.idpeticionario = ams.idcreadopor " +
                    "where ams.idestado != 1 and ams.idestado != 3 and ams.idestado != 4 and ams.idestado != 5  and YEAR(ams.fechaamecs) > 2013 and ams.fechaamecs < '" + Data.Date.ToString("yyyy/MM/dd HH:mm:ss") + "'";
                return ds.Tables[0];

            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public static int GetLogMailRecordatorio(string idamecs)
        {
            int count;
            MySqlConnection conx = new MySqlConnection();
            try
            {
                string connectionString = ConfigurationSettings.AppSettings["ConnectionString"];
                MySqlDataAdapter addapter = new MySqlDataAdapter();
                conx.ConnectionString = connectionString;
                conx.Open();
                string query = string.Format("select count(*) from dbo_iw_logmail_flujoamec log where log.idamecs = '{0}'" +
                    " and log.tipo_mail = 'MailRecordatorio' and log.envio_correcto = 1 ", idamecs);
                MySqlCommand command = new MySqlCommand(query, conx);
                object obj = command.ExecuteScalar();
                count = int.Parse(obj.ToString());
                //2012-03-03 00:00:00

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conx.Close();
            }
            return count;
        }

       
    }
}
