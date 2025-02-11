<%@ Page Title="Documentos del expediente" Language="C#" MasterPageFile="~/Styles/EOS.Master"
    AutoEventWireup="true" CodeBehind="Documentos.aspx.cs" Inherits="EOS.Documentos"
    EnableEventValidation="false" Culture="es-ES" UICulture="es" %>

<asp:Content ID="ContentBotonera" ContentPlaceHolderID="eosHeaderBotonera" runat="server">
    <div id="eosHPBotones">
        <div class="eosBotonera eosBotonInformes">
            <a title="Acceder a Informes y Listados" onmouseout="javascript:window.status='';return true;" onmouseover="javascript:window.status='Acceder a Informes y Listados';return true;" href="javascript:document.location.href = '/Informes/FiltroInforme.aspx';">
                Acceso a Informes y Listados</a></div>
        <div class="eosBotonera eosBotonDatosPer">
            <a title="Acceder a Datos Personales" onmouseout="javascript:window.status='';return true;"
                onmouseover="javascript:window.status='Cambiar datos personales';return true;"
                href="javascript:document.location.href = '/CambiarDatosPersonales.aspx';">Acceso
                a Datos Personales</a></div>
        <div class="eosBotonera eosBotonNuevoExp">
            <a title="Crear Nuevo Expediente" onmouseout="javascript:window.status='';return true;"
                onmouseover="javascript:window.status='Crear un Nuevo Expediente';return true;"
                href="javascript:document.location.href = '/NuevoExpedientePasoA.aspx?idexp=0';">
                Nuevo Expediente</a></div>
        <div class="eosBotonera eosBotonAMEC">
            <a title="Acceder a Listados AMEC" onmouseout="javascript:window.status='';return true;"
                onmouseover="javascript:window.status='Litados de AMECs';return true;" href="javascript:document.location.href = '/ListadoAMECs.aspx';">
                Listados de AMECs</a></div>
        
        <div class="eosBotonera eosBotonSalir">
            <a title="Volver al Detalle de Expediente" id="btnSalir" runat="server" href="/DetalleExpediente.aspx">
                Volver al detalle de Expediente</a></div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="eosContentStatus" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="eosHeaderContent" runat="server">
    <script language="javascript" type="text/javascript">
        function GoBack() {
            document.location = 'DetalleExpediente.aspx?idexp=<%= Request.QueryString["idexp"] %>';
        }
    </script>
    <div id="eosFilterHeaderDetalle">
    <div id="infoPanel">
        <asp:Table ID="Table1" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell>
                        <span class="lblTitle" style="font-weight:bolder">EVENTO:&nbsp;</span>
                        <asp:Label ID="congresoLabelValue" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                        <span class="lblTitle" style="font-weight:bolder">|&nbsp;&nbsp;LUGAR:&nbsp;</span>
                        <asp:Label ID="LblPoblacion"  runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell RowSpan="2" HorizontalAlign="Right">
                        <div id="eosFilterHeaderEstadoDetalle">
                            <span style="float: left;">ESTADO:</span>
                            <asp:Label ID="lblEstado" runat="server" Text="Label"></asp:Label>
                        </div>
                    </asp:TableCell>
                </asp:TableRow>
             <asp:TableRow>
                    <asp:TableCell>
                        <span class="lblTitle" style="font-weight:bolder">INICIO:&nbsp;</span>
                        <asp:Label ID="lblFechaDesde"  runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                        <span class="lblTitle">|&nbsp;&nbsp;FIN:&nbsp;</span>
                        <asp:Label ID="LblFechaHasta"  runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                    </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                    <asp:TableCell>
                        <span class="lblTitle">FECHA EXPEDIENTE:</span>
                        <asp:Label ID="fechaExpedienteLabelValue" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                        &nbsp;|&nbsp; <span class="lblTitle">PEDIDO:</span>
                        <asp:Label ID="pedidoLabelValue" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                        &nbsp;|&nbsp;
                        <!-- Xavier Morell (GP) 16-01-11-->
                        <span class="lblTitle">Nº EXPEDIENTE:</span><asp:Label ID="pedidoLabelExpediente"
                            runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                    </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                    <asp:TableCell>
                        <asp:ImageButton ID="imgExpedienteDetalle" runat="server" ImageUrl="~/Styles/images/ic_lupa.png"
                            Visible="false" /><span class="lblTitle">AMEC:</span><asp:Label ID="amecLabelValue"
                                runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                    </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
           <%-- <div class="eosBotonera eosBotonVolver" style="float: right;">
                <a title="Volver"  href="javascript:document.location.href='/Expedientes.aspx';return false;">Volver</a>
            </div>  --%>  
    </div>
    </div>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="eosContentResults" runat="server">
    <div id="eosContentResults" style="padding-top: 10px;">
     
        <asp:ListView ID="lvDocumentos" runat="server" OnSorting="lvDocumentos_Sorting" >
            <EmptyDataTemplate>
                <table class="eosTablaResultados">
                    <thead>
                        <tr>
                            <th>
                            </th>
                            <th>
                                Nº Doc
                            </th>
                            <th>
                                Tipo
                            </th>
                            <th width="50%">
                                Documento
                            </th>
                            <th>
                                Fecha
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="10">
                                <span class="eosTituloRojo">No se han encontrado ningún documento para el expediente</span>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table class="eosTablaResultados">
                    <thead>
                        <tr>
                            <th width="20px">
                            </th>
                            <th width="20px">
                            </th>
                            <th>
                                <asp:LinkButton ID="lbTipo" runat="server" Text="Tipo" CommandName="Sort" CommandArgument="Extension"  ></asp:LinkButton>
                            </th>
                            <th width="50%">
                                <asp:LinkButton ID="lbDocumento" runat="server" Text="Documento" CommandName="Sort"
                                    CommandArgument="Name"></asp:LinkButton>
                            </th>
                            <th>
                                <asp:LinkButton ID="lbFecha" runat="server" Text="Fecha" CommandName="Sort" CommandArgument="LastWriteTime"></asp:LinkButton>
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
                    <td class="par">
                        <asp:ImageButton ID="imgDownload" runat="server" ToolTip='<%# String.Format("Descargar el documento {0}", Eval("documento")) %>'
                            ImageUrl="~/Styles/images/download.png" CommandName='<%# Eval("documento") %>'
                            OnCommand="imgDownload_Command" />
                    </td>
                    <td class="par">
                        <asp:Image ID="imgType" runat="server" ImageUrl='<%# String.Format("~/Styles/images/{0}", Eval("iddoc")) %>' />
                    </td>
                    <td>
                        <asp:Label ID="tipoLabel" runat="server" Text='<%# Eval("tipo") %>' />
                    </td>
                    <td>
                        <asp:Label ID="expedienteLabel" runat="server" Text='<%# Eval("documento") %>' />
                    </td>
                    <td class="par">
                        <asp:Label ID="fechacreacionLabel" runat="server" Text='<%# String.Format("{0:dd/MM/yy}",Eval("fecha")) %>' />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>
</asp:Content>
