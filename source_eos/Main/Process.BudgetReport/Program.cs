using System;
using Process.BudgetReport.Class;
using Quodem.Monitor.Procesos;

namespace Process.BudgetReport
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
