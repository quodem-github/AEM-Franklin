using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Logica;
using EOS.Web;
using EOS.Entidades.Datos;

namespace EOS.Controls
{
    public partial class BotoneraPorEstados : System.Web.UI.UserControl
    {
        public event EventHandler OnEnviar;
        public event EventHandler OnModificar;
        public event EventHandler OnAprobar;
        public event EventHandler OnCancelar;

        private DCabeceraExpedienteAmpliado expediente
        {
            get
            {
                AgenteExpedientes agenteExp = new AgenteExpedientes();
                DCabeceraExpedienteAmpliado value = agenteExp.ObtenerExpedientePorID(Request.QueryString["idexp"]);
                HttpContext.Current.Session["currentFKIdCongreso"] = value.Idactividad;

                return value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                
                
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.changeVisibilit();
        }
        protected void changeVisibilit()
        {
            if (this.expediente != null)
            {
                this.enviarImageButton.Visible = this.expediente.Idestado.Equals(MaquinaEstados.SinEnviar);
                this.modificarImageButton.Visible = (
                            this.expediente.Idestado.Equals(MaquinaEstados.Enviado)
                        || this.expediente.Idestado.Equals(MaquinaEstados.Cotizado)
                        || this.expediente.Idestado.Equals(MaquinaEstados.Aceptado)
                    );
                this.aprobarImageButton.Visible = (
                            this.expediente.Idestado.Equals(MaquinaEstados.Cotizado)
                    );
                this.cancelarImageButton.Visible = (
                            this.expediente.Idestado.Equals(MaquinaEstados.Enviado)
                        || this.expediente.Idestado.Equals(MaquinaEstados.Cotizando)
                        || this.expediente.Idestado.Equals(MaquinaEstados.Cotizado)
                        || this.expediente.Idestado.Equals(MaquinaEstados.Aceptado)
                        || this.expediente.Idestado.Equals(MaquinaEstados.Tramitado)
                        || this.expediente.Idestado.Equals(MaquinaEstados.Tramitando)
                    );
            }
        }

        protected void ImageButton_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Enviar")
            {
                if (OnEnviar != null) OnEnviar(this, e);
            }
            if (e.CommandName == "Modificar")
            {
                if (OnModificar != null) OnModificar(this, e);
            }
            if (e.CommandName == "Aprobar")
            {
                if (OnAprobar != null) OnAprobar(this, e);
            }
            if (e.CommandName == "Cancelar")
            {
                if (OnCancelar != null) OnCancelar(this, e);
            }
        }
    }
}