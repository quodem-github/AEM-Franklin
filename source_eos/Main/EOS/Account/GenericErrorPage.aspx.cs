using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using EOS.Logica;
using EOS.Web;
using System.Diagnostics;

namespace EOS.Account
{
    public partial class GenericErrorPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                try {
                    if (Server != null && Server.GetLastError() != null) {
                        Exception ex = Server.GetLastError();
                        if (ex.InnerException != null)
                            lbmensaje.Text = ex.InnerException.Message;
                        else
                            lbmensaje.Text = ex.Message;
                    } else if (Session["LastError"] != null) {
                        if (Session["LastError"] != null) {
                            Exception ex = (Exception)Session["LastError"];
                            if (ex.InnerException != null)
                                lbmensaje.Text = ex.InnerException.Message;
                            else
                                lbmensaje.Text = ex.Message;
                        }
                    //} else {
                    //    lbmensaje.Text = Request.QueryString["errormes"].ToString();
                    }
                } catch (Exception ex) {
                    //Ismael Ameller 09-03-2011 Envio de Mail
                    Mail mail = new Mail();
                    mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                    //FIN Ismael Ameller 09-03-2011 Envio de Mail
                    try {
                        //TODO revisar esta zona, esta eliminada a causa de un posible error
                        EventLog.WriteEntry(this.Title, ex.Message.ToString(), EventLogEntryType.Error);
                    } catch {
                        //do nothing, no rights to write to eventlog, here you would need some kind of logging...
                    }
                }
                //don´t show a message
                lbmensaje.Text = string.Empty;
            }
        }

        protected void SalirButton_OnClick(object sender, EventArgs e)
        {
            try {
                if (HttpContext.Current.Session != null) {
                    System.Web.Security.FormsAuthentication.SignOut();
                    Session.Clear();
                    Session.Abandon();
                }
            }
            catch (Exception ex) {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                try {
                    //TODO revisar esta zona, esta eliminada a causa de un posible error
                    EventLog.WriteEntry(this.Title, ex.Message.ToString(), EventLogEntryType.Error);
                } catch {
                    //do nothing, no rights to write to eventlog, here you would need some kind of logging...
                }
            }
            finally {
                FormsAuthentication.RedirectToLoginPage();
                //Server.Transfer("/Account/Login.aspx", false);
            }
        }

        protected void btn_Volver_Click(object sender, EventArgs e)
        {
            Response.Redirect(EOS.Logica.Variables.URL_HOME);
        }
    }
}
