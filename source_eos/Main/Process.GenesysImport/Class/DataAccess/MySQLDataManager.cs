using System;
using System.Data;
using System.IO;
using System.Text;
using Excel;
using Quodem.Monitor.Procesos;
using System.Data.SqlClient;

namespace GenesysImportProcess.Class.DataAccess
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
        public bool RunQueryNoResult(string commandQuery, CommandType commandType = CommandType.Text, SqlParameterCollection parametersCollection = null)
        {
            try
            {
                Connection.Open();
                var command = Connection.CreateCommand();
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
            catch (Exception ex)
            {
                Utility.WriteLog(string.Format("Error: MySQLDataManager->RunQueryNoResult: {0}", ex.Message));
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
        public bool RunStoreProcedure(string procedureName, SqlParameterCollection parametersCollection = null)
        {
            if (RunQueryNoResult(procedureName, CommandType.StoredProcedure, parametersCollection))
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
                        "INSERT INTO {0} (MsdId, GenesysCode, GoldenId, FirstName, LastName, Specialty, RiskLevel, Nif, CenterCode, WorkCenter, LocationCenter, ProvinceCenter, AddressCenter, PostalCodeCenter, Phone, StatusOrigin, Status, CreationDate, UpdateDate  ) VALUES ",
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
                        var separator = AppConfigs.CsvSeparator == @"\t" ? "\t" : AppConfigs.CsvSeparator;
                        var values = line.Split(separator.ToCharArray());
                        if (values.Length == 15)
                        {

                            var fullNameSplited = values[2].Split(',');
                            isInsertedGroup = false;
                            var paramList = new object[]
                            {
                                FormatApostropheChars(values[1]),//MSDID   
                                FormatApostropheChars(values[0]),//GENESIS CODE    
                                FormatApostropheChars(values[14]),//GOLDENID            
                                fullNameSplited.Length > 1? FormatApostropheChars(fullNameSplited[fullNameSplited.Length - 1]): string.Empty,//FirstName
                                CreateString(fullNameSplited, 0, fullNameSplited.Length - 1),//LastName
                                FormatApostropheChars(values[3]),//Specialty
                                FormatApostropheChars(values[6]),//RiskLevel   
                                FormatApostropheChars(values[5]),//Nif
                                FormatApostropheChars(values[7]),//CenterCode
                                FormatApostropheChars(values[8]),//WorkCenter
                                FormatApostropheChars(values[10]),//LocationCenter
                                FormatApostropheChars(values[11]),//ProvinceCenter
                                FormatApostropheChars(values[9]),//AddressCenter
                                FormatApostropheChars(values[12]),//PostalCodeCenter                            
                                FormatApostropheChars(values[13]),//Phone
                                FormatApostropheChars(values[4]),//StatusOrigin
                                FormatApostropheChars("1"),//Status
                                "'" + dateTimeNow + "'",//CreationDate
                                "'" + dateTimeNow + "'"//UpdateDate
                            };

                            //Crear insert query a partir de un stringbuilder
                            stringBuilderQuery.AppendFormat(
                                "({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18});",
                                paramList);
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
                var query = string.Format("TRUNCATE TABLE {0};", tableName);
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
            if (string.IsNullOrEmpty(value) || value.Trim().ToLower() == "null")
            {
                return "null";
            }
            return "'" + value.Replace("'", "''").Replace("\"", "") + "'";
        }

        #endregion
    }
}
