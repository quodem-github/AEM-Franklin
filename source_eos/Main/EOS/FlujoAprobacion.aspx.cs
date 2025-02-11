using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Entidades.Datos;
using EOS.Web;



namespace EOS
{
    public partial class FlujoAprobacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ///////////////Controlar Si caduca la sesión en la aplicación//////////////
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Session["rolesuser"] == null) Response.Redirect("~/Account/Login.aspx");
            //////////////////////////////////////////////////////////////////////////


            //Session.Remove("idDelegacionSeleccionada");
            EOS.Entidades.Datos.DVPeticionariosRoles datosRoles = (EOS.Entidades.Datos.DVPeticionariosRoles)Session["rolesuser"];
            //bAprobador = (datosRoles.aprobador.HasValue) ? datosRoles.aprobador.Value : false;
            if (!Page.IsPostBack)
            {
                CargarValoresIniciales(datosRoles);
            }

        }

        public void CargarValoresIniciales(DVPeticionariosRoles datosUsuario)
        {

                //Si es Administrador aquest podrà delegar a qui sigui a nom de qui sigui, es a dir que haurem de treure tots els usuaris existents
            AgenteFlujoAprobacion agenteFlujo = new AgenteFlujoAprobacion();

            string idtipoactividad = "0";
            ICollection<DTipoActividadFlujo> datosFiltroActividad = agenteFlujo.ObtenerTipoActividad(idtipoactividad, false);
            ICollection<DTipoFlujo> datosFiltroFlujo = agenteFlujo.ObtenerTipoFlujo();
            ICollection<DNivelesAprobacion> datosNivelesAprobacion = agenteFlujo.ObtenerNivelesAprobacion();
            

            ddlTipoActividad.DataSource = datosFiltroActividad;
            ddlTipoActividad.DataBind();
            ddlTipoFlujo.DataSource = datosFiltroFlujo;
            ddlTipoFlujo.DataBind();


            


        }




        protected void odsFlujoAprobacion_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
               
            //AQUÍ S'HAN D'ADJUNTAR ELS VALORS DEL (DATA SOURCE) AMB ELS DELS (COMBOBOX, TEXTBOX,...)
            if (!string.IsNullOrEmpty(ddlTipoFlujo.SelectedValue.Trim())) e.InputParameters["filtroidtipoflujo"] = ddlTipoFlujo.SelectedValue.Trim();
            if (!string.IsNullOrEmpty(ddlTipoActividad.SelectedValue.Trim())) e.InputParameters["filtroidactividad"] = ddlTipoActividad.SelectedValue.Trim();
             if (!Page.IsPostBack)
            {

                e.Arguments.MaximumRows = 10;
                e.Arguments.StartRowIndex = 0;
                //e.InputParameters[1] = "2";
                //e.Arguments.SortExpression.
            }
            else
            {
                //oFiltroExp.startRowIndex = e.Arguments.StartRowIndex;
                //oFiltroExp.sortParameter = e.Arguments.SortExpression;
            }
            //e.InputParameters["mes"] = "hola";

        }

        protected void lvFlujoAprobacion_Load(object sender, EventArgs e)
        {
            //lvDelegaciones.DataBind();
        }

        protected void imgDelegacionEliminar_Command(object sender, CommandEventArgs e)
        {
            AgenteFlujoAprobacion agenteFlujo = new AgenteFlujoAprobacion();
            int idflujoaprobacion = Convert.ToInt32(e.CommandArgument.ToString());
            agenteFlujo.EliminarFlujoAprobacion(idflujoaprobacion);
            //AprobarSeleccion();
            //LimpiarDatosFormulario();
            lvFlujoAprobacion.DataBind();
        }
        protected void btnNuevoFlujo_Click(object sender, ImageClickEventArgs e)
        {


        }
        protected void btnFiltrarFlujo_Click(object sender, ImageClickEventArgs e)
        {
            lvFlujoAprobacion.DataBind();

        }
    }
}