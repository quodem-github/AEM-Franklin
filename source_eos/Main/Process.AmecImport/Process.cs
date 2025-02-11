using AmecImport.FTP;
using AmecImport.BLL;
using AmecImport.DataAccess;
using Quodem.Monitor.Procesos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Text;
using AmecImport.DTO;
using EOS.Web;

namespace AmecImport
{
    public class Process : Ejecucion
    {
        #region Public properties

        private string Filepattern { get; set; }
        private string ToPath { get; set; }
        /// <summary>
        /// </summary>
        private int CountInserGroup { get; set; }

        /// <summary>
        /// Representa el objeto que realizará todas las operaciones relacionadas con la base de datos
        /// </summary>
        public SqlDataManager SqlDataManager { get; set; }
        /// <summary>
        /// Representa el objeto que se encarga de la gestión del servidor SFTP
        /// </summary>
        public SftpManager SftpManager { get; set; }
        Resultado _resultado;
        /// <summary>
        /// Objeto para registrar los resultados de las pricipales operaciones.
        /// </summary>
        public Resultado Result
        {
            get
            {
                if (_resultado == null)
                {
                    _resultado = new Quodem.Monitor.Procesos.Resultado();
                    _resultado.Texto = "Resultado";
                }
                return _resultado;
            }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Importa los datos a partir de ficheros en un SFTP
        /// </summary>
        protected override void Run()
        {
            try
            {
                Filepattern = AppConfigs.AmecsFileMasterPattern;
                ToPath = AppConfigs.LocalDirectoryToDownloadPath;
                CountInserGroup = AppConfigs.ItemsPerInsert;
                SftpManager = new SftpManager(this, Result);
                SqlDataManager = new SqlDataManager(this, Result);
                var filePathAmecsSrc = "";
                Result.Texto = AppConfigs.ProcessDescription;

                WriteLineApp(Result.Texto, -1);
                WriteLineApp("Iniciado proceso...", -1);

                if (AppConfigs.GetFileFromLocal)
                    filePathAmecsSrc = AppConfigs.LocalDirectoryFiledPath + AppConfigs.AmecsFileMasterPattern;
                else
                    filePathAmecsSrc = DownloadFile(ToPath, Filepattern, false,AppConfigs.DateFormatAmecFileMasterPattern);

                var executionDateTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");

                if (filePathAmecsSrc == null)
                {
                    WriteLineApp("No existe en el FTP el fichero necesarios para realizar la importación", -1);
                    Result.Errores++;
                }
                else
                {
                    ErrorResult errorResult = new ErrorResult();
                    errorResult.ErrorList = new List<ErrorDto>();

                    WriteLineApp("Importando datos...", 0);

                    ErrorResult resultDataTemp = new ErrorResult();
                    resultDataTemp.ErrorList = new List<ErrorDto>();
                    resultDataTemp = ImportData(filePathAmecsSrc, executionDateTime, CountInserGroup);
                    if (resultDataTemp.Status)
                    {
                        if (resultDataTemp.ErrorList != null && resultDataTemp.ErrorList.Count > 0)
                            errorResult.ErrorList.AddRange(resultDataTemp.ErrorList);
                        WriteLineApp("Datos importados satisfactoriamente", -1);
                    }
                    else
                    {
                        WriteLineApp("ImportData - Datos importados erróneamente", -1);
                        Result.Errores++;
                    }
                    
                    WriteLineApp("Actualizando tabla maestra de amecs en la base de datos...", -1);
                    if (LoadMasterTable(executionDateTime))
                    {
                        WriteLineApp("Datos cargados satisfactoriamente", -1);
                    }
                    else
                    {
                        WriteLineApp("Datos cargados erróneamente", -1);
                        Result.Errores++;
                    }

                    WriteLineApp("Comprobando los errores del procedimiento almacenado", -1);
                    resultDataTemp = new ErrorResult();
                    resultDataTemp.ErrorList = new List<ErrorDto>();
                    resultDataTemp = EvaluateStoredProcedureErrors(executionDateTime);
                    if (resultDataTemp.Status)
                    {
                        if (resultDataTemp.ErrorList != null && resultDataTemp.ErrorList.Count > 0)
                            errorResult.ErrorList.AddRange(resultDataTemp.ErrorList);
                        WriteLineApp("Datos importados satisfactoriamente", -1);
                    }

                    if (errorResult.ErrorList.Count > 0)
                    {
                        WriteLineApp("Enviamos correo con los errores obtenidos", -1);
                        SendErrorsMail(errorResult.ErrorList);
                    }
                    else
                    {
                        WriteLineApp("No hay errores para enviar por email", -1);
                    }
                }

                WriteLineApp("Proceso Terminado", -1);

                if (AppConfigs.TestMode)
                    Console.ReadLine();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                Console.WriteLine(message);
                AddMensajeAndWriteLineLog(message, 0);
                this.ErrorSolicitud = true;
            }

            ErrorSolicitud = ErrorSolicitud || Result.Errores > 0;
            WriteLineApp(Result.ToString(), -1);

            base.Run();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Descarga un fichero desde el FTP
        /// </summary>
        /// <param name="pfilepattern">Espreción regular para filtrar ficheros. Descargará solo el fichero más reciente que corresponda con la expreción regular.</param>
        /// <param name="pToPath">Directorio de destino de descarga</param>
        /// <param name="pDeleteAfterDownload">Indica si se ba a eliminar el ficnero del FTP después de ser descargado</param>
        private string DownloadFile(string pToPath, string pfilepattern, bool pDeleteAfterDownload, string dateFormat)
        {
            return SftpManager.DownloadFileAndGetFilePath(pToPath, pfilepattern, pDeleteAfterDownload, dateFormat);
        }

        /// <summary>
        /// Inserta datos en las tablas temporales
        /// </summary>
        /// <param name="genesiStream"></param>
        /// <param name="goldenStrema"></param>
        /// <param name="pCountInserGroup">Cantidad de insert que conforman un grupo</param>
        private ErrorResult ImportData(string filePath, string executionDateTime, int pCountInserGroup = 50)
        {
            return SqlDataManager.RunInserForAmecsTempTable(filePath, executionDateTime, pCountInserGroup);
        }

        /// <summary>
        /// Actualiza la tabla dbo_iw_hcp_genesyscr a partir de los dato en las tablas temporales
        /// </summary>
        private bool LoadMasterTable(string recordCreateDate)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "recordCreateDate";
            param.Value = recordCreateDate;
            return SqlDataManager.RunStoreProcedure(AppConfigs.StoreProcedureNameLoadMasterTableAmecs, param);
        }

        /// <summary>
        /// Consultamos los posibles error de carga de datos del procedimiento almacenado
        /// </summary>
        private ErrorResult EvaluateStoredProcedureErrors(string recordCreateDate)
        {
            return SqlDataManager.EvaluateStoredProcedureErrors(recordCreateDate);
        }
        

        /// <summary>
        /// Envia mensaje, escribe a log y a consola.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="level">level</param>
        public void WriteLineApp(string message, int level)
        {
            if (level == -1)
            {
                AddMensajeAndWriteLineLog(message, level);
            }
            else
            {
                WriteLine(message, level);
            }
            Console.WriteLine(message);
        }

        /// <summary>
        /// Envia email con los datos que faltan de los registros de la carga.
        /// </summary>
        private void SendErrorsMail(List<ErrorDto> lstError)
        {
            try
            {
                //Procesamos la lista para convertirla en tabla.
                StringBuilder textTable = new StringBuilder();
                textTable.AppendLine("<table>");
                    textTable.AppendLine("<tr>");
                        textTable.AppendLine("<th>EventName</th>");
                        textTable.AppendLine("<th>EmEventId</th>");
                        textTable.AppendLine("<th>RecordTypeDescription</th>");
                        textTable.AppendLine("<th>EventTypeDescription</th>");
                        textTable.AppendLine("<th>StatusDescription</th>");
                        textTable.AppendLine("<th>OwnerWein</th>");
                        textTable.AppendLine("<th>OwnerName</th>");
                        textTable.AppendLine("<th>Canceled</th>");
                        textTable.AppendLine("<th>CommittedCost</th>");
                        textTable.AppendLine("<th>ActualCost</th>");
                    textTable.AppendLine("</tr>");
                foreach (var error in lstError)
                {
                    textTable.AppendLine("<tr>");
                    textTable.AppendLine("<td>" + EvaluateTextToSend(error.EventName) + "</td>");
                        textTable.AppendLine("<td>" + EvaluateTextToSend(error.EmEventId) + "</td>");
                        textTable.AppendLine("<td>" + EvaluateTextToSend(error.RecordTypeDescription) + "</td>");
                        textTable.AppendLine("<td>" + EvaluateTextToSend(error.EventTypeDescription) + "</td>");
                        textTable.AppendLine("<td>" + EvaluateTextToSend(error.StatusDescription) + "</td>");
                        textTable.AppendLine("<td>" + EvaluateTextToSend(error.OwnerWein) + "</td>");
                        textTable.AppendLine("<td>" + EvaluateTextToSend(error.OwnerName) + "</td>");
                        textTable.AppendLine("<td>" + EvaluateTextToSend(error.Canceled) + "</td>");
                        textTable.AppendLine("<td>" + EvaluateTextToSend(error.CommittedCost.ToString()) + "</td>");
                        textTable.AppendLine("<td>" + EvaluateTextToSend(error.ActualCost.ToString()) + "</td>");
                    textTable.AppendLine("</tr>");
                }
                textTable.AppendLine("</table>");

                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                message.From = (new MailAddress(AppConfigs.FromEmail));
                message.To.Add(new MailAddress(AppConfigs.SupportQuodemEmail)); 
                message.Subject = "PROCESO DE CARGA DE AMECS (DATOS NO COMPLETADOS)";
                message.Body = textTable.ToString();
                Mail.EnviaMail(message, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error enviando el mail de Notificación al Gestor de Archivo: " + ex.Message);                
            }
        }

        private string EvaluateTextToSend(string value)
        {
            if (string.IsNullOrEmpty(value) || value == "-1")
            {
                return "VACÍO";
            }
            else
            {
                return value;
            }
        }
        #endregion
    }
}
