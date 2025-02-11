using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Quodem.Monitor.Procesos;
using System.Data.SqlClient;

namespace Process.MeetingReport.Class.DataAccess
{
    public class SqlDataManager
    {
        #region Constructors
        public SqlDataManager(Process process, Resultado result)
        {
            Process = process;
            Result = result;
        }
        #endregion

        #region Public Properties
        public Process Process { get; set; }
        public Resultado Result { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Ejecuta una query que no retirna lista de resultados. Eje, INSERT o UPDATE
        /// </summary>
        public bool RunQueryNoResult(string commandQuery, CommandType commandType = CommandType.Text, List<SqlParameter> parametersCollection = null)
        {
            using (var connection = new SqlConnection(AppConfigs.SqlConnectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandTimeout = 0;
                        command.CommandType = commandType;
                        command.CommandText = commandQuery;
                        if (parametersCollection != null)
                        {
                            foreach (var parameter in parametersCollection)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Utility.WriteLog(string.Format("Error: SqlDataManager->RunQueryNoResult: {0}", ex.Message));
                    Process.WriteLineApp(
                        string.Format("Error: Ejecutando Consulta: Tipo: {0}, {1}", commandType, commandQuery), 2);
                    Result.Errores++;
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return true;
        }

        /// <summary>
        /// Ejecuta store Procedure
        /// </summary>
        public bool RunStoreProcedureNoResult(string procedureName, List<SqlParameter> parametersCollection = null)
        {
            if (RunQueryNoResult(procedureName, CommandType.StoredProcedure, parametersCollection))
            {
                return true;
            }
            return false;
        }

        public DataSet RunStoreProcedureWithResult(string procedureName, List<SqlParameter> parametersCollection = null)
        {
            var dataSet = new DataSet();

            using (var connection = new SqlConnection(AppConfigs.SqlConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    try
                    {
                        connection.Open();
                        command.CommandTimeout = 0;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = procedureName;

                        if (parametersCollection != null)
                        {
                            foreach (var parameter in parametersCollection)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }

                        using (var sqlDataAdapter = new SqlDataAdapter(command))
                        {
                            sqlDataAdapter.Fill(dataSet);
                        }
                    }
                    catch (Exception ex)
                    {
                        Utility.WriteLog(string.Format("Error: SqlDataManager->RunStoreProcedureWithResult: {0}",
                            ex.Message));
                        Process.WriteLineApp(string.Format("Error: Ejecutando Consulta: {0}", procedureName), 2);
                        Result.Errores++;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return dataSet;
        }

        public DataSet PocessedData(string procedureName, List<SqlParameter> parametersCollection = null)
        {
            var dataSet = new DataSet();

            using (var connection = new SqlConnection(AppConfigs.SqlConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    try
                    {
                        connection.Open();
                        command.CommandTimeout = 0;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = procedureName;

                        if (parametersCollection != null)
                        {
                            foreach (var parameter in parametersCollection)
                            {
                                command.Parameters.Add(parameter);
                            }

                            using (var sqlDataAdapter = new SqlDataAdapter(command))
                            {
                                sqlDataAdapter.Fill(dataSet);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Utility.WriteLog(string.Format("Error: SqlDataManager->RunStoreProcedureWithResult: {0}",
                            ex.Message));
                        Process.WriteLineApp(string.Format("Error: Ejecutando Consulta: {0}", procedureName), 2);
                        Result.Errores++;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return dataSet;
        }

        public DataSet RunQueryWithResult(string query)
        {
            var dataSet = new DataSet();

            using (var connection = new SqlConnection(AppConfigs.SqlConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandTimeout = 0;
                    command.CommandType = CommandType.Text;
                    command.CommandText = query;

                    try
                    {
                        connection.Open();

                        using (var sqlDataAdapter = new SqlDataAdapter(command))
                        {
                            sqlDataAdapter.Fill(dataSet);
                        }
                    }
                    catch (Exception ex)
                    {
                        Utility.WriteLog(string.Format("Error: SqlDataManager->RunStoreProcedureWithResult: {0}",
                            ex.Message));
                        Result.Errores++;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return dataSet;
        }

        /// <summary>
        /// Ejecura un bloque de sentencias Insert
        /// </summary>
        /// <param name="stringBuilderQuery"></param>
        /// <param name="baseInsert"></param>
        /// <param name="isInsertedGroup"></param>
        /// <returns></returns>
        public bool ExecuteBlockoInsert(StringBuilder stringBuilderQuery, string baseInsert, ref bool isInsertedGroup)
        {
            try
            {
                var query = stringBuilderQuery.ToString();
                stringBuilderQuery.Clear();
                stringBuilderQuery.Append(baseInsert);
                query = query.Substring(0, query.Length - 1);
                isInsertedGroup = true;
                return RunQueryNoResult(query);
            }
            catch (Exception ex)
            {
                Utility.WriteLog(string.Format("Error: SqlDataManager->ExecuteBlockoInsert: {0}", ex.Message));
                Result.Errores++;
            }
            return false;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Ejecuta sentencia TRUNCATE en una tabla
        /// </summary>
        public bool TruncateTable(string tableName)
        {
            try
            {
                var query = string.Format("SET SQL_SAFE_UPDATES = 0;TRUNCATE {0};SET SQL_SAFE_UPDATES = 1;", tableName);
                return RunQueryNoResult(query);
            }
            catch (Exception ex)
            {
                Utility.WriteLog(string.Format("Error: SqlDataManager->TruncateTable: {0}", ex.Message));
                Result.Errores++;
            }
            return false;
        }

        protected string FormatApostropheChars(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return value.Replace("'", "''").Replace("\"", "");
        }

        #endregion
    }
}
