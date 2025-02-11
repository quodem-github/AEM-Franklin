using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Web;
using EOS.Entidades.Datos;

namespace EOS
{
    public partial class CambiarDatosCompliance : System.Web.UI.Page
    {
        public int nBack = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                nBack = -1;
                ViewState.Add("ComplianceHistoryBack", nBack);

                AgenteMaestros agente = new AgenteMaestros();
                DEmpresaConf datos = agente.ObtenerEmpresaConf(1);

                this.MailFrom.Text = datos.mailfromcomunicfi;
                this.MailTo.Text = datos.mailtocomunicfi;
                this.MailCC.Text = datos.mailcccomunicfi;
                this.MailToRspta.Text = datos.mailtorespuesta;
                this.Formato.Text = datos.formatoprgmailcomunicfi;
                this.Asunto.Text = datos.asuntomailcomunicfi;
                this.Cuerpo.Text = datos.cuerpomailcomunicfi;
            }
            else
            {
                nBack = (int)ViewState["ComplianceHistoryBack"] - 1;
            }

            ViewState["ComplianceHistoryBack"] = nBack;
            this.CancelarImageButton.OnClientClick = GetBackString(true); 
        }

        protected string GetBackString(bool bAddFalse)
        {
            int nBackActual = -1;
            if (ViewState["ComplianceHistoryBack"] != null)
            {
                nBackActual = (int)ViewState["ComplianceHistoryBack"];
            }
            return string.Format("history.go({0});{1}", nBackActual, bAddFalse ? "return false;":""); 
        }

        protected void AceptarImageButton_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                AgenteMaestros agente = new AgenteMaestros();
                DEmpresaConf datos = agente.ObtenerEmpresaConf(1);

                datos.mailfromcomunicfi = this.MailFrom.Text;
                datos.mailtocomunicfi = this.MailTo.Text;                
                datos.mailcccomunicfi = this.MailCC.Text;
                datos.mailtorespuesta = this.MailToRspta.Text;
                datos.formatoprgmailcomunicfi = this.Formato.Text;
                datos.asuntomailcomunicfi = this.Asunto.Text;
                datos.cuerpomailcomunicfi = this.Cuerpo.Text;
                
                if (agente.ActualizarDatosCompliance(datos))
                {
                    Alert.ShowBack("Los datos se han actualizado correctamente", GetBackString(false));
                }
                else
                {
                    Alert.Show("Alguno de los datos introducidos no es correcto. Por favor revise la información introducida.",null);
                }
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Alert.Show("Se ha producido un error actualizando sus datos. Por favor revise la información introducida.", null);
            }
        }

    }
}