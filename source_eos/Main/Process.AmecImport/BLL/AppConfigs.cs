using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmecImport.BLL
{
    public static class AppConfigs
    {
        public static readonly string ProcessedFolderName = "Processed";
                
        public static string SqlConnectionString { get { return ConfigurationManager.AppSettings["SqlConnectionString"]; } }
        public static string StoreProcedureNameLoadMasterTableAmecs { get { return ConfigurationManager.AppSettings["StoreProcedureNameLoadMasterTableAmecs"]; } }

        #region FTP
        public static string SftpHost { get { return ConfigurationManager.AppSettings["SftpHost"]; } }
        public static string SftpUserName { get { return ConfigurationManager.AppSettings["SftpUserName"]; } }
        public static string SftpPassword { get { return ConfigurationManager.AppSettings["SftpPassword"]; } }
        public static int SftpPort { get { return int.Parse(ConfigurationManager.AppSettings["SftpPort"]); } }
        public static string SftpInitialDirectory { get { return ConfigurationManager.AppSettings["SftpInitialDirectory"]; } }
        #endregion

        public static string TempTableName { get { return ConfigurationManager.AppSettings["TempTableName"]; } }
        public static int ItemsPerInsert { get { return int.Parse(ConfigurationManager.AppSettings["ItemsPerInsert"]); } }
        public static string DateFormatAmecFileMasterPattern { get { return ConfigurationManager.AppSettings["DateFormatAmecFileMasterPattern"]; } }
        public static string ProcessDescription { get { return ConfigurationManager.AppSettings["ProcessDescription"]; } }
        public static string LocalDirectoryToDownloadPath { get { return ConfigurationManager.AppSettings["LocalDirectoryToDownloadPath"]; } }
        public static string LocalDirectoryFiledPath { get { return ConfigurationManager.AppSettings["LocalDirectoryFilePath"]; } }
        public static string AmecsFileMasterPattern { get { return ConfigurationManager.AppSettings["AmecsFileMasterPattern"]; } }
        public static string TxtSeparator { get { return ConfigurationManager.AppSettings["TxtSeparator"]; } }
        public static string FileLogDirectory { get { return ConfigurationManager.AppSettings["FileLogDirectory"]; } }
        public static bool TestMode { get { return Boolean.Parse(ConfigurationManager.AppSettings["TestMode"]); } }
        public static bool GetFileFromLocal { get { return Boolean.Parse(ConfigurationManager.AppSettings["GetFileFromLocal"]); } }
        public static string SupportQuodemEmail { get { return ConfigurationManager.AppSettings["SupportQuodemEmail"]; } }
        public static string FromEmail { get { return ConfigurationManager.AppSettings["FromEmail"]; } }
        
    }
}
