using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace EOS.Web
{
    public class Variables
    {
        private const string EMAIL_LEGAL = "MailDeptLegal";
        private const string EMAIL_MEDICO = "MailDeptMedico";
        private const string AMEC_DOC_PATH = "AMECUpload";

        public static string EmailLegal { get { return ConfigurationManager.AppSettings[EMAIL_LEGAL]; } }
        public static string EmailMedico { get { return ConfigurationManager.AppSettings[EMAIL_MEDICO]; } }
        public static string AMECDocPath { get { return ConfigurationManager.AppSettings[AMEC_DOC_PATH]; } }

    }
}
