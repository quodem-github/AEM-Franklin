<%@ Page Title="" Language="C#" MasterPageFile="~/Styles/EOS.Master" AutoEventWireup="true"
    CodeBehind="RecuperaPassword.aspx.cs" Inherits="EOS.Account.RecuperaPassword" %>

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
    <asp:HiddenField runat="server" ID="urlHost"/>
    <asp:PasswordRecovery ID="ctrlPasswordRecovery" runat="server" BorderStyle="None"
        BorderWidth="0" SubmitButtonText="Send" UserNameFailureText="El sistema no puede enviar la contraseña, por favor póngase en contacto con el Administrador para solventar el problema"
        OnSendingMail="ctrlPasswordRecovery_SendingMail">
        <MailDefinition Subject="Contraseña EOS" 
            BodyFileName="~/Account/CambiarPasswordMail.htm"  IsBodyHtml="true" >
        </MailDefinition>
        <UserNameTemplate>
            <div class="eosTituloWizard">
                Crear Nueva Contraseña</div>
            <div>
                <fieldset id="eosContentLogin">
                    <div id="eosContentLoginDatos">
                        <table class="eosTablaFiltros">
                            <tr>
                                <td colspan="2">
                                    <asp:Literal ID="UserNameInstruction" runat="server" Text="Escriba su usuario para recibir una nueva contraseña" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="eosCampoLabelSin" for="UserName">
                                        Usuario<span class="eosCampoObligatorio">*</span></label>
                                </td>
                                <td align="right">
                                    <asp:TextBox ID="UserName" runat="server" autocomplete="off" CssClass="eosSizeW210" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe introducir un usuario"
                                        ControlToValidate="UserName" ValidationGroup="LoginUserValidationGroup" SetFocusOnError="True"
                                        ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:ImageButton ID="SubmitButton" ValidationGroup="LoginUserValidationGroup" CommandName="Submit"
                                        runat="server" ImageUrl="~/Styles/images/bt_enviar.png" />&nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="CancelarImageButton" runat="server" CommandName="Login" ImageUrl="~/Styles/images/bt_volver.png"
                                        OnClientClick="javascript:history.back();return false;" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="eosContentLoginErrores" style="float: left; width: 300px;">
                        <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification"
                            ValidationGroup="LoginUserValidationGroup" />
                        <span class="failureNotification">
                            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                            <asp:Literal ID="FailureText" runat="server" />
                        </span>
                    </div>
                </fieldset>
            </div>
        </UserNameTemplate>
        <SuccessTemplate>
            <div class="eosTituloWizard">
                Recuperar Contraseña</div>
            <div id="eosContentLogin">
                <p>
                    En breve recibirá un mensaje a su cuenta de correo con los datos solicitados</p>
                <p>
                    <asp:ImageButton ID="btnVolver" runat="server" ImageUrl="~/Styles/images/bt_volver.png"
                        PostBackUrl="~/Account/Login.aspx" />
                </p>
            </div>
        </SuccessTemplate>
    </asp:PasswordRecovery>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="eosContentResults" runat="server">
</asp:Content>
