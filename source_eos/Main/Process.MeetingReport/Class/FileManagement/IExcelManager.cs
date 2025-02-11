using System.Data;
using Quodem.Monitor.Procesos;

namespace Process.MeetingReport.Class.FileManagement
{
    public interface IExcelManager
    {
        /// <summary>
        /// Process
        /// </summary>
        Process Process { get; }

        /// <summary>
        /// Represent the result of the process
        /// </summary>
        Resultado Result { get; }

        /// <summary>
        /// Export an ADO.NET Table to a Excel file.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        bool ExportDataTableToExcel(DataTable dt, string filepath);
    }
}
