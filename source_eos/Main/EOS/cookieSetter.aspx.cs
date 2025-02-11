using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EOS
{
    public partial class cookieSetter : System.Web.UI.Page
    {

        
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Response.Write("cookiesetter ok"); //etq

            var jsonProf = String.Format("{0}", Request.Form["jsonProf"]);
            var redirectUrl = String.Format("{0}", Request.Form["redirectUrl"]);

            if (redirectUrl.Trim() != "" && jsonProf.Trim() != "")
            {
                HttpCookie myCookie = new HttpCookie("profile_SAML_MSD")
                {
                    Value = jsonProf,
                    Expires = DateTime.Now.AddDays(1d)
                };

                Response.Cookies.Add(myCookie);
                Response.Redirect(redirectUrl);
            }
            Response.Redirect("unauthorized-access.aspx");
        }
    }
}