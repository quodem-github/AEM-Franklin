using System;
using Process.MeetingReport.Class;
using Quodem.Monitor.Procesos;

namespace Process.MeetingReport
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
