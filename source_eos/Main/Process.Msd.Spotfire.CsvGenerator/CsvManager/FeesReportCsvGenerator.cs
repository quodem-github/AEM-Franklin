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
    public class FeesReportCsvGenerator
    {
        private IProcess _process { get; set; }
        private Resultado _result { get; set; }

        public FeesReportCsvGenerator(IProcess process, Resultado result)
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

            var filepath = string.Format("{0}EOS_FeesReport_{1}_{2}.csv", Utilities.Utility.CreateDirectory(AppConfig.FileExportDirectory), DateTime.Now.ToString("MMMM", ci).FirstCharToUpper(), DateTime.Now.Year);
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
                ,"petitioner_surname"
                ,"petitioner_name"
                ,"amec"
                ,"agency"
                ,"event"
                ,"event_start_date"
                ,"surname_1"
                ,"name"
                ,"msd_id"
                ,"dossier_type"
                ,"fees"
                ,"assistant_type"
                ,"activity_type"
                ,"risk_level"
                ,"belongs_genesis"
                ,"justifications"
                ,"direct_payment"
                ,"society_payment"
                ,"speakers_and_moderators_type_of_meeting"
                ,"speakers_and_moderators_duration_of_activity"
                ,"speakers_and_moderators_preparation_time"
                ,"speakers_and_moderators_ps_level"
                ,"speakers_and_moderators_fee_hour"
                ,"speakers_and_moderators_maximum_fees"
                ,"speakers_and_moderators_presentation_at_health_centre_meeting"
                ,"speakers_and_moderators_more_than_one_presentation_in_a_day"
                ,"speakers_and_moderators_videoconference_replay"
                ,"speakers_and_moderators_type_of_speaker"
                ,"eif_and_advisory_boards_duration_of_activity"
                ,"eif_and_advisory_boards_preparation_time"
                ,"eif_and_advisory_boards_ps_level"
                ,"eif_and_advisory_boards_fee_hour"
                ,"eif_and_advisory_boards_maximum_fees"
                ,"consulting_contract_type"
                ,"consulting_number_of_days_consulting"
                ,"calculator_justification"
                ,"calculator_rules_applied"
                ,"district"
                ,"department"
                ,"salesforce"
            };

            return columns;
        }
    }
}