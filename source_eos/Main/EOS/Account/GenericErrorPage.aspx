<%@ Page Title="Generic Error Page" Language="C#" MasterPageFile="~/Styles/EOSError.master" AutoEventWireup="true" CodeBehind="GenericErrorPage.aspx.cs" Inherits="EOS.Account.GenericErrorPage" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="headContent"></asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="eosHeaderInfoBasica"></asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="eosHeaderBotonera"></asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="eosContentFilter">
        
    <div class="eosTituloWizard">Error</div>
    <div>
        <fieldset id="eosContentLogin">
            <asp:Table runat="server" Width="100%" Height="100%">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Label runat="server" CssClass="eosTablaError" Text="Un error inesperado ha occurido"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>&nbsp;</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center">
                        <asp:Label runat="server" CssClass="eosTablaError" Text="Si el error persiste por favor contacte el administrador"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>&nbsp;</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label id="lbmensaje" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>&nbsp;</asp:TableCell>
                </asp:TableRow> 
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center"><asp:Button runat="server"  CssClass="btnVolver" ID="btn_Volver" OnClick="btn_Volver_Click" Text="Volver" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>&nbsp;</asp:TableCell>
                </asp:TableRow>
            </asp:Table> 
        </fieldset>
    </div>
</asp:Content>
