using EOS.Web;
using Quodem.Msd.SAML.Integrator;
using System.Web;
using System.Web.Security;

namespace EOS.Account
{
    public class CustomMSDLogin
    {
        private WebSiteProfileManager tokVal = null;

        public void SignIn()
        {
            this.tokVal.SignIn();
        }

        public void start()
        {
        }

        public CustomMSDLogin()
        {
            this.tokVal = new WebSiteProfileManager();

            dynamic usermsd = tokVal.GetUserProfile();

            if (usermsd != null)
            {
                string eml = GeteMail(usermsd);

                autolog("quodem");
            }
        }

        private string GeteMail(dynamic usermsd)
        {
            foreach (var item in usermsd)
            {
                if (item.Type != "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier") { continue; }

                var email = item.Value; if (email == "jbarcoc@quodem.com") { email = "jihernandez@quodem.com"; }

                return email;
            }

            return null;
        }

        private void autolog(string userloging)
        {
            AgenteUsuarios agente = new AgenteUsuarios();

            Entidades.Datos.DVPeticionariosRoles rolesUser = agente.ObtenerDatosRolesPorLogin(userloging);
            HttpContext.Current.Session["rolesUser"] = rolesUser;

            HttpContext.Current.Session.Remove("usuarioDesactivado");

            if (rolesUser.Inactivo)
            {
                HttpContext.Current.Session["usuarioDesactivado"] = true;
                HttpContext.Current.Response.Redirect("~/Account/Login.aspx");
            }
            else if (agente.EsPrimerLogin(userloging))
            {
                HttpContext.Current.Response.Cookies.Add(FormsAuthentication.GetAuthCookie(userloging, true));
                HttpContext.Current.Response.Redirect("~/CambiarDatosPersonales.aspx");
            }

            else if (rolesUser.aprobador.HasValue && rolesUser.aprobador.Value)
            {
                HttpContext.Current.Response.Redirect("~/ListadoAMECs.aspx");
            }
            else
            {
                FormsAuthentication.RedirectFromLoginPage(userloging, true);
            }
        }

    }
}