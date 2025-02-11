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
    public class TransparencyReportCsvGenerator
    {
        private IProcess _process { get; set; }
        private Resultado _result { get; set; }

        public TransparencyReportCsvGenerator(IProcess process, Resultado result)
        {
            _process = process;
            _result = result;
        }

        public string GenerateCsv(string fileName, DataRow[] dataRows)
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

            var filepath = string.Format("{0}EOS_TransparencyReport_{1}_{2}_{3}.csv", Utilities.Utility.CreateDirectory(AppConfig.FileExportDirectory), fileName, DateTime.Now.ToString("MMMM", ci).FirstCharToUpper(), DateTime.Now.Year);
            File.WriteAllText(filepath, sb.ToString(), new UTF8Encoding(true));

            return filepath;
        }

        private IEnumerable<string> GetCsvColumns()
        {
            IEnumerable<string> columns = new List<string>()
            {
                "id"
                ,"hcp_id"
                ,"source_id"
                ,"golden_id"
                ,"company_id"
                ,"meeting_id"
                ,"meeting_name"
                ,"attendee_type_code"
                ,"meeting_start_date"
                ,"meeting_end_date"
                ,"meeting_address"
                ,"meeting_city"
                ,"meeting_postal_code"
                ,"meeting_country"
                ,"number_of_attendees"
                ,"meeting_type"
                ,"meeting_subtype"
                ,"attendee_title"
                ,"attendee_first_name"
                ,"attendee_last_name"
                ,"hcp_identification"
                ,"full_name"
                ,"affiliated_institution_hco"
                ,"hcp_address"
                ,"city"
                ,"postal_code"
                ,"country"
                ,"license_type"
                ,"license_number"
                ,"license_issuing_country"
                ,"currency_code"
                ,"flights_per_person"
                ,"ground_transfers_per_person"
                ,"public_transport_per_person"
                ,"other_services_per_person"
                ,"bed_and_breakfast_per_person"
                ,"room_only_per_person"
                ,"total_registration_fees_to_efpia"
                ,"mpa_fees_per_person"
                ,"attendee_cancellation_fees_per_person"
                ,"other_costs_per_person_1"
                ,"other_costs_per_person_2"
                ,"av_production_costs_per_group"
                ,"meeting_room_hire"
                ,"other_costs_per_group_1"
                ,"other_costs_per_group_2"
                ,"sponsorship_amount"
                ,"services_and_consultancy_fee_amount"
                ,"services_and_consultancy_contract_related_expense_amount"
                ,"msd_id"
                ,"assistant_type"
                ,"name"
                ,"surname_1"
                ,"type"
                ,"congress_type"
            };

            return columns;
        }
    }
}