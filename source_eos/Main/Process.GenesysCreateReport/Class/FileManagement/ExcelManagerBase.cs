using System.Data;
using Quodem.Monitor.Procesos;

namespace msd.GenesysCreateReportProcess.Class.FileManagement
{
    public abstract class ExcelManagerBase : IExcelManager
    {
        protected ExcelManagerBase(Process process, Resultado result)
        {
            Process = process;
            Result = result;
        }

        /// <summary>
        /// Process
        /// </summary>
        public Process Process { get; private set; }

        /// <summary>
        /// Represent the result of the process
        /// </summary>
        public Resultado Result { get; private set; }

        public abstract bool ExportDataTableToExcel(DataTable dt, string filepath);
    }
}
