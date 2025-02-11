<%@ Page Title="Documentos del expediente" Language="C#" MasterPageFile="~/Styles/EOS.Master"
    AutoEventWireup="true" CodeBehind="DocumentosExpediente.aspx.cs" Inherits="EOS.DocumentosExpediente"
    EnableEventValidation="false" Culture="es-ES" UICulture="es" %>

<%@ Import Namespace="EOS" %>

<asp:Content ID="Scripts" ContentPlaceHolderID="eosContentStatus" runat="server">
    <script type="text/javascript" src="/Scripts/jQuery/jquery-1.12.4.min.js"></script>
    <script type="text/javascript">
        var jQuery_1_12 = $.noConflict(true);
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            loadAccordion();
            $('#eosContentDocument').addClass('div-table');
        });

        var fileToDelete = "";

        function descargarDocumento(url) {
            alert(url);
        }

        function ShowPopup(file) {
            fileToDelete = file;
            $(function () {
                $("#modal").html();
                $("#modal").dialog({
                    title: "Confirmar",
                    buttons: {
                        Cerrar: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true,
                    width: 300
                });
            });
        };

        function CloseDialog() {
            $("#modal").dialog('close');
        }

        function UnvalidFile() {
            var info = {
                filePath: fileToDelete
            }
            jQuery_1_12.ajax({
                method: 'GET',
                url: '/api//UploadFile/UnvalidateFile.aspx',
                contentType: 'application/json',
                data: info,
                success: function (data, status, request) {
                    alert('Documento invalidado correctamente');
                    location.reload();
                },
                error: function (data) {
                    alert(data);
                }
            });
        };
        
        function loadAccordion() {
            $('ul.QC_Menu li.QC_Item_Open').hide();
            $(document).ready(function () {

                $('ul.QC_Menu li.QC_Item').click(function () {

                    $(this).next('li.QC_Item_Open').slideToggle("slow");//.siblings("li.QC_Item_Open:visible").slideUp("slow");
                    $(this).toggleClass("active");
                    $(this).siblings("li.QC_Item").removeClass("active");
                });

            });
        }
    </script>
</asp:Content>
<%--<asp:Content ID="ContentBotonera" ContentPlaceHolderID="eosHeaderBotonera" runat="server">
    <div id="eosHPBotones">
        <div class="eosBotonera eosBotonInformes">
            <a title="Acceder a Informes y Listados" onmouseout="javascript:window.status='';return true;" onmouseover="javascript:window.status='Acceder a Informes y Listados';return true;" href="javascript:document.location.href = '/Informes/FiltroInforme.aspx';">Acceso a Informes y Listados</a>
        </div>
        <div class="eosBotonera eosBotonDatosPer">
            <a title="Acceder a Datos Personales" onmouseout="javascript:window.status='';return true;"
                onmouseover="javascript:window.status='Cambiar datos personales';return true;"
                href="javascript:document.location.href = '/CambiarDatosPersonales.aspx';">Acceso
                a Datos Personales</a>
        </div>
        <div class="eosBotonera eosBotonNuevoExp">
            <a title="Crear Nuevo Expediente" onmouseout="javascript:window.status='';return true;"
                onmouseover="javascript:window.status='Crear un Nuevo Expediente';return true;"
                href="javascript:document.location.href = '/NuevoExpedientePasoA.aspx?idexp=0';">Nuevo Expediente</a>
        </div>
        <div class="eosBotonera eosBotonAMEC">
            <a title="Acceder a Listados AMEC" onmouseout="javascript:window.status='';return true;"
                onmouseover="javascript:window.status='Litados de AMECs';return true;" href="javascript:document.location.href = '/ListadoAMECs.aspx';">Listados de AMECs</a>
        </div>

        <div class="eosBotonera eosBotonSalir">
            <a title="Volver al Detalle de Expediente" id="btnSalir" runat="server" href="/DetalleExpediente.aspx">Volver al detalle de Expediente</a>
        </div>
    </div>
</asp:Content>--%>

<asp:Content ID="Content6" ContentPlaceHolderID="eosHeaderContent" runat="server">
   
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="eosContentResults" runat="server">
     <script language="javascript" type="text/javascript">
         function GoBack() {
             document.location = 'DetalleExpediente.aspx?idexp=<%= Request.QueryString["idexp"] %>';
         }
     </script>
    <h1>Documentación de Auditoría</h1>
    <div id="eosFilterHeaderDetalle" class="contentDetalleGeneric">
        <h2>Información</h2>
        <div id="infoPanel">
            <div id="eosFilterHeaderEstadoDetalle">
                <span class="contentIconEstado"></span>
                <asp:Label ID="lblEstado" runat="server" Text="Label" class="eosImagenEstado eosImagenLeyendaAB"></asp:Label>
            </div>
            <div class="contentInfoPanel">
                <div class="contentInfoPanelElement">
                    <span class="lblTitle" >EVENTO:&nbsp;</span>
                    <asp:Label ID="congresoLabelValue" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                    <span class="lblTitle" >|&nbsp;&nbsp;LUGAR:&nbsp;</span>
                    <asp:Label ID="LblPoblacion" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                </div>
                <div class="contentInfoPanelElement">
                    <span class="lblTitle" >INICIO:&nbsp;</span>
                    <asp:Label ID="lblFechaDesde" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                    <span class="lblTitle">|&nbsp;&nbsp;FIN:&nbsp;</span>
                    <asp:Label ID="LblFechaHasta" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label> &nbsp;|&nbsp; 
                    <span class="lblTitle">AGENCIA:&nbsp;</span><asp:Label ID="lblAgencia"
                        runat="server" Text="" CssClass="eosCampoLabelDato"></asp:Label>
                </div>
                <div class="contentInfoPanelElement">
                    <span class="lblTitle">FECHA EXPEDIENTE:</span>
                    <asp:Label ID="fechaExpedienteLabelValue" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                    &nbsp;|&nbsp; <span class="lblTitle">PEDIDO:</span>
                    <asp:Label ID="pedidoLabelValue" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                    &nbsp;|&nbsp;
                    <!-- Xavier Morell (GP) 16-01-11-->
                    <span class="lblTitle">Nº EXPEDIENTE:</span><asp:Label ID="pedidoLabelExpediente"
                        runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                </div>
                <div class="contentInfoPanelElement">
                    <asp:ImageButton ID="imgExpedienteDetalle" runat="server" ImageUrl="~/Styles/images/ic_lupa.png"
                        Visible="false" /><span class="lblTitle">AMEC:</span><asp:Label ID="amecLabelValue"
                            runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                </div>
            </div>
            <%-- <div class="eosBotonera eosBotonVolver" style="float: right;">
                <a title="Volver"  href="javascript:document.location.href='/Expedientes.aspx';return false;">Volver</a>
            </div>  --%>
        </div>
    </div>
    <div id="modal" style="display: none">
        <div class="noValid">
            <span>¿Desea marcar este elemento como documento no válido?</span>
            <a class="btAccept" onclick="UnvalidFile()">Ok</a>
            <a class="btCancel" onclick="CloseDialog()">Cancelar</a>
        </div>
    </div>
    <div class="eosTituloWizard">
            DOCUMENTACIÓN ASOCIADA AL AMEC</div>
    <div id="eosContentAmecDocument" style="padding-top: 10px;">
        <div class="eosTablaResultados">
            <% if (DocumentosAmecs.Count == 0)
                { %>
            <div style="background-color: white;">
                <span style="margin-left: 20px">No hay documentos del amec.</span>
            </div>
            <% } %>
            <ul class="QC_Menu">
                <li class="QC_Item" style="display: list-item;">
                    <ul class="">
                        <% foreach (var doc in DocumentosAmecs)
                            { %>
                        <%if (doc.SubTipo == null)
                            { %>
                        <li class="QC_Item pos01"><a style="cursor: pointer;"><div class="icoField"></div><span><%=doc.Ruta.Split('\\').Last()%></span></a></li>
                        <li class="QC_Item_Open programa" style="display: none;cursor: auto;">
                            <% foreach (var file in doc.Ficheros)
                                { %>
                            <% foreach (var fileVer in file.Versiones)
                                { %>
                                <div>
                                    <div class="docuField"><a  style="cursor: pointer;" href="DownloadGestor.aspx?file=<%=HttpUtility.UrlEncode(fileVer.NombreFicheroEncriptado) %>"><span><%=fileVer.NombreFichero.Split('/').Last()%></span></a></div>
                                    <div class="dateField"><span><%=fileVer.FechaFichero.ToString() %></span></div>
                                </div>
                            <% } %>
                            <% }%>
                        </li>
                        <%}
                        else
                        { %>
                        <li class="QC_Item pos01"><a style="cursor: pointer;"><div class="icoField"></div><span><%=doc.Ruta.Split('\\').Last()%></span></a></li>
                        <li class="QC_Item_Open" style="display: none;">
                            <ul class="">
                                <% foreach (var subDoc in doc.SubTipo)
                                    { %>
                                <li class="QC_Item pos02"><a style="cursor: pointer;"><span><%=subDoc.Ruta.Split('\\').Last()%> </span></a></li>
                                <li class="QC_Item_Open documents" style="display: none; cursor: auto;">
                                    <% foreach (var file in subDoc.Ficheros)
                                        {
                                            foreach (var fileVer in file.Versiones)
                                            { %>
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td class="docuField"><a  style="cursor: pointer;" href="DownloadGestor.aspx?file=<%=HttpUtility.UrlEncode(fileVer.NombreFicheroEncriptado) %>"><span><%=fileVer.NombreFichero.Split('/').Last()%></span></a></td>
                                                <td class="dateField"><span><%=fileVer.FechaFichero.ToString() %></span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <% } %>
                                    <% } %>
                                </li>
                                <% } %>
                            </ul>
                        </li>
                        <% } %>
                        <% } %>
                    </ul>
                </li>
            </ul>
        </div>
    </div>
    <div class="eosTituloWizard">
            <p>DOCUMENTACIÓN ASOCIADA AL EXPEDIENTE</p>
         <% if (DocumentosExpedientes.Count > 0)
                { %>
        <div class="download-container">
            <a class="download" style="cursor: pointer;" href="DownloadGestorAllZip.aspx?file=<%=HttpUtility.UrlEncode(DocumentosExpedientes.FirstOrDefault().Ruta) %>">Descargar todo</a>
        </div>
    <% } %>
    </div>
    <div id="eosContentDocument">
        <div class="eosTablaResultados">
            <% if (DocumentosExpedientes.Count == 0)
                { %>
            <div style="background-color: white;">
                <span style="margin-left: 20px">No hay documentos para este expediente.</span>
            </div>
            <% } %>
            <ul class="QC_Menu">
                <li class="QC_Item" style="display: list-item;">
                    <ul class="downCarpeta">
                        <% foreach (var doc in DocumentosExpedientes)
                            { %>
                        <a class="downFile" style="cursor: pointer;" href="DownloadGestorZip.aspx?file=<%=HttpUtility.UrlEncode(doc.Ruta) %>">Descargar carpeta</a>
                        <li class="QC_Item pos01"><a style="cursor: pointer;"><div class="icoField"></div><span><%=doc.Ruta.Split('\\').Last()%></span></a></li>
                        
                        <li class="QC_Item_Open" style="display: none;">
                            <ul class="">
                                <% if(IsExpIndividual) { %>
                                <% foreach (var subDoc in doc.SubTipo)
                                    { %>
                                <li class="QC_Item pos02"><a style="cursor: pointer;"><span><%=subDoc.Ruta.Split('\\').Last()%> </span></a></li>
                                <li class="QC_Item_Open" style="display: none;">
                                    <ul class="">
                                        <% foreach (var file in subDoc.Ficheros)
                                            { %>
                                        <li class="QC_Item pos03"><a style="cursor: pointer;"><span><%=file.NombreOrignal %></span></a></li>
                                        <li class="QC_Item_Open" style="display: none;">
                                            <ul class="">
                                                <% foreach (var fileVer in file.Versiones)
                                                    { %>
                                                <li class="QC_Item pos04" style="cursor: auto;">
                                                    <table>
                                                        <tbody>
                                                            <% if (!fileVer.DocumentoNoValido)
                                                            { %>
                                                            <tr>
                                                                <td class="docuField">
                                                                    <a style="cursor: pointer;" href="DownloadGestor.aspx?file=<%=HttpUtility.UrlEncode(fileVer.NombreFicheroEncriptado) %>">
                                                                        <span><%=fileVer.NombreFichero.Split('/').Last()%></span>
                                                                    </a>
                                                                </td>
                                                                <td class="dateField"><span><%=fileVer.FechaFichero.ToString() %></span></td>
                                                                <td class="contentValidDoc">
                                                                    <% if (!fileVer.DocumentoNoValido)
                                                                    { %>
                                                                        <a style="cursor: pointer;" class="delete" onclick="ShowPopup('<%=fileVer.NombreFicheroEncriptado%>')">Invalidar</a>
                                                                    <% }
                                                                        else
                                                                        { %>
                                                                    <img class="marked" src="Styles/images/Boton-cerrar.png" />
                                                                    <% } %>
                                                                </td>
                                                            </tr>
                                                             <% } %>
                                                        </tbody>
                                                    </table>
                                                </li>
                                                <% } %>
                                            </ul>
                                        </li>
                                        <% } %>
                                    </ul>
                                </li>
                                <% } %>
                                <% } else { %>
                                
                                        <% foreach (var file in doc.Ficheros)
                                            { %>
                                        <li class="QC_Item pos02 colectivo" style="cursor: auto;"><a style="cursor: pointer;"><span><%=file.NombreOrignal %></span></a></li>
                                        <li class="QC_Item_Open" style="display: none;">
                                            <ul class="">
                                                <% foreach (var fileVer in file.Versiones)
                                                    { %>
                                                <li class="QC_Item pos03 colectivo">
                                                    <table>
                                                        <tbody>
                                                            <% if (!fileVer.DocumentoNoValido)
                                                            { %>
                                                            <tr>
                                                                <td class="docuField"><a style="cursor: pointer;" href="DownloadGestor.aspx?file=<%=HttpUtility.UrlEncode(fileVer.NombreFicheroEncriptado) %>"><span><%=fileVer.NombreFichero.Split('/').Last()%></span></a></td>
                                                                <td class="dateField"><span><%=fileVer.FechaFichero.ToString() %></span></td>
                                                                <td class="contentValidDoc"><% if (!fileVer.DocumentoNoValido)
                                                                                                { %>
                                                                    <a style="cursor: pointer;" class="delete" onclick="ShowPopup('<%=fileVer.NombreFicheroEncriptado%>')">Invalidar</a>
                                                                    <% }
                                                                        else
                                                                        { %>
                                                                    <img class="marked" src="Styles/images/Boton-cerrar.png" />
                                                                    <% } %>
                                                                </td>
                                                            </tr>
                                                            <% } %>
                                                        </tbody>
                                                    </table>
                                                </li>
                                                <% } %>
                                            </ul>
                                        </li>
                                        <% } %>
                                <% } %>
                            </ul>
                        </li>
                        <% } %>
                    </ul>
                </li>
            </ul>
        </div>
    </div>
</asp:Content>
<asp:Content ID="cphScripts" ContentPlaceHolderID="ContentPlaceHolderScripts" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("ul#eosHPBotones li").removeClass("active");
            $("ul#eosHPBotones li#liDocumentacionAuditoria").addClass("active");
        });
    </script>
</asp:Content>