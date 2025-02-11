using System.Collections.Generic;
using System.Configuration;
using Process.BudgetReport.Properties;

namespace Process.BudgetReport.Class
{
    public static class AppConfigs
    {
        #region Constants
        public const char MeetingSeparator = '-';
        public const string ColumnNameMeetingId = "Meeting ID #";
        public const string ColumnNameHcphcoIdentification = "HCP / HCO Identification";
        public const string ColumnNamePostalCode = "Postal Code"; 
        #endregion

        /// <summary>
        /// Cadena de conección a la base de datos de MySQl
        /// </summary>
        public static string SqlConnectionString { get { return ConfigurationManager.ConnectionStrings["Process.BudgetReport.Properties.Settings.SqlConnectioString"].ConnectionString; } }

        /// <summary>
        /// Nombre del procedimiento de almacenado que será utilizado en la syncronización de la tabla final y las tablas temporales.
        /// </summary>
        public static string StoreProcedureDataName { get { return Settings.Default.StoreProcedureDataName; } }

        /// <summary>
        /// Directorio de fichero de log
        /// </summary>
        public static string FileLogDirectory { get { return Settings.Default.FileLogDirectory; } }

        /// <summary>
        /// Descripción del proceso
        /// </summary>
        public static string ProcessDescription { get { return Settings.Default.ProcessDescription; } }
        /// <summary>
        /// Directory to export the excel file
        /// </summary>
        public static string FileExportDirectory { get { return Settings.Default.FileExportDirectory; } }
        /// <summary>
        /// Posfix of the name of the excel file
        /// </summary>
        public static string FileExportName { get { return Settings.Default.FileExportName; } }
        /// <summary>
        /// List of Emails to send report
        /// </summary>
        public static string EmailList { get { return Settings.Default.EmailList; } }
        /// <summary>
        /// From of the email
        /// </summary>
        public static string EmailFrom { get { return Settings.Default.EmailFrom; } }
        /// <summary>
        /// Body tempalte of Email
        /// </summary>
        public static string EmailBodyTemplate { get { return Settings.Default.EmailBodyTemplate; } }
        /// <summary>
        /// Subject of email
        /// </summary>
        public static string EmailSubject { get { return Settings.Default.EmailSubject; } }
        /// <summary>
        /// Name of store procdedure that get the dates of period of data filter.
        /// </summary>
        public static string StoreProcedureTimeControllername { get { return Settings.Default.StoreProcedureTimeControllername; } }

        public static string StoreProcedurePrepare { get { return Settings.Default.StoreProcedurePrepare; } }

        /// <summary>
        /// Number to Instance case
        /// </summary>
        public static string InstanceNumber { get { return Settings.Default.InstanceNumber; } }

        public static List<string> CurrencyFieldsName
        {
            get
            {
                var list = new List<string>
                {
                    "Initial",
                    "Initial Cost",                    
                    "Initial Tax",
                    "Initial Gratuity",
                    "Actual",
                    "Actual Cost",                    
                    "Actual Tax",
                    "Actual Gratuity",
                    "Contracted",
                    "Contracted Cost",                    
                    "Contracted Tax",
                    "Contracted Gratuity",
                    "5 Days Out",
                    "5 Days Out Cost",                    
                    "5 Days Out Tax",
                    "5 Days Out Gratuity"
                };
                return list;
            }
        }

        public static List<string> NumberFieldsName
        {
            get
            {
                var list = new List<string>
                {
                    "Initial Units",
                    "Actual Units",
                    "Contracted Units",
                    "5 Days Out Units"
                };
                return list;
            }
        }

        public static List<string> DateFieldsName
        {
            get
            {
                var list = new List<string>
                {
                    "Planning start date",
                    "Start Date",
                    "End Date"
                };
                return list;
            }
        }

    }
}
