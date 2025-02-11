using Process.Msd.Spotfire.CsvGenerator.CsvManager;
using Process.Msd.Spotfire.CsvGenerator.DataAccess;
using Process.Msd.Spotfire.CsvGenerator.EmailManagement;
using Process.Msd.Spotfire.CsvGenerator.Interfaces;
using Process.Msd.Spotfire.CsvGenerator.Utilities;
using Quodem.Monitor.Procesos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Process.Msd.Spotfire.CsvGenerator
{
    public class PetitionReportProcess : Ejecucion, IProcess
    {
        private Resultado _result { get; set; }
        private SqlDataManager _sqlDataManager { get; set; }
        private EmailManager _emailManager { get; set; }
        private PetitionReportCsvGenerator _petitionReportCsvGenerator { get; set; }

        public PetitionReportProcess(Resultado result)
        {
            _result = result;
            _sqlDataManager = new SqlDataManager(this, _result);
            _emailManager = new EmailManager(this, _result);
            _petitionReportCsvGenerator = new PetitionReportCsvGenerator(this, _result);
        }

        protected override void Run()
        {
            base.Run();

            WriteLineApp("Proceso de informe de honorarios", -1);
            WriteLineApp("Iniciado proceso...", -1);
            WriteLineApp("Procesando datos...", -1);

            DataSet dataSet = null;

            dataSet = _sqlDataManager.RunStoreProcedureWithResult("sp_spotfire_generate_csv_petition");

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                WriteLineApp(string.Format("Se han procesado {0} registros...", dataSet.Tables[0].Rows.Count), -1);
                _result.Correctos++;
                WriteLineApp("Exportando Excel...", -1);

                var generatedFilePath = GenerateCsv(dataSet.Tables[0]);

                if (!string.IsNullOrEmpty(generatedFilePath))
                {
                    WriteLineApp(string.Format("Se ha generado fichero Excel satisfactoriamente en: {0}...", AppConfig.FileExportDirectory), -1);
                    _result.Correctos++;
                    WriteLineApp("Enviando Email...", -1);

                    if (_emailManager.SendEmail("[EOS MSD - Spotfire] CSV Exportación peticiones", "Registros a procesar: " + dataSet.Tables[0].Rows.Count + "\n Archivo generado en: " + generatedFilePath))
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

        private string GenerateCsv(DataTable dataTable)
        {
            try
            {
                DataRow[] dataRows = dataTable.Select();

                var generatedFilePath = _petitionReportCsvGenerator.GenerateCsv(dataRows);

                return generatedFilePath;
            }
            catch (Exception ex)
            {
                WriteLineApp("Error: GenerateExcel: " + ex.Message, -1);
                _result.Errores++;
            }

            return string.Empty;
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
                AddMensajeAndWriteLineLog(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " [INF_PETI] " + message, level);
            }
            else
            {
                WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " [INF_PETI] " + message, level);
            }
            Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " [INF_PETI] " + message);
        }
    }
}