<%@ Page Title="" Language="C#" MasterPageFile="~/Styles/EOS.Master" AutoEventWireup="true"
    CodeBehind="CambiarDatosPersonales.aspx.cs" Inherits="EOS.CambiarDatosPersonales" %>


<asp:Content ID="Content2" ContentPlaceHolderID="eosHeaderBotonera" runat="server">
    &nbsp;
<asp:ScriptManager ID="MainScriptManager" runat="server" EnableScriptGlobalization="True"
        EnablePartialRendering="true">
    </asp:ScriptManager>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="eosContentFilter" runat="server">
    <div class="eosTituloWizard">Cambiar Datos Personales</div>
    <div>
        <fieldset id="eosContentLogin">
            <div style="float:left;width:700px;">
                <div class="eosTablaFiltros">
                    <div>
                        <div>
                            <asp:Label ID="NombreLabel" runat="server" AssociatedControlID="Nombre">Nombre<span class="eosCampoObligatorio">*</span></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="Nombre" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                                autocomplete="off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="NombreRequired" runat="server" ControlToValidate="Nombre"
                                CssClass="failureNotification" Text="*" ErrorMessage="El nombre es obligatorio."
                                ToolTip="El nombre es obligatorio." ValidationGroup="LoginUserValidationGroup">&nbsp;</asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Contraseña<span class="eosCampoObligatorio">*</span></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="Password" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequired" Text="*" runat="server" ControlToValidate="Password"
                                CssClass="failureNotification" ErrorMessage="La contraseña es obligatoria." ToolTip="La contraseña es obligatoria."
                                ValidationGroup="LoginUserValidationGroup">&nbsp;</asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div>
                        <div>
                            <asp:Label ID="PrimerApellidoLabel" runat="server" AssociatedControlID="PrimerApellido">Primer apellido<span class="eosCampoObligatorio">*</span></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="PrimerApellido" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                                autocomplete="off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PrimerApellidoRequired" runat="server" ControlToValidate="PrimerApellido"
                                CssClass="failureNotification" Text="*" ErrorMessage="El primer apellido es obligatorio."
                                ToolTip="El primer apellido es obligatorio." ValidationGroup="LoginUserValidationGroup">&nbsp;</asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <asp:Label ID="RepetirPasswordLabel" runat="server" AssociatedControlID="RepetirPassword">Repetir contraseña<span class="eosCampoObligatorio">*</span></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="RepetirPassword" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                                TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RepetirPasswordRequired" runat="server" ControlToValidate="RepetirPassword"
                                CssClass="failureNotification" Text="*" ErrorMessage="Debe repetir la contraseña."
                                ToolTip="Debe repetir la contraseña." ValidationGroup="LoginUserValidationGroup">&nbsp;</asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="RepetirPasswordCompare" Text="*" runat="server" ControlToValidate="RepetirPassword"
                                ControlToCompare="Password" ErrorMessage="Las contraseñas no coinciden." ValidationGroup="LoginUserValidationGroup"></asp:CompareValidator>
                        </div>
                    </div>
                    <div>
                        <div>
                            <asp:Label ID="SegundoApellidoLabel" runat="server" AssociatedControlID="SegundoApellido">Segundo apellido</asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="SegundoApellido" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                                autocomplete="off"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Label ID="DireccionLabel" runat="server" AssociatedControlID="Direccion">Dirección</asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="Direccion" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                                autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div>
                        <div>
                            <asp:Label ID="TelefonoLabel" runat="server" AssociatedControlID="Telefono">Teléfono</asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="Telefono" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                                autocomplete="off"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Label ID="CodigoPostalLabel" runat="server" AssociatedControlID="CodigoPostal">Código postal</asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="CodigoPostal" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                                autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div>
                        <div>
                            <asp:Label ID="ExtensionLabel" runat="server" AssociatedControlID="Extension">Extensión</asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="Extension" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                                autocomplete="off"></asp:TextBox>
                        </div>
                        <div>
                            <asp:Label ID="PoblacionLabel" runat="server" AssociatedControlID="ddlPoblacion">Población</asp:Label>
                        </div>
                        <div>
                            <asp:DropDownList ID="ddlPoblacion" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190" 
                                DataTextField="Poblacion" DataValueField="IDPoblacion" AutoPostBack="True" 
                                onselectedindexchanged="ddlPoblacion_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div>
                        <div>
                            <asp:Label ID="MovilLabel" runat="server" AssociatedControlID="Nombre">Móvil<span class="eosCampoObligatorio">*</span></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="Movil" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                                autocomplete="off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="MovilRequired" runat="server" ControlToValidate="Movil"
                                CssClass="failureNotification" Text="*" ErrorMessage="El móvil es obligatorio."
                                ToolTip="El móvil es obligatorio." ValidationGroup="LoginUserValidationGroup">&nbsp;</asp:RequiredFieldValidator>
                        </div>
                        <div>
                            <asp:Label ID="ProvinciaLabel" runat="server" AssociatedControlID="Provincia">Provincia<span class="eosCampoObligatorio">*</span></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="Provincia" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                                autocomplete="off" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                    <div>
                        <div>
                            <asp:Label ID="CorreoElectronicoLabel" runat="server" AssociatedControlID="CorreoElectronico">Correo electrónico<span class="eosCampoObligatorio">*</span></asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="CorreoElectronico" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                                autocomplete="off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="CorreoElectronicoRequired" runat="server" ControlToValidate="CorreoElectronico"
                                CssClass="failureNotification" Text="*" ErrorMessage="El correo electrónico es obligatorio."
                                ToolTip="El correo electrónico es obligatorio." ValidationGroup="LoginUserValidationGroup">&nbsp;</asp:RequiredFieldValidator>
                        </div>
                         <div>
                            <asp:Label ID="PaisLabel" runat="server" AssociatedControlID="Pais">Pais</asp:Label>
                        </div>
                        <div>
                            <asp:TextBox ID="Pais" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                                autocomplete="off" ReadOnly="True"></asp:TextBox>
                        </div>
                    </div>
                    <div>
                        <div>
                            <%--<asp:Label ID="Label1" runat="server">Amex</asp:Label>--%>
                        </div>
                        <div>
                            <asp:Panel runat ="server"  Visible="false">
                                <asp:TextBox ID="TxtAmex" runat="server" MaxLength="20" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                                    autocomplete="off"></asp:TextBox>
                             </asp:Panel>
                        </div>
                         <div>
                            <%--<asp:Label ID="Label2" runat="server">Fecha de caducidad</asp:Label>--%>
                        </div>
                        <div>  
                            <asp:Panel runat="server" Visible="false">
                                <eos:InputDatePickerControl ID="FechaCaducidadDatePickerControl" runat="server" />    
                            </asp:Panel>                                         
                        </div>
                        <div>
                            <asp:ImageButton ID="AceptarImageButton" runat="server" CommandName="Login" ImageUrl="~/Styles/images/bt_guardar_cambios.png"
                                ValidationGroup="LoginUserValidationGroup" 
                                onclick="AceptarImageButton_Click"  />
                        </div>
                        <div>
                            <asp:ImageButton ID="CancelarImageButton" runat="server" CommandName="Login" ImageUrl="~/Styles/images/bt_volver.png"
                                OnClientClick="javascript:history.back();return false;"/>
                        </div>
                    </div>
                       
                      
                    </div>
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

