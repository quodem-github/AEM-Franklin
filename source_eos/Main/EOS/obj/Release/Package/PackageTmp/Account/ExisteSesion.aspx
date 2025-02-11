<%@ Page Title="Control de sesión" Language="C#" MasterPageFile="~/Styles/EOS.master" AutoEventWireup="true" CodeBehind="ExisteSesion.aspx.cs" Inherits="EOS.Account.ExisteSesion" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="headContent"></asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="eosHeaderInfoBasica"></asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="eosHeaderBotonera"></asp:Content>



<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="eosContentFilter">
         <script type="text/javascript">
             $(document).ready(function () {
                 $('#eosContentMain').addClass('agencyLogin');
                 $('body').addClass('agencyLogin');
             });
         </script>






                        <div class="container body-content loginContent">

                    <div class="elementLoginMid ">
                         
        <fieldset id="eosContentLogin">

               <h1>Control de sesión</h1>
            <asp:Table runat="server" Width="100%" Height="100%">
                <asp:TableRow>
                    <asp:TableCell >
                       <h2><asp:Label runat="server" CssClass="eosTablaError" Text="Existe una sesión abierta"></asp:Label></h2>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>&nbsp;</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label runat="server" CssClass="eosTablaError" Text="Para iniciar una nueva sesión debe cerrar primero la sesión abierta"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>&nbsp;</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>&nbsp;</asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </fieldset>

    </div>



             <div class="elementLoginMid rightLogin">
        </div>


     </div>


</asp:Content>
