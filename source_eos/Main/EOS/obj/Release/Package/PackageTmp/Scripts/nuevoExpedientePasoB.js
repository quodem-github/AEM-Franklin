$(function () {
    //     eura plugin para mostrar la información cada vez que se selecciona un profesional.
    $('.eura').eura({
        //text: 'Certifico la revisión del nivel de riesgo de los profesionales sanitarios incluidos en esta actividad, con respecto a la misma según lo establecido en Pol.Corp nº 20/FCPA.',
        text: 'Certifico la revisión del nivel de riesgo de los profesionales sanitarios incluidos en esta actividad, con respecto a la misma según lo establecido en C.Pol 5/FCPA.',
        action: function (e) {
            // Llamar al método __doPostBack "fantasma" de asp.net para realizar la acción del control, si existe.
            if (__doPostBack) {
                __doPostBack(e.currentTarget.attributes.name.nodeValue, '');
            }
        }
    });
});

//$(function () {
//    //     eura plugin para mostrar la información cada vez que se selecciona un profesional.

//    $('.eurahonorarios').eura({
//        text: 'Certifico la revisión del nivel de riesgo BAJO de los profesionales sanitarios incluidos en esta actividad, con respecto a la misma según lo establecido en Pol. Corp. Nº 20/FCPA. \nEn caso de riesgo Medio/Alto, se tramitará la solicitud de FCPA en Axentis.',
//        text: 'Certifico la revisión del nivel de riesgo BAJO de los profesionales sanitarios incluidos en esta actividad, con respecto a la misma según lo establecido en C.Pol 5/FCPA. \nEn caso de riesgo Medio/Alto, se tramitará la solicitud de FCPA en Axentis.',
//        action: function (e) {
//            // Llamar al método __doPostBack "fantasma" de asp.net para realizar la acción del control, si existe.
//            if (__doPostBack) {
//                __doPostBack(e.currentTarget.attributes.name.nodeValue, '');
//            }
//        }
//    });
//});


function DisableValidator(idvalidator) {
    if (typeof Page_Validators != 'undefined') {
        for (i = 0; i <= Page_Validators.length; i++) {
            if (Page_Validators[i] != null && Page_Validators[i].id == idvalidator) {
                Page_Validators[i].enabled = false;
                Page_Validators[i].disabled = true;
//                Page_Validators[i].
            }
        }
    };
}

function EnableValidator(idvalidator) {
    if (typeof Page_Validators != 'undefined') {
        for (i = 0; i <= Page_Validators.length; i++) {
            if (Page_Validators[i] != null && Page_Validators[i].id == idvalidator) {
                Page_Validators[i].enabled = true;
                Page_Validators[i].disabled = false;
            }
        }
    };
}

$(document).ready(function () {

    var mensaje = " ";
    init();

    function init() {
        $('#eosContentFilter_btnGuardarDatosUsuarioConEura').hide();
        $('#eosContentFilter_btnGuardarDatosUsuario').show();
        $('#eosContentFilter_ddlTipoActividadPax').val(' ');
        $('#eosContentFilter_ddlTipoAsistente').val(' ');
        $("#eosContentFilter_ddljustificaciones").val(' ');
        $("#eosContentFilter_txthonorarios").val('');
        $("#eosContentFilter_txtPagoSociedadOtros").val(''); 
        $("#eosContentFilter_rblpagodirecto_0").removeAttr("checked");
        $("#eosContentFilter_rblpagodirecto_1").removeAttr("checked");
        $("#eosContentFilter_rbficheroGenesis_0").removeAttr("checked");
        $("#eosContentFilter_rbficheroGenesis_1").removeAttr("checked");
        $("#eosContentFilter_ContenHonorarioValidationSummary").html("");
    }

    $.getMensaje = function (idrisklevel, idtipoasistente, idtipoactividad) {
        if (idrisklevel != " " && idtipoasistente != " " && idtipoactividad != " ") {
            $('#eosContentFilter_ajaxImage').attr("style", "visibility: visible");
            $.ajax({
                type: "POST",
                url: "NuevoExpedientePasoB.aspx/obtenerMensaje",
                data: '{ idrisk:' + idrisklevel + ',idtipoasis:' + idtipoasistente + ', idtipoact: ' + idtipoactividad + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function () {
                    alert("Error: ");
                },
                success: function (result) {
                    $('#eosContentFilter_ajaxImage').attr("style", "visibility: hidden");
                    var btn = $('#eosContentFilter_btnGuardarDatosUsuarioConEura');
                    $('#eosContentFilter_btnGuardarDatosUsuarioConEura').clone().insertAfter(btn);
                    $('#eosContentFilter_btnGuardarDatosUsuarioConEura').remove();
                    mensaje = result.d;
                    if (mensaje == 'NULL' || mensaje == ' ') {
                        $('#eosContentFilter_btnGuardarDatosUsuarioConEura').hide();
                        $('#eosContentFilter_btnGuardarDatosUsuario').show();
                    } else {
                        if ($('#eosContentFilter_inputRiskLevel').val() == "1") {
                            $('#eosContentFilter_btnGuardarDatosUsuarioConEura').hide();
                            $('#eosContentFilter_btnGuardarDatosUsuario').show();
                        } else {
                            $('#eosContentFilter_btnGuardarDatosUsuarioConEura').show();
                            $('#eosContentFilter_btnGuardarDatosUsuario').hide();
                        }
                    }
                    $('.eurahonorarios').eura({
                        text: result.d,
                        action: function (e) {
                            // Llamar al método __doPostBack "fantasma" de asp.net para realizar la acción del control, si existe.

                            if (__doPostBack) {
                                __doPostBack(e.currentTarget.attributes.name.nodeValue, '');
                            }
                        }
                    });
                }
            });
        }
    };

    $("img#imgParticipanteView").click(function (event) {

        $("#eosContentFilter_idModuloParticipantes input").prop('disabled', true);
        $("#eosContentFilter_idModuloParticipantes select").prop('disabled', true);
        $("#eosContentFilter_idModuloParticipantes textarea").prop('disabled', true);
        $("#eosContentFilter_btnCancelarDatosUsuario").prop('disabled', false);

        event.preventDefault();

        $('#eosContentFilter_btnGuardarDatosUsuarioConEura').hide();
        $('#eosContentFilter_btnGuardarDatosUsuario').hide();

        var data = JSON.parse($('#hiddenParticipanteObj_' + $(this).attr('alt')).val());
        var datanivelps = JSON.parse($('#hiddenPonentesNivelPSList').val());

        var ponenteFeeHora = $.grep(datanivelps, function (n, i) {
            return n.id == data.ponentes_nivelps;
        });
        if (data.tiporeunion == 1) {
            $('#idPlhTipoPonente_1').show();
            $('#idPlhTipoPonente_2').show();
        } else {
            $('#idPlhTipoPonente_1').hide();
            $('#idPlhTipoPonente_2').hide();
        }
        $('#eosContentFilter_ddlTipoActividadPax').val(data.idtipoactividadpax);
        $('#eosContentFilter_txtRiskLevel').val(data.nivelriesgo);
        $('#eosContentFilter_ddlTipoAsistente').val(data.idtipoasistente);
        $("#eosContentFilter_ddljustificaciones").val(data.justificaciones);
        $("#eosContentFilter_txthonorarios").val(data.honorarios);
        $("#eosContentFilter_txtPagoSociedadOtros").val(data.pagosociedad);
        $('#eosContentFilter_rblpagodirecto input[value="' + data.pagodirecto + '"]').prop('checked', true);
        $('#eosContentFilter_rbficheroGenesis input[value="' + data.ficherogenesis + '"]').prop('checked', true);
        $('#eosContentFilter_ddlSpeakerChairTipoReunion').val(data.tiporeunion);
        $('#eosContentFilter_rblTipoPonente input[value="' + data.tipo_ponente + '"]').prop('checked', true);
        $('#eosContentFilter_ddlSpeakerChairTipoPS').val(data.ponentes_nivelps);
        if (ponenteFeeHora != undefined && ponenteFeeHora.length == 1) {
            $('#eosContentFilter_txtSpeakerChairFeeHora').val(ponenteFeeHora[0].Value);
        }
        $('#eosContentFilter_ddlSpeakerChairDuracionActividad').val(data.ponentes_duracionactividad); 
        $('#eosContentFilter_ddlSpeakerChairTiempoPreparacion').val(data.ponentes_preparacion); 
        $('#eosContentFilter_chkBoxPonenciaCentroSalud').attr('checked', data.ponencia_centro_salud == 1);
        $('#eosContentFilter_chkBoxTalleres').attr('checked', data.talleres == 1); 
        $('#eosContentFilter_chkBoxVideoconferenciasRepetidas').attr('checked', data.videoconferencia_repetida == 1); 
        $('#eosContentFilter_txtSpeakerChairHonorariosMaximos').val(data.ponentes_honorariosmaximos);
        $('#eosContentFilter_rblConsultoriaTipoReunion  input[value="' + data.idTipoContratoConsultoria + '"]').prop('checked', true);
        if (data.idTipoContratoConsultoria == 1) {
            $('#idtrCalculadora_Consultoria_numdias').show();
            $('#eosContentFilter_txtConsultoriaHonorariosMaximos').val(data.FloatMinValue * data.numero_dias_consultoria);
        } else {
            $('#eosContentFilter_txtConsultoriaHonorariosMaximos').val(data.FloatMinValue);
            $('#idtrCalculadora_Consultoria_numdias').hide();
        }
        $('#eosContentFilter_txtConsultoriaDiaConsultor').val(data.numero_dias_consultoria);
        $('#eosContentFilter_ddlEifABTipoPS').val(data.abeif_nivelps);
        //$('#eosContentFilter_txtEifABFeeHora').val('');
        $('#eosContentFilter_ddlEifABDuracionActividad').val(data.abeif_duracionactividad); 
        $('#eosContentFilter_ddlEifABTiempoPreparacion').val(data.abeif_preparacion); 
        $('#eosContentFilter_txtEifABHonorariosMaximos').val(data.abeif_honorariosmaximos);
        $('#eosContentFilter_txtJustificacion').val(data.justificacion);

        setVisibilityForm();

        //Deshabilitar tots els elements de dintre del div.
        $('#eosContentFilter_plPaso3_Ind').find('*').prop('disabled', true);

        if ($.browser != undefined && $.browser.msie && parseInt($.browser.version) < 9)
            $("#eosContentFilter_idModuloParticipantes").addClass("moduloParticipantesIE8");
        else
            $("#eosContentFilter_idModuloParticipantes").addClass("moduloParticipantes");

        $("#eosContentFilter_idModuloParticipantes").show(1000);

        $('#Participante').val($(this).attr('title'));
    });

    //Popup que se muestra de datos Adicionales al adjuntar un participante
    $("input[id^='eosContentFilter_lvParticipantes_idimgparticipantes'], input[id^='eosContentFilter_lvParticipantesVeeva_idimgparticipantesVeeva']").click(function (event) {

        event.preventDefault();
        var arr = ($(this).attr('title')).split(' ');
        $('#eosContentFilter_ddlRiskLevel').val(arr[8]);
        //        alert($('#eosContentFilter_ddlRiskLevel').val());

        $('#eosContentFilter_txtRiskLevel').val($("#eosContentFilter_ddlRiskLevel option:selected").text());
        $('#eosContentFilter_inputRiskLevel').val(arr[8]);
        if ($('#eosContentFilter_inputRiskLevel').val() == "1") {
            $('#eosContentFilter_btnGuardarDatosUsuarioConEura').hide();
            $('#eosContentFilter_btnGuardarDatosUsuario').show();
        } else {
            $('#eosContentFilter_btnGuardarDatosUsuarioConEura').show();
            $('#eosContentFilter_btnGuardarDatosUsuario').hide();
        }
        //Deshabilitar tots els elements de dintre del div.
        $('#eosContentFilter_plPaso3_Ind').find('*').prop('disabled', true);
        if ($.browser != undefined && $.browser.msie && parseInt($.browser.version) < 9)
            $("#eosContentFilter_idModuloParticipantes").addClass("moduloParticipantesIE8");
        else
            $("#eosContentFilter_idModuloParticipantes").addClass("moduloParticipantes");

        $("#eosContentFilter_idModuloParticipantes").show(1000);
        $("#popupBackground").show();


        $('#Participante').val($(this).attr('title'));
    });

    $("a[id^='link_tabla_honorarios_maximos']").click(function (event) {

        event.preventDefault();
        if ($.browser != undefined && $.browser.msie && parseInt($.browser.version) < 9)
            $("#eosContentFilter_idHonorariosMaximos").addClass("moduloParticipantesIE8");
        else
            $("#eosContentFilter_idHonorariosMaximos").addClass("moduloParticipantes");

        $("#eosContentFilter_idHonorariosMaximos").show(1000);
    });

    $("a[id^='link_tabla_honorarios_country_to_country']").click(function (event) {

        event.preventDefault();
        if ($.browser != undefined && $.browser.msie && parseInt($.browser.version) < 9)
            $("#eosContentFilter_idCountryToCountry").addClass("moduloParticipantesIE8");
        else
            $("#eosContentFilter_idCountryToCountry").addClass("moduloParticipantes");

        $("#eosContentFilter_idCountryToCountry").show(1000);
    });

    function cerrarModalHonorariosMaximos() {
        $("#eosContentFilter_idHonorariosMaximos").hide();
    }

    function setVisibilityForm() {
        //if ($("#eosContentFilter_ddlTipoAsistente").val() == 1 || $("#eosContentFilter_ddlTipoAsistente").val() == 2 || $("#eosContentFilter_ddlTipoAsistente").val() == 3) {
        if ($("#eosContentFilter_ddlTipoAsistente").val() == 1 || $("#eosContentFilter_ddlTipoAsistente").val() == 2 || $("#eosContentFilter_ddlTipoAsistente").val() == 3 || $("#eosContentFilter_ddlTipoAsistente").val() == 6 || $("#eosContentFilter_ddlTipoAsistente").val() == 7 || $("#eosContentFilter_ddlTipoAsistente").val() == 8 || $("#eosContentFilter_ddlTipoAsistente").val() == 9 || $("#eosContentFilter_ddlTipoAsistente").val() == 10) {
            if ($("#eosContentFilter_hiddenInternacional").val() == "1") {
                //Cuando el evento que tiene el Expediente NO es internacional no se tienen que mostrar las justificaciones
                //Cuando un evento no es internacional el selectedvalue del ddjustificaciones = ""
                $("#idtrjustificaciones").show();
            }
            if ($("#eosContentFilter_ddlTipoAsistente").val() == 2 || $("#eosContentFilter_ddlTipoAsistente").val() == 3 || $("#eosContentFilter_ddlTipoAsistente").val() == 6 || $("#eosContentFilter_ddlTipoAsistente").val() == 7 || $("#eosContentFilter_ddlTipoAsistente").val() == 8 || $("#eosContentFilter_ddlTipoAsistente").val() == 9 || $("#eosContentFilter_ddlTipoAsistente").val() == 10) {
                //Cuando tipoAsistente = 2 se mostrarán los honorarios y el radiobuton de pago directo
                $("#idtrhonorarios_1").show();
                $("#idtrhonorarios_2").show();
                $("#idtrhonorarios_3").show();
                $("#idtrpagodirecto_1").show();
                $("#idtrpagodirecto_2").show();
                if ($("#eosContentFilter_rblpagodirecto_0").is(":checked"))
                //Si el radiobuton de pago directo es SI también se mostrará el Nombre de pago..
                    $("#idtrpagofundacionotros").show();
            } else {
                //Si tipoAsistente es igual a 1 solo se mostrará el apartado justificaciones y solo en el caso de que el evento sea internacional
                $("#idtrhonorarios_1").hide();
                $("#idtrhonorarios_2").hide();
                $("#idtrhonorarios_3").hide();
                $("#idtrpagodirecto_1").hide();
                $("#idtrpagodirecto_2").hide();
                $("#idtrpagofundacionotros").hide();
            }
        } else {
            //Si tipoAsistente es diferente de 1, 2, 3, 6, 7, 8, 9 y 10 no se mostrarán ningún apartado 
            $("#idtrjustificaciones").hide();
            $("#idtrhonorarios_1").hide();
            $("#idtrhonorarios_2").hide();
            $("#idtrhonorarios_3").hide();
            $("#idtrpagodirecto_1").hide();
            $("#idtrpagodirecto_2").hide();
            $("#idtrpagofundacionotros").hide();
        }

        //Calculadora Ponentes
        if ($("#eosContentFilter_ddlTipoAsistente").val() == 2 || $("#eosContentFilter_ddlTipoAsistente").val() == 3 || $("#eosContentFilter_ddlTipoAsistente").val() == 6 || $("#eosContentFilter_ddlTipoAsistente").val() == 9) {
            $("#idtrCalculadora_Speaker_Chair").show();
            $("#idtrCalculadora_Speaker_Chair_Contenido").show();
            $("#idtrCalculadora_Speaker_Chair_Cierre").show();

            EnableValidator("eosContentFilter_rfvddlSpeakerChairTipoReunion");
            EnableValidator("eosContentFilter_rfvddlSpeakerChairTipoPS");
            EnableValidator("eosContentFilter_rfvddlSpeakerChairDuracionActividad");
            EnableValidator("eosContentFilter_rfvddlSpeakerChairTiempoPreparacion");

        } else {
            $("#idtrCalculadora_Speaker_Chair").hide();
            $("#idtrCalculadora_Speaker_Chair_Contenido").hide();
            $("#idtrCalculadora_Speaker_Chair_Cierre").hide();

            DisableValidator("eosContentFilter_rfvddlSpeakerChairTipoReunion");
            DisableValidator("eosContentFilter_rfvddlSpeakerChairTipoPS");
            DisableValidator("eosContentFilter_rfvddlSpeakerChairDuracionActividad");
            DisableValidator("eosContentFilter_rfvddlSpeakerChairTiempoPreparacion");

        }
        //Fin Calculadora Ponentes

        //Calculadora Consultoria
        if ($("#eosContentFilter_ddlTipoAsistente").val() == 10) {
            $("#idtrCalculadora_Consultoria").show();
            $("#idtrCalculadora_Consultoria_Container").show();
            $("#idtrCalculadora_Consultoria_Contenido").show();
            $("#idtrCalculadora_Consultoria_Cierre").show();
        } else {
            $("#idtrCalculadora_Consultoria").hide();
            $("#idtrCalculadora_Consultoria_Container").hide();
            $("#idtrCalculadora_Consultoria_Contenido").hide();
            $("#idtrCalculadora_Consultoria_Cierre").hide();
        }
        //Fin Calculadora Consultoria

        //Calculadora EIF & Advisory Boards
        if ($("#eosContentFilter_ddlTipoAsistente").val() == 7 || $("#eosContentFilter_ddlTipoAsistente").val() == 8) {
            $("div[id^='idtrCalculadora_Eif_AB_Comment_']").hide();
            $('#idtrCalculadora_Eif_AB_a_hide').hide();
            $('#idtrCalculadora_Eif_AB_a_show').show();
            $("#idtrCalculadora_Eif_AB").show();
            $("#idtrCalculadora_Eif_AB_container").show();
            $("#idtrCalculadora_Eif_AB_Contenido").show();
            $("#idtrCalculadora_Eif_AB_Cierre").show();

            EnableValidator("eosContentFilter_rfvddlEifABDuracionActividad");
            EnableValidator("eosContentFilter_rfvddlEifABTiempoPreparacion");

        } else {
            $("#idtrCalculadora_Eif_AB").hide();
            $("#idtrCalculadora_Eif_AB_container").hide();
            $("#idtrCalculadora_Eif_AB_Contenido").hide();
            $("#idtrCalculadora_Eif_AB_Cierre").hide();

            DisableValidator("eosContentFilter_rfvddlEifABDuracionActividad");
            DisableValidator("eosContentFilter_rfvddlEifABTiempoPreparacion");

        }

        if ($("#eosContentFilter_ddlTipoAsistente").val() == 4) {
            $("#idtrtipoactividad").hide();
            $("#idtrpertenece").hide();
        } else {
            $("#idtrpertenece").show();
            $("#idtrtipoactividad").show();
        }
    }

    //$("[id^='eosContentFilter_ddlTipoAsistenteinput']").change(function (event) {
    $("#eosContentFilter_ddlTipoAsistente").change(function (event) {
        $.getMensaje($('#eosContentFilter_ddlRiskLevel').val(), $("#eosContentFilter_ddlTipoAsistente").val(), $("#eosContentFilter_ddlTipoActividadPax").val());

        setVisibilityForm();

        CleanFormFields();
        //Fin Calculadora EIF & Advisory Boards

        if ($("#eosContentFilter_ddlTipoAsistente").val() == 4) {
            //Si el tipo de asistente = 4 entonces no se muestra el mensaje de aceptar política y tampoco se muestra el de pertenece a Genesis

            DisableValidator("eosContentFilter_rfdtxtjustificaciones");
            DisableValidator("eosContentFilter_rfvddlTipoActividadPax");
            DisableValidator("eosContentFilter_rfdtxtficheroGenesis");

            $("#idtrtipoactividad").hide();
            $('#eosContentFilter_btnGuardarDatosUsuarioConEura').hide();
            $('#eosContentFilter_btnGuardarDatosUsuario').show();
            $("#idtrpertenece").hide();
        } else {
            if ($("#eosContentFilter_ddlTipoAsistente").val() == 5)
                DisableValidator("eosContentFilter_rfdtxtjustificaciones");
            else {
                if ($("#eosContentFilter_hiddenInternacional").val() == "1") {
                    EnableValidator("eosContentFilter_rfdtxtjustificaciones");
                } else {
                    DisableValidator("eosContentFilter_rfdtxtjustificaciones");
                }
            }

            EnableValidator("eosContentFilter_rfdtxtficheroGenesis");
            EnableValidator("eosContentFilter_rfvddlTipoActividadPax");
            $("#idtrpertenece").show();
            $("#idtrtipoactividad").show();

        }
    });

    $("#eosContentFilter_ddlTipoActividadPax").change(function(event) {
        $.getMensaje($('#eosContentFilter_ddlRiskLevel').val(), $("#eosContentFilter_ddlTipoAsistente").val(), $("#eosContentFilter_ddlTipoActividadPax").val());
    });

    $(':radio[id="eosContentFilter_rblpagodirecto_1"]').click(function () {
        $("#idtrpagofundacionotros").hide();
    });
    $(':radio[id="eosContentFilter_rblpagodirecto_0"]').click(function () {
        $("#idtrpagofundacionotros").show();
    });

    $('#eosContentFilter_txthonorarios').keyup(function () {
        ValidateHonorariosAndJustificaciones(mensaje);
    });

    $('#eosContentFilter_txtJustificacion').keyup(function () {
        ValidateHonorariosAndJustificaciones(mensaje);
    });
});

function ValidateHonorariosAndJustificaciones(mensaje) {
    var valHonorariosMaximos = ValidateHonorariosMaximos();
    var valJustificacion = validateTxtJustificacion();
    if (valHonorariosMaximos && valJustificacion) {
        if (mensaje == 'NULL' || mensaje == ' ') {
            $('#eosContentFilter_btnGuardarDatosUsuarioConEura').hide();
            $('#eosContentFilter_btnGuardarDatosUsuario').show();
        } else {
            $('#eosContentFilter_btnGuardarDatosUsuarioConEura').show();
            $('#eosContentFilter_btnGuardarDatosUsuario').hide();
        }
        $('#errorMaximoHonorario').hide();
        $('#errorHonorarioJustificacion').hide();
    } else {
        if (!valHonorariosMaximos) {
            $('#eosContentFilter_btnGuardarDatosUsuarioConEura').hide();
            $('#eosContentFilter_btnGuardarDatosUsuario').hide();
            $('#errorMaximoHonorario').show();
        }
        if (!valJustificacion) {
            $('#eosContentFilter_btnGuardarDatosUsuarioConEura').hide();
            $('#eosContentFilter_btnGuardarDatosUsuario').hide();
            $('#errorHonorarioJustificacion').show();
        }
    }
}

function toggleConsultoriaInfo(element, hideElement, visible) {
    $(element).hide();
    $(hideElement).show();
    if (visible) {
        $("div[id^='idtrCalculadora_Consultoria_Comment_']").show();
    } else {
        $("div[id^='idtrCalculadora_Consultoria_Comment_']").hide();
    }
}

function toggleCalculadoraSpeaker(element, hideElement, visible) {
    $(element).hide();
    $(hideElement).show();
    if (visible) {
        $("div[id^='idtrCalculadora_Speaker_Chair_Comment_']").show();
    } else {
        $("div[id^='idtrCalculadora_Speaker_Chair_Comment_']").hide();
    }
}
function toggleCalculadoraEif_AB(element, hideElement, visible) {
    $(element).hide();
    $(hideElement).show();
    if (visible) {
        $("div[id^='idtrCalculadora_Eif_AB_Comment_']").show();
        if ($("#eosContentFilter_ddlTipoAsistente").val() == 8) {
            $('div#idtrCalculadora_Eif_AB_Comment_2').hide();
        }
    } else {
        $("div[id^='idtrCalculadora_Eif_AB_Comment_']").hide();
    }
}

function validateTxtJustificacion() {
    var honorariosIntroducidos = parseFloat($('#eosContentFilter_txthonorarios').val());
    var honorariosCalculados = 0;
    //PONENTES
    if ($("#eosContentFilter_ddlTipoAsistente").val() == 2 || $("#eosContentFilter_ddlTipoAsistente").val() == 3 || $("#eosContentFilter_ddlTipoAsistente").val() == 6 || $("#eosContentFilter_ddlTipoAsistente").val() == 9) {
        honorariosCalculados = parseFloat($('#eosContentFilter_txtSpeakerChairHonorariosMaximos').val().replace('.', '').replace(',', '.'));
    }
    //CONSULTORIA
    if ($("#eosContentFilter_ddlTipoAsistente").val() == 10) {
        honorariosCalculados = parseFloat($('#eosContentFilter_txtConsultoriaHonorariosMaximos').val().replace('.', '').replace(',', '.'));
    }
    //Calculadora EIF & Advisory Boards
    if ($("#eosContentFilter_ddlTipoAsistente").val() == 7 || $("#eosContentFilter_ddlTipoAsistente").val() == 8) {
        honorariosCalculados = parseFloat($('#eosContentFilter_txtEifABHonorariosMaximos').val().replace('.', '').replace(',', '.'));
    }

    return !((honorariosCalculados > honorariosIntroducidos) && $('#eosContentFilter_txtJustificacion').val() == "");
}

function ValidateHonorariosMaximos() {
    var honorarios = parseFloat($('#eosContentFilter_txthonorarios').val());
    var honorariosMaximos = parseFloat($('#hidFieldValueHonorariosMaximos').val().replace('.', '').replace(',', '.'));

    if ($("#eosContentFilter_ddlTipoAsistente").val() == 2 || $("#eosContentFilter_ddlTipoAsistente").val() == 3 || $("#eosContentFilter_ddlTipoAsistente").val() == 6 || $("#eosContentFilter_ddlTipoAsistente").val() == 7 || $("#eosContentFilter_ddlTipoAsistente").val() == 8 || $("#eosContentFilter_ddlTipoAsistente").val() == 9 || $("#eosContentFilter_ddlTipoAsistente").val() == 10) {
        if (honorarios > honorariosMaximos || (isNaN(honorariosMaximos) && !isNaN(honorarios))) {
            return false;
        }
    }
    return true;
}

function CalculateHonorariosConsultoria() {
    var honorariosMaximosPorDia = parseFloat($('#hidFieldValueHonorariosMaximosConsultoria').val().replace('.', '').replace(',', '.'));
    if ($('#eosContentFilter_txtConsultoriaDiaConsultor').val() != '') {
        var numDays = parseFloat($('#eosContentFilter_txtConsultoriaDiaConsultor').val());
        var totalHonorarios = numDays * honorariosMaximosPorDia;
        $('#eosContentFilter_txtConsultoriaHonorariosMaximos').val(totalHonorarios);
        $("#eosContentFilter_txthonorarios").val($('#eosContentFilter_txtConsultoriaHonorariosMaximos').val());
        $('#hidFieldValueHonorariosMaximos').val(totalHonorarios);
    } else {
        $('#eosContentFilter_txtConsultoriaHonorariosMaximos').val('');
        $('#hidFieldValueHonorariosMaximos').val('');
    }
}

function CleanFormFields() {
    $('#eosContentFilter_ddlSpeakerChairTipoReunion').val('');
    $('#eosContentFilter_rblTipoPonente input').attr('checked', false);
    $('#eosContentFilter_ddlSpeakerChairTipoPS').val('');
    $('#eosContentFilter_txtSpeakerChairFeeHora').val('');
    $('#eosContentFilter_ddlSpeakerChairDuracionActividad').val('');
    $('#eosContentFilter_ddlSpeakerChairTiempoPreparacion').val('');
    $('#eosContentFilter_chkBoxPonenciaCentroSalud').attr('checked', false);
    $('#eosContentFilter_chkBoxTalleres').attr('checked', false);
    $('#eosContentFilter_chkBoxVideoconferenciasRepetidas').attr('checked', false);
    $('#eosContentFilter_txtSpeakerChairHonorariosMaximos').val('');
    $('#eosContentFilter_rblConsultoriaTipoReunion input').attr('checked', false);
    $('#eosContentFilter_txtConsultoriaDiaConsultor').val('');
    $('#eosContentFilter_txtConsultoriaHonorariosMaximos').val('');
    $('#eosContentFilter_ddlEifABTipoPS').val('');
    $('#eosContentFilter_txtEifABFeeHora').val('');
    $('#eosContentFilter_ddlEifABDuracionActividad').val('');
    $('#eosContentFilter_ddlEifABTiempoPreparacion').val('');
    $('#eosContentFilter_txtEifABHonorariosMaximos').val('');
    $('#eosContentFilter_txtJustificacion').val('');
    $("#eosContentFilter_txthonorarios").val('');
}