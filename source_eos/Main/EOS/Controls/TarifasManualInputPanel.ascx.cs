using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EOS.Controls
{
    public partial class TarifasManualInputPanel : System.Web.UI.UserControl
    {
        public string Tipo
        {
            get { return this.tipoTextBox.Text; }
            set { this.tipoTextBox.Text = value; }
        }

        public double Importe
        {
            get {
                double d = 0;
                Double.TryParse(this.importeTextBox.Text, out d);
                return d;
            
            
            }
            set { this.importeTextBox.Text = value.ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}