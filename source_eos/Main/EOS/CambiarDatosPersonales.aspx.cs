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
    public partial class CambiarDatosPersonales : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ///////////////Controlar Si caduca la sesión en la aplicación//////////////
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Session["rolesuser"] == null) Response.Redirect("~/Account/Login.aspx");
            //////////////////////////////////////////////////////////////////////////


            if (!this.IsPostBack)
            {
                Response.Redirect("~/Expedientes.aspx");
                RellenarCombos(1);
                //Xavier Morell
                //AgenteUsuarios agente = new AgenteUsuarios();
                //DDatosPersonalesUsuario datos = agente.ObtenerDatosPersonalesPorLogin();
                DDatosPersonalesUsuario datos = (DDatosPersonalesUsuario)Session["datosUsuario"];

                ViewState.Add("PwdTemporal", System.Web.Security.Membership.GeneratePassword(8, 0));
                ViewState.Add("PwdHashAnterior", datos.password);

                this.PrimerApellido.Text = datos.Apellido1;
                this.SegundoApellido.Text = datos.Apellido2;

                //this.Password.Text = ViewState["PwdTemporal"].ToString();
                this.Password.Attributes.Add("value", ViewState["PwdTemporal"].ToString());
                this.RepetirPassword.Attributes.Add("value", ViewState["PwdTemporal"].ToString());

                this.Nombre.Text = datos.Nombre;
                this.Telefono.Text = datos.Telefono;
                this.Extension.Text = datos.extension;
                this.Movil.Text = datos.Movil;
                this.CorreoElectronico.Text = datos.Email;
                this.Direccion.Text = datos.Direccion;
                this.CodigoPostal.Text = datos.CodPostal;
                this.ddlPoblacion.SelectedValue = datos.IdPoblacion.HasValue ? datos.IdPoblacion.ToString() : "-1";
                RellenarProvincia(this.ddlPoblacion.SelectedValue, datos.IdConfEmpresa ?? 1);
                //Ismael Ameller 20/04/2011 Añadimos AMEX al peticionario
                this.TxtAmex.Text = datos.amex;
                if (datos.fechacaducidad != null)
                {
                    this.FechaCaducidadDatePickerControl.Value = datos.fechacaducidad.ToString().Substring(0, 10);
                }
                //FIN Ismael Ameller 20/04/2011 Añadimos AMEX al peticionario

            }
        }

        private void RellenarCombos(int idconfempresa)
        {
            AgenteMaestros agente = new AgenteMaestros();
            this.ddlPoblacion.DataSource = agente.ObtenerPoblacionesTodas("(Todas)", idconfempresa);
            this.ddlPoblacion.DataBind();
        }

        private void RellenarProvincia(string sIdPoblacion, int idconfempresa)
        {
            int nIdPoblacion = int.Parse(sIdPoblacion);
            AgenteMaestros agente = new AgenteMaestros();
            DVPoblacion dpSelect = agente.ObtenerPoblacionesTodas(null, idconfempresa).FirstOrDefault(f => f.IdPoblacion == nIdPoblacion);

            if (dpSelect != null)
            {
                this.Pais.Text = dpSelect.Pais;
                this.Provincia.Text = dpSelect.Provincia;
            }
            else
            {
                this.Pais.Text = "";
                this.Provincia.Text = "";
            }
        }

        //protected void RedirigirAPaginaInicial()
        //{
        //    Response.Redirect("~/Expedientes.aspx");
        //}

        protected void AceptarImageButton_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //Xavier Morell
                AgenteUsuarios agente = new AgenteUsuarios();
                //DDatosPersonalesUsuario datos = agente.ObtenerDatosPersonalesPorLogin();
                DDatosPersonalesUsuario datos = (DDatosPersonalesUsuario)Session["datosUsuario"];

                if (this.Password.Text != this.RepetirPassword.Text)
                {
                    Alert.Show("Las contraseñas deben ser iguales", null);
                }
                else
                {
                    string sPwdTemporal = (string)ViewState["PwdTemporal"];
                    if (sPwdTemporal != this.Password.Text)
                    {
                        //Ha cambiado la contraseña, por lo tanto hay que guardarla de nuevo
                        datos.password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(this.Password.Text, "sha1");
                    }
                    else
                    {
                        //no ha cambiado la contraseña por loque dejamos el anterior hash
                        datos.password = (string)ViewState["PwdHashAnterior"];
                    }
                }

                datos.Apellido1 = this.PrimerApellido.Text;
                datos.Apellido2 = this.SegundoApellido.Text;
                datos.Nombre = this.Nombre.Text;
                datos.Telefono = this.Telefono.Text;
                datos.extension = this.Extension.Text;
                datos.Movil = this.Movil.Text;
                datos.Email = this.CorreoElectronico.Text;
                datos.Direccion = this.Direccion.Text;
                datos.CodPostal = this.CodigoPostal.Text;
                datos.IdPoblacion = int.Parse(this.ddlPoblacion.SelectedValue) < 0 ? new Nullable<int>() : int.Parse(this.ddlPoblacion.SelectedValue);
                //Ismael Ameller 20/04/2011 Añadimos AMEX al peticionario
                datos.amex = this.TxtAmex.Text;
                if (string.IsNullOrEmpty(this.FechaCaducidadDatePickerControl.Value))
                {
                    datos.fechacaducidad = null;
                }
                else
                {
                    datos.fechacaducidad = Convert.ToDateTime(this.FechaCaducidadDatePickerControl.Value);
                }
                //FIN Ismael Ameller 20/04/2011 Añadimos AMEX al peticionario

                if (agente.ActualizarDatosPersonalesPrimerLogin(datos))
                {
                    Alert.Show("Los datos se han actualizado correctamente", agente.NavegacionDesdeLogin());
                }
                else
                {
                    Alert.Show("Alguno de los datos introducidos no es correcto. Por favor revise la información introducida.", null);
                }

                //RedirigirAPaginaInicial();
            }
            catch (Exception ex)
            {
                //Ismael Ameller 09-03-2011 Envio de Mail
                Mail mail = new Mail();
                mail.Send(ConfigUtil.GetAppSetting("ContactoError"), ex);
                //FIN Ismael Ameller 09-03-2011 Envio de Mail
                Alert.Show("Se ha producido un error actualizando sus datos. Por favor revise la información introducida.", null);
            }
        }

        protected void ddlPoblacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            DDatosPersonalesUsuario datos = (DDatosPersonalesUsuario)Session["datosUsuario"];
            RellenarProvincia(this.ddlPoblacion.SelectedValue, datos.IdConfEmpresa ?? 1);
        }

        //protected void CancelarImageButton_Click(object sender, ImageClickEventArgs e)
        //{
        //    RedirigirAPaginaInicial();
        //}
    }
}