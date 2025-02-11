<%@ Page Title="Flujo de Aprobacion" Language="C#" MasterPageFile="~/Styles/EOS.Master" AutoEventWireup="true"  CodeBehind="FlujoAprobacion.aspx.cs" Inherits="EOS.FlujoAprobacion" EnableEventValidation="false" Culture="es-ES" UICulture="es" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content3" ContentPlaceHolderID="eosContentFilter" runat="server">    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>
    <div id="eosContentStatusTitulo" class="eosTituloWizard" runat="server" visible="true" style="padding-left: 10px">
        Flujo de Aprobación</div>
    <div id="eosContentFilter">
     <table class="eosTablaFiltros" id="eosFiltroBasico">
            <tr>
                <td colspan="8">
                    <span class="eosTituloNormal" style="padding-left: 10px">RELLENE LOS FILTROS NECESARIOS Y PULSE EL BOTÓN DE FILTRAR
                        </span>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                   <asp:Label ID="lblFlujo" runat="server" CssClass="eosCampoLabel" 
                         style="margin-right: 19px; margin-left:10px">Tipo de flujo</asp:Label>
                         <asp:DropDownList ID="ddlTipoFlujo" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW240" 
                                DataTextField="tipoflujo" AppendDataBoundItems = "True" DataValueField="idtipoflujo" Visible="true">
                                <asp:ListItem value="-1">Selecciona Tipo de Flujo</asp:ListItem>                   
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                   <asp:Label ID="Label1" runat="server" CssClass="eosCampoLabel" 
                         style="margin-right: 10px; margin-left:10px">Tipo de Actividad</asp:Label>                            
                         <asp:DropDownList ID="ddlTipoActividad" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW240" 
                                DataTextField="tipoactividad" AppendDataBoundItems = "True" DataValueField="idtipoactividad" Visible="true">
                                <asp:ListItem value="-1">Selecciona Tipo de Actividad</asp:ListItem>
                   </asp:DropDownList>
                </td>
            </tr>           
        </table>       
        <table class="eosTablaFiltros" id="eosFiltroBotones">
            <tr>
                <td colspan="8">                   
                    <div class="eosBotonera eosBotonAprobarSeleccion" style="margin-left:10px">
                        <asp:ImageButton ID="imgNuevoFlujoAprobacion" runat="server" PostBackUrl= "EditFlujoAprobacion.aspx" ToolTip="Aprobar Selección" ImageUrl="~/Styles/images/btnNuevaApr.png" />
                    </div>
                </td>
                <td>
                   <div class="eosBotonera eosBotonFiltrado" >
                        <asp:ImageButton ID="imgFiltrarFlujoAprobacion" runat="server"  ToolTip="Filtrar" OnClick="btnFiltrarFlujo_Click" ImageUrl="~/Styles/images/bt_filtrar.png" />
                    </div>
                </td>

            </tr>
        </table> 
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="eosContentResults" runat="server">
    <div id="eosContentResults">
        <asp:ListView ID="lvFlujoAprobacion" runat="server" DataSourceID ="odsFlujoAprobacion">
            <EmptyDataTemplate>
                <table class="eosTablaResultados">
                    <thead>
                        <tr>
                            <th>ORDEN</th>
                            <th>FASE</th>
                            <th>NIVEL INICIAL</th>
                            <th >NIVEL FINAL</th>                            
                            <th>PREAPROBADO</th>
                            <th>CONDICIONADO</th>
                            <th>IMPORTE</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="10">
                                <span class="eosTituloRojo">No se han encontrado aprobaciones que cumplan los criterios
                                    de búsqueda seleccionados</span>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table class="eosTablaResultados">
                    <thead>
                        <tr>
                            <th >
                            </th>
                            <th >
                            </th>
                            <th>
                                <asp:LinkButton ID="lbOrden" runat="server" Text="Orden" CommandName="Sort" CommandArgument="ORDENFASE"></asp:LinkButton>
                            </th>
                            <th>
                                <asp:LinkButton ID="lbFase" runat="server" Text="Fase" CommandName="Sort" CommandArgument="FASE"></asp:LinkButton>
                            </th>
                            <th>
                                <asp:LinkButton ID="lbNivelInicial" runat="server" Text="Nivel Inicial" CommandName="Sort" CommandArgument="NIVELINICIAL"></asp:LinkButton>
                            </th>
                            <th >
                                <asp:LinkButton ID="lbNivelSiguiente" runat="server" Text="Nivel Siguiente" CommandName="Sort" CommandArgument="NIVELSIGUIENTE"></asp:LinkButton>
                            </th>
                            <th>
                                <asp:LinkButton ID="lbPreaprobado" runat="server" Text="Preaprobado" CommandName="Sort" CommandArgument="PREAPROBADO"></asp:LinkButton>
                            </th>
                            <th>
                                <asp:LinkButton ID="lbCondicionado" runat="server" Text="Condicionado" CommandName="Sort" CommandArgument="CONDICIONADA"></asp:LinkButton>
                            </th>
                            <th>
                                <asp:LinkButton ID="lblImporte" runat="server" Text="Importe" CommandName="Sort" CommandArgument="IMPORTEPREAPROBACION"></asp:LinkButton>
                            </th>                              
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="10">
                                <asp:DataPager ID="DataPager3" runat="server" PageSize="10">
                                    <Fields>
                                        <asp:NextPreviousPagerField ButtonType="Image" ShowFirstPageButton="True" ShowNextPageButton="False"
                                            ShowPreviousPageButton="True" FirstPageText=" " PreviousPageText=" " PreviousPageImageUrl="Styles/images/ic_pagina_anterior.png"
                                            FirstPageImageUrl="Styles/images/ic_pagina_primera.png" RenderDisabledButtonsAsLabels="True" />
                                        <asp:NumericPagerField CurrentPageLabelCssClass="pageSel" />
                                        <asp:NextPreviousPagerField ButtonType="Image" ShowLastPageButton="True" ShowNextPageButton="True"
                                            ShowPreviousPageButton="False" LastPageText=" " NextPageText=" " LastPageImageUrl="Styles/images/ic_pagina_ultima.png"
                                            NextPageImageUrl="Styles/images/ic_pagina_siguiente.png" RenderDisabledButtonsAsLabels="True" />
                                    </Fields>
                                </asp:DataPager>
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                     <td>
                            <asp:ImageButton Text="Borrar" ID="ImgDeleteButton" runat="server" ImageUrl="~/Styles/images/ic_participante_aspa.png"
                            CommandArgument='<%# Eval("idflujoaprobacion") %>' ToolTip='<%# String.Format("Eliminar Flujo de Aprobación {0}", Eval("idflujoaprobacion")) %>'
                            OnCommand="imgDelegacionEliminar_Command"/>
                    </td>    
                    <td>
                        <asp:ImageButton Text="Editar" ID="ImageButton1" runat="server" ImageUrl="~/Styles/images/ic_modificar.png"
                        CommandArgument='<%# Eval("idflujoaprobacion") %>' ToolTip='<%# String.Format("Editar la Flujo Aprobación {0}", Eval("idflujoaprobacion")) %>'
                        PostBackUrl='<%# Eval("idflujoaprobacion", "EditFlujoAprobacion.aspx?idflujoaprobacion={0}") %>' />
                       
                    </td>              
                    <td class="par">
                        <asp:Label ID="lblOrden" runat="server" Text='<%# Eval("ordenfase") %>'  />
                    </td>
                    <td>
                        <asp:Label ID="lblFase" runat="server" Text='<%# Eval("fase") %>' />
                    </td>
                    <td class="par">
                        <asp:Label ID="lblNivelInicial" runat="server"  Text='<%# Eval("NivelInicial") %>'/>
                    </td>
                    <td>
                        <asp:Label ID="lblNivelSiguiente" runat="server"  Text='<%# Eval("NivelSiguiente") %>' />
                    </td>
                    <td>
                        <asp:Label ID="lblpreaprobado" runat="server" Text='<%# Eval("preaprobado") %>' />
                    </td>
                    <td>
                        <asp:Label ID="lblcondicionado" runat="server"  Text='<%#(Eval("condicionada").ToString() == "True" ? "Sí" : "No") %>'/>
                    </td>  
                    <td>
                        <asp:Label ID="lblimporte" runat="server"  Text='<%# Eval("importepreaprobacion") %>' />
                    </td> 
                </tr>               
            </ItemTemplate>
        </asp:ListView> 
        
        <asp:ObjectDataSource ID="odsFlujoAprobacion" runat="server" SelectMethod="ObtenerFlujosAprobacion"
                        TypeName="EOS.Web.AgenteFlujoAprobacion" SelectCountMethod="ObtenerNumeroFlujosAprobacion"
                        EnablePaging="True" OnSelecting = "odsFlujoAprobacion_Selecting" SortParameterName="sortParameter">
            <SelectParameters>
                <asp:Parameter Name="filtroidactividad" Type="String" />
                <asp:Parameter Name="filtroidtipoflujo" Type="String" />
            </SelectParameters>       
         </asp:ObjectDataSource>            
    </div>
</asp:Content>



