using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Web;

namespace EOS
{
    public partial class Contacto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                AgenteMaestros agMaestro = new AgenteMaestros();
                IEnumerable<EOS.Entidades.Datos.DVEmpresa> lEmpresas = agMaestro.ObtenerEmpresas(null);
                if (lEmpresas != null)
                {
                    rptAgencias.DataSource = lEmpresas;
                    rptAgencias.DataBind();
                }
            }
        }
    }
}