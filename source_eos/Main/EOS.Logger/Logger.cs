using System;
using System.IO;
using System.Configuration;


namespace EOS.Logger
{
    public class Logger
    {

        #region enumeraciones

        private enum ThromboTipoMensaje
        {
            Error,
            Info,
            Advertencia
        }

        #endregion

        private static StreamWriter GetFicheroLog(string tipoLog)
        {
            StreamWriter sw;
            string path;

            path = getLogPath(null);

            if (!File.Exists(path))
            {
                sw = File.CreateText(path);
            }
            else
            {
                sw = File.AppendText(path);
            }

            return sw;

        }

        public static string getLogPath(string tipoLog)
        {
            string path;

            path = ConfigurationManager.AppSettings[ConstantesLogger.Path] + @"\" + ConstantesLogger.NombreFicheroLibrerias + ".txt";

            return path;

        }

        #region printMsg

        public static void PrintError(string clase, string metodo, string mensaje, Exception ex)
        {
            PrintMensaje(clase, metodo, mensaje, ex, ThromboTipoMensaje.Error);
        }

        public static void PrintInfo(string clase, string metodo, string mensaje)
        {
            PrintMensaje(clase, metodo, mensaje, new Exception(), ThromboTipoMensaje.Info);
        }

        public static void PrintAdvertencia(string clase, string metodo, string mensaje)
        {
            PrintMensaje(clase, metodo, mensaje, new Exception(), ThromboTipoMensaje.Advertencia);
        }

        private static void PrintMensaje(string clase, string metodo, string mensaje, Exception ex, ThromboTipoMensaje eTipoMensaje)
        {
            string strTituloMsg = string.Empty;
            string seprador = "----------------------------";

            switch (eTipoMensaje)
            {
                case ThromboTipoMensaje.Error:

                    strTituloMsg = "Se ha producido un error";

                    break;

                case ThromboTipoMensaje.Info:

                    strTituloMsg = "Mensaje Información";

                    break;

                case ThromboTipoMensaje.Advertencia:

                    strTituloMsg = "Mensaje de advertencia";

                    break;

            }

            using (StreamWriter sw = GetFicheroLog(null))
            {
                sw.WriteLine(Environment.NewLine + seprador + Environment.NewLine + seprador);

                sw.WriteLine(DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + Environment.NewLine + Environment.NewLine);
                sw.WriteLine(strTituloMsg + Environment.NewLine);
                sw.WriteLine("CLASE: " + clase + Environment.NewLine);
                sw.WriteLine("Metodo: " + metodo + Environment.NewLine);
                sw.WriteLine("Mensaje: " + mensaje + Environment.NewLine);

                if (ex == null) 
                    return;

                sw.WriteLine("Source: " + ex.Source + Environment.NewLine);
                sw.WriteLine("StackTrace: " + ex.StackTrace + Environment.NewLine);

                if (ex.GetType() == typeof (Exception)) 
                    return;

                if (eTipoMensaje == ThromboTipoMensaje.Error && ex.Message != String.Empty)
                {
                    sw.WriteLine("Exception meassage: " + ex.Message);
                }
            }
        }

        #endregion



    }
}
