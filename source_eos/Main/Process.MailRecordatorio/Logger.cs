using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace msd.MailRecordatorio
{
    public class Logger
    {
        private string fileName;

        // si pasamos la ruta de un archivo, su utilizará ese para hacer el log
        public Logger(string file)
        {
            fileName = file;
        }

        // caso contrario se utiliza uno por defecto
        public Logger()
        {
            fileName = ConfigurationManager.AppSettings["rutaCsvLog"];
        }

        public void EscribirLog(string logText)
        {
            try
            {
                using (StreamWriter w = File.AppendText(fileName))
                {
                    w.WriteLine(DateTime.Now.ToString() + " - " + logText);
                }
            }
            catch { }
        }

        public void EscribirLog(Exception ex)
        {
            try
            {
                using (StreamWriter w = File.AppendText(fileName))
                {
                    w.WriteLine("--------------------------------------------------------------------------------");
                    w.WriteLine(DateTime.Now.ToString() + " - EXCEPCION");
                    w.WriteLine("Message: " + ex.Message);
                    w.WriteLine("Source: " + ex.Source);
                    w.WriteLine("TargetSite: " + ex.TargetSite);
                    w.WriteLine("StackTrace: " + ex.StackTrace);
                    w.WriteLine("InnerException: " + ex.InnerException);
                    w.WriteLine("--------------------------------------------------------------------------------");
                }
            }
            catch { }
        }
    }
}
