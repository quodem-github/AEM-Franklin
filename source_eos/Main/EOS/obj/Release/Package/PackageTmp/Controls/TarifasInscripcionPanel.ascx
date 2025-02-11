<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TarifasInscripcionPanel.ascx.cs"
    Inherits="EOS.Controls.TarifasInscripcionPanel" %>
<%@ Register TagPrefix="eosc" TagName="TarifasInscripcionListViewControl" Src="~/Controls/TarifasInscripcionListViewControl.ascx" %>
<%@ Register TagPrefix="eosc" TagName="TarifasManualInputPanel" Src="~/Controls/TarifasManualInputPanel.ascx" %>
<%@ Register TagPrefix="eosc" TagName="AlternativasListViewControl" Src="~/Controls/AlternativasListViewControl.ascx" %>
<%@ Register TagPrefix="eosc" TagName="ParticipantesSelect" Src="~/Controls/ParticipantesSelect.ascx" %>


<div id="eosFilterHeaderEstado" style="position: absolute; top: -120px; left: 700px">
    <span style="float: left;">ESTADO:&nbsp;</span>
    <asp:Label ID="lblEstado" runat="server" Text=""></asp:Label>
</div>
<fieldset class="eos">
    
    <eosc:TarifasInscripcionListViewControl runat="server" ID="eoscTarifasInscripcionListView" />
    <eosc:TarifasManualInputPanel runat="server" ID="eoscTarifasManualInputPanel" Visible="True" />
    <eosc:ParticipantesSelect ID="Test" runat="server" />


    <div class="eosBotonera eosBotonFiltrado"> 
        <asp:Button ID="btnGuardar" runat="server" text="Guardar"
            Style="margin-right: 15px;" OnClientClick="ConfirmaGuardar();" ClientIDMode="Static"
            OnClick="btnGuardar_Click" Visible="False" />
        <asp:Button ID="btnEnviar" runat="server" text="Enviar" 
            Style="margin-right: 15px;" OnClick="btnEnviar_Click" OnClientClick="ConfirmaEnvio();"
            Visible="False" ClientIDMode="Static" />
        <asp:Button ID="btnAprobar" runat="server" text="Aprobar"  
            Style="margin-right: 15px;" OnClick="btnAprobar_Click" OnClientClick="ConfirmaAprobar();"
            Visible="False" ClientIDMode="Static" />
        <asp:Button ID="btnRechazar" runat="server" text="Rechazar" 
            Style="margin-right: 15px;" OnClick="btnRechazar_Click" OnClientClick="ConfirmaRechazar();"
            Visible="False" ClientIDMode="Static" />
        <asp:Button ID="btnCancelar" runat="server" text="Cancelar" 
            Style="margin-right: 15px;" OnClick="btnCancelar_Click" OnClientClick="ConfirmaCancelacion();"
            Visible="False" ClientIDMode="Static" />
    </div>


    <eosc:AlternativasListViewControl runat="server" ID="Alternativas" TipoServicio="0" />
</fieldset>
