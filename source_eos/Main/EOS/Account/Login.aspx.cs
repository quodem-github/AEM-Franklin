using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using EOS.Logica;
using EOS.Web;
using Quodem.Msd.SAML.Integrator;

namespace EOS.Account
{
    public partial class Login : Page
    {
        private WebSiteProfileManager tokVal = null;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var qUserOption = Request["agency"] != null ? Request["agency"].Trim().ToLower() : string.Empty;
            if (qUserOption != "GP".Trim().ToLower() && qUserOption != "Amex".Trim().ToLower() && qUserOption != "quodem".Trim().ToLower())
            {
                plhMsdLogin.Visible = false;
                
                tokVal = new WebSiteProfileManager();

                dynamic usermsd = tokVal.GetUserProfile();

                if (usermsd != null)
                {
                    LogicaUsuarios instance = new LogicaUsuarios();

                    string login = instance.ObtenerElLoginMedianteEmail(GeteMail(usermsd));

                    instance = null;

                    if (login.Trim() == ""){ Response.Redirect("/public/AccessError.aspx"); }

                    autolog(login);
                }else
                {
                    tokVal.SignIn();
                }
            }
            else
            {
                plhAgencyLogin.Visible = true;
                if (!IsPostBack)
                {

                    if ((Session["rolesuser"] != null && Request.UrlReferrer != null) || Session["rolesuser"] == null)
                    {
                        if (Session["usuarioDesactivado"] == null)
                        {
                            // Elimina la cookie y borra los datos de sesión            
                            FormsAuthentication.SignOut();
                            Session.Clear();
                            Session.Abandon();
                        }

                        // clear authentication cookie
                        HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, string.Empty);
                        cookie1.Expires = DateTime.Now.AddYears(-1);
                        Response.Cookies.Add(cookie1);

                        // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
                        HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
                        cookie2.Expires = DateTime.Now.AddYears(-1);
                        Response.Cookies.Add(cookie2);
                    }
                }

                if (Session["usuarioDesactivado"] != null)
                {
                    Alert.Show("Usuario desactivado");
                }
                else if (Session["rolesuser"] != null && Request.UrlReferrer == null)
                {
                    if (Request.QueryString.ToString().Contains("DownloadProgramDoc.aspx"))
                    {
                        string[] split1 = Request.QueryString.ToString().Split('=');

                        string idamecs = split1[3].Split('&')[0];
                        string file = split1[4];

                        Response.Redirect("~/DownloadProgramDoc.aspx?Tipodoc=Documentacion" + "&idamec=" + idamecs +
                                          "&file=" +
                                          file);
                    }
                    else
                    {
                        if (qUserOption != "GP".Trim().ToLower() && qUserOption != "Amex".Trim().ToLower() && qUserOption != "quodem".Trim().ToLower())
                        {
                            Response.Redirect("~/Expedientes.aspx");
                        }
                    }

                }
            }
        }

    protected void LoginUser_LoggedIn(object sender, EventArgs e)
        {
            if (Session["rolesuser"] != null)
            {
                Response.Redirect("~/Account/ExisteSesion.aspx");
            }
            else
            {
                if (Membership.ValidateUser(LoginUser.UserName, LoginUser.Password))
                {
                    AgenteUsuarios agente = new AgenteUsuarios();
                    //Xavier Morell: En vez de obtener cada vez, se guarda en sesión para ahorrar accesos a la BD
                    //EOS.Entidades.Datos.DVPeticionariosRoles rolesUser = (EOS.Entidades.Datos.DVPeticionariosRoles)Session["rolesuser"];

                    Entidades.Datos.DVPeticionariosRoles rolesUser = agente.ObtenerDatosRolesPorLogin(LoginUser.UserName);
                    Session["rolesUser"] = rolesUser;

                    Session.Remove("usuarioDesactivado");
                    //Xavier Morell: Si el usuario está inactivo no se puede acceder con ese usuario ni gestionar sus datos.
                    if (rolesUser.Inactivo)
                    {
                        //Alert.Show("Usuario desactivado");
                        Session["usuarioDesactivado"] = true;
                        Response.Redirect("~/Account/Login.aspx");
                    }
                    else if (agente.EsPrimerLogin(LoginUser.UserName))
                    {
                        Response.Cookies.Add(FormsAuthentication.GetAuthCookie(LoginUser.UserName, LoginUser.RememberMeSet));
                        Response.Redirect("~/CambiarDatosPersonales.aspx");
                    }
                    //No redirigimos a la pantalla de AMEC
                    else if (rolesUser.aprobador.HasValue && rolesUser.aprobador.Value)
                    {
                        Response.Redirect("~/ListadoAMECs.aspx");
                    }
                    else
                    {
                        FormsAuthentication.RedirectFromLoginPage(LoginUser.UserName, LoginUser.RememberMeSet);
                    }
                }
            }
        }

        protected void golog(object sender, EventArgs e)
        {
            tokVal.SignIn();
        }

        private string GeteMail(dynamic usermsd)
        {
            foreach (var item in usermsd)
            {
                if (item.Type != "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier") { continue; }

                var email = item.Value;

                return email;
            }

            return null;
        }

        private void autolog(string userloging)
        {
            AgenteUsuarios agente = new AgenteUsuarios();

            Entidades.Datos.DVPeticionariosRoles rolesUser = agente.ObtenerDatosRolesPorLogin(userloging);
            Session["rolesUser"] = rolesUser;

            Session.Remove("usuarioDesactivado");
            if (rolesUser.Inactivo)
            {
                Session["usuarioDesactivado"] = true;
                Response.Redirect("~/Account/Login.aspx");
            }
            else if (agente.EsPrimerLogin(userloging))
            {
                Response.Cookies.Add(FormsAuthentication.GetAuthCookie(userloging, true));
                Response.Redirect("~/CambiarDatosPersonales.aspx");
            }
            else
            {
                FormsAuthentication.RedirectFromLoginPage(userloging, true);
            }
        }

    }

}
