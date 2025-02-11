<%@ Page Title="" Language="C#" MasterPageFile="~/Styles/EOS.Master" AutoEventWireup="True"
    CodeBehind="BuscadorGestorInvitados.aspx.cs" Inherits="EOS.BuscadorGestorInvitados"
    Culture="es-ES" UICulture="es" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <script type="text/javascript" src="<%# ResolveUrl("~/Scripts/eventosForms.js?v=" + System.DateTime.Now.ToString("yyyyMMdd"))%>"></script>

    <script type="text/javascript">
        function pageLoad() {
            var chngCombo = $find('eosContentFilter_DropDownExtender1');
            if (chngCombo != null) {
                var chngPosition = chngCombo._dropPopupPopupBehavior;
                chngPosition.set_positioningMode(2);
            }
        }

        function TramitarReserva(idEventoFormulario) {
            if (idEventoFormulario != null) {

                if (confirm('Este evento tiene gestor de invitados, ¿esta seguro que desea tramitar su reserva en EOS fuera del grupo organizado?'))
                    return true;
                else
                    return false;
            }
            return true;
        }

        function selectAmec(id, idselector) {
            $("#selectedIdAmecs").val(id);
            $("#selectedAmecs").val($('#' + idselector).text());
            $("#searchAmec").val($('#' + idselector).text());
            $("#amecsFound").empty();
        }

        function BindSearch() {
            $("#searchAmec").prop('disabled', true);
            $.ajax({
                type: "POST",
                url: "NuevoExpedientePasoA.aspx/amecsSearch",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{ "search":"' + $("#searchAmec").val().toLowerCase() + '" , "idConfEmpresa":"' + $("#hdnIdConfEmpresa").val() + '", "includeOldAmecs": "' + $("#chkIncludeOldAmecs").is(":checked") + '"}',
                success: function (response) {
                    if (response.d.length > 0) {
                        $("#amecsFound").empty();
                        $(response.d).each(function () {
                            var funcion = "selectAmec('" + this.idamecs + "', '" + this.idamecs.replace(/[\W_]+/g, "") + "')";
                            var appendContent = ('<div class="amecsSelect" id="' + this.idamecs.replace(/[\W_]+/g, "") + '" onclick="' + funcion + '">' + this.AmecConcatSolicitante + '</div>');
                            $("#amecsFound").append(appendContent);
                        })
                    }
                    else {
                        $("#amecsFound").text("No se han encontrado resultados");
                    }
                    $("#searchAmec").prop('disabled', false);
                    $("#searchAmec").focus();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $("#searchAmec").prop('disabled', false);
                    $("#searchAmec").focus();
                    $("#amecsFound").empty();
                    $("#amecsFound").text("Se ha producido un error recuperando datos");
                }
            });
        }

        $(document).ready(function () {
            $("#searchAmec").on("input", function () {
                if ($("#searchAmec").val().length > 7) {
                    BindSearch();
                }
                else {
                    $("#searchAmec").prop('disabled', false);
                    $("#searchAmec").focus();
                    $("#amecsFound").empty();
                }
            });
            $('#chkIncludeOldAmecs').change(function () {
                if ($("#searchAmec").val().length > 7) {
                    BindSearch();
                }
            });
            $('.iconFiltroAvanzado').on('click', function () {
                $(this).toggleClass('active');
                $('#dropAdvancedFilter').slideToggle();
            });
            $('#simplemodal-container').css('width', 'max-content');
            $('#eosContentFilter_eventosFormsCnt').css('height', 'unset');
            if (window.innerWidth >= 768) {
                $('a.modalCloseImg.simplemodal-close').css('right', '-15px');
            }
        });

    </script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="eosContentFilter" runat="server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true">
    </act:ToolkitScriptManager>

    <h1>Buscador Gestor de Invitados
    </h1>
    <asp:Panel ID="plPaso1" runat="server">

        <div id="eosContentFilter">
            <div class="eosTablaFiltros" id="eosFiltroBasico">

                <h2>Filtro de actividad</h2>

                <div class="contentFormGeneric">
                    <div class="contentForm  eosTresColumna">
                        <label class="eosCampoLabel" for="txtNombre">
                            El nombre contiene
                    <div class="tooltip">
                        <span class="masInfo">i</span> <span class="tooltiptext">
                            <p>Nombre de la actividad</p>
                        </span>
                    </div>
                        </label>
                        <asp:TextBox ID="txtNombre" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW180"></asp:TextBox>
                    </div>
                    <div class="contentForm  eosTresColumna">
                        <label class="eosCampoLabel" for="txtCiudad" style="padding-top: 12px;">
                            Ciudad o Sede donde se realiza</label>
                        <asp:DropDownList ID="ddlPoblacion" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW220"
                            DataTextField="Poblacion" DataValueField="IDPoblacion">
                        </asp:DropDownList>
                    </div>

                    <div class="contentForm  eosTresColumna">

                        <label class="eosCampoLabel" for="xxxFiltroAMEC" style="padding-top: 12px;">Tipo Evento</label>
                        <asp:DropDownList ID="ddlTipoActividad" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW180"
                            DataTextField="Nombre" DataValueField="IdTipoActividad">
                        </asp:DropDownList>
                    </div>
                </div>



                <div class="contentFormGeneric">
                    <div class="contentForm  eosTresColumna">
                        <label class="eosCampoLabel" for="txtFechaDesde">Desde</label>

                        <asp:TextBox ID="txtFechaDesde" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW50"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Styles/images/iconCalculadora.png" CssClass="iconCalendar" />
                        <act:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFechaDesde" runat="server"
                            TodaysDateFormat="d MMMM yyyy" FirstDayOfWeek="Monday" PopupButtonID="ImageButton1" />
                    </div>

                    <div class="contentForm  eosTresColumna">
                        <label class="eosCampoLabel" for="txtFechaHasta">Hasta</label>
                        <asp:TextBox ID="txtFechaHasta" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW50"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Styles/images/iconCalculadora.png" CssClass="iconCalendar" />
                        <act:CalendarExtender ID="CalendarExtender2" TargetControlID="txtFechaHasta" runat="server"
                            TodaysDateFormat="d MMMM yyyy" FirstDayOfWeek="Monday" PopupButtonID="ImageButton2" />


                    </div>

                </div>



                <div class="eosBotonera eosBotonFiltrado ">
                    <asp:Button ID="btnFiltrar" runat="server" ImageUrl=""
                        OnClick="btnFiltrar_Click" Text="Filtrar" />
                </div>
            </div>
        </div>



        <div id="eosContentResults" class="contentTableGeneric" runat="server" visible="false">
            <asp:ListView ID="lvActividades" runat="server" DataSourceID="odsActividades" EnableViewState="false"
                OnSelectedIndexChanged="lvActividades_SelectedIndexChanged" DataKeyNames="IDCONGRESO"
                EnablePersistedSelection="True">
                <EmptyDataTemplate>
                    <table class="eosTablaResultados">
                        <thead>
                            <tr>
                                <th>ID
                                </th>
                                <th width="50%">Nombre
                                </th>
                                <th>MSD
                                </th>
                                <th>Lugar
                                </th>
                                <th>Inicio
                                </th>
                                <th>Final
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="6">
                                    <span class="eosTituloRojo">No se han encontrado eventos que cumplan los criterios de
                                búsqueda seleccionados</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <table class="eosTablaResultados">
                        <thead>
                            <tr>
                                <th>
                                    <asp:LinkButton ID="lbId" runat="server" Text="ID" CommandName="Sort" CommandArgument="IDCONGRESO"></asp:LinkButton>
                                </th>
                                <th width="50%" style="text-align: left;">
                                    <asp:LinkButton ID="lbNombre" runat="server" Text="Nombre" CommandName="Sort" CommandArgument="CONGRESO"></asp:LinkButton>
                                </th>
                                <th style="text-align: left;">
                                    <!-- GESTOR DE INVITADOS -->
                                </th>
                                <th style="text-align: left;">
                                    <asp:LinkButton ID="lbPoblacion" runat="server" Text="Lugar" CommandName="Sort" CommandArgument="POBLACION"></asp:LinkButton>
                                </th>
                                <th style="text-align: left;">
                                    <asp:LinkButton ID="lbFechaDesde" runat="server" Text="Desde" CommandName="Sort" CommandArgument="DESDE"></asp:LinkButton>
                                </th>
                                <th style="text-align: left;">
                                    <asp:LinkButton ID="lbFechaHasta" runat="server" Text="Hasta" CommandName="Sort" CommandArgument="HASTA"></asp:LinkButton>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </tbody>

                    </table>

                    <div class="contentPagination" style="display: none">
                        <asp:DataPager ID="DataPager3" runat="server" PageSize="10000">
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
                        <td class="par">
                            <asp:Label ID="idActividadID" runat="server" Text='<%# Eval("idcongreso") %>' />
                        </td>
                        <td>
                            <asp:Label ID="nombreLabel" runat="server" Text='<%# Eval("congreso") %>' />
                        </td>
                        <td align="center" <%# (Eval("IdEventoFormulario") != null) ? "style=\"padding:0;\"" : "" %>>
                            <img id="Img1" alt="grupos" title="El congreso tiene un formulario de gestor de invitados"
                                style="width: 16px" src="Styles/images/ic_mundo.png" runat="server" visible='<%# Eval("IdEventoFormulario") != null %>' />
                            <asp:ImageButton ID="BtnPost" Visible='<%# Eval("IdEventoFormulario") != null %>'
                                runat="server" AlternateText='<%# String.Format("{0}|{1}|{2}", Eval("idCongreso"), Eval("idconfempresa"), Eval("LinkGestorInvitados")) %>'
                                ToolTip="Ir a formulario gestor de invitados" ImageUrl="~/Styles/images/ic_amec.png"
                                OnClick="lnkbtnGRUPOS_Click" />
                        </td>
                        <td class="par">
                            <asp:Label ID="poblacionLabel" runat="server" Text='<%# Eval("poblacion") %>' />
                        </td>
                        <td>
                            <asp:Label ID="fechainicioLabel" runat="server" Text='<%# String.Format("{0:dd/MM/yy}",Eval("desde")) %>' />
                        </td>
                        <td>
                            <asp:Label ID="fechafinLabel" runat="server" Text='<%# String.Format("{0:dd/MM/yy}",Eval("hasta")) %>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
            <asp:ObjectDataSource ID="odsActividades" runat="server" SelectMethod="ObtenerActividades" EnableViewState="false"
                TypeName="EOS.Web.AgenteExpedientes" SelectCountMethod="ObtenerNumeroActividades"
                EnablePaging="True" SortParameterName="sortParameter" OnSelecting="odsActividades_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="filtroID" Type="String" />
                    <asp:Parameter Name="filtroNombre" Type="String" />
                    <asp:Parameter Name="filtroPoblacion" Type="String" />
                    <asp:Parameter Name="filtroAMEC" Type="String" />
                    <asp:Parameter Name="filtroTipoActividad" Type="String" />
                    <asp:Parameter Name="filtroFechaDesde" Type="String" />
                    <asp:Parameter Name="filtroFechaHasta" Type="String" />
                    <asp:Parameter Name="isGestorInvitados" Type="Boolean" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
        <%--GESTOR INVITADOS OGP "19/09/2012"--%>
        <div id="eventosFormsCnt" runat="server" visible="false">
            <asp:ListView ID="lvEventosForms" runat="server" DataSourceID="odsEventosForms" EnableViewState="false"
                OnSelectedIndexChanged="lvEventosForms_SelectedIndexChanged" DataKeyNames="IdEventoFormulario"
                EnablePersistedSelection="True">
                <EmptyDataTemplate>
                    <table class="eosTablaResultados" style="width: 580px;">
                        <thead>
                            <tr>
                                <th></th>
                                <th width="30%">Descripci&oacute;n
                                </th>
                                <th>Inicio
                                </th>
                                <th>Final
                                </th>
                                <th>Lugar
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="6">
                                    <span class="eosTituloRojo">No se han encontrado eventos que cumplan los criterios de
                                búsqueda seleccionados</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <table class="eosTablaResultados" style="width: 580px;">
                        <thead>
                            <tr>
                                <th style="text-align: left;"></th>
                                <th style="width: 30%; text-align: left;">
                                    <asp:LinkButton ID="lnkbDescripcion" runat="server" Text="Descripci&oacute;n" CommandName="Sort"
                                        CommandArgument="DESCRIPCIONGESTOR" OnClick="EventosFormsHeader_Click"></asp:LinkButton>
                                </th>
                                <th style="text-align: left;">
                                    <asp:LinkButton ID="lnkFechaDesde" runat="server" Text="Desde" CommandName="Sort"
                                        CommandArgument="FECHAINICIO" OnClick="EventosFormsHeader_Click"></asp:LinkButton>
                                </th>
                                <th style="text-align: left;">
                                    <asp:LinkButton ID="lnkFechaHasta" runat="server" Text="Hasta" CommandName="Sort"
                                        CommandArgument="FECHAFIN" OnClick="EventosFormsHeader_Click"></asp:LinkButton>
                                </th>
                                <th style="text-align: left;">
                                    <asp:LinkButton ID="lnkPoblacion" runat="server" Text="Lugar" CommandName="Sort"
                                        CommandArgument="POBLACION" OnClick="EventosFormsHeader_Click"></asp:LinkButton>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </tbody>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:ImageButton ID="imgBtActividadDetalle" class="goMSDBtnClass" runat="server"
                                AlternateText='<%# String.Format("{0}|{1}|{2}", Eval("idEventoFormulario"), Eval("idconfempresa"), Eval("LinkGestorInvitados")) %>' ToolTip="Ir a gestor de invitados"
                                ImageUrl="~/Styles/images/ic_amec.png" />
                        </td>
                        <td class="par">
                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Eval("descripciongestor") %>' />
                        </td>
                        <td class="par">
                            <asp:Label ID="lblFechaInicio" runat="server" Text='<%# String.Format("{0:dd/MM/yy}",Eval("fechainicio")) %>' />
                        </td>
                        <td>
                            <asp:Label ID="lblFechaFin" runat="server" Text='<%# String.Format("{0:dd/MM/yy}",Eval("fechafin")) %>' />
                        </td>
                        <td>
                            <asp:Label ID="lblPoblacion" runat="server" Text='<%# Eval("poblacion") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
            <asp:ObjectDataSource ID="odsEventosForms" runat="server" SelectMethod="ObtenerDatosEventosFormulario" EnableViewState="false"
                TypeName="EOS.Web.AgenteExpedientes" EnablePaging="True" SortParameterName="sortParameter"
                OnSelecting="odsEventosForms_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="filtroID" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
        <p />

    </asp:Panel>
    <asp:Panel ID="plPaso2" runat="server" Visible="false">
        <div id="eosDatosActividadFiltrada" class="eosFiltroBasico">

            <div class="eosTablaFiltros" id="eosTablaDatosActividad">


                <h2>Filtro de actividad</h2>

                <div class="contentFormGeneric">
                    <label class="eosCampoLabel fullWidth100" for="txtPedido">
                        Actividad</label>
                    <div class="contentForm  eosMitadColumna paddingReset">

                        <asp:TextBox ID="txtIdCongreso" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW120"
                            ReadOnly="true"></asp:TextBox>
                    </div>

                    <div class="contentForm  eosMitadColumna">
                        <asp:TextBox ID="txtCongreso" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW530"
                            ReadOnly="true"></asp:TextBox>
                    </div>

                </div>

                <div class="eosBotonera eosBotonFiltrado ">
                    <asp:Button ID="btnCambiarActividad" runat="server" ImageUrl=""
                        OnClick="btnCambiarActividad_Click" TabIndex="3" Text="Cambiar actividad" />
                </div>



            </div>
        </div>
        <p />
    </asp:Panel>
    <!-- nuevo código -->
    <asp:Panel ID="plGRUPOS" runat="server" Visible="false">
        <div id="divGRUPOS">
            <asp:LinkButton ID="lnkbtnGRUPOS" runat="server" Text="Ir a GRUPOS" OnClick="lnkbtnGRUPOS_Click"
                Visible="False" />
        </div>
    </asp:Panel>
    <!-- fin nuevo código-->
    <asp:Panel ID="plPaso3" runat="server" Visible="false">

        <div id="eosDatosExpedientes" class="eosFiltroBasico">
            <span class="failureNotification"></span>



            <div class="eosTablaFiltros fullWidth" id="eosTablaDatosExpediente">

                <div class="contentFormGeneric">
                    <div class="contentForm  eosMitadColumna">

                        <!-- Ismael Amneller 24/02/2011 Cambio del nombre de la label -->
                        <label class="eosCampoLabel" for="txtPedido">
                            NetworkActivity/Centro de Coste <span class="eosCampoObligatorio">*</span></label>
                        <%--<asp:TextBox ID="txtPedido" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW170"></asp:TextBox>--%>
                        <asp:DropDownList ID="ddlPedido" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                            DataTextField="CentroCoste" DataValueField="CentroCoste">
                        </asp:DropDownList>
                    </div>





                    <div class="contentForm  eosMitadColumna" id="divTipoPago" runat="server">
                        <asp:Label ID="lblTipoPagoDropDownList" CssClass="eosCampoLabelSin" runat="server" AssociatedControlID="tipoPagoDropDownList">Tipo de pago: <span class="eosCampoObligatorio">*</span></asp:Label>
                        <asp:DropDownList ID="tipoPagoDropDownList" runat="server">
                            <asp:ListItem Value="0">Seleccione un tipo de pago</asp:ListItem>
                            <asp:ListItem Value="1">Orden de compra</asp:ListItem>
                            <asp:ListItem Value="2">Tarjeta de crédito</asp:ListItem>
                            <asp:ListItem Value="3">Pago Ponente (Calculadoras)</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="contentFormGeneric">


                    <div class="contentForm  eosMitadColumna">


                        <div colspan="2" class="conInfo">


                            <label class="eosCampoLabel">
                                <%= System.Configuration.ConfigurationManager.AppSettings["codigoPresupuesto"].ToString() %>
                                <span class="eosCampoObligatorio">*</span></label>


                            <asp:ImageButton ID="DetalleAmec" runat="server" ImageUrl="~/Styles/images/ic_lupa.png"
                                OnClick="imgDetalleAmec_Click" Visible="true" ToolTip="Ver el Detalle del Amec" />
                            <div class="tooltip">
                                <span class="masInfo">i</span> <span class="tooltiptext">
                                    <p>Código de actividad EM</p>
                                    <p>Esta página de búsqueda permite identificar todos los eventos que tengan fecha de finalización en el año en curso. Con este cambio el proceso de identificación del EM será más ágil.</p>
                                    <p>En caso de querer buscar un evento con fecha de fin en otro año, es necesario seleccionar la casilla de Búsqueda avanzada.</p>
                                </span>
                            </div>

                            <!-- Jose Laguna este cuadro de texto, lo ocultamos en función de si la aplicación tiene control de presupuestos o no -->
                            <asp:TextBox MaxLength="9" ID="txtAreaNuevoAMEC" runat="server" CausesValidation="True"
                                ToolTip="Introduce el número" Visible="false"></asp:TextBox>
                            <!-- Jose Laguna esta lista de selección, la mostramos en función de si la aplicación tiene control de presupuestos o no -->
                            <asp:DropDownList ID="ddlAmecPorCongreso" Visible="false" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW730" AutoPostBack="True"
                                DataTextField="AmecConcatSolicitante" DataValueField="idamecs" OnSelectedIndexChanged="ddlAmecPorCongreso_OnSelectedIndexChanged">
                            </asp:DropDownList>
                            <%--                        <input type="text" id="searchAmec" placeholder="Busca un Amec" class="eosSizeW730"/>--%>
                            <asp:TextBox runat="server" ID="searchAmec" ClientIDMode="Static" placeholder="Copie y pegue el código del Amec sin espacios (ejemplo: EM1810-0011111) " Enabled="false" CssClass="eosSizeW730" />
                            <div id="amecsFound"></div>
                            <asp:HiddenField ID="selectedIdAmecs" ClientIDMode="Static" Value="" OnValueChanged="selectedAmecs_Change" runat="server" />
                            <asp:HiddenField ID="selectedAmecs" ClientIDMode="Static" Value="" OnValueChanged="selectedAmecs_Change" runat="server" />
                            <asp:HiddenField ID="hdnIdConfEmpresa" ClientIDMode="Static" Value="" runat="server" />
                            &nbsp;&nbsp;&nbsp;

                        </div>

                    </div>





                    <div class="contentForm  eosMitadColumna contentFormRadio">


                        <asp:CheckBox ID="chkIncludeOldAmecs" runat="server" CssClass="eosDisabledInputVacio eosCampoLabel radioCheck"
                            Text="Búsqueda avanzada" ToolTip="Búsqueda avanzada" ClientIDMode="Static" />

                        <asp:CheckBox ID="chkUrgente" runat="server" CssClass="eosDisabledInputVacio eosCampoLabel  radioCheck"
                            Text="Urgente" ToolTip="Urgente" />
                    </div>

                </div>

            </div>




            <asp:PlaceHolder runat="server" ID="plhShowAgency" Visible="false">
                <div>
                    <label class="eosCampoLabel" for="txtAgencias">Agencia<span class="eosCampoObligatorio">*</span> &nbsp;</label>
                    <asp:DropDownList ID="ddlAgencias" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW160"
                        AutoPostBack="false" Enabled="true">
                    </asp:DropDownList>
                </div>
            </asp:PlaceHolder>
            <div class="contentError">
                <!--<label>* Busqueda de Amec - Introduce al menos 8 caracteres para comenzar la búsqueda y selecciona el Amec deseado de la lista de resultados antes de continuar</label>-->
                <label>* Tenga en cuenta que el buscador puede tardar unos segundos. Durante este tiempo, por favor, no toque ningún botón ni realice ninguna acción adicional. Una vez aparezca el resultado, pulse sobre él para que se añada al campo AMEC.</label>

            </div>
            <!-- <tr>
                    <td class="comment">
                        <label>* Es necesario que los amec tengan una agencia asignada antes de crear un expediente</label></td>
                </tr>-->


        </div>
    </asp:Panel>
    <asp:Panel ID="Panel1" runat="server" Visible="false">
        <div id="Div1" class="eosFiltroBasico">
            <div class="eosTablaFiltros" id="Table1">
                <div class="contentFormGeneric">
                    <div class="contentForm  eosMitadColumna">
                        <label class="eosCampoLabel" for="xxxFiltroAMEC">
                            Vinculado a <span class="eosCampoObligatorio">*</span></label>
                        <asp:DropDownList ID="ddltiposDeProductos" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                            DataTextField="Nombre" DataValueField="IdTipoProducto">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <div style="display: none;">
        <div>
            <asp:Table ID="grdProductos" runat="server" Width="400px" BackColor="White" BorderStyle="Solid"
                BorderColor="#999999" BorderWidth="1px">
            </asp:Table>
        </div>
    </div>
    <p />
    <div class="eosBotonera eosBotonFiltrado ">
        <asp:Button ID="btnContinuar" runat="server" ImageUrl=""
            OnClick="btnContinuar_Click" ValidationGroup="AMECValidationGroup" TabIndex="1" Visible="false" Text="Continuar" />
        <asp:Button ID="btnGuardar" runat="server" ClientIDMode="Static" ImageUrl=""
            OnClientClick="confirma();" OnClick="btnGuardar_Click" Visible="false" Text="Guardar" />
    </div>
</asp:Content>
<asp:Content ID="cphScripts" ContentPlaceHolderID="ContentPlaceHolderScripts" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("ul#eosHPBotones li").removeClass("active");
            $("ul#eosHPBotones li#liBuscadorGestorInvitados").addClass("active");            
        });
    </script>
</asp:Content>
