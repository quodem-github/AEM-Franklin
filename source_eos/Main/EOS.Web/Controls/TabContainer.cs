using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace EOS.Web.Controls
{
    [ToolboxData("<{0}:TabContainer runat=server></{0}:TabContainer>")]
    public class TabContainer : AjaxControlToolkit.TabContainer 
    {
        private int lastTabIndex = 0;

        protected override void OnActiveTabChanged(EventArgs e)
        {
            foreach (var item in this.Controls)
            {
                if (item.GetType().Equals(typeof(EOS.Web.Controls.TabPanel)))
                {
                    EOS.Web.Controls.TabPanel panel = (EOS.Web.Controls.TabPanel) item;
                    if (panel.hasPendingChanges)
                    {
                        this.ActiveTabIndex = this.lastTabIndex;
                        return;
                    }
                }
            }
            this.lastTabIndex = base.ActiveTabIndex;
        }

        public TabContainer() : base()
        {
            base.AutoPostBack = false;
        }


    }
}
