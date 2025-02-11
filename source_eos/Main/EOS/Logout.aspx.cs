using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace EOS
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Clear();
            }
        }
    }
}