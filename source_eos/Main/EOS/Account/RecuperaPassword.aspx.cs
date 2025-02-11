using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Web;

namespace EOS.Account
{
    public partial class RecuperaPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //FGI -> Se elimina la línea para para que el Remitente del mensaje sea el que viene por defecto
            //      en la clave del web.config "<mailSettings>"
            //ctrlPasswordRecovery.MailDefinition.From = ConfigUtil.GetAppSetting("ContactoError");
        }

        protected void ctrlPasswordRecovery_SendingMail(object sender, MailMessageEventArgs e)
        {
            e.Message.Body = e.Message.Body.Replace(EOS.Logica.Variables.MAIL_HOST_VARIABLE, HttpContext.Current.Request.Url.Host);
                 
            //aqui nunca entra, está mandando el email por otro método
            /*
            if (false) {
                try {
                    System.Net.Mail.SmtpClient smtpSender = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                    smtpSender.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtpSender.Credentials = new System.Net.NetworkCredential("demogrupopacifico@gmail.com", "demogp2010");
                    smtpSender.EnableSsl = true;
                    smtpSender.Timeout = 20;
                    e.Message.From = new System.Net.Mail.MailAddress("demogrupopacifico@gmail.com", "Demo Grupo Pacífico");
                    smtpSender.Send(e.Message);
                }
                catch (Exception ex)
                {
                    //Ismael Ameller 09-03-2011 Envio de Mail
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                    //FIN Ismael Ameller 09-03-2011 Envio de Mail
                    int k = 0;
                }
                finally
                {
                    e.Cancel = true;
                }

            }
            */
        }
    }
}