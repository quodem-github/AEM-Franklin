<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="msg.aspx.cs" Inherits="EOS.Account.msg" MasterPageFile="~/Styles/EOS.master"%>


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="headContent">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="eosHeaderInfoBasica">
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="eosHeaderBotonera">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="eosContentFilter">
    
    <div class="dv">
        <p style="font-family: arial; font-size: 25px; color: darkgreen;"><asp:Literal Id="lit" runat="server">...</asp:Literal></p>
    </div>

</asp:Content>
