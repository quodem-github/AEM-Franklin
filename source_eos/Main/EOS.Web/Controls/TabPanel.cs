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
    [ToolboxData("<{0}:TabPanel runat=server></{0}:TabPanel>")]
    public class TabPanel : AjaxControlToolkit.TabPanel,IEOSControl 
    {
        public TabPanel() : base()
        {
            
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        public bool hasPendingChanges
        {
            get
            {
                if (ViewState["hasPendingChanges"] == null)
                    ViewState["hasPendingChanges"] = false;
                return (bool)ViewState["hasPendingChanges"];
            }
        }

        public void setHasPendingChanges()
        {
            ViewState["hasPendingChanges"] = true;
        }
    }
}
