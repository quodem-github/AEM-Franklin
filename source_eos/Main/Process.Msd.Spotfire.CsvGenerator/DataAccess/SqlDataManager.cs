using Process.Msd.Spotfire.CsvGenerator.Interfaces;
using Process.Msd.Spotfire.CsvGenerator.Utilities;
using Quodem.Monitor.Procesos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Process.Msd.Spotfire.CsvGenerator.DataAccess
{
    public class SqlDataManager
    {
        private IProcess _process { get; set; }
        private Resultado _result { get; set; }

        public SqlDataManager(IProcess process, Resultado result)
        {
            _process = process;
            _result = result;
        }

        /// <summary>
        /// Ejecuta query sin retornar resultado
        /// </summary>
        /// <param name="commandQuery"></param>
        /// <param name="commandType"></param>
        /// <param name="parametersCollection"></param>
        /// <returns></returns>
        public bool RunQueryNoResult(string commandQuery, CommandType commandType = CommandType.Text, List<SqlParameter> parametersCollection = null)
        {
            using (var connection = new SqlConnection(AppConfig.SqlConnectionString))
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
                    _process.WriteLineApp(
                        string.Format("Error: Ejecutando Consulta: Tipo: {0}, {1}", commandType, commandQuery), 2);
                    _result.Errores++;
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

        /// <summary>
        /// Ejecuta procedimiento almacenado con resultado de vuelta
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parametersCollection"></param>
        /// <returns></returns>
        public DataSet RunStoreProcedureWithResult(string procedureName, List<SqlParameter> parametersCollection = null)
        {
            var dataSet = new DataSet();

            using (var connection = new SqlConnection(AppConfig.SqlConnectionString))
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
                        _process.WriteLineApp(string.Format("Error: Ejecutando Consulta: {0}", procedureName), 2);
                        _result.Errores++;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return dataSet;
        }

        public DataSet ProcessedData(string procedureName, List<SqlParameter> parametersCollection = null)
        {
            var dataSet = new DataSet();

            using (var connection = new SqlConnection(AppConfig.SqlConnectionString))
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
                        _process.WriteLineApp(string.Format("Error: Ejecutando Consulta: {0}", procedureName), 2);
                        _result.Errores++;
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
        /// Ejecuta una query que retorna resultado
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataSet RunQueryWithResult(string query)
        {
            var dataSet = new DataSet();

            using (var connection = new SqlConnection(AppConfig.SqlConnectionString))
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
                        _result.Errores++;
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
                _result.Errores++;
            }
            return false;
        }
    }
}