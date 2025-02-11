<%@ Page Title="Listado de AMECs" Language="C#" MasterPageFile="~/Styles/EOS.Master" AutoEventWireup="true" 
    CodeBehind="ListadoAMECs.aspx.cs" Inherits="EOS.ListadoAMECs" EnableEventValidation="false"
     ValidateRequest="false"  Culture="es-ES" UICulture="es" %>

<%@Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script type='text/javascript'>
        function UnidadesOrg(check) {
            if (check.checked) {
                document.getElementById("abrirunidades").style.display = "";
            } else {
                document.getElementById("abrirunidades").style.display = "none";
            }
        }
        function ToggleAdvancedInfo(celda) {

            var filaOculta = celda.parentNode.parentNode.nextSibling;
            if (filaOculta.innerHTML == null) // Esto es porque en firefox encuentra otro elemento antes que la siguiente TR
                filaOculta = filaOculta.nextSibling;

            var sActual = celda.innerHTML;
            if (sActual == '+') {
                is_loaded();
                var tr = document.getElementsByClassName('datoVisible');
                for (i = 0; i < tr.length; i++) {
                    if (tr[i].className == 'datoVisible') {
                        tr[i].className = 'datoOculto';

                    }
                }

                filaOculta.className = 'datoVisible';
                celda.innerHTML = '-';

            } else {
                filaOculta.className = 'datoOculto';
                celda.innerHTML = '+';
            }
        }

        function is_loaded() { //DOM
            if (document.getElementById('preloader').style.visibility == 'hidden') {
                document.getElementById('preloader').style.visibility = 'visible';
                setTimeout('document.getElementById("preloader").style.visibility = "hidden"', 4300);

            }
        }
    </script>
</asp:Content>
<%--<asp:Content ID="ContentBotonera" ContentPlaceHolderID="eosHeaderBotonera" runat="server">

    <div id="eosHPBotones">
        <div class="eosBotonera eosBotonInformes"><a title="Acceder a Informes y Listados" onmouseout="javascript:window.status='';return true;" onmouseover="javascript:window.status='Acceder a Informes y Listados';return true;" href="javascript:document.location.href = 'Informes/FiltroInforme.aspx';">Acceso a Informes y Listados</a></div>
        <div class="eosBotonera eosBotonDatosPer"><a title="Acceder a Datos Personales" onmouseout="javascript:window.status='';return true;" onmouseover="javascript:window.status='Cambiar datos personales';return true;" href="javascript:document.location.href = 'CambiarDatosPersonales.aspx';">Acceso a Datos Personales</a></div>
        <div class="eosBotonera eosBotonExpedientes"><a title="Acceder a Listados de Expedientes" onmouseout="javascript:window.status='';return true;" onmouseover="javascript:window.status='Litados de Expedientes';return true;" href="javascript:document.location.href = 'Expedientes.aspx';">Listados de Expedientes</a></div>
        <div class="eosBotonera eosBotonSalir"><a title="Volver al listado de Expedientes" href="Expedientes.aspx">Volver al listado de Expedientes</a></div>
    </div>
</asp:Content>--%>

<asp:Content ID="Content3" ContentPlaceHolderID="eosContentFilter" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>
    <%--<asp:ScriptManager runat="server" ID="ScriptManager1" EnablePartialRendering="true">
    </asp:ScriptManager>--%>
    <div id="eosContentFilter">
        <div><h1>Buscador de eventos</h1></div>
        <div class="eosTablaFiltros" id="eosFiltroBasico">
            <div>
                <h2>Filtro de actividad</h2>
               <p class="eosTituloNormal">RELLENE LOS FILTROS NECESARIOS Y PULSE EL BOTÓN DE FILTRAR</p>
            </div>
            <div class="contentFormGeneric">
                <div class="contentForm  eosMitadColumna">
                    <label class="eosCampoLabel" for="txtActividad">AMEC</label>
                    <!--<input type="text" onkeypress="changeOnKeyPress(this);" onmousedown="this.focus();" onblur="changeOnBlur(this, 'Nombre');" onfocus="changeOnFocus(this, 'Nombre');" id="xxxTextNombre" name="xxxNombre" maxlength="80" class="eosInputVacio eosSizeW120" value="Nombre" gtbfieldid="1">-->
                    <asp:TextBox ID="txtAMEC" runat="server" MaxLength="50"
                        CssClass="eosInputVacio eosSizeW120"></asp:TextBox>
                </div>
                <div class="contentForm  eosMitadColumna">
                    <label class="eosCampoLabel" for="xxxFiltroAsistente">Solicitante</label>
                    <asp:DropDownList ID="ddlUsuario" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW220"
                        DataTextField="NombreCompleto" DataValueField="IdPeticionario" Visible="true" AutoPostBack="True" AppendDataBoundItems="true">
                        <asp:ListItem Value="-1">(Todos)</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="contentFormGeneric">
                <div class="contentForm  eosMitadColumna">
                    <label class="eosCampoLabel" for="xxxFiltroEstadoExpediente">Estado AMEC</label>
                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="eosInputVacio eosSizeW50" DataTextField="estado">
                        <asp:ListItem Value="-1">(Todos)</asp:ListItem>
                        <asp:ListItem Value="1">Aprobado</asp:ListItem>
                        <asp:ListItem Value="2">Pendiente de Aprobar</asp:ListItem>
                        <asp:ListItem Value="3">Cancelado</asp:ListItem>
                        <asp:ListItem Value="4">Rechazado</asp:ListItem>
                        <asp:ListItem Value="5">Borrador</asp:ListItem>
                        <asp:ListItem Value="6">Pendiente de Aprobar por mi</asp:ListItem>
                        <asp:ListItem Value="7">Pendiente de Someter</asp:ListItem>
                        <asp:ListItem Value="8">Cerrado</asp:ListItem>
                        <asp:ListItem Value="9">Completado</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="contentForm  eosMitadColumna">
                    <label class="eosCampoLabel" for="xxxFiltroAMEC">Nombre del Programa/Actividad</label>
                    <asp:TextBox ID="txtActividad" runat="server" MaxLength="50"
                        CssClass="eosInputVacio eossi eosSizeW210"></asp:TextBox>
                </div>
            </div>
            <div class="contentFormGeneric">
                <div class="contentForm  eosMitadColumna">
                    <label class="eosCampoLabel" for="xxxFiltroAMEC">Año Creación</label>&nbsp;
                    <asp:DropDownList
                        ID="txtAnyo" runat="server" CssClass="eosInputVacio eosSizeW50">
                    </asp:DropDownList>
                </div>
                <div class="contentForm  eosMitadColumna">
                    <label class="eosCampoLabel" for="xxxFiltroAMEC">Mes Creación</label>&nbsp;
                            <asp:DropDownList
                                ID="txtMes" runat="server" CssClass="eosInputVacio eosSizeW90">
                                <asp:ListItem Value="-1">(Todos)</asp:ListItem>
                                <asp:ListItem Value="1">Enero</asp:ListItem>
                                <asp:ListItem Value="2">Febrero</asp:ListItem>
                                <asp:ListItem Value="3">Marzo</asp:ListItem>
                                <asp:ListItem Value="4">Abril</asp:ListItem>
                                <asp:ListItem Value="5">Mayo</asp:ListItem>
                                <asp:ListItem Value="6">Junio</asp:ListItem>
                                <asp:ListItem Value="7">Julio</asp:ListItem>
                                <asp:ListItem Value="8">Agosto</asp:ListItem>
                                <asp:ListItem Value="9">Septiembre</asp:ListItem>
                                <asp:ListItem Value="10">Octubre</asp:ListItem>
                                <asp:ListItem Value="11">Noviembre</asp:ListItem>
                                <asp:ListItem Value="12">Diciembre</asp:ListItem>
                            </asp:DropDownList>
                </div>
            </div>
            <div class="contentFormGeneric">
                <div class="contentForm  eosMitadColumna">
                    <label class="eosCampoLabelSin">
                        Fecha inicio actividad</label>

                    <asp:TextBox ID="txtFechaComienzo" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW90"></asp:TextBox>
                    <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="La fecha de Comienzo debe tener un formato válido"
                        Type="Date" ControlToValidate="txtFechaComienzo" Operator="DataTypeCheck" ValidationGroup="AMECValidationGroup">&nbsp;</asp:CompareValidator>
                    <asp:ImageButton ID="btnCalendarIni" runat="server" ImageUrl="~/Styles/images/ic_calendario.png" class="iconCalendar"/>
                    <act:CalendarExtender ID="CalendarExtender4" TargetControlID="txtFechaComienzo" runat="server"
                        TodaysDateFormat="d MMMM yyyy" FirstDayOfWeek="Monday" PopupButtonID="btnCalendarIni"
                        Format="dd/MM/yy" />
                </div>
                <div class="contentForm  eosMitadColumna">
                    <label class="eosCampoLabelSin" for="txtNombre">
                        Fecha fin actividad</label>

                    <asp:TextBox ID="txtFechaFinalizacion" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW90"></asp:TextBox>
                    <asp:CompareValidator ID="CompareValidator3" runat="server" ErrorMessage="La Fecha de Finalización debe tener un formato válido"
                        Type="Date" ControlToValidate="txtFechaFinalizacion" Operator="DataTypeCheck"
                        ValidationGroup="AMECValidationGroup">&nbsp;</asp:CompareValidator>
                    <asp:RangeValidator ID="RangeValidator6" runat="server" ErrorMessage="La fecha de Finalización no puede ser inferior a hoy"
                        MaximumValue="31/12/99" ControlToValidate="txtFechaFinalizacion" ValidationGroup="AMECValidationGroup"
                        Display="Dynamic">&nbsp;</asp:RangeValidator>
                    <asp:ImageButton ID="btnCalendarFin" runat="server" ImageUrl="~/Styles/images/ic_calendario.png" class="iconCalendar"/>
                    <act:CalendarExtender ID="CalendarExtender5" TargetControlID="txtFechaFinalizacion"
                        runat="server" TodaysDateFormat="d MMMM yyyy" FirstDayOfWeek="Monday" PopupButtonID="btnCalendarFin"
                        Format="dd/MM/yy" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </div>
            </div>
            <div class="contentFormGeneric">
                <div class="contentForm  eosMitadColumna">

                    <label class="eosCampoLabel" for="">Paraguas</label>
                    <asp:DropDownList
                        ID="ddlParaguas" runat="server" CssClass="eosInputVacio eosSizeW50">
                        <asp:ListItem Value="-1">(Todos)</asp:ListItem>
                        <asp:ListItem Value="1">Si</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </div>
                <div class="contentForm  eosMitadColumna">
                    <label class="eosCampoLabel" for="" >Importe</label>
                    <div>
                        <asp:DropDownList ID="ddlTipoImporte" runat="server" CssClass="eosInputVacio eosSizeW50">
                            <asp:ListItem Value="-1">(Todos)</asp:ListItem>
                            <asp:ListItem Value="1"><</asp:ListItem>
                            <asp:ListItem Value="2"><=</asp:ListItem>
                            <asp:ListItem Value="3">=</asp:ListItem>
                            <asp:ListItem Value="4">>=</asp:ListItem>
                            <asp:ListItem Value="5">></asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtImporte" runat="server" MaxLength="80" CssClass="eosInputVacio eosSizeW115"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="contentFormGeneric">
                <div class="contentForm  eosMitadColumna">
                        <label class="eosCampoLabel" for="" style="padding-right: 5px;">Producto</label>
                        <asp:TextBox ID="txtImporteCargoProducto" runat="server" MaxLength="180" CssClass="eosInputVacio eosSizeW210"></asp:TextBox>
                </div>
                <div class="contentForm  eosMitadColumna">
                    <label class="eosCampoLabel" for="">
                        &nbsp;&nbsp;Departamento:</label>
                    <asp:DropDownList ID="ddlDepartament" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDepartament_OnSelectedIndexChanged" CssClass="eosInputVacio eosSizeW50"></asp:DropDownList>
                 </div>
            </div>
            <div class="contentFormGeneric">
                <div class="contentForm  eosMitadColumna">
                    <label class="eosCampoLabel" for="">
                        &nbsp;&nbsp;Fuerda de venta:</label>
                    <asp:DropDownList ID="ddlSaleForce" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSaleForce_OnSelectedIndexChanged" CssClass="eosInputVacio eosSizeW50"></asp:DropDownList>
                </div>
                <div class="contentForm  eosMitadColumna">
                    <label class="eosCampoLabel" for="">
                        &nbsp;&nbsp;Distrito:</label>
                    <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="eosInputVacio eosSizeW50"></asp:DropDownList>
                </div>
            </div>
            <div class="contentFormGeneric">
                <div class="radioCheck">
                    <asp:CheckBox ID="chkUnidadesOrganizativas" onclick="javascript:UnidadesOrg(this);" runat="server" Text=" " />
                    <label class="eosCampoLabel" for="">Filtro de Unidades Organizativas</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </div>
            </div>
        </div>


        <% if (UnidadesOrganizativas())
            { %>
        <div id="abrirunidades">
            <% }
                else
                { %>
            <div id="abrirunidades" style="display: none">
                <% } %>
                <asp:UpdatePanel ID="UpdateUnidades" runat="server">
                    <ContentTemplate>
                        <div id="unidadesOrganizativas" style="margin-top: 10px;" class="eosTablaFiltros">
                            <div>
                                <p class="eosTituloNormal">UNIDAD ORGANIZATIVA</p>
                            </div>
                            <div class="contentFormGeneric">
                                <div class="eosCuartoColumna">
                                    <label class="eosCampoLabelSin" for="txtUnidad">
                                        Unidad</label>
                                    <asp:DropDownList ID="ddlUnidadMas" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW160"
                                        OnSelectedIndexChanged="ddlUnidadMas_SelectedIndexChanged" AutoPostBack="True"
                                        Visible="true" Enabled="false">
                                    </asp:DropDownList>
                                </div>
                                <div class="eosCuartoColumna">
                                    <label class="eosCampoLabelSin" for="txtArea">
                                        Área</label>
                                    <asp:DropDownList ID="ddlAreaMas" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW160"
                                        OnSelectedIndexChanged="ddlAreaMas_SelectedIndexChanged" Visible="true" AutoPostBack="True"
                                        Enabled="false">
                                    </asp:DropDownList>
                                </div>
                                <div class="eosCuartoColumna">
                                    <label class="eosCampoLabelSin" for="txtRegion">
                                        Región</label>
                                    <asp:DropDownList ID="ddlRegionMas" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW160"
                                        OnSelectedIndexChanged="ddlRegionMas_SelectedIndexChanged" Visible="true" AutoPostBack="True"
                                        Enabled="false">
                                    </asp:DropDownList>
                                </div>
                                <div class="eosCuartoColumna">
                                    <label class="eosCampoLabelSin" for="txtDistrito">
                                        Distrito</label>
                                    <asp:DropDownList ID="ddlDistritoMas" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW160 "
                                        Visible="true" AutoPostBack="True" Enabled="false">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdateUnidades">
                    <ProgressTemplate>
                        <center>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/images/ajax-loader.gif"
                                Title="Cargando..." />
                            <br />
                            <strong>Cargando...</strong><br />
                        </center>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div class="eosTablaFiltros" id="eosFiltroBotones">
                    <div></div>
                    <div>
                        <!--<div class="eosBotonera eosBotonFiltrado"><a title="Filtrar Expedientes" href="javascript:;" onclick="javascript:document.getElementById('MainForm').submit();return false;">Filtrar Expedientes</a></div>-->
                        <div class="eosBotonera eosBotonFiltrado">
                            <asp:Button ID="btnFiltrar" runat="server" OnClick="btnFiltrar_Click" Text="Filtrar"
                                ValidationGroup="AMECValidationGroup" />
                        </div>
                    </div>
            </div>
        </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="eosContentResults" runat="server">
    <div id="preloader" style="position: absolute; visibility: hidden; left: 0;right: 0;">
        <div style="text-align: center; background-color: transparent;">
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <div>
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/images/ajax-loader.gif"
                    Title="Cargando..." /></div>
            <br />
            <strong>Cargando...</strong><br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
        </div>
    </div>
    <div id="eosContentResults" class="contentTableGeneric">
        <asp:ListView ID="lvAMECs" runat="server" DataSourceID="odsAMECs" OnSorting="lvAMECs_Sorting"
            OnItemDataBound="lvAMECs_ItemDataBound">
            <EmptyDataTemplate>
                <table class="eosTablaResultados">
                    <thead>
                        <tr>
                            <th></th>
                            <th>Amec
                            </th>
                            <th >Nombre del Programa/Actividad
                            </th>
                            <th>fecha creación
                            </th>
                            <th>estado
                            </th>
                            <th>importe
                            </th>
                            <th>paraguas
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="8">
                                <span class="eosTituloRojo">No se han encontrado AMECs que cumplan los criterios de búsqueda seleccionados</span>
                            </td>
                        </tr>                 
                    </tbody>
                </table>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table class="eosTablaResultados">
                    <thead>
                        <tr>
                            <th></th>
                            <th>
                                <asp:LinkButton ID="lbAmec" runat="server" Text="Amec" CommandName="Sort" CommandArgument="IDAMECS"></asp:LinkButton>
                            </th>
                            <th>
                                <asp:LinkButton ID="lbActividad" runat="server" Text="Nombre del Programa/Actividad"
                                    CommandName="Sort" CommandArgument="DESCRIPCION"></asp:LinkButton>
                            </th>
                            <th>
                                <asp:LinkButton ID="lbFecha" runat="server" Text="Fecha Creación" CommandName="Sort"
                                    CommandArgument="FECHAAMECS"></asp:LinkButton>
                            </th>
                            <th>
                                <asp:LinkButton ID="lbAprobado" runat="server" Text="Estado" CommandName="Sort" CommandArgument="ESTADO"></asp:LinkButton>
                            </th>
                            <th>
                                <asp:LinkButton ID="lbImporte" runat="server" Text="Importe" CommandName="Sort" CommandArgument="IMPORTEGASTO"></asp:LinkButton>
                            </th>
                            <th>
                                <asp:LinkButton ID="lblParaguas" runat="server" Text="Paraguas" CommandName="Sort"
                                    CommandArgument="PARAGUAS"></asp:LinkButton>
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
                            <asp:NextPreviousPagerField ButtonType="Image" ShowFirstPageButton="True" ShowNextPageButton="False"
                                ShowPreviousPageButton="True" FirstPageText="" PreviousPageText="" PreviousPageImageUrl="Styles/images/ic_pagina_anterior.png"
                                FirstPageImageUrl="Styles/images/ic_pagina_primera.png" RenderDisabledButtonsAsLabels="True" />
                            <asp:NumericPagerField CurrentPageLabelCssClass="pageSel" />
                            <asp:NextPreviousPagerField ButtonType="Image" ShowLastPageButton="True" ShowNextPageButton="True"
                                ShowPreviousPageButton="False" LastPageText="" NextPageText="" LastPageImageUrl="Styles/images/ic_pagina_ultima.png"
                                NextPageImageUrl="Styles/images/ic_pagina_siguiente.png" RenderDisabledButtonsAsLabels="True" />
                        </Fields>
                    </asp:DataPager>
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td class="table-icon-container">
                        <asp:ImageButton ID="imgAmecDetalle" runat="server" ToolTip='<%# String.Format("Ver detalles del AMEC {0}", Eval("idamecs")) %>'
                            ImageUrl="~/Styles/images/icoLupa.png" />
                        <asp:ImageButton ID="imgAmecListado" runat="server" ToolTip='<%# String.Format("Imprimir informes del AMEC {0}", Eval("idamecs")) %>'
                            ImageUrl="~/Styles/images/ic_listado.png" OnCommand="imgExportListAmec_Command"
                            CommandArgument='<%# Eval("idamecs") %>' />
                    </td>
                    <td class="par">
                        <asp:Label ID="labelidamecs" runat="server" Text='<%# Eval("idamecs") %>' />
                    </td>
                    <td>
                        <asp:Label ID="amecLabel" runat="server" Text='<%# Eval("descripcion") %>' />
                         <asp:LinkButton ID="imgEventos" runat="server" CssClass="contentMoreInfo" CommandName="Select" CommandArgument='<%# Eval("idamecs") %>'
     OnClick="CargarListView" Text="+" OnClientClick="javascript:ToggleAdvancedInfo(this);" />
                    </td>
                    <td class="par">
                        <asp:Label ID="fechacreacionLabel" runat="server" Text='<%# Eval("fechaamecs") %>' />
                    </td>
                    <td>
                        <asp:Label ID="expedienteLabel" runat="server" Text='<%# Eval("estado") %>' />
                    </td>
                    <td class="par" align="right">
                        <asp:Label ID="Label1" runat="server" Text='<%# string.Format("{0:C}", Eval("importegasto")) %>' />
                    </td>
                    <td class="par" align="right">
                        <asp:Label ID="Label2" runat="server" Text='<%# ((Boolean)Eval("veeva") ? "" : Eval("paraguas").ToString() == "True" ? "Sí" : "No")%>' />
                    </td>
                </tr>
                <tr class="datoOculto" id="filasList">
                    <td colspan="10">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:ListView ID="lvAmecsCongresos" runat="server">
                                    <EmptyDataTemplate>
                                        <div class="eosSubTablaResultados">
                                            <div>
                                                <span>Congreso:
                                                </span>
                                                  &nbsp;&nbsp;|
                                                <span>Población:
                                                </span>
                                                  &nbsp;&nbsp;|
                                                <span>Fecha desde:
                                                </span>
                                                  &nbsp;&nbsp;|
                                                <span>Fecha hasta:
                                                </span>
                                            </div>
                                            <div>
                                                <span class="eosTituloRojo">No se han encontrado Congresos para este Amec</span>
                                            </div>
                                        </div>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <div class="eosSubTablaResultados">
                                            <div>
                                                <asp:LinkButton ID="evento" runat="server" Text="Congreso" CommandName="Sort" CommandArgument="Evento"></asp:LinkButton>
                                                <asp:LinkButton ID="lbFecha" runat="server" Text="Población" CommandName="Sort" CommandArgument="FECHAAMECS"></asp:LinkButton>
                                                <asp:LinkButton ID="lbAprobado" runat="server" Text="Fecha Desde" CommandName="Sort"
                                                    CommandArgument="ESTADO"></asp:LinkButton>
                                                <asp:LinkButton ID="lbImporte" runat="server" Text="Fecha Hasta" CommandName="Sort"
                                                    CommandArgument="IMPORTEGASTO"></asp:LinkButton>
                                            </div>
                                            <div>
                                                <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                            </div>
                                        </div>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <asp:Label ID="nombreLabel" runat="server" Text='<%# Eval("congreso") %>' />
                                            <asp:Label ID="poblacionLabel" runat="server" Text='<%# Eval("poblacion") %>' />
                                            <asp:Label ID="fechainicioLabel" runat="server" Text='<%# String.Format("{0:dd/MM/yy}",Eval("desde")) %>' />
                                            <asp:Label ID="fechafinLabel" runat="server" Text='<%# String.Format("{0:dd/MM/yy}",Eval("hasta")) %>' />
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="imgEventos" EventName="click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <asp:ObjectDataSource ID="odsActividadesAsigAmec" runat="server" SelectMethod="DataSetObtenerActividadesAsigAmec"
                    TypeName="EOS.Web.AgenteAmecInfo" EnablePaging="True" SortParameterName="sortParameter"
                    ViewStateMode="Disabled" OnSelecting="odsActividadesAsigAmec_Selecting"></asp:ObjectDataSource>
            </ItemTemplate>
            <%--     </td>


                        </tr>--%>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsAMECs" runat="server" SelectMethod="ObtenerListadoAMECs"
            TypeName="EOS.Web.AgenteAmecInfo" SelectCountMethod="ObtenerNumeroListadoAMECs"
            OnSelecting="odsAMEC_Selecting" SortParameterName="sortParameter" EnablePaging="True">
            <SelectParameters>
                <asp:Parameter Name="filtroAMEC" Type="String" />
                <asp:Parameter Name="filtroSolicitante" Type="String" />
                <asp:Parameter Name="filtroActividad" Type="String" />
                <asp:Parameter Name="filtroAprobacion" Type="String" />
                <asp:Parameter Name="filtroEstadoAmec" Type="String" />
                <asp:Parameter Name="filtroAnyo" Type="String" />
                <asp:Parameter Name="filtroMes" Type="String" />
                <asp:Parameter Name="filtroImporte" Type="String" />
                <asp:Parameter Name="filtroTipoImporte" Type="String" />
                <asp:Parameter Name="filtroParaguas" Type="String" />
                <asp:Parameter Name="filtroIdAreaUsuarioConectado" Type="String" />
                <asp:Parameter Name="filtroIdDistritoUsuarioConectado" Type="String" />
                <asp:Parameter Name="filtroIdUnidadUsuarioConectado" Type="String" />
                <asp:Parameter Name="filtroIdRegionUsuarioConectado" Type="String" />
                <asp:Parameter Name="filtroIdArea" Type="String" />
                <asp:Parameter Name="filtroIdDistrito" Type="String" />
                <asp:Parameter Name="filtroIdUnidad" Type="String" />
                <asp:Parameter Name="filtroIdRegion" Type="String" />
                <asp:Parameter Name="filtroXecUnidadesOrganizativas" Type="String" />
                <asp:Parameter Name="filtroProducto" Type="String" />
                <asp:Parameter Name="filtroFechaInicio" Type="String" />
                <asp:Parameter Name="filtroFechaFin" Type="String" />
                <asp:Parameter Name="filtroIdDistrict" Type="String" />
                <asp:Parameter Name="filtroIdDepartament" Type="String" />
                <asp:Parameter Name="filtroIdSaleForce" Type="String" />
                <asp:Parameter Name="filtroIdPeticionarioSession" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
        <div>
            <div>
                <div class="eosBotonera eosBotonFiltrado">
                    <!--<asp:Button ID="btnVolver" Visible="true" runat="server" Text="Volver"
                        OnClick="btnVolver_Click" />-->
                </div>
            </div>
            <div class="contentFormGeneric separator">
                <div class="excell-icon-container">
                    <asp:ImageButton ID="btnVerExcel" runat="server" ImageUrl="~/Styles/images/iconExcel.png"
                        OnClick="btnVerExcel_Click" class="icon-not-boder"/>
                    <asp:Label ID="Label5" runat="server">Imprimir Amecs Filtrados</asp:Label>
                </div>
                <div class="excell-icon-container">
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Styles/images/iconExcel.png"
                        OnClick="btnVerExcelInforme_Click" class="icon-not-boder"/>
                    <asp:Label ID="Label3" runat="server">Imprimir Informe FCPA</asp:Label>

                </div>              
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="cphScripts" ContentPlaceHolderID="ContentPlaceHolderScripts" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("ul#eosHPBotones li").removeClass("active");
            $("ul#eosHPBotones li#liBuscadorActividades").addClass("active");
        });
    </script>
</asp:Content>