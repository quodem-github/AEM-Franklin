using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace EOS.Account
{
    public partial class CambiarPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            if (NuevaPassword.Text != RepetirPassword.Text) 
                return;

            MembershipUser usuario = Membership.GetUser(UserName.Text);

            if (usuario != null && usuario.ChangePassword(Password.Text, NuevaPassword.Text))
            {
                Web.Alert.Show("La contraseña se ha cambiado correctamente", "Login.aspx");
            }
            else
            {
                Web.Alert.Show("Se ha producido un error al cambiar la contraseña. Inténtelo de nuevo", null);
            }
        }
    }
}