<%@ Page Title="" Language="C#" MasterPageFile="~/Styles/EOS.Master" AutoEventWireup="true"
    CodeBehind="CambiarDatosCompliance.aspx.cs" Inherits="EOS.CambiarDatosCompliance" %>

<asp:Content ID="Content2" ContentPlaceHolderID="eosHeaderBotonera" runat="server">
&nbsp;
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="eosContentFilter" runat="server">
    <div class="eosTituloWizard">Cambiar Datos Compliance</div>
    <div>
        <fieldset id="eosContentLogin">
            <div style="float:left;width:700px;">
                <table class="eosTablaFiltros" width="900px;">
                    <tr>
                        <td width="115px">
                            <asp:Label ID="MailFromLabel" runat="server" AssociatedControlID="MailFrom">Mail From<span class="eosCampoObligatorio">*</span></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="MailFrom" runat="server" MaxLength="255" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                                autocomplete="off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="MailFromRequired" runat="server" ControlToValidate="MailFrom"
                                CssClass="failureNotification" Text="*" ErrorMessage="El Mail From es obligatorio."
                                ToolTip="El Mail From." ValidationGroup="LoginUserValidationGroup">&nbsp;</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="MailToLabel" runat="server" AssociatedControlID="MailTo">Mail To<span class="eosCampoObligatorio">*</span></asp:Label>
                        </td>
                        <td width="240px">
                            <asp:TextBox ID="MailTo" MaxLength="255" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="MailToRequired" Text="*" runat="server" ControlToValidate="MailTo"
                                CssClass="failureNotification" ErrorMessage="El Mail To es obligatorio." ToolTip="El Mail To es obligatoria"
                                ValidationGroup="LoginUserValidationGroup">&nbsp;</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="MailCCLabel" runat="server" AssociatedControlID="MailCC">Mail CC</asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="MailCC" runat="server" MaxLength="255" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                                autocomplete="off"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="MailToRsptaLabel" runat="server" AssociatedControlID="MailToRspta">Mail To Respuesta</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="MailToRspta" runat="server" MaxLength="255" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="FormatoLabel" runat="server" AssociatedControlID="Formato">Formato Programa</asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="Formato" runat="server" MaxLength="255" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                                autocomplete="off"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="AsuntoLabel" runat="server" AssociatedControlID="Asunto">Asunto<span class="eosCampoObligatorio">*</span></asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="Asunto" runat="server" MaxLength="255" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW550"
                                autocomplete="off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="AsuntoRequired" Text="*" runat="server" ControlToValidate="Asunto"
                                CssClass="failureNotification" ErrorMessage="El Asunto es obligatorio" ToolTip="El Asunto es obligatorio"
                                ValidationGroup="LoginUserValidationGroup">&nbsp;</asp:RequiredFieldValidator>
                        </td>                        
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="CuerpoLabel" runat="server" AssociatedControlID="Cuerpo">Cuerpo del Mensaje<span class="eosCampoObligatorio">*</span></asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="Cuerpo" runat="server" MaxLength="2000" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW550" TextMode="MultiLine" Rows="5"
                                autocomplete="off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="CuerpoRequired" Text="*" runat="server" ControlToValidate="Cuerpo"
                                CssClass="failureNotification" ErrorMessage="El Cuerpo del Mensaje es obligatorio" ToolTip="El Cuerpo del Mensaje es obligatorio"
                                ValidationGroup="LoginUserValidationGroup">&nbsp;</asp:RequiredFieldValidator>
                        </td>
                        <td valign="bottom">
                            <asp:ImageButton ID="AceptarImageButton" runat="server" CommandName="Login" ImageUrl="~/Styles/images/bt_guardar_cambios.png"
                                ValidationGroup="LoginUserValidationGroup" 
                                onclick="AceptarImageButton_Click"  />
                        </td>
                        <td valign="bottom">
                            <asp:ImageButton ID="CancelarImageButton" runat="server" CommandName="Login" ImageUrl="~/Styles/images/bt_cancelar.png"
                                OnClientClick="javascript:history.back();return false;"/>
                        </td>
                    </tr>
                    </table>
            </div>
            <div id="eosContentLoginErrores" style="float:left;width:200px;">
                <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification"
                    ValidationGroup="LoginUserValidationGroup" />
                <span class="failureNotification">
                    <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                </span>
            </div>
        </fieldset>
    </div>
</asp:Content>

