using System;
using GenesysImportProcess.Class;
using Quodem.Monitor.Procesos;

namespace GenesysImportProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = new Process();
            process.Execute();
        }
    }
}
