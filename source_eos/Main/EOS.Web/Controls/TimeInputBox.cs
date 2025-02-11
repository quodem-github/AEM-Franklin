using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EOS.Web.Controls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TimeInputBox runat=server></{0}:TimeInputBox>")]
    public class TimeInputBox : CompositeControl
    {
        protected Label label = new Label();
        protected TextBox inputTextBox = new TextBox();
        protected AjaxControlToolkit.MaskedEditExtender maskedEditExtender = new AjaxControlToolkit.MaskedEditExtender();

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public TextBox InputTextBox
        {
            get
            {
                return inputTextBox;
            }

            set
            {
                inputTextBox = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = label.Text;
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                label.Text = value;
            }
        }

        public string Value
        {
            get
            {
                return this.inputTextBox.Text;
            }
            set
            {
                this.inputTextBox.Text = value;
            }
        }


        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            label.ID = this.ID + "_label";
            inputTextBox.ID = this.ID + "_inputTextBox";
            maskedEditExtender.ID = this.ID + "_maskedEditExtender";

            //this.inputTextBox.Width = new Unit("40px");
            //this.inputTextBox.Style.Add("font-family", "Arial, Helvetica, sans-serif; font-size: x-small");
            //this.inputTextBox.Style.Add("font-size", "x-small");

            label.CssClass = "eosCampoLabelSin";
            label.Font.Bold = true;

            inputTextBox.CssClass = "eosDisabledInputVacio";

            this.Controls.Add(label);
            this.Controls.Add(inputTextBox);

            if (!this.DesignMode)
            {
                this.label.AssociatedControlID = this.inputTextBox.ID;
                this.maskedEditExtender.TargetControlID = inputTextBox.ID;
            }

            this.maskedEditExtender.Mask = "99:99";
            this.maskedEditExtender.MaskType = AjaxControlToolkit.MaskedEditType.Time;

            this.Controls.Add(maskedEditExtender);

            //e.bloem: check date now add date checker javascript
            string script = "EOS.Web.Controls.CheckTime.js";

            this.inputTextBox.Attributes.Add("onchange", "isTime(this.value);");
            if (!this.Parent.Page.ClientScript.IsClientScriptBlockRegistered(typeof(TimeInputBox), script))
                this.Parent.Page.ClientScript.RegisterClientScriptResource(typeof(TimeInputBox), script);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this.Enabled == false) {
                this.Enabled = true;
                this.inputTextBox.Enabled = false;
            } else {
                this.inputTextBox.Enabled = true;
            }
            base.OnPreRender(e);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            foreach (Control item in this.Controls)
            {
                item.RenderControl(output);
            }
        }
    }
}
