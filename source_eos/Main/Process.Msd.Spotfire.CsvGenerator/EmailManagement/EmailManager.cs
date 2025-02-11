using Process.Msd.Spotfire.CsvGenerator.Interfaces;
using Process.Msd.Spotfire.CsvGenerator.Utilities;
using Quodem.Correo;
using Quodem.Monitor.Procesos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Process.Msd.Spotfire.CsvGenerator.EmailManagement
{
    public class EmailManager
    {
        private IProcess _process { get; set; }
        private Resultado _result { get; set; }

        public EmailManager(IProcess process, Resultado result)
        {
            _process = process;
            _result = result;
        }

        #region Public Methods

        /// <summary>
        /// Send Email Method
        /// </summary>
        /// <param name="attachedFile">Path of attached file</param>
        /// <returns>Return true if the email is sent correctly</returns>
        public bool SendEmail(string subject, string body)
        {
            var result = true;
            try
            {
                var envioMail = new EnvioMail
                {
                    Subject = subject,
                    From = AppConfig.EmailFrom,
                    To = AppConfig.EmailList,
                    BodyFormat = BodyFormat.TEXT,
                    Body = body
                };

                envioMail.Send();
            }
            catch (Exception ex)
            {
                Utility.WriteLog(string.Format("Error: EmailManager->SendEmail: {0}", ex.Message));
                _process.WriteLineApp(string.Format("Error: El email no se ha enviado correctamente: {0}", ex.Message), 2);
                _result.Errores++;
                result = false;
            }

            return result;
        }

        #endregion Public Methods
    }
}