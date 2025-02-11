<%@ Page Title="Delegaciones de Aprobación"
     Language="C#" MasterPageFile="~/Styles/EOS.Master"
     AutoEventWireup="true" CodeBehind="Delegacion.aspx.cs"
     Inherits="EOS.Delegacion" EnableEventValidation="false" 
     Culture="es-ES" UICulture="es" %>

<%--<asp:Content ID="ContentBotonera" ContentPlaceHolderID="eosHeaderBotonera" runat="server">
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
        Delegaciones de permisos de aprobación
    </div>
    <div id="eosContentFilter">
        <div id="eosContentAMECErrores">
            <asp:ValidationSummary ID="ContenAMECValidationSummary" runat="server" CssClass="failureNotification"
                ValidationGroup="ValidacionesCamposDelegacionFiltro" HeaderText="ATENCIÓN:" />
            <span class="failureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>
        </div>
        <div class="eosTablaFiltros" id="eosFiltroBasico">
            <h2>Delegaciones de permisos de aprobación</h2>
            <p>Rellene los campos necesarios y pulse el botón FILTRAR</p>

            <div class="contentFormGeneric">
                <div class="contentForm eosMitadColumna">
                    <label class="eosCampoLabel" for="">Usuario</label>
                    <asp:DropDownList ID="ddlUsuario" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW160"
                        DataTextField="NombreCompleto" AppendDataBoundItems="True" DataValueField="IdPeticionario" Visible="true">
                        <asp:ListItem Value="-1">TODOS</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Fecha" ControlToValidate="ddlUsuario" ValidationGroup="ValidacionesCamposDelegacionObligatorios" SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                </div>
                <div class="contentForm eosMitadColumna">
                    <label class="eosCampoLabel" for="" style="margin-right: 15px; margin-left: 20px">Fecha Desde</label>
                    <asp:TextBox ID="txtFechaDesde" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW140"></asp:TextBox>
                    <asp:ImageButton ID="btnCalendarDesde" runat="server" ImageUrl="~/Styles/images/iconCalculadora.png" CssClass="iconCalendar"/>
                    <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="La Fecha debe tener un formato válido" Type="Date"
                        ControlToValidate="txtFechaDesde" Operator="DataTypeCheck" ValidationGroup="ValidacionesCamposDelegacionFiltro">&nbsp;</asp:CompareValidator>
                    <act:CalendarExtender ID="CalendarExtender2" TargetControlID="txtFechaDesde"
                        runat="server" TodaysDateFormat="d MMMM yyyy" FirstDayOfWeek="Monday"
                        PopupButtonID="btnCalendarDesde" Format="dd/MM/yy" />
                    &nbsp;&nbsp;&nbsp;
                </div>
            </div>
            <div class="contentFormGeneric">
                <div class="contentForm eosTresColumna">
                    <label class="eosCampoLabel" for="">Delegado</label>
                    <asp:DropDownList ID="ddlDelegado" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW160"
                        DataTextField="NombreCompleto" AppendDataBoundItems="True" DataValueField="IdPeticionario" Visible="true">
                        <asp:ListItem Value="-1">TODOS</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="contentForm eosTresColumna">
                    <label class="eosCampoLabel" for="" style="margin-right: 15px; margin-left: 20px">Fecha Hasta</label>
                    <asp:TextBox ID="txtFechaHasta" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW140"></asp:TextBox>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="La Fecha debe tener un formato válido" Type="Date"
                        ControlToValidate="txtFechaHasta" Operator="DataTypeCheck" ValidationGroup="ValidacionesCamposDelegacionFiltro">&nbsp;</asp:CompareValidator>
                    <asp:ImageButton ID="btnCalendarHasta" runat="server" ImageUrl="~/Styles/images/iconCalculadora.png" CssClass="iconCalendar"/>
                    <act:CalendarExtender ID="CalendarExtender3" TargetControlID="txtFechaHasta"
                        runat="server" TodaysDateFormat="d MMMM yyyy" FirstDayOfWeek="Monday"
                        PopupButtonID="btnCalendarHasta" Format="dd/MM/yy" />
                    &nbsp;&nbsp;&nbsp;
                </div>
                <div class="contentForm eosTresColumna">
                    <label class="eosCampoLabel" for="" style="margin-right: 10px; margin-left: 100px">Estado</label>
                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW160" Visible="true">
                       
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="eosTablaFiltros" id="eosFiltroBotones">

            <div class="eosBotonera eosBotonFiltrado">
                <asp:ImageButton ID="imgNuevaDelegacion" runat="server" PostBackUrl="EditDelegacion.aspx" ToolTip="Aprobar Selección" value="Nueva delegación" />
                <asp:ImageButton ID="btnFiltrar" runat="server"
                    OnClick="btnFiltrarDelegaciones_Click" value="Filtrar" ValidationGroup="ValidacionesCamposDelegacionFiltro" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="eosContentResults" runat="server">
    <div id="eosContentResults" class="contentTableGeneric">
        <asp:ListView ID="lvDelegaciones" runat="server" DataSourceID="odsDelegacion" EnablePersistedSelection="True" OnLoad="lvDelegaciones_Load" DataKeyNames="IDDELEGAPROBACION">
            <EmptyDataTemplate>
                <table class="eosTablaResultados">
                    <thead>
                        <tr>
                            <th></th>
                            <th></th>
                            <th>Usuario</th>
                            <th>Delegado</th>
                            <th>Fecha Desde</th>
                            <th>Fecha Hasta</th>
                            <th>Estado</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="10">
                                <span class="eosTituloRojo">No se han encontrado Delegaciones que cumplan los criterios de búsqueda seleccionados</span>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table class="eosTablaResultados">
                    <thead>
                        <tr>
                            <th style="text-align: left;"></th>
                            <th style="text-align: left;"></th>
                            <th style="text-align: left;">
                                <asp:LinkButton ID="lbUsuario" runat="server" Text="Usuario" CommandName="Sort" CommandArgument="NOMBRECOMPLETOUSUARIO"></asp:LinkButton>
                            </th>
                            <th style="text-align: left;">
                                <asp:LinkButton ID="lbDelegado" runat="server" Text="Delegado" CommandName="Sort" CommandArgument="NOMBRECOMPLETODELEGADO"></asp:LinkButton>
                            </th>
                            <th style="text-align: left;">
                                <asp:LinkButton ID="lbFechaDesde" runat="server" Text="Fecha Desde" CommandName="Sort" CommandArgument="FECHADESDE"></asp:LinkButton>
                            </th>
                            <th style="text-align: left;">
                                <asp:LinkButton ID="lbFechaHasta" runat="server" Text="Fecha Hasta" CommandName="Sort" CommandArgument="FECHAHASTA"></asp:LinkButton>
                            </th>
                            <th style="text-align: left;">
                                <asp:LinkButton ID="lbEstado" runat="server" Text="Estado" CommandName="Sort" CommandArgument="ESTADO"></asp:LinkButton>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </tbody>
                </table>
                <div class="contentPagination">
                    <asp:DataPager ID="DataPager3" runat="server" PageSize="10">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Image" ShowFirstPageButton="True"
                                ShowNextPageButton="False" ShowPreviousPageButton="True" FirstPageText="" PreviousPageText="" PreviousPageImageUrl="Styles/images/ic_pagina_anterior.png" FirstPageImageUrl="Styles/images/ic_pagina_primera.png" RenderDisabledButtonsAsLabels="True" />

                            <asp:NumericPagerField CurrentPageLabelCssClass="pageSel" />
                            <asp:NextPreviousPagerField ButtonType="Image" ShowLastPageButton="True"
                                ShowNextPageButton="True" ShowPreviousPageButton="False" LastPageText="" NextPageText="" LastPageImageUrl="Styles/images/ic_pagina_ultima.png" NextPageImageUrl="Styles/images/ic_pagina_siguiente.png" RenderDisabledButtonsAsLabels="True" />
                        </Fields>
                    </asp:DataPager>
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:ImageButton Text="Borrar" ID="ImgDeleteButton" runat="server" ImageUrl="~/Styles/images/ic_participante_aspa.png"
                            CommandArgument='<%# Eval("iddelegaprobacion") %>' ToolTip='<%# String.Format("Anular la Delegacion {0}", Eval("iddelegaprobacion")) %>'
                            OnCommand="imgDelegacionAnular_Command" />
                    </td>
                    <td>
                        <asp:ImageButton Text="Editar" ID="ImageButton1" runat="server" ImageUrl="~/Styles/images/ic_modificar.png"
                            CommandArgument='<%# Eval("iddelegaprobacion") %>' ToolTip='<%# String.Format("Editar la Delegacion {0}", Eval("iddelegaprobacion")) %>'
                            OnCommand="imgDelegacionEditar_Command" />
                    </td>
                    <td class="par">
                        <asp:Label ID="idexpedienteLabel" runat="server" Text='<%# Eval("NombreCompletoUsuario") %>' />
                    </td>
                    <td>
                        <asp:Label ID="amecLabel" runat="server" Text='<%# Eval("NombreCompletoDelegado") %>' />
                    </td>
                    <td class="par">
                        <asp:Label ID="fechadesde" runat="server" Text='<%# String.Format("{0:dd/MM/yy}",Eval("fechadesde")) %>' />
                    </td>
                    <td class="par">
                        <asp:Label ID="fechahasta" runat="server" Text='<%# String.Format("{0:dd/MM/yy}",Eval("fechahasta")) %>' />
                    </td>
                    <td>
                        <asp:Label ID="estado" runat="server" Text='<%# ((EOS.Web.Enums.EstadoDelegacion)int.Parse(Eval("IdEstado").ToString())).ToString() %>' />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsDelegacion" runat="server" SelectMethod="ObtenerDelegacionesTotal"
            TypeName="EOS.Web.AgenteDelegacion" SelectCountMethod="ObtenerNumeroDelegaciones"
            EnablePaging="True" OnSelecting="odsDelegacion_Selecting" SortParameterName="sortParameter">
            <SelectParameters>
                <asp:Parameter Name="filtroidusuario" Type="String" />
                <asp:Parameter Name="filtroiddelegado" Type="String" />
                <asp:Parameter Name="filtrofechadesde" Type="String" />
                <asp:Parameter Name="filtrofechahasta" Type="String" />
                <asp:Parameter Name="filtroestado" Type="String" />

            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    <table width="100%">
        <tr>
            <td>
                <div class="eosDivCentrada">
                    <asp:ImageButton ID="btnVolver" Visible="false" runat="server" ImageUrl="~/Styles/images/bt_volver.png" />
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
