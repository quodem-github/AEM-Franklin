<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BotoneraPanelBase.ascx.cs" Inherits="EOS.Controls.BotoneraPanelBase" %>

        <div class="eosDivCentrada" style="margin-top: 10px">
      <asp:ImageButton ID="btnGuardar" runat="server" ImageUrl="~/Styles/images/bt_guardar.png"
        Style="margin-right: 15px;" OnClick="btnGuardar_Click" />
      <asp:ImageButton ID="btnNuevo" runat="server" ImageUrl="~/Styles/images/bt_nuevo.png"
        Style="margin-right: 5px;" OnClick="btnNuevo_Click" Visible="false" />
     <asp:ImageButton ID="btnEnviar" runat="server" ImageUrl="~/Styles/images/bt_enviar.png"
        Style="margin-right: 15px;" OnClick="btnEnviar_Click" OnClientClick="return ConfirmaEnvio();" />
      <asp:ImageButton ID="btnAprobar" runat="server" ImageUrl="~/Styles/images/bt_aprobar.png"
        Style="margin-right: 15px;" OnClick="btnAprobar_Click" 
            OnClientClick="return ConfirmaAprobar();"   />
      <asp:ImageButton ID="btnRechazar" runat="server" ImageUrl="~/Styles/images/bt_rechazar.png"
        Style="margin-right: 15px;" OnClick="btnRechazar_Click" 
            OnClientClick="return ConfirmaRechazar();"  />
      <asp:ImageButton ID="btnCancelar" runat="server" ImageUrl="~/Styles/images/bt_cancelar.png"
        Style="margin-right: 15px;" OnClick="btnCancelar_Click" OnClientClick="return ConfirmaCancelacion();" />
<%--            <asp:ImageButton ID="btnNueva" runat="server" ImageUrl="~/Styles/images/bt_nueva_combinacion.png"
                Style="margin-right: 15px; margin-left: 15px;"  
                OnClick="btnNueva_Click" />--%>
        </div>
