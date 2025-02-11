using System;
using System.Data;
using System.Reflection;
using Quodem.Monitor.Procesos;

namespace msd.GenesysCreateReportProcess.Class.FileManagement
{
    public class ExcelManager : ExcelManagerBase
    {
        #region Cosntructor
        public ExcelManager(Process process, Resultado result) : base(process, result) { } 
        #endregion

        #region Public Methods

        /// <summary>
        /// Export an ADO.NET Table to a Excel file.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public override bool ExportDataTableToExcel(DataTable dt, string filepath)
        {
            var result = true;
            try
            {
                // Start Excel and get Application object. 
                var oXL = new Microsoft.Office.Interop.Excel.Application { Visible = true, DisplayAlerts = false };

                // Get a new workbook. 
                var oWB = oXL.Workbooks.Add(Missing.Value);

                // Get the Active sheet 
                var oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oWB.ActiveSheet;
                oSheet.Name = "Data";

                var rowCount = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    rowCount += 1;
                    for (var i = 1; i < dt.Columns.Count + 1; i++)
                    {
                        // Add the header the first time through 
                        if (rowCount == 2)
                        {
                            oSheet.Cells[1, i] = dt.Columns[i - 1].ColumnName;
                        }
                        oSheet.Cells[rowCount, i] = dr[i - 1].ToString();
                    }
                }

                // Resize the columns 
                var oRange = oSheet.Range[oSheet.Cells[1, 1], oSheet.Cells[rowCount, dt.Columns.Count]];
                oRange.EntireColumn.AutoFit();

                // Save the sheet and close 
                oWB.SaveAs(filepath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
                    Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value);
                oWB.Close(Missing.Value, Missing.Value, Missing.Value);
                oXL.Quit();

            }
            catch (Exception ex)
            {
                Utility.WriteLog(string.Format("Error: ExcelManager->ExportDataTableToExcel: {0}", ex.Message));
                Process.WriteLineApp(string.Format("Error: Creando Excel en: {0}", filepath), 2);
                Result.Errores++;
                result = false;

            }
            finally
            {
                // Clean up 
                // NOTE: When in release mode, this does the trick 
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            return result;
        } 
        #endregion

    }
}
