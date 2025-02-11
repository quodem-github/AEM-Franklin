<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BotoneraPorEstados.ascx.cs" Inherits="EOS.Controls.BotoneraPorEstados" %>

<asp:ImageButton ID="enviarImageButton" runat="server" 
    ImageUrl="~/Styles/images/bt_enviar.png" CommandName="Enviar" OnCommand="ImageButton_Command"/>
<asp:ImageButton ID="modificarImageButton" runat="server" 
    ImageUrl="~/Styles/images/bt_modificar.png" CommandName="Modificar" OnCommand="ImageButton_Command" Visible="false" />
<asp:ImageButton ID="aprobarImageButton" runat="server" 
    ImageUrl="~/Styles/images/bt_aprobar.png" CommandName="Aprobar" OnCommand="ImageButton_Command" Visible="false" />
<asp:ImageButton ID="cancelarImageButton" runat="server" 
    ImageUrl="~/Styles/images/bt_cancelar.png" CommandName="Cancelar" OnCommand="ImageButton_Command" Visible="false" />
