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
    [ToolboxData("<{0}:InscripcionTabPanel runat=server></{0}:InscripcionTabPanel>")]
    public class InscripcionTabPanel : TabPanel
    {        
        protected GestorParticipantesPanel inscripcionGestorParticipantesPanel = new GestorParticipantesPanel();

        public InscripcionTabPanel()
            : base()
        {
            this.HeaderText = "INSCRIPCIÓN";
        }
        protected override void CreateChildControls()
        {
            Controls.Clear();
            this.inscripcionGestorParticipantesPanel.ID = "inscripcionGestorParticipantesPanel";
            // this.Controls.Add(this.Page.LoadControl("bin/Controls/Templates/TarifasInscripcionListView.ascx"));
            //this.tarifasInscripcionListView = (TarifasInscripcionListView) this.Page.LoadControl("TarifasInscripcionListView.ascx");
            //this.Controls.Add(this.tarifasInscripcionListView);
            this.Controls.Add(this.Page.LoadControl("bin/Controls/Templates/TarifasManualInputPanel.ascx"));
            this.Controls.Add(this.inscripcionGestorParticipantesPanel);
        }
    }
}
