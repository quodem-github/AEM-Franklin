using System.Configuration;

namespace msd.MailRecordatorio
{
    public class Variables
    {
        #region Constantes
        public const string MONITOR_DESCRIPTION = "Quodem.Monitor.Procesos.Descripcion";
        public static string MonitorDescription => Quodem.Utility.Configuration.GetKey(MONITOR_DESCRIPTION);

        public const string NOMBRE = "Quodem.Monitor.Procesos.Nombre";
        public static string Nombre => Quodem.Utility.Configuration.GetKey(NOMBRE);

        public const string ASUNTO = "Quodem.Monitor.Procesos.Asunto";
        public static string Asunto => Quodem.Utility.Configuration.GetKey(ASUNTO);

        public const string TIPO_LOG = "Quodem.Monitor.Procesos.TipoLog";
        public static string TipoLog => Quodem.Utility.Configuration.GetKey(TIPO_LOG);

        public const string ENVIO_LOG = "Quodem.Monitor.Procesos.EnvioLog";
        public static string EnvioLog => Quodem.Utility.Configuration.GetKey(ENVIO_LOG);

        public const string ENVIO_MAIL = "Quodem.Monitor.Procesos.EnvioMail";
        public static string EnvioMail => Quodem.Utility.Configuration.GetKey(ENVIO_MAIL);

        public const string NIVEL_MENSAJES = "Quodem.Monitor.Procesos.NivelMensajes";
        public static string NivelMensajes => Quodem.Utility.Configuration.GetKey(NIVEL_MENSAJES);
        
        public const string HOST_MAIL = "HostMail";
        public static string HostMail => Quodem.Utility.Configuration.GetKey(HOST_MAIL);

        public const string USERNAME_MAIL = "UserNameMail";
        public static string UserNameMail => Quodem.Utility.Configuration.GetKey(USERNAME_MAIL);

        public const string PORT_MAIL = "PortMail";
        public static string PortMail => Quodem.Utility.Configuration.GetKey(PORT_MAIL);

        public const string PASS_MAIL = "PasMail";
        public static string PasMail => Quodem.Utility.Configuration.GetKey(PASS_MAIL);

        public const string FROM_MAIL = "FromEmail";
        public static string FromEmail => Quodem.Utility.Configuration.GetKey(FROM_MAIL);

        public const string RUTA_LINK_MAIL = "RutaLinkMail";
        public static string RutaLinkMail => Quodem.Utility.Configuration.GetKey(RUTA_LINK_MAIL);

        public const string HOSTMAIL_ALTERNATIVO = "HostMailAlternativo";
        public static string HostMailAlternativo => Quodem.Utility.Configuration.GetKey(HOSTMAIL_ALTERNATIVO);

        public const string USERNAME_MAIL_ALTERNATIVO = "UserNameMailAlternativo";
        public static string UserNameMailAlternativo => Quodem.Utility.Configuration.GetKey(USERNAME_MAIL_ALTERNATIVO);

        public const string PORTMAIL_ALTERNATIVO = "PortMailAlternativo";
        public static string PortMailAlternativo => Quodem.Utility.Configuration.GetKey(PORTMAIL_ALTERNATIVO);

        public const string PASSMAIL_ALTERNATIVO = "PasswordMailAlternativo";
        public static string PasswordMailAlternativo => Quodem.Utility.Configuration.GetKey(PASSMAIL_ALTERNATIVO);

        public const string RANGO_DIAS = "RangoDias";
        public static string RangoDias => Quodem.Utility.Configuration.GetKey(RANGO_DIAS);

        private const string EMAIL_LEGAL = "MailDeptLegal";
        public static string EmailLegal => ConfigurationManager.AppSettings[EMAIL_LEGAL];

        private const string EMAIL_MEDICO = "MailDeptMedico";
        public static string EmailMedico => ConfigurationManager.AppSettings[EMAIL_MEDICO];

        private const string DIAS_AVISO_DELEGACION = "DiasAvisoDelegacion";
        public static string DiasAvisoDelegacion => ConfigurationManager.AppSettings[DIAS_AVISO_DELEGACION];

        private const string MAILS_ENVIO_DELEGACION = "MailsEnvioDelegacion";
        public static string MailsEnvioDelegacion => ConfigurationManager.AppSettings[MAILS_ENVIO_DELEGACION];



        #endregion

        #region Enums
        public enum ResultCode
        {
            Ok = 200,
            InternalError = 500
        }
        #endregion
    }
}
