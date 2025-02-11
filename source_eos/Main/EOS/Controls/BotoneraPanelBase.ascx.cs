using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EOS.Controls
{
    public partial class BotoneraPanelBase : UserControl
    {

        /// <summary>
        /// btnGuardar control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        public global::System.Web.UI.WebControls.ImageButton btnGuardar;

        /// <summary>
        /// btnNuevo control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        public global::System.Web.UI.WebControls.ImageButton btnNuevo;

        /// <summary>
        /// btnEnviar control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        public global::System.Web.UI.WebControls.ImageButton btnEnviar;

        /// <summary>
        /// btnAprobar control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        public global::System.Web.UI.WebControls.ImageButton btnAprobar;

        /// <summary>
        /// btnRechazar control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        public global::System.Web.UI.WebControls.ImageButton btnRechazar;

        /// <summary>
        /// btnCancelar control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        public global::System.Web.UI.WebControls.ImageButton btnCancelar;

        protected void Page_Load(object sender, EventArgs e) { }

        public void SetButtons(string state)
        {
            this.btnAprobar.Visible = false;
            this.btnRechazar.Visible = false;
            this.btnNuevo.Visible = false;
            //this.btnEnviar.Visible = false;
            //this.btnGuardar.Visible = false;
            //this.btnEnviar.Visible =  !showAlternative;
            //this.btnGuardar.Visible = !showAlternative;
            this.btnCancelar.Visible = false;
            switch (state)
            {
                case "AB": //sin enviar
                    this.btnEnviar.Visible = true;
                    this.btnGuardar.Visible = true;
                    this.btnRechazar.Visible = true;
                    break;
                case "ABI": //abierto
                    break;
                case "AC": //aceptada
                    break;
                case "ACP": //aceptada
                    break;
                case "AN": //rechazado
                    break;
                case "CER": //cerrado
                    break;
                case "CFP": //aceptado
                    break;
                case "CN": //cancelado
                    break;
                case "CR": //enviado
                    this.btnRechazar.Visible = true;
                    this.btnEnviar.Visible = false;
                    this.btnGuardar.Visible = false;
                    break;
                case "CTZD": //cotizando
                    this.btnRechazar.Visible = false;
                    this.btnEnviar.Visible = false;
                    this.btnGuardar.Visible = false;
                    break;
                case "FZ": //finalizado
                    break;
                case "MDF": //MDF
                    break;
                case "NC": // en curso
                    break;
                case "CTZ": //cotizado
                    this.btnEnviar.Visible = false;
                    this.btnGuardar.Visible = false;
                    this.btnAprobar.Visible = true;
                    this.btnRechazar.Visible = true;
                    break;
                case "PTR": //tramitando
                    //if (showAlternative)  // && this.Alternativas.NumAlternativas > 0)
                    //{
                    //    this.btnAprobar.Visible = true;
                    //    this.btnRechazar.Visible = true;
                    //}
                    this.btnEnviar.Visible = false;
                    this.btnGuardar.Visible = false;
                    this.btnCancelar.Visible = false;
                    break;
                case "TR": //tramitado
                    this.btnEnviar.Visible = false;
                    this.btnGuardar.Visible = false;
                    this.btnCancelar.Visible = true;
                    break;
            }
        }
    }
}