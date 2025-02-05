<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Styles/EOS.Master"
    CodeBehind="FiltroInforme.aspx.cs" Inherits="EOS.FiltroInforme" Culture="es-ES"
    UICulture="es" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <title>INFORME EOS</title>
    <link href="<%# ResolveUrl("~/Styles/style.css?v=" + System.DateTime.Now.ToString("yyyyMMdd"))%>" rel="stylesheet" type="text/css" />
    <link href="<%# ResolveUrl("~/Styles/custom.css?v=" + System.DateTime.Now.ToString("yyyyMMdd"))%>" rel="stylesheet" type="text/css" />
    <link href="<%# ResolveUrl("~/Informes/src/epoch_styles.css?v=" + System.DateTime.Now.ToString("yyyyMMdd"))%>" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="src/epoch_classes.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="eosHeaderInfoBasica" runat="server">
    <div>
        <div id="eosHPDatos">
            <div id="eosHeaderInfoBasica_eosNombreContacto">
                Quodem Consultores (quodemsupport@quodem.com)
            </div>
            <asp:Repeater runat="server" ID="rptEmpleadosGP">
                <ItemTemplate>
                    <div style="text-transform: uppercase">
                        <div class="logoTecnicos">
                            <asp:Image ID="imgLogo" runat="server" ImageUrl='<%# "~/Styles/images/" + Eval("logosuperiorpantalla") %>'
                                AlternateText='<%#Eval("nombreagencia")%>' />
                        </div>
                        <span style="font-weight: bold;">TÉCNICO: </span>
                        <%#Eval("Nombre") + " " + Eval("Apellido")%>&nbsp;|&nbsp;<a class="logoEmail" href="mailto:<%#Eval("Email")%>"><img height="15px" width="15px" alt="" src="/Styles/images/ic_correo.png" /><%#Eval("Email")%></a>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

        </div>
    </div>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="eosContentFilter" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>
    <div id="eosContentFilter1">
        <h1>Informes y listados</h1>
        <div runat="server" class="hidden eosTablaFiltros">
            <div>
                <asp:Label ID="lbPeticionarioLogueado" runat="server" Text="Peticionario Logueado:"></asp:Label>
                <%Response.Write(peticionario.Nombre + " " + peticionario.Apellidos);%>
            </div>
            <div>
                <asp:Label ID="lbCargopeticionario" runat="server" Text="Cargo peticionario:"></asp:Label>
                <%Response.Write(peticionario.Rol);%>
            </div>
            <div>
                <asp:Label ID="lbUnidadpeticionario" runat="server" Text="Unidad peticionario:"></asp:Label>
                <%Response.Write(peticionario.NomUnidad);%>
            </div>
            <div>
                <asp:Label ID="lbAreapeticionario" runat="server" Text="Area peticionario:"></asp:Label>
                <%Response.Write(peticionario.NomArea);%>
            </div>
            <div>
                <asp:Label ID="lbRegionpeticionario" runat="server" Text="Region peticionario:"></asp:Label>
                <%Response.Write(peticionario.NomRegion);%>
            </div>
            <div>
                <asp:Label ID="lbDistritopeticionario" runat="server" Text="Distrito* peticionario:"></asp:Label>
                <%Response.Write(peticionario.NomDistrito);%>
            </div>
            <div>
                <asp:Label ID="lblDepartamentPeticionario" runat="server" Text="Departamento peticionario:"></asp:Label>
                <%Response.Write(peticionario.NomDepartament);%>
            </div>
            <div>
                <asp:Label ID="lblFuerzaVentasPeticionario" runat="server" Text="Fuerza de ventas peticionario:"></asp:Label>
                <%Response.Write(peticionario.NomFuerzaVentas);%>
            </div>
            <div>
                <asp:Label ID="lbDistritpeticionario" runat="server" Text="Distrito peticionario:"></asp:Label>
                <%Response.Write(peticionario.NomDistrict);%>
            </div>
        </div>
        <div runat="server" class="eosTablaFiltros">
            <h2>Buscador de informes</h2>
            <div class="contentFormGeneric">
                <div class="eosMitadColumna">
                    <asp:Label ID="lbUnidad" runat="server" Text="Unidad" for="ddlUnidad" CssClass="eosCampoLabel"></asp:Label>
                    <asp:DropDownList ID="ddlUnidad" runat="server" CssClass="eosInputVacio eosSizeW270"
                        OnSelectedIndexChanged="ddlUnidad_Changed" AutoPostBack="True">
                    </asp:DropDownList>
                </div>
                <div class="eosMitadColumna">
                    <asp:Label ID="lblDepartament" runat="server" Text="Departamento" for="ddlDepartamento" CssClass="eosCampoLabel"></asp:Label>
                    <asp:DropDownList ID="ddlDepartament" runat="server" CssClass="eosInputVacio eosSizeW270"
                        OnSelectedIndexChanged="ddlDepartament_OnSelectedIndexChanged" AutoPostBack="True">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="contentFormGeneric">
                <div class="eosMitadColumna">
                    <asp:Label ID="lbArea" runat="server" Text="Area" CssClass="eosCampoLabel"></asp:Label>
                    <asp:DropDownList ID="ddlArea" runat="server" CssClass="eosInputVacio eosSizeW270"
                        OnSelectedIndexChanged="ddlArea_Changed" AutoPostBack="True">
                    </asp:DropDownList>
                </div>
                <div class="eosMitadColumna">
                    <asp:Label ID="lblSaleForce" runat="server" Text="Fuerza de venta" CssClass="eosCampoLabel"></asp:Label>
                    <asp:DropDownList ID="ddlSaleForce" runat="server" CssClass="eosInputVacio eosSizeW270"
                        OnSelectedIndexChanged="ddlSaleForce_OnSelectedIndexChanged" AutoPostBack="True">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="contentFormGeneric">
                <div class="eosMitadColumna">
                    <asp:Label ID="lbRegion" runat="server" Text="Region" CssClass="eosCampoLabel"></asp:Label>
                    <asp:DropDownList ID="ddlRegion" runat="server" CssClass="eosInputVacio eosSizeW270"
                        OnSelectedIndexChanged="ddlRegion_Changed" AutoPostBack="True">
                    </asp:DropDownList>
                </div>
                <div class="eosMitadColumna">
                    <asp:Label ID="lblDistrict" runat="server" Text="Distrito" CssClass="eosCampoLabel"></asp:Label>
                    <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="eosInputVacio eosSizeW270"
                        OnSelectedIndexChanged="ddlRegion_Changed" AutoPostBack="True">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="contentFormGeneric">
                <div class="eosTresColumna">
                    <asp:Label ID="lbDistrito" runat="server" Text="Distrito*" ToolTip="* Antes de Julio de 2017" CssClass="eosCampoLabel"></asp:Label>
                    <asp:DropDownList ID="ddlDistrito" runat="server" CssClass="eosInputVacio eosSizeW270"
                        OnSelectedIndexChanged="ddlDistrito_Changed" AutoPostBack="True">
                    </asp:DropDownList>
                </div>
                <div class="eosTresColumna">
                    <asp:Label ID="lbPeticionario" runat="server" Text="Peticionario" CssClass="eosCampoLabel"></asp:Label>
                    <asp:DropDownList ID="ddlPeticionario" runat="server" CssClass="eosInputVacio eosSizeW270"
                        OnSelectedIndexChanged="ddlPeticionario_Changed" AutoPostBack="True">
                    </asp:DropDownList>
                </div>
                <div class="eosTresColumna">
                    <asp:Label ID="lbProducto" runat="server" Text="Producto" Visible="true" CssClass="eosCampoLabel"></asp:Label>
                    <asp:DropDownList ID="ddltiposDeProductos" runat="server" CssClass="eosInputVacio eosSizeW270"
                        OnSelectedIndexChanged="ddlProducto_Changed" Visible="true">
                    </asp:DropDownList>
                </div>
            </div>

            <div class="contentFormGeneric">
                <asp:Label ID="ToolTip" runat="server" Text="* Antes de julio de 2017" CssClass="eosCampoLabel"></asp:Label>
                <!-- AJAX TOOLKIT -->
                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" TargetControlID="txtCongreso"
                    ServiceMethod="GetCompletionListEspecialidades" UseContextKey="True" CompletionSetCount="20"
                    DelimiterCharacters=";, :" EnableCaching="true" MinimumPrefixLength="1" ServicePath="AutoComplete.asmx"
                    ShowOnlyCurrentWordInCompletionListItem="true">
                </asp:AutoCompleteExtender>
            </div>
        </div>
    </div>
    <asp:Panel ID="plPaso1" runat="server">
        <div class="eosTituloWizard">
            1º Selecciona Evento
        </div>
        <div id="eosContentFilter">
            <div class="eosTablaFiltros" id="eosFiltroBasico">
                <div class="contentFormGeneric">
                    <span class="eosTituloNormal eosCampoLabel">DEFINA LOS FILTROS DEL LISTADO SI ES NECESARIO Y HAGA
                            CLICK EN 'FILTRAR'</span>
                </div>
                <div class="contentFormGeneric">
                    <div class="eosMitadColumna">
                        <label for="txtNombre1">
                            El nombre contiene</label>

                        <asp:TextBox ID="txtNombre1" runat="server" MaxLength="80" CssClass="eosInputVacio eosSizeW190"></asp:TextBox>
                    </div>
                    <div class="eosMitadColumna">
                        <label for="txtCiudad">
                            Ciudad o Sede donde se realiza</label>
                        <asp:DropDownList ID="ddlPoblacion" runat="server" CssClass="eosInputVacio eosSizeW170"
                            DataTextField="Poblacion" DataValueField="IDPoblacion">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="contentFormGeneric">
                    <div class="eosMitadColumna">
                        <label for="txtAMEC">
                            AMEC</label>
                        <asp:TextBox ID="txtAMEC" runat="server" MaxLength="80" autocomplete="off" CssClass="eosInputVacio eosSizeW170"></asp:TextBox>
                    </div>
                    <div class="eosMitadColumna">
                        <label for="xxxFiltroAMEC">
                            Tipo Evento</label>
                        <asp:DropDownList ID="ddlTipoActividad" runat="server" CssClass="eosInputVacio eosSizeW190"
                            DataTextField="Nombre" DataValueField="IdTipoActividad">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="contentFormGeneric">
                    <div class="eosMitadColumna centerCalendar">
                        <eos:InputDatePickerControl ID="txtFechaActDesde" runat="server" Text="Desde" class="iconCalendar" CssClass="eosDisabledInputVacio style1" />
                    </div>
                    <div class="eosMitadColumna centerCalendar">
                        <eos:InputDatePickerControl ID="txtFechaActHasta" runat="server" Text="Hasta" class="iconCalendar" CssClass="eosDisabledInputVacio style1" />
                    </div>
                </div>
            </div>
        </div>
        <div class="eosTablaFiltros" id="eosFiltroBotones">
            <div>
                <div class="eosBotonera eosBotonFiltrado">
                    <asp:Button ID="btnFiltrar" runat="server"
                        OnClick="btnFiltrar_Click" Text="Filtrar" />
                </div>
            </div>
        </div>
        <div id="eosContentResults" runat="server" visible="false" class="contentTableGeneric">
            <!--  -->
            <!-- Pau Ferrer  08-06-2011 quito el EnableViewState= false-->
            <asp:ListView ID="lvActividades" runat="server" OnSelectedIndexChanged="lvActividades_SelectedIndexChanged"
                EnablePersistedSelection="True" DataKeyNames="IDCONGRESO">
                <EmptyDataTemplate>
                    <table class="eosTablaResultados">
                        <thead>
                            <tr>
                                <th></th>
                                <th>ID
                                </th>
                                <th width="50%">Nombre
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
                                <th></th>
                                <th style="text-align: left;">
                                    <asp:LinkButton ID="lbId" runat="server" Text="ID" CommandName="Sort" CommandArgument="IDCONGRESO"></asp:LinkButton>
                                </th>
                                <th width="50%" style="text-align: left;">
                                    <asp:LinkButton ID="lbNombre" runat="server" Text="Nombre" CommandName="Sort" CommandArgument="CONGRESO"></asp:LinkButton>
                                </th>
                                <th style="text-align: left;">
                                    <asp:LinkButton ID="lbPoblacion" runat="server" Text="Lugar" CommandName="Sort" CommandArgument="POBLACION"></asp:LinkButton>
                                </th>
                                <th style="text-align: left;">
                                    <asp:LinkButton ID="lbFechaDesde" runat="server" Text="Desde" CommandName="Sort"
                                        CommandArgument="DESDE"></asp:LinkButton>
                                </th>
                                <th style="text-align: left;">
                                    <asp:LinkButton ID="lbFechaHasta" runat="server" Text="Hasta" CommandName="Sort"
                                        CommandArgument="HASTA"></asp:LinkButton>
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
                                    ShowPreviousPageButton="True" FirstPageText="" PreviousPageText="" PreviousPageImageUrl="~/Styles/images/ic_pagina_anterior.png"
                                    FirstPageImageUrl="~/Styles/images/ic_pagina_primera.png" RenderDisabledButtonsAsLabels="True" />
                                <asp:NumericPagerField CurrentPageLabelCssClass="pageSel" />
                                <asp:NextPreviousPagerField ButtonType="Image" ShowLastPageButton="True" ShowNextPageButton="True"
                                    ShowPreviousPageButton="False" LastPageText="" NextPageText="" LastPageImageUrl="~/Styles/images/ic_pagina_ultima.png"
                                    NextPageImageUrl="~/Styles/images/ic_pagina_siguiente.png" RenderDisabledButtonsAsLabels="True" />
                            </Fields>
                        </asp:DataPager>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:ImageButton ID="imgActividadDetalle" runat="server" CommandName="Select" ToolTip='<%# String.Format("Seleccionar Evento {0}", Eval("idcongreso")) %>'
                                ImageUrl="~/Styles/images/ic_amec.png" />
                        </td>
                        <td class="par">
                            <asp:Label ID="idActividadID" runat="server" Text='<%# Eval("idcongreso") %>' />
                        </td>
                        <td>
                            <asp:Label ID="nombreLabel" runat="server" Text='<%# Eval("congreso") %>' />
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
            <asp:ObjectDataSource ID="odsActividades" runat="server" SelectMethod="ObtenerActividades1"
                TypeName="EOS.Web.AgenteExpedientes" SelectCountMethod="ObtenerNumeroActividades1"
                EnablePaging="True" SortParameterName="sortParameter" OnSelecting="odsActividades_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="filtroID" Type="String" />
                    <asp:Parameter Name="filtroNombre1" Type="String" />
                    <asp:Parameter Name="filtroPoblacion" Type="String" />
                    <asp:Parameter Name="filtroAMEC" Type="String" />
                    <asp:Parameter Name="filtroTipoActividad" Type="String" />
                    <asp:Parameter Name="filtroFechaDesde" Type="String" />
                    <asp:Parameter Name="filtroFechaHasta" Type="String" />
                    <asp:Parameter Name="isGestorInvitados" Type="Boolean" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </asp:Panel>
    <asp:Table ID="Table1" runat="server" CssClass="eosFiltroBasico " Width="935px" BorderWidth="0"
        CellSpacing="0" CellPadding="0" >
        <asp:TableRow>
            <asp:TableCell Width="50px">&nbsp;</asp:TableCell>
            <asp:TableCell Width="700px">
                <asp:Table runat="server" CssClass="eosTablaFiltros" Width="100%">
                    <asp:TableRow Height="35px">
                        <asp:TableCell ColumnSpan="2">
                            <div class="eosTablaFiltros">
                                <div class="contentFormGeneric">
                                    <div class="contentForm eosMitadColumna">
                                        <label>Congreso</label>
                                    </div>
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow Height="40px" VerticalAlign="Top">
                        <asp:TableCell ColumnSpan="2">
                            <div class="eosTablaFiltros">
                                <div class="contentFormGeneric">
                                    <div class="contentForm eosMitadColumna">
                                        <asp:TextBox ID="txtIdCongreso" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW120"
                                            Disabled="true"></asp:TextBox>
                                    </div>                                
                                    <div class="contentForm eosMitadColumna">
                                        <asp:TextBox ID="txtCongreso" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW530"
                                            Disabled="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow Height="25px" Visible="false">
                        <asp:TableCell Width="150" Visible="false">
                            <asp:Label ID="lbAmec" runat="server" Visible="false">AMEC</asp:Label>
                        </asp:TableCell>
                        <asp:TableCell Width="200px" Visible="false">
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Debe seleccionar un AMEC válido"
                                Visible="false" ControlToCompare="lblAMEC" ValueToCompare="lalala"></asp:CompareValidator>
                            <asp:TextBox CssClass="eosInputVacio eosSizeW210" MaxLength="80" ID="lblAMEC" runat="server"
                                Visible="false">(Seleccione un AMEC válido)</asp:TextBox>
                            <act:DropDownExtender ID="DropDownExtender1" runat="server" TargetControlID="lblAMEC"
                                DropDownControlID="pnlGrid" Enabled="true" DynamicServicePath="">
                            </act:DropDownExtender>
                            <asp:Panel runat="server" ID="pnlGrid" BorderColor="Aqua" BorderWidth="1" Visible="false">
                                <asp:ListView ID="lvAmecs" runat="server" DataSourceID="odsAmecs" DataKeyNames="IDAMEC"
                                    EnablePersistedSelection="True" OnSelectedIndexChanged="lvAmecs_SelectedIndexChanged">
                                    <LayoutTemplate>
                                        <table class="eosTablaResultados" style="width: 300px;">
                                            <thead>
                                                <tr>
                                                    <th></th>
                                                    <th>AMEC
                                                    </th>
                                                    <th>PETICIONARIO
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
                                                <asp:ImageButton ID="ImageButton3" runat="server" CommandName="Select" ToolTip='<%# String.Format("Seleccione el AMEC {0}", Eval("amec")) %>'
                                                    ImageUrl="~/Styles/images/ic_amec.png" />
                                            </td>
                                            <td class="par">
                                                <asp:Label ID="amecLabel" runat="server" Text='<%# Eval("amec") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("peticionario") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                                <asp:ObjectDataSource ID="odsAmecs" runat="server" SelectMethod="ObtenerAMECPorActividad"
                                    TypeName="EOS.Web.AgenteExpedientes">
                                    <FilterParameters>
                                        <asp:Parameter Name="filtroIDCongreso" />
                                    </FilterParameters>
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="txtIdCongreso" Name="filtroIdCongreso" PropertyName="Text"
                                            Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br />
    <asp:Panel ID="plPaso3_Ind2" runat="server" Visible="true">
        <div id="Div1">
            <asp:Table ID="emptyTable" runat="server" Visible="false">
                <asp:TableRow>
                    <asp:TableCell>&nbsp;</asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:ListView ID="lvServicioParticipantes" runat="server" DataKeyNames="IdPassengerlist"
                EnablePersistedSelection="True" Visible="false" OnSelectedIndexChanged="lvServicioParticipantes_SelectedIndexChanged">
                <EmptyDataTemplate>
                    <div class="eosTablaResultados">
                        <div>
                            <span>Nombre
                            </span>
                            <span>Apellido 1
                            </span>
                            <span>Apellido 2
                            </span>
                            <span>Hospital
                            </span>
                            <span>MSDID
                            </span>
                            <span></span>
                        </div>
                        <div>
                            <span class="eosTituloRojo">&nbsp;</span>
                        </div>
                    </div>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <div class="eosTablaResultados">
                        <div>
                            <asp:LinkButton ID="lbNombre" runat="server" Text="Nombre" CommandName="Sort" CommandArgument="NOMBRE"></asp:LinkButton>

                            <asp:LinkButton ID="lbApel1" runat="server" Text="Apellido 1" CommandName="Sort"
                                CommandArgument="APEL1"></asp:LinkButton>

                            <asp:LinkButton ID="lbApel2" runat="server" Text="Apellido 2" CommandName="Sort"
                                CommandArgument="APEL2"></asp:LinkButton>

                            <asp:LinkButton ID="lbHospital" runat="server" Text="Hospital" CommandName="Sort"
                                CommandArgument="HOSPITAL"></asp:LinkButton>

                            <asp:LinkButton ID="lbMSDID" runat="server" CommandName="Sort" CommandArgument="MSDID"><%= System.Configuration.ConfigurationManager.AppSettings["codigoInvitado"].ToString() %></asp:LinkButton>
                        </div>
                        <div>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </div>
                        <div>
                            <asp:DataPager ID="DataPager3" runat="server" PageSize="10">
                                <Fields>
                                    <asp:NextPreviousPagerField ButtonType="Image" ShowFirstPageButton="True" ShowNextPageButton="False"
                                        ShowPreviousPageButton="True" FirstPageText="" PreviousPageText="" PreviousPageImageUrl="~/Styles/images/ic_pagina_anterior.png"
                                        FirstPageImageUrl="~/Styles/images/ic_pagina_primera.png" RenderDisabledButtonsAsLabels="True" />
                                    <asp:NumericPagerField CurrentPageLabelCssClass="pageSel" />
                                    <asp:NextPreviousPagerField ButtonType="Image" ShowLastPageButton="True" ShowNextPageButton="True"
                                        ShowPreviousPageButton="False" LastPageText="" NextPageText="" LastPageImageUrl="~/Styles/images/ic_pagina_ultima.png"
                                        NextPageImageUrl="~/Styles/images/ic_pagina_siguiente.png" RenderDisabledButtonsAsLabels="True" />
                                </Fields>
                            </asp:DataPager>
                        </div>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Label ID="nombreLabel" runat="server" Text='<%# Eval("nombre") %>' />
                        </td>
                        <td class="par">
                            <asp:Label ID="apel1Label" runat="server" Text='<%# Eval("apel1") %>' />
                        </td>
                        <td>
                            <asp:Label ID="apel2Label" runat="server" Text='<%# Eval("apel2") %>' />
                        </td>
                        <td class="par">
                            <asp:Label ID="hospitalLabel" runat="server" Text='<%# Eval("hospital") %>' />
                        </td>
                        <td>
                            <asp:Label ID="msdidLabel" runat="server" Text='<%# Eval("msdid") %>' />
                        </td>
                        <td class="par">
                            <asp:ImageButton ID="imgParticipanteDetalle" runat="server" CommandName="Select"
                                ToolTip='<%# String.Format("Eliminar el Participante {0}", Eval("IdPassengerlist")) %>'
                                ImageUrl="~/Styles/images/ic_participante_aspa.png" AlternateText="Eliminar" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
            <asp:ObjectDataSource ID="odsParticipantesExpediente" runat="server" SelectMethod="ObtenerParticipantesExpediente"
                TypeName="EOS.Web.AgenteParticipantes" SelectCountMethod="ObtenerNumeroParticipantesExpediente"
                EnablePaging="True" SortParameterName="sortParameter" OnSelecting="odsParticipantesExpediente_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="filtroIDExpediente" Type="String" DefaultValue="-1" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
        <div class="eosTituloWizard">
            2º Selecciona Asistente
        </div>
        <div class="eosFiltroBasico">
            <asp:Table runat="server" ID="Table4" CssClass="eosTablaFiltros contentFormTable" Width="100%">
                <asp:TableRow Width="100%">
                    <asp:TableCell Width="100%" ColumnSpan="2">
                        <div class="contentFormGeneric">
                            <span class="eosTituloNormal eosCampoLabel">DEFINA LOS FILTROS DEL LISTADO SI ES NECESARIO Y HAGA CLICK EN 'FILTRAR'</span>
                        </div>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Width="100%">
                        <div class="eosTablaFiltros">
                            <div class="contentFormGeneric">
                                <div class="contentForm eosMitadColumna">
                                    <label for="txtNombre2">Nombre</label>
                                    <asp:TextBox ID="txtNombre2" runat="server" MaxLength="80" CssClass="eosInputVacio eosSizeW190"></asp:TextBox>
                                </div>
                                <div class="contentForm eosMitadColumna">
                                    <label for="txtCiudad">Apellidos</label>
                                    <asp:TextBox ID="txtApel1" runat="server" MaxLength="80" CssClass="eosInputVacio eosSizeW190"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Width="100%">
                        <div class="eosTablaFiltros">
                            <div class="contentFormGeneric">
                                <div class="contentForm eosMitadColumna">
                                    <label for="xxxFiltroAMEC">Hospital</label>&nbsp;
                                    <asp:TextBox ID="txtHospital" runat="server" MaxLength="80" CssClass="eosInputVacio eosSizeW190"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow CssClass="butonTable">
                    <asp:TableCell ColumnSpan="4" HorizontalAlign="Right">
                        <asp:Button ID="btnFiltrar2" runat="server" Text="Filtrar"
                            OnClick="btnFiltrar2_Click" />&nbsp;
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div id="Div11" class="contentTableGeneric">
            <asp:ListView ID="lvParticipantes2" runat="server" DataKeyNames="IdPassengerlist"
                EnablePersistedSelection="True" Visible="false" OnSelectedIndexChanged="lvParticipantes2_SelectedIndexChanged">
                <EmptyDataTemplate>
                    <table class="eosTablaResultados">
                        <thead>
                            <tr>
                                <th>Nombre
                                </th>
                                <th>Apellidos
                                </th>
                                <th width="30%">Hospital
                                </th>
                                <th width="20%">MSDID
                                </th>
                                <th width="4%"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="6">
                                    <span class="eosTituloRojo">No se han encontrado participantes que cumplan los criterios
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
                                <th style="text-align: left;">
                                    <asp:LinkButton ID="lbNombre" runat="server" Text="Nombre" CommandName="Sort" CommandArgument="NOMBRE"></asp:LinkButton>
                                </th>
                                <th style="text-align: left;">
                                    <asp:LinkButton ID="lbApel1" runat="server" Text="Apellidos" CommandName="Sort" CommandArgument="APEL1"></asp:LinkButton>
                                </th>
                                <th width="35%" style="text-align: left;">
                                    <asp:LinkButton ID="lbHospital" runat="server" Text="Hospital" CommandName="Sort"
                                        CommandArgument="HOSPITAL"></asp:LinkButton>
                                </th>
                                <th width="10%" style="text-align: left;">
                                    <asp:LinkButton ID="lbMSDID" runat="server" CommandName="Sort" CommandArgument="MSDID"><%= System.Configuration.ConfigurationManager.AppSettings["codigoInvitado"].ToString() %></asp:LinkButton>
                                </th>
                                <th width="4%"></th>
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
                                    ShowPreviousPageButton="True" FirstPageText="" PreviousPageText="" PreviousPageImageUrl="~/Styles/images/ic_pagina_anterior.png"
                                    FirstPageImageUrl="~/Styles/images/ic_pagina_primera.png" RenderDisabledButtonsAsLabels="True" />
                                <asp:NumericPagerField CurrentPageLabelCssClass="pageSel" />
                                <asp:NextPreviousPagerField ButtonType="Image" ShowLastPageButton="True" ShowNextPageButton="True"
                                    ShowPreviousPageButton="False" LastPageText="" NextPageText="" LastPageImageUrl="~/Styles/images/ic_pagina_ultima.png"
                                    NextPageImageUrl="~/Styles/images/ic_pagina_siguiente.png" RenderDisabledButtonsAsLabels="True" />
                            </Fields>
                        </asp:DataPager>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Label ID="nombreLabel" runat="server" Text='<%# Eval("nombre") %>' />
                        </td>
                        <td class="par">
                            <asp:Label ID="apel1Label" runat="server" Text='<%# Eval("apel1")+ " " + Eval("apel2") %>' />
                        </td>
                        <td class="par">
                            <asp:Label ID="hospitalLabel" runat="server" Text='<%# Eval("hospital") %>' />
                        </td>
                        <td>
                            <asp:Label ID="msdidLabel" runat="server" Text='<%# Eval("msdid") %>' />
                        </td>
                        <td class="par">
                            <asp:ImageButton ID="imgParticipanteDetalle" runat="server" CommandName="Select"
                                ToolTip='<%# String.Format("Añadir el Participante {0}", Eval("IdPassengerlist")) %>'
                                ImageUrl="~/Styles/images/ic_participante_flecha.png" AlternateText="Añadir" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
            <asp:ObjectDataSource ID="odsParticipantes" runat="server" SelectMethod="ObtenerParticipantes2"
                TypeName="EOS.Web.AgenteParticipantes" SelectCountMethod="ObtenerNumeroParticipantes2"
                EnablePaging="True" SortParameterName="sortParameter" OnSelecting="odsParticipantes_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="filtroID" Type="String" DefaultValue="-1" />
                    <asp:Parameter Name="filtroNombre2" Type="String" />
                    <asp:Parameter Name="filtroApel1" Type="String" />
                    <asp:Parameter Name="filtroApel2" Type="String" />
                    <asp:Parameter Name="filtroHospital" Type="String" />
                    <asp:Parameter Name="filtroMSDID" Type="String" />
                    <asp:Parameter Name="filtroIDDistrito" Type="String" />
                    <asp:Parameter Name="filtroIDRegion" Type="String" />
                    <asp:Parameter Name="filtroIDEmpresa" Type="String" />
                    <asp:Parameter Name="filtroIDExpediente" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </asp:Panel>
    <asp:Table ID="Table3" runat="server" Width="100%" Height="15px">
        <asp:TableRow>
            <asp:TableCell CssClass="eosTituloWizard hidden" HorizontalAlign="Center">
                <!-- Ismael Ameller  02-03-2011 Cambio de botones-->
                <asp:ImageButton ID="btnCancelar" runat="server" value="Volver"
                    OnClientClick="javascript:document.location.href = 'FiltroInforme.aspx';return false;" />&nbsp;
                <%--<asp:ImageButton ID="btnVolver" runat="server" ImageUrl="~/Styles/images/bt_volver.png" OnClientClick="javascript:document.location.href = '../Expedientes.aspx';return false;" />--%>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Table ID="Table11" runat="server" CssClass="eosFiltroBasico" Width="100%" BorderWidth="0"
        CellSpacing="0" CellPadding="0">
        <asp:TableRow>
            <asp:TableCell Width="700px">
                <div class="eosTablaFiltros">
                    <div class="contentFormGeneric">
                        <div class="eosMitadColumna">
                            <!--Ismael Ameller 28-02-2011 Cambio de label -->
                            <asp:Label ID="lbNPedido" runat="server" Text="Nº Pedido" CssClass="eosCampoLabel"></asp:Label>
                            <!-- FIN Ismael Ameller 28-02-2011 Cambio de label -->
                            <asp:TextBox ID="txtNPedido" runat="server"></asp:TextBox>
                        </div>
                        <div class="eosMitadColumna">
                            <asp:Label ID="lblNExpediente" runat="server" Text="Nº Expediente" CssClass="eosCampoLabel"></asp:Label>
                            <asp:TextBox ID="txtNumExpediente" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="contentFormGeneric">
                        <div class="eosTresColumna">
                            <asp:Label CssClass="eosCampoLabel" ID="lblNAmec" runat="server"><%= System.Configuration.ConfigurationManager.AppSettings["codigoPresupuesto"].ToString() %></asp:Label>
                            <asp:TextBox ID="txtNumAmec" runat="server"></asp:TextBox>
                        </div>
                        <div class="eosTresColumna">
                            <asp:Label ID="Label3" runat="server" Text="Estado Petición" CssClass="eosCampoLabel"></asp:Label>
                            <asp:DropDownList ID="ddlEstadoReserva" runat="server" CssClass="eosInputVacio eosSizeW190">
                            </asp:DropDownList>
                        </div>
                        <div class="eosTresColumna">
                            <asp:Label ID="Label2" runat="server" Text="Estado Expediente" CssClass="eosCampoLabel"></asp:Label>
                            <asp:DropDownList ID="ddlEstadoExpediente" runat="server" CssClass="eosInputVacio eosSizeW190">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="contentFormGeneric">
                        <div class="eosMitadColumna">
                            <asp:Label ID="lbValFarma" runat="server" Text="Valorado Farmaindustria" CssClass="eosCampoLabel"></asp:Label>
                            <asp:DropDownList ID="ddlValFarma" runat="server" CssClass="eosInputVacio eosSizeW190">
                            </asp:DropDownList>
                        </div>
                        <div class="eosMitadColumna ">
                            <asp:Label ID="lbPendientePedido" runat="server" Text="Pendiente Nº Pedido" CssClass="eosCampoLabel"></asp:Label>
                            <asp:Panel ID="PanelPendientePedido" runat="server" CssClass="PanelPendientes radioCheck">
                                <asp:RadioButton ID="PendienteSi" runat="server" GroupName="PendientePedido" Text="Si" />
                                <asp:RadioButton ID="PendienteNo" runat="server" GroupName="PendientePedido" Text="No" />
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="contentFormGeneric">
                        <asp:Label ID="lbFecha" runat="server" Text="Expediente:" CssClass="eosCampoLabel"></asp:Label>
                    </div>
                    <div class="contentFormGeneric" style="margin-bottom: 15px">
                        <div class="eosMitadColumna centerCalendar">
                            <asp:Label ID="lbFechaDesde" runat="server" Text="Desde:" CssClass="eosCampoLabel"></asp:Label>
                            <eos:InputDatePickerControl ID="txtFechaDesde" class="iconCalendar" runat="server" CssClass="eosDisabledInputVacio style1" />
                        </div>
                        <div class="eosMitadColumna centerCalendar">
                            <asp:Label ID="lbFechaHasta" runat="server" Text="Hasta:" CssClass="eosCampoLabel"></asp:Label>
                            <eos:InputDatePickerControl ID="txtFechaHasta" class="iconCalendar" runat="server" CssClass="eosDisabledInputVacio style1" />
                            <!-- 
                                            <asp:CheckBox ID="chkDatosHojas" runat="server" />
                                            Desea que cada dato se exporte en hojas distintas (hojas/región/area/distrito)
                                            -->
                        </div>
                    </div>
                </div>
            </asp:TableCell>
            <asp:TableCell Width="25px">&nbsp;</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
                <asp:Table ID="Table2" runat="server" CssClass="eosTablaFiltros">
                    <asp:TableRow>
                        <asp:TableCell>
                            <!--Ismael Ameller 02-03-2011 Marcamos por defecto el radio button de Expediente-->
                            <div class="contentFormGeneric absolute-center">
                                <div class="radioCheck">
                                    <asp:RadioButton ID="rbExpediente" runat="server" Text="Expediente" Checked="true"
                                        GroupName="Informe" />
                                </div>
                                <div class="radioCheck">
                                    <asp:RadioButton ID="rbReserva" runat="server" Text="Petición" Visible="false" GroupName="Informe" />
                                </div>
                                <div class="radioCheck">
                                    <asp:RadioButton ID="rbGrupos" runat="server" Text="Petición" Visible="false" GroupName="Informe" />
                                </div>
                                <div class="radioCheck">
                                    <asp:RadioButton ID="rbHonorarios" runat="server" Text="Honorarios" Visible="false"
                                        GroupName="Informe" />
                                </div>
                                <div>
                                    <asp:ImageButton ImageUrl="~/Styles/images/iconExcel.png" ID="btnExportar" runat="server" CssClass="texto_boton"
                                        ToolTip="Exportar a Informe" OnClick="btnInforme_Click" />
                                </div>
                                <!--Ismael Ameller 02-03-2011 Cambio de etiqueta-->
                                <%--<asp:RadioButton ID="rbReserva" runat="server" Text="Reserva" GroupName="Informe" />&nbsp;&nbsp;&nbsp;--%>

                                <%--                            <div runat="server" id="divParticipante" style="display: none">
                                    <br/><br/>    
                                <asp:ImageButton ID="btnNuevoParticipante" runat="server" ImageUrl="~/Styles/images/bt_crear_n_participante.png"
                                 OnClick="btnNuevoParticipante_Click" />
                                </div>--%>
                                <!--
                                    <asp:Button ID="btnInform1" runat="server" Text="Informe" OnClick="btnInforme_Click"
                                    Width="130px" />
                                    -->
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="3">&nbsp;</asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
<asp:Content ID="cphScripts" ContentPlaceHolderID="ContentPlaceHolderScripts" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("ul#eosHPBotones li").removeClass("active");
            $("ul#eosHPBotones li#liInformesYListados").addClass("active");
        });
    </script>
</asp:Content>
