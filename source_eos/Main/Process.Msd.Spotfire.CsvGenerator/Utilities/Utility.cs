using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Process.Msd.Spotfire.CsvGenerator.Utilities
{
    public class Utility
    {

        public static string CreateDirectory(string pathBase)
        {
            CultureInfo ci = new CultureInfo("en-US");
            string destDirectory = pathBase + DateTime.Now.ToString("MMMM", ci).Substring(0, 3).FirstCharToUpper() + " " + DateTime.Now.Year + "/";
            DirectoryInfo destino = new DirectoryInfo(destDirectory);
            if (!destino.Exists)
            {
                destino.Create();
                WriteLog("Directorio creado: " + destDirectory);
                
            }
            return destDirectory;
        }

        public static string WriteLog(string textToWrite)
        {
            try
            {
                var fileName = string.Format("{0}Log.txt", AppConfig.FileLogDirectory);

                if (!Directory.Exists(AppConfig.FileLogDirectory))
                {
                    Directory.CreateDirectory(AppConfig.FileLogDirectory);
                }

                var write = false;

                while (!write)
                {
                    try
                    {
                        var writer = new StreamWriter(fileName, true);
                        writer.WriteLine(textToWrite);
                        writer.Close();
                        write = true;
                    }
                    catch (Exception)
                    {
                    }
                }
                return fileName;
            }
            catch (Exception ex)
            {
                Utility.WriteLog("Error no controlado en WriteLog: " + ex.Message);
                throw ex;
            }
        }
    }
}