using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace EOS
{
    public class Constantes
    {
        private const string LOG_PATH = "LogPath";
        private const string LOG_FILE = "LogFile";

        public static string LogPath
        {
            get { return ConfigurationManager.AppSettings[LOG_PATH]; }
        }
        public static string LogFile
        {
            get { return ConfigurationManager.AppSettings[LOG_FILE]; }
        }
        public class identificadoresBD
        {

            public const int poblaciones_otros = 1;

        }

        public class Log
        {
            public const string NombreFichero = "LogEOS";
            public const string Path = "Logs";

        }

        public class Session
        {
            public const string LogPath = "LogPath";
            public const string idReserva = "idreserva";
            public const string oFiltroExpedientes = "oFiltroExpedientes";
            public const string oFiltroExpedientesDoc = "oFiltroExpedientesDoc";

        }

        public class AppParams
        {
            public const string MailContactoError = "ContactoError";
            public const string ActivarLog = "ActivarLog";
            public const string CopiaContacto = "MailCopia";
            public const string MailCCHcp = "MailCCHCP";

        }

        public class Application
        {
            public const string LogPath = "LogPath";
        }

        public static string[] NAMESIMPORTES = new string[] { "ImporteExpediente", "Importe Alojamiento", "Importe Inscripciones", "Importe Otros servicios", "Importe Desplazamiento", "Importe Reserva" };

    }
}