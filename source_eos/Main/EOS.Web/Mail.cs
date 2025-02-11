using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Configuration;


namespace EOS.Web
{
    public class Mail
    {
        //public void Send(string to, Exception ex)
        //{
        //    Send(to, ex, null);
        //}

        public void Send(string to, Exception ex)
        {
            MailMessage mail = new MailMessage();
            string[] destinatarios = to.Split(';');

            foreach (string t in destinatarios.Where(t => !string.IsNullOrEmpty(t)))
            {
                mail.To.Add(new MailAddress(t));
            }

            if (mail.To.Count <= 0) 
                return;

            try
            {
                mail.Subject = "Quodem.Monitor - eosmsd.com - " + ConfigurationManager.AppSettings["runType"];
            }
            catch
            {
                mail.Subject = "Quodem.Monitor - eosmsd.com";
            }

            mail.Body = String.Empty;

            mail.Body += "MENSAJE DE ERROR \n  " +
                        ex.Message + "\n" +
                        ex.InnerException + "\n" +
                        ex.StackTrace + "\n" +
                        ex.Source + "\n" +
                        ex.TargetSite + "\n";

            mail.Body += "IP Origen: " + System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + " (url - " + System.Web.HttpContext.Current.Request.Url.ToString() + ")\n";

            EnviaMail(mail);
        }

        public void EnvioReservaMail(string info, string idexpediente, string idempleadogp, bool Enviar, string mailCopia, int idconfempresa)
        {
            AgenteMaestros agMaestro = new AgenteMaestros();
            IEnumerable<Entidades.Datos.DVEmpresa> lEmpresas = agMaestro.ObtenerEmpresas(null);
            string destinatario = "";

            if (!string.IsNullOrEmpty(idempleadogp))
            {
                string mailEmpleadogt = agMaestro.ObtenerMailEmpleadoGt(idempleadogp, idconfempresa);

                if (string.IsNullOrEmpty(mailEmpleadogt))
                {
                    if (lEmpresas != null)
                    {
                        Entidades.Datos.DVEmpresa miEmpresa = lEmpresas.FirstOrDefault();
                        destinatario = (miEmpresa != null) ? miEmpresa.MailContacto : string.Empty;
                    }
                }
                else
                {
                    destinatario = mailEmpleadogt;
                }
            }
            else
            {
                if (lEmpresas != null)
                {
                    Entidades.Datos.DVEmpresa miEmpresa = lEmpresas.FirstOrDefault();
                    destinatario = (miEmpresa != null) ? miEmpresa.MailContacto : string.Empty;
                }
            }

            MailMessage mail = new MailMessage();

            if (destinatario.Contains('@'))           
                mail.To.Add(new MailAddress(destinatario));
            if (mailCopia.Contains('@'))
            {
                foreach (string s in mailCopia.Split(';'))
                {
                    mail.To.Add(new MailAddress(s));
                }
            }
           

            if (Enviar)
            {
                mail.Subject = "Envío de Reserva EOS para el expediente: " + idexpediente;
            }
            else
            {
                mail.Subject = "Aprobada la Reserva EOS para el expediente: " + idexpediente;
            }
            mail.Body = info;

            EnviaMail(mail);

        }


        // Funcion que se usará SIEMPRE para realizar el envío de mails y en caso de error de server buscar alternativa Jose Laguna 29-03-2012 13:33 
        public static void EnviaMail(MailMessage message, bool printError = true)
        {
            SmtpClient smtpSender = new SmtpClient();
            SmtpClient smtpSenderAlternativo = new SmtpClient(ConfigurationManager.AppSettings["HostMail"], int.Parse(ConfigurationManager.AppSettings["PortMail"]))
                                                   {
                                                       EnableSsl = false,
                                                       Credentials =
                                                           new System.Net.NetworkCredential(
                                                           ConfigurationManager.AppSettings["UserNameMail"],
                                                           ConfigurationManager.AppSettings["PasswordMail"])
                                                   };

            try
            {
                smtpSender.Send(message);
            }
            catch (Exception ex)
            {
                smtpSenderAlternativo.Send(message);
                if (printError)
                {
                    Logger.Logger.PrintError("Mail", "amecNotificaciones", ex.Message, ex);
                }
            }
        }
    }
}
