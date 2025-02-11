using Process.Msd.Spotfire.CsvGenerator.CsvManager;
using Process.Msd.Spotfire.CsvGenerator.DataAccess;
using Process.Msd.Spotfire.CsvGenerator.EmailManagement;
using Process.Msd.Spotfire.CsvGenerator.Interfaces;
using Process.Msd.Spotfire.CsvGenerator.Utilities;
using Quodem.Monitor.Procesos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Process.Msd.Spotfire.CsvGenerator
{
    public class MeetingReportProcess : Ejecucion, IProcess
    {
        private Resultado _result { get; set; }
        private SqlDataManager _sqlDataManager { get; set; }
        private EmailManager _emailManager { get; set; }
        private MeetingReportCsvGenerator _meetingReportCsvGenerator { get; set; }

        public MeetingReportProcess(Resultado result)
        {
            _result = result;
            _sqlDataManager = new SqlDataManager(this, _result);
            _emailManager = new EmailManager(this, _result);
            _meetingReportCsvGenerator = new MeetingReportCsvGenerator(this, _result);
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
                AddMensajeAndWriteLineLog(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " [INF_REUNION] " + message, level);
            }
            else
            {
                WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " [INF_REUNION] " + message, level);
            }
            Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " [INF_REUNION] " + message);
        }

        /// <summary>
        /// Importa los datos a partir de ficheros en un SFTP
        /// </summary>
        protected override void Run()
        {
            base.Run();

            DateTime startDateTime = new DateTime(2000, 01, 01);
            DateTime endDateTime = DateTime.MaxValue;
            var executionDateTime = DateTime.Now;

            DataSet dataSet = null;

            WriteLineApp("Proceso de informe de reuniones", -1);
            WriteLineApp("Iniciado proceso...", -1);
            WriteLineApp("Procesando datos...", -1);

            _sqlDataManager.RunStoreProcedureNoResult("sp_meeting_report_prepare");

            dataSet = ProcessData(startDateTime, endDateTime, executionDateTime);

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                WriteLineApp(string.Format("Se han procesado {0} registros...", dataSet.Tables[0].Rows.Count), -1);
                _result.Correctos++;
                WriteLineApp("Exportando Excel reuniones...", -1);

                var generatedFilePath = GenerateCsv(dataSet.Tables[0]);

                if (!string.IsNullOrEmpty(generatedFilePath))
                {
                    WriteLineApp(string.Format("Se han generado el fichero Excel de reuniones satisfactoriamente en: {0}...", AppConfig.FileExportDirectory), -1);
                    _result.Correctos++;
                    WriteLineApp("Enviando Email Excel reuniones...", -1);

                    if (_emailManager.SendEmail("[EOS MSD - Spotfire] CSV Exportación informe reuniones", "Registros a procesar: " + dataSet.Tables[0].Rows.Count +
                                "\n Archivos generado: " + generatedFilePath))
                    {
                        WriteLineApp("Enviando Email satisfactorio...", -1);
                        _result.Correctos++;
                    }
                }
            }
            else
            {
                _result.Correctos++;
                WriteLineApp("No hay datos que exportar", -1);
            }

            WriteLineApp("Proceso Terminado", -1);

            ErrorSolicitud = ErrorSolicitud || _result.Errores > 0;
            WriteLineApp(_result.ToString(), -1);
        }

        /// <summary>
        /// Make a call to a store procedure in order to process data on database.
        /// </summary>
        /// <returns>Return the processed data</returns>
        private DataSet ProcessData(DateTime startDateTime, DateTime endDateTime, DateTime executionDateTime)
        {
            try
            {
                var parametersCollection = new List<SqlParameter>
                {
                    new SqlParameter("p_start_date", startDateTime),
                    new SqlParameter("p_end_date", endDateTime),
                    new SqlParameter("p_execution_date", executionDateTime)
                };

                return _sqlDataManager.ProcessedData("sp_meeting_report", parametersCollection);
            }
            catch (Exception ex)
            {
                WriteLineApp("Error: ProcessData: " + ex.Message, -1);
                _result.Errores++;
            }

            return null;
        }

        private string GenerateCsv(DataTable dataTable)
        {
            try
            {
                DataRow[] dataRows = dataTable.Select();

                var generatedFilePath = _meetingReportCsvGenerator.GenerateCsv(dataRows);

                return generatedFilePath;
            }
            catch (Exception ex)
            {
                WriteLineApp("Error: GenerateExcel: " + ex.Message, -1);
                _result.Errores++;
            }

            return string.Empty;
        }
    }
}