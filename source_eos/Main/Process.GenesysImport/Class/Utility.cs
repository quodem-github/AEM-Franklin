using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace GenesysImportProcess.Class
{
    public class Utility
    {
        public static string WriteLog(string textToWrite)
        {
            try
            {
                var fileName = string.Format("{0}Log.txt", AppConfigs.FileLogDirectory); 

                if (!Directory.Exists(AppConfigs.FileLogDirectory))
                {
                    Directory.CreateDirectory(AppConfigs.FileLogDirectory);
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
