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
    public partial class NuevaActividad : System.Web.UI.Page
    {
        public static string sURL;
        public static string idAmec;

        protected void Page_Load(object sender, EventArgs e)
        {

            ///////////////Controlar Si caduca la sesión en la aplicación//////////////
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Session["rolesuser"] == null) Response.Redirect("~/Account/Login.aspx");
            //////////////////////////////////////////////////////////////////////////


            if (!Page.IsPostBack)
            {
                SetBackString();
                //Xavier Morell
                //AgenteUsuarios agenteUsu = new AgenteUsuarios();
                //DDatosPersonalesUsuario datosUsuario = agenteUsu.ObtenerDatosPersonalesPorLogin();
                DDatosPersonalesUsuario datosUsuario = (DDatosPersonalesUsuario)Session["datosUsuario"];

                if (datosUsuario != null)
                {
                    this.txtCreadoPor.Text = String.Format("{0} {1}", datosUsuario.Nombre, datosUsuario.Apellido1);
                    this.txtFechaCreacion.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                Session["IdConfEmpresa"] = null;
                int IdConfEmpresa = 0;
                int.TryParse(Request.QueryString["idconf"], out IdConfEmpresa);
                if (IdConfEmpresa != 0)
                {
                    RellenarCombos(IdConfEmpresa);
                    Session["IdConfEmpresa"] = IdConfEmpresa;
                    hdnIdConfEmpresa.Value = IdConfEmpresa.ToString();
                }
                else
                {
                    RellenarCombos(datosUsuario.IdConfEmpresa ?? 1);
                    Session["IdConfEmpresa"] = datosUsuario.IdConfEmpresa ?? 1;
                    hdnIdConfEmpresa.Value = datosUsuario.IdConfEmpresa.ToString() ?? "1"; 

                }
            }
        }

        private void RellenarCombos(int idconfempresa)
        {
            AgenteMaestros agente = new AgenteMaestros();
            this.ddlEspecialidad.DataSource = agente.ObtenerTiposEspecialidades(null);
            this.ddlEspecialidad.DataBind();

            this.ddlPais.DataSource = agente.ObtenerPaisesTodos(null, idconfempresa);
            this.ddlPais.DataBind();

            //this.ddlPoblacion.DataSource = agente.ObtenerPoblacionesTodas(null, idconfempresa);
            //this.ddlPoblacion.DataBind();
            AgenteUsuarios agenteUsu = new AgenteUsuarios();
            DVPeticionariosRoles datosRoles = agenteUsu.ObtenerDatosRolesPorLogin();
            this.ddlTipoActividad.DataSource = agente.ObtenerTiposActividad(null, datosRoles.administrador, datosRoles.newco);
            this.ddlTipoActividad.DataBind();

            //DEQ: 20180702 Se permite temporalmente el crear actividades en fechas pasadas
            //DEQ: 20190212 Deja de permitirse el crear actividades en fechas pasadas
            this.RangeValidatorFechaInicio.MinimumValue = DateTime.Now.ToString("dd/MM/yyyy");
            this.RangeValidatorFechaFin.MinimumValue = DateTime.Now.ToString("dd/MM/yyyy");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try 
            {
                DateTime dtInicio = DateTime.Parse("01/01/0001");

                if (!DateTime.TryParse(txtFechaInicio.InputTextBox.Text, out dtInicio))
                    dtInicio = DateTime.Parse("01/01/0001");

                if (dtInicio < DateTime.Parse("02/01/0001")) {
                    Alert.Show("La fecha de inicio tiene que tener un valor", null);
                    return;
                }

                DateTime dtFin = DateTime.Parse("01/01/0001");

                if (!DateTime.TryParse(txtFechaFin.InputTextBox.Text, out dtFin))
                    dtFin = DateTime.Parse("01/01/0001");

                if (dtFin < DateTime.Parse("02/01/0001")) {
                    Alert.Show("La fecha de finalización tiene que tener un valor", null);
                    return;
                }

                if (dtInicio > dtFin)
                {
                    Alert.Show("La fecha de inicio no puede ser mayor que la fecha de finalización", null);
                    return;
                }

                DateTime dtCreacion = System.DateTime.Now;

                if (!DateTime.TryParse(txtFechaCreacion.Text, out dtCreacion))
                    dtCreacion = System.DateTime.Now;

                DDatosPersonalesUsuario datosUsuario = (DDatosPersonalesUsuario)Session["datosUsuario"];

                int nIDPeticionario = 0;

                if (datosUsuario != null)
                {
                    nIDPeticionario = datosUsuario.IdPeticionario;
                }

                AgenteExpedientes agente = new AgenteExpedientes();

                int idActividadNueva = agente.NuevaPeticionActividad(dtInicio, dtFin, dtCreacion, nIDPeticionario, int.Parse(selectedIdPoblacion.Value), txtNombre.Text.Replace("'", "''"), int.Parse(ddlTipoActividad.SelectedValue),
                    int.Parse(ddlEspecialidad.SelectedValue), false, chkInternacional.Checked, txtSede.Text.Replace("'", "''"), txtUrl.Text.Replace("'", "''"), txtEmail.Text.Replace("'", "''"), int.Parse(Session["IdConfEmpresa"].ToString()));

                Alert.Show(string.Format("Se ha creado una nueva actividad", idActividadNueva),
                           sURL == "NuevoDetalleAmec.aspx"
                               ? string.Format("NuevoDetalleAmec.aspx?idact={0}", idActividadNueva)
                               : string.Format("NuevoExpedientePasoA.aspx?tipo={0}&idact={1}", Request.QueryString["tipo"], idActividadNueva));

                AgenteMaestros agenteMaestro = new AgenteMaestros();
                agenteMaestro.ObtenerCongresos(null, true);
                Session["IdConfEmpresa"] = null;
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Global.SendApplicationError(ex, Request, Session, GetType().Name);
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Alert.Show("Se ha producido un error en la grabación de la Actividad", null);
            }
            
        }

        protected void SetBackString()
        {

            if (Page.Request.UrlReferrer != null)
            {

                if (Page.Request.UrlReferrer.AbsoluteUri.Contains("NuevoDetalleAMEC.aspx"))
                {

                }
                if (Page.Request.UrlReferrer.AbsoluteUri.Contains("NuevoExpedientePasoA.aspx"))
                {

                }
            }

        }

        [System.Web.Services.WebMethod]
        public static IEnumerable<DVPoblacion> poblacionSearch(string idPais, string search, int idConfEmpresa)
        {
            try
            {
                AgenteMaestros agente = new AgenteMaestros();
                var resultList = (agente.ObtenerPoblacionesTodas(null, idConfEmpresa)).Where(x=>x.IdPais == idPais && x.Poblacion.ToLower().Contains(search.ToLower())).OrderBy(x => x.Poblacion).ToList().Take(15);
                return resultList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}