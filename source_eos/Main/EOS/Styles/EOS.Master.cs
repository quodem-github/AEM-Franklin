using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Web;
using EOS.Entidades.Datos;
using EOS.Web.Controls;
using Quodem.Msd.SAML.Integrator;

namespace EOS
{
    public partial class Site1 : MasterPage
    {
        private bool isOldVersion = false;
        public bool isAdministrator = false;
        public bool isMedico = false;
        public bool isLegal = false;
        public bool isAssistant = false;
        public bool isBudMdAdmin = false;
        public bool sePuedeMostrarLogo = true;
        public bool sePuedeMostrarNuevoAmec { get; set; }
        
        public bool sePuedeMostrarListadoAmec = true;
        public bool estamosEnInformes = false;
        //Borrar
        String x;

        public bool IsOldVersion
        {
            get { return isOldVersion; }
            set { isOldVersion = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //IE 6 check enabled, first test whether we deal with an internet explorer 6 browser
            try
            {
                double version = Convert.ToDouble(Request.Browser.Version);
                if (Request.Browser.Browser.ToUpper() == "IE" && version < 70.0)
                    isOldVersion = true;
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                //some problem with converting from string to integer, just test only for IE6
                if (Request.Browser.Browser.ToUpper() == "IE" && Request.Browser.Version == "6.0")
                    isOldVersion = true;
            }
            if (isOldVersion)
                //now check all controls on this page
                checkControls(this.Page.Controls);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AgenteMaestros agMaestro = new AgenteMaestros();
                IEnumerable<EOS.Entidades.Datos.DVEmpresa> lEmpresas = agMaestro.ObtenerEmpresas(null);
                //if (lEmpresas != null)
                //{
                //    rptAgencias.DataSource = lEmpresas;
                //    rptAgencias.DataBind();
                //}
                // Evaluar las expresiones <%# %> (databinding expresions) que existen en el header.
                Page.Header.DataBind();

                if (Session["logoCabecera"] == null)
                {
                    if (lEmpresas != null)
                    {
                        EOS.Entidades.Datos.DVEmpresa miEmpresa = lEmpresas.FirstOrDefault();
                        if (miEmpresa != null)
                        {
                            Session.Add("logoCabecera", "~/Styles/images/" + "header.png");
                            Session.Add("logoSuperiorPantalla", "~/Styles/images/" + miEmpresa.LogoSuperiorPantalla);
                            Session.Add("logoInferiorPantalla", "~/Styles/images/" + miEmpresa.LogoInferiorPantalla);
                        }
                    }
                }
                //this.eosImgHeader.ImageUrl = (string)Session["logoCabecera"];
                //if (this.imgLogo != null) this.imgLogo.ImageUrl = (string)Session["logoSuperiorPantalla"];
                //if (this.imgLogoBottom != null) this.imgLogoBottom.ImageUrl = (string)Session["logoInferiorPantalla"];

                // Si no estamos en una pantalla como la de Login en la que no hay parte de cabecera.
                AgenteUsuarios agenteUsu = new AgenteUsuarios();
                DDatosPersonalesUsuario datosUsuario = (DDatosPersonalesUsuario)Session["datosUsuario"];
                if (datosUsuario == null)
                {
                    datosUsuario = agenteUsu.ObtenerDatosPersonalesPorLogin();
                    Session["datosUsuario"] = datosUsuario;
                }


                if (datosUsuario != null)
                {
                    ICollection<DEmpleadosGP> datosEmpleados = null;
                    if (HttpContext.Current.Session["DatosEmpleados"] != null)
                    {
                        datosEmpleados = HttpContext.Current.Session["DatosEmpleados"] as List<DEmpleadosGP>;
                    }
                    if (datosEmpleados == null)
                    {
                        AgenteEmpleados agenteEmp = new AgenteEmpleados();
                        datosEmpleados = agenteEmp.ObtenerDatosEmpleadosPorID(datosUsuario.IdPeticionario);
                        HttpContext.Current.Session["DatosEmpleados"] = datosEmpleados;
                    }

                    rptEmpleadosGP.DataSource = datosEmpleados;
                    rptEmpleadosGP.DataBind();

                    if (((DVPeticionariosRoles)Session["rolesuser"]) != null)
                    {
                        sePuedeMostrarNuevoAmec = (((DVPeticionariosRoles)Session["rolesuser"]).newco.HasValue && ((DVPeticionariosRoles)Session["rolesuser"]).newco.Value) || (((DVPeticionariosRoles)Session["rolesuser"]).administrador.HasValue && ((DVPeticionariosRoles)Session["rolesuser"]).administrador.Value);
                    }
                    isMedico = datosUsuario.medico.Value;
                    isLegal = datosUsuario.legal.Value;
                    isAssistant = datosUsuario.assistant.Value;
                    if (datosUsuario.admin == true) { isAdministrator = true; isBudMdAdmin = false; }
                    if (datosUsuario.IdCargo == 561 || datosUsuario.IdCargo == 534) isBudMdAdmin = false;
                    //if (Request.AppRelativeCurrentExecutionFilePath.Contains("ListadoAMECs.aspx") || Request.AppRelativeCurrentExecutionFilePath.Contains("DetalleAMEC.aspx") || Request.AppRelativeCurrentExecutionFilePath.Contains("Delegacion.aspx")) imgLogo.Visible = false;

                    if (Page.Request.Url.AbsolutePath.Contains("DetalleAMEC.aspx") || Page.Request.Url.AbsolutePath.Contains("ListadoAMECs.aspx") || Page.Request.Url.AbsolutePath.Contains("EditDelegacion.aspx") || Page.Request.Url.AbsolutePath.Contains("EditFlujoAprobacion.aspx") || Page.Request.Url.AbsolutePath.Contains("Delegacion.aspx") || Page.Request.Url.AbsolutePath.Contains("FlujoAprobacion.aspx")) sePuedeMostrarLogo = false;
                    if (Page.Request.Url.AbsolutePath.Contains("CambiarDatosPersonales.aspx") || Page.Request.Url.AbsolutePath.Contains("CambiarDatosCompliance.aspx") || Page.Request.Url.AbsolutePath.Contains("NuevoExpedientePasoA.aspx") || System.Configuration.ConfigurationManager.AppSettings["controlPresupuesto"] == "0") { sePuedeMostrarNuevoAmec = false; }
                    if (Page.Request.Url.AbsolutePath.Contains("FiltroInforme.aspx")) estamosEnInformes = true;
                    if (System.Configuration.ConfigurationManager.AppSettings["controlPresupuesto"] == "0") { sePuedeMostrarListadoAmec = false; }

                }

                //  }

                if (Page.IsPostBack)
                    return;

                html.Attributes["class"] = System.Configuration.ConfigurationManager.AppSettings["htmlClass"];
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
            }
        }

        private void checkControls(ControlCollection ctrls)
        {
            //this is browser 6.0, now check disabled, does not work with advanced style sheets.
            foreach (Control ctl in ctrls)
            {
                if (ctl is InputDatePickerControl)
                {
                    //we are now going to test whether the control is disabled
                    InputDatePickerControl idp = (InputDatePickerControl)ctl;
                    //has subcontrols
                    if (idp.Enabled == false)
                    {
                        idp.Enabled = true;
                        idp.InputTextBox.Enabled = false;
                        idp.ClickImageButton.Enabled = false;
                    }
                }
                if (ctl is TimeInputBox)
                {
                    //we are now going to test whether the control is disabled
                    TimeInputBox tib = (TimeInputBox)ctl;
                    //has subcontrols
                    if (tib.Enabled == false)
                    {
                        tib.Enabled = true;
                        tib.InputTextBox.Enabled = false;
                    }
                }
                if (ctl is TextBox)
                {
                    //we are now going to test whether the control is disabled
                    TextBox txtbox = (TextBox)ctl;

                    string id = txtbox.ID;
                    string client = txtbox.ClientID;
                    if (txtbox.Enabled && !txtbox.ReadOnly)
                    {
                        //if (txtbox.CssClass.Contains("eosDisabledInputVacioIE6"))
                        //    txtbox.CssClass.Replace("eosDisabledInputVacioIE6", "");
                        //if (txtbox.CssClass.Contains("eosDisabledInputVacio"))
                        //    txtbox.CssClass.Replace("eosDisabledInputVacio", "");
                        txtbox.CssClass = "";
                    }
                    else if (!txtbox.Enabled || txtbox.ReadOnly)
                    {
                        //not enabled, so change the style
                        if (string.IsNullOrEmpty(txtbox.CssClass))
                            txtbox.CssClass = "eosDisabledInputVacioIE6";
                        else
                            if (!txtbox.CssClass.Contains("eosDisabledInputVacioIE6"))
                            txtbox.CssClass += " eosDisabledInputVacioIE6";
                    }
                }
                if (ctl is DropDownList)
                {
                    //we are now going to test whether the control is disabled
                    DropDownList ddl = (DropDownList)ctl;
                    if (ddl.Enabled)
                    {
                        if (ddl.CssClass.Contains("eosDisabledInputVacioIE6"))
                            ddl.CssClass.Replace("eosDisabledInputVacioIE6", string.Empty);
                    }
                    else
                    {
                        //not enabled, so change the style
                        if (string.IsNullOrEmpty(ddl.CssClass))
                            ddl.CssClass = "eosDisabledInputVacioIE6";
                        else
                            if (!ddl.CssClass.Contains("eosDisabledInputVacioIE6"))
                            ddl.CssClass += " eosDisabledInputVacioIE6";
                    }
                }
                if (ctl.Controls != null && ctl.Controls.Count > 0)
                    checkControls(ctl.Controls);
            }
        }

        protected void logout(Object sender, System.EventArgs e)
        {
            WebSiteProfileManager tokVal = new WebSiteProfileManager();

            //comprobación de si es un usuario MSD pendiente de programar. A la espera de la actualización de la base de datos y modelo

            EOS.Entidades.Datos.DVPeticionariosRoles datosUsuarioAMEC = (DVPeticionariosRoles)Session["rolesuser"];
            
            if (datosUsuarioAMEC.externalUser != null && datosUsuarioAMEC.externalUser.Value)
            {
                Session["rolesuser"] = null;
                tokVal.SignOut("~/Account/Login.aspx");
            }
            else
            {
                Session["rolesuser"] = null;
                tokVal.SignOut("~/Default.aspx");
            }
        }
    }
}
