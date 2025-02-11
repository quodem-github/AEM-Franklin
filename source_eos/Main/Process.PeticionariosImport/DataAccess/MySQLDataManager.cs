using PeticionariosImportProcess.BLL;
using Quodem.Monitor.Procesos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeticionariosImportProcess.DataAccess
{
    public class SqlDataManager
    {
        #region Constructors
        public SqlDataManager(Process process, Resultado result)
        {
            Process = process;
            Result = result;
            Connection = new SqlConnection(AppConfigs.SqlConnectionString);
        }

        #endregion

        #region Public Properties
        public SqlConnection Connection { get; private set; }        
        public Process Process { get; set; }
        public Resultado Result { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Ejecuta una query que no retirna lista de resultados. Eje, INSERT o UPDATE
        /// </summary>
        public bool RunQueryNoResult(string commandQuery, CommandType commandType = CommandType.Text, SqlParameter parameter = null)
        {
            try
            {
                Connection.Open();
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
                Connection.Close();
            }
            return true;
        }

        /// <summary>
        /// Ejecuta store Procedure
        /// </summary>
        public bool RunStoreProcedure(string procedureName, SqlParameter parameter = null)
        {
            if (RunQueryNoResult(procedureName, CommandType.StoredProcedure, parameter))
            {
                return true;
            }
            return false;
        }
       
        /// <summary>
        /// Importa datos de File stream en la tabla dbo_iw_hcp_genesystemp
        /// </summary>
        public bool RunInserForGenesysTempTable(string filePath, int pCountInserGroup = 50)
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
                        "INSERT INTO {0} (login, wein, Cargo, Nombre, Apellido1, Apellido2, Direccion, Poblacion, CodPostal, Telefono, Movil, Email, Departamento, FuerzaVentas, Distrito, WeinManager, LevelCode, Company) VALUES ",
                        AppConfigs.TempTableName);

                var stringBuilderQuery = new StringBuilder(baseInsert);
                var queriesCount = 0;
                var isInsertedGroup = true;

                var dateTimeNow = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");

                Process.WriteLineApp(string.Format("Borrando datos de tabla temporal: {0}", AppConfigs.TempTableName), 1);
                if (TruncateTable(AppConfigs.TempTableName))
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
                        if (values.Length == 18)
                        {

                            var paramList = new object[]
                            {                                
                                FormatApostropheChars(values[0]),//login                
                                FormatApostropheChars(values[1]),//wein
                                FormatApostropheChars(values[2]),//Cargo
                                FormatApostropheChars(values[3]),//Nombre
                                FormatApostropheChars(values[4]),//Apellido1
                                FormatApostropheChars(values[5]),//Apellido2
                                FormatApostropheChars(values[6]),//Direccion
                                FormatApostropheChars(values[7]),//Poblacion
                                FormatApostropheChars(values[8]),//CodPostal
                                FormatApostropheChars(values[9]),//Telefono
                                FormatApostropheChars(values[10]),//Movil
                                FormatApostropheChars(values[11]),//Email
                                FormatApostropheChars(string.IsNullOrWhiteSpace(values[12]) ? "null" : values[12]),//Departamento
                                FormatApostropheChars(string.IsNullOrWhiteSpace(values[13]) ? "null" : values[13]),//FuerzaVentas
                                FormatApostropheChars(string.IsNullOrWhiteSpace(values[14]) ? "null" : values[14]),//Distrito
                                FormatApostropheChars(values[15]),//WeinManager
                                FormatApostropheChars(values[16]),//WeinManager
                                FormatApostropheChars(values[17])//Company -> MSD u ORGANON
                            };

                            //Crear insert query a partir de un stringbuilder
                            stringBuilderQuery.AppendFormat(
                                "(RTRIM(LTRIM('{0}')),RTRIM(LTRIM('{1}')),RTRIM(LTRIM('{2}')),RTRIM(LTRIM('{3}')),RTRIM(LTRIM('{4}')),RTRIM(LTRIM('{5}')),RTRIM(LTRIM('{6}')),RTRIM(LTRIM('{7}')),RTRIM(LTRIM('{8}')),RTRIM(LTRIM('{9}')),RTRIM(LTRIM('{10}')),RTRIM(LTRIM('{11}')),RTRIM(LTRIM('{12}')),RTRIM(LTRIM('{13}')),RTRIM(LTRIM('{14}')),RTRIM(LTRIM('{15}')),RTRIM(LTRIM('{16}')),RTRIM(LTRIM('{17}')));",
                                paramList).Replace("'null'", "null");
                            queriesCount++;
                            totalRegisters++;

                            if (queriesCount == pCountInserGroup)
                            {
                                queriesCount = 0;
                                if (!ExecuteBlockoInsert(stringBuilderQuery, baseInsert, ref isInsertedGroup))
                                {
                                    ret = false;
                                }

                            }
                        }
                    }
                    if (!isInsertedGroup)
                    {
                        ret = ExecuteBlockoInsert(stringBuilderQuery, baseInsert, ref isInsertedGroup);
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

        #endregion

        #region Private Methods

        private bool ExecuteBlockoInsert(StringBuilder stringBuilderQuery, string baseInsert, ref bool isInsertedGroup)
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
        private bool TruncateTable(string tableName)
        {
            try
            {
                var query = string.Format("TRUNCATE table {0};", tableName);
                return RunQueryNoResult(query);
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
