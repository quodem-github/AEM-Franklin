using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Quodem.Monitor.Procesos;
using System.Globalization;

namespace Process.MeetingReport.Class.FileManagement
{
    public class ExcelManagerNpoi : ExcelManagerBase
    {
        #region Cosntructor

        public ExcelManagerNpoi(Process process, Resultado result)
            : base(process, result)
        {            
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
                var sheet = (XSSFSheet)xssfworkbook.CreateSheet("Report");

                var rowHeader = (XSSFRow)sheet.CreateRow(0);

                var styleBody = (XSSFCellStyle)xssfworkbook.CreateCellStyle();

                // cell background
                styleBody.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Blue.Index;

                // font color
                var fontBody = (XSSFFont)xssfworkbook.CreateFont();
                fontBody.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                styleBody.SetFont(fontBody);

                var styleHeader = xssfworkbook.CreateCellStyle();
                styleHeader.FillForegroundColor = IndexedColors.White.Index;
                styleHeader.FillPattern = FillPattern.SolidForeground;
                styleHeader.FillBackgroundColor = IndexedColors.White.Index;

                var fontHeader = (XSSFFont)xssfworkbook.CreateFont();
                fontHeader.Color = NPOI.HSSF.Util.HSSFColor.Black.Index;
                fontHeader.IsBold = true;
                styleHeader.SetFont(fontHeader);

                NPOI.XSSF.UserModel.XSSFCellStyle cellStyle = (NPOI.XSSF.UserModel.XSSFCellStyle)xssfworkbook.CreateCellStyle();
                NPOI.SS.UserModel.IFont fontcell = (NPOI.SS.UserModel.IFont)xssfworkbook.CreateFont();
                fontcell.FontHeightInPoints = 11;
                cellStyle.SetFont(fontcell);

                NPOI.XSSF.UserModel.XSSFCellStyle cellStyleDateTime = (NPOI.XSSF.UserModel.XSSFCellStyle)xssfworkbook.CreateCellStyle();
                cellStyleDateTime.SetFont(fontBody);
                IDataFormat dateFormatCustom = xssfworkbook.CreateDataFormat();
                cellStyleDateTime.DataFormat = dateFormatCustom.GetFormat("dd/MM/yyyy");

                NPOI.XSSF.UserModel.XSSFCellStyle cellStyleNumberDecimals = (NPOI.XSSF.UserModel.XSSFCellStyle)xssfworkbook.CreateCellStyle();
                cellStyleNumberDecimals.SetFont(fontBody);
                IDataFormat numberDecimalsFormatCustom = xssfworkbook.CreateDataFormat();
                cellStyleNumberDecimals.DataFormat = numberDecimalsFormatCustom.GetFormat("0.00");

                NPOI.XSSF.UserModel.XSSFCellStyle cellStyleNumberDecimalsDollars = (NPOI.XSSF.UserModel.XSSFCellStyle)xssfworkbook.CreateCellStyle();
                cellStyleNumberDecimalsDollars.SetFont(fontBody);
                IDataFormat numberDecimalsFormatCustomDollars = xssfworkbook.CreateDataFormat();
                cellStyleNumberDecimalsDollars.DataFormat = numberDecimalsFormatCustomDollars.GetFormat("$0.00");

                NPOI.XSSF.UserModel.XSSFCellStyle cellStyleNumber = (NPOI.XSSF.UserModel.XSSFCellStyle)xssfworkbook.CreateCellStyle();
                cellStyleNumber.SetFont(fontBody);
                IDataFormat numberFormatCustom = xssfworkbook.CreateDataFormat();
                cellStyleNumber.DataFormat = numberFormatCustom.GetFormat("0");

                XSSFCell cell;

                //here you can set the headers for your excel sheet bu if you dont want to you can use the default column names from database
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    cell = (XSSFCell)rowHeader.CreateCell(j);
                    var columnName = dt.Columns[j].ToString();
                    cell.CellStyle = styleHeader;
                    cell.SetCellValue(columnName);
                    sheet.AutoSizeColumn(j);
                }

                //here you will fill the tables with values from database
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var row = (XSSFRow)sheet.CreateRow(i + 1);

                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        cell = (XSSFCell)row.CreateCell(j);
                        var columnName = dt.Columns[j].ToString();
                        if (!string.IsNullOrEmpty(dt.Rows[i][columnName].ToString()))
                        {
                            decimal decimalValue = 0;
                            int numericField = 0;
                            var value = dt.Rows[i][columnName].ToString();

                            if (AppConfigs.CurrencyFieldsName.Contains(columnName) &&
                                decimal.TryParse(dt.Rows[i][columnName].ToString(), out decimalValue))
                            {
                                cell.CellStyle = cellStyleNumberDecimalsDollars;
                                cell.SetCellValue(double.Parse(dt.Rows[i][columnName].ToString()));
                            }
                            else if (AppConfigs.NumberFieldsName.Contains(columnName) &&
                                int.TryParse(dt.Rows[i][columnName].ToString(), out numericField))
                            {
                                cell.CellStyle = cellStyleNumber;
                                cell.SetCellValue(int.Parse(dt.Rows[i][columnName].ToString()));
                            }
                            else if (AppConfigs.DateFieldsName.Contains(columnName))
                            {
                                cell.CellStyle = cellStyleDateTime;
                                cell.SetCellValue((DateTime)dt.Rows[i][columnName]);
                            }
                            else
                            {
                                cell.CellStyle = cellStyle;
                                value = ApplyTrasformations(value, columnName);
                                cell.SetCellValue(value);
                            }
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
