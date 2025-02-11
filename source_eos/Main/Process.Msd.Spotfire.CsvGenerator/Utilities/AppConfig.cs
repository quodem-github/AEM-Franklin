using Process.Msd.Spotfire.CsvGenerator.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Process.Msd.Spotfire.CsvGenerator.Utilities
{
    public static class AppConfig
    {
        /// <summary>
        /// Cadena de conexión
        /// </summary>
        public static string SqlConnectionString { get { return ConfigurationManager.ConnectionStrings["Process.Msd.Spotfire.CsvGenerator.Properties.Settings.SqlConnectionString"].ConnectionString; } }

        /// <summary>
        /// Directorio de fichero de log
        /// </summary>
        public static string FileLogDirectory { get { return Settings.Default.FileLogDirectory; } }

        public static string StoreProcedurePrepare { get { return Settings.Default.StoreProcedurePrepare; } }

        /// <summary>
        /// Nombre del procedimiento de almacenado que será utilizado en la sincronización de la tabla final y las tablas temporales.
        /// </summary>
        public static string StoreProcedureDataName { get { return Settings.Default.StoreProcedureDataName; } }

        /// <summary>
        /// Directory to export the excel file
        /// </summary>
        public static string FileExportDirectory { get { return Settings.Default.FileExportDirectory; } }

        /// <summary>
        /// List of Emails to send report
        /// </summary>
        public static string EmailList { get { return Settings.Default.EmailList; } }

        /// <summary>
        /// From of the email
        /// </summary>
        public static string EmailFrom { get { return Settings.Default.EmailFrom; } }
    }
}