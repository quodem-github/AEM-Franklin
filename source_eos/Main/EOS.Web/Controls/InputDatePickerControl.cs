using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Reflection;
using System.Globalization;

namespace EOS.Web.Controls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:InputDatePickerControl runat=server></{0}:InputDatePickerControl>")]
    [ValidationProperty("Value")]
    public class InputDatePickerControl : CompositeControl
    {
        protected Label label = new Label();
        protected TextBox inputTextBox = new TextBox();
        protected ImageButton clickImageButton = new ImageButton();
        protected AjaxControlToolkit.CalendarExtender calExt = new AjaxControlToolkit.CalendarExtender();
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
        public ImageButton ClickImageButton
        {
            get
            {
                return clickImageButton;
            }

            set
            {
                clickImageButton = value;
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

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }

        protected override void CreateChildControls() {
            Controls.Clear();

            label.ID = this.ID + "_label";
            inputTextBox.ID = this.ID + "_inputTextBox";
            maskedEditExtender.ID = this.ID + "_maskedEditExtender";
            clickImageButton.ID = this.ID + "_clickImageButton";
            calExt.ID = this.ID + "_calExt";

            label.CssClass = "eosCampoLabelSin eosCampoCalendario";
            label.Font.Bold = true;

            inputTextBox.MaxLength = 80;
            inputTextBox.CssClass = "eosDisabledInputVacio eosInputVacio eosSizeW90";

            clickImageButton.ImageUrl = "~/Styles/images/ic_calendario.png";
            clickImageButton.CssClass = "iconCalendar";

            calExt.TodaysDateFormat = "d MMMM yyyy";
            calExt.FirstDayOfWeek = FirstDayOfWeek.Monday;
            calExt.Animated = true;

            this.Controls.Add(label);
            this.Controls.Add(inputTextBox);

            if (!this.DesignMode) {
                label.AssociatedControlID = inputTextBox.ID;
                this.maskedEditExtender.TargetControlID = inputTextBox.ID;
                calExt.TargetControlID = inputTextBox.ID;
                calExt.PopupButtonID = clickImageButton.ID;
            }
            this.maskedEditExtender.Mask =  "99/99/9999" ;           
            this.maskedEditExtender.MaskType = AjaxControlToolkit.MaskedEditType.Date;

            this.Controls.Add(maskedEditExtender);

            this.Controls.Add(clickImageButton);
            this.Controls.Add(calExt);

            //e.bloem: check date now add date checker javascript
            string script = "EOS.Web.Controls.CheckDate.js";

            if (this.ID == "txtFechaInicio")
                this.inputTextBox.Attributes.Add("onchange", "isDate(this.value);document.getElementById('eosContentResults_txtFechaFin_txtFechaFin_inputTextBox').value = this.value;");
            else
                this.inputTextBox.Attributes.Add("onchange", "isDate(this.value);");
            if (!this.Parent.Page.ClientScript.IsClientScriptBlockRegistered(typeof(InputDatePickerControl), script))
                this.Parent.Page.ClientScript.RegisterClientScriptResource(typeof(InputDatePickerControl), script);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this.Enabled == false) {
                this.Enabled = true;
                this.inputTextBox.Enabled = false;
                this.clickImageButton.Enabled = false;
            } else {
                this.inputTextBox.Enabled = true;
                this.clickImageButton.Enabled = true;
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
