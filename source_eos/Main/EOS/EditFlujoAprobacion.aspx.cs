using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Entidades.Datos;
using EOS.Web;

namespace EOS
{
	public partial class EditFlujoAprobacion : System.Web.UI.Page
	{
        EOS.Entidades.Datos.DVPeticionariosRoles datosRoles = null;
		protected void Page_Load(object sender, EventArgs e)
		{

            string sIdDel = string.Empty;
            sIdDel = Request.QueryString["idflujoaprobacion"]; 
            //Session.Remove("idDelegacionSeleccionada");
            datosRoles = (EOS.Entidades.Datos.DVPeticionariosRoles)Session["rolesuser"];
            //bAprobador = (datosRoles.aprobador.HasValue) ? datosRoles.aprobador.Value : false;
            if (!Page.IsPostBack)
            {
                CargarValoresIniciales(datosRoles, sIdDel);
            }
		}


        public void CargarValoresIniciales(DVPeticionariosRoles datosUsuario, string idFlujoAprobacion)
        {

            //Si es Administrador aquest podrà delegar a qui sigui a nom de qui sigui, es a dir que haurem de treure tots els usuaris existents
            AgenteFlujoAprobacion agenteFlujo = new AgenteFlujoAprobacion();

            string idtipoactividad = "0";
            ICollection<DTipoActividadFlujo> datosFiltroActividad = agenteFlujo.ObtenerTipoActividad(idtipoactividad, false);
            ICollection<DTipoFlujo> datosFiltroFlujo = agenteFlujo.ObtenerTipoFlujo();
            ICollection<DNivelesAprobacion> datosNivelesAprobacion = agenteFlujo.ObtenerNivelesAprobacion();


            ddlNivelIncial.DataSource = datosNivelesAprobacion;
            ddlNivelIncial.DataBind();
            ddlNivelSiguiente.DataSource = datosNivelesAprobacion;
            ddlNivelSiguiente.DataBind();
            ddlTipoActividad.DataSource = datosFiltroActividad;
            ddlTipoActividad.DataBind();
            ddlTipoFlujo.DataSource = datosFiltroFlujo;
            ddlTipoFlujo.DataBind();

            if (idFlujoAprobacion != null)
            {

                CargarValoresPorDefecto(Convert.ToInt32(idFlujoAprobacion));

            }     

        }

        protected void CargarValoresPorDefecto(int idFlujo)
        {
            AgenteFlujoAprobacion agDeleg = new AgenteFlujoAprobacion();
            DVFlujoAprobacion dvFlujo = agDeleg.ObtenerFlujoAprobacionPorID(idFlujo);

            if (dvFlujo.condicionada.Value == true) ddlCondicionado.SelectedItem.Value = "1";
            else ddlCondicionado.SelectedItem.Value = "0";
            ddlNivelIncial.SelectedValue = dvFlujo.idnivelinicial.ToString();
            ddlNivelSiguiente.SelectedValue = dvFlujo.idnivelsiguiente.ToString();
            ddlTipoActividad.SelectedValue = dvFlujo.idtipoactividad.ToString();
            ddlTipoFlujo.SelectedValue = dvFlujo.idtipoflujo.ToString();
            if (dvFlujo.preaprobado == "Preaprobada medico") ddlPreAprobado.SelectedValue = "1";
            else if (dvFlujo.preaprobado == "Preaprobada negocio") ddlPreAprobado.SelectedValue = "2";
            else ddlPreAprobado.SelectedValue = "3";
            ddlPreAprobado.SelectedValue = dvFlujo.preaprobado;
            txtFase.Text = dvFlujo.fase;
            txtOrden.Text = dvFlujo.ordenfase.ToString();
            txtImporte.Text = dvFlujo.importepreaprobacion.ToString();


        }
        protected void btnAprobarFlujo_Click(object sender, ImageClickEventArgs e)
        {
            AgenteFlujoAprobacion agenteFlujo = new AgenteFlujoAprobacion();

            string sIdFluj = string.Empty;
            sIdFluj = Request.QueryString["idflujoaprobacion"];
            int idCreadoPor = datosRoles.IdPeticionario; 
            int BenGuardat = 0;
            if (sIdFluj == null)
            {
                BenGuardat = agenteFlujo.AprobarFlujoAprobacion(ddlTipoFlujo.SelectedValue.Trim(), ddlTipoActividad.SelectedValue.Trim(), txtOrden.Text.Trim(), txtFase.Text.Trim(), txtImporte.Text.Trim(), ddlNivelIncial.SelectedValue.Trim(), ddlNivelSiguiente.SelectedValue.Trim(), ddlPreAprobado.SelectedItem.Text.Trim(), ddlCondicionado.SelectedValue.Trim(), idCreadoPor);
            }
            else
            {
                BenGuardat = agenteFlujo.ActualizarFlujoAprobacion(ddlTipoFlujo.SelectedValue.Trim(), ddlTipoActividad.SelectedValue.Trim(), txtOrden.Text.Trim(), txtFase.Text.Trim(), txtImporte.Text.Trim(), ddlNivelIncial.SelectedValue.Trim(), ddlNivelSiguiente.SelectedValue.Trim(), ddlPreAprobado.SelectedItem.Text.Trim(), ddlCondicionado.SelectedValue.Trim(), Convert.ToInt32(sIdFluj), idCreadoPor);
            }
            if (BenGuardat == 1)
            {
                Response.Write("Se ha guardado correctamente el Flujo de Aprobación");
                Response.Redirect("FlujoAprobacion.aspx?");
            }
            else Response.Write("No se ha guardado correctamente el Flujo de Aprobación");

        }
	}
}