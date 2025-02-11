using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EOS.Account
{
    public partial class msg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            this.lit.Text = Request.QueryString["msg"];

        }
    }
}