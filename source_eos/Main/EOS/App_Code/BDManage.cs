using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Configuration;
using System.Data.Sql;
using System.Drawing;
using System.Web.Mail;
using System.Collections;
using System.Resources;
using System.Globalization;
using System.Threading;

using System.Web.UI.WebControls;
using EOS.Web;

/**
 * BDManage.cs
 * 
 * Classe per a la gestió de la base de dades
 * Xavier Morell mar-2007
 * 
 * WEBS
 * http://www.dotnetcr.com/Libreria.aspx?art=97&tag=Como-invocar-Procedimientos-Almacenados-en-nuestra-clase-conexion
 * http://www.functionx.com/vcsharp/index.htm
 * http://samples.gotdotnet.com/quickstart/aspplus/doc/webformsintro.aspx
 * http://www.asp101.com/samples/
 */
namespace EOS
{
    public class BDManage
    {
        #region CONSTANTS
        /**
    * Constants pel tipus de sentencia
    */
        public const int _SELECT = 1;
        public const int _SELECT_PROC = 11;
        public const int _INSERT = 2;
        public const int _UPDATE = 3;

        private const int ERROR_INSERT_ASISTENTE = 1;
        private const int ERROR_INSERT_REGISTRO = 2;
        private const int ERROR_INSERT_ALOJAMIENTO = 3;
        private const int ERROR_INSERT_GASTOSGESTION = 4;
        private const int ERROR_INSERT_DATOS_FACTURA = 5;
        private const int ERROR_INSERT_PAGO = 6;
        private const int ERROR_INSERT_TOURS = 7;
        private const int ERROR_INSERT_VISITAS_TEC = 8;
        private const int ERROR_INSERT_OTROS_SERVICIOS = 9;
        private const int ERROR_INSERT_INSCRIPCIONES = 10;
        private const int ERROR_INSERT_QUIZ = 11;

        #endregion CONSTANTS

        /*****************************************************/

        #region Variables membre
        public SqlConnection cnn;
        private SqlDataAdapter da;
        private DataSet ds;
        private DataView dv;
        private SqlCommand cmd;

        #endregion Variables membre

        #region GETs/SETs
        /// <summary>
        /// Retorna el resultat de la consulta en un DataView
        /// </summary>
        /// <returns></returns>
        public DataView getDataView()
        {
            return dv;
        }

        /// <summary>
        /// Retorna el resultat de la consulta en un DataAdapter
        /// </summary>
        /// <returns></returns>
        public SqlDataAdapter getDataAdapter()
        {
            return da;
        }

        /// <summary>
        /// Retorna el resultat de la consulta en un DataSet
        /// </summary>
        /// <returns></returns>
        public DataSet getDataSet()
        {
            return ds;
        }


        /// <summary>
        /// Retorna el SqlCommand de la consulta
        /// </summary>
        /// <returns></returns>
        public SqlCommand getSqlCommand()
        {
            return cmd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public MySqlReader getDataReader()
        //{
        //    return dr;
        //}
        #endregion GETs/SETs

        #region Constructores y destructores

        public BDManage()
        {
            cnn = new SqlConnection();
        }

        /// <summary>
        /// Realitza la consulta 'sentencia' 
        /// i desa els valor en els diferents objectes
        /// </summary>
        /// <param name="sentencia">Sentència SQL que s'executarà</param>
        /// <param name="tipusSelect">Tipus de select: insert, update, remove...</param>

        public BDManage(string sentencia, int tipusSelect)
        {
            try
            {
                //crea la connexió
                cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[EOS.ServiceLogic.Variables.IdConnectionDatabase].ConnectionString);
                cnn.Open();

                switch (tipusSelect)
                {
                    case 1:
                        //omple els diferents objectes que després es poden retornar
                        da = new SqlDataAdapter(sentencia, cnn);
                        da.SelectCommand.CommandTimeout = int.Parse(ConfigurationManager.AppSettings["LimiteComandoConexion"]); 
                        ds = new DataSet();
                        da.Fill(ds);
                        dv = new DataView(ds.Tables[0]);
                        break;
                    case 2: //execució d'un insert
                    case 11: //execució d'un procediment
                        cmd = new SqlCommand(sentencia, cnn);
                        //cmd.Transaction = tr;
                        cmd.CommandType = CommandType.StoredProcedure;
                        break;
                    case 3:
                        cmd = new SqlCommand(sentencia, cnn);
                        cmd.ExecuteNonQuery();
                        break;
                    case 5:
                        cmd = new SqlCommand(sentencia, cnn);
                        break;
                }
            }
            catch (SqlException err)
            {
                throw err;
            }
            catch (Exception exc)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), exc);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Console.WriteLine(exc.Message);
            }
            finally
            {
                cnn.Close();
            }
        }


        /// <summary>
        /// Destructor
        /// </summary>
        ~BDManage()
        {
            if (da != null)
                da.Dispose();
            if (cmd != null)
                cmd.Dispose();
            if (dv != null)
                dv.Dispose();
            if (ds != null)
                ds.Dispose();
            try { if (cnn.State == ConnectionState.Open) cnn.Close(); }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
            }
        }
        #endregion
    }
}
