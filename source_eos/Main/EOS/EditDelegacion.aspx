<%@ Page Title="Delegaciones de Aprobación" Language="C#" MasterPageFile="~/Styles/EOS.Master" AutoEventWireup="true" CodeBehind="EditDelegacion.aspx.cs" Inherits="EOS.EditDelegacion" EnableEventValidation="true" Culture="es-ES" UICulture="es"%>



<%--<asp:Content ID="Content2" ContentPlaceHolderID="eosHeaderBotonera" runat="server">
    <div id="eosHPBotones">
        <div class="eosBotonera eosBotonInformes"><a title="Acceder a Informes y Listados" onmouseout="javascript:window.status='';return true;" onmouseover="javascript:window.status='Acceder a Informes y Listados';return true;" href="javascript:document.location.href = 'Informes/FiltroInforme.aspx';">Acceso a Informes y Listados</a></div>
        <div class="eosBotonera eosBotonDatosPer"><a title="Acceder a Datos Personales" onmouseout="javascript:window.status='';return true;" onmouseover="javascript:window.status='Cambiar datos personales';return true;" href="javascript:document.location.href = 'CambiarDatosPersonales.aspx';">Acceso a Datos Personales</a></div>
        <div class="eosBotonera eosBotonExpedientes"><a title="Acceder a Listados de Expedientes" onmouseout="javascript:window.status='';return true;" onmouseover="javascript:window.status='Litados de Expedientes';return true;" href="javascript:document.location.href = 'Expedientes.aspx';">Listados de Expedientes</a></div>
    </div>
</asp:Content>--%>


<asp:Content ID="Content3" ContentPlaceHolderID="eosContentFilter" runat="server">	
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
    </act:ToolkitScriptManager>
        <div id="eosContentStatusTitulo" class="eosTituloWizard" runat="server" visible="true">
        Delegaciones de permisos de aprobación</div>
        <div id="eosContentFilter">
        <div id="eosContentAMECErrores">
            <asp:ValidationSummary ID="ContenAMECValidationSummary" runat="server" CssClass="failureNotification"
                ValidationGroup="ValidacionesCamposDelegacion" HeaderText="ATENCIÓN:" />
            <span class="failureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>
        </div>
		        <table class="eosTablaFiltros" id="eosFiltroBasico">			        
                    <tr>
                        <td colspan="8">
                            <span class="eosTituloNormal">RELLENE LOS FILTROS NECESARIOS Y PULSE EL BOTÓN DE FILTRAR</span>
                        </td>
                    </tr>
			        <tr>
                        <td>
                            <label class="eosCampoLabel" for="">Usuario<span class="eosCampoObligatorio">*</span></label>
                    <asp:DropDownList ID="ddlUsuario" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW160" 
                                DataTextField="NombreCompleto" AppendDataBoundItems = "True" DataValueField="IdPeticionario" Visible="true">
                                <asp:ListItem value="">TODOS</asp:ListItem>       
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Usuario Obligatorio" ControlToValidate="ddlUsuario" ValidationGroup="ValidacionesCamposDelegacion" SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                   
                         </td>
                         <td>
                         <label class="eosCampoLabel" for="">Fecha Desde<span class="eosCampoObligatorio">*</span></label>
                    <asp:TextBox ID="txtFechaDesde" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW140"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Fecha Desde Obligatorio" ControlToValidate="txtFechaDesde" ValidationGroup="ValidacionesCamposDelegacion" SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                   <asp:ImageButton ID="btnCalendarDesde" runat="server" 
                                ImageUrl="~/Styles/images/ic_calendario.png" />
                             <act:CalendarExtender ID="CalendarExtender2" TargetControlID="txtFechaDesde" 
                                runat="server" TodaysDateFormat="d MMMM yyyy" FirstDayOfWeek="Monday" 
                                PopupButtonID="btnCalendarDesde" Format="dd/MM/yy" />&nbsp;&nbsp;&nbsp;
                         </td>
                         <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="La Fecha debe tener un formato válido" Type="Date" ControlToValidate="txtFechaDesde" Operator="DataTypeCheck" ValidationGroup="ValidacionesCamposDelegacion"></asp:CompareValidator>
                        
                     </tr>
                     <tr>
                         <td>
                         <label class="eosCampoLabel" for="">Delegado<span class="eosCampoObligatorio">*</span></label>
                    <asp:DropDownList ID="ddlDelegado" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW160" 
                                DataTextField="NombreCompleto" AppendDataBoundItems = "True" DataValueField="IdPeticionario" Visible="true">
                                <asp:ListItem value="">TODOS</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Delegado Obligatorio" ControlToValidate="ddlDelegado" ValidationGroup="ValidacionesCamposDelegacion" SetFocusOnError="True" ToolTip="Campo Obligatorio"></asp:RequiredFieldValidator>
                    
                         </td>
                         <td>
                         <label class="eosCampoLabel" for="">Fecha Hasta<span class="eosCampoObligatorio">*</span></label>
                         <asp:TextBox ID="txtFechaHasta" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW140"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Fecha Hasta Obligatorio" ControlToValidate="txtFechaHasta" ValidationGroup="ValidacionesCamposDelegacion" SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                    <asp:CompareValidator id="CompareValidator1" runat="server" ErrorMessage="La Fecha debe tener un formato válido" Type="Date"
                        ControlToValidate="txtFechaHasta" Operator="DataTypeCheck" ValidationGroup="ValidacionesCamposDelegacion">&nbsp;</asp:CompareValidator>
                    <asp:RangeValidator id="RangeValidator2" runat="server" ErrorMessage="La Fecha no puede ser inferior a hoy" MaximumValue="31/12/99"
                        ControlToValidate="txtFechaHasta" ValidationGroup="ValidacionesCamposDelegacion" Display="Dynamic" Enabled="false">&nbsp;</asp:RangeValidator>

                    <asp:ImageButton ID="btnCalendarHasta" runat="server" 
                                ImageUrl="~/Styles/images/ic_calendario.png" />
                             <act:CalendarExtender ID="CalendarExtender3" TargetControlID="txtFechaHasta" 
                                runat="server" TodaysDateFormat="d MMMM yyyy" FirstDayOfWeek="Monday" 
                                PopupButtonID="btnCalendarHasta" Format="dd/MM/yy" />&nbsp;&nbsp;&nbsp;
                         </td>
                         <td></td>
                    </tr>
                </table>

                <table class="eosTablaFiltros" id="eosFiltroBotones" >
                    <tr>
                        
                        <td colspan="6">                   
                            <div class="eosBotonera eosBotonAprobarSeleccion"  style="margin-left:10px">
                                    <asp:ImageButton ID="btnAprobar" runat="server" 
                                         onclick="btnAprobarDelegaciones_Click" ImageUrl="~/Styles/images/bt_aprobar_seleccion.png" ValidationGroup = "ValidacionesCamposDelegacion"/></div>
                        </td>
                        <td>
                        <div class="eosBotonera eosBotonFiltrado">
                            <asp:ImageButton ID="CancelarImageButton" runat="server" CommandName="Login" ImageUrl="~/Styles/images/bt_volver.png"
                                onclick="btnVolver_Click"/></div>
                        </td>
                    </tr>
                </table>

		</div>
     </asp:Content>
