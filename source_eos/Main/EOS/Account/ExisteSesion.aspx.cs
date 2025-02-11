using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using EOS.Web;
using System.Diagnostics;

namespace EOS.Account
{
    public partial class ExisteSesion : Page
    {
        protected void SalirButton_OnClick(object sender, EventArgs e)
        {
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}
