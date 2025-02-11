using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Net.Mail;

using EOS.Web;
using EOS.Entidades.Datos;
using EOS.Repositorios;

namespace EOS
{
    public partial class EditDelegacion : System.Web.UI.Page
    {
        EOS.Entidades.Datos.DVPeticionariosRoles datosRoles = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            string sIdDel = string.Empty;
            sIdDel = Request.QueryString["iddelegaprobacion"];

            //Session.Remove("idDelegacionSeleccionada");
            datosRoles = (EOS.Entidades.Datos.DVPeticionariosRoles)Session["rolesuser"];
            //bAprobador = (datosRoles.aprobador.HasValue) ? datosRoles.aprobador.Value : false;
            if (!Page.IsPostBack)
            {
                CargarValoresIniciales(datosRoles, sIdDel);
            }
        }

        public bool ValidateFecha()
        {
            if (DateTime.Compare(DateTime.Parse(txtFechaDesde.Text), DateTime.Parse(DateTime.Now.ToShortDateString())) < 0)
            {
                Alert.Show("La Fecha DESDE NO puede ser anterior a hoy");
                return false;
            }
            else if (DateTime.Compare(DateTime.Parse(txtFechaHasta.Text), DateTime.Parse(txtFechaDesde.Text)) < 0)
            {
                Alert.Show("La Fecha HASTA NO puede ser anterior a la Fecha DESDE");
                return false;
            }
            else return true;

        }

        public void EnviaMailDelegacion()
        {
            try
            {
                AgenteUsuarios agUsu = new AgenteUsuarios();
                DDatosPersonalesUsuario dDelegado = agUsu.ObtenerDatosPersonalesPorIDPeticionario(ddlDelegado.SelectedValue);

                System.Net.Mail.SmtpClient smtpSender = new System.Net.Mail.SmtpClient();
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                //message.From = new MailAddress("sender@foo.bar.com");
                try { message.To.Add(new MailAddress(dDelegado.Email)); }
                catch { Alert.Show("Se ha producido un error enviando el mail de Creación de Delegación. Por favor revise la dirección de correo del Solicitante.", null); return; }

                message.Subject = string.Format("Delegación Creada");
                message.Body = string.Format("Delegación Creada: \r\nUsuario: {0} \r\n Delegado: {1}\r\n Fecha Desde: {2}\r\n Fecha Hasta: {3}", ddlUsuario.SelectedItem.Text, ddlDelegado.SelectedItem.Text, txtFechaDesde.Text, txtFechaHasta.Text);

                smtpSender.Send(message);

            }
            catch (Exception ex)
            {
                Alert.Show("Se ha producido un error enviando el mail de Creación de Delegación", null);
            }
        }



        public void CargarValoresIniciales(DVPeticionariosRoles datosUsuario, string idDelegacion)
        {

            //Si es Administrador aquest podrà delegar a qui sigui a nom de qui sigui, es a dir que haurem de treure tots els usuaris existents
            this.txtFechaDesde.Text = DateTime.Now.ToShortDateString();
            AgenteUsuarios agenteUsuActual = new AgenteUsuarios();
            //ICollection<DDatosPersonalesUsuario> datosUsuariosDelegadosAdmin = agenteUsuActual.ObtenerTodosUsuarios();
            //ICollection<DDatosPersonalesUsuario> datosDelegados = agenteUsuActual.ObtenerTodosUsuarios();
            if (datosUsuario.administrador.Value)
            {
                ddlUsuario.DataSource = agenteUsuActual.ObtenerTodosUsuarios();
                ddlUsuario.DataBind();
                ddlDelegado.DataSource = agenteUsuActual.ObtenerTodosUsuarios();
                ddlDelegado.DataBind();      
            }
            
            //Si no estan haciendo una Delegación Nueva
            if (idDelegacion != null)
            {

                CargarValoresPorDefecto(Convert.ToInt32(idDelegacion), datosUsuario);

            }
            



        }


        public void CargarValoresPorDefecto(int idDelegacion, DVPeticionariosRoles datosUsuario)
        {

             
            AgenteDelegacion agDeleg = new AgenteDelegacion();
            DVDelegacion dvDelegacion = agDeleg.ObtenerDelegacionPorID(Convert.ToInt32(idDelegacion));
            ddlDelegado.SelectedValue = dvDelegacion.iddelegado.ToString();
            ddlUsuario.SelectedValue = dvDelegacion.idusuariodel.ToString();

            
            txtFechaDesde.Text = String.Format("{0:dd/MM/yy}", dvDelegacion.fechadesde);
            txtFechaHasta.Text = String.Format("{0:dd/MM/yy}", dvDelegacion.fechahasta);
            //if fecha desde ya ha pasado solo se podrá modificar la fecha hasta
            if (DateTime.Now > dvDelegacion.fechadesde) 
            {
                txtFechaDesde.Enabled = false;
                ddlDelegado.Enabled = false;
            }
        }


        protected void btnVolver_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Delegacion.aspx");
        }
        protected void btnAprobarDelegaciones_Click(object sender, ImageClickEventArgs e)
        {

            if (ValidateFecha())
            {
                //Abans de tot s'ha de comprovar que els valors indicats són bons
                AgenteDelegacion agDeleg = new AgenteDelegacion();

                string sIdDel = string.Empty;
                sIdDel = Request.QueryString["iddelegaprobacion"];
                int idCreadoPor = datosRoles.IdPeticionario;
                int BenGuardat = 0;
                if (sIdDel == null)
                {
                    BenGuardat = agDeleg.AprobarDelegacion(ddlDelegado.SelectedValue.Trim(), ddlUsuario.SelectedValue.Trim(), txtFechaDesde.Text.Trim(), txtFechaHasta.Text.Trim(), idCreadoPor);
                }
                else
                {
                    BenGuardat = agDeleg.ActualizarDelegacion(ddlDelegado.SelectedValue.Trim(), ddlUsuario.SelectedValue.Trim(), txtFechaDesde.Text.Trim(), txtFechaHasta.Text.Trim(), Convert.ToInt32(sIdDel), idCreadoPor);
                }

                if (BenGuardat == 1)
                {
                    EnviaMailDelegacion();
                    Alert.Show("Se ha guardado correctamente la Delegación");
                    Response.Redirect("Delegacion.aspx");
                }
                else Alert.Show("No se ha guardado correctamente la Delegación");

            }

        }
    }
}