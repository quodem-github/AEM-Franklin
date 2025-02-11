using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeticionariosImportProcess.BLL
{
    public static class AppConfigs
    {
        public static readonly string ProcessedFolderName = "Processed";
                
        public static string SqlConnectionString { get { return ConfigurationManager.AppSettings["SqlConnectionString"]; } }
        public static string StoreProcedureNameLoadMasterTablePeticionarios { get { return ConfigurationManager.AppSettings["StoreProcedureNameLoadMasterTablePeticionarios"]; } }
        public static string StoreProcedureNameLoadMasterTableEstructuraAprobacion { get { return ConfigurationManager.AppSettings["StoreProcedureNameLoadMasterTableEstructuraAprobacion"]; } }        
        public static string InsertPeticionariosIfNotExists { get { return ConfigurationManager.AppSettings["InsertPeticionariosIfNotExists"]; } }
        public static string DeleteNotLoaded { get { return ConfigurationManager.AppSettings["DeleteNotLoaded"]; } }
        public static string SftpHost { get { return ConfigurationManager.AppSettings["SftpHost"]; } }
        public static string SftpUserName { get { return ConfigurationManager.AppSettings["SftpUserName"]; } }
        public static string SftpPassword { get { return ConfigurationManager.AppSettings["SftpPassword"]; } }
        public static int SftpPort { get { return int.Parse(ConfigurationManager.AppSettings["SftpPort"]); } }
        public static string SftpInitialDirectory { get { return ConfigurationManager.AppSettings["SftpInitialDirectory"]; } }
        public static string TempTableName { get { return ConfigurationManager.AppSettings["TempTableName"]; } }
        public static int ItemsPerInsert { get { return int.Parse(ConfigurationManager.AppSettings["ItemsPerInsert"]); } }
        public static string DateFormatGenesysFileMasterPattern { get { return ConfigurationManager.AppSettings["DateFormatGenesysFileMasterPattern"]; } }
        public static string ProcessDescription { get { return ConfigurationManager.AppSettings["ProcessDescription"]; } }
        public static string LocalDirectoryToDownloadPath { get { return ConfigurationManager.AppSettings["LocalDirectoryToDownloadPath"]; } }
        public static string LocalDirectoryFiledPath { get { return ConfigurationManager.AppSettings["LocalDirectoryFilePath"]; } }
        public static string PeticionariosFileMasterPattern { get { return ConfigurationManager.AppSettings["PeticionariosFileMasterPattern"]; } }
        public static string TxtSeparator { get { return ConfigurationManager.AppSettings["TxtSeparator"]; } }
        public static string FileLogDirectory { get { return ConfigurationManager.AppSettings["FileLogDirectory"]; } }
        public static bool TestMode { get { return Boolean.Parse(ConfigurationManager.AppSettings["TestMode"]); } }
        public static bool GetFileFromLocal { get { return Boolean.Parse(ConfigurationManager.AppSettings["GetFileFromLocal"]); } }
        
    }
}
