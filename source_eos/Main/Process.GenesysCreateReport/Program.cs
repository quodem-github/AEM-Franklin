using System;
using msd.GenesysCreateReportProcess.Class;
using Quodem.Monitor.Procesos;

namespace msd.GenesysCreateReportProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = new Process(new Resultado());
            process.Execute();
        }
    }
}
