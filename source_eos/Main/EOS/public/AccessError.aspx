<%@ Page Title="Generic Error Page" Language="C#" MasterPageFile="~/Styles/EOSError.master" AutoEventWireup="true" %>

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
                        <asp:Label runat="server" CssClass="eosTablaError" Text="Acceso no autorizado"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>&nbsp;</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label runat="server" CssClass="eosTablaError2" Text="No tiene autorización para acceder a esta aplicación. Por favor, contacte con el administrador para solicitar su registro."></asp:Label>
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
            </asp:Table>
        </fieldset>
    </div>
</asp:Content>