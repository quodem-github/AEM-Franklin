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
    public class TransparencyReportProcess : Ejecucion, IProcess
    {
        private SqlDataManager _sqlDataManager { get; set; }
        private TransparencyReportCsvGenerator _transparencyReportCsvGenerator { get; set; }

        private EmailManager _emailManager { get; set; }

        private Resultado _result { get; set; }

        public TransparencyReportProcess(Resultado result)
        {
            _result = result;
            _sqlDataManager = new SqlDataManager(this, _result);
            _transparencyReportCsvGenerator = new TransparencyReportCsvGenerator(this, _result);
            _emailManager = new EmailManager(this, _result);
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
                AddMensajeAndWriteLineLog(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " [INF_TRANS] " + message, level);
            }
            else
            {
                WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " [INF_TRANS] " + message, level);
            }
            Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " [INF_TRANS] " + message);
        }

        /// <summary>
        /// Importa los datos a partir de ficheros en un SFTP
        /// </summary>
        protected override void Run()
        {
            base.Run();

            DateTime startDateTime = new DateTime(2000, 01, 01);
            DateTime endDateTime = DateTime.MaxValue;

            DataSet dataSet = null;

            WriteLineApp("Proceso de informe de transparencia de datos", -1);
            WriteLineApp("Iniciado proceso...", -1);
            WriteLineApp("Procesando datos...", -1);

            _sqlDataManager.RunStoreProcedureNoResult(AppConfig.StoreProcedurePrepare);

            dataSet = ProcessData(startDateTime, endDateTime);

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                WriteLineApp(string.Format("Se han procesado {0} registros...", dataSet.Tables[0].Rows.Count), -1);
                _result.Correctos++;
                WriteLineApp("Exportando Excel datos...", -1);
                WriteLineApp("Exportando Excel no msd id...", -1);
                WriteLineApp("Exportando Excel no genesis...", -1);
                WriteLineApp("Exportando Excel no golden id...", -1);

                var generatedFilePathData = GenerateDataCsv(dataSet.Tables[0]);
                var generatedFilePathNoMsdId = GenerateNoMsdIdCsv(dataSet.Tables[0]);
                var generatedFilePathNoGenesis = GenerateNoGenesysCsv(dataSet.Tables[0]);
                var generatedFilePathNoGoldenId = GenerateNoGoldenIdCsv(dataSet.Tables[0]);

                if (!string.IsNullOrEmpty(generatedFilePathData) && !string.IsNullOrEmpty(generatedFilePathNoMsdId) && !string.IsNullOrEmpty(generatedFilePathNoGenesis) && !string.IsNullOrEmpty(generatedFilePathNoGoldenId))
                {
                    WriteLineApp(string.Format("Se han generado los ficheros Excel de transparencia satisfactoriamente en: {0}...", AppConfig.FileExportDirectory), -1);
                    _result.Correctos++;
                    WriteLineApp("Enviando Email Excel transparencia...", -1);

                    if (_emailManager.SendEmail("[EOS MSD - Spotfire] CSV Exportación informes transparencia", "Registros a procesar: " + dataSet.Tables[0].Rows.Count +
                                "\n Archivos generados: \n" + generatedFilePathData + "\n" + generatedFilePathNoMsdId + "\n" + generatedFilePathNoGenesis + "\n" + generatedFilePathNoGoldenId))
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
        private DataSet ProcessData(DateTime startDateTime, DateTime endDateTime)
        {
            try
            {
                var parametersCollection = new List<SqlParameter>
                {
                    new SqlParameter("p_start_date", startDateTime),
                    new SqlParameter("p_end_date", endDateTime)
                };

                return _sqlDataManager.ProcessedData(AppConfig.StoreProcedureDataName, parametersCollection);
            }
            catch (Exception ex)
            {
                WriteLineApp("Error: ProcessData: " + ex.Message, -1);
                _result.Errores++;
            }

            return null;
        }

        private string GenerateDataCsv(DataTable dataTable)
        {
            string filterQuery = "TRIM(MsdId) <> '' AND TRIM(MsdId) IS NOT NULL AND TRIM(MsdId) LIKE 'ES%' AND TRIM([Golden ID / GCM (if available)]) <> '' AND TRIM([Golden ID / GCM (if available)]) IS NOT NULL AND TRIM([HCP/HCO ID #]) <> '' AND    TRIM([HCP/HCO ID #]) IS NOT NULL";

            try
            {
                var dataFiltered = dataTable.Select(filterQuery);

                var generatedFilePath = _transparencyReportCsvGenerator.GenerateCsv("Data", dataFiltered);

                return generatedFilePath;
            }
            catch (Exception ex)
            {
                WriteLineApp("Error: GenerateExcel: " + ex.Message, -1);
                _result.Errores++;
            }

            return string.Empty;
        }

        private string GenerateNoMsdIdCsv(DataTable dataTable)
        {
            string filterQuery = "TRIM(MsdId) = '' OR TRIM(MsdId)  IS NULL OR TRIM(MsdId) NOT LIKE 'ES%'";

            try
            {
                var dataFiltered = dataTable.Select(filterQuery);

                var generatedFilePath = _transparencyReportCsvGenerator.GenerateCsv("No_Msd_Id", dataFiltered);

                return generatedFilePath;
            }
            catch (Exception ex)
            {
                WriteLineApp("Error: GenerateExcel: " + ex.Message, -1);
                _result.Errores++;
            }

            return string.Empty;
        }

        private string GenerateNoGenesysCsv(DataTable dataTable)
        {
            string filterQuery = "TRIM(MsdId)  <> '' AND TRIM(MsdId)  IS NOT NULL AND TRIM(MsdId)  LIKE 'ES%' AND (TRIM([HCP/HCO ID #]) = '' OR TRIM([HCP/HCO ID #]) IS NULL)";

            try
            {
                var dataFiltered = dataTable.Select(filterQuery);

                var generatedFilePath = _transparencyReportCsvGenerator.GenerateCsv("No_Genesis", dataFiltered);

                return generatedFilePath;
            }
            catch (Exception ex)
            {
                WriteLineApp("Error: GenerateExcel: " + ex.Message, -1);
                _result.Errores++;
            }

            return string.Empty;
        }

        private string GenerateNoGoldenIdCsv(DataTable dataTable)
        {
            string filterQuery = "TRIM(MsdId)  <> '' AND TRIM(MsdId)  IS NOT NULL AND TRIM(MsdId)  LIKE 'ES%' AND TRIM([HCP/HCO ID #]) <> '' AND TRIM([HCP/HCO ID #]) IS NOT NULL AND (TRIM([Golden ID / GCM (if available)]) = '' OR TRIM([Golden ID / GCM (if available)]) IS NULL) ";

            try
            {
                var dataFiltered = dataTable.Select(filterQuery);

                var generatedFilePath = _transparencyReportCsvGenerator.GenerateCsv("No_Golden_Id", dataFiltered);

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