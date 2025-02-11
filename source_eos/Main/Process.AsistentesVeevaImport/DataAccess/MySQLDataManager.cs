using Process.AsistentesVeevaImport.BLL;
using Quodem.Monitor.Procesos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Process.AsistentesVeevaImport.DataAccess
{
    public class SqlDataManager
    {
        #region Constructors
        public SqlDataManager(Process process, Resultado result)
        {
            Process = process;
            Result = result;
            //Connection = new SqlConnection(AppConfigs.SqlConnectionString);
        }

        #endregion

        #region Public Properties
        //public SqlConnection Connection { get; private set; }        
        public Process Process { get; set; }
        public Resultado Result { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Ejecuta una query que no retirna lista de resultados. Eje, INSERT o UPDATE
        /// </summary>
        public bool RunQueryNoResult(SqlConnection Connection, string commandQuery, CommandType commandType = CommandType.Text, SqlParameter parameter = null)
        {
            try
            {
                var command = Connection.CreateCommand();
                command.CommandTimeout = 0;
                command.CommandType = commandType;
                command.CommandText = commandQuery;
                if (parameter != null)
                {
                    command.Parameters.Add(parameter);
                }
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Utility.WriteLog(string.Format("Error: SqlDataManager->RunQueryNoResult: {0}", ex.Message));
                Process.WriteLineApp(string.Format("Error: Ejecutando Consulta: Tipo: {0}, {1}", commandType, commandQuery), 2);
                Result.Errores++;
                return false;
            }
            finally
            {
            }
            return true;
        }

        /// <summary>
        /// Ejecuta store Procedure
        /// </summary>
        public bool RunStoreProcedure(SqlConnection Connection, string procedureName, SqlParameter parameter = null)
        {
            if (RunQueryNoResult(Connection, procedureName, CommandType.StoredProcedure, parameter))
            {
                return true;
            }
            return false;
        }
       
        /// <summary>
        /// Importa datos de File stream en la tabla dbo_iw_hcp_genesystemp
        /// </summary>
        public bool RunInserForGenesysTempTable(SqlConnection Connection, string filePath, int pCountInserGroup = 50)
        {
            if (filePath == null || string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException("fileStream");

            Process.WriteLineApp(string.Format("Importando datos en tabla temporal: {0}", AppConfigs.TempTableName), -1);

            #region Local Vars
            var totalRegisters = 0;
            var ret = true;
            #endregion
            StreamReader excelReader = new StreamReader(filePath, Encoding.UTF8);
            try
            {

                var baseInsert =
                    string.Format(
                        "INSERT INTO {0} (idamecs, idpassengerlist, prescribercode, msdid, wein, attendeetype, speakerrole, status, speakerid, speakername, deleted, creationdate, lastupdatedate) VALUES ",
                        AppConfigs.TempTableName);

                var stringBuilderQuery = new StringBuilder(baseInsert);
                var queriesCount = 0;
                var isInsertedGroup = true;

                var dateTimeNow = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");

                Process.WriteLineApp(string.Format("Borrando datos de tabla temporal: {0}", AppConfigs.TempTableName), 1);
                if (TruncateTable(Connection, AppConfigs.TempTableName))
                {
                    Process.WriteLineApp(string.Format("Borrados datos de la tabla temporal satisfactoriamente: {0}", AppConfigs.TempTableName), 1);

                }
                while (!excelReader.EndOfStream)
                {
                    var line = excelReader.ReadLine();
                    if (line != null)
                    {
                        var separator = AppConfigs.TxtSeparator == @"\t" ? "\t" : AppConfigs.TxtSeparator;
                        var values = line.Split(separator.ToCharArray());
                        if (values.Length == 9)
                        {

                            var paramList = new object[]
                            {                                
                                FormatApostropheChars(values[0]),//idamecs                
                                FormatApostropheChars(values[1]),//prescribercode
                                FormatApostropheChars(values[2]),//msdid
                                FormatApostropheChars(values[3]),//wein
                                FormatApostropheChars(values[4]),//attendeetype - cuenta, usuario o ponente
                                FormatApostropheChars(values[5]),//speakerrole
                                FormatApostropheChars(values[6]),//status
                                FormatApostropheChars(values[7]),//speakerid
                                FormatApostropheChars(values[8])//speakername
                            };

                            //Crear insert query a partir de un stringbuilder
                            stringBuilderQuery.AppendFormat(
                                "(RTRIM(LTRIM('{0}')),null,RTRIM(LTRIM('{1}')),RTRIM(LTRIM('{2}')),RTRIM(LTRIM('{3}')),RTRIM(LTRIM('{4}')),RTRIM(LTRIM('{5}')),RTRIM(LTRIM('{6}')),RTRIM(LTRIM('{7}')),RTRIM(LTRIM('{8}')),0,GETDATE(),GETDATE());",
                                paramList).Replace("'null'", "null");
                            queriesCount++;
                            totalRegisters++;

                            if (queriesCount == pCountInserGroup)
                            {
                                queriesCount = 0;
                                if (!ExecuteBlockoInsert(Connection, stringBuilderQuery, baseInsert, ref isInsertedGroup))
                                {
                                    ret = false;
                                }

                            }
                        }
                    }
                    if (!isInsertedGroup)
                    {
                        ret = ExecuteBlockoInsert(Connection, stringBuilderQuery, baseInsert, ref isInsertedGroup);
                    }
                    Process.WriteLineApp(string.Format("Total de registros importados: {0} en {1}", totalRegisters, AppConfigs.TempTableName), 1);
                    Result.Correctos++;
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(string.Format("Error: MySQLDataManager->RunInserForGenesysTempTable: {0}", ex.Message));
                Result.Errores++;
                ret = false;
            }
            finally
            {
                excelReader.Close();
            }

            return ret;
        }

        public List<string> AmecsNoEncontrados(SqlConnection Connection)
        {
            var query = "SELECT * FROM asistentes_veeva_evento_tmp WHERE idamecs NOT IN (SELECT idamecs FROM amecs)";
            List<string> listado = new List<string>();

            SqlCommand command = new SqlCommand(query, Connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                listado.Add(Convert.ToString(reader["idamecs"]));
            }
            reader.Close();
            return listado;
        }

        public List<Passenger> PassengersNoEncontrados(SqlConnection Connection)
        {
            var query = "SELECT * FROM asistentes_veeva_evento_tmp WHERE idpassengerlist IS NULL";
            List<Passenger> listado = new List<Passenger>();

            SqlCommand command = new SqlCommand(query, Connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Passenger pass = new Passenger();
                pass.idamecs = Convert.ToString(reader["idamecs"]);
                pass.prescriberCode = Convert.ToString(reader["prescribercode"]);
                pass.msdId = Convert.ToString(reader["msdid"]);
                pass.wein = Convert.ToString(reader["wein"]);
                pass.attendeeType = Convert.ToString(reader["attendeetype"]);
                pass.speakerId = Convert.ToString(reader["speakerid"]);
                pass.speakerName = Convert.ToString(reader["speakername"]);
                listado.Add(pass);
            }
            reader.Close();
            return listado;
        }

        #endregion

        #region Private Methods

        private bool ExecuteBlockoInsert(SqlConnection Connection, StringBuilder stringBuilderQuery, string baseInsert, ref bool isInsertedGroup)
        {
            try
            {
                var query = stringBuilderQuery.ToString();
                stringBuilderQuery.Clear();
                stringBuilderQuery.Append(baseInsert);
                query = query.Substring(0, query.Length - 1);
                isInsertedGroup = true;
                return RunQueryNoResult(Connection, query);
            }
            catch (Exception ex)
            {
                Utility.WriteLog(string.Format("Error: MySQLDataManager->ExecuteBlockoInsert: {0}", ex.Message));
                Result.Errores++;
            }
            return false;
        }

        private string CreateString(string[] cads, int from, int to)
        {
            string ret = string.Empty;

            try
            {
                for (int i = from; i < to; i++)
                {
                    ret += cads[i];
                }
            }
            catch (Exception ex)
            {
                Utility.WriteLog(string.Format("Error: MySQLDataManager->CreateString: {0}", ex.Message));
                Result.Errores++;
            }

            return FormatApostropheChars(ret);
        }

        /// <summary>
        /// Ejecuta sentencia TRUNCATE en una tabla
        /// </summary>
        private bool TruncateTable(SqlConnection Connection, string tableName)
        {
            try
            {
                var query = string.Format("TRUNCATE table {0};", tableName);
                return RunQueryNoResult(Connection, query);
            }
            catch (Exception ex)
            {
                Utility.WriteLog(string.Format("Error: MySQLDataManager->TruncateTable: {0}", ex.Message));
                Result.Errores++;
            }
            return false;
        }

        private string FormatApostropheChars(string value)
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
