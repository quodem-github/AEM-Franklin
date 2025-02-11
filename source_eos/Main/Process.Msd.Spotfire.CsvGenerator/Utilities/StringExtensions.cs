using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Process.Msd.Spotfire.CsvGenerator.Utilities
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        public static List<string> ClearFields(List<string> fields)
        {
            List<string> newFields = new List<string>();
            foreach (var field in fields)
            {
                string newField = ExcapeCharacters(field);
                newFields.Add(newField);
            }
            return newFields;
        }

        public static string ExcapeCharacters(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                s=s.Replace("&amp;", "&");
                s=s.Replace(";", ",");
            }
            return s;
        }
    }
}