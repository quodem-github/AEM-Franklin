using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Specialized;
using EOS.Entidades.Datos;
using EOS.Entidades.Filtros;
using EOS.Repositorios;
using EOS.Web;
using EOS.Entidades.Modelo;

namespace EOS
{
    public partial class EventosForms : System.Web.UI.Page
    {
        public string FormAction
        {
            get
            {
                AgenteMaestros agente = new AgenteMaestros();
                string idConfEmpresa = Request.QueryString["idConfEmpresa"];
                string urlEvento = Request.QueryString["urlEvento"];
                string idEvento = Request.QueryString["idEvento"];
                DEmpresaConf empresa = agente.ObtenerEmpresasConf().ToList().Find(x => x.idconfempresa.ToString() == idConfEmpresa);
                if (urlEvento.EndsWith("=" + idEvento) && idEvento.Length > 0)
                {
                    return empresa.ruta_formulario_grupos;
                }
                else
                {
                    return urlEvento;
                }
            }
        }

        public string FormMethod
        {
            get {
                AgenteMaestros agente = new AgenteMaestros();
                string idConfEmpresa = Request.QueryString["idConfEmpresa"];
                string urlEvento = Request.QueryString["urlEvento"];
                string idEvento = Request.QueryString["idEvento"];
                DEmpresaConf empresa = agente.ObtenerEmpresasConf().ToList().Find(x => x.idconfempresa.ToString() == idConfEmpresa);
                if (urlEvento.EndsWith("=" + idEvento) && idEvento.Length > 0)
                {
                    return "post";
                }
                else
                {
                    return "get";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            EOS.Entidades.Datos.DVPeticionariosRoles rolesUser = new EOS.Entidades.Datos.DVPeticionariosRoles();
            string idEvento = Request.QueryString["idEvento"];
            string urlEvento = Request.QueryString["urlEvento"];

            if (Session["rolesUser"] != null && idEvento != null)
            {
                rolesUser = Session["rolesUser"] as EOS.Entidades.Datos.DVPeticionariosRoles;                
                if (urlEvento.EndsWith("=" + idEvento) && idEvento.Length > 0)
                {
                    IdEvento.Value = idEvento;
                }
                else
                {
                    IdEvento.Value = urlEvento.Substring(urlEvento.LastIndexOf("/") + 1);
                }
                IdUser.Value = rolesUser.login.ToString();
                EosToken.Value = rolesUser.password.ToString();
            }
        }
    }
}