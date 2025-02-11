using System;
using System.Web.UI;

namespace EOS.Styles
{
    public partial class EOSError : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                html.Attributes["class"] = System.Configuration.ConfigurationManager.AppSettings["htmlClass"];
                eosImgHeader.CssClass = string.Format("{0} imgHeader", System.Configuration.ConfigurationManager.AppSettings["htmlClass"]);
            }
        }
    }
}