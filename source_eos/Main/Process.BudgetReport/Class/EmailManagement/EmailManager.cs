using System;
using Quodem.Correo;
using Quodem.Monitor.Procesos;

namespace Process.BudgetReport.Class.EmailManagement
{
    public class EmailManager
    {
        #region Constructor
        public EmailManager(Process process, Resultado result)
        {
            Result = result;
            Process = process;
        }
        
        #endregion

        #region Public Properties
        /// <summary>
        /// Reference of the process
        /// </summary>
        public Process Process { get; set; }

        /// <summary>
        /// Reference of the result of the process.
        /// </summary>
        public Resultado Result { get; set; } 
        #endregion

        #region Public Methods

        /// <summary>
        /// Send Email Method
        /// </summary>
        /// <param name="attachedFile">Path of attached file</param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns>Return true if the email is sent correctly</returns>
        public bool SendEmail(string attachedFile, DateTime startDateTime, DateTime endDateTime)
        {
            var result = true;
            try
            {
                var envioMail = new EnvioMail();
                var startdateFormatted = startDateTime.ToString("dd MMMM yyyy");
                var endDateFormatted = endDateTime.ToString("dd MMMM yyyy");

                envioMail.AddArchivoAdjunto(attachedFile);
                envioMail.Subject = string.Format("{0} - Periodo: {1} - {2}", AppConfigs.EmailSubject, startdateFormatted, endDateFormatted);
                envioMail.From = AppConfigs.EmailFrom;
                envioMail.To = AppConfigs.EmailList;
                envioMail.BodyFormat = BodyFormat.TEXT;
                envioMail.Body = string.Format("Periodo: {0} - {1}\r\n{2}", startdateFormatted, endDateFormatted, AppConfigs.EmailBodyTemplate);

                envioMail.Send();
            }
            catch (Exception ex)
            {
                Utility.WriteLog(string.Format("Error: EmailManager->SendEmail: {0}", ex.Message));
                Process.WriteLineApp(string.Format("Error: El email no se ha enviado correctamente: {0}", ex.Message), 2);
                Result.Errores++;
                result = false;
            }

            return result;
        } 
        #endregion
    }
}
