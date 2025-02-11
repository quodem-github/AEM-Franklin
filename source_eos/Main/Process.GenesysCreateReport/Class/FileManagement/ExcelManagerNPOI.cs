using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Quodem.Monitor.Procesos;
using System.Globalization;
using NPOI.SS.Formula.Functions;
using NPOI.SS.Util;

namespace msd.GenesysCreateReportProcess.Class.FileManagement
{
    public class ExcelManagerNpoi : ExcelManagerBase
    {
        #region Properties

        private Dictionary<string, string> dictionarySheets;

        #endregion

        #region Cosntructor

        public ExcelManagerNpoi(Process process, Resultado result)
            : base(process, result)
        {
            dictionarySheets = new Dictionary<string, string>();

            dictionarySheets.Add("NUEVAS", "NuevoRegistro = 1");
            dictionarySheets.Add("AGREGADO", "NuevoRegistro = 0");

        }
        #endregion

        #region Public Methods
        public override bool ExportDataTableToExcel(DataTable dt, string filepath)
        {
            var xssfworkbook = new XSSFWorkbook();
            var file = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            var result = true;

            try
            {
                var sheetDisclaimer = (XSSFSheet)xssfworkbook.CreateSheet("DISCLAIMER");
                

                var rowHeaderDisclaimer = (XSSFRow)sheetDisclaimer.CreateRow(12);

                var styleHeaderDisclaimer = xssfworkbook.CreateCellStyle();
                styleHeaderDisclaimer.FillForegroundColor = IndexedColors.White.Index;
                styleHeaderDisclaimer.FillPattern = FillPattern.SolidForeground;
                styleHeaderDisclaimer.FillBackgroundColor = IndexedColors.White.Index;
                styleHeaderDisclaimer.Alignment = HorizontalAlignment.Center;
                styleHeaderDisclaimer.VerticalAlignment = VerticalAlignment.Center; 

                var fontHeaderDisclaimer = (XSSFFont)xssfworkbook.CreateFont();
                fontHeaderDisclaimer.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                fontHeaderDisclaimer.IsBold = true;
                styleHeaderDisclaimer.SetFont(fontHeaderDisclaimer);
                
                XSSFCell cellDisclaimer = (XSSFCell)rowHeaderDisclaimer.CreateCell(5);
                cellDisclaimer.CellStyle = styleHeaderDisclaimer;
                cellDisclaimer.SetCellValue("Los datos de este informe han sido revisados y verificados por el equipo técnico de Quodem");

                var cra = new NPOI.SS.Util.CellRangeAddress(12, 20, 5, 17);
                sheetDisclaimer.AddMergedRegion(cra);
                
                RegionUtil.SetBorderLeft(1, cra,sheetDisclaimer, xssfworkbook);
                RegionUtil.SetBorderRight(1, cra, sheetDisclaimer, xssfworkbook);
                RegionUtil.SetBorderTop(1, cra, sheetDisclaimer, xssfworkbook);
                RegionUtil.SetBorderBottom(1, cra, sheetDisclaimer, xssfworkbook);
                


                foreach (KeyValuePair<string, string> dictionarySheet in dictionarySheets)
                {
                    var sheet = (XSSFSheet) xssfworkbook.CreateSheet(dictionarySheet.Key);

                    var rowHeader = (XSSFRow) sheet.CreateRow(0);

                    var styleBody = (XSSFCellStyle) xssfworkbook.CreateCellStyle();

                    // cell background
                    styleBody.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Blue.Index;
                    
                    // font color
                    var fontBody = (XSSFFont) xssfworkbook.CreateFont();
                    fontBody.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                    styleBody.SetFont(fontBody);
                    
                    var styleHeader = xssfworkbook.CreateCellStyle();
                    styleHeader.FillForegroundColor = IndexedColors.White.Index;
                    styleHeader.FillPattern = FillPattern.SolidForeground;
                    styleHeader.FillBackgroundColor = IndexedColors.White.Index;

                    var fontHeader = (XSSFFont) xssfworkbook.CreateFont();
                    fontHeader.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                    fontHeader.IsBold = true;
                    styleHeader.SetFont(fontHeader);

                    XSSFCell cell;

                    //here you can set the headers for your excel sheet bu if you dont want to you can use the default column names from database
                    for (var j = 0; j < dt.Columns.Count - 1; j++)
                    {
                        cell = (XSSFCell) rowHeader.CreateCell(j);
                        var columnName = dt.Columns[j].ToString();
                        cell.CellStyle = styleHeader;
                        cell.SetCellValue(columnName);
                        sheet.AutoSizeColumn(j);
                    }

                    //here you will fill the tables with values from database
                    var dtFiltered = dt.Select(dictionarySheet.Value);
                    for (var i = 0; i < dtFiltered.Length; i++)
                    {
                        var row = (XSSFRow) sheet.CreateRow(i + 1);

                        for (var j = 0; j < dt.Columns.Count - 1; j++)
                        {
                            cell = (XSSFCell) row.CreateCell(j);
                            var columnName = dt.Columns[j].ToString();
                            if (!string.IsNullOrEmpty(dtFiltered[i][columnName].ToString()))
                            {
                                decimal nuemricValue = 0;
                                var value = dtFiltered[i][columnName].ToString();

                                if (AppConfigs.CurrencyFieldsName.Contains(columnName) &&
                                    decimal.TryParse(dtFiltered[i][columnName].ToString(), out nuemricValue))
                                {
                                    value = String.Format(CultureInfo.InvariantCulture,"{0:0.00}", nuemricValue);
                                }

                                value = ApplyTrasformations(value, columnName);

                                cell.SetCellValue(value);
                            }
                            cell.CellStyle = styleBody;
                        }
                    }
                }
                xssfworkbook.Write(file);
            }
            catch (Exception ex)
            {
                Utility.WriteLog(string.Format("Error: ExcelManagerNPOI->ExportDataTableToExcel: {0}", ex.Message));
                Process.WriteLineApp(string.Format("Error: Creando Excel en: {0}", filepath), 2);
                Result.Errores++;
                result = false;
            }
            finally
            {
                // Clean up 
                file.Close();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            return result;
        }

     
        #endregion

        #region Private Methods
        /// <summary>
        /// Applay specifics values trasformations </summary>
        /// <param name="value">Value</param>
        /// <param name="columnName">Column Name</param>
        /// <returns></returns>
        private static string ApplyTrasformations(string value, string columnName)
        {
            if (columnName == AppConfigs.ColumnNameMeetingId)
            {
                value = string.Format("{0}{1}", AppConfigs.InstanceNumber, value);
            }

            if (columnName == AppConfigs.ColumnNameHcphcoIdentification)
            {
                var elements = value.Split(AppConfigs.MeetingSeparator);
                if (elements.Length == 2)
                {
                    value = string.Format("{0} {1} {2}{3}", elements[0], AppConfigs.MeetingSeparator, AppConfigs.InstanceNumber, elements[1]);
                }
            }

            if (columnName == AppConfigs.ColumnNamePostalCode)
            {
                if (value.Length > 5 || value.Length < 4)
                {
                    value = string.Empty;
                }
                else if(value.Length == 4)
                {
                    value = string.Format("0{0}", value);
                }
            }

            return value;
        }
        #endregion
    }
}
