using System;
using System.IO;
using GenesysImportProcess.Class;
using GenesysImportProcess.Class.DataAccess;
using GenesysImportProcess.Class.FTP;
using Quodem.Monitor.Procesos;

namespace GenesysImportProcess
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
        Quodem.Monitor.Procesos.Resultado _resultado;
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
                Filepattern = AppConfigs.GenesysFileMasterPattern;
                ToPath = AppConfigs.LocalDirectoryToDownloadPath;
                CountInserGroup = AppConfigs.ItemsPerInsert;
                SftpManager = new SftpManager(this, Result);
                SqlDataManager = new SqlDataManager(this, Result);

                Result.Texto = AppConfigs.ProcessDescription;

                WriteLineApp(Result.Texto, -1);
                WriteLineApp("Iniciado proceso...", -1);

                var filePathGenesyscr = DownloadFile(ToPath, Filepattern, false, AppConfigs.DateFormatGenesysFileMasterPattern);

                if (filePathGenesyscr == null)
                {
                    WriteLineApp("No existe en el FTP el fichero necesarios para realizar la importación", -1);
                    //Result.Correctos++;
                }
                else
                {
                    WriteLineApp("Importando datos...", 0);
                    if (ImportData(filePathGenesyscr, CountInserGroup))
                    {
                        WriteLineApp("Datos importados satisfactoriamente", -1);
                        //Result.Correctos++;
                    }
                    else
                    {
                        WriteLineApp("Datos importados erróneamente", -1);
                        Result.Errores++;
                    }
                    
                    WriteLineApp("Cargando tabla maestra de genesys en la base de datos...", -1);
                    if (LoadMasterTable())
                    {
                        WriteLineApp("Datos cargados satisfactoriamente", -1);
                        //Result.Correctos++;
                    }
                    else
                    {
                        WriteLineApp("Datos cargados erróneamente", -1);
                        Result.Errores++;
                    }

                    WriteLineApp("Actualizando tabla maestra passengers_list las bases de datos...", -1);
                    if (UpdatePassengerList())
                    {
                        WriteLineApp("Datos procesados satisfactoriamente", -1);
                        //Result.Correctos++;
                    }
                    else
                    {
                        WriteLineApp("Datos procesados erróneamente", -1);
                        Result.Errores++;
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
        private bool ImportData(string filePath, int pCountInserGroup = 50)
        {
            return SqlDataManager.RunInserForGenesysTempTable(filePath, pCountInserGroup);
        }

        /// <summary>
        /// Actualiza la tabla dbo_iw_hcp_genesyscr a partir de los dato en las tablas temporales
        /// </summary>
        private bool LoadMasterTable()
        {
            return SqlDataManager.RunStoreProcedure(AppConfigs.StoreProcedureNameLoadMasterTable);
        }

        /// <summary>
        /// Actualiza la tabla dbo_iw_hcp_genesyscr a partir de los dato en las tablas temporales
        /// </summary>
        private bool UpdatePassengerList()
        {
            bool result = true;

            result = result && SqlDataManager.RunStoreProcedure(AppConfigs.StoreProcedureUpdatePassengerList);

            return result;
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
        #endregion
    }
}
