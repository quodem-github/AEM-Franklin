using System.Configuration;

namespace GenesysImportProcess.Class
{
    public static class AppConfigs
    {
        public static readonly string ProcessedFolderName = "Processed";


        /// <summary>
        /// Sftp Host
        /// </summary>
        public static string Host { get { return Properties.Settings.Default.SftpHost; } }
        /// <summary>
        /// Sftp usuario
        /// </summary>
        public static string UserName { get { return Properties.Settings.Default.SftpUserName; } }
        /// <summary>
        /// Sftp Contraseña
        /// </summary>
        public static string Password { get { return Properties.Settings.Default.SftpPassword; } }
        /// <summary>
        /// Sftp Puerto
        /// </summary>
        public static int Port { get { return Properties.Settings.Default.SftpPort; } }
        /// <summary>
        /// Sftp directorio base
        /// </summary>
        public static string InitialDirectory { get { return Properties.Settings.Default.SftpInitialDirectory; } }
        /// <summary>
        /// Cadena de conección a la base de datos de MySQl
        /// </summary>
        public static string SqlConnectionString { get { return ConfigurationManager.ConnectionStrings["GenesysImportProcess.Properties.Settings.SqlConnectionString"].ConnectionString; } }        
        /// <summary>
        /// Nombre del procedimiento de almacenado que será utilizado en la syncronización de la tabla final y las tablas temporales.
        /// </summary>
        public static string StoreProcedureNameLoadMasterTable { get { return Properties.Settings.Default.StoreProcedureNameLoadMasterTable; } }
        /// <summary>
        /// 
        /// </summary>
        public static string StoreProcedureUpdatePassengerList { get { return Properties.Settings.Default.StoreProcedureUpdatePassengerList; } }        
        /// <summary>
        /// Expreción regular de fichero de hcp
        /// </summary>
        public static string GenesysFileMasterPattern { get { return Properties.Settings.Default.GenesysFileMasterPattern; } }
        /// <summary>
        /// Directorio de descarga
        /// </summary>
        public static string LocalDirectoryToDownloadPath { get { return Properties.Settings.Default.LocalDirectoryToDownloadPath; } }
        /// <summary>
        /// Cantidad de filas que conformarán in un insert en la generación de los mismos.
        /// </summary>
        public static int ItemsPerInsert { get { return Properties.Settings.Default.ItemsPerInsert; } }
        /// <summary>
        /// Nombre en Base de datos de la tabla con los hcp
        /// </summary>
        public static string TempTableName { get { return Properties.Settings.Default.TempTableName; } }
        /// <summary>
        /// Separador de CSV
        /// </summary>
        public static string CsvSeparator { get { return Properties.Settings.Default.CsvSeparator; } }
        /// <summary>
        /// Indica el uso de fichero local en la importación, en caso de existir.
        /// </summary>
        public static bool TestMode { get { return Properties.Settings.Default.TestMode; } }
        /// <summary>
        /// Directorio de fichero de log
        /// </summary>
        public static string FileLogDirectory { get { return Properties.Settings.Default.FileLogDirectory; } }
        /// <summary>
        /// Descripción del proceso
        /// </summary>
        public static string ProcessDescription { get { return Properties.Settings.Default.ProcessDescription; } }
        public static string DateFormatGenesysFileMasterPattern { get { return Properties.Settings.Default.DateFormatGenesysFileMasterPattern; } }
    }
}
