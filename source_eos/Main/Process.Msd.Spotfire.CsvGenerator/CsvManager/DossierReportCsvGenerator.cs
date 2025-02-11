using Process.Msd.Spotfire.CsvGenerator.Interfaces;
using Process.Msd.Spotfire.CsvGenerator.Utilities;
using Quodem.Monitor.Procesos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Process.Msd.Spotfire.CsvGenerator.CsvManager
{
    public class DossierReportCsvGenerator
    {
        private IProcess _process { get; set; }
        private Resultado _result { get; set; }

        public DossierReportCsvGenerator(IProcess process, Resultado result)
        {
            _process = process;
            _result = result;
        }

        public string GenerateCsv(DataRow[] dataRows)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = GetCsvColumns();
            sb.AppendLine(string.Join(";", columnNames));

            int rowId = 1;

            foreach (DataRow row in dataRows)
            {
                List<string> fields = new List<string>() { rowId.ToString() };
                fields.AddRange(row.ItemArray.Select(field => Regex.Replace(field.ToString(), @"\t|\n|\r", " ")));
                fields = Utilities.StringExtensions.ClearFields(fields);
                sb.AppendLine(string.Join(";", fields));

                rowId++;
            }

            CultureInfo ci = new CultureInfo("en-US");

            var filepath = string.Format("{0}EOS_DossierReport_{1}_{2}.csv", Utilities.Utility.CreateDirectory(AppConfig.FileExportDirectory), DateTime.Now.ToString("MMMM", ci).FirstCharToUpper(), DateTime.Now.Year);
            File.WriteAllText(filepath, sb.ToString(), new UTF8Encoding(true));

            return filepath;
        }

        private IEnumerable<string> GetCsvColumns()
        {
            IEnumerable<string> columns = new List<string>()
            {
                "id"
                ,"dossier_id"
                ,"unity_code"
                ,"region"
                ,"area"
                ,"district"
                ,"petitioner_name"
                ,"petitioner_surname"
                ,"petitioner_position"
                ,"amec"
                ,"agency"
                ,"congress_type_code"
                ,"activity"
                ,"place"
                ,"start_date"
                ,"end_date"
                ,"dossier_status"
                ,"order_number"
                ,"dossier_date"
                ,"dossier_amount"
                ,"registrations_amount"
                ,"accommodation_amount"
                ,"other_services_amount"
                ,"displacement_amoun"
                ,"people_number"
                ,"product"
                ,"product_percentage"
                ,"department"
                ,"salesforce"
                ,"district"
            };

            return columns;
        }
    }
}