// -----------------------------------------------------------------------
// <copyright file="Helper.cs" company="Quodem Consultores S.L.">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Data.SqlClient;

namespace EOS.Repositorios
{
    using System.Data;
    using System.Data.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Method to Get Command TimeOut
        /// </summary>
        /// <returns></returns>
        public static int GetCommandTimeout()
        {
            var keyValue = System.Configuration.ConfigurationManager.AppSettings["SQL.CommandTimeout"];
            return string.IsNullOrWhiteSpace(keyValue) ? 0 : int.Parse(keyValue);
        }


        /// <summary>
        /// Method to get new command.
        /// </summary>
        /// <param name="query">Query string.</param>
        /// <param name="connection">Context connection</param>
        /// <param name="commandType">Command type</param>
        /// <returns></returns>
        public static SqlCommand GetSelectCommand(string query, SqlConnection connection, CommandType commandType)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(query, connection);
            command.CommandTimeout = GetCommandTimeout();
            return command;
        }

        #region Converter

        public static IList<string> IListStringConvertToDto(DataTable dtTable)
        {
            IList<string> collection = new List<string>();
            foreach (DataRow row in dtTable.Rows)
            {
                collection.Add(row[0].ToString());
            }

            return collection;
        }

        public static IList<int> IListIntConvertToDto(DataTable dtTable)
        {
            IList<int> collection = new List<int>();
            foreach (DataRow row in dtTable.Rows)
            {
                collection.Add(int.Parse(row[0].ToString()));
            }

            return collection;
        }

        public static ICollection<int> ICollectionIntConvertToDto(DataTable dtTable)
        {
            ICollection<int> collection = new List<int>();
            foreach (DataRow row in dtTable.Rows)
            {
                collection.Add(int.Parse(row[0].ToString()));
            }

            return collection;
        }

        #endregion

    }
}
