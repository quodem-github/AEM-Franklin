using Process.AsistentesVeevaImport.BLL;
using Process.AsistentesVeevaImport.DataAccess;
using Process.AsistentesVeevaImport.FTP;
using Quodem.Monitor.Procesos;
using System;
using System.Data.SqlClient;

namespace Process.AsistentesVeevaImport
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
            SqlConnection Connection = new SqlConnection(AppConfigs.SqlConnectionString);
            try
            {
                Connection.Open();
                Filepattern = AppConfigs.AsistentesVeevaFileMasterPattern;
                ToPath = AppConfigs.LocalDirectoryToDownloadPath;
                CountInserGroup = AppConfigs.ItemsPerInsert;
                SftpManager = new SftpManager(this, Result);
                SqlDataManager = new SqlDataManager(this, Result);
                var filePathGenesyscr = "";
                Result.Texto = AppConfigs.ProcessDescription;

                WriteLineApp(Result.Texto, -1);
                WriteLineApp("Iniciado proceso...", -1);

                if (AppConfigs.GetFileFromLocal)
                    filePathGenesyscr = AppConfigs.LocalDirectoryToDownloadPath + AppConfigs.AsistentesVeevaFileMasterPattern;
                else
                    filePathGenesyscr = DownloadFile(ToPath, Filepattern, false,AppConfigs.DateFormatGenesysFileMasterPattern);
                    

                if (filePathGenesyscr == null)
                {
                    WriteLineApp("No existe en el FTP el fichero necesarios para realizar la importación", -1);
                    //Result.Correctos++;
                }
                else
                {
                    WriteLineApp("Importando datos...", 0);
                    if (ImportData(Connection, filePathGenesyscr, CountInserGroup))
                    {
                        WriteLineApp("Datos importados satisfactoriamente", -1);
                        //Result.Correctos++;
                    }
                    else
                    {
                        WriteLineApp("Datos importados erróneamente", -1);
                        Result.Errores++;
                    }
                    
                    WriteLineApp("Actualizando tabla maestra de asistentes veeva en la base de datos...", -1);
                    if (LoadMasterTable(Connection))
                    {
                        WriteLineApp("Datos cargados satisfactoriamente", -1);
                        //Result.Correctos++;
                    }
                    else
                    {
                        WriteLineApp("Datos cargados erróneamente", -1);
                        Result.Errores++;
                    }

                }

                

                WriteLineApp("Proceso Terminado", -1);
                WriteLineApp("", -1);

                //coger registros que han "fallado"
                //en cada pasada del foreach del listado de errores -> Result.Errores++;
                //en caso de que haya errores -> this.ErrorSolicitud = true;
                var amecsNoEncontrados = SqlDataManager.AmecsNoEncontrados(Connection);
                var passengersNoEncontrados = SqlDataManager.PassengersNoEncontrados(Connection);

                if (amecsNoEncontrados.Count > 0 || passengersNoEncontrados.Count > 0)
                {
                    this.ErrorSolicitud = true;

                    WriteLineApp("Amecs no encontrados: " + amecsNoEncontrados.Count, -1);
                    foreach (var item in amecsNoEncontrados)
                    {
                        WriteLineApp(item, -1);
                        Result.Errores++;
                    }

                    WriteLineApp("", -1);
                    WriteLineApp("Passengers no encontrados: " + passengersNoEncontrados.Count, -1);
                    if (passengersNoEncontrados.Count > 0)
                    {
                        WriteLineApp("IDAMECS - PRESCRIBERCODE - MSDID - WEIN - ATTENDEETYPE - SPEAKERID - SPEAKERNAME", -1);
                    }
                    foreach (var item2 in passengersNoEncontrados)
                    {
                        WriteLineApp(item2.idamecs + " - " + item2.prescriberCode + " - " + item2.msdId + " - " + item2.wein + " - " + item2.attendeeType + " - " + item2.speakerId + " - " + item2.speakerName, -1);
                        Result.Errores++;
                    }
                    WriteLineApp("", -1);
                }

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
            finally
            {
                Connection.Close();
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
        private bool ImportData(SqlConnection Connection, string filePath, int pCountInserGroup = 50)
        {
            return SqlDataManager.RunInserForGenesysTempTable(Connection, filePath, pCountInserGroup);
        }

        /// <summary>
        /// Actualiza la tabla asistentes_veeva_evento a partir de los dato en la tabla temporal
        /// </summary>
        private bool LoadMasterTable(SqlConnection Connection)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "insertNewRegisters";
            param.Value = AppConfigs.InsertPeticionariosIfNotExists;
            return SqlDataManager.RunStoreProcedure(Connection, AppConfigs.StoreProcedureNameLoadMasterTableAsistentesVeeva, param);
        }
        
        /// <summary>
        /// Actualiza la tabla dbo_iw_hcp_genesyscr a partir de los dato en las tablas temporales
        /// </summary>
        private bool LoadMasterTableAprobaciones(SqlConnection Connection)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = "DeleteNotLoaded";
            param.Value = AppConfigs.DeleteNotLoaded;
            return SqlDataManager.RunStoreProcedure(Connection, AppConfigs.StoreProcedureNameLoadMasterTableEstructuraAprobacion, param);
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
