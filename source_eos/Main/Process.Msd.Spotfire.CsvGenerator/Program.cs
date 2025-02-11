using Quodem.Monitor.Procesos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Process.Msd.Spotfire.CsvGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var transparencyReportProcess = new TransparencyReportProcess(new Resultado());
            transparencyReportProcess.Execute();

            var dossierReportProcess = new DossierReportProcess(new Resultado());
            dossierReportProcess.Execute();

            var feesReportProcess = new FeesReportProcess(new Resultado());
            feesReportProcess.Execute();

            var petitionReportProcess = new PetitionReportProcess(new Resultado());
            petitionReportProcess.Execute();

            var meetingReportProcess = new MeetingReportProcess(new Resultado());
            meetingReportProcess.Execute();
        }
    }
}