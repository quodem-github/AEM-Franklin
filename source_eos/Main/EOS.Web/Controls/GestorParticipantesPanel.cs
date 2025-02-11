using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOS.Entidades.Modelo;


namespace EOS.Web.Controls
{
    [ToolboxData("<{0}:GestorParticipantesPanel runat=server></{0}:GestorParticipantesPanel>")]
    [ParseChildren(true)]
    public class GestorParticipantesPanel : CompositeControl, IEOSControl
    {
        protected String idExpediente = String.Empty;
        protected ListBox participantesDisponiblesListBox;
        protected ListBox participantesSelecionadosListBox;
        protected TextBox observacionesInscripcionTextBox;
        protected ImageButton añadirParticipanteButton;
        protected ImageButton eliminarParticipanteButton;
        protected String tabName = String.Empty;

        [Bindable(true)]
        [Category("EOS")]
        [DefaultValue("")]
        public String IdExpediente
        {
            get
            {
                if(this.idExpediente.Equals(String.Empty) && this.Page.Request.QueryString["idexp"] != null)
                {
                    idExpediente = this.Page.Request.QueryString["idexp"];
                }
                return idExpediente;
            }

            set
            {
                idExpediente = value;
            }
        }

        [Bindable(true)]
        [Category("EOS")]
        [DefaultValue("")]
        public String TabParentName
        {
            get
            {
                return tabName;
            }

            set
            {
                tabName = value;
            }
        }

        [Bindable(true)]
        [Category("EOS")]
        [DefaultValue("")]
        public string Observaciones
        {
            get
            {
                EnsureChildControls();
                return observacionesInscripcionTextBox.Text;
            }

            set
            {
                EnsureChildControls();
                observacionesInscripcionTextBox.Text = value;
            }
        }
        /**
         * Participantes Seleccionados */
        public System.Collections.ArrayList ParticipatesSeleccionados
        {
            get {
                System.Collections.ArrayList list = new System.Collections.ArrayList();
                foreach (ListItem item in this.participantesSelecionadosListBox.Items)
                {
                    list.Add(int.Parse(item.Value));
                }
                return list;
            }
        }

        public GestorParticipantesPanel()
            : base()
        {
            

        }

        protected override void RecreateChildControls()
        {
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            wintour_robotEntities db = new Entidades.Modelo.wintour_robotEntities();

            this.participantesDisponiblesListBox = new ListBox();
            participantesDisponiblesListBox.Width = 220;
            participantesDisponiblesListBox.Height = 88;
             
            if (!this.DesignMode)
            {
                // Cargamos participantes disponibles
                int idexp = -1;
                if(int.TryParse(this.IdExpediente, out idexp))
                {
                    IEnumerable<passengers_list> personas = null;

                    if (this.TabParentName.ToLower().StartsWith("inscripci"))
                    {
                        personas = passengers_list.getAvailableInscripcionList(db,idexp);

                    }
                    if (this.TabParentName.ToLower().StartsWith("alojamiento"))
                    {
                        personas = passengers_list.getAvailableAlojamientoList(db, idexp);

                    }
                    if (this.TabParentName.ToLower().StartsWith("transporte"))
                    {
                        personas = passengers_list.getAvailableTransporteList(db, idexp);

                    }
                    if (this.TabParentName.ToLower().StartsWith("otros"))
                    {
                        personas = passengers_list.getAvailableOtrosServiciosList(db, idexp);

                    }

                    participantesDisponiblesListBox.DataSource = personas;
                    participantesDisponiblesListBox.DataValueField = "DataValueField";
                    participantesDisponiblesListBox.DataTextField = "DataTextField";
                    
                    participantesDisponiblesListBox.DataBind();
                }

            }

            this.participantesSelecionadosListBox = new ListBox();
            participantesSelecionadosListBox.Width = 220;
            participantesSelecionadosListBox.Height = 88;
            if (!this.DesignMode)
            {
                // Cargamos participantes seleccionados
                
                int idexp = -1;
                if (int.TryParse(this.IdExpediente, out idexp))
                {
                    IEnumerable<passengers_list> personas = null;

                    if (this.TabParentName.ToLower().StartsWith("inscripci"))
                    {
                        personas = passengers_list.getInscripcionList(db,idexp);

                    }
                    if (this.TabParentName.ToLower().StartsWith("alojamiento"))
                    {
                        personas = passengers_list.getAlojamientoList(db, idexp);

                    }
                    if (this.TabParentName.ToLower().StartsWith("transporte"))
                    {
                        personas = passengers_list.getTransporteList(db, idexp);

                    }
                    if (this.TabParentName.ToLower().StartsWith("otros"))
                    {
                        personas = passengers_list.getOtrosServiciosList(db, idexp); ;

                    }

                    participantesSelecionadosListBox.DataSource = personas;
                    participantesSelecionadosListBox.DataValueField = "idpassengerlist";
                    participantesSelecionadosListBox.DataValueField = "DataValueField";
                    participantesSelecionadosListBox.DataTextField = "DataTextField";
                    participantesSelecionadosListBox.DataBind();
                }
            }

            this.observacionesInscripcionTextBox = new TextBox();
            observacionesInscripcionTextBox.Width = 320;
            observacionesInscripcionTextBox.Height = 88;
            observacionesInscripcionTextBox.TextMode = TextBoxMode.MultiLine;
            observacionesInscripcionTextBox.TextChanged += new EventHandler(observacionesInscripcionTextBox_TextChanged);

            this.añadirParticipanteButton = new ImageButton();
            this.eliminarParticipanteButton = new ImageButton();

            this.initButtons();

            this.Controls.Add(this.participantesDisponiblesListBox);
            this.Controls.Add(this.participantesSelecionadosListBox);
            this.Controls.Add(this.observacionesInscripcionTextBox);
            //this.Controls.Add(this.observacionesInscripcionTextBox);
            this.Controls.Add(this.añadirParticipanteButton);
            this.Controls.Add(this.eliminarParticipanteButton);
        }

        void observacionesInscripcionTextBox_TextChanged(object sender, EventArgs e)
        {
            this.setHasPendingChanges();
        }

        protected void initButtons()
        {
            añadirParticipanteButton.ImageUrl = "~/Styles/images/ic_participante_flecha.png";
            eliminarParticipanteButton.ImageUrl = "~/Styles/images/ic_participante_aspa.png";

            añadirParticipanteButton.Command += new CommandEventHandler(añadirParticipanteImageButton_OnClick);
            eliminarParticipanteButton.Command += new CommandEventHandler(eliminarParticipanteImageButton_OnClick);
        }

        protected void añadirParticipanteImageButton_OnClick(Object sender, EventArgs e)
        {
            if (this.participantesDisponiblesListBox.SelectedItem != null)
            {
                this.participantesSelecionadosListBox.SelectedIndex = -1;
                this.participantesSelecionadosListBox.Items.Add(this.participantesDisponiblesListBox.SelectedItem);
                this.participantesDisponiblesListBox.Items.Remove(this.participantesDisponiblesListBox.SelectedItem);
                this.setHasPendingChanges();
            }   
        }

        protected void eliminarParticipanteImageButton_OnClick(Object sender, EventArgs e)
        {
            if (this.participantesSelecionadosListBox.SelectedItem != null)
            {
                this.participantesDisponiblesListBox.SelectedIndex = -1;
                this.participantesDisponiblesListBox.Items.Add(this.participantesSelecionadosListBox.SelectedItem);
                this.participantesSelecionadosListBox.Items.Remove(this.participantesSelecionadosListBox.SelectedItem);
                this.setHasPendingChanges();
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            output.Write("<hr/>");
            output.Write("<table cellspacing=\"20px\">");
            output.Write("\t<tr>");
            output.Write("\t\t<td style=\"margin:15px\">");
            output.Write("\t\t\t<div><b>Participantes disponibles</b></div>");

            participantesDisponiblesListBox.RenderControl(output);

            output.Write("\t\t</td>");

            output.Write("\t\t<td style=\"margin:20px\">");

            añadirParticipanteButton.RenderControl(output);
            
            output.Write("\t\t</td>");

            output.Write("\t\t<td style=\"margin:15px\">");
            output.Write("\t\t\t<div><b>Participantes seleccionados <span style=\"color:red\">*</span></b></div>");

            participantesSelecionadosListBox.RenderControl(output);
            
            output.Write("\t\t</td>");

            output.Write("\t\t<td style=\"margin:20px\">");
            eliminarParticipanteButton.RenderControl(output);
            output.Write("\t\t</td>");

            output.Write("\t\t<td style=\"margin:15px\">");
            output.Write("\t\t\t<div><b>Observaciones de inscripción</b></div>");

            observacionesInscripcionTextBox.RenderControl(output);

            output.Write("\t\t</td>");
            output.Write("\t</tr>");
            output.Write("</table>");
        }

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
