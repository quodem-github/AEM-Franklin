<%@ Page Title="" Language="C#" MasterPageFile="~/Styles/EOS.Master" AutoEventWireup="true"
    CodeBehind="DetalleExpediente.aspx.cs" Inherits="EOS.DetalleExpediente" Culture="es-ES"
    UICulture="es" %>

<%@ Import Namespace="NPOI.SS.Util" %>

<%@ Register Src="~/Controls/ModalDocumentUploader.ascx" TagName="DocumentModal" TagPrefix="cc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <script type="text/javascript" src="/Scripts/jQuery/jquery-1.12.4.min.js"></script>
    <script type="text/javascript">
        var jQuery_1_12 = $.noConflict(true);
    </script>
    <script type="text/javascript">

        var DocumentData = {};
        var DocumentDataOrignal = {};
        var originalTable = null;
        var formdata = new FormData();
        var fileList = [];
        var simpleItem = {};
        var indexHelper = 0;
        var DocTypeList = <%=DocTypes%>;
        var DocumentNameTypeSubType = <%=DocumentNameTypeSubType%>;
        var isFirstDoc = true;
        var isExpIndividual = true;

        function LoadDocTypesAndSubTypes() {
            var options = $("#ddlTipo");
            var subtipos = $("#ddlSubTipo");
            $.each(DocTypeList, function (key, value) {
                options.append($("<option />").val(value.Id).text(value.Nombre));
                if (value.SubTypes.length > 0 && value.Id.toString() === '1') {
                    $("#ddlSubTipo").empty();
                    $.each(value.SubTypes, function (k, v) {
                        subtipos.append($("<option />").val(v.Id).text(v.Nombre));
                    });
                };
            });

        }



        function TypeChanged() {
            $('#DocVersion').prop('checked', false);
            ShowDocumentddl($('#DocVersion'));
            var ficherosOriginales = $("#ddlNombreFichero");
            ficherosOriginales.empty();
            $.each(DocumentNameTypeSubType, function (key, value) {
                if (value.IdTipo.toString() === $("#ddlTipo").val()) {
                    ficherosOriginales.append($("<option />").val(value.NombreOriginal).text(value.NombreOriginal));
                }
            });
            if ($("#ddlTipo :selected").text() === 'Otros') {
                SubTypeChanged();
            }
        }

        function ReloadFiles() {
            var ficherosOriginales = $("#ddlNombreFichero");
            ficherosOriginales.empty();
            if (isExpIndividual) {
                $.each(DocumentNameTypeSubType, function (key, value) {
                    if (value.IdTipo.toString() === $("#ddlTipo").val() && value.IdSubTipo.toString() === $("#ddlSubTipo").val()) {
                        ficherosOriginales.append($("<option />").val(value.NombreOriginal).text(value.NombreOriginal));
                    }
                });
            } else {
                $.each(DocumentNameTypeSubType, function (key, value) {
                    if (value.IdTipo.toString() === $("#ddlTipo").val() && value.IdSubTipo.toString() === "-1") {
                        ficherosOriginales.append($("<option />").val(value.NombreOriginal).text(value.NombreOriginal));
                    }
                });
            }
        }

        function SubTypeChanged() {
            $('#DocVersion').prop('checked', false);
            ShowDocumentddl($('#DocVersion'));
            var ficherosOriginales = $("#ddlNombreFichero");
            ficherosOriginales.empty();
            $.each(DocumentNameTypeSubType, function (key, value) {
                if (value.IdTipo.toString() === $("#ddlTipo").val() && value.IdSubTipo.toString() === $("#ddlSubTipo").val()) {
                    ficherosOriginales.append($("<option />").val(value.NombreOriginal).text(value.NombreOriginal));
                }
            });
        }

        function EnableAditionalText() {
            if (($("#ddlSubTipo :selected").text() === 'Otros' && isExpIndividual) || (!isExpIndividual))
                $("#NombreAdicional").prop('disabled', false);
            else {
                $("#NombreAdicional").prop('disabled', true);
                $("#NombreAdicional").val('');
            }
        }

        function ResetControls() {
            $('#upload').val('');
            $('#uploadName').text('Examinar')
            $('#ddlTipo').val(1);
            $('#ddlSubTipo').val(1);
            $('#ModalDocumentId_ddlAsistente').val(-1);
            $('#DocVersion').attr('checked', false);
            $('#ddlNombreFichero').val();
            $('#NombreAdicional').val('');
            $('#ddlNombreFichero').parent().hide();
            TypeChanged();
            ChangeSubTypes();
        }

        function ChangeSubTypes() {

            var subtipos = $("#ddlSubTipo");
            $("#ddlSubTipo").empty();
            subtipos.append($("<option />").val(-1).text(""));
            $.each(DocTypeList, function (key, value) {
                if (value.Id.toString() === $("#ddlTipo").val()) {
                    if (value.SubTypes.length > 0) {
                        $("#ddlSubTipo").empty();
                        $.each(value.SubTypes, function (k, v) {
                            subtipos.append($("<option />").val(v.Id).text(v.Nombre));
                        });
                    };
                }
            });
            EnableAditionalText();
        }

        $(document).ready(function () {
            if ('<%=IsExpIndividual%>' === 'False') {
                isExpIndividual = false;
            }
            LoadDocTypesAndSubTypes();
            $('#ddlTipo').change(function () {
                TypeChanged();
            });

            $('.popup').click(function (e) {
                e.preventDefault();
            });

            $('#ddlSubTipo').change(function () {
                SubTypeChanged();
            });
            originalTable = $.extend({}, $('#BucketTable tr'));
            formdata = new FormData();
            jQuery_1_12.ajax({
                method: 'GET',
                url: '/api/UploadFile/GetList.aspx',
                success: function (data) {
                    DocumentData = data.List;
                    DocumentDataOrignal = DocumentData;
                    simpleItem = data.obj;
                },
                error: function (data) {
                    alert(data);
                }
            });
        });

        function SendDataFile() {
            if (!$('a.sendDocument').hasClass('disabled')) {

                $("a.sendDocument").addClass("disabled");
                if (DocumentData.List.length === 0) {
                    alert('Adjunte un fichero');
                    return;
                }

                /*Versión buena*/
                var data = JSON.stringify(DocumentData);
                jQuery_1_12.ajax({
                    method: 'POST',
                    url: '/api/UploadFile/SaveFileInfo.aspx',
                    contentType: 'application/json',
                    data: data,
                    success: function (data) {
                        var errorInPost = false;
                        $.each(fileList,
                            function (key, value) {
                                formdata = new FormData();
                                formdata.append(value.filename, value.file, value.name);

                                jQuery_1_12.ajax({
                                    method: 'POST',
                                    url: '/api/UploadFile/PostFormData.aspx',
                                    cache: false,
                                    contentType: false,
                                    processData: false,
                                    data: formdata,
                                    async: false,
                                    success: function (data) {

                                    },
                                    error: function (data) {
                                        errorInPost = true;
                                    }
                                });
                            });
                        if (errorInPost) {
                            alert('Ha habido un error al enviar el fichero al servidor.');
                            $("a.sendDocument").removeClass("disabled");
                        }
                        else {
                            $('#modal').dialog('close');
                            alert('Fichero(s) almacenados correctamente.');
                            $('#BucketTable tr').remove();
                            $('#BucketTable').append(originalTable[0]);
                            DocumentData.List = [];
                            fileList = [];
                            DocumentData.List = DocumentDataOrignal.List;
                            $("a.sendDocument").removeClass("disabled");
                        }

                    },
                    error: function (data) {
                        alert('Ha habido un error al guardar los datos del fichero en el servidor.');
                        $("a.sendDocument").removeClass("disabled");
                    }
                });

            };
        };

        function ReDraw() {

            //$("#BucketTable tbody tr").remove();
            $('#BucketTable tr').remove();
            $('#BucketTable').append(originalTable[0]);
            var isFirst = DocumentData.List.length === 1;
            $.each(DocumentData.List, function (key, value) {
                var nombreFichero = "";
                if (value.NuevaVersion)
                    nombreFichero = $('#ddlNombreFichero :selected').text();
                var count = value.Fichero.split('\\').length;
                var newRow = '<tr>';
                newRow += '<td><span>' + value.Fichero.split('\\')[count - 1] + '</span></td>';
                newRow += '<td><span>' + ObtenerNombreTipo(value.Tipo) + '</span></td>';
                newRow += '<td><span>' + ObtenerNombreSubtipo(value.SubTipo) + '</span></td>';
                newRow += '<td><span>' + value.NombreAdicional + '</span></td>';
                newRow += '<td><label></label><span>' + value.AsistenteNombre + '</span></td>';

                newRow += '<td><input type="checkbox" onclick="return false;"';
                if (value.NuevaVersion)
                    newRow += 'checked /></td>';
                else {
                    newRow += '/></td>';
                }
                newRow += '<td><span>' + nombreFichero + '</span></td>';
                newRow += '<td><a onclick="DeleteFile(' + value.Indexhelper + ')">BORRAR</a></td>';
                newRow += '</tr>';

                if (isFirst) {
                    $('#BucketTable').append(newRow);
                } else {
                    $('#BucketTable tr:last').after(newRow);
                }


            });
        }

        function CheckIfExist(document) {
            var result = false;
            $.each(DocumentData.List, function (key, value) {

                if (isExpIndividual) {
                    if (value.Tipo === document.Tipo &&
                        value.SubTipo === document.SubTipo &&
                        value.Asistente === document.Asistente &&
                        value.DocumentoOriginal === document.DocumentoOriginal &&
                        value.Fichero === document.Fichero) {
                        result = true;
                    }
                } else {
                    if (value.Tipo === document.Tipo &&
                        value.Asistente === document.Asistente &&
                        value.DocumentoOriginal === document.DocumentoOriginal &&
                        value.Fichero === document.Fichero) {
                        result = true;
                    }
                }
            });
            return result;
        }

        function CheckFileNameLength() {
            //La suma de la ruta del fichero + las constantes como el numero de caracteres del amec y del expediente
            var serverPathLengthCount = 72;
            serverPathLengthCount = $('#ddlTipo :selected').text().length + serverPathLengthCount;
            if (isExpIndividual) {
                serverPathLengthCount = $('#ddlSubTipo :selected').text().length + serverPathLengthCount;
            }
            var fichero = $('#upload').val().length;
            var nombAdicional = $('#NombreAdicional').val().length;
            var docVer = $('#DocVersion').is(":checked");
            if (docVer) {
                fichero = $('#ddlNombreFichero :selected').text().length;
            }
            var asistente = $('#ModalDocumentId_ddlAsistente :selected').text().length;

            var result = serverPathLengthCount + fichero + nombAdicional + asistente;

            if (result > 240) {
                return true;
            } else {
                return false;
            }


        }

        function AddTableElement() {
            if (CheckFileNameLength()) {
                alert('La combinación de nombre del fichero, asistente (y si aplica, nombre adicional) es demasiada larga y ' +
                    'sobrepasa la cantidad permitida de caracteres. Por favor, renombre el fichero para poder añadirlo.');
                return;
            }
            var file = $('#upload')[0].files[0];

            if (!file) {
                alert('Adjunte un fichero');
                return;
            }
            var tipo = $('#ddlTipo').val();
            var subTipo = -1;
            if (isExpIndividual) { subTipo = $('#ddlSubTipo').val() }
            var asistente = $('#ModalDocumentId_ddlAsistente').val();
            var asistenteNombre = $('#ModalDocumentId_ddlAsistente :selected').text();
            var fichero = $('#upload').val();
            var docVer = $('#DocVersion').is(":checked");
            var nombOriginal = "";
            var nombAdicional = $('#NombreAdicional').val();
            var amec = '<%=entExp.Amec %>';
            var expediente = <%=entExp.Idexpediente %>;
            var peticionario = <%=IdPeticionario %>;

            if (docVer) {
                nombOriginal = $('#ddlNombreFichero :selected').text();
            }

            if (!asistente)
                asistente = "COMUNES";

            var localItem = $.extend({}, simpleItem);
            localItem.Tipo = tipo;
            localItem.SubTipo = subTipo;
            localItem.Asistente = asistente;
            localItem.AsistenteNombre = asistenteNombre;
            localItem.Fichero = fichero;
            localItem.Amec = amec;
            localItem.Expediente = expediente;
            localItem.NombreAdicional = nombAdicional;
            localItem.NuevaVersion = docVer;
            localItem.DocumentoOriginal = nombOriginal;
            localItem.Peticionario = peticionario;
            localItem.Indexhelper = indexHelper;
            if (CheckIfExist(localItem)) {
                alert('El documento ya está presente en la lista');
            } else {


                DocumentData.List.push(localItem);

                //formdata = new FormData();
                //var separacion = file.name.split(".");
                //formdata.append(separacion[0] + "_" + peticionario, file, separacion[0] + "_" + peticionario);

                var separacion = file.name.split(".");
                var filename = file.name +
                    "|" + tipo +
                    "|" + subTipo +
                    "|" + asistente +
                    "|" + peticionario +
                    "|" + expediente +
                    "|" + amec +
                    "|" + nombOriginal;
                var fileItem = [];
                fileItem.indexHelper = indexHelper;
                fileItem.filename = filename;
                fileItem.file = file;
                //fileItem.name = separacion[0] + "_" + peticionario + "." + file.name.split(".").pop();
                fileItem.name = file.name;
                fileList.push(fileItem);
                //formdata.append(filename,file);

                ReDraw();
                indexHelper++;
                ResetControls();
            }
        };

        function DeleteFile(idHelper) {
            var DocumentDataAux = [];
            var fileListAux = [];
            $.each(DocumentData.List, function (key, value) {
                if (value.Indexhelper !== idHelper) {
                    DocumentDataAux.push(value);
                }
            });
            $.each(fileList, function (key, value) {
                if (value.indexHelper !== idHelper) {
                    fileListAux.push(value);
                }
            });
            fileList = fileListAux;
            DocumentData.List = DocumentDataAux;
            ReDraw();
        }

        function ObtenerNombreTipo(id) {
            var nombre = "";
            $.each(DocTypeList, function (key, value) {
                if (value.Id.toString() === id)
                    nombre = value.Nombre;
            });
            return nombre;
        }

        function ObtenerNombreSubtipo(id) {
            var nombresubtipo = "";
            $.each(DocTypeList, function (key, value) {
                if (value.SubTypes.length > 0) {
                    $.each(value.SubTypes, function (k, v) {
                        if (v.Id.toString() === id)
                            nombresubtipo = v.Nombre;
                    });
                };
            });
            return nombresubtipo;
        }
        function ShowDocumentddl(input) {
            var checked = $(input).is(":checked");
            ReloadFiles();
            if (checked) {
                if ($('#ddlNombreFichero').children('option').length === 0) {
                    alert('Esta configuración no tiene documentos para versionar.');
                    $(input).prop('checked', false);
                }
                else
                    $(input).parent().next('td').show();
            }
            else
                $(input).parent().next('td').hide();
        }

        function ShowPopup(type) {
            $('#ddlTipo').val(type);
            ChangeSubTypes();
            $(function () {
                $("#modal").html();
                $("#modal").dialog({
                    title: "Añadir archivos",
                    buttons: {
                        Cerrar: function () {
                            $(this).dialog('close');
                        }
                    },
                    modal: true,
                    width: 1000
                });
            });
        };

        function InputFileChange(input) {
            $("#uploadName").text($(input).val().replace(/.*[\/\\]/, ''));
        };

        function ShowOptionalBox(ddl) {
            $(ddl).val();
        }


        function ConfirmaCalcelacion(idExpediente) {
            return confirm('¿Desea cancelar el expediente ' + idExpediente + '?');
        }

        function ConfirmAprobarBtn() {
            var idExpediente = <%=entExp.Idexpediente %>;
            return ConfirmaModificacionBInscripcion("Este proceso acepta sólo los servicios cotizados por participante, ¿desea continuar?", idExpediente, 0, 0, 0, 0);
        }

        function ConfirmaModificacionB(idExpediente, idReserva, idServicio, idServicioAlojamiento, idServicioTransporte) {
            if (idExpediente != null && idExpediente != undefined && idReserva != null && idReserva != undefined && idServicio != null && idServicio != undefined && (idServicioAlojamiento != null && idServicioAlojamiento != undefined || idServicioTransporte != null && idServicioTransporte != undefined)) {
                return ConfirmaModificacionBInscripcion("Esta cotización se materializará y tendrá gastos de Cancelación", idExpediente, idReserva, idServicio, idServicioAlojamiento, idServicioTransporte);
            }
            else {
                return confirm("Esta cotización se materializará y tendrá gastos de Cancelación");
            }
        }

        function ConfirmaModificacionBInscripcion(mensajeConfirm, idExpediente, idReserva, idServicio, idServicioAlojamiento, idServicioTransporte) {
            var result = false;
            $.ajax({
                type: "POST",
                url: "DetalleExpediente.aspx/CheckJustificanteInscripcion",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{ "idExpediente":"' + idExpediente + '", "idReserva":"' + idReserva + '" , "idServicio":"' + idServicio + '", "idServicioAlojamiento":"' + idServicioAlojamiento + '", "idServicioTransporte":"' + idServicioTransporte + '"}',
                async: false,
                success: function (response) {
                    if (response.d == true) {
                        result = confirm(mensajeConfirm);
                    }
                    else {
                        alert("Falta la inscripción y/o certificado de algunos de los asistentes para este congreso");
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert("Se ha producido un error comprobando los justificantes de pago");
                }
            });
            return result;
        }

        function ConfirmaModificacionC(idEstado) {

            var msg;

            if (idEstado == 'AB' || idEstado == 'CR' || idEstado == 'CTZ' || idEstado == 'CFG' || idEstado == 'CTZD') {
                msg = "anular";
            }
            else {

                if (idEstado == 'CFP' || idEstado == 'PTR' || idEstado == 'TR') {
                    msg = "cancelar";
                }
                else {
                    msg = "rechazar";
                }

            }

            return confirm("¿Está seguro de que desea " + msg + " la reserva?");
        }

        function ToggleAdvancedInfo(celda) {

            var filaOculta = celda.parentNode.parentNode.nextSibling;
            if (filaOculta.innerHTML == null) // Esto es porque en firefox encuentra otro elemento antes que la siguiente TR
                filaOculta = filaOculta.nextSibling;

            var sActual = celda.innerHTML;

            if (sActual == '+') {
                filaOculta.className = 'datoVisible';
                celda.innerHTML = '-';
            } else {
                filaOculta.className = 'datoOculto';
                celda.innerHTML = '+';
            }
        }
    </script>

    <script type="text/javascript" src="Scripts/detalleExpediente.js"></script>
</asp:Content>

<asp:Content ID="ContentDetalles" ContentPlaceHolderID="eosHeaderContent" runat="server">
</asp:Content>


<asp:Content ID="ContentResultados" ContentPlaceHolderID="eosContentResults" runat="server">

    <h1>Detalle Expediente</h1>



    <div id="eosFilterHeaderDetalle" class="contentDetalleGeneric">

        <h2>Información</h2>
        <div id="infoPanel">

            <div id="eosFilterHeaderEstadoDetalle" runat="server" clientidmode="Static">
                <span class="contentIconEstado"></span>
                <asp:Label ID="lblEstado" runat="server" Text=""></asp:Label>
            </div>

            <div class="contentInfoPanel">


                <div class="contentInfoPanelElement">
                    <!--Cambio txtnombre Marco 22/02/2011-->
                    <span class="lblTitle">Evento:</span>
                    <asp:Label ID="lblCongreso" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                    <!--<span class="lblTitle">| FECHAS DEL CONGRESO:</span>
                        <asp:Label ID="lblFechas" runat="server" Text="Label" 
                            CssClass="eosCampoLabelDato"></asp:Label>-->
                    <!--Añadido Marco 21/02/2011-->
                    <span class="separateElement">|</span>
                    <span class="lblTitle">Lugar:</span>
                    <asp:Label ID="LblPoblacion" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>



                </div>




                <div class="contentInfoPanelElement">

                    <span class="lblTitle">Inicio:</span>
                    <asp:Label ID="lblFechaDesde" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                    <span class="separateElement">|</span>
                    <span class="lblTitle">Fin:</span>
                    <asp:Label ID="LblFechaHasta" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                    <span class="separateElement">|</span>
                    <span class="lblTitle">Agencia: </span>
                    <asp:Label ID="lblAgencia"
                        runat="server" Text="" CssClass="eosCampoLabelDato"></asp:Label>
                </div>

                <div class="contentInfoPanelElement">

                    <span class="lblTitle">Empresa:</span>
                    <asp:Label ID="lblCompany"
                        runat="server" Text="" CssClass="eosCampoLabelDato"></asp:Label>
                    <!--Fin de añadido datos Marco 21/02/2011-->

                </div>
                <div class="contentInfoPanelElement">

                    <span class="lblTitle">Fecha expediente:</span>
                    <asp:Label ID="lblFecha" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                    <span class="separateElement">|</span>
                    <span class="lblTitle">Pedido:</span>
                    <asp:Label ID="lblPedido" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                    <!-- Xavier Morell (GP) 16-01-11-->
                    <span class="separateElement">|</span>
                    <span class="lblTitle">Nº Expediente:</span>
                    <asp:Label ID="lblExpediente" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                    <asp:HyperLink ID="eosAmecBusca" runat="server" ImageUrl="~/Styles/images/ic_lupa.png"
                        NavigateUrl="javascript:NoImplementado();" Visible="false"></asp:HyperLink>
                </div>

                <div class="contentInfoPanelElement">

                    <span class="lblTitle">AMEC:</span>
                    <asp:Label ID="lblAMEC" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                    <span class="separateElement">|</span>
                    <span class="lblTitle">Vinculado a:</span>
                    <asp:Label ID="lblProductos" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                    <span class="separateElement">|</span>
                    <span class="lblTitle">Peticionario:</span>
                    <asp:Label ID="lblPeticionario" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                </div>
                <div class="contentInfoPanelElement">
                    <span class="lblTituloTipoPago">Tipo de pago:</span>
                    <asp:Label ID="lblTipoPago" runat="server" Text="Label" CssClass="eosCampoLabelDato"></asp:Label>
                </div>

            </div>

            <%--             <asp:Table runat="server" Width="100%">
                <asp:TableRow>
         
                    <asp:TableCell HorizontalAlign="Right" runat="server" ID="botoneraAcciones">
                        <div>
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Styles/images/bt_enviar.png"
                                OnClick="btnEnviar_Click" OnClientClick="javascript:return confirm('Este proceso cursa los servicios que estén sin enviar, ¿desea continuar?')" />&nbsp;
                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Styles/images/bt_aprobar.png"
                                OnClick="btnAprobar_Click" OnClientClick="javascript:return ConfirmAprobarBtn()" />&nbsp;
                            <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Styles/images/bt_cancelar_exp.png"
                                OnClick="btnCancelar_Click" OnClientClick="javascript:return confirm('Este proceso cancela el expediente por completo, pulse Aceptar si desea cancelar el expediente ¿desea continuar?')" />
                        </div>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>--%>

            <div class="eosHPBotonesDetalle" id="">

                <asp:TableCell HorizontalAlign="Right" runat="server" ID="botoneraAcciones">
                    <asp:Button ID="btnEnviar" runat="server" Text="Enviar"
                        OnClick="btnEnviar_Click" OnClientClick="javascript:return confirm('Este proceso cursa los servicios que estén sin enviar, ¿desea continuar?')" />

                    <asp:Button ID="btnAprobar" runat="server" Text="Aprobar"
                        OnClick="btnAprobar_Click" OnClientClick="javascript:return ConfirmAprobarBtn()" />

                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"
                        OnClick="btnCancelar_Click" OnClientClick="javascript:return confirm('Este proceso cancela el expediente por completo, pulse Aceptar si desea cancelar el expediente ¿desea continuar?')" />

                </asp:TableCell>
            </div>
        </div>
        <div id="eosHPBotonesDetalle" class="eosBotonera eosBotonFiltrado">

            <a id="lnkModificar" runat="server" title="Modificar Expediente" href="NuevoExpedientePasoA.aspx?idexp=0">Modificar Expediente</a>

            <!--<a title="Volver al listado de Expedientes" href="Expedientes.aspx">Volver al listado
                                    de Expedientes</a>-->

        </div>

        <div class="contentDetalleGeneric contentAddService" id="divAnadirServicio" runat="server">

            <h2>Añadir Servicio</h2>
            <asp:DropDownList ID="lstServicios" runat="server" CssClass="eosDisabledInputVacio eosCampoLabelCombo"
                OnSelectedIndexChanged="lstServicios_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="#">Añadir Servicio</asp:ListItem>
                <asp:ListItem Value="Servicios.aspx?idexp=1&tab=1">Inscripciones (CAM)</asp:ListItem>
                <asp:ListItem Value="Servicios.aspx?idexp=1&tab=2">Alojamientos (CAM)</asp:ListItem>
                <asp:ListItem Value="Servicios.aspx?idexp=1&tab=3">Transportes (CAM)</asp:ListItem>
                <asp:ListItem Value="Servicios.aspx?idexp=1&tab=4">Otros Servicios</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="cboTipo" runat="server" OnSelectedIndexChanged="cboTipo_SelectedIndexChanged"
                AutoPostBack="True">
            </asp:DropDownList>
        </div>
    </div>

    <div class="contentDetalleGeneric contentServicesBox contentTableGeneric" id="divInscripciones" runat="server">

        <h2>Inscripciones</h2>


        <asp:ListView ID="lvServicioInscripcion" runat="server" DataSourceID="odsServiciosInscripciones"
            OnItemDataBound="lvServicioInscripcion_ItemDataBound" OnLayoutCreated="lvServicioInscripcion_LayoutCreated"
            OnItemCommand="Listas_ItemCommand"
            Visible="<%# (entExp.Idtiporeserva == 1) %>">
            <EmptyDataTemplate>
                <table class="eosTablaResultados">
                    <thead style="font-weight: bold;">
                        <tr>
                            <th>
                                <%--  <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/images/ic_modificar.png" />--%>
                            </th>
                            <th>
                                <%-- <asp:Image ID="Image2" runat="server" ImageUrl="~/Styles/images/ic_aceptado.png" />--%>
                            </th>
                            <th>
                                <%--    <asp:Image ID="Image3" runat="server" ImageUrl="~/Styles/images/ic_cancelado.png" />--%>
                            </th>
                            <th width="15%">Inscripción
                            </th>
                            <th width="15%">Importe Max.
                            </th>
                            <th width="30%">Observaciones
                            </th>
                            <th class="dato">Pax
                            </th>
                            <th class="dato">
                                <asp:Image ID="Image4" runat="server" ImageUrl="~/Styles/images/ic_alternativas.png" />
                            </th>
                            <th class="dato">
                                <asp:Image ID="Image5" runat="server" ImageUrl="~/Styles/images/ic_inscripciones.png" />
                            </th>
                            <th class="cotizado">Importe
                            <!-- Ismael Ameller 03-03-2011 Cambio de label -->
                                <%--Cotizado--%>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="13">
                                <span class="eosTituloRojo">No se han encontrado inscripciones en este expediente</span>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table class="eosTablaResultados">
                    <thead style="font-weight: bold;">
                        <tr>
                            <th>
                                <%-- <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/images/ic_modificar.png" />--%>
                            </th>
                            <th>
                                <%--<asp:Image ID="Image2" runat="server" ImageUrl="~/Styles/images/ic_aceptado.png" />--%>
                            </th>
                            <th>
                                <%--<asp:Image ID="Image3" runat="server" ImageUrl="~/Styles/images/ic_cancelado.png" />--%>
                            </th>
                            <th width="15%">Inscripción
                            </th>
                            <th width="15%">Importe Max.
                            </th>
                            <th width="30%">Observaciones
                            </th>
                            <th class="dato">Pax
                            </th>
                            <th class="dato">
                                <asp:Image ID="Image4" runat="server" ImageUrl="~/Styles/images/ic_alternativas.png" />
                            </th>
                            <th class="dato">
                                <asp:Image ID="Image5" runat="server" ImageUrl="~/Styles/images/ic_inscripciones.png" />
                            </th>
                            <th class="cotizado">Importe
                            <!-- Ismael Ameller 03-03-2011 Cambio de label -->
                                <%--Cotizado--%>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </tbody>
                    <!--<tfoot>-->
                    <tr class="borderDashedTopTotal">
                        <td colspan="6" class="eosTablaResultadosResumen par"></td>
                        <td class="eosTablaResultadosResumen par">
                            <asp:Label ID="InscripcionPax" runat="server" />
                            <a title="Mostrar Información del PAX"
                                onclick="javascript:ToggleAdvancedInfo(this);return false;" onmouseout="javascript:window.status='';return true;"
                                onmouseover="javascript:window.status='Mostrar Información del PAX';return true" class="contentMoreInfo">+</a>
                        </td>

                        <td colspan="3" class="eosTablaResultadosResumen par totalStrong">
                            <asp:Label ID="InscripcionTotal" runat="server" />
                        </td>
                    </tr>



                    <tr class="datoOculto">
                        <td colspan="10">
                            <div class="eosExpedienteDetalleAmpliado">
                                <asp:Label ID="InscripcionPaxDetalleTotal" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr class="borderDashedTop">
                        <td colspan="12">
                            <a href="#" class="popup addDocumentLink" onclick="ShowPopup(3);">
                                <%--<img src="Styles/images/document_add.png" />--%>Añadir documentos</a>
                        </td>
                    </tr>


                    <!--</tfoot>-->
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td class="par">
                        <asp:ImageButton ID="imgServicioMod" runat="server" ToolTip='<%# String.Format("Modificar el servicio {0}", Eval("inscripcion")) %>'
                            ImageUrl="~/Styles/images/ic_modificar.png" CommandName="MOD0" CommandArgument='<%# Eval("idreserva") %>' />
                    </td>
                    <td class="par">
                        <asp:ImageButton ID="imgServicioAprobar" runat="server" ToolTip='<%# String.Format("Confirmar el servicio {0}", Eval("inscripcion")) %>'
                            ImageUrl="~/Styles/images/ic_aceptado.png" CommandName="ACE" CommandArgument='<%# Eval("idreserva") %>'
                            OnClientClick='<%# String.Format("return ConfirmaModificacionB();") %>' OnClick="AprobarMail_Click" />
                        <asp:Image ID="imgServicioAprobado" runat="server" ImageUrl="~/Styles/images/ic_estado_aceptado.png"
                            Visible="false" />
                    </td>
                    <td class="par">
                        <asp:ImageButton ID="imgServicioCancelar" runat="server" ToolTip='<%# String.Format("Rechazar el servicio {0}", Eval("inscripcion")) %>'
                            ImageUrl="~/Styles/images/ic_cancelado.png" CommandName="CAN" CommandArgument='<%# Eval("idreserva") %>' />

                        <asp:Image ID="imgServicioCancelado" runat="server" ImageUrl="~/Styles/images/ic_estado_cancelado.png"
                            Visible="false" />
                    </td>
                    <td>
                        <%--<asp:Label ID="inscripcionLabel" runat="server" Text='<%# Eval("inscripcion") %>' />--%>
                        <%--Ismael Ameller 30-03-2011--%>
                        <%--<asp:Label ID="inscripcionLabel" runat="server" Text='<%# Eval("descripcion") %>' />--%>
                        <asp:Label ID="inscripcionLabel" runat="server" Text='<%# Eval("descripcion") == ""? Eval("descripcion") : Eval("inscripcion")%>' />
                    </td>
                    <td align="center" class="par">
                        <asp:Label ID="pvpLabel" runat="server" Text='<%# Eval("pvp")!=null ? ((double)Eval("pvp")).ToString("N2") + " €" : "0,00 €" %>' />
                    </td>
                    <td>
                        <asp:Label ID="observacionesLabel" runat="server" Text='<%# Eval("observaciones") %>' />
                    </td>
                    <td class="borderLeft">
                        <asp:Label ID="paxLabel" runat="server" Text='<%# String.Format("{0}",Eval("pax")) %>' />
                        <a title="Mostrar Información del PAX" onclick="javascript:ToggleAdvancedInfo(this);return false;"
                            onmouseout="javascript:window.status='';return true;" onmouseover="javascript:window.status='Mostrar Información del PAX';return true"
                            class="contentMoreInfo <%# Eval("pax")!=null ? "" : "oculta" %>">+</a>
                    </td>
                    <td class="par">
                        <asp:ImageButton ID="imgServicioAlternativas" runat="server" Visible='<%# GetEstadoShow(Eval("idestado").ToString())%>'
                            ToolTip='<%# String.Format("Mostrar Alternativas del Servicio {0}", Eval("inscripcion")) %>'
                            ImageUrl="~/Styles/images/ic_alternativas.png" />

                    </td>
                    <td align="center">
                        <span title='<%# GetEstadoTooltip(Eval("idestado").ToString())%>' class="eosImagenCotizacion <%# String.Format("eosImagenLeyenda{0}", Eval("idestado"))%>"></span>
                    </td>
                    <td align="right" class="par">
                        <asp:Label ID="cotizadoLabel" runat="server" Text='<%# Eval("cotizado")!=null ? ((float)Eval("cotizado")).ToString("N2") + " €" : "0,00 €" %>' />
                    </td>
                </tr>
                <tr class="datoOculto">
                    <td colspan="10">
                        <div class="eosExpedienteDetalleAmpliado">
                            <asp:Label ID="InscripcionPaxDetalle" runat="server" />
                        </div>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>

        <asp:ObjectDataSource ID="odsServiciosInscripciones" runat="server" SelectMethod="ObtenerServiciosInscripciones"
            TypeName="EOS.Web.AgenteExpedientes" OnSelecting="odsServiciosInscripciones_Selecting">
            <SelectParameters>
                <asp:Parameter DefaultValue="1" Name="idExpediente" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>

    </div>

    <div class="contentDetalleGeneric contentServicesBox contentTableGeneric" id="divAlojamiento" runat="server">

        <h2>Alojamiento</h2>



        <asp:ListView ID="lvServicioAlojamiento" runat="server" DataSourceID="odsServiciosAlojamientos"
            OnItemDataBound="lvServicioAlojamiento_ItemDataBound" OnLayoutCreated="lvServicioAlojamiento_LayoutCreated"
            OnItemCommand="Listas_ItemCommand" Visible="<%# (entExp.Idtiporeserva == 1) %>">
            <EmptyDataTemplate>
                <table class="eosTablaResultados">
                    <thead>
                        <tr>
                            <th>
                                <%-- <asp:Image ID="Image6" runat="server" ImageUrl="~/Styles/images/ic_modificar.png" />--%>
                            </th>
                            <th>
                                <%--  <asp:Image ID="Image7" runat="server" ImageUrl="~/Styles/images/ic_aceptado.png" />--%>
                            </th>
                            <th>
                                <%--    <asp:Image ID="Image8" runat="server" ImageUrl="~/Styles/images/ic_cancelado.png" />--%>
                            </th>
                            <th>Alojamiento
                            </th>
                            <th>Entrada
                            </th>
                            <th>Salida
                            </th>
                            <th>Hab
                            </th>
                            <th width="10%">Importe Max.
                            </th>
                            <th width="30%">Observaciones
                            </th>
                            <th class="dato">Pax
                            </th>
                            <th class="dato">
                                <asp:Image ID="Image9" runat="server" ImageUrl="~/Styles/images/ic_alternativas.png" />
                            </th>
                            <th class="dato">
                                <asp:Image ID="Image10" runat="server" ImageUrl="~/Styles/images/ic_alojamiento.png" />
                            </th>
                            <th class="cotizado">Importe
                            <!-- Ismael Ameller 03-03-2011 Cambio de label -->
                                <%--Cotizado--%>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="13">
                                <span class="eosTituloRojo">No se han encontrado alojamientos en este expediente</span>
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
                                <%--  <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/images/ic_modificar.png" />--%>
                            </th>
                            <th>
                                <%-- <asp:Image ID="Image2" runat="server" ImageUrl="~/Styles/images/ic_aceptado.png" />--%>
                            </th>
                            <th>
                                <%--   <asp:Image ID="Image3" runat="server" ImageUrl="~/Styles/images/ic_cancelado.png" />--%>
                            </th>
                            <th>Alojamiento
                            </th>
                            <th>Entrada
                            </th>
                            <th>Salida
                            </th>
                            <th>Hab
                            </th>
                            <th width="10%">Importe Max.
                            </th>
                            <th width="30%">Observaciones
                            </th>
                            <th class="dato">Pax
                            </th>
                            <th class="dato">
                                <asp:Image ID="Image4" runat="server" ImageUrl="~/Styles/images/ic_alternativas.png" />
                            </th>
                            <th class="dato">
                                <asp:Image ID="Image5" runat="server" ImageUrl="~/Styles/images/ic_alojamiento.png" />
                            </th>
                            <th class="cotizado">Importe
                            <!-- Ismael Ameller 03-03-2011 Cambio de label -->
                                <%--Cotizado--%>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </tbody>
                    <!--<tfoot>-->
                    <tr>
                        <td colspan="9" class="eosTablaResultadosResumen par"></td>
                        <td class="eosTablaResultadosResumen par">
                            <asp:Label ID="HotelPax" runat="server" />
                            <a title="Mostrar Información del PAX"
                                onclick="javascript:ToggleAdvancedInfo(this);return false;" onmouseout="javascript:window.status='';return true;"
                                onmouseover="javascript:window.status='Mostrar Información del PAX';return true" class="contentMoreInfo">+</a>
                        </td>

                        <td colspan="3" class="eosTablaResultadosResumen par totalStrong">
                            <asp:Label ID="HotelTotal" runat="server" />
                        </td>
                    </tr>
                    <tr class="datoOculto">
                        <td colspan="13">
                            <div class="eosExpedienteDetalleAmpliado">
                                <asp:Label ID="HotelPaxDetalleTotal" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr class="borderDashedTop">
                        <td colspan="13">
                            <a href="#" class="popup addDocumentLink" onclick="ShowPopup(2);">
                                <%--<img src="Styles/images/document_add.png" />--%>Añadir documentos</a>
                        </td>
                    </tr>
                    <!--</tfoot>-->
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td class="par">
                        <asp:ImageButton ID="imgServicioMod" runat="server" ToolTip='<%# String.Format("Modificar el servicio {0}", Eval("hotel")) %>'
                            ImageUrl="~/Styles/images/ic_modificar.png" CommandName="MOD1" CommandArgument='<%# Eval("idserviciohotel") %>' />
                    </td>
                    <td class="par">
                        <asp:ImageButton ID="imgServicioAprobar" runat="server" ToolTip='<%# String.Format("Confirmar el servicio {0}", Eval("hotel")) %>'
                            ImageUrl="~/Styles/images/ic_aceptado.png" CommandName="ACE" CommandArgument='<%# Eval("idreserva") %>'
                            OnClientClick='<%# String.Format("return ConfirmaModificacionB({0}, {1}, {2}, {3}, {4});", Eval("idexpediente"), Eval("idreserva"), Eval("idservicio"), Eval("idserviciohotel"), 0) %>' OnClick="AprobarMail_Click" />
                        <asp:Image ID="imgServicioAprobado" runat="server" ImageUrl="~/Styles/images/ic_estado_aceptado.png"
                            Visible="false" />
                    </td>
                    <td class="par">
                        <asp:ImageButton ID="imgServicioCancelar" runat="server" ToolTip='<%# String.Format("Rechazar el servicio {0}", Eval("hotel")) %>'
                            ImageUrl="~/Styles/images/ic_cancelado.png" CommandName="CAN" CommandArgument='<%# Eval("idreserva") %>' />
                        <asp:Image ID="imgServicioCancelado" runat="server" ImageUrl="~/Styles/images/ic_estado_cancelado.png"
                            Visible="false" />
                    </td>
                    <td>
                        <asp:Label ID="hotelLabel" runat="server" Text='<%# Eval("hotel") %>' />
                    </td>
                    <td class="par">
                        <asp:Label ID="entradaLabel" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}",Eval("fechahorallegada")) %>' />
                    </td>
                    <td>
                        <asp:Label ID="salidaLabel" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}",Eval("fechahorasalida")) %>' />
                    </td>
                    <td class="par">
                        <asp:Label ID="habitacionLabel" runat="server" Text='<%# Eval("idtipohab") %>' />
                    </td>
                    <td align="center">
                        <asp:Label ID="pvpLabel" runat="server" Text='<%# Eval("pvp")!=null ? ((double)Eval("pvp")).ToString("N2") + " €" : "0,00 €" %>' />
                    </td>
                    <td class="par">
                        <asp:Label ID="observacionesLabel" runat="server" Text='<%# Eval("observaciones") %>' />
                    </td>
                    <td class="borderLeft">
                        <asp:Label ID="paxLabel" runat="server" Text='<%# String.Format("{0}",Eval("pax")) %>' />
                        <a title="Mostrar Información del PAX" onclick="javascript:ToggleAdvancedInfo(this);return false;"
                            onmouseout="javascript:window.status='';return true;" onmouseover="javascript:window.status='Mostrar Información del PAX';return true"
                            class="contentMoreInfo <%# Eval("pax")!=null ? "" : "oculta" %>">+</a>
                    </td>
                    <td class="par">
                        <asp:ImageButton ID="imgServicioAlternativas" runat="server" Visible='<%# GetEstadoShow(Eval("idestado").ToString())%>'
                            ToolTip='<%# String.Format("Mostrar Alternativas del Servicio {0}", Eval("hotel")) %>'
                            ImageUrl="~/Styles/images/ic_alternativas.png" />
                    </td>
                    <td align="center">
                        <span title='<%# GetEstadoTooltip(Eval("idestado").ToString())%>' class="eosImagenCotizacion <%# String.Format("eosImagenLeyenda{0}", Eval("idestado"))%>"></span>
                    </td>
                    <td align="right" class="par">
                        <asp:Label ID="cotizadoLabel" runat="server" Text='<%# Eval("cotizado")!=null ? ((float)Eval("cotizado")).ToString("N2") + " €" : "0,00 €" %>' />
                    </td>
                </tr>
                <tr class="datoOculto">
                    <td colspan="13">
                        <div class="eosExpedienteDetalleAmpliado">
                            <asp:Label ID="HotelPaxDetalle" runat="server" />
                        </div>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsServiciosAlojamientos" runat="server" SelectMethod="ObtenerServiciosHoteles"
            TypeName="EOS.Web.AgenteExpedientes" OnSelecting="odsServiciosAlojamientos_Selecting">
            <SelectParameters>
                <asp:Parameter DefaultValue="1" Name="idExpediente" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>

    <div class="contentDetalleGeneric contentServicesBox contentTableGeneric" id="divTransporte" runat="server">

        <h2>Transporte</h2>



        <asp:ListView ID="lvServicioTransporte" runat="server" DataSourceID="odsServiciosTransportes"
            OnItemDataBound="lvServicioTransporte_ItemDataBound" OnLayoutCreated="lvServicioTransporte_LayoutCreated"
            OnItemCommand="Listas_ItemCommand" Visible="<%# (entExp.Idtiporeserva == 1) %>">
            <EmptyDataTemplate>
                <table class="eosTablaResultados">
                    <thead>
                        <tr>
                            <th>
                                <%--             <asp:Image ID="Image6" runat="server" ImageUrl="~/Styles/images/ic_modificar.png" />--%>
                            </th>
                            <th>
                                <%--      <asp:Image ID="Image7" runat="server" ImageUrl="~/Styles/images/ic_aceptado.png" />--%>
                            </th>
                            <th>
                                <%--      <asp:Image ID="Image8" runat="server" ImageUrl="~/Styles/images/ic_cancelado.png" />--%>
                            </th>
                            <th>Transporte
                            </th>
                            <th>Fecha
                            </th>
                            <th>Número
                            </th>
                            <th>Origen
                            </th>
                            <th>Destino
                            </th>
                            <th>Salida
                            </th>
                            <th>Llegada
                            </th>
                            <th>Importe Max.
                            </th>
                            <th>Observaciones
                            </th>
                            <th class="dato">Pax
                            </th>
                            <th class="dato">
                                <asp:Image ID="Image9" runat="server" ImageUrl="~/Styles/images/ic_alternativas.png" />
                            </th>
                            <th class="dato">
                                <asp:Image ID="Image10" runat="server" ImageUrl="~/Styles/images/ic_transporte.png" />
                            </th>
                            <th class="cotizado">Importe
                            <!-- Ismael Ameller 03-03-2011 Cambio de label -->
                                <%--Cotizado--%>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="16">
                                <span class="eosTituloRojo">No se han encontrado transportes en este expediente</span>
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
                                <%--                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/images/ic_modificar.png" />--%>
                            </th>
                            <th>
                                <%--<asp:Image ID="Image2" runat="server" ImageUrl="~/Styles/images/ic_aceptado.png" />--%>
                            </th>
                            <th>
                                <%--                  <asp:Image ID="Image3" runat="server" ImageUrl="~/Styles/images/ic_cancelado.png" />--%>
                            </th>
                            <th>Transporte
                            </th>
                            <th>Fecha
                            </th>
                            <th>Número
                            </th>
                            <th>Origen
                            </th>
                            <th>Destino
                            </th>
                            <th>Salida
                            </th>
                            <th>Llegada
                            </th>
                            <th>Importe Max.
                            </th>
                            <th>Observaciones
                            </th>
                            <th class="dato">Pax
                            </th>
                            <th class="dato">
                                <asp:Image ID="Image4" runat="server" ImageUrl="~/Styles/images/ic_alternativas.png" />
                            </th>
                            <th class="dato">
                                <asp:Image ID="Image5" runat="server" ImageUrl="~/Styles/images/ic_transporte.png" />
                            </th>
                            <th class="cotizado">Importe
                            <!-- Ismael Ameller 03-03-2011 Cambio de label -->
                                <%--Cotizado--%>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </tbody>
                    <!--<tfoot>-->
                    <tr class="borderDashedTopTotal">
                        <td colspan="12" class="eosTablaResultadosResumen par"></td>
                        <td class="eosTablaResultadosResumen par">
                            <asp:Label ID="TransportePax" runat="server" />
                            <a title="Mostrar Información del PAX"
                                onclick="javascript:ToggleAdvancedInfo(this);return false;" onmouseout="javascript:window.status='';return true;"
                                onmouseover="javascript:window.status='Mostrar Información del PAX';return true" class="contentMoreInfo">+</a>
                        </td>

                        <td colspan="3" class="eosTablaResultadosResumen par totalStrong">
                            <asp:Label ID="TransporteTotal" runat="server" />
                        </td>
                    </tr>
                    <tr class="datoOculto">
                        <td colspan="16">
                            <div class="eosExpedienteDetalleAmpliado">
                                <asp:Label ID="TransportePaxDetalleTotal" runat="server" />
                            </div>
                        </td>
                    </tr>

                    <tr class="borderDashedTop">
                        <td colspan="16">
                            <a href="#" class="popup addDocumentLink" onclick="ShowPopup(1);">
                                <%--<img src="Styles/images/document_add.png" />--%>Añadir documentos</a>
                        </td>
                    </tr>
                    <!--</tfoot>-->
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td colspan="16" class="ida">Ida
                    </td>
                </tr>
                <tr runat="server" visible="true" id="trIda1">
                    <td colspan="3" class="par"></td>
                    <td>
                        <%# Eval("ida1_transporte")%>
                    </td>
                    <td class="par">
                        <%# String.Format("{0:dd/MM/yyyy}", Eval("ida1_fechasalida"))%>
                    </td>
                    <td>
                        <%# Eval("ida1_numvuelo_tren")%>
                    </td>
                    <td class="par">
                        <%# Eval("ida1_origen")%>
                    </td>
                    <td>
                        <%# Eval("ida1_destino")%>
                    </td>
                    <td class="par">
                        <%# Eval("ida1_horasalida")%>
                    </td>
                    <td>
                        <%# Eval("ida1_horallegada")%>
                    </td>
                    <td class="par">
                        <asp:Label ID="lblImporte1" runat="server" Text='<%# Eval("importe_max")!=null ? ((double)Eval("importe_max")).ToString("N2") + " €" : "0,00 €" %> '
                            Width="60px"></asp:Label>
                    </td>
                    <td>
                        <%# Eval("observaciones_ida")%>
                    </td>
                    <td class="par"></td>
                    <td></td>
                    <td class="par" colspan="2"></td>
                </tr>
                <tr runat="server" visible="true" id="trIda2">
                    <td colspan="3" class="par"></td>
                    <td>
                        <%# Eval("ida2_transporte")%>
                    </td>
                    <td class="par">
                        <%# String.Format("{0:dd/MM/yyyy}", Eval("ida2_fechasalida"))%>
                    </td>
                    <td>
                        <%# Eval("ida2_numvuelo_tren")%>
                    </td>
                    <td class="par">
                        <%# Eval("ida2_origen")%>
                    </td>
                    <td>
                        <%# Eval("ida2_destino")%>
                    </td>
                    <td class="par">
                        <%# Eval("ida2_horasalida")%>
                    </td>
                    <td>
                        <%# Eval("ida2_horallegada")%>
                    </td>
                    <td class="par"></td>
                    <td></td>
                    <td class="par"></td>
                    <td></td>
                    <td class="par" colspan="2"></td>
                </tr>
                <tr>
                    <td colspan="16" class="ida">Vuelta
                    </td>
                </tr>
                <tr runat="server" visible="true" id="trReg1">
                    <td colspan="3" class="par"></td>
                    <td>
                        <%# Eval("reg1_transporte")%>
                    </td>
                    <td class="par">
                        <%# String.Format("{0:dd/MM/yyyy}", Eval("reg1_fechasalida"))%>
                    </td>
                    <td>
                        <%# Eval("reg1_numvuelo_tren")%>
                    </td>
                    <td class="par">
                        <%# Eval("reg1_origen")%>
                    </td>
                    <td>
                        <%# Eval("reg1_destino")%>
                    </td>
                    <td class="par">
                        <%# Eval("reg1_horasalida")%>
                    </td>
                    <td>
                        <%# Eval("reg1_horallegada")%>
                    </td>
                    <td class="par">
                        <asp:Label ID="lblImporteMax2" runat="server" Text='<%# Eval("importe_max")!=null ? ((double)Eval("importe_max")).ToString("N2") + " €" : "0,00 €" %> '
                            Width="60px" />
                    </td>
                    <td>
                        <%# Eval("observaciones_reg")%>
                    </td>
                    <td class="par"></td>
                    <td></td>
                    <td class="par" colspan="2"></td>
                </tr>
                <tr runat="server" visible="true" id="trReg2">
                    <td colspan="3" class="par"></td>
                    <td>
                        <%# Eval("reg2_transporte")%>
                    </td>
                    <td class="par">
                        <%# String.Format("{0:dd/MM/yyyy}", Eval("reg2_fechasalida"))%>
                    </td>
                    <td>
                        <%# Eval("reg2_numvuelo_tren")%>
                    </td>
                    <td class="par">
                        <%# Eval("reg2_origen")%>
                    </td>
                    <td>
                        <%# Eval("reg2_destino")%>
                    </td>
                    <td class="par">
                        <%# Eval("reg2_horasalida")%>
                    </td>
                    <td>
                        <%# Eval("reg2_horallegada")%>
                    </td>
                    <td class="par"></td>
                    <td></td>
                    <td class="par"></td>
                    <td></td>
                    <td class="par" colspan="2"></td>
                </tr>
                <tr class="par">
                    <td>
                        <asp:ImageButton ID="imgServicioMod" runat="server" ToolTip='<%# String.Format("Modificar el servicio {0}", Eval("observaciones_ida")) %>'
                            ImageUrl="~/Styles/images/ic_modificar.png" CommandName="MOD2" CommandArgument='<%# Eval("idserviciotransporte") %>' />
                    </td>
                    <td>
                        <asp:ImageButton ID="imgServicioAprobar" runat="server" ToolTip='<%# String.Format("Confirmar el servicio {0}", Eval("observaciones_ida")) %>'
                            ImageUrl="~/Styles/images/ic_aceptado.png" CommandName="ACE" CommandArgument='<%# Eval("idreserva") %>'
                            OnClientClick='<%# String.Format("return ConfirmaModificacionB({0}, {1}, {2}, {3}, {4});", Eval("idexpediente"), Eval("idreserva"), Eval("idservicio"), 0, Eval("idserviciotransporte")) %>' OnClick="AprobarMail_Click" />
                        <asp:Image ID="imgServicioAprobado" runat="server" ImageUrl="~/Styles/images/ic_estado_aceptado.png"
                            Visible="false" />
                    </td>
                    <td>
                        <asp:ImageButton ID="imgServicioCancelar" runat="server" ToolTip='<%# String.Format("Rechazar el servicio {0}", Eval("observaciones_ida")) %>'
                            ImageUrl="~/Styles/images/ic_cancelado.png" CommandName="CAN" CommandArgument='<%# Eval("idreserva") %>' />
                        <asp:Image ID="imgServicioCancelado" runat="server" ImageUrl="~/Styles/images/ic_estado_cancelado.png"
                            Visible="false" />
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td align="center">
                        <asp:Label ID="importemaxLabel" runat="server" Text='' />
                    </td>
                    <td class="borderLeft">
                        <asp:Label ID="paxLabel" runat="server" Text='<%# String.Format("{0}",Eval("pax")) %>' />
                        <a title="Mostrar Información del PAX" onclick="javascript:ToggleAdvancedInfo(this);return false;"
                            onmouseout="javascript:window.status='';return true;" onmouseover="javascript:window.status='Mostrar Información del PAX';return true"
                            class="contentMoreInfo <%# Eval("pax")!=null ? "" : "oculta" %>">+</a>
                    </td>
                    <td>
                        <asp:ImageButton ID="imgServicioAlternativas" runat="server" Visible='<%# GetEstadoShow(Eval("idestado").ToString())%>'
                            ToolTip='<%# String.Format("Mostrar Alternativas del Servicio {0}", Eval("observaciones_ida")) %>'
                            ImageUrl="~/Styles/images/ic_alternativas.png" OnClientClick='<%# String.Format("Pruebas.aspx?idexp={0}", Eval("idexpediente")) %>' />
                    </td>
                    <td align="center">
                        <span title='<%# GetEstadoTooltip(Eval("idestado").ToString())%>' class="eosImagenCotizacion <%# String.Format("eosImagenLeyenda{0}", Eval("idestado"))%>"></span>
                    </td>
                    <td align="right">
                        <asp:Label ID="cotizadoLabel" runat="server" Text='<%# Eval("cotizado")!=null ? ((float)Eval("cotizado")).ToString("N2") + " €" : "0,00 €" %>' />
                    </td>
                </tr>
                <tr class="datoOculto">
                    <td colspan="16">
                        <div class="eosExpedienteDetalleAmpliado">
                            <asp:Label ID="TransportePaxDetalle" runat="server" />
                        </div>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsServiciosTransportes" runat="server" SelectMethod="ObtenerServiciosTransportes"
            TypeName="EOS.Web.AgenteExpedientes" OnSelecting="odsServiciosTransportes_Selecting">
            <SelectParameters>
                <asp:Parameter DefaultValue="1" Name="idExpediente" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>

    </div>

    <div class="contentDetalleGeneric contentServicesBox contentTableGeneric" id="divOtrosServicios" runat="server">

        <h2>Otros Servicios</h2>


        <asp:ListView ID="lvServicioActividades" runat="server" DataSourceID="odsOtrosServicios"
            OnItemDataBound="lvServicioActividades_ItemDataBound" OnLayoutCreated="lvServicioActividades_LayoutCreated"
            OnItemCommand="Listas_ItemCommand">
            <EmptyDataTemplate>
                <table class="eosTablaResultados">
                    <thead>
                        <tr>
                            <th>
                                <%--  <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/images/ic_modificar.png" />--%>
                            </th>
                            <th>
                                <%--  <asp:Image ID="Image2" runat="server" ImageUrl="~/Styles/images/ic_aceptado.png" />--%>
                            </th>
                            <th>
                                <%--       <asp:Image ID="Image3" runat="server" ImageUrl="~/Styles/images/ic_cancelado.png" />--%>
                            </th>
                            <th>Servicio
                            </th>
                            <th>Tipo
                            </th>
                            <th>Inicio
                            </th>
                            <th>Final
                            </th>
                            <th>Sede
                            </th>
                            <th>Importe Max.
                            </th>
                            <th width="30%">Observaciones
                            </th>
                            <th class="dato">Pax
                            </th>
                            <th class="dato">
                                <asp:Image ID="Image4" runat="server" ImageUrl="~/Styles/images/ic_alternativas.png" />
                            </th>
                            <th class="dato">
                                <asp:Image ID="Image5" runat="server" ImageUrl="~/Styles/images/ic_servicios.png" />
                            </th>
                            <th class="cotizado">Importe
                            <!-- Ismael Ameller 03-03-2011 Cambio de label -->
                                <%--Cotizado--%>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="14">
                                <span class="eosTituloRojo">No se han encontrado servicios en este expediente</span>
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
                                <%-- <asp:Image ID="Image1" runat="server" ImageUrl="~/Styles/images/ic_modificar.png" />--%>
                            </th>
                            <th>
                                <%--    <asp:Image ID="Image2" runat="server" ImageUrl="~/Styles/images/ic_aceptado.png" />--%>
                            </th>
                            <th>
                                <%-- <asp:Image ID="Image3" runat="server" ImageUrl="~/Styles/images/ic_cancelado.png" />--%>
                            </th>
                            <th>Servicio
                            </th>
                            <th>Tipo
                            </th>
                            <th>Inicio
                            </th>
                            <th>Final
                            </th>
                            <th>Sede
                            </th>
                            <th>Importe Max.
                            </th>
                            <th width="30%">Observaciones
                            </th>
                            <th class="dato">Pax
                            </th>
                            <th class="dato">
                                <asp:Image ID="Image4" runat="server" ImageUrl="~/Styles/images/ic_alternativas.png" />
                            </th>
                            <th class="dato">
                                <asp:Image ID="Image5" runat="server" ImageUrl="~/Styles/images/ic_servicios.png" />
                            </th>
                            <th class="cotizado">Importe
                            <!-- Ismael Ameller 03-03-2011 Cambio de label -->
                                <%--Cotizado--%>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                    </tbody>
                    <!--<tfoot>-->
                    <tr class="borderDashedTopTotal">
                        <td colspan="10" class="eosTablaResultadosResumen par"></td>
                        <td class="eosTablaResultadosResumen par">
                            <asp:Label ID="ActividadPax" runat="server" />
                            <a id="ActividadPaxPlus"
                                runat="server" title="Mostrar Información del PAX" onclick="javascript:ToggleAdvancedInfo(this);return false;"
                                onmouseout="javascript:window.status='';return true;" onmouseover="javascript:window.status='Mostrar Información del PAX';return true" class="contentMoreInfo">+ </a>
                        </td>

                        <td colspan="3" class="eosTablaResultadosResumen par totalStrong">
                            <asp:Label ID="ActividadTotal" runat="server" />
                        </td>
                    </tr>
                    <tr class="datoOculto">
                        <td>
                            <div class="eosExpedienteDetalleAmpliado">
                                <asp:Label ID="ActividadPaxDetalleTotal" runat="server" />
                            </div>
                        </td>
                    </tr>


                    <tr class="borderDashedTop">
                        <td colspan="14"><a href="#" class="popup addDocumentLink" onclick="ShowPopup(5);">
                            <%--<img src="Styles/images/document_add.png" />--%>Añadir documentos</a></td>
                    </tr>
                    <!--</tfoot>-->
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td class="par">
                        <asp:ImageButton ID="imgServicioMod" runat="server" ToolTip='<%# String.Format("Modificar el servicio {0}", Eval("descripcion")) %>'
                            ImageUrl="~/Styles/images/ic_modificar.png" CommandName="MOD3" CommandArgument='<%# Eval("idreserva") %>' />
                    </td>
                    <td class="par">
                        <asp:ImageButton ID="imgServicioAprobar" runat="server" ToolTip='<%# String.Format("Confirmar el servicio {0}", Eval("descripcion")) %>'
                            ImageUrl="~/Styles/images/ic_aceptado.png" CommandName="ACE" CommandArgument='<%# Eval("idreserva") %>'
                            OnClientClick='<%# String.Format("return ConfirmaModificacionB();") %>' OnClick="AprobarMail_Click" />
                        <asp:Image ID="imgServicioAprobado" runat="server" ImageUrl="~/Styles/images/ic_estado_aceptado.png"
                            Visible="false" />
                    </td>
                    <td class="par">
                        <asp:ImageButton ID="imgServicioCancelar" runat="server" ToolTip='<%# String.Format("Rechazar el servicio {0}", Eval("descripcion")) %>'
                            ImageUrl="~/Styles/images/ic_cancelado.png" CommandName="CAN" CommandArgument='<%# Eval("idreserva") %>' />
                        <asp:Image ID="imgServicioCancelado" runat="server" ImageUrl="~/Styles/images/ic_estado_cancelado.png"
                            Visible="false" />
                    </td>
                    <td>
                        <asp:Label ID="DescripcionLabel" runat="server" Text='<%# Eval("descripcion") %>' />
                    </td>
                    <td class="par">
                        <asp:Label ID="tipoLabel" runat="server" Text='<%# Eval("tipo") %>' />
                    </td>
                    <td>
                        <asp:Label ID="fechainicioLabel" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}",Eval("fechainicio")) %>' />
                    </td>
                    <td class="par">
                        <asp:Label ID="fechafinLabel" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}",Eval("fechafin")) %>' />
                    </td>
                    <td>
                        <asp:Label ID="sedeLabel" runat="server" Text='<%# Eval("sede") %>' />
                    </td>
                    <td class="par">
                        <asp:Label ID="importemaxLabel" runat="server" Text='<%# Eval("pvp")!=null ? ((double)Eval("pvp")).ToString("N2") + " €" : "0,00 €" %>'
                            Width="70px" />
                    </td>
                    <td>
                        <asp:Label ID="observacionesLabel" runat="server" Text='<%# Eval("observaciones") %>' />
                    </td>
                    <td class="borderLeft">
                        <asp:Label ID="paxLabel" runat="server" Text='<%# String.Format("{0}",Eval("pax")) %>' />
                        <a style="" title="Mostrar Información del PAX" onclick="javascript:ToggleAdvancedInfo(this);return false;"
                            onmouseout="javascript:window.status='';return true;" onmouseover="javascript:window.status='Mostrar Información del PAX';return true"
                            class=" contentMoreInfo <%# Eval("pax")!=null ? "" : "oculta" %>">+</a>
                    </td>
                    <td class="par">
                        <asp:ImageButton ID="imgServicioAlternativas" runat="server" Visible='<%# GetEstadoShow(Eval("idestado").ToString())%>'
                            ToolTip='<%# String.Format("Mostrar Alternativas del Servicio {0}", Eval("observaciones")) %>'
                            ImageUrl="~/Styles/images/ic_alternativas.png" />
                    </td>
                    <td align="center">
                        <span title='<%# GetEstadoTooltip(Eval("idestado").ToString())%>' class="eosImagenCotizacion <%# String.Format("eosImagenLeyenda{0}", Eval("idestado"))%>"></span>
                    </td>
                    <td align="right" class="par">
                        <asp:Label ID="cotizadoLabel" runat="server" Text='<%# Eval("cotizado")!=null ? ((float)Eval("cotizado")).ToString("N2") + " €" : "0,00 €" %>' />
                    </td>
                </tr>
                <tr class="datoOculto">
                    <td colspan="14">
                        <div class="eosExpedienteDetalleAmpliado">
                            <asp:Label ID="ActividadPaxDetalle" runat="server" />
                        </div>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="odsOtrosServicios" runat="server" SelectMethod="ObtenerOtrosServicios"
            TypeName="EOS.Web.AgenteExpedientes" OnSelecting="odsOtrosServicios_Selecting">
            <SelectParameters>
                <asp:Parameter DefaultValue="1" Name="idExpediente" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>

    </div>

    <asp:PlaceHolder ID="plhParticipantesNoCalculadora" runat="server">
        <div class="contentDetalleGeneric  contentServicesBoxResumen contentTableGeneric">

            <asp:ListView ID="lvResumenParticipantes" runat="server" DataSourceID="odsResumenParticipantes"
                OnSorting="lvResumenParticipantes_Sorting" OnItemDataBound="lvResumenParticipantes_ItemDataBound"
                Visible="<%# (entExp.Idtiporeserva != 2) %>">
                <EmptyDataTemplate>
                    <table class="eosTablaResultados">
                        <thead>
                            <tr>
                                <th width="23%">Nombre
                                </th>
                                <th colspan="2" width="23%">Apellidos
                                </th>
                                <th>
                                    <asp:Image ID="Image11" runat="server" ImageUrl="~/Styles/images/ic_inscripciones.png" />
                                </th>
                                <th>
                                    <asp:Image ID="Image12" runat="server" ImageUrl="~/Styles/images/ic_transporte.png" />
                                </th>
                                <th>
                                    <asp:Image ID="Image13" runat="server" ImageUrl="~/Styles/images/ic_alojamiento.png" />
                                </th>
                                <th width="10%">
                                    <asp:Image ID="Image14" runat="server" ImageUrl="~/Styles/images/ic_servicios.png" />
                                </th>
                                <th width="12%">Total
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="8">
                                    <span class="eosTituloRojo">No se han encontrado participantes en este expediente</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <table class="eosTablaResultados">
                        <thead>
                            <tr>
                                <th width="20%">
                                    <asp:LinkButton ID="lbNombre" runat="server" Text="Nombre" CommandName="Sort" CommandArgument="Nombre"></asp:LinkButton>
                                </th>
                                <th colspan="2" width="20%">
                                    <asp:LinkButton ID="lbApel1" runat="server" Text="Apellidos" CommandName="Sort" CommandArgument="Apel1"></asp:LinkButton>
                                </th>
                                <th width="8%">
                                    <asp:Image ID="Image11" runat="server" ImageUrl="~/Styles/images/ic_inscripciones.png" />
                                </th>
                                <th width="8%">
                                    <asp:Image ID="Image12" runat="server" ImageUrl="~/Styles/images/ic_transporte.png" />
                                </th>
                                <th width="8%">
                                    <asp:Image ID="Image13" runat="server" ImageUrl="~/Styles/images/ic_alojamiento.png" />
                                </th>
                                <th width="10%">
                                    <asp:Image ID="Image14" runat="server" ImageUrl="~/Styles/images/ic_servicios.png" />
                                </th>
                                <th width="12%">
                                    <asp:LinkButton ID="lbTotal" runat="server" Text="Total" CommandName="Sort" CommandArgument="Total"></asp:LinkButton>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                        </tbody>
                        <tfoot>
                            <tr class="borderDashedTopTotal">
                                <td colspan="3" class="eosTablaResultadosResumen par"></td>
                                <td class="par">
                                    <asp:Label ID="TotalIns" runat="server" />
                                </td>
                                <td class="par">
                                    <asp:Label ID="TotalDsp" runat="server" />
                                </td>
                                <td class="par">
                                    <asp:Label ID="TotalHot" runat="server" />
                                </td>
                                <td class="par">
                                    <asp:Label ID="TotalAct" runat="server" />
                                </td>
                                <td align="right" class="par">
                                    <asp:Label ID="TotalRes" runat="server" />
                                </td>
                            </tr>
                            <tr class="borderDashedTopTotal">
                                <td colspan="3" class="eosTablaResultadosResumen par"></td>
                                <td class="par"></td>
                                <td class="par"></td>
                                <td class="par"></td>
                                <td class="par">
                                    <asp:Label ID="lblFee" runat="server" />
                                </td>
                                <td align="right" class="par">
                                    <asp:Label ID="lblTotalFee" runat="server" />
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Label ID="nombreLabel" runat="server" Text='<%# Eval("Nombre") %>' />
                        </td>
                        <td colspan="2" class="par">
                            <asp:Label ID="apel1Label" runat="server" Text='<%# Eval("Apel1") %>' />
                        </td>
                        <td style="text-align: center" class="par">
                            <asp:Label ID="importeinsLabel" runat="server" Text='<%# ((double)Eval("importeins")).ToString("N2") + " €" %>' />
                        </td>
                        <td class="par" style="text-align: center">
                            <asp:Label ID="importedspLabel" runat="server" Text='<%# ((double)Eval("importedsp")).ToString("N2") + " €" %>' />
                        </td>
                        <td class="par" style="text-align: center">
                            <asp:Label ID="importehotLabel" runat="server" Text='<%# ((double)Eval("importehot")).ToString("N2") + " €" %>' />
                        </td>
                        <td class="par" style="text-align: center">
                            <asp:Label ID="importeactLabel" runat="server" Text='<%# ((double)Eval("importeact")).ToString("N2") + " €" %>' />
                        </td>
                        <td align="right" class="par">
                            <asp:Label ID="cotizadoLabel" runat="server" Text='<%# ((double)Eval("total")).ToString("N2") + " €" %>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
            <asp:ObjectDataSource ID="odsResumenParticipantes" runat="server" SelectMethod="ObtenerResumenParticipantes"
                TypeName="EOS.Web.AgenteExpedientes" SelectCountMethod="ObtenerNumeroResumenParticipantes"
                EnablePaging="True" OnSelecting="odsParticipantes_Selecting" SortParameterName="sortParameter">
                <SelectParameters>
                    <asp:Parameter DefaultValue="1" Name="filtroIDExp" Type="Int32" />
                    <asp:Parameter DefaultValue="exp.IdPassengerList" Name="sortParameter" Type="String" />
                    <asp:Parameter DefaultValue="exp.IdPassengerList" Name="groupParameter" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
    </asp:PlaceHolder>
    <div class="leyenda-tabla">
        <span class="eosImagenLeyendaX eosImagenLeyendaSer1">Alojamientos</span>
        <span class="eosImagenLeyendaX eosImagenLeyendaSer2">Transporte</span>
        <span class="eosImagenLeyendaX eosImagenLeyendaSer3">Inscripciones</span>
        <span class="eosImagenLeyendaX eosImagenLeyendaSer4">Servicios</span>
    </div>
    <div class="leyenda-tabla-estado">
        <span class="eosImagenLeyendaY eosImagenLeyenda1">Modificar</span>
        <span class="eosImagenLeyendaY eosImagenLeyenda2">Eliminar</span>
        <span class="eosImagenLeyendaX eosImagenLeyendaAB">Borrador</span>
        <span class="eosImagenLeyendaX eosImagenLeyendaCR">Enviado</span>
        <span class="eosImagenLeyendaX eosImagenLeyendaCTZD">Cotizando</span>
        <span class="eosImagenLeyendaX eosImagenLeyendaCTZ">Presupuestado</span>
        <span class="eosImagenLeyendaX eosImagenLeyendaCFP">Aceptado</span>
        <span class="eosImagenLeyendaX eosImagenLeyendaPTR">Tramitando</span>
        <span class="eosImagenLeyendaX eosImagenLeyendaTR">Finalizado</span>
        <span class="eosImagenLeyendaX eosImagenLeyendaCN">Cancelado</span>
        <span class="eosImagenLeyendaX eosImagenLeyenda10">Alternativas</span>
    </div>
    <asp:PlaceHolder ID="plhParticipantesCalculadora" runat="server">
        <div class="contentTableGeneric">
            <br /><br />
            <asp:ListView ID="lvServicioParticipantes" runat="server" DataSourceID="odsParticipantesExpediente"
                DataKeyNames="IdPassengerlist" EnablePersistedSelection="True" Visible="true"
                OnSelectedIndexChanged="lvServicioParticipantes_SelectedIndexChanged">
                <EmptyDataTemplate>
                    <table class="eosTablaResultados">
                        <thead>
                            <tr>
                                <th>Nombre Completo
                                </th>
                                <th>Tipo Asistente
                                </th>
                                <th width="20%">Tipo Actividad Pax
                                </th>
                                <th>Nivel de riesgo HCP en APPIAN
                                </th>
                                <th width="10%">
                                    <%= ConfigurationManager.AppSettings["codigoInvitado"] %>
                                </th>
                                <th width="20%">Honorarios
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="6">
                                    <span class="eosTituloRojo">&nbsp;</span>
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
                                    <asp:LinkButton ID="lbNombre" runat="server" Text="Nombre Completo" CommandName="Sort"
                                        CommandArgument="NOMBRE"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="LinkButton1" runat="server" Text="Tipo Asistente" CommandName="Sort"
                                        CommandArgument="TIPOASISTENTE"></asp:LinkButton>
                                </th>
                                <th width="30%">
                                    <asp:LinkButton ID="lbHospital" runat="server" Text="Tipo Actividad" CommandName="Sort"
                                        CommandArgument="TIPOACTIVIDADPAX"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lbNivelRiesgo" runat="server" Text="Nivel de Riesgo" CommandName="Sort"
                                        CommandArgument="NIVELRIESGO"></asp:LinkButton>
                                </th>
                                <th width="20%">
                                    <asp:LinkButton ID="lbMSDID" runat="server" CommandName="Sort" CommandArgument="MSDID"><%= ConfigurationManager.AppSettings["codigoInvitado"]%></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lbHonorarios" runat="server" Text="Honorarios" CommandName="Sort"
                                        CommandArgument="HONORARIOS"></asp:LinkButton>
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
                        <td class="par">
                            <asp:Label ID="nombreLabel" runat="server" Text='<%#  Eval("nombre")+ " " + Eval("apel1") %>' />
                        </td>
                        <td>
                            <asp:Label ID="tipoasistenteLabel" runat="server" Text='<%# Eval("tipoasistente") %>' />
                        </td>
                        <td class="par">
                            <asp:Label ID="tipoactividadpaxLabel" runat="server" Text='<%# Eval("tipoactividadpax") %>' />
                        </td>
                        <td>
                            <asp:Label ID="nivelriesgolabel" runat="server" Text='<%# Eval("nivelriesgo") %>' />
                        </td>
                        <td class="par">
                            <asp:Label ID="msdidLabel" runat="server" Text='<%# Eval("msdid") %>' />
                        </td>
                        <td>
                            <asp:Label ID="honorariosLabel" runat="server" Text='<%# Eval("honorarios") %>' />&nbsp;€
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
    </asp:PlaceHolder>

</asp:Content>
<asp:Content ID="ModalDocument" ContentPlaceHolderID="ModalDocumentId" runat="server">
    <!-- Modal -->
    <div id="modal" style="display: none">
        <div>
            <table id="BucketTable">
                <tr class="BucketTableHead">
                    <td>Fichero</td>
                    <td>Tipo</td>
                    <% if (IsExpIndividual)
                        {%><td>SubTipo</td>
                    <% } %>
                    <td style="min-width: 187px;">Nombre Adicional</td>
                    <td>Asistente</td>
                    <td style="min-width: 127px;">Nueva Versión</td>
                    <td>Documento</td>
                    <td></td>
                </tr>
                <tbody>
                </tbody>
            </table>
            <br />
            <table id="UploadTable">
                <tr class="UploadTableHead">
                    <td>Fichero</td>
                    <td>Tipo</td>
                    <% if (IsExpIndividual)
                        {%><td>SubTipo</td>
                    <% } %>
                    <td>Nombre Adicional</td>
                    <td>Asistente</td>
                    <td>Nueva Versión</td>
                    <td>Documento</td>
                    <%--<td></td>--%>
                </tr>
                <tr>
                    <td>
                        <label for="upload" id="uploadName" class="btn">Examinar</label><input type="file" style="display: none;" class="upload" id="upload" onchange="InputFileChange(this)" /></td>
                    <td>
                        <select id="ddlTipo" onchange="ChangeSubTypes()"></select></td>
                    <% if (IsExpIndividual)
                        {%><td>
                            <select id="ddlSubTipo" onchange="EnableAditionalText()"></select></td>
                    <% } %>
                    <td>
                        <input id="NombreAdicional" type="text" disabled="disabled" /></td>
                    <td>
                        <asp:DropDownList ID="ddlAsistente" runat="server" /></td>
                    <td>
                        <input type="checkbox" id="DocVersion" onchange="ShowDocumentddl(this)" /></td>
                    <td style="display: none">
                        <select id="ddlNombreFichero"></select></td>
                </tr>
            </table>
        </div>
        <br />
        <div id="ModalButtons">
            <a class="addDocument" onclick="AddTableElement()">Añadir documento</a>
            <a class="sendDocument" onclick="SendDataFile()">Enviar documentos</a>
        </div>
    </div>
    <!-- Modal -->
</asp:Content>
<asp:Content ID="cphScripts" ContentPlaceHolderID="ContentPlaceHolderScripts" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("ul#eosHPBotones li").removeClass("active");
            var tipoExpediente = "<%=entExp.Idtiporeserva%>";
            var tipoPago = "<%=entExp.TipoPagoFee%>";
            if (tipoExpediente == "1") {
                if (tipoPago == "3") {
                    $("ul#eosHPBotones li#liCalculadoraHonorarios").addClass("active");
                }
                else {
                    $("ul#eosHPBotones li#liCreacionExpedienteIndividual").addClass("active");
                }
            }
            else if (tipoExpediente == "2") {
                $("ul#eosHPBotones li#liCreacionExpedienteColectivo").addClass("active");
            }
        });
    </script>
</asp:Content>
