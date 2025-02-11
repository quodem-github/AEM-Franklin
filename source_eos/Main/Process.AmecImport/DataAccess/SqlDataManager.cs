using AmecImport.BLL;
using Quodem.Monitor.Procesos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmecImport.DTO;
using System.Globalization;

namespace AmecImport.DataAccess
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
                Connection.Close();
                return false;
            }
            Connection.Close();
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
        /// Importa datos de File stream en la tabla temporal
        /// </summary>
        public ErrorResult RunInserForAmecsTempTable(string filePath, string executionDateTime, int pCountInserGroup = 50)
        {
            if (filePath == null || string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException("fileStream");

            Process.WriteLineApp(string.Format("Importando datos en tabla temporal: {0}", AppConfigs.TempTableName), -1);

            ErrorResult result = new ErrorResult();
            result.Status = true;

            #region Local Vars
            var totalRegisters = 0;
            var ret = true;
            #endregion
            StreamReader excelReader = new StreamReader(filePath, Encoding.UTF8);
            try
            {
                
                var baseInsert =
                    string.Format(
                        "INSERT INTO {0} ([Event name], [EM Event id], [Record type description], [Event type description], [Status description], [Owner wein], [Owner name], [Canceled], [Committed cost], [Actual cost], [RecordCreateDate], fechacomienzo, fechafinalizacion) VALUES ",
                        AppConfigs.TempTableName);
                //meter los campos nuevos
                var stringBuilderQuery = new StringBuilder(baseInsert);
                var queriesCount = 0;
                var isInsertedGroup = true;                

                while (!excelReader.EndOfStream)
                {
                    var line = excelReader.ReadLine();
                    if (line != null)
                    {
                        var separator = AppConfigs.TxtSeparator == @"\t" ? "\t" : AppConfigs.TxtSeparator;
                        var values = line.Split(separator.ToCharArray());

                        if (line.Contains('"')) { 
                            var name = line.Substring(0, line.LastIndexOf('"') + 1);
                            var allData = line.Substring(line.LastIndexOf('"') + 1);
                            var values2 = new List<string>();
                            values2.Add(name.Substring(1, name.Length - 2));
                            values2.AddRange(allData.Substring(1).Split(separator.ToCharArray()));
                            values = values2.ToArray();
                        }

                        if (values.Length == 12)
                        {
                            var fechaInicio = "null";
                            var fechaFin= "null";

                            if (!string.IsNullOrEmpty(values[10]))
                            {
                                fechaInicio = DateTime.ParseExact(values[10], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                            }

                            if (!string.IsNullOrEmpty(values[11]))
                            {
                                fechaFin = DateTime.ParseExact(values[11], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
                            }

                            var paramList = new object[]
                            {                                
                                FormatApostropheChars(values[0]),//[Event name]                
                                FormatApostropheChars(values[1]),//[EM Event id]
                                FormatApostropheChars(values[2]),//[Record type description]
                                FormatApostropheChars(values[3]),//[Event type description]
                                FormatApostropheChars(values[4]),//[Status description]
                                FormatApostropheChars(values[5]),//[Owner wein]
                                FormatApostropheChars(values[6]),//[Owner name]
                                FormatApostropheChars(values[7]),//[Canceled]
                                FormatApostropheChars(values[8]),//[Committed cost]
                                FormatApostropheChars(values[9]),//[Actual cost]
                                fechaInicio, //[fechacomienzo]
                                fechaFin//[fechafinalizacion]
                            };

                            //Crear insert query a partir de un stringbuilder
                            stringBuilderQuery.AppendFormat(
                                "(RTRIM(LTRIM('{0}')),RTRIM(LTRIM('{1}')),RTRIM(LTRIM('{2}')),RTRIM(LTRIM('{3}')),RTRIM(LTRIM('{4}')),RTRIM(LTRIM('{5}')),RTRIM(LTRIM('{6}')), RTRIM(LTRIM('{7}')), RTRIM(LTRIM('{8}')), RTRIM(LTRIM('{9}')), '" + executionDateTime + "', RTRIM(LTRIM('{10}')), RTRIM(LTRIM('{11}')) );",
                                paramList).Replace("'null'", "null");
                            queriesCount++;
                            totalRegisters++;

                            if (queriesCount == pCountInserGroup)
                            {
                                queriesCount = 0;
                                if (!ExecuteBlockoInsert(stringBuilderQuery, baseInsert, ref isInsertedGroup))
                                {
                                    //ret = false;
                                    result.Status = false;
                                    ErrorDto error = new ErrorDto();
                                    error.EventName = FormatApostropheChars(values[0]);//[Event name]                
                                    error.EmEventId = FormatApostropheChars(values[1]);//[EM Event id]
                                    error.RecordTypeDescription = FormatApostropheChars(values[2]);//[Record type description]
                                    error.EventTypeDescription = FormatApostropheChars(values[3]);//[Event type description]
                                    error.StatusDescription = FormatApostropheChars(values[4]);//[Status description]
                                    error.OwnerWein = FormatApostropheChars(values[5]);//[Owner wein]
                                    error.OwnerName = FormatApostropheChars(values[6]);//[Owner name]
                                    error.Canceled = FormatApostropheChars(values[7]);//[Canceled]
                                    error.CommittedCost = FormatApostropheChars(values[8]);//[Committed cost]
                                    error.ActualCost = FormatApostropheChars(values[9]);//[Actual cost]
                                    error.fechaComienzo = DateTime.ParseExact(values[10], "dd/MM/yyyy", CultureInfo.InvariantCulture); //[fechacomienzo]
                                    error.fechaFinalizacion = DateTime.ParseExact(values[11], "dd/MM/yyyy", CultureInfo.InvariantCulture);//[fechafinalizacion]
                                    
                                    result.ErrorList.Add(error);
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
                Utility.WriteLog(string.Format("Error: SqlDataManager->RunInserForAmecsTempTable: {0}", ex.Message));
                Result.Errores++;
                //ret = false;
                result.Status = false;
            }
            finally
            {
                excelReader.Close();
            }

            return result;
        }

        public ErrorResult EvaluateStoredProcedureErrors(string recordCreateDate)
        {
            ErrorResult result = new ErrorResult();
            result.ErrorList = new List<ErrorDto>();
            result.Status = true;

            string query =
                "select	[Event name], [EM Event id], [Record type description], [Event type description], [Status description], " +
                "[Owner wein], [Owner name], [Canceled], [Committed cost], [Actual cost], convert(varchar(10),[RecordCreateDate], 103) as [RecordCreateDate], convert(varchar(10),fechacomienzo, 103) as fechacomienzo, convert(varchar(10),fechafinalizacion, 103) as fechafinalizacion " + 
                "from amecs_masterImport " +
                "am where RecordCreateDate = '" + recordCreateDate + "' AND  " +
                " (( " +
                                  " am.idamecs is null or  am.idamecs = '') OR " +
                " not exists(select * from amecs  where amecs.idamecs = am.idamecs ) " +
                                  " ) " +
                " and " +
                "  ( " +
                "      am.idamecs is null or " +
                "      am.idamecs = '' or " +
                "      am.idsolicitante is null or " +
                "      am.idsolicitante = '' or " +
                "      am.idcreadopor is null or " +
                "      am.idcreadopor = '' or " +
                "      am.fechaamecs is null or " +
                "      am.fechaamecs = '' or " +
                "      am.nwein is  null or " +
                "      am.nwein = '' or " +
                "      am.idtipoactividad is null or " +
                "      am.idtipoactividad = '' or " +
                "      am.idestado is null or " +
                "      am.idestado = '' or " +
                "      am.fechaultimaactualizacion is null or " +
                "      am.fechaultimaactualizacion = '' or " +
                "      am.idposition is null or " +
                "      am.idposition = '' " +
                "  )";

            Connection.Open();
            var command = Connection.CreateCommand();
            command.CommandTimeout = 0;
            command.CommandText = query; 
            SqlDataReader resultQuery = command.ExecuteReader();

            if (resultQuery.HasRows)
            {
                while (resultQuery.Read())
                {
                    ErrorDto error = new ErrorDto();
                    error.EventName = resultQuery[0].ToString();//[Event name]                
                    error.EmEventId = resultQuery[1].ToString();//[EM Event id]
                    error.RecordTypeDescription = resultQuery[2].ToString();//[Record type description]
                    error.EventTypeDescription = resultQuery[3].ToString();//[Event type description]
                    error.StatusDescription = resultQuery[4].ToString();//[Status description]
                    error.OwnerWein = resultQuery[5].ToString();//[Owner wein]
                    error.OwnerName = resultQuery[6].ToString();//[Owner name]
                    error.Canceled = resultQuery[7].ToString();//[Canceled]
                    error.CommittedCost = resultQuery[8].ToString(); //[Committed cost]
                    error.ActualCost = resultQuery[9].ToString();//[Actual cost]
                    error.RecordCreateDate = DateTime.ParseExact(resultQuery[10].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);//[fechacomienzo]
                    error.fechaComienzo = DateTime.ParseExact(resultQuery[11].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);//[fechacomienzo]
                    error.fechaFinalizacion = DateTime.ParseExact(resultQuery[12].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);//[fechafinalizacion]

                    result.ErrorList.Add(error);
                }

                resultQuery.Close();
            }

            return result;
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
                Utility.WriteLog(string.Format("Error: SqlDataManager->ExecuteBlockoInsert: {0}", ex.Message));
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
                Utility.WriteLog(string.Format("Error: SqlDataManager->CreateString: {0}", ex.Message));
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
                Utility.WriteLog(string.Format("Error: SqlDataManager->TruncateTable: {0}", ex.Message));
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
