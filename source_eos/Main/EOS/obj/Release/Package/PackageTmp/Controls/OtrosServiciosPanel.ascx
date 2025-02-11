<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OtrosServiciosPanel.ascx.cs"
    Inherits="EOS.Controls.OtrosServiciosPanel" %>
<%@ Register TagPrefix="eosc" TagName="TarifasOtrosServiciosListViewControl" Src="~/Controls/TarifasOtrosServiciosListViewControl.ascx" %>
<%@ Register TagPrefix="eosc" TagName="AlternativasListViewControl" Src="~/Controls/AlternativasListViewControl.ascx" %>
<%@ Register TagPrefix="eosc" TagName="ParticipantesSelect" Src="~/Controls/ParticipantesSelect.ascx" %>

<div id="eosFilterHeaderEstado" style="position: absolute; top: -120px; left: 700px">
    <span style="float: left;">ESTADO:&nbsp;</span>
    <asp:Label ID="lblEstado" runat="server" Text="Label"></asp:Label>
</div>
<div class="contentTableGeneric space-under">
    <eosc:TarifasOtrosServiciosListViewControl runat="server" ID="eoscTarifasOtrosServiciosListView" />
</div>
<fieldset class="eos eosTablaFiltros">
        <div class="contentFormGeneric">
            <div class="contentForm eosUnoColumna">
                <asp:Label runat="server" AssociatedControlID="descripcionTextBox">Descripcion<span class="eosCampoObligatorio">*</span></asp:Label>
                <asp:TextBox ID="descripcionTextBox" runat="server" Width="55%" Enabled="false" CssClass="eosDisabledInputVacio style1"></asp:TextBox>
            </div>
        </div>
        <div class="contentFormGeneric">
            <div class="contentForm eosTresColumna ">
                <asp:Label runat="server" AssociatedControlID="tipoTextBox">Tipo</asp:Label>
                <asp:TextBox ID="tipoTextBox" runat="server" Enabled="false" CssClass="eosDisabledInputVacio style1"></asp:TextBox>
            </div>
            <div class="contentForm eosTresColumna ">
                <asp:Label runat="server" AssociatedControlID="sedeTextBox">Sede</asp:Label>
                <asp:TextBox ID="sedeTextBox" runat="server" Enabled="false" CssClass="eosDisabledInputVacio style1"></asp:TextBox>
            </div>
            <div class="contentForm eosTresColumna ">
                <asp:Label runat="server" AssociatedControlID="importeTextBox">Importe Máximo</asp:Label>
                <asp:TextBox ID="importeTextBox" runat="server" Enabled="false" CssClass="eosDisabledInputVacio style1"></asp:TextBox>
            </div>    
        </div>
        <div class="contentFormGeneric">
            <div class="contentForm eosMitadColumna calendarContentSpan">
                <eos:InputDatePickerControl ID="fechaInicioInputDatePickerControl" runat="server"
    Text="Fecha inicio" Enabled="false" CssClass="aspNetDisabled" />
            </div>
            <div class="contentForm eosMitadColumna">
                <eos:TimeInputBox ID="horaInicioTimeInputBox" CssClass="hour-col" runat="server" Text="Hora" Enabled="false"
    Value="" />
            </div>
            <div class="contentForm eosMitadColumna calendarContentSpan no-padding-left space-up">
                <eos:InputDatePickerControl ID="fechaFinalInputDatePickerControl" Text="Fecha final"
    runat="server" Enabled="false" />
            </div>
            <div class="contentForm eosMitadColumna space-up">
                <eos:TimeInputBox ID="horaFinalTimeInputBox" CssClass="hour-col" runat="server" Text="Hora" Enabled="false" />
            </div>            
        </div>
        <div class="contentSelectW">
            <eosc:ParticipantesSelect ID="Test" runat="server" />
        </div>
        <div class="eosBotonera eosBotonFiltrado" style="margin-top: 15px">
            <asp:Button ID="btnGuardar" runat="server"
                OnClick="btnGuardar_Click" Visible="False" OnClientClick="ConfirmaGuardar();" ClientIDMode="Static" Text="Guardar"/>
            <asp:Button ID="btnEnviar" runat="server"
                OnClick="btnEnviar_Click" Visible="False" OnClientClick="ConfirmaEnvio();" ClientIDMode="Static" Text="Enviar"/>
            <asp:Button ID="btnAprobar" runat="server"
                OnClick="btnAprobar_Click" OnClientClick="ConfirmaAprobar();" ClientIDMode="Static"
                Visible="False" Text="Aprobar"/>
            <asp:Button ID="btnRechazar" runat="server"
                OnClick="btnRechazar_Click" OnClientClick="ConfirmaRechazar();" ClientIDMode="Static"
                Visible="False" Text="Rechazar"/>
            <asp:Button ID="btnCancelar" runat="server" ClientIDMode="Static"
                OnClientClick="ConfirmaCancelacion();" Visible="False" Text="Cancelar"/>
        </div>
    <eosc:AlternativasListViewControl runat="server" ID="Alternativas" TipoServicio="3" />
    
</fieldset>
