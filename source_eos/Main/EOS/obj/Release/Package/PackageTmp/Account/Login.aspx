<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Styles/EOS.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="EOS.Account.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="headContent">
    <style type="text/css">
        .butClicked {
            background-color: #C8EAEB;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("div#eosHeaderPersonal").hide();
        });
    </script>
</asp:Content> 
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="eosHeaderInfoBasica">
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="eosHeaderBotonera">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="eosContentFilter">
    <asp:PlaceHolder runat="server" ID="plhAgencyLogin" Visible="false">
        <script type="text/javascript">
            $(document).ready(function() {
                $('#eosContentMain').addClass('agencyLogin');
                $('body').addClass('agencyLogin');
            });
        </script>


                    <div class="container body-content loginContent">
                    <div class="elementLoginMid ">


        <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false"
            FailureText="La combinación de usuario y contraseña no son válidos" OnLoggedIn="LoginUser_LoggedIn">



                        <LayoutTemplate>
                            <div id="eosAccountInfo">
                                <fieldset id="eosContentLogin">
                                    <div id="eosContentLoginDatos">
                           <h1>Iniciar sesión</h1>

                                        <div class="eosContentLoginDatosElement">
                                      
                                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Usuario:</asp:Label>
                                       

                                                    <asp:TextBox ID="UserName" runat="server" MaxLength="80" CssClass="eosInputVacio eosSizeW190"
                                                        autocomplete="off"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                        CssClass="failureNotification" ErrorMessage="El usuario es obligatorio." ToolTip="El usuario es obligatorio."
                                                        ValidationGroup="LoginUserValidationGroup">&nbsp;</asp:RequiredFieldValidator>
                                      </div>
                                                                    <div class="eosContentLoginDatosElement">

                                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Contraseña:</asp:Label>
                                             


                                                    <asp:TextBox ID="Password" runat="server" CssClass="eosInputVacio eosSizeW190" TextMode="Password"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                        CssClass="failureNotification" ErrorMessage="La contraseña es obligatoria." ToolTip="La contraseña es obligatoria."
                                                        ValidationGroup="LoginUserValidationGroup">&nbsp;</asp:RequiredFieldValidator>

                                      </div>
                                        <div class="eosBotonera eosBotonFiltrado">

                                                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" value="Acceder" ImageUrl=""
                                                        ValidationGroup="LoginUserValidationGroup" Text="Acceder" />
                                      </div>

                                     
                                     

                                    </div>
                                    <div id="eosContentLoginErrores">
                                        <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification"
                                            ValidationGroup="LoginUserValidationGroup" />
                                        <span class="failureNotification">
                                            <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                                        </span>
                                    </div>
                                </fieldset>
                            </div>
                        </LayoutTemplate>




        </asp:Login>                 </div>



                <div class="elementLoginMid rightLogin">
        </div>


     </div>

    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="plhMsdLogin" Visible="false">
        <script type="text/javascript">
            $(document).ready(function() {
                $('#eosContentMain').addClass('samlLogin');
            });
        </script>
        <div class="loginBox">
            <div class="contentLoginBox">
                <div class="contentLoginBox_top">
                </div>
                <div class="contentLoginBox_bot">
                    <asp:Button ID="IdTest" runat="server" Text="Iniciar sesión" OnClick="golog" CssClass="but" OnClientClick="changeButtonText();" />
                </div>
            </div>
        </div>
        <script type="text/javascript">
            function changeButtonText(but) {
                $('input.but').val('Entrando...');
                $('input.but').addClass('butClicked');
            }
                </script>
    </asp:PlaceHolder>
</asp:Content>
