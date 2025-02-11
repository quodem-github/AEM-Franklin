using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Repositorio
{
    public class Inserts
    {

        //public static int Insertlog(string exceptionMessage)
        //{
        //    MySqlConnection conx = new MySqlConnection();
        //    int result = 0;
        //    try
        //    {
        //        //string connectionString = "server=82.165.146.178;Connect Timeout=180;User Id=root;Persist Security Info=True;database=wintour_robot;password=123;port=1433";
        //        string connectionString = ConfigurationSettings.AppSettings["ConnectionString"];
        //        MySqlDataAdapter addapter = new MySqlDataAdapter();
        //        conx.ConnectionString = connectionString;
        //        conx.Open();
        //        string query = string.Format("insert into prueba (log, createdon) values ('{0}', '{1}')", exceptionMessage, DateTime.Now);
        //        MySqlCommand command = new MySqlCommand(query, conx);
        //        result = command.ExecuteNonQuery();

        //    }
        //    catch (Exception a)
        //    {

        //        throw;
        //    }
        //    finally
        //    {
        //        conx.Close();
        //    }
        //    return result;
        //}

        public static int GuardaLogMail(int idamecs, string tipo_mail, string message_to, string message_subject, string message_body, string message_fileattach, bool envioCorrecto, string error)
        {
            MySqlConnection conx = new MySqlConnection();
            int result = 0;
            try
            {
                //string connectionString = "server=82.165.146.178;Connect Timeout=180;User Id=root;Persist Security Info=True;database=wintour_robot;password=123;port=1433";
                string connectionString = ConfigurationSettings.AppSettings["ConnectionString"];
                MySqlDataAdapter addapter = new MySqlDataAdapter();
                conx.ConnectionString = connectionString;
                conx.Open();
                string query = string.Empty;
                query = "INSERT INTO dbo_iw_logmail_flujoamec (idamecs, tipo_mail, mail_to, mail_subject, mail_body, mail_fileattach, envio_correcto, error, fecha_envio) VALUES(" + idamecs + ",'" + tipo_mail + "','" + message_to + "','" + message_subject + "','" + message_body + "','" + message_fileattach + "'," + envioCorrecto + ",'" + error + "',CURRENT_TIMESTAMP())";
                MySqlCommand command = new MySqlCommand(query, conx);
                result = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally 
            {
                conx.Close();
            }
            return result;
        }

    }
}
