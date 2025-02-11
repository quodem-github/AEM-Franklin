<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TarifasManualInputPanel.ascx.cs" Inherits="EOS.Controls.TarifasManualInputPanel" %>




<asp:Panel ID="ManualInputPanel" runat="server"> 
<div class="eosTablaFiltros">

        <div class="contentFormGeneric ">
                        <div class="contentForm eosMitadColumna">
                <label for="eosContentResults_InscripcionTabContainer_inscripcionTabPanel_tipoTextBox">Tipo: <span class="eosCampoObligatorio">*</span></label><asp:TextBox 
                  ID="tipoTextBox" runat="server" Enabled="false" 
                    CssClass="eosDisabledInputVacio style1"></asp:TextBox>
                                </div>
                                            <div class="contentForm eosMitadColumna">
                <label for="eosContentResults_InscripcionTabContainer_inscripcionTabPanel_importeTextBox">Importe:</label><asp:TextBox 
                  ID="importeTextBox" runat="server" Enabled="false" CssClass="eosDisabledInputVacio style1"></asp:TextBox>
            </div>
        </div>
    </div>
</asp:Panel>