using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Entidades.Datos;
using EOS.Web;
using System.Net;
using System.Net.Mail;
using EOS.Web.Enums;
using NPOI.SS.Formula.Functions;

namespace EOS
{
    public partial class Delegacion : System.Web.UI.Page
    {
        EOS.Entidades.Datos.DVPeticionariosRoles datosRoles = null;
        #region Clases

        private class FiltroDelegacion
        {
            public string Usuario { get; set; }
            public string Delegado { get; set; }
            public string EstadoExpediente { get; set; }
            public string FechaDesde { get; set; }
            public string FechaHasta { get; set; }
            public string Estado { get; set; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            datosRoles = (EOS.Entidades.Datos.DVPeticionariosRoles)Session["rolesuser"];
            if (!Page.IsPostBack)
            {
                CargarValoresIniciales(datosRoles);
            }
        }
        
        public void CargarValoresIniciales(DVPeticionariosRoles datosUsuario)
        {
            AgenteUsuarios agenteUsuActual = new AgenteUsuarios();
            //var estadosDel = Enum.GetValues(typeof(BusinessLogic.Enums.EstadoDelegacion)).Cast<BusinessLogic.Enums.EstadoDelegacion>();
            Dictionary<int, string> estadosDelegacion = new Dictionary<int, string>();
            foreach (int value in Enum.GetValues(typeof(EstadoDelegacion)))
            {
                String name = Enum.GetName(typeof(EstadoDelegacion), value);
                estadosDelegacion.Add(value,name);
            }

            ddlEstado.DataSource = estadosDelegacion;
            ddlEstado.DataValueField = "key";
            ddlEstado.DataTextField = "value";
            ddlEstado.DataBind();

            ddlEstado.SelectedValue = "-1";


            if (datosUsuario.administrador.Value)
            {
                ddlUsuario.DataSource = agenteUsuActual.ObtenerTodosUsuarios();
                ddlUsuario.DataBind();
                ddlDelegado.DataSource = agenteUsuActual.ObtenerTodosUsuarios();
                ddlDelegado.DataBind();
            }
            else
            {
                ddlUsuario.SelectedItem.Value = datosUsuario.IdPeticionario.ToString();
                ddlUsuario.SelectedItem.Text = datosUsuario.Nombre + ' ' + datosUsuario.Apellido1;
                ddlUsuario.Enabled = false;
                if (datosRoles.IdCargo == 561)
                {
                    ddlDelegado.DataSource = agenteUsuActual.ObtenerReportesDirectosAssistant(datosRoles.idunidad.Value, 538);
                    ddlDelegado.DataBind();
                }
                if (datosRoles.IdCargo == 534)
                {
                    ddlDelegado.DataSource = agenteUsuActual.ObtenerUsuariosPorCargo(538);
                    ddlDelegado.DataBind();
                }
            }
        }

        public bool ValidateFecha()
        {
            if (txtFechaDesde.Text != "" && txtFechaHasta.Text != "")
            {
                if (DateTime.Compare(DateTime.Parse(txtFechaHasta.Text), DateTime.Parse(txtFechaDesde.Text)) < 0)
                {
                    Alert.Show("No se puede hacer un filtro donde la Fecha HASTA NO es anterior a la Fecha DESDE");
                    return false;
                }
                else return true;
            }
            else return true;
        }

        private bool ComprobarAnularDelegacion(int iddelegacion, AgenteDelegacion agDeleg)
        {

            DVDelegacion DVDel = agDeleg.ObtenerDelegacionPorID(iddelegacion);
            if (DateTime.Now <= DVDel.fechadesde) return true;
            else if (DVDel.IdEstado == EstadoDelegacion.Anulado.GetHashCode()) return false;
            else return false;
        }

        private bool ComprobarEditarDelegacion(int iddelegacion, AgenteDelegacion agDeleg)
        {

            DVDelegacion DVEdit = agDeleg.ObtenerDelegacionPorID(iddelegacion);
            if (DateTime.Now > DVEdit.fechahasta || DVEdit.IdEstado == EstadoDelegacion.Anulado.GetHashCode() 
                || DVEdit.IdEstado == EstadoDelegacion.Vencido.GetHashCode()) return false;
            else return true;
        }

        public void EnviaMailAnularDelegacion(int iddelegacion)
        {
            try
            {
                AgenteDelegacion adDel = new AgenteDelegacion();
                DVDelegacion dDelegacion = adDel.ObtenerDelegacionPorID(iddelegacion);

                AgenteUsuarios agUsu = new AgenteUsuarios();
                DDatosPersonalesUsuario dDelegado = agUsu.ObtenerDatosPersonalesPorIDPeticionario(dDelegacion.iddelegado.ToString());
                DDatosPersonalesUsuario dUsuario = agUsu.ObtenerDatosPersonalesPorIDPeticionario(dDelegacion.idcreadopor.ToString());

                System.Net.Mail.SmtpClient smtpSender = new System.Net.Mail.SmtpClient();
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

                try { message.To.Add(new MailAddress(dDelegado.Email)); }
                catch { Alert.Show("Se ha producido un error enviando el mail de Anulación de Delegación. Por favor revise la dirección de correo del Solicitante.", null); return; }

                message.Subject = string.Format("Delegación Anulada");

                message.Body = string.Format("Se ha Anulado la Delegación con:  \r\nUsuario: {0} \r\n Delegado: {1}\r\n Fecha Desde: {2}\r\n Fecha Hasta: {3}", dUsuario.NombreCompleto, dDelegado.NombreCompleto, dDelegacion.fechadesde, dDelegacion.fechahasta);

                smtpSender.Send(message);
            }
            catch (Exception ex)
            {
                Alert.Show("Se ha producido un error enviando el mail de Anulación de Delegación", null);
            }
        }


        protected void imgDelegacionAnular_Command(object sender, CommandEventArgs e)
        {
            AgenteDelegacion agDeleg = new AgenteDelegacion();
            int iddelegaprobacion = Convert.ToInt32(e.CommandArgument.ToString());
            bool sePuedeAnular = ComprobarAnularDelegacion(iddelegaprobacion, agDeleg);
            if (sePuedeAnular)
            {
                EnviaMailAnularDelegacion(iddelegaprobacion);
                agDeleg.AnularDelegacion(iddelegaprobacion);
                Alert.Show("Delegación anulada correctamente");
            }
            else Alert.Show("No se puede Anular la Delegación");
            lvDelegaciones.DataBind();
        }

        protected void imgDelegacionEditar_Command(object sender, CommandEventArgs e)
        {
            AgenteDelegacion agDeleg = new AgenteDelegacion();
            int iddelegaprobacion = Convert.ToInt32(e.CommandArgument.ToString());
            bool sePuedeEditar = ComprobarEditarDelegacion(iddelegaprobacion, agDeleg);
            if (sePuedeEditar)
            {
                Response.Redirect("EditDelegacion.aspx?iddelegaprobacion=" + iddelegaprobacion);
            }
            else Alert.Show("No se puede Editar la Delegación");
            lvDelegaciones.DataBind();
        }


        protected void odsDelegacion_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlUsuario.SelectedValue.Trim())) e.InputParameters["filtroidusuario"] = ddlUsuario.SelectedValue.Trim();
            if (!string.IsNullOrEmpty(ddlDelegado.SelectedValue.Trim())) e.InputParameters["filtroiddelegado"] = ddlDelegado.SelectedValue.Trim();
            if (!string.IsNullOrEmpty(txtFechaHasta.Text.Trim())) e.InputParameters["filtrofechahasta"] = txtFechaHasta.Text.Trim();
            if (!string.IsNullOrEmpty(txtFechaDesde.Text.Trim())) e.InputParameters["filtrofechadesde"] = txtFechaDesde.Text.Trim();
            if (!string.IsNullOrEmpty(ddlEstado.SelectedValue.Trim())) e.InputParameters["filtroestado"] = ddlEstado.SelectedValue.Trim();

            if (!Page.IsPostBack)
            {
                e.Arguments.MaximumRows = 10;
                e.Arguments.StartRowIndex = 0;
            }
        }

        protected void lvDelegaciones_Load(object sender, EventArgs e)
        {

        }

        protected void btnFiltrarDelegaciones_Click(object sender, ImageClickEventArgs e)
        {
            if (ValidateFecha())
            {
                lvDelegaciones.DataBind();
            }
        }
    }
}