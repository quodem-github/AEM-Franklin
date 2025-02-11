<%@ Page Title="" Language="C#" MasterPageFile="~/Styles/EOS.Master" AutoEventWireup="true" CodeBehind="NuevaActividad.aspx.cs" Inherits="EOS.NuevaActividad" Culture="es-ES" UICulture="es" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>



<asp:Content ID="Content3" ContentPlaceHolderID="eosContentResults" runat="server">
    <act:toolkitscriptmanager id="ToolkitScriptManager1" runat="server" enablescriptglobalization="true">
    </act:toolkitscriptmanager>


    <h1>Solicitud de alta de actividad</h1>
    <h2>Datos de la actividad</h2>






    <div id="eosContentFilter" class="contentGenericBox">
        <div id="eosContentAMECErrores" style="float: left;">
            <asp:ValidationSummary ID="ContenActividadValidationSummary" runat="server"
                CssClass="failureNotification" ValidationGroup="ActividadValidationGroup"
                HeaderText="<strong>ATENCIÓN. Los siguientes campos son obligatorios:</strong>" />
            <span class="failureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>
        </div>


        <div id="eosTablaActividad" class="eosTablaFiltros">

            <div class="contentFormGeneric">
                <div class="contentForm  eosTresColumna">
                    <label class="eosCampoLabelSin" for="txtCiudad">Inicio<span class="eosCampoObligatorio">*</span></label>
                    <eos:inputdatepickercontrol id="txtFechaInicio" runat="server" cssclass="eosDisabledInputVacio style1 iconCalendar" />

                    <asp:RequiredFieldValidator CssClass="" ID="RequiredFieldValidatorFechaInicio" runat="server" ErrorMessage="Inicio" ControlToValidate="txtFechaInicio" ValidationGroup="ActividadValidationGroup" SetFocusOnError="True" ToolTip="Campo Obligatorio"></asp:RequiredFieldValidator>
                    <asp:CompareValidator CssClass="" ID="CompareValidatorFechaInicio" runat="server" ErrorMessage="La fecha de Inicio debe tener un formato válido" Type="Date"
                        ControlToValidate="txtFechaInicio" Operator="DataTypeCheck" ValidationGroup="ActividadValidationGroup"></asp:CompareValidator>
                    <asp:RangeValidator CssClass="" ID="RangeValidatorFechaInicio" runat="server" ErrorMessage="La fecha de Inicio no puede ser inferior a hoy" MaximumValue="31/12/2099"
                        ControlToValidate="txtFechaInicio" ValidationGroup="ActividadValidationGroup" Display="Dynamic" Type="Date"></asp:RangeValidator>


                </div>


                <div class="contentForm  eosTresColumna">
                    <label class="eosCampoLabelSin" for="txtCiudad">Finalización<span class="eosCampoObligatorio">*</span></label>
                    <eos:inputdatepickercontrol id="txtFechaFin" runat="server" cssclass="eosDisabledInputVacio style1 iconCalendar" />

                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorFechaFin" runat="server" ErrorMessage="Finalización" ControlToValidate="txtFechaFin" ValidationGroup="ActividadValidationGroup" SetFocusOnError="True" ToolTip="Campo Obligatorio"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidatorFechaFin" runat="server" ErrorMessage="La fecha de Finalización debe tener un formato válido" Type="Date"
                        ControlToValidate="txtFechaFin" Operator="DataTypeCheck" ValidationGroup="ActividadValidationGroup" Display="Dynamic"></asp:CompareValidator>
                    <asp:CompareValidator ID="CompareValidatorFechaFin2" runat="server" ErrorMessage="La fecha de Finalización debe tener una fecha superior a la fecha de Inicio" Type="Date"
                        ControlToValidate="txtFechaFin" ControlToCompare="txtFechaInicio" Operator="GreaterThanEqual" ValidationGroup="ActividadValidationGroup" Display="Dynamic" CultureInvariantValues="true"></asp:CompareValidator>
                    <asp:RangeValidator ID="RangeValidatorFechaFin" runat="server" ErrorMessage="La fecha de Finalización no puede ser inferior a hoy" MinimumValue="27/08/2011" MaximumValue="31/12/2099"
                        ControlToValidate="txtFechaFin" ValidationGroup="ActividadValidationGroup" Display="Dynamic" Type="Date"></asp:RangeValidator>
                </div>

            </div>


            <div class="contentFormGeneric" style="display: none">
                <div class="contentForm  eosMitadColumna">
                    <label class="eosCampoLabelSin" for="txtNombre">Email secretaría</label>
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW350"></asp:TextBox>
                </div>
                <div class="contentForm  eosMitadColumna">
                    <label class="eosCampoLabelSin" for="txtNombre">Url Web</label>
                    <asp:TextBox ID="txtUrl" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW350"></asp:TextBox>
                </div>
            </div>







            <div class="contentFormGeneric">


                <div class="contentForm  eosCuartoColumna">
                    <label class="eosCampoLabelSin" for="txtPais">País<span class="eosCampoObligatorio">*</span></label>
                    <asp:DropDownList
                        ID="ddlPais" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW140"
                        DataTextField="Pais" DataValueField="IDPais">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="País" ControlToValidate="ddlPais" ValidationGroup="ActividadValidationGroup" SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>

                </div>


                <div class="contentForm  eosCuartoColumna contentFormRadio">

                    <asp:CheckBox ID="chkInternacional" Text="Internacional" runat="server" CssClass="radioCheck" />

                </div>


                <div class="contentForm  eosMitadColumna boxCampoWithBtn">
                    <label class="eosCampoLabelSin" for="txtPoblacion">Población<span class="eosCampoObligatorio">*</span></label>
                    <input name="txtBusquedaPoblacion" type="text" maxlength="80" id="eosContentResults_txtBusquedaPoblacion" class="eosDisabledInputVacio eosInputVacio eosWidth50" <%--readonly="readonly"--%> />
                    <input type="button" class="buscarPoblacion" id="buscarPoblacion" value="Buscar" />
                    <asp:HiddenField ID="selectedIdPoblacion" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="selectedPoblacion" runat="server" ClientIDMode="Static" />
                </div>


                <div class="TDresultados">
                    <div id="poblacionesFound">
                    </div>
                </div>
            </div>




            <div class="contentFormGeneric">

                <div class="contentForm  eosTresColumna">
                    <label class="eosCampoLabelSin" for="txtCiudad">Especialidad<span class="eosCampoObligatorio">*</span></label>
                    <asp:DropDownList
                        ID="ddlEspecialidad" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW140"
                        DataTextField="Especialidad" DataValueField="IdEspecialidad">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Especialidad" ControlToValidate="ddlEspecialidad" ValidationGroup="ActividadValidationGroup" SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>

                </div>
                <div class="contentForm  eosTresColumna">
                    <label class="eosCampoLabelSin" for="txtCiudad">Sede</label>
                    <asp:TextBox ID="txtSede" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW140"></asp:TextBox>
                </div>
                <div class="contentForm  eosTresColumna">
                    <label class="eosCampoLabelSin" for="txtNombre">Fecha Creación</label>
                    <asp:TextBox ID="txtFechaCreacion" ReadOnly="true" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW100"></asp:TextBox>
                </div>

            </div>





            <div class="contentFormGeneric">

                <div class="contentForm  eosTresColumna">
                    <label class="eosCampoLabelSin" for="txtCiudad">Nombre<span class="eosCampoObligatorio">*</span></label>
                    <asp:TextBox ID="txtNombre" runat="server" MaxLength="70" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW300"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Nombre" ControlToValidate="txtNombre" ValidationGroup="ActividadValidationGroup" SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>

                </div>
                <div class="contentForm  eosTresColumna">
                    <label class="eosCampoLabelSin" for="txtNombre">Creado Por</label>
                    <asp:TextBox ID="txtCreadoPor" ReadOnly="true" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW350"></asp:TextBox>

                </div>
                <div class="contentForm  eosTresColumna">
                    <label class="eosCampoLabelSin" for="txtCiudad">Tipo evento<span class="eosCampoObligatorio">*</span></label>
                    <asp:DropDownList
                        ID="ddlTipoActividad" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW220"
                        DataTextField="Nombre" DataValueField="IdTipoActividad">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Tipo evento" ControlToValidate="ddlTipoActividad" ValidationGroup="ActividadValidationGroup" SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>

                </div>

            </div>



        </div>







    </div>


    <asp:HiddenField runat="server" ID="hdnIdConfEmpresa" ClientIDMode="Static" />
    <div class="eosBotonera eosBotonFiltrado">

        <asp:Button ID="btnGuardar" runat="server"
            ValidationGroup="ActividadValidationGroup"
            OnClick="btnGuardar_Click" Text="Guardar" />
        <!--<asp:Button ID="btnCancelar" runat="server"
            Text="Volver" OnClientClick="javascript:history.back();return false;" />-->
    </div>


    <script type="text/javascript">
        function selectPoblacion(id, idselector) {
            $("#selectedIdPoblacion").val(id);//lo que se recoge al hacer el submit
            $("#selectedPoblacion").val($('#' + idselector).text());//
            $("#eosContentResults_txtBusquedaPoblacion").val($('#' + idselector).text());//campo de búsqueda
            $("#poblacionesFound>div").removeClass("selected");
            $('#' + idselector).addClass("selected");
            $("#poblacionesFound").empty();
        }
        $(document).ready(function () {
            $("#eosContentResults_txtBusquedaPoblacion").on("keyup", function () {
                if ($(this).val().length >= 3) {
                    $("#buscarPoblacion").click();
                }
                else {
                    $("#poblacionesFound").empty();
                }
            });
            //$("#eosContentResults_ddlPais").change(function () {
            //    $("#eosContentResults_txtBusquedaPoblacion").removeAttr("readonly");
            //})
            $("#eosContentResults_ddlPais").val("E");
            $("#buscarPoblacion").click(function () {
                $.ajax({
                    type: "POST",
                    url: "NuevaActividad.aspx/poblacionSearch",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{ "idPais":"' + $("#eosContentResults_ddlPais").val() + '", "search":"' + $("#eosContentResults_txtBusquedaPoblacion").val().toLowerCase() + '" , "idConfEmpresa":"' + $("#hdnIdConfEmpresa").val() + '"}',
                    success: function (response) {
                        if (response.d.length > 0) {
                            $("#poblacionesFound").empty();
                            $("#poblacionesFound").append("<div id='cuadroEstilo'></div>")
                            $(response.d).each(function () {
                                var funcion = "selectPoblacion('" + this.IdPoblacion + "', '" + this.Poblacion.replace(/[\W_]+/g, "") + "')";
                                var appendContent = ('<div class="poblacionSelect" id="' + this.Poblacion.replace(/[\W_]+/g, "") + '" onclick="' + funcion + '">' + this.Poblacion + '</div>');
                                $("#cuadroEstilo").append(appendContent);
                            })
                        }
                        else {
                            $("#poblacionesFound").empty();
                            $("#poblacionesFound").append("<div id='cuadroEstilo'>No se han encontrado resultados</div>");
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        $("#poblacionesFound").empty();
                        $("#poblacionesFound").append("<div id='cuadroEstilo'>Se ha producido un error recuperando datos</div>");
                    }
                });
            })
            $("#MainForm").on('submit', function (e) {
                if (!$("#selectedIdPoblacion").val()) {
                    e.preventDefault();
                    $("#poblacionesFound").empty();
                    $("#poblacionesFound").append("<div id='errorPoblacion'>Tras darle a buscar, debe pulsar sobre la población deseada de entre las que aparecerán recuadradas en verde</div>");
                }


            })
        })
    </script>

</asp:Content>
