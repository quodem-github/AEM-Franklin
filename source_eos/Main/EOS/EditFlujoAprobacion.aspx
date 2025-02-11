<%@ Page Title="Editar Flujo de Aprobacion" Language="C#" MasterPageFile="~/Styles/EOS.Master" AutoEventWireup="true" CodeBehind="EditFlujoAprobacion.aspx.cs" Inherits="EOS.EditFlujoAprobacion" EnableEventValidation="false" Culture="es-ES" UICulture="es"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="eosHeaderBotonera" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="eosContentFilter" runat="server">    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>
    <div id="eosContentStatusTitulo" class="eosTituloWizard" runat="server" visible="true" style="padding-left: 10px">
        Flujo de Aprobación</div>
    <div id="eosContentFilter">
    <div id="eosContentAMECErrores">
            <asp:ValidationSummary ID="ContenAMECValidationSummary" runat="server" CssClass="failureNotification"
                ValidationGroup="ValidacionesCamposFlujoAprobacion" HeaderText="ATENCIÓN:" />
            <span class="failureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>
        </div>
     <table class="eosTablaFiltros" id="eosFiltroBasico" width="100%">
            <tr>
                <td colspan="6">
                    <span class="eosTituloNormal" style="padding-left: 10px">RELLENE LOS FILTROS NECESARIOS Y PULSE EL BOTÓN DE FILTRAR
                        </span>
                </td>
            </tr>
            <tr>
                <td>
                   <asp:Label ID="lblFlujo" runat="server" CssClass="eosCampoLabel">Tipo de flujo<span class="eosCampoObligatorio">*</span></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTipoFlujo" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW240" DataTextField="tipoflujo" AppendDataBoundItems = "True" DataValueField="idtipoflujo" Visible="true">
                                <asp:ListItem value="">Selecciona Tipo de Flujo</asp:ListItem>                  
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Tipo de Flujo Obligatorio" ControlToValidate="ddlTipoFlujo" ValidationGroup="ValidacionesCamposFlujoAprobacion" SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                
                </td>
            
               <td>
                   <asp:Label ID="lblActividad" runat="server" CssClass="eosCampoLabel">Tipo de Actividad</asp:Label>
                </td>
               <td>                            
                         <asp:DropDownList ID="ddlTipoActividad" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW240" 
                                DataTextField="tipoactividad" AppendDataBoundItems = "True" DataValueField="idtipoactividad" Visible="true">
                                <asp:ListItem value="">Selecciona Tipo de Actividad</asp:ListItem>
                   </asp:DropDownList>
                   
                 </td>
                 <td>

                    <label class="eosCampoLabel" for="" >Condicionada</label>
                </td>
                <td>
                     <asp:DropDownList ID="ddlCondicionado" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW20" 
                                 AppendDataBoundItems = "True" Visible="true">
                                <asp:ListItem value="1">Si</asp:ListItem>
                                <asp:ListItem value="0">No</asp:ListItem>
                   </asp:DropDownList>

                </td>
            </tr>
            <tr>
                <td>
                      <label class="eosCampoLabel" for="" >Nivel Inicial<span class="eosCampoObligatorio">*</span></label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlNivelIncial" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW200" 
                                DataTextField="nivelaprobacion" AppendDataBoundItems = "True" DataValueField="idnivelaprobacion" Visible="true">
                                <asp:ListItem value="">Selecciona Nivel Inicial</asp:ListItem>
                   </asp:DropDownList>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Nivel Inicial Obligatorio" ControlToValidate="ddlNivelIncial" ValidationGroup="ValidacionesCamposFlujoAprobacion" SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
              </td>
                <td>
                    <label class="eosCampoLabel" for="" >Nivel Final<span class="eosCampoObligatorio">*</span></label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlNivelSiguiente" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW200" 
                                DataTextField="nivelaprobacion" AppendDataBoundItems = "True" DataValueField="idnivelaprobacion" Visible="true">
                                <asp:ListItem value="">Selecciona Nivel Siguiente</asp:ListItem>
                   </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Nivel Siguiente Obligatorio" ControlToValidate="ddlNivelSiguiente" ValidationGroup="ValidacionesCamposFlujoAprobacion" SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                 </td>
                <td>  
                    <label class="eosCampoLabel" for="" >Fase<span class="eosCampoObligatorio">*</span></label>
                </td>
                <td>
                    <asp:TextBox ID="txtFase" runat="server" Width="90" MaxLength="30" CssClass="eosDisabledInputVacio eosInputVacio"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Fase Obligatoria" ControlToValidate="txtFase" ValidationGroup="ValidacionesCamposFlujoAprobacion" SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                   
                 </td>
                
            </tr>
            <tr>
                <td>
                    <label class="eosCampoLabel" for="">Preaprobado</label>
                </td>

                <td>

                    <asp:DropDownList ID="ddlPreAprobado" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW240" 
                                AppendDataBoundItems = "True" Visible="true">
                                <asp:ListItem value="">Selecciona Tipo de PreAprobación</asp:ListItem>
                                <asp:ListItem value="1">Preaprobada medico</asp:ListItem>
                                <asp:ListItem value="2">Preaprobada negocio</asp:ListItem>
                                <asp:ListItem value="3">Preaprobada legal/compliance</asp:ListItem>
                   </asp:DropDownList>
                 </td>
                
                
                <td>
                    <label class="eosCampoLabel" for="" >Importe Preaprobación</label>
                </td>
                <td>
                    <asp:TextBox ID="txtImporte" runat="server" MaxLength="30" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW200"></asp:TextBox>
                </td>
                <td>
                    <label class="eosCampoLabel" for="">Orden<span class="eosCampoObligatorio">*</span></label>
                </td>
                <td>
                    <asp:TextBox ID="txtOrden" runat="server" Width="20" MaxLength="30" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW20"></asp:TextBox> 
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Orden Obligatorio" ControlToValidate="txtOrden" ValidationGroup="ValidacionesCamposFlujoAprobacion" SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator" runat="server" ErrorMessage="Orden es Numérico" ControlToValidate="txtOrden" ValidationGroup="ValidacionesCamposFlujoAprobacion" ValidationExpression="^\d+$" SetFocusOnError="True"></asp:RegularExpressionValidator>
                 </td>

            
            
            </tr>             
        </table>       
        <table class="eosTablaFiltros" id="eosFiltroBotones">
            <tr>
                <td colspan="8">   
                    <div class="eosBotonera eosBotonAprobarSeleccion" style="margin-left:10px">
                            <asp:ImageButton ID="btnAprobar" runat="server" 
                                    onclick="btnAprobarFlujo_Click" ImageUrl="~/Styles/images/bt_aprobar_seleccion.png" ValidationGroup = "ValidacionesCamposFlujoAprobacion"/></div>
                </td>
                <td>
                <div class="eosBotonera eosBotonFiltrado">
                    <asp:ImageButton ID="CancelarImageButton" runat="server" CommandName="Enrera" ImageUrl="~/Styles/images/bt_volver.png"
                        OnClientClick="javascript:history.back();return false;"/></div>
                </td>

            </tr>
        </table> 
    </div>
</asp:Content>



