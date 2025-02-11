using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Quodem.Monitor.Procesos;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using AmecImport.BLL;

namespace AmecImport.FTP
{
    public class SftpManager
    {
        #region Properties
        /// <summary>
        /// Objeto SftpClient
        /// </summary>
        private SftpClient Client { get; set; }
        /// <summary>
        /// Objeto que representa el proceso de ejecución
        /// </summary>
        private Process Process { get; set; }
        /// <summary>
        /// Objeto para registrar los resultados de las pricipales operaciones.
        /// </summary>
        private Resultado Result { get; set; }

        #endregion

        #region Constructors
        public SftpManager(Process process, Resultado result)
        {
            Process = process;
            Result = result;

            Client = new SftpClient(AppConfigs.SftpHost, AppConfigs.SftpPort, AppConfigs.SftpUserName, AppConfigs.SftpPassword);
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Obtiene una lista de SftpFile con los ficheros que cumplen con el patrón
        /// </summary>
        /// <param name="pPatternMatch">expreción regular Patrón por la que se va a hacer la búsqueda</param>
        public List<SftpFile> GetAllSftpFiles(string pPatternMatch = ".*")
        {
            try
            {
                return Client.ListDirectory(AppConfigs.SftpInitialDirectory).Where(x => Regex.IsMatch(x.FullName, pPatternMatch)).ToList();
            }
            catch (Exception ex)
            {
                Utility.WriteLog(string.Format("Error: SftpManager->GetAllSftpFiles: {0}", ex.Message));
            }
            return null;
        }

        /// <summary>
        /// Descarga el fichero más reciente que coincida con el patrón
        /// </summary>
        /// <param name="pToLocalPath">Directorio local de descarga</param>
        /// <param name="pPatternMatch">Expreción regular</param>
        /// <param name="deleteAfter">Indica si se va a eliminar el fichero del servidor remoto después de haberlo descargado satisfactoriamente</param>
        /// <param name="dateFormat"></param>
        public string DownloadFileAndGetFilePath(string pToLocalPath, string pPatternMatch = ".*", bool deleteAfter = false, string dateFormat = "ddMMyyyy")
        {
            try
            {
                Client.Connect();
                Process.WriteLineApp("Conectado al servidor de SFTP satisfactoriamente", -1);
                //Result.Correctos++;

                var files = GetAllSftpFiles(pPatternMatch);
                var firstFile = GetLastByName(files, dateFormat);

                if (firstFile == null)
                {
                    return null;
                }

                Process.WriteLineApp(string.Format("Descargando fichero {0}", firstFile.Name), -1);

                if (!Directory.Exists(pToLocalPath))
                {
                    Directory.CreateDirectory(pToLocalPath);
                }


                if (File.Exists(pToLocalPath + firstFile.Name) && AppConfigs.TestMode)
                {
                    return (pToLocalPath + firstFile.Name);
                }

                try
                {
                    var memoryStream = File.Open(pToLocalPath + firstFile.Name, FileMode.Create);

                    Client.DownloadFile(firstFile.FullName, memoryStream);

                    memoryStream.Close();

                    string previousName = firstFile.Name;

                    if (!AppConfigs.TestMode)
                    {
                        if (!Client.Exists(Path.Combine(AppConfigs.ProcessedFolderName, (firstFile.Name.Replace(".txt", "") + "_" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt"))))
                        {
                            firstFile.MoveTo(string.Format("{0}{1}/{2}", AppConfigs.SftpInitialDirectory, AppConfigs.ProcessedFolderName, (firstFile.Name.Replace(".txt", "") + "_" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt")));
                        }
                        else
                        {
                            firstFile.MoveTo(string.Format("{0}{1}/{2}", AppConfigs.SftpInitialDirectory, AppConfigs.ProcessedFolderName, (firstFile.Name.Replace(".txt", "") + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt")));
                        }
                    }

                    Process.WriteLineApp(
                        string.Format("Fichero {0} descargado satisfactoriamente", previousName), -1);
                    //Result.Correctos++;


                    if (!deleteAfter)
                    {
                        return (pToLocalPath + previousName);
                    }

                    firstFile.Delete();
                    Process.WriteLineApp(
                        string.Format("Fichero {0} eliminado satisfactoriamente del servidor remoto", previousName),
                        -1);
                    //Result.Correctos++;


                    return (pToLocalPath + previousName);
                }
                catch (Exception ex)
                {
                    Utility.WriteLog(string.Format("Error: No se ha podido obtener el fichero: {0} : {1}",
                        firstFile.FullName, ex.Message));
                    Result.Errores++;
                }

            }
            catch (Exception ex)
            {
                Utility.WriteLog(string.Format("Error: SftpManager->DownloadFileAndGetStream: {0}", ex.Message));
                Result.Errores++;
            }
            finally
            {
               Client.Disconnect();
            }

            return null;
        }

        private SftpFile GetLastByName(IEnumerable<SftpFile> files, string dateFormat)
        {
            var values = new Dictionary<DateTime, SftpFile>();
            var pattern = dateFormat.Replace("d", @"\d").Replace("M", @"\d").Replace("y", @"\d");
            var provider = CultureInfo.InvariantCulture;

            foreach (var file in files)
            {
                if (string.IsNullOrWhiteSpace(pattern))
                {
                    return file;
                }
                else
                {
                    Match match = Regex.Match(file.Name, pattern);
                    if (match.Success && match.Length > 0)
                    {
                        try
                        {
                            values.Add(DateTime.ParseExact(match.Value, dateFormat, provider), file);
                        }
                        catch (FormatException) { }
                    }
                }
            }

            if (values.Count > 0)
            {
                var resultList = values.OrderByDescending(x => x.Key);

                return resultList.FirstOrDefault().Value;
            }

            return null;
        }

        public bool UploadFile(string path)
        {
            var sftpcilent = Client;

            try
            {
                if (File.Exists(path))
                {
                    var file = new FileStream(path, FileMode.Open);
                    sftpcilent.Connect();
                    sftpcilent.UploadFile(file, string.Format("{0}/{1}", AppConfigs.SftpInitialDirectory + AppConfigs.ProcessedFolderName, Path.GetFileName(path)));
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(string.Format("Error: SftpManager->UploadFile: {0}", ex.Message));
                Process.WriteLineApp(string.Format("Error: El fichero no se ha subido correctamente: {0}", ex.Message), 2);
                Result.Errores++;
            }

            return false;
        }

        #endregion
    }
}
