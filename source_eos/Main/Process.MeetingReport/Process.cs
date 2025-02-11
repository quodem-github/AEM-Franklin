using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using Process.MeetingReport.Class;
using Process.MeetingReport.Class.DataAccess;
using Process.MeetingReport.Class.EmailManagement;
using Process.MeetingReport.Class.FileManagement;
using NPOI.SS.Formula.Functions;
using Quodem.Monitor.Procesos;
using System.Data.SqlClient;

namespace Process.MeetingReport
{
    public class Process : Ejecucion
    {
        #region Class variables
        private readonly int _countInserGroup;
        #endregion

        #region Constructors

        /// <param name="result"></param>
        /// <param name="countInserGroup">CountInserGroup</param>
        public Process(Resultado result)
        {
            if (result == null) throw new ArgumentNullException("result");

     

            Result = result;

            SqlDataManager = new SqlDataManager(this, Result);
            ExcelManager = new ExcelManagerNpoi(this, Result);
            EmailManager = new EmailManager(this, Result);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Representa el objeto que realizará todas las operaciones relacionadas con la base de datos
        /// </summary>
        public SqlDataManager SqlDataManager { get; set; }

        public IExcelManager ExcelManager { get; set; }

        public EmailManager EmailManager { get; set; }

        /// <summary>
        /// Objeto para registrar los resultados de las pricipales operaciones.
        /// </summary>
        public Resultado Result { get; set; }

        #endregion

        #region Public Methods

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

        #endregion

        #region Protected Methods

        /// <summary>
        /// Importa los datos a partir de ficheros en un SFTP
        /// </summary>
        protected override void Run()
        {
            base.Run();

            Result.Texto = AppConfigs.ProcessDescription;

            var dateTimeCollection = GetPeriod(DateTime.Today);
            var startDateTime = new DateTime();
            var endDateTime = new DateTime();

            var executionDateTime = DateTime.Now;

            DataSet dataSet = null;

            WriteLineApp(Result.Texto, -1);
            WriteLineApp("Iniciado proceso...", -1);
            WriteLineApp("Procesando datos...", -1);

            if (dateTimeCollection.Any())
            {
                startDateTime = dateTimeCollection[0];
                endDateTime = dateTimeCollection[1];
                SqlDataManager.RunStoreProcedureNoResult(AppConfigs.StoreProcedurePrepare);

                dataSet = ProcessData(startDateTime, endDateTime, executionDateTime);

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    WriteLineApp(string.Format("Se han procesado {0} registros...", dataSet.Tables[0].Rows.Count), -1);
                    Result.Correctos++;
                    WriteLineApp("Exportando Excel...", -1);

                    var filepath = string.Format("{0}{1}-f{2}-t{3}-{4}", AppConfigs.FileExportDirectory, DateTime.UtcNow.ToString("ddMMyyHHmm"), startDateTime.ToString("dd_MM_yyyy"), endDateTime.ToString("dd_MM_yyyy"), AppConfigs.FileExportName);

                    if (GenerateExcel(dataSet, filepath))
                    {
                        WriteLineApp(string.Format("Se ha generado fichero Excel satisfactoriamente en: {0}...", AppConfigs.FileExportDirectory), -1);
                        Result.Correctos++;
                        WriteLineApp("Enviando Email...", -1);

                        if (EmailManager.SendEmail(filepath, startDateTime, endDateTime))
                        {
                            WriteLineApp("Enviando Email satisfactorio...", -1);
                            Result.Correctos++;
                        }
                    }
                }
                else
                {
                    Result.Correctos++;
                    WriteLineApp("No hay datos que exportar", -1);
                }

                WriteLineApp("Proceso Terminado", -1);

                ErrorSolicitud = ErrorSolicitud || Result.Errores > 0;
                WriteLineApp(Result.ToString(), -1);
            }

            
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Make a call to a store procedure in order to process data on database.
        /// </summary>
        /// <returns>Return the processed data</returns>
        private DataSet ProcessData(DateTime startDateTime, DateTime endDateTime, DateTime executionDateTime)
        {
            var rowsCount = 0;
            try
            {
                var parametersCollection = new List<SqlParameter>
                {
                    new SqlParameter("p_start_date", startDateTime),
                    new SqlParameter("p_end_date", endDateTime),
                    new SqlParameter("p_execution_date", executionDateTime)
                };

                return SqlDataManager.PocessedData(AppConfigs.StoreProcedureDataName, parametersCollection);
            }
            catch (Exception ex)
            {
                WriteLineApp("Error: ProcessData: " + ex.Message, -1);
                Result.Errores++;
            }

            return null;
        }

       

        private bool GenerateExcel(DataSet dataSet, string path)
        {
            try
            {
                if (dataSet == null || dataSet.Tables.Count == 0)
                    return false;

                return ExcelManager.ExportDataTableToExcel(dataSet.Tables[0], path);
            }
            catch (Exception ex)
            {
                WriteLineApp("Error: GenerateExcel: " + ex.Message, -1);
                Result.Errores++;
            }

            return false;
        }


        private List<DateTime> GetPeriod(DateTime date)
        {
            var dates = new List<DateTime>();

            var parametersCollection = new List<SqlParameter> { new SqlParameter("p_date", date) };

            var dsDates = SqlDataManager.RunStoreProcedureWithResult(AppConfigs.StoreProcedureTimeControllername, parametersCollection);

            if (dsDates != null && dsDates.Tables.Count > 0 && dsDates.Tables[0].Rows.Count > 0)
            {
                var row = dsDates.Tables[0].Rows[0];
                dates.Add(DateTime.Parse(row["meetingstartmonth"].ToString()));
                dates.Add(DateTime.Parse(row["meetingendmonth"].ToString()));
            }

            return dates;
        }



        #endregion
    }
}
