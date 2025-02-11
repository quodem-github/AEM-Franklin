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
    public class MeetingReportCsvGenerator
    {
        private IProcess _process { get; set; }
        private Resultado _result { get; set; }

        public MeetingReportCsvGenerator(IProcess process, Resultado result)
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

            var filepath = string.Format("{0}EOS_MeetingReport_{1}_{2}.csv", Utilities.Utility.CreateDirectory(AppConfig.FileExportDirectory), DateTime.Now.ToString("MMMM", ci).FirstCharToUpper(), DateTime.Now.Year);
            File.WriteAllText(filepath, sb.ToString(), new UTF8Encoding(true));

            return filepath;
        }

        private IEnumerable<string> GetCsvColumns()
        {
            IEnumerable<string> columns = new List<string>()
            {
                "id",
                "country_funding_meeting",
                "service_level",
                "request_status",
                "idamec",
                "event_code",
                "meeting_id",
                "service_type",
                "event_title",
                "planning_start_date",
                "start_date",
                "end_date",
                "division_funding_meeting",
                "business_unit_department_therapeutic_area",
                "requester_email",
                "meeting_owner_email",
                "meeting_type",
                "meeting_subtype",
                "meeting_logistics_provider",
                "estimated_number_of_internal_attendees",
                "estimated_number_of_external_attendees",
                "estimated_number_of_speakers",
                "request_expected_attendees",
                "actual_number_of_internal_attendees",
                "actual_number_of_external_attendees",
                "actual_number_of_speakers",
                "number_of_physician_attendees",
                "product_1",
                "product_2",
                "product_3",
                "product_4",
                "product_5",
                "reasons_for_hotel_choice",
                "event_country",
                "event_city",
                "location_name",
                "total_initial",
                "total_contracted",
                "total_5_days_out",
                "total_actual",
                "actual_accommodation",
                "actual_other_fees",
                "actual_commission_rebate",
                "actual_food_and_beverage",
                "actual_ground_transportation",
                "actual_hotel_and_venue_miscellaneous",
                "actual_management_fees",
                "actual_registration_participant_fees",
                "actual_miscellaneous",
                "actual_signage_communication",
                "actual_meeting_space_and_room_rental",
                "actual_audio_and_visual",
                "actual_group_air_and_rail",
                "meeting_planner",
                "merck_account_manager",
                "organizational_unit",
                "organizational_code",
                "who_is_doing_procurement",
                "msd_event_id"
            };

            return columns;
        }
    }
}