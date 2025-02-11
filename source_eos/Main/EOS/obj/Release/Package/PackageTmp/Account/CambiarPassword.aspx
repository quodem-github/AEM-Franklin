<%@ Page Title="" Language="C#" MasterPageFile="~/Styles/EOS.Master" AutoEventWireup="true"
    CodeBehind="CambiarPassword.aspx.cs" Inherits="EOS.Account.CambiarPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="eosHeaderInfoBasica" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="eosHeaderBotonera" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="eosHeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="eosContentStatus" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="eosContentFilter" runat="server">
    <div class="eosTituloWizard">Cambiar Contraseña</div>
    <div>
        <fieldset id="eosContentLogin">
            <div id="eosContentLoginDatos">
                <table class="eosTablaFiltros" width="550px;">
                    <tr>
                        <td width="170px">
                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Usuario<span class="eosCampoObligatorio">*</span></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="UserName" runat="server" MaxLength="80" CssClass="eosInputVacio eosSizeW190"
                                autocomplete="off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                CssClass="failureNotification" ErrorMessage="Debe introducir usuario para enviar su contraseña."
                                ToolTip="El usuario es obligatorio." ValidationGroup="LoginUserValidationGroup">&nbsp;</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Contraseña<span class="eosCampoObligatorio">*</span></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="Password" runat="server" CssClass="eosInputVacio eosSizeW190" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                CssClass="failureNotification" ErrorMessage="La contraseña es obligatoria." ToolTip="La contraseña es obligatoria."
                                ValidationGroup="LoginUserValidationGroup">&nbsp;</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="NuevaPasswordLabel" runat="server" AssociatedControlID="NuevaPassword">Nueva contraseña<span class="eosCampoObligatorio">*</span></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="NuevaPassword" runat="server" CssClass="eosInputVacio eosSizeW190"
                                TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NuevaPasswordRequired" runat="server" ControlToValidate="NuevaPassword"
                                CssClass="failureNotification" ErrorMessage="La nueva contraseña es obligatoria."
                                ToolTip="Debe introducir la nueva contraseña." ValidationGroup="LoginUserValidationGroup">&nbsp;</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="RepetirPasswordLabel" runat="server" AssociatedControlID="RepetirPassword">Confirmar nueva contraseña<span class="eosCampoObligatorio">*</span></asp:Label>
                        </td>
                        <td width="220px">
                            <asp:TextBox ID="RepetirPassword" runat="server" CssClass="eosInputVacio eosSizeW190"
                                TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RepetirPasswordRequired" runat="server" ControlToValidate="RepetirPassword"
                                CssClass="failureNotification" Text="*" ErrorMessage="La contraseña es obligatoria." ToolTip="Debe repetir la nueva contraseña."
                                ValidationGroup="LoginUserValidationGroup">&nbsp;</asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="ComparePasswords" runat="server" ControlToValidate="RepetirPassword"
                                ControlToCompare="NuevaPassword" Text="*" ErrorMessage="Las contraseñas no coinciden."></asp:CompareValidator>
                        </td>
                        <td>
                                <asp:ImageButton ID="ImageButton2" runat="server" CommandName="Login" ImageUrl="~/Styles/images/bt_cambiar_contrasena.png"
                                ValidationGroup="LoginUserValidationGroup" onclick="ImageButton2_Click" />
                        </td>
                    </tr>
                    <tr><td colspan="3"><br />
                            <asp:ImageButton ID="CancelarImageButton" runat="server" CommandName="Login" ImageUrl="~/Styles/images/bt_volver.png"
                                OnClientClick="javascript:history.back();return false;"/>
                        </td></tr>
                </table>
            </div>
            <div id="eosContentLoginErrores" style="float:left;width:300px;">
                <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification"
                    ValidationGroup="LoginUserValidationGroup" />
                <span class="failureNotification">
                    <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                </span>
            </div>
        </fieldset>
    </div>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="eosContentResults" runat="server">
</asp:Content>
