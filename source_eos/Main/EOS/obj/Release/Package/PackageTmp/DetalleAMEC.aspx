<%@ Page Title="Nuevo Amec" Language="C#" MasterPageFile="~/Styles/EOS.Master" AutoEventWireup="true"
    CodeBehind="DetalleAMEC.aspx.cs" Inherits="EOS.DetalleAMEC" EnableEventValidation="false"
    Culture="es-ES" UICulture="es" %>

<asp:Content ID="Content1" ContentPlaceHolderID="eosContentFilter" runat="server">

    <script type="text/javascript" src="Scripts/nuevoDetalleAmec.js"></script>

    <script type="text/javascript">

        function CheckFileSize() {
            var size = document.getElementById("<%=fuploadDocumentacion.ClientID%>").files[0].size;

            if (size > 6291456) {
                alert("No se puede subir un documento más grande de 6MB");
                return false;

            } else {
                return true;
            }

        }



        function CheckFileSizeProgram() {
            var size = document.getElementById("<%=fUploadPrograma.ClientID%>").files[0].size;

            if (size > 6291456) {
                alert("No se puede subir un documento más grande de 6MB");
                return false;

            } else {
                return true;
            }

        }
        function paraguas(check) {
            if (check.checked) {
                var div_to_disable = document.getElementById('<%=plAMECDatos3.ClientID %>').getElementsByTagName("input");
                var children = div_to_disable; //.childNodes;
                for (var i = 0; i < children.length; i++) {
                    children[i].disabled = false;
                }
                //document.getElementById('eosContentFilter_UpdateUnidades').disabled = false;
            } else {
                var div_to_disable = document.getElementById('<%=plAMECDatos3.ClientID %>').getElementsByTagName("input");
                var children = div_to_disable;//.childNodes;
                for (var i = 0; i < children.length; i++) {
                    children[i].disabled = true;
                }
                //document.getElementById('eosContentFilter_UpdateUnidades').disabled = false;
                //document.getElementById("unidades").style.display = "none";
                //document.getElementById('eosContentFilter_UpdateUnidades').disabled = false;
                //document.getElementById('eosContentFilter_btnAnadirUnidadesOrgAMEC').disabled = false;
                //document.getElementById('eosContentFilter_lvUnidadesOrg_ImgDeleteButton_0').disabled = false;


            }
        }

        function GetVal() {
            var a = null;
            var f = document.forms[0];
            var e = document.getElementById("eosContentFilter_rblFarmaindustria_0");
            var g = document.getElementById("eosContentFilter_rblFarmaindustria");
            g.checked;
            e.checked;
            for (var i = 0; i < e.attributes.length; i++) {
                if (e[i].checked) {
                    a = e[i].value;
                    break;
                }
            }
            return a;
        }

        function ConfirmarEnviarFarmaIndustria() {
            var radioButtonFarma = document.getElementById("eosContentFilter_rblFarmaindustria_0");
            var checkbox = document.getElementById('<%=chkemail.ClientID %>');
            if (CheckFileSize()) {
                if (checkbox.checked) {
                    if (radioButtonFarma.checked) {
                        if (confirm("Este documento será enviado para su comunicación a Farmaindustria. Pulsa 'OK' o 'Aceptar' si ya has incluido TODOS los documentos necesarios en AMEC.")) {
                            document.getElementById("TornarEnviarFarma").value = "true";
                            //document.getElementById("DescEnviarMails").style.display = "";
                        } else {
                            document.getElementById("TornarEnviarFarma").value = "false";
                            //document.getElementById("DescEnviarMails").style.display = "none";
                        }
                    } else {
                        document.getElementById("TornarEnviarFarma").value = "false";
                    }
                } else {
                    document.getElementById("TornarEnviarFarma").value = "false";
                }
                return true;
            } else return false;
        }

        function ConfirmarResometer(YaSeHaEnviadoAFarma, YaSeHaEnviadoACasosClinicos) {
            debugger;
            var radioButtonFarma = document.getElementById("eosContentFilter_rblFarmaindustria_0");
            var radioButtonCasos = document.getElementById("eosContentFilter_rblCasosClinicos_0");
            if (confirm("Este Amec Ya está Sometido. ¿Desea Resometer el AMEC?")) {

                if ($('#eosAMECUnidadesOrganizativas table tbody').children().length === 1) {

                }

                if (YaSeHaEnviadoAFarma == "True") {
                    if (radioButtonFarma.checked) {
                        if (confirm("Quiere enviar otra vez a FarmaIndustria")) {
                            document.getElementById("TornarEnviarFarma").value = "true";
                        }
                        else {
                            document.getElementById("TornarEnviarFarma").value = "false";
                        }
                    }
                }
                if (YaSeHaEnviadoACasosClinicos == "True") {
                    if (radioButtonCasos.checked) {
                        if (confirm("Quiere enviar otra vez a Casos Clínicos")) {
                            document.getElementById("TornarEnviarCasosClinicos").value = "true";
                        }
                        else {
                            document.getElementById("TornarEnviarCasosClinicos").value = "false";
                        }
                    }
                }

                return true;
            }
            else {
                return false;
            }
        }

        //        function ConfirmarEliminarUnidadOrganizativa() {
        //            if (confirm("¿Esta seguro que quiere eliminar la Unidad Organizativa?. Si elimina será obligatorio Resometer el amec")) {
        //                return true;
        //            }
        //            else {
        //                return false;
        //            }
        //        }

        function ConfirmarRechazar() {
            if (confirm("Esta acción rechazará el AMEC. No podrá volver a modificar, someter o utilizar ¿Está de acuerdo?")) {
                return true;
            }
            else {
                return false;
            }
        }

        function ConfirmarCancelacion() {
            if (confirm("Esta acción cancelará el AMEC. No se podrá volver a modificar, someter o utilizar. ¿Está de acuerdo?")) {
                return true;
            }
            else {
                return false;
            }
        }
        function ConfirmarAprobacion() {
            if (confirm("Este Amec No tiene un Comentario para la Aprobación Condicionada. Debe Introducir un Comentario")) {
                return false;
            }
            else {
                return false;
            }
        }
        function asociarAmec(check) {
            if (check.checked) {
                document.getElementById("AsociarEvento").style.display = "";
                document.getElementById("tituloAsociarEvento").style.display = "";


            } else {
                document.getElementById("AsociarEvento").style.display = "none";
                document.getElementById("tituloAsociarEvento").style.display = "none";
            }
        }


        function docAdicional(check) {
            if (check.checked) {
                document.getElementById("DocAdicional").style.display = "";

            } else {
                document.getElementById("DocAdicional").style.display = "none";
            }
        }

        function OnLoadPrograma() {
            if (document.getElementById("eosContentFilter_ddlArchivoPrograma") !== null &&
                document.getElementById("eosContentFilter_ddlArchivoPrograma") !== undefined) {
                if (document.getElementById("eosContentFilter_ddlArchivoPrograma").value == "ARCHIVO") {
                    document.getElementById("url").style.display = "none";
                    document.getElementById("descripcion").style.display = "none";
                    document.getElementById("adjuntar").style.display = "";
                    document.getElementById("BotonAdjuntarPrograma").style.display = "";
                    onOff("<%= rfvUploadPrograma.ClientID %>", true);
                    onOff("<%= rfvURLProgAMEC.ClientID %>", false);
                    onOff("<%= rfvDescProgAMEC.ClientID %>", false);
                }
                if (document.getElementById("eosContentFilter_ddlArchivoPrograma").value == "ENLACE") {
                    document.getElementById("adjuntar").style.display = "none";
                    document.getElementById("descripcion").style.display = "none";
                    document.getElementById("url").style.display = "";
                    document.getElementById("BotonAdjuntarPrograma").style.display = "none";
                    onOff("<%= rfvUploadPrograma.ClientID %>", false);
                    onOff("<%= rfvURLProgAMEC.ClientID %>", true);
                    onOff("<%= rfvDescProgAMEC.ClientID %>", false);
                }
                if (document.getElementById("eosContentFilter_ddlArchivoPrograma").value == "DESCRIPCION") {
                    document.getElementById("adjuntar").style.display = "none";
                    document.getElementById("url").style.display = "none";
                    document.getElementById("descripcion").style.display = "";
                    document.getElementById("BotonAdjuntarPrograma").style.display = "none";
                    onOff("<%= rfvUploadPrograma.ClientID %>", false);
                    onOff("<%= rfvURLProgAMEC.ClientID %>", false);
                    onOff("<%= rfvDescProgAMEC.ClientID %>", true);
                }
            }
        }
        function aportarDoc(cmbAporta) {
            if (cmbAporta.value == "ARCHIVO") {
                document.getElementById("url").style.display = "none";
                document.getElementById("descripcion").style.display = "none";
                document.getElementById("adjuntar").style.display = "";
                document.getElementById("BotonAdjuntarPrograma").style.display = "";
                onOff("<%= rfvUploadPrograma.ClientID %>", true);
                onOff("<%= rfvURLProgAMEC.ClientID %>", false);
                onOff("<%= rfvDescProgAMEC.ClientID %>", false);

            }
            if (cmbAporta.value == "ENLACE") {
                document.getElementById("adjuntar").style.display = "none";
                document.getElementById("descripcion").style.display = "none";
                document.getElementById("url").style.display = "";
                document.getElementById("BotonAdjuntarPrograma").style.display = "none";
                onOff("<%= rfvUploadPrograma.ClientID %>", false);
                onOff("<%= rfvURLProgAMEC.ClientID %>", true);
                onOff("<%= rfvDescProgAMEC.ClientID %>", false);
            }
            if (cmbAporta.value == "DESCRIPCION") {
                document.getElementById("adjuntar").style.display = "none";
                document.getElementById("url").style.display = "none";
                document.getElementById("descripcion").style.display = "";
                document.getElementById("BotonAdjuntarPrograma").style.display = "none";
                onOff("<%= rfvUploadPrograma.ClientID %>", false);
                onOff("<%= rfvURLProgAMEC.ClientID %>", false);
                onOff("<%= rfvDescProgAMEC.ClientID %>", true);
            }
        }


        function onOff(validatorId, activar) {
            var validator = document.getElementById(validatorId);
            ValidatorEnable(validator, activar);
        }




        function mostrarEspecificar(ddlCriteriosSeleccion) {
            document.getElementById("CriterioSeleccion").style.display = "none";

            var texto = ddlCriteriosSeleccion.options[ddlCriteriosSeleccion.selectedIndex].text;
            if (texto == "OTRO") {
                debugger;
                document.getElementById("CriterioSeleccion").style.display = "";
                onOff("<%= rfvEspecificar.ClientID %>", true);
            }

        }

        function mostrarDescPreMed() {
            if (document.getElementById("DescPreMed").style.display == "none") {
                document.getElementById("DescPreMed").style.display = "";
            }
            else {
                document.getElementById("DescPreMed").style.display = "none";
            }
        }

        function mostrarDescGENESYS() {
            if (document.getElementById("DescGENESYS").style.display == "none") {
                document.getElementById("DescGENESYS").style.display = "";
            }
            else {
                document.getElementById("DescGENESYS").style.display = "none";
            }

        }


        function checkControl(obj, items) {

            var rbl = document.getElementById(obj);
            var rblChild = null;
            for (i = 0; i < items; i++) {
                rblChild = document.getElementById(obj + "_" + i.toString());

                if (rblChild.checked) {
                    if (rblChild.value == "1") {
                    }
                    if (rblChild.value == "0") {

                    }
                }
            }
        }

        function MostrarDescFarmaIndustria(obj, items) {

            var rbl = document.getElementById(obj);
            var rblChild = null;
            for (i = 0; i < items; i++) {
                rblChild = document.getElementById(obj + "_" + i.toString());

                if (rblChild.checked) {
                    if (rblChild.value == "1") {
                        document.getElementById("DescFarmaIndustria").style.display = "";
                        document.getElementById("DescEnviarMails").style.display = "";
                        document.getElementById("chkAdjuntarFarma").style.display = "";
                    }
                    if (rblChild.value == "0") {
                        document.getElementById("DescFarmaIndustria").style.display = "none";
                        document.getElementById("DescEnviarMails").style.display = "none";
                        document.getElementById("chkAdjuntarFarma").style.display = "none";
                    }
                }
            }
        }

        function MostrarDescDocumentacionFarmaIndustria() {

            var rblChild = document.getElementById("eosContentFilter_rblFarmaindustria_0");
            if (rblChild.checked == true) {
                document.getElementById("DescEnviarMails").style.display = "";
                document.getElementById("chkAdjuntarFarma").style.display = "";
            }
            if (rblChild.checked == false) {
                document.getElementById("DescEnviarMails").style.display = "none";
                document.getElementById("chkAdjuntarFarma").style.display = "none";
            }
        }

        function btnAprobarDisable() {
            $("#btnAprobar").hide();
            $("#btnAprobarHid").show();
        }

        window.onload = function () {
            MostrarDescDocumentacionFarmaIndustria();
            OnLoadPrograma();

        };
    </script>
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
                        <label id="Label17" class="eosCampoLabelSin bigger" runat="server">
                            ORGANON
                        </label>
                    </div>
                    <div class="align-end-txt">
                        <div class="estate-container">
                            <asp:TextBox ID="txtIdCreadoPor" ReadOnly="true" Visible="false" MaxLength="10" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txtIdEstado" ReadOnly="true" Visible="false" MaxLength="10" runat="server"></asp:TextBox>

                            <!--<label id="Label3" class="eosCampoLabelSin">
                                ESTADO:</label>-->
                            <asp:Image runat="server" ID="imgIconoEstado" />
                            <%--<asp:ImageButton ID="imgEstadoAMEC" runat="server" CausesValidation="false" />--%>
                            <asp:Label ID="lbEstadoAMEC" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="plAMECDatos1" runat="server">
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
            <div id="eosContentAMECErrores">
                <asp:ValidationSummary ID="ContenAMECValidationSummary" runat="server" CssClass="failureNotification"
                    ValidationGroup="AMECValidationGroup" HeaderText="ATENCIÓN. Los siguientes campos son obligatorios:" />
                <span class="failureNotification">
                    <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                </span>
            </div>
            <div id="eosTablaAMEC1" class="eosTablaFiltros" width="100%">
                <div class="contentFormGeneric">
                    <div class="contentForm  eosTresColumna">
                        <label class="eosCampoLabelSin" for="txtNombre">
                            Solicitante<span class="eosCampoObligatorio">*</span></label>
                        <asp:DropDownList ID="ddlSolicitante" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW220"
                            DataTextField="NombreCompleto" DataValueField="IdPeticionario" OnSelectedIndexChanged="ddlSolicitante_SelectedIndexChanged"
                            Visible="true" AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvSolicitante" runat="server" ErrorMessage="Solicitante."
                            ControlToValidate="ddlSolicitante" ValidationGroup="AMECValidationGroup" SetFocusOnError="True"
                            ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                    </div>
                    <div class="contentForm  eosTresColumna">
                        <label class="eosCampoLabelSin" for="txtCiudad">
                            N. Wein<span class="eosCampoObligatorio">*</span></label>
                        <asp:TextBox ID="txtWein" ReadOnly="true" MaxLength="45" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW140"></asp:TextBox>
                    </div>
                    <div class="contentForm  eosTresColumna">
                        <label class="eosCampoLabelSin" for="txtCiudad">
                            N. AMEC<span class="eosCampoObligatorio">*</span></label>
                         <asp:TextBox ID="txtNAmec" ReadOnly="true" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW190"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="rfvNAmec" runat="server" ErrorMessage="N. AMEC." ControlToValidate="txtNAmec"
                             ValidationGroup="AMECValidationGroup" SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="contentFormGeneric">
                    <div class="contentForm  eosTresColumna">
                        <label id="lblDistrito" class="eosCampoLabelSin">Distrito: &nbsp;</label>
                        <asp:TextBox ID="txtDistrito" ReadOnly="true" runat="server" MaxLength="128" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW220"></asp:TextBox>
                    </div>
                    <div class="contentForm  eosTresColumna">
                        <label id="lblDepartamento" class="eosCampoLabelSin">Depart: &nbsp;</label>
                        <asp:TextBox ID="txtDepartamento" ReadOnly="true" runat="server" MaxLength="128" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW140"></asp:TextBox>
                    </div>
                    <div class="contentForm  eosTresColumna">
                        <label id="lblFuerzaVentas" class="eosCampoLabelSin">F. Ventas: &nbsp;</label>
                        <asp:TextBox ID="txtFuerzaVentas" ReadOnly="true" runat="server" MaxLength="128" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW190"></asp:TextBox>
                    </div>
                </div>
                <div class="contentFormGeneric">
                    <div class="contentForm  eosTresColumna">
                        <label class="eosCampoLabelSin" for="txtNombre">
                            Cargo<span class="eosCampoObligatorio">*</span></label>
                        <asp:TextBox ID="txtCargo" ReadOnly="true" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW220"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvCargo" runat="server" ErrorMessage="Cargo." ControlToValidate="txtCargo"
                            ValidationGroup="AMECValidationGroup" SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                    </div>
                    <div class="contentForm  eosTresColumna">
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
                        <asp:ImageButton ID="btnCalendar" runat="server" ImageUrl="~/Styles/images/ic_calendario.png" class="iconCalendar"/>
                        <act:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFecha" runat="server"
                            TodaysDateFormat="d MMMM yyyy" FirstDayOfWeek="Monday" PopupButtonID="btnCalendar"
                            Format="dd/MM/yy" />
                        &nbsp;&nbsp;&nbsp;
                    </div>
                    <div class="contentForm  eosTresColumna">
                        <label class="eosCampoLabelSin" id="lblCreadopor">
                            Creado por<span class="eosCampoObligatorio">*</span>&nbsp;</label>
                        <asp:TextBox ID="txtCreadoPor" ReadOnly="true" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW190"></asp:TextBox>
                    </div>
                </div>
                <div class="contentFormGeneric">
                    <div class="contentForm  eosTresColumna">
                        <asp:Label ID="Label7" runat="server" CssClass="eosCampoLabel">Tipo de Actividad<span class="eosCampoObligatorio">*</span></asp:Label>
                        <asp:DropDownList ID="ddlActividad" runat="server" AppendDataBoundItems="True" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW220"
                            DataTextField="tipoactividad" DataValueField="idtipoactividad" Visible="true"
                            AutoPostBack="True" OnSelectedIndexChanged="ddlActividad_SelectedIndexChanged">
                            <asp:ListItem Value="-1">Selecciona Tipo de Actividad</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="ddlActividad"
                            ErrorMessage="Tipo de actividad." MaximumValue="9999999" MinimumValue="1" SetFocusOnError="true"
                            ToolTip="Campo Obligatorio" ValidationGroup="AMECValidationGroup">&nbsp;</asp:RangeValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ddlActividad"
                            ErrorMessage="Tipo de actividad." SetFocusOnError="True" ToolTip="Campo Obligatorio"
                            ValidationGroup="AMECValidationGroup">&nbsp;</asp:RequiredFieldValidator>
                    </div>
                    <div class="contentForm  eosTresColumna">
                        <label class="eosCampoLabelSin" for="txtAgencias">Agencia<span class="eosCampoObligatorio">*</span> &nbsp;</label>
                        <asp:DropDownList ID="ddlAgencias" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW160"
                            AutoPostBack="false" Enabled="true">
                        </asp:DropDownList>
                    </div>
                    <div class="contentForm  eosTresColumna contentFormRadio">
                        <div class="radioCheck">
                            <asp:Label ID="lblParaguas" runat="server" Text="Paraguas"></asp:Label>
                            <asp:CheckBox ID="chkParaguas" runat="server" OnCheckedChanged="chkParaguas_OnCheckedChanged" Text=" " AutoPostBack="true" />
                        </div>
                    </div>
                </div>
                <div class="contentFormGeneric">
                    <div class="contentForm eosUnoColumna">
                        <asp:Label ID="lblDescripcionAMEC" runat="server" CssClass="eosCampoLabel">Nombre Programa/Actividad<span class="eosCampoObligatorio">*</span>&nbsp;</asp:Label>
                        <asp:TextBox ID="txtDescripcionAMEC" runat="server" CssClass="eosSizeW730" MaxLength="200"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDescripcionAMEC" runat="server" ErrorMessage="Nombre Programa/Actividad."
                            ControlToValidate="txtDescripcionAMEC" ValidationGroup="AMECValidationGroup"
                            SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="eosTablaFiltros">
                <div class="contentFormGeneric">
                    <div class="contentForm eosTresColumna">
                        <div>
                            <asp:Label ID="lblPreaprobadaMedico" class="eosCampoLabel" ToolTip="Como norma general, se consideran preaprobados aquellos programas o reuniones que cumplan los requisitos especificados en el punto 6 ii.) Aprobación de la ejecución: reglas aplicables del procedimiento AMEC."
                                runat="server">Preaprobada médico </asp:Label>
                            <asp:HyperLink ID="HyperLink1" onclick="javascript:mostrarDescPreMed(this);" runat="server"
                                class="eosVerMas contentMoreInfo">+</asp:HyperLink><span class="eosCampoObligatorio">*</span>
                        </div>
                        <div class="radioCheck" style="margin-top: 10px;">
                            <asp:RadioButtonList ID="rblPreaprobMedico" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Text="Si&nbsp;&nbsp;&nbsp;" Value="1" />
                                <asp:ListItem Text="No" Value="0" />
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="rfvPreaprobMedico" runat="server" ErrorMessage="Preaprobada médico."
                                ControlToValidate="rblPreaprobMedico" ValidationGroup="AMECValidationGroup" SetFocusOnError="True"
                                ToolTip="Campo Obligatorio">&nbsp;
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="contentForm eosTresColumna">
                        <label id="lblFarmaindustria" class="eosCampoLabelSin">
                            Comunicación a Farmaindustria<span class="eosCampoObligatorio">*</span></label> 
                        <div class="radioCheck">
                            <asp:RadioButtonList ID="rblFarmaindustria" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Text="Si&nbsp;&nbsp;&nbsp;" Value="1" />
                                <asp:ListItem Text="No" Value="0" />
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="rfvFarmaindustria" runat="server" ErrorMessage="Farmaindustria."
                                ControlToValidate="rblFarmaindustria" ValidationGroup="AMECValidationGroup" SetFocusOnError="True"
                                ToolTip="Campo Obligatorio">&nbsp;
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="contentForm eosTresColumna">
                        <label id="lblPreaprobNegocio" class="eosCampoLabelSin">
                            Preaprobada negocio<span class="eosCampoObligatorio">*</span></label>
                        <div class="radioCheck">
                            <asp:RadioButtonList ID="rblPreaprobNegocio" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Text="Si&nbsp;&nbsp;&nbsp;" Value="1" />
                                <asp:ListItem Text="No" Value="0" />
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="rfvPreaprobNegocio" runat="server" ErrorMessage="Preaprobada negocio."
                                ControlToValidate="rblPreaprobNegocio" ValidationGroup="AMECValidationGroup"
                                SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div id="DescFarmaIndustria" style="display: none">
                    <div></div>
                    <div></div>
                    <div width="40%">
                        <label id="lblFarmaindustrias" class="eosCampoLabelSin" style="font-style: italic">
                            Recuerda descargarte en supporting documents el formulario correspondiente para
                            Farmaindustria, completarlo y subirlo a este AMEC mediante la opción INCLUIR DOCUMENTACION
                            ADICIONAL al final de esta página; recuerda asimismo seleccionar la opción "Adjuntar
                            a Email automático a departamento de Compliance/Farmaindustria (formularios Farmaindustria
                            y otros documentos-contratos relevantes)."</label>
                    </div>
                    <div></div>
                </div>
                <div id="DescPreMed" style="display: none">
                    <div>
                        <label id="lblDescPredMed" style="font-style: italic" class="eosCampoLabelSin">
                            Como norma general, se consideran preaprobados aquellos programas o reuniones que cumplan 
los requisitos especificados en el punto 6 ii.) Aprobación de la ejecución: reglas aplicables del procedimiento AMEC.</label>
                    </div>
                    <div></div>
                </div>
                <div class="contentFormGeneric">                    
                    <div class="contentForm eosTresColumna">
                        <label id="lblCasosClinicos" class="eosCampoLabelSin">
                            ¿Conlleva la gestión de casos clínicos con productos MSD?<span class="eosCampoObligatorio">*</span></label>
                         <div class="radioCheck">
                             <asp:RadioButtonList ID="rblCasosClinicos" runat="server" RepeatDirection="Horizontal"
                                 RepeatLayout="Flow">
                                 <asp:ListItem Text="Si&nbsp;&nbsp;&nbsp;" Value="1" />
                                 <asp:ListItem Text="No" Value="0" />
                             </asp:RadioButtonList>
                             <asp:RequiredFieldValidator ID="rfvCasosClinicos" runat="server" ErrorMessage="Casos clínicos."
                                 ControlToValidate="rblCasosClinicos" ValidationGroup="AMECValidationGroup" SetFocusOnError="True"
                                 ToolTip="Campo Obligatorio">&nbsp;
                             </asp:RequiredFieldValidator>
                         </div>
                    </div>
                    <div class="contentForm eosTresColumna">
                        <label id="lblPreaprovLegal_Compliance" class="eosCampoLabelSin">
                            Preaprobada legal <span class="eosCampoObligatorio">*</span></label>
                        <div class="radioCheck">
                            <asp:RadioButtonList ID="rblPreaprovLegal_Comp" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Text="Si&nbsp;&nbsp;&nbsp;" Value="1" />
                                <asp:ListItem Text="No" Value="0" />
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="rfvPreaprovLegal_Comp" runat="server" ErrorMessage="Preaprobada legal / compliance."
                                ControlToValidate="rblPreaprovLegal_Comp" ValidationGroup="AMECValidationGroup"
                                SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="contentForm eosTresColumna">
                         <label id="Label2" class="eosCampoLabelSin">
                            Actividad conforme con la C.Pol 5/FCPA<span class="eosCampoObligatorio">*</span></label>
                        <div class="radioCheck">
                            <asp:RadioButtonList ID="rblpoliticaN20" runat="server" RepeatDirection="Horizontal"
                                RepeatLayout="Flow">
                                <asp:ListItem Text="Si&nbsp;&nbsp;&nbsp;" Value="1" />
                                <asp:ListItem Text="No" Value="0" />
                            </asp:RadioButtonList>
                            <%--                            <asp:RequiredFieldValidator ID="rfvpoliticaN20" runat="server" ErrorMessage="Actividad conforme con la política N.20/FCPA"
                                    ControlToValidate="rblpoliticaN20" ValidationGroup="AMECValidationGroup" SetFocusOnError="True"
                                    ToolTip="Campo Obligatorio">&nbsp;
                                </asp:RequiredFieldValidator>--%>
                            <asp:RequiredFieldValidator ID="rfvpoliticaN20" runat="server" ErrorMessage="Actividad conforme con la C.Pol 5/FCPA."
                                ControlToValidate="rblpoliticaN20" ValidationGroup="AMECValidationGroup" SetFocusOnError="True"
                                ToolTip="Campo Obligatorio">&nbsp;
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <div id="unidades">
        <asp:Panel ID="plAMECDatos3" runat="server">
            <div class="eosTituloWizard">
                AÑADIR APROBADORES ADICIONALES
            </div>
            <div id="eosAMECUnidadesOrganizativas" class="eosFiltroColor contentTableGeneric">
                <div class="eosFiltroColorTabla">
                    <table class="eosTablaResultados">
                        <thead>
                            <tr>
                                <th></th>
                                <th>APROBADOR
                                </th>
                                <th>DEPARTAMENTO
                                </th>
                                <th>FUERZA DE VENTAS
                                </th>
                                <th>DISTRITO
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="listManagers" runat="server" OnItemDataBound="listManagers_ItemDataBound">
                                <ItemTemplate>
                                    <tr id="NoRecords" runat="server" visible="False">
                                        <td>No records are available.</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ImageButton Text="Borrar" ID="ImgDeleteButton" runat="server" ImageUrl="~/Styles/images/ic_participante_aspa.png"
                                                CommandArgument='<%# Eval("IdPeticionario") %>' ToolTip="Borrar aprobador"
                                                OnCommand="imgAprobadorEliminar_Command" />
                                        </td>
                                        <td class="par">
                                            <asp:Label ID="lblAprobador" runat="server" Text='<%#" "+ Eval("Nombre").ToString()+ " " + Eval("Apellido1").ToString() +" "+ Eval("Apellido2") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDepartamento" runat="server" Text='<%# Eval("departament") %>' />
                                        </td>
                                        <td class="par">
                                            <asp:Label ID="lblFuerza" runat="server" Text='<%# Eval("saleforce") %>' />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDistrito" runat="server" Text='<%# Eval("district") %>' />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                    <br />
                    <div class="eosTablaFiltros" width="100%">
                        <div class="contentFormGeneric">
                            <div class="contentForm eosUnoColumna">
                                &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="ToolTip" runat="server" Text="* No es necesario que añadas a tu superior jerárquico"></asp:Label>
                            </div>
                            <div class="contentForm eosMitadColumna" style="padding: 0;">
                                &nbsp;&nbsp;&nbsp;&nbsp;<label class="eosCampoLabelSin" for="ddlManager">Aprobadores adicionales: &nbsp;</label>
                                <asp:DropDownList ID="ddlManager" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW300"
                                    AutoPostBack="false" Enabled="true">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="eosBotonera eosBotonFiltrado">
                        <asp:Button ID="btnAnadirAprobador" runat="server" Visible="true" Enabled="True" Text="Añadir aprobador"
                            OnClick="btnAnadirAprobador_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <asp:Panel ID="plPaso1" runat="server">
        <br />
        <div class="eosTituloWizard">
            <div class="radioCheck inline-content">
                 <label>ASOCIAR AMEC A EVENTO O ACTIVIDAD</label>
                 <asp:CheckBox ID="chkAsociarEvento" onclick="javascript:asociarAmec(this);" runat="server"
                     Text=" " />
            </div>
        </div>
        <% if (AsociarEvento())
            { %>
        <div id="AsociarEvento">
            <% }
                else
                { %>
            <div id="AsociarEvento" style="display: none">
                <% } %>
                <div class="eosSubTitulo">
                    Eventos o Actividades asociados al AMEC
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
                <br />
                <div id="tituloAsociarEvento" class="eosSubTitulo">
                    Asociar Eventos
                </div>
                <div id="AsociarEvento" class="eosFiltroColor">
                    <div class="eosFiltroColorTabla">
                        <div class="eosTablaFiltros">
                            <div>
                                <div>
                                    <span class="eosTituloNormal">Filtro de Eventos<br />
                                        Rellene los campos necesarios y pulse el botón FILTRAR</span>
                                </div>
                            </div>
                            <div class="contentFormGeneric">
                                <div class="contentForm eosTresColumna">
                                    <label class="eosCampoLabel" for="txtNombre">El nombre contiene</label>
                                    <asp:TextBox ID="txtNombre" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"></asp:TextBox>
                                </div>
                                <div class="contentForm eosTresColumna">
                                    <label class="eosCampoLabel" for="txtCiudad">Ciudad o Sede donde se realiza:</label>
                                    <asp:DropDownList ID="ddlPoblacion" runat="server" CssClass="eosDisabledInputVacio eosInputVacio   eosSizeW220"
                                        DataTextField="Poblacion" DataValueField="IDPoblacion">
                                    </asp:DropDownList>
                                </div>
                                <div class="contentForm eosTresColumna">
                                    <label class="eosCampoLabel" for="txtAMEC">AMEC</label>
                                    <asp:TextBox ID="txtAMEC" runat="server" MaxLength="40" autocomplete="off" CssClass="eosDisabledInputVacio"></asp:TextBox>
                                </div>
                            </div>
                            <div class="contentFormGeneric">
                                <div class="contentForm eosTresColumna">
                                    <label class="eosCampoLabel" for="xxxFiltroAMEC">Tipo Evento</label>
                                    <asp:DropDownList ID="ddlTipoActividad" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW190"
                                        DataTextField="Nombre" DataValueField="IdTipoActividad">
                                    </asp:DropDownList>
                                </div>
                                <div class="contentForm eosTresColumna">
                                    <label class="eosCampoLabel" for="txtFechaDesde">Desde</label>
                                    <asp:TextBox ID="txtFechaDesde" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW50"></asp:TextBox>
                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Styles/images/ic_calendario.png" class="iconCalendar"/>
                                    <act:CalendarExtender ID="CalendarExtender2" TargetControlID="txtFechaDesde" runat="server"
                                        TodaysDateFormat="d MMMM yyyy" FirstDayOfWeek="Monday" PopupButtonID="ImageButton2" />
                                    <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="La Fecha no puede ser inferior a hoy."
                                        MaximumValue="31/12/99" ControlToValidate="txtFechaDesde" ValidationGroup="ValidacionesFecha"
                                        Display="Dynamic" Enabled="true"></asp:RangeValidator>
                                </div>
                                <div class="contentForm eosTresColumna">
                                    <label class="eosCampoLabel" for="txtFechaHasta">Hasta</label>
                                    <asp:TextBox ID="txtFechaHasta" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW50"></asp:TextBox>
                                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Styles/images/ic_calendario.png" class="iconCalendar" />
                                    <act:CalendarExtender ID="CalendarExtender3" TargetControlID="txtFechaHasta" runat="server"
                                        TodaysDateFormat="d MMMM yyyy" FirstDayOfWeek="Monday" PopupButtonID="ImageButton3" />
                                </div>
                                <div class="eosBotonera eosBotonFiltrado">
                                    <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar"
                                        OnClick="btnFiltrar_Click" ValidationGroup="ValidacionesFecha" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="eosContentResults" runat="server" visible="false" class="contentTableGeneric">
                    <asp:ListView ID="lvActivid" EnableViewState="false" runat="server" DataSourceID="odsActividades"
                        OnSelectedIndexChanged="lvActivid_SelectedIndexChanged" DataKeyNames="IDCONGRESO"
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
                                <tfoot>
                                    <tr>
                                        <td colspan="10">
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
                                        </td>
                                    </tr>
                                </tfoot>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:ImageButton ID="imgActividadDetalle" runat="server" CommandName="Select" ToolTip='<%# String.Format("Asignar el Evento {0} al Amec", Eval("idcongreso")) %>'
                                        ImageUrl="~/Styles/images/ic_amec.png" CommandArgument='<%# Eval("idcongreso") %>'
                                        OnCommand="imgAsignarEvento_Command" />
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
                    <asp:ObjectDataSource ID="odsActividades" runat="server" SelectMethod="ObtenerEventosNoRelacionadosAMEC"
                        TypeName="EOS.Web.AgenteAmecInfo" SelectCountMethod="ObtenerNumeroEventosNoRelacionadosAMEC"
                        EnablePaging="True" SortParameterName="sortParameter" OnSelecting="odsActividades_Selecting">
                        <SelectParameters>
                            <asp:Parameter Name="filtroIdAMEC" Type="String" />
                            <asp:Parameter Name="filtroIdCongreso" Type="String" />
                            <asp:Parameter Name="filtroNombre" Type="String" />
                            <asp:Parameter Name="filtroPoblacion" Type="String" />
                            <asp:Parameter Name="filtroAMEC" Type="String" />
                            <asp:Parameter Name="filtroTipoActividad" Type="String" />
                            <asp:Parameter Name="filtroFechaDesde" Type="String" />
                            <asp:Parameter Name="filtroFechaHasta" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
                <p />
                <div>
                    <asp:ImageButton ID="btnCrearNuevaActividad" runat="server" ImageUrl="~/Styles/images/bt_crear_n_actividad.png"
                        PostBackUrl="~/NuevaActividad.aspx" Visible="false" />
                </div>
            </div>
    </asp:Panel>
    <asp:Panel ID="Panel8" runat="server" CssClass="contentTableGeneric">
        <br />
        <div class="eosTituloWizard">
            Descripción / Objetivos
        </div>
        <div>
            <label id="Label9" class="eosSubTitulo">
                Adjuntar programa <span class="eosSubTituloNegrita">o</span> web de programa <span
                    class="eosSubTituloNegrita">o</span>,en su defecto, introducir descripción</label>
        </div>
        <table id="TablaPrograma" class="eosTablaFiltros">
            <asp:ListView ID="lvPrograma" runat="server" EnableViewState="false" DataSourceID="odsProgramaAMEC" EnablePersistedSelection="True"
                DataKeyNames="IDAMECS">
                <EmptyDataTemplate>
                    <table style="margin-top: 20px" class="eosTablaResultados">
                        <thead>
                            <tr>
                                <th></th>
                                <th>Archivo Programa
                                </th>
                                <th>Web del Programa
                                </th>
                                <th>Descripción del Programa
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="10">
                                    <span class="eosTituloRojo">No se ha adjuntado el Programa</span>
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
                                    <asp:LinkButton ID="lbUsuario" runat="server" Text="Archivo del Programa" CommandName="Sort"
                                        CommandArgument="USUARIO"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lbFechaComentario" runat="server" Text="Web del Programa" CommandName="Sort"
                                        CommandArgument="FECHA"></asp:LinkButton>
                                </th>
                                <th>
                                    <asp:LinkButton ID="lbComentarioComentario" runat="server" Text="Descripción Del Programa"
                                        CommandName="Sort" CommandArgument="COMENTARIO"></asp:LinkButton>
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
                                    ShowPreviousPageButton="True" FirstPageText=" " PreviousPageText=" " PreviousPageImageUrl="Styles/images/ic_pagina_anterior.png"
                                    FirstPageImageUrl="Styles/images/ic_pagina_primera.png" RenderDisabledButtonsAsLabels="True" />
                                <asp:NumericPagerField CurrentPageLabelCssClass="pageSel" />
                                <asp:NextPreviousPagerField ButtonType="Image" ShowLastPageButton="True" ShowNextPageButton="True"
                                    ShowPreviousPageButton="False" LastPageText=" " NextPageText=" " LastPageImageUrl="Styles/images/ic_pagina_ultima.png"
                                    NextPageImageUrl="Styles/images/ic_pagina_siguiente.png" RenderDisabledButtonsAsLabels="True" />
                            </Fields>
                        </asp:DataPager>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="table-icon-container">
                            <asp:ImageButton Text="Borrar" ID="imgComentarioEliminar" runat="server" ImageUrl="~/Styles/images/ic_participante_aspa.png"
                                CommandArgument='<%# Eval("idamecs") %>' ToolTip="Anular Programa" OnCommand="imgAnularPrograma_Command" />
                            <asp:ImageButton Text="Descargar" ID="ImgDescargarDoc" runat="server" ImageUrl="~/Styles/images/download.png"
                                ToolTip='<%# String.Format("Descargar Programa") %>' CommandName='<%# Eval("programaamecs") %>'
                                OnCommand="imgDownloadPrograma_Command" />
                        </td>
                        <td class="par">
                            <asp:Label ID="lbUsuario" runat="server" Text='<%# Eval("programaamecs") %>' />
                        </td>
                        <td>
                            <asp:Label ID="lbFechaComentario" runat="server" Text='<%# Eval("urlprograma") %>' />
                        </td>
                        <td class="par">
                            <asp:Label ID="lbComentarioComentario" runat="server" Text='<%# Eval("descripcionobjetivo") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </table>
        <br />
        <% if (VisualitzarPrograma())
            { %>
        <div id="Div6" class="eosFiltroColor">
            <div class="eosFiltroColorTabla">
                <table id="Table3" class="eosTablaFiltros">
                    <tr>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlArchivoPrograma" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW300"
                                DataTextField="Nombre" DataValueField="IdTipoActividad" OnChange="javascript:aportarDoc(this);"
                                EnableViewState="true">
                                <asp:ListItem Text="Adjuntar Archivo del Programa" Value="ARCHIVO"></asp:ListItem>
                                <asp:ListItem Text="Introducir Web del Programa" Value="ENLACE"></asp:ListItem>
                                <asp:ListItem Text="Descripción del Programa" Value="DESCRIPCION"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="adjuntar">
                        <td colspan="2">
                            <span class="eosCampoObligatorio">*</span>
                            <asp:FileUpload ID="fUploadPrograma" runat="server" ValidationGroup="AMECValidationGroup" />
                        </td>
                        <asp:RequiredFieldValidator ID="rfvUploadPrograma" runat="server" ControlToValidate="fUploadPrograma"
                            ErrorMessage="Informar Archivo Programa." SetFocusOnError="False" ToolTip="Campo Obligatorio"
                            ValidationGroup="AMECValidationGroup" Enabled="false">&nbsp;</asp:RequiredFieldValidator>
                    </tr>
                    <tr id="url" style="display: none">
                        <td>
                            <label id="lblURLProgAMEC" class="eosCampoLabelSin" title="Indicar URL del programa">
                                Introducir Web del Programa<span class="eosCampoObligatorio">*</span></label>
                        </td>
                        <td colspan="1">
                            <asp:TextBox ID="txtURLProgAMEC" Visible="true" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW450"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvURLProgAMEC" runat="server" ControlToValidate="txtURLProgAMEC"
                                ErrorMessage="Informar Web del Programa." SetFocusOnError="True" ToolTip="Campo Obligatorio"
                                ValidationGroup="AMECValidationGroup" Enabled="false">&nbsp;</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="descripcion">
                        <td>
                            <label id="lblDescProgAMEC" class="eosCampoLabelSin">
                                Descripción del Programa<span class="eosCampoObligatorio">*</span></label>
                        </td>
                        <td colspan="1">
                            <asp:TextBox ID="txtDescProgAMEC" Visible="true" runat="server" MaxLength="300" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW450"
                                OnTextChanged="txtDescProgAMEC_TextChanged"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDescProgAMEC" runat="server" ControlToValidate="txtDescProgAMEC"
                                ErrorMessage="Informar Descripción del Programa." SetFocusOnError="True" ToolTip="Campo Obligatorio"
                                ValidationGroup="AMECValidationGroup" Enabled="false">&nbsp;</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="BotonAdjuntarPrograma">
                        <td colspan="2">
                            <asp:ImageButton ImageAlign="Middle" ID="btnAdjuntarPrograma" runat="server" ImageUrl="~/Styles/images/bt_adjuntar_programa.png"
                                OnCommand="imgAdjuntarPrograma_Command" OnClientClick="return CheckFileSizeProgram();" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <% } %>
    </asp:Panel>
    <asp:Panel ID="plAMECDatos7" runat="server">
        <div class="eosTituloWizard">
            Datos específicos de la actividad
        </div>
        <div class="eosSubTitulo">
            Donde aplique
        </div>
        <div id="Div5" class="eosFiltroColor">
            <div class="eosFiltroColorTabla">
                <div class="eosTablaFiltros">
                    <div class="contentFormGeneric">
                        <div class="contentForm eosMitadColumna">
                            <label id="lblLugarSede" class="eosCampoLabelSin" title="Numero total de participantes por parte de MSD">
                                Lugar de realización, sede y categoría:</label>
                            <asp:TextBox ID="txtLugarSede" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW150 "></asp:TextBox>
                        </div>
                        <div class="contentForm eosMitadColumna">
                            <label id="lbHoras" class="eosCampoLabelSin">
                                Duración horas:</label>
                            <asp:TextBox ID="txtHoras" Visible="true" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW120"></asp:TextBox>
                            <asp:CompareValidator ID="cvtxtHoras" runat="server" ErrorMessage="La Duración horas debe ser un número."
                                Type="String" ControlToValidate="txtHoras" Operator="DataTypeCheck" ValidationGroup="AMECValidationGroup">&nbsp;</asp:CompareValidator>
                        </div>
                    </div>
                    <div class="contentFormGeneric">
                        <div class="contentForm eosMitadColumna">
                            <label id="lblTotalParticipantes" class="eosCampoLabelSin"
                                title="Numero total de participantes por parte de MSD">
                                N. total de participantes por parte de MSD:</label>
                            <asp:TextBox ID="txtTotalParticipantes" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW150"></asp:TextBox>
                            <asp:CompareValidator ID="cvtxtTotalParticipantes" runat="server" ErrorMessage="El total de participantes debe ser un número."
                                Type="Integer" ControlToValidate="txtTotalParticipantes" Operator="DataTypeCheck"
                                ValidationGroup="AMECValidationGroup">&nbsp;</asp:CompareValidator>
                        </div>
                        <div class="contentForm eosMitadColumna">
                            <label id="lblNumPonentes" class="eosCampoLabelSin" 
                                title="Número ponentes patrocinados por MSD">
                                N. ponentes patrocinados por MSD:</label>
                            <asp:TextBox ID="txtNumPonentes" Visible="true" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW120 "></asp:TextBox>
                            <asp:CompareValidator ID="cvtxtNumPonentes" runat="server" ErrorMessage="N. ponentes patrocinados por MSD debe ser un número."
                                Type="Integer" ControlToValidate="txtNumPonentes" Operator="DataTypeCheck" ValidationGroup="AMECValidationGroup">&nbsp;</asp:CompareValidator>
                        </div>
                    </div>
                    <div class="contentFormGeneric">
                        <div class="contentForm eosMitadColumna">
                            <label id="lblProfesionalesSanitarios" class="eosCampoLabelSin"
                                title="Numero total de participantes por parte de MSD">
                                N. Profesionales Sanitarios percibiendo honorarios de MSD:</label>
                             <asp:TextBox ID="txtProfesionalesSanitarios" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW150"></asp:TextBox>
                             <asp:CompareValidator ID="CompareValidator4" runat="server" ErrorMessage="El total de Profesionales Sanitarios debe ser un número."
                                 Type="Integer" ControlToValidate="txtProfesionalesSanitarios" Operator="DataTypeCheck"
                                 ValidationGroup="AMECValidationGroup">&nbsp;</asp:CompareValidator>
                        </div>
                    </div>
                    <div class="contentFormGeneric">
                        <div nowrap="contentForm eosUnaColumna">
                            <label class="eosCampoLabelSin" id="lblCriteriorSeleccion"
                                title="Detalle de los criterios de selección">
                                Detalle de los criterios de selección:</label>
                        </div>

                    </div>
                    <div class="contentFormGeneric">
                        <div class="contentForm eosUnoColumna">
                            <asp:DropDownList ID="ddlCriteriosSeleccion" Style="margin-left: 10px" runat="server" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW800"
                                DataTextField="criterioseleccion" RepeatLayout="Flow" RepeatDirection="Horizontal"
                                DataValueField="idcriterio" Visible="true" CausesValidation="True" Font-Strikeout="False"
                                onclick="javascript:mostrarEspecificar(this);">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <% if (VerCriterioEspecifico())
                        { %>
                    <div id="CriterioSeleccion">
                        <% }
                            else
                            { %>
                        <div id="CriterioSeleccion" style="display: none">
                            <% } %>
                            <div>
                                <label class="eosCampoLabelSin" id="lblEspecificar" title="Especificar criterio de selección">
                                    Especificar:<span class="eosCampoObligatorio">*</span></label>
                            </div>
                            <div colspan="3">
                                <asp:TextBox ID="txtEspecificarCriterioSeleccion" runat="server" MaxLength="200"
                                    CssClass="eosDisabledInputVacio eosInputVacio    eosSizeW550  "></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEspecificar" runat="server" ErrorMessage="Especificar Criterio Seleccion."
                                    ControlToValidate="ddlSolicitante" ValidationGroup="AMECValidationGroup" SetFocusOnError="True"
                                    ToolTip="Campo Obligatorio" Enabled="false">&nbsp;</asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="contentFormGeneric">
                            <div class="contentForm eosMitadColumna">
                                <label class="eosCampoLabelSin">
                                    Fecha de Comienzo Prevista</label>
                                <asp:TextBox ID="txtFechaComienzo" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio  eosSizeW90"></asp:TextBox>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="La fecha de Comienzo debe tener un formato válido."
                                    Type="Date" ControlToValidate="txtFechaComienzo" Operator="DataTypeCheck" ValidationGroup="AMECValidationGroup">&nbsp;</asp:CompareValidator>
                                <asp:ImageButton ID="btnCalendarIni" runat="server" ImageUrl="~/Styles/images/ic_calendario.png" class="iconCalendar" />
                                <act:CalendarExtender ID="CalendarExtender4" TargetControlID="txtFechaComienzo" runat="server"
                                    TodaysDateFormat="d MMMM yyyy" FirstDayOfWeek="Monday" PopupButtonID="btnCalendarIni"
                                    Format="dd/MM/yy" />
                            </div>
                            <div class="contentForm eosMitadColumna">
                                <label class="eosCampoLabelSin" for="txtNombre">
                                    Fecha de Finalización Prevista</label>
                                <asp:TextBox ID="txtFechaFinalizacion" runat="server" MaxLength="80" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW90"></asp:TextBox>
                                <asp:CompareValidator ID="CompareValidator3" runat="server" ErrorMessage="La Fecha de Finalización debe tener un formato válido."
                                    Type="Date" ControlToValidate="txtFechaFinalizacion" Operator="DataTypeCheck"
                                    ValidationGroup="AMECValidationGroup">&nbsp;</asp:CompareValidator>
                                <asp:RangeValidator ID="RangeValidator6" runat="server" ErrorMessage="La fecha de Finalización no puede ser inferior a hoy."
                                    MaximumValue="31/12/99" ControlToValidate="txtFechaFinalizacion" ValidationGroup="AMECValidationGroup"
                                    Display="Dynamic">&nbsp;</asp:RangeValidator>
                                <asp:ImageButton ID="btnCalendarFin" runat="server" ImageUrl="~/Styles/images/ic_calendario.png" class="iconCalendar" />
                                <act:CalendarExtender ID="CalendarExtender5" TargetControlID="txtFechaFinalizacion"
                                    runat="server" TodaysDateFormat="d MMMM yyyy" FirstDayOfWeek="Monday" PopupButtonID="btnCalendarFin"
                                    Format="dd/MM/yy" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </div>
                        </div>
                        <div>
                            <div class="contentFormGeneric">
                                <div class="contentForm eosUnoColumna">
                                    <div>
                                        <label class="eosCampoLabelSin" id="lblMedicosGenesys"
                                            title="El listado definitivo de asistentes se subira a APPIAN sin demora una vez concluida la actividad">
                                            Todos los médicos que han sido incluidos en la actividad están dados de alta en
                                                el fichero APPIAN del delegado.</label>
                                        <asp:HyperLink ID="HyperLink2" onclick="javascript:mostrarDescGENESYS(this);"
                                            runat="server" class="eosVerMas contentMoreInfo">+</asp:HyperLink>
                                    </div>
                                    <asp:RadioButtonList ID="rbMedicosGenesys" RepeatLayout="Flow" RepeatDirection="Horizontal"
                                        runat="server" CellSpacing="10" CssClass="radioCheck right-space" >
                                        <asp:ListItem Text="Si&nbsp;&nbsp;&nbsp;" Value="1" Selected="True" />
                                        <asp:ListItem Text="No" Value="0" />
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                        <div id="DescGENESYS" style="display: none">
                            <div>El listado definitivo de asistentes se subira a APPIAN sin demora una vez concluida
                                    la actividad
                            </div>
                        </div>
                        <div class="contentFormGeneric">
                            <div class="contentForm eosUnoColumna">
                                <label id="lblGastosDesglose" class="eosCampoLabelSin"
                                    title="Concepto de los gastos y desglose de euros">
                                    Concepto de los gastos y desglose en euros (honorarios, alojamiento, inscripciones, desplazamiento, hospitalidad) (máx. 5000 caracteres)
                                        :<span class="eosCampoObligatorio">*</span></label>
                                <asp:ImageButton ID="btnExportarInfoExpediente" runat="server"
                                    ImageUrl="~/Styles/images/btn_ObtenerDelExpediente.png" OnClick="btnExportarInfoExpediente_Click" />
                                <asp:DropDownList ID="ddlExpedientesAmec" runat="server" CssClass="eosDisabledInputVacio eosInputVacio"
                                    DataTextField="idxpediente" DataValueField="idxpediente" Visible="false" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtgastoDesglose" runat="server" CssClass="eosDisabledInputVacio eosInputVacioGastos large-text-area not-margin"
                                    Height="129px" MaxLength="1000" TextMode="MultiLine"
                                    Visible="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Introducir Concepto de gastos y desglose."
                                    ControlToValidate="txtgastoDesglose" ValidationGroup="AMECValidationGroup" SetFocusOnError="True"
                                    ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="contentFormGeneric">
                            <div class="contentForm eosTresColumna">
                                <label id="lblImporteDaxas" class="eosCampoLabelSin">
                                    Importe con cargo a Producto:</label>
                                <asp:TextBox ID="txtCargoADaxas" runat="server" CssClass="eosDisabledInputVacio eosInputVacio "
                                    MaxLength="80" Visible="true"></asp:TextBox>
                            </div>
                            <div class="contentForm eosTresColumna">
                                <label id="lblImporteTotalGasto" class="eosCampoLabelSin" 
                                    title="Introduzca una cifra entera">
                                    Importe total del gasto (con IVA):<span class="eosCampoObligatorio">*</span></label>
                                <asp:TextBox ID="txtImporteTotalGasto" runat="server" CssClass="eosDisabledInputVacio eosInputVacio "
                                    MaxLength="80" Visible="true"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Importe Obligatorio."
                                    ControlToValidate="txtImporteTotalGasto" ValidationGroup="AMECValidationGroup"
                                    SetFocusOnError="True" ToolTip="Campo Obligatorio">&nbsp;</asp:RequiredFieldValidator>
                            </div>
                            <div class="contentForm eosTresColumna">
                                <label id="lblNumeroCartas" class="eosCampoLabelSin" 
                                    title="Indique el número (real o estimado) total de cartas contrato que esperamos recibir.">
                                    N. cartas contrato esperadas:</label>
                                <asp:TextBox ID="txtNumeroCartas" runat="server" CssClass="eosDisabledInputVacio eosInputVacio "
                                    MaxLength="80" Visible="true"></asp:TextBox>
                            </div>
                        </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="Panel9" runat="server">
        <div class="contentFormGeneric">
            <div class="eosTituloWizard radioCheck">
                <label>Incluir Documentación Adicional</label>
                    <asp:CheckBox ID="chkDocAdicional" onclick="javascript:docAdicional(this);" runat="server"
                        Text=" " />
            </div>
        </div>

        <% if (DocAdicional())
            { %>
        <div id="DocAdicional">
            <% }
                else
                { %>
            <div id="DocAdicional" style="display: none">
                <% } %>
                <div id="Div7" class="eosFiltroColor contentTableGeneric">
                    <div class="eosFiltroColorTabla">
                        <div style="text-align: center;">
                            <div id="Table4">
                                <asp:ListView ID="lvDocumentacion" runat="server" DataSourceID="odsDocumentacion" EnableViewState="false"
                                    EnablePersistedSelection="True" DataKeyNames="IDAMECS">
                                    <EmptyDataTemplate>
                                        <table style="margin-top: 20px" class="eosTablaResultados">
                                            <thead>
                                                <tr>
                                                    <th></th>
                                                    <th>TIPO
                                                    </th>
                                                    <th>USUARIO
                                                    </th>
                                                    <th>DOCUMENTACION
                                                    </th>
                                                    <th>FECHA
                                                    </th>
                                                    <th>COMENTARIO
                                                    </th>
                                                    <th>ADJUNTARAEMAIL
                                                    </th>
                                                    <th>CATEGORÍA
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td colspan="10">
                                                        <span class="eosTituloRojo">No se han encontrado documentos.</span>
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
                                                        <asp:LinkButton ID="lbTipo" runat="server" Text="TIPO" CommandName="Sort" CommandArgument="tipodoc"></asp:LinkButton>
                                                    </th>

                                                    <th>
                                                        <asp:LinkButton ID="lbUsuario" runat="server" Text="USUARIO" CommandName="Sort" CommandArgument="USUARIO"></asp:LinkButton>
                                                    </th>

                                                    <th>
                                                        <asp:LinkButton ID="lbDocumentacion" runat="server" Text="DOCUMENTACIÓN" CommandName="Sort"
                                                            CommandArgument="nombredoc"></asp:LinkButton>
                                                    </th>
                                                    <th>
                                                        <asp:LinkButton ID="lbFecha" runat="server" Text="FECHA" CommandName="Sort" CommandArgument="fechacreacion"></asp:LinkButton>
                                                    </th>
                                                    <th>
                                                        <asp:LinkButton ID="lbComentario" runat="server" Text="COMENTARIO" CommandName="Sort"
                                                            CommandArgument="comentariosdoc"></asp:LinkButton>
                                                    </th>
                                                    <th>
                                                        <asp:LinkButton ID="LbaAdjuntarAEmail" runat="server" Text="ADJUNTAR A EMAIL" CommandName="Sort"
                                                            CommandArgument="adjuntaraemail"></asp:LinkButton>
                                                    </th>
                                                    <th>
                                                        <asp:LinkButton ID="lbCategoria" runat="server" Text="CATEGORÍA" CommandName="Sort"
                                                            CommandArgument="CATEGORIA"></asp:LinkButton>
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
                                                        ShowPreviousPageButton="True" FirstPageText=" " PreviousPageText=" " PreviousPageImageUrl="Styles/images/ic_pagina_anterior.png"
                                                        FirstPageImageUrl="Styles/images/ic_pagina_primera.png" RenderDisabledButtonsAsLabels="True" />
                                                    <asp:NumericPagerField CurrentPageLabelCssClass="pageSel" />
                                                    <asp:NextPreviousPagerField ButtonType="Image" ShowLastPageButton="True" ShowNextPageButton="True"
                                                        ShowPreviousPageButton="False" LastPageText=" " NextPageText=" " LastPageImageUrl="Styles/images/ic_pagina_ultima.png"
                                                        NextPageImageUrl="Styles/images/ic_pagina_siguiente.png" RenderDisabledButtonsAsLabels="True" />
                                                </Fields>
                                            </asp:DataPager>
                                        </div>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td class="table-icon-container">
                                                <asp:ImageButton Text="Borrar" ID="imgDocumentacionEliminar" runat="server" ImageUrl="~/Styles/images/ic_participante_aspa.png"
                                                    CommandArgument='<%# Eval("iddocumentacion") %>' ToolTip="Anular Documentación"
                                                    OnCommand="imgAnularDocumentacion_Command" />
                                                <asp:ImageButton Text="Descargar" ID="ImgDescargarDoc" runat="server" ImageUrl="~/Styles/images/download.png"
                                                    ToolTip='<%# String.Format("Descargar Documento") %>' CommandName='<%# Eval("nombredoc") + "|" + Eval("categoriadocumento")%>'
                                                    OnCommand="imgDownloadDocumentacion_Command" />
                                            </td>
                                            <td class="par">
                                                <asp:Label ID="lbTipo" runat="server" Text='<%# Eval("tipodoc") %>' />
                                            </td>

                                            <td>
                                                <asp:Label ID="Label24" runat="server" Text='<%# Eval("nombreusuario") %>' />
                                            </td>
                                            <td class="par">
                                                <asp:Label ID="lbDocumentacion" runat="server" Text='<%# Eval("nombredoc") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbFecha" runat="server" Text='<%# Eval("fechacreacion") %>' />
                                            </td>
                                            <td class="par">
                                                <asp:Label ID="lbComentario" runat="server" Text='<%# Eval("comentariosdoc") %>' />
                                            </td>
                                            <td>
                                                <asp:Label ID="LbaAdjuntarAEmail" runat="server" Text='<%# (Eval("adjuntaraemail").ToString() == "True" ? "Sí" : "No") %>' />
                                            </td>
                                            <td class="par">
                                                <asp:Label ID="lbCategoria" runat="server" Text='<%# Eval("categoriadocumento") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                        <div id="tablas">
                            <div id="Table5" class="eosTablaFiltros">
                                <div class="eosTituloWizard">
                                    ADJUNTAR DOCUMENTOS
                                </div>
                                <div class="contentFormGeneric">
                                    <div class="contentForm eosMitadColumna">
                                        <label id="lblArchivoDocumento" class="eosCampoLabelSin" title="Archivo">Documento</label>
                                        <asp:FileUpload ID="fuploadDocumentacion" runat="server" />
                                    </div>
                                    <div class="contentForm eosMitadColumna">
                                        <label id="lblCategoriaDocumento" class="eosCampoLabelSin" title="Archivo">Categoría</label>
                                        <asp:DropDownList ID="ddlCategoriaDocumento" DataValueField="idcategoriadocumento" DataTextField="categoriadocumento" runat="server" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW360">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div id="chkAdjuntarFarma" style="display: none" class="contentFormGeneric">
                                    <div class="radioCheck">
                                        <label id="Label8" class="eosCampoLabelSin" title="Archivo">Adjuntar a Email para Farmaindutria</label>
                                        <asp:CheckBox ID="chkemail" runat="server" Text=" " />
                                    </div>
                                </div>
                                <div id="DescEnviarMails" style="display: none">
                                    <div colspan="2">
                                        <label id="Label4" class="eosCampoLabelSin">
                                            "Adjuntar a Email automático a departamento de Compliance/Farmaindustria (formularios
                                            Farmaindustria y otros documentos-contratos relevantes)"</label>
                                    </div>
                                </div>
                                <div class="contentFormGeneric">
                                    <div class="contentForm eosUnoColumna">
                                        <label id="Label6" class="eosCampoLabelSin">Comentarios</label>
                                        <asp:TextBox ID="txtComentarioDoc" Visible="true" TextMode="MultiLine" runat="server"
                                            MaxLength="500" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW360 large-text-area not-margin"></asp:TextBox>
                                    </div>
                                </div>
                                <div id="trGuardarDocumentacio" align="center" class="eosBotonera eosBotonFiltrado">
                                    <div>
                                        <asp:Button ID="btnAdjuntarDoc" runat="server" Text="Adjuntar documento"
                                            OnCommand="imgAdjuntarDocumentacion_Command" CommandName="Documentacion" OnClientClick="return CheckFileSize();" />
                                    </div>
                                    <div></div>
                                    <div></div>
                                    <div></div>
                                    <div></div>
                                </div>
                            </div>
                            <div class="">
                                    <div>
                                        <label id="Label16" class="eosCampoLabelSin"> Recuerda esta otra documentación que debes adjuntarfuera de EOS</label>
                                    </div>

                                <%--Eliminado por ticket http://support.quodem.com/support/staff/index.php?_m=tickets&_a=viewticket&ticketid=55083--%>

                                <%--<tr>   
                                    <td style="border:0">
                                    <label id="Label17" class="eosCampoLabelSin" style="padding-left: 10px; color:#424242" >
                                            -Listado asistentes final (archivo en APPIAN)</label>
                                    </td>
                                </tr>--%>
                                <div>
                                    <label id="Label18" class="eosCampoLabelSin">-Convenio de Colaboración con Instituciones (visado por legal)</label>
                                </div>
                                <div>
                                    <label id="Label19" class="eosCampoLabelSin">-Contrato Consultores (visado por legal)</label>
                                </div>
                                <div>
                                    <label id="Label20" class="eosCampoLabelSin">-Contrato Advisory Boards (visado por legal)</label>
                                </div>
                                <div>
                                    <label id="Label21" class="eosCampoLabelSin">-Contrato Ponente (visado por legal)</label>
                                </div>
                                <div>
                                    <label id="Label23" class="eosCampoLabelSin">-Minutas de las reuniones de Asesores (Archivo Unidad de Negocio)</label>
                                </div>
                            </div>
                            <div style="clear: both;"></div>
                        </div>

                    </div>
                </div>
            </div>
            <br />
    </asp:Panel>
    <asp:Panel ID="plAMEC_Comentarios" runat="server" Visible="false">
        <div class="eosTituloWizard">
            COMENTARIOS
        </div>
        <div id="Div8" class="eosFiltroColor contentTableGeneric">
            <div class="eosFiltroColorTabla">
                <table id="Table7" class="eosTablaFiltros">
                    <asp:ListView ID="lvComentarios" runat="server" DataSourceID="odsComentariosAMEC" EnableViewState="false"
                        EnablePersistedSelection="True" DataKeyNames="IDAMECS">
                        <EmptyDataTemplate>
                            <table style="margin-top: 20px" class="eosTablaResultados">
                                <thead>
                                    <tr>
                                        <th></th>
                                        <th>USUARIO
                                        </th>
                                        <th>FECHA
                                        </th>
                                        <th>COMENTARIO
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td colspan="10">
                                            <span class="eosTituloRojo">No se han encontrado comentarios.</span>
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
                                            <asp:LinkButton ID="lbUsuario" runat="server" Text="USUARIO" CommandName="Sort" CommandArgument="USUARIO"></asp:LinkButton>
                                        </th>
                                        <th>
                                            <asp:LinkButton ID="lbFechaComentario" runat="server" Text="FECHA" CommandName="Sort"
                                                CommandArgument="FECHA"></asp:LinkButton>
                                        </th>
                                        <th>
                                            <asp:LinkButton ID="lbComentarioComentario" runat="server" Text="COMENTARIO" CommandName="Sort"
                                                CommandArgument="COMENTARIO"></asp:LinkButton>
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
                                            ShowPreviousPageButton="True" FirstPageText=" " PreviousPageText=" " PreviousPageImageUrl="Styles/images/ic_pagina_anterior.png"
                                            FirstPageImageUrl="Styles/images/ic_pagina_primera.png" RenderDisabledButtonsAsLabels="True" />
                                        <asp:NumericPagerField CurrentPageLabelCssClass="pageSel" />
                                        <asp:NextPreviousPagerField ButtonType="Image" ShowLastPageButton="True" ShowNextPageButton="True"
                                            ShowPreviousPageButton="False" LastPageText=" " NextPageText=" " LastPageImageUrl="Styles/images/ic_pagina_ultima.png"
                                            NextPageImageUrl="Styles/images/ic_pagina_siguiente.png" RenderDisabledButtonsAsLabels="True" />
                                    </Fields>
                                </asp:DataPager>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:ImageButton Text="Borrar" ID="imgComentarioEliminar" runat="server" ImageUrl="~/Styles/images/ic_participante_aspa.png"
                                        CommandArgument='<%# Eval("idcomentarios") %>' ToolTip="Anular Comentario" OnCommand="imgComentarioEliminar_Command" />
                                </td>
                                <td class="par">
                                    <asp:Label ID="lbUsuario" runat="server" Text='<%# Eval("nombreusuario") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lbFechaComentario" runat="server" Text='<%# Eval("fechacreacion") %>' />
                                </td>
                                <td class="par">
                                    <asp:Label ID="lbComentarioComentario" runat="server" Text='<%# Eval("comentariosdoc") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </table>
                <div class="contentFormGeneric">
                    <div class="contentForm eosUnoColumna">
                        <label id="lblComentario" class="eosCampoLabelSin">
                            Comentario</label>
                        <asp:TextBox ID="txtComentario" Visible="true" TextMode="MultiLine" runat="server"
                            MaxLength="200" CssClass="eosDisabledInputVacio eosInputVacio eosSizeW220 large-text-area not-margin bottom-space"></asp:TextBox>
                    </div>
                    <div>
                        <asp:ImageButton ID="btnPublicarComentario" runat="server"
                            ImageUrl="~/Styles/images/bt_publicar_comentario.png" OnClick="btnPublicarComentario_Click" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="plAMEC_Historial" runat="server">
        <div class="eosTituloWizard">
            HISTORIAL DE ESTADOS
        </div>
        <div id="Div9" class="eosFiltroColor">
            <div class="eosFiltroColorTabla contentTableGeneric">
                <table id="Table9" class="eosTablaFiltros">
                    <asp:ListView ID="lvFlujo" runat="server" DataSourceID="odsHistEstadosAMEC" EnablePersistedSelection="True" EnableViewState="false"
                        DataKeyNames="idestadoamechist">
                        <EmptyDataTemplate>
                            <table style="margin-top: 20px" class="eosTablaResultados">
                                <thead>
                                    <tr>
                                        <th>ACCIÓN
                                        </th>
                                        <th>USUARIO
                                        </th>
                                        <th>CARGO
                                        </th>
                                        <th>FECHA ÚLTIMA ACCIÓN
                                        </th>
                                        <th>ESTADO
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td colspan="10">
                                            <span class="eosTituloRojo">No se han encontrado registros.</span>
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
                                            <asp:LinkButton ID="lbAccion" runat="server" Text="ACCION" CommandName="Sort" CommandArgument="ACCIÓN"></asp:LinkButton>
                                        </th>
                                        <th>
                                            <asp:LinkButton ID="lbUsuario" runat="server" Text="USUARIO" CommandName="Sort" CommandArgument="USUARIO"></asp:LinkButton>
                                        </th>
                                        <th>
                                            <asp:LinkButton ID="lbCargo" runat="server" Text="CARGO" CommandName="Sort" CommandArgument="CARGO"></asp:LinkButton>
                                        </th>
                                        <th>
                                            <asp:LinkButton ID="lbFechaUltimaAccion" runat="server" Text="FECHA_ÚLTIMA_ACCIÓN"
                                                CommandName="Sort" CommandArgument="FECHAULTIMAACCION"></asp:LinkButton>
                                        </th>
                                        <th>
                                            <asp:LinkButton ID="lbEstado" runat="server" Text="ESTADO" CommandName="Sort" CommandArgument="ESTADO"></asp:LinkButton>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                </tbody>
                            </table>
                            <div class="contentPagination">
                                <asp:DataPager ID="DataPager3" runat="server" PageSize="20">
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
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="par">
                                    <asp:Label ID="lbAccion" runat="server" Text='<%# Eval("nivelaprobacion") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lbUsuario" runat="server" Text='<%# Eval("nombreusuario") %>' />
                                </td>
                                <td class="par">
                                    <asp:Label ID="lbCargo" runat="server" Text='<%# Eval("cargo") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="lbFechaUltimaAccion" runat="server" Text='<%# Eval("fechacreacion") %>' />
                                </td>
                                <td class="par" style="<%# Eval("nombreusuariopendiente") == "" ? Eval("idestado").ToString() == EOS.Web.Enums.EstadosAmec.PendienteNegocio.GetHashCode().ToString() ? "color: red": "": "" %>">
                                    <asp:Label ID="lbEstado" runat="server" Text='<%# Eval("estado") %>' />&nbsp;<asp:Label ID="Label14" runat="server" Text='<%# Eval("nombreusuariopendiente") == "" ? Eval("idestado").ToString() == EOS.Web.Enums.EstadosAmec.PendienteNegocio.GetHashCode().ToString() ? "(Falta Sup. Jerárquico en la estructura)": "": "(" + Eval("nombreusuariopendiente") + ")" %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </table>
            </div>
        </div>
    </asp:Panel>
    <div>
        <div>
            <div>
                <div class="eosBotonera eosBotonFiltrado">
                    <asp:Button ID="btnGuardar" Visible="false" runat="server" Text="Guardar"
                        OnClick="btnGuardar_Click" ValidationGroup="AMECValidationGroupBotonGuardar" />
                    <asp:Button ID="btnAprobar" runat="server" Text="Aprobar"
                        ClientIDMode="Static" OnClientClick="btnAprobarDisable();" OnClick="btnAprobar_Click" ValidationGroup="AMECValidationGroup" />
                    <input type="image" id="btnAprobarHid" class="hidden" src="Styles/images/bt_aprobar.png" disabled="disabled" />
                    <asp:ImageButton ID="btnAprobarConCondicion" Visible="false" runat="server" ImageUrl="~/Styles/images/btn_AprobarCondicionado.png"
                        OnClick="btnAprobarConCondicion_Click" ValidationGroup="AMECValidationGroup"
                        OnClientClick="return ConfirmarAprobacion();" />
                    <asp:ImageButton ID="btnRechazar" Visible="false" runat="server" ImageUrl="~/Styles/images/bt_rechazar.png"
                        OnClick="btnRechazar_Click" ValidationGroup="AMECValidationGroup" OnClientClick="return ConfirmarRechazar();" />
                    <asp:ImageButton ID="btnCancelar" Visible="false" runat="server" ImageUrl="~/Styles/images/bt_cancelar.png"
                        OnClick="btnCancelar_Click" ValidationGroup="AMECValidationGroup" OnClientClick="return ConfirmarCancelacion();" />
                    <!--<asp:Button ID="btnVolver" Visible="false" runat="server" Text="Volver"
                        OnClick="btnVolver_Click" />-->
                    <asp:ImageButton ID="btnSometer" Visible="false" runat="server" ImageUrl="~/Styles/images/btn_Someter.png"
                        OnClick="btnSometer_Click" ValidationGroup="AMECValidationGroup" />
                </div>
            </div>
        </div>
        <div>
            <div class="contentFormGeneric separator">
                <div class="excell-icon-container">
                    <asp:ImageButton ID="btnInformeAdicional" runat="server" ImageUrl="~/Styles/images/iconExcel.png"
                        OnClick="btnInformeAdicional_Click" class="icon-not-boder"/>
                    <asp:Label ID="Label5" runat="server">Informe Documentación Adicional</asp:Label>
                </div>
                <div class="excell-icon-container">
                    <asp:ImageButton ID="btnVerExcel" runat="server" ImageUrl="~/Styles/images/iconExcel.png"
                             OnClick="btnVerExcel_Click" class="icon-not-boder"/>
                    <asp:Label ID="Label13" runat="server">Descargar AMEC</asp:Label>
                </div>                
            </div>
            <div>&nbsp;&nbsp;&nbsp;&nbsp;
            </div>
        </div>
    </div>
    <div>
        <div>
            <div>&nbsp;</div>
        </div>
        <div>
            <div class="eosLink">
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
        </div>
    </div>
    <asp:ObjectDataSource ID="odsSupJerarquico" runat="server" SelectMethod="ObtenerManagersList" InsertMethod="InsertarUsuarioEnLista"
        TypeName="EOS.Web.AgentePeticionarioManager" EnablePaging="True" SortParameterName="sortParameter"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsComentariosAMEC" runat="server" SelectMethod="ObtenerComentariosAMEC"
        TypeName="EOS.Web.AgenteComentariosAMEC" SelectCountMethod="ObtenerNumeroComentariosAMEC"
        EnablePaging="True" OnSelecting="odsComentariosAMEC_Selecting" SortParameterName="sortParameter">
        <SelectParameters>
            <asp:Parameter Name="filtroidamec" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsHistEstadosAMEC" runat="server" SelectMethod="ObtenerHistorialEstadosAMEC"
        TypeName="EOS.Web.AgenteAmecInfo" SelectCountMethod="ObtenerNumeroHistorialEstadosAMEC"
        EnablePaging="True" OnSelecting="odsHistEstadosAMEC_Selecting" SortParameterName="sortParameter">
        <SelectParameters>
            <asp:Parameter Name="filtroidamec" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsDocumentacion" runat="server" SelectMethod="ObtenerDocumentacionAMEC"
        TypeName="EOS.Web.AgenteAmecInfo" SelectCountMethod="ObtenerNumeroDocumentacionAMEC"
        EnablePaging="True" OnSelecting="odsDocumentacionAMEC_Selecting" SortParameterName="sortParameter">
        <SelectParameters>
            <asp:Parameter Name="filtroidamec" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsProgramaAMEC" runat="server" SelectMethod="ObtenerProgramaAMEC"
        TypeName="EOS.Web.AgenteAmecInfo" SelectCountMethod="ObtenerNumeroProgramaAMEC"
        EnablePaging="True" OnSelecting="odsProgramaAMEC_Selecting" SortParameterName="sortParameter">
        <SelectParameters>
            <asp:Parameter Name="filtroidamec" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
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