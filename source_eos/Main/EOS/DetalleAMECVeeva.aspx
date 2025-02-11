<%@ Page Title="Nuevo Amec" Language="C#" MasterPageFile="~/Styles/EOS.Master" AutoEventWireup="true"
    CodeBehind="DetalleAMECVeeva.aspx.cs" Inherits="EOS.DetalleAMECVeeva" EnableEventValidation="false"
    Culture="es-ES" UICulture="es" %>

<asp:Content ID="Content1" ContentPlaceHolderID="eosContentFilter" runat="server">

    <asp:Panel ID="Panel" runat="server">
        <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" EnableScriptGlobalization="true"
            EnableScriptLocalization="true" CombineScripts="True" ScriptMode="Release" />
        <br />
        <div id="Div1" class="eosFiltroColor">
            <div class="eosFiltroColorTabla">
                <div class="header-info-container">
                    <div class="align-start-txt">
                        <label id="Label1" class="eosCampoLabelSin bigger">
                            AMEC:
                        </label>
                        <label id="lbidAMEC" class="eosCampoLabelSin bigger" runat="server">
                        </label>
                        <label id="lbidAMECClonado" class="eosCampoLabelSin bigger red" runat="server">
                        </label>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <label id="Label1" class="eosCampoLabelSin bigger">
                            EMPRESA:
                        </label>
                        <label id="Label4" class="eosCampoLabelSin bigger" runat="server">
                            MSD
                        </label>
                    </div>
                    <div class="align-end-txt">
                        <div class="estate-container">
                            <asp:TextBox ID="txtIdCreadoPor" ReadOnly="true" Visible="false" MaxLength="10" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txtIdEstado" ReadOnly="true" Visible="false" MaxLength="10" runat="server"></asp:TextBox>
                            <asp:Image runat="server" ID="imgIconoEstado" />
                            <%--<asp:ImageButton ID="imgEstadoAMEC" runat="server" CausesValidation="false" />--%>
                            <asp:Label ID="lbEstadoAMEC" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="plAMECDatos" runat="server">
        <input type="hidden" id="TornarEnviarFarma" name="TornarEnviarFarma" value="true" />
        <input type="hidden" id="TornarEnviarCasosClinicos" name="TornarEnviarCasosClinicos" value="true" />
        <div class="eosTituloAMEC">
            SOLICITUD DE APROBACIÓN DE ACTIVIDADES MÉDICO CIENTÍFICAS
        </div>
        <br />
        <div class="eosTituloWizard">
            DATOS GENERALES
        </div>

        <div id="eosContentFilter">
            <div id="eosContentAMECErrores"">
                <asp:ValidationSummary ID="ContenAMECValidationSummary" runat="server" CssClass="failureNotification"
                    ValidationGroup="AMECValidationGroup" HeaderText="ATENCIÓN. Los siguientes campos son obligatorios:" />
                <span class="failureNotification">
                    <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                </span>
            </div>
            <div id="eosTablaAMEC1" class="eosTablaFiltros">
                    <div class="contentFormGeneric">
                        <div class="contentForm  eosMitadColumna">
                            <label class="eosCampoLabelSin" for="txtNombre">
                                Solicitante<span class="eosCampoObligatorio">*</span></label>
                            <asp:DropDownList ID="ddlSolicitante" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW220"
                                DataTextField="NombreCompleto" DataValueField="IdPeticionario" Visible="true" Enabled="false">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvSolicitante" runat="server" ErrorMessage="Solicitante."
                                ControlToValidate="ddlSolicitante" ValidationGroup="AMECValidationGroup" SetFocusOnError="True"
                                ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                        </div>
                        <div class="contentForm  eosMitadColumna">
                            <label class="eosCampoLabelSin" for="txtCiudad">
                                N. Wein<span class="eosCampoObligatorio">*</span></label>
                            <asp:TextBox ID="txtWein" ReadOnly="true" MaxLength="45" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW140"></asp:TextBox>
                        </div>
                    </div>
                <div class="contentFormGeneric">
                    <div class="contentForm  eosMitadColumna">
                        <label class="eosCampoLabelSin" for="txtCiudad">
                            N. AMEC<span class="eosCampoObligatorio">*</span></label>
                        <asp:TextBox ID="txtNAmec" ReadOnly="true" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW190"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvNAmec" runat="server" ErrorMessage="N. AMEC." ControlToValidate="txtNAmec"
                            ValidationGroup="AMECValidationGroup" SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                    </div>
                    <div class="contentForm  eosMitadColumna">
                        <label id="lblDistrito" class="eosCampoLabelSin">Distrito: &nbsp;</label>
                        <asp:TextBox ID="txtDistrito" ReadOnly="true" runat="server" MaxLength="128" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW220"></asp:TextBox>
                    </div>
                </div>
                <div class="contentFormGeneric">
                    <div class="contentForm  eosMitadColumna">
                        <label id="lblDepartamento" class="eosCampoLabelSin">Depart: &nbsp;</label>
                         <asp:TextBox ID="txtDepartamento" ReadOnly="true" runat="server" MaxLength="128" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW140"></asp:TextBox>
                    </div>
                    <div class="contentForm  eosMitadColumna">
                        <label id="lblFuerzaVentas" class="eosCampoLabelSin">F. Ventas: &nbsp;</label>
                         <asp:TextBox ID="txtFuerzaVentas" ReadOnly="true" runat="server" MaxLength="128" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW190"></asp:TextBox>
                    </div>
                </div>
                <div class="contentFormGeneric">
                    <div class="contentForm  eosMitadColumna">
                        <label class="eosCampoLabelSin" for="txtNombre">
                            Cargo<span class="eosCampoObligatorio">*</span></label>
                        <asp:TextBox ID="txtCargo" ReadOnly="true" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW220"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCargo" runat="server" ErrorMessage="Cargo." ControlToValidate="txtCargo"
                            ValidationGroup="AMECValidationGroup" SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                    </div>
                    <div class="contentForm  eosMitadColumna">
                        <label class="eosCampoLabelSin" for="txtCiudad">
                            Fecha<span class="eosCampoObligatorio">*</span></label>
                        <asp:TextBox ID="txtFecha" runat="server" MaxLength="80" ReadOnly="true" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW140"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Fecha."
                            ControlToValidate="txtFecha" ValidationGroup="AMECValidationGroup2" SetFocusOnError="True"
                            ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="La Fecha debe tener un formato válido."
                            Type="Date" ControlToValidate="txtFecha" Operator="DataTypeCheck" ValidationGroup="AMECValidationGroup2">&nbsp;</asp:CompareValidator>
                        <asp:RangeValidator ID="RangeValidator4" runat="server" ErrorMessage="La Fecha no puede ser inferior a hoy."
                            MaximumValue="31/12/99" ControlToValidate="txtFecha" ValidationGroup="AMECValidationGroup2"
                            Display="Dynamic" Enabled="false">&nbsp;</asp:RangeValidator>
                        <asp:ImageButton ID="btnCalendar" runat="server" ImageUrl="~/Styles/images/ic_calendario.png" class="iconCalendar" />
                        <act:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFecha" runat="server"
                            TodaysDateFormat="d MMMM yyyy" FirstDayOfWeek="Monday" PopupButtonID="btnCalendar"
                            Format="dd/MM/yy" />
                        &nbsp;&nbsp;&nbsp;
                    </div>
                </div>
                <div class="contentFormGeneric">
                    <div class="contentForm  eosMitadColumna">
                        <label class="eosCampoLabelSin" id="lblCreadopor">
                            Creado por<span class="eosCampoObligatorio">*</span>&nbsp;</label>
                        <asp:TextBox ID="txtCreadoPor" ReadOnly="true" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW190"></asp:TextBox>
                    </div>
                    <div class="contentForm  eosMitadColumna">
                        <asp:Label ID="Label2" runat="server" CssClass="eosCampoLabel">Tipo de Evento<span class="eosCampoObligatorio">*</span></asp:Label>
                        <asp:DropDownList ID="ddlTipoEvento" runat="server" AppendDataBoundItems="True" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW220"
                            DataTextField="tipogrupoactividad" DataValueField="idtiporegistroactividad" Visible="true" Enabled="false">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="contentFormGeneric">
                    <div class="contentForm  eosMitadColumna">
                        <asp:Label ID="Label7" runat="server" CssClass="eosCampoLabel">Tipo de Actividad<span class="eosCampoObligatorio">*</span></asp:Label>
                        <asp:DropDownList ID="ddlActividad" runat="server" AppendDataBoundItems="True" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW450"
                            DataTextField="tipoactividad" DataValueField="idtipoactividad" Visible="true" Enabled="false">
                            <asp:ListItem Value="-1">Selecciona Tipo de Actividad</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="ddlActividad"
                            ErrorMessage="Tipo de actividad." MaximumValue="9999999" MinimumValue="1" SetFocusOnError="true"
                            ToolTip="Campo Obligatorio" ValidationGroup="AMECValidationGroup">&nbsp;</asp:RangeValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ddlActividad"
                            ErrorMessage="Tipo de actividad." SetFocusOnError="True" ToolTip="Campo Obligatorio"
                            ValidationGroup="AMECValidationGroup">&nbsp;</asp:RequiredFieldValidator>
                    </div>
                    <div class="contentForm  eosMitadColumna">
                        <asp:Label ID="lblDescripcionAMEC" runat="server" class="eosCampoLabel">Nombre Programa/Actividad<span class="eosCampoObligatorio">*</span>&nbsp;</asp:Label>
                        <asp:TextBox ID="txtDescripcionAMEC" runat="server" CssClass="eosSizeW730" MaxLength="200" Enabled="false"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDescripcionAMEC" runat="server" ErrorMessage="Nombre Programa/Actividad."
                            ControlToValidate="txtDescripcionAMEC" ValidationGroup="AMECValidationGroup"
                            SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="contentFormGeneric">
                    <div class="contentForm  eosMitadColumna">
                        <label id="lblImporteTotalGasto" class="eosCampoLabelSin" title="Introduzca una cifra entera">
                           Importe total del gasto (con IVA):<span class="eosCampoObligatorio">*</span></label>
                        <asp:TextBox ID="txtImporteTotalGasto" runat="server" CssClass="eosDisabledInputVacio eosInputVacio " Enabled="false"
                            MaxLength="80" Visible="true" Width="74px"></asp:TextBox>
                    </div>
                    <div class="contentForm  eosMitadColumna">
                        <label class="eosCampoLabelSin" for="txtAgencias">Agencia<span class="eosCampoObligatorio">*</span> &nbsp;</label>
                        <asp:DropDownList ID="ddlAgencias" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW160"
                            AutoPostBack="false" Enabled="true">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="plPaso1" runat="server">
        <br />
        <div class="eosTituloWizard">
            EVENTOS O ACTIVIDADES ASOCIADOS AL AMEC
        </div>
        <div id="AsociarEvento">
            <div class="eosSubTitulo" style="padding-left: 10px">
                &nbsp;  
            </div>
            <div id="Div3" runat="server" class="contentTableGeneric">
                <asp:ListView ID="lvActividadAsigAmec" EnableViewState="false" runat="server" DataSourceID="odsActividadesAsigAmec"
                    OnSelectedIndexChanged="lvActividadAsigAmec_SelectedIndexChanged" DataKeyNames="IDCONGRESO"
                    EnablePersistedSelection="True">
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
                                    <th>
                                        <asp:LinkButton ID="lbId" runat="server" Text="ID" CommandName="Sort" CommandArgument="IDCONGRESO"></asp:LinkButton>
                                    </th>
                                    <th width="50%">
                                        <asp:LinkButton ID="lbNombre" runat="server" Text="Nombre" CommandName="Sort" CommandArgument="CONGRESO"></asp:LinkButton>
                                    </th>
                                    <th>
                                        <asp:LinkButton ID="lbPoblacion" runat="server" Text="Lugar" CommandName="Sort" CommandArgument="POBLACION"></asp:LinkButton>
                                    </th>
                                    <th>
                                        <asp:LinkButton ID="lbFechaDesde" runat="server" Text="Desde" CommandName="Sort"
                                            CommandArgument="DESDE"></asp:LinkButton>
                                    </th>
                                    <th>
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
                            <td>
                                <asp:ImageButton ID="imgActividadDetalle" runat="server" CommandName="Select" ToolTip='<%# String.Format("Eliminar el Evento {0} Asignado al Amec {1}", Eval("idcongreso"), Eval("idameccongreso")) %>'
                                    ImageUrl="~/Styles/images/ic_participante_aspa.png" CommandArgument='<%# Eval("idameccongreso") %>'
                                    OnCommand="imgEliminarAsignacionEvento_Command" />
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
                <asp:ObjectDataSource ID="odsActividadesAsigAmec" runat="server" SelectMethod="ObtenerActividadesAsigAmec"
                    TypeName="EOS.Web.AgenteAmecInfo" SelectCountMethod="ObtenerNumeroActividadesAsigAmec"
                    EnablePaging="True" SortParameterName="sortParameter" OnSelecting="odsActividadesAsigAmec_Selecting">
                    <SelectParameters>
                        <asp:Parameter Name="filtroIdAMEC" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </asp:Panel>
    <div>
        <div class="eosBotonera eosBotonFiltrado">
            <asp:Button ID="btnGuardar" Visible="false" runat="server" Text="Guardar"
                OnClick="btnGuardar_Click" ValidationGroup="AMECValidationGroupBotonGuardar" />
            <!--<asp:Button ID="btnVolver" Visible="false" runat="server" OnClick="btnVolver_Click" Text="Volver"/>-->
        </div>
        <div class="contentFormGeneric separator">
            <div class="excell-icon-container">
                <asp:ImageButton ID="btnInformeAdicional" runat="server" ImageUrl="~/Styles/images/iconExcel.png"
                    OnClick="btnInformeAdicional_Click" class="icon-not-boder" />
                <asp:Label ID="Label5" runat="server">Informe Documentación Adicional</asp:Label>
            </div>
            <div class="excell-icon-container">
                <asp:ImageButton ID="btnVerExcel" runat="server" ImageUrl="~/Styles/images/iconExcel.png"
                            OnClick="btnVerExcel_Click" class="icon-not-boder" />
                <asp:Label ID="Label13" runat="server">Descargar AMEC</asp:Label>
            </div>      
        </div>
    </div>
    <div class="center-grey">
        <asp:Label ID="lblContratoFreq" runat="server"><a href="http://org.merck.com/MRK/GeoReg/Eur/About%20MSD%20in%20Spain/Leg/Pages/Impresos%20de%20actividades%20más%20frecuentes.aspx" target="_blank" >Contratos de uso más frecuente</a></asp:Label>
        <asp:Label ID="lblbarra1" runat="server" Style="font-weight: bold;"> | </asp:Label>
        <asp:Label ID="Label10" runat="server"><a href="http://org.merck.com/MRK/GeoReg/Eur/About%20MSD%20in%20Spain/Leg/amec-principios-globales-de-merck/Pages/Pol%C3%ADticas%20y%20Procedimientos.aspx" target="_blank" >Políticas y Procedimientos AMEC</a></asp:Label>
        <asp:Label ID="lblbarra2" runat="server" Style="font-weight: bold;"> | </asp:Label>
        <asp:Label ID="Label15" runat="server"><a href="./Documentos/Pre_AprobadoNegocioSH.pdf" target="_blank">Preaprobado Unidad Specialty & Hospitales</a></asp:Label>
        <asp:Label ID="lblbarra4" runat="server" Style="font-weight: bold;"> | </asp:Label>
        <br />
        <asp:Label ID="Label11" runat="server"><a href="http://org.merck.com/MRK/GeoReg/Eur/About%20MSD%20in%20Spain/Com/Codigo%e2%80%93%20USD-FARMAINDUSTRIA/Pages/default.aspx" target="_blank">Formulario de Comunicación a Farmaindustria</a>  </asp:Label>
        <asp:Label ID="lblbarra5" runat="server" Style="font-weight: bold;"> | </asp:Label>
        <asp:Label ID="Label12" runat="server"><a href="http://org.merck.com/MRK/GeoReg/Eur/About%20MSD%20in%20Spain/For/Herramientas%20-Gestion/Pages/Aplicacion-EOS.aspx" target="_blank">Manual de Formación</a>  </asp:Label>
    </div>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="headContent">
    <style type="text/css">
        .style1 {
            height: 27px;
        }
    </style>
</asp:Content>
<asp:Content ID="cphScripts" ContentPlaceHolderID="ContentPlaceHolderScripts" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("ul#eosHPBotones li").removeClass("active");
            $("ul#eosHPBotones li#liBuscadorActividades").addClass("active");
        });
    </script>
</asp:Content>