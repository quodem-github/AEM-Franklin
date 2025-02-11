using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Configuration;


namespace EOS
{
    /// <summary>
    /// 
    /// </summary>
    public class EOSLogger
    {

        #region enumeraciones

        private enum ThromboTipoMensaje
        {
            Error,
            Info,
            Advertencia
        }

        #endregion

        

        private static System.IO.StreamWriter GetFicheroLog(string path)
        {
            StreamWriter sw;

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

        public static string getLogPath(string ServerPath)
        {
            string path;

            if (!(ServerPath.LastIndexOf(@"\") == (ServerPath.Length - 1)))
            {
                ServerPath = ServerPath + @"\";
            }

            path = ServerPath + Constantes.Log.Path  + @"\" + DateTime.Now.ToString("yyyy_MM_dd_HHmmss") + "_" + Constantes.Log.NombreFichero + ".txt";

            return path; 

        }

        public static void PrintError( string clase, string metodo, string mensaje, Exception ex, string nombreEmpleado)
        {

            PrintMensaje(clase, metodo, mensaje, ex, ThromboTipoMensaje.Error, nombreEmpleado);
        }


        public static void PrintError(string clase, string metodo, string mensaje, Exception ex)
        {
            
            PrintMensaje(clase, metodo, mensaje, ex, ThromboTipoMensaje.Error, string.Empty);
        }


        public static void PrintInfo( string clase, string metodo, string mensaje, string nombreEmpleado)
        {
            PrintMensaje( clase, metodo, mensaje, new Exception(), ThromboTipoMensaje.Info, nombreEmpleado);
        }


        public static void PrintInfo(string clase, string metodo, string mensaje)
        {
            PrintMensaje(clase, metodo, mensaje, new Exception(), ThromboTipoMensaje.Info, string.Empty);
        }

        public static void PrintAdvertencia(string clase, string metodo, string mensaje)
        {
            PrintMensaje(clase, metodo, mensaje, new Exception(), ThromboTipoMensaje.Advertencia, string.Empty);
        }



        private static void PrintMensaje(string clase, string metodo, string mensaje, Exception ex, ThromboTipoMensaje eTipoMensaje, string NombreEmpleado)
        {
            string strTituloMsg = string.Empty;
            string seprador = "----------------------------";
                
            if (ConfigurationManager.AppSettings.Get("ActivarLog") != "1")
            {
                return;
            }

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

                using (StreamWriter sw = GetFicheroLog(Constantes.LogPath+Constantes.LogFile))
                {
                    sw.WriteLine(Environment.NewLine + seprador + Environment.NewLine + seprador);

                    sw.WriteLine(DateTime.Now.ToLongTimeString() + " " + DateTime.Now.ToLongDateString() + Environment.NewLine + Environment.NewLine);
                    sw.WriteLine(strTituloMsg + Environment.NewLine);
                    sw.WriteLine("Empleado: " + NombreEmpleado + Environment.NewLine);
                    sw.WriteLine("CLASE: " + clase + Environment.NewLine);
                    sw.WriteLine("Metodo: " + metodo + Environment.NewLine);
                    sw.WriteLine("Mensaje: " + mensaje + Environment.NewLine);


                    if (ex.GetType() != typeof(Exception))
                    {
                        if (eTipoMensaje == ThromboTipoMensaje.Error && ex.Message != String.Empty)
                        {
                            sw.WriteLine("Exception meassage: " + ex.Message);
                        }
                    }

                }

            

        }


    }


}