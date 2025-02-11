$(function () {
    // eura plugin para mostrar la información cada vez que se selecciona un profesional.
    $('.eura').eura({
        //text: 'Certifico la revisión del nivel de riesgo BAJO de los profesionales sanitarios incluidos en esta actividad, con respecto a la misma según lo establecido en Pol. Corp. Nº 20/FCPA. \nEn caso de riesgo Medio/Alto, se tramitará la solicitud de FCPA en Axentis.',
        text: 'Certifico la revisión del nivel de riesgo BAJO de los profesionales sanitarios incluidos en esta actividad, con respecto a la misma según lo establecido en C.Pol 5/FCPA. \nEn caso de riesgo Medio/Alto, se tramitará la solicitud de FCPA en Axentis.',        
        action: function (e) {
            // Llamar al método __doPostBack "fantasma" de asp.net para realizar la acción del control, si existe.
            if (__doPostBack) {
                __doPostBack(e.currentTarget.attributes.name.nodeValue, '');
            }
        }
    });
});

$(document).ready(function () {
    $('#eosContentResults_btnGuardarDatosUsuario').hide();
    //$("[id^='eosContentFilter_ddlTipoAsistenteinput']").change(function (event) {
    $("#eosContentResults_ddlTipoAsistente").change(function (event) {
        if ($("#eosContentResults_ddlTipoAsistente").val() == 1 || $("#eosContentResults_ddlTipoAsistente").val() == 2) {
            if ($("#eosContentResults_ddljustificaciones").val() != " ") {
                $("#idtrjustificaciones").attr("style", "visibility: visible");
            }
            if ($("#eosContentResults_ddlTipoAsistente").val() == 2) {
                $("#idtrhonorarios").attr("style", "visibility: visible");
                $("#idtrpagodirecto").attr("style", "visibility: visible");
                if ($("#eosContentResults_rblpagodirecto_0").is(":checked")) {
                    $("#idtdpagofundacionotros1").attr("style", "visibility: visible");
                    $("#idtdpagofundacionotros2").attr("style", "visibility: visible");
                }
            } else {
                $("#idtrhonorarios").attr("style", "visibility: hidden");
                $("#idtrpagodirecto").attr("style", "visibility: hidden");
                $("#idtdpagofundacionotros1").attr("style", "visibility: hidden");
                $("#idtdpagofundacionotros2").attr("style", "visibility: hidden");
            }
        } else {
            $("#idtrjustificaciones").attr("style", "visibility: hidden");
            $("#idtrhonorarios").attr("style", "visibility: hidden");
            $("#idtrpagodirecto").attr("style", "visibility: hidden");
            $("#idtdpagofundacionotros1").attr("style", "visibility: hidden");
            $("#idtdpagofundacionotros2").attr("style", "visibility: hidden");
        }

        if ($("#eosContentResults_ddlTipoAsistente").val() == 4) {
            $("#idtdpertenece1").attr("style", "visibility: hidden");
            $("#idtdpertenece2").attr("style", "visibility: hidden");
            $('#eosContentResults_btnGuardarDatosUsuarioConEura').hide();
            $('#eosContentResults_btnGuardarDatosUsuario').show();
        }
        else {
            $("#idtdpertenece1").attr("style", "visibility: visible");
            $("#idtdpertenece2").attr("style", "visibility: visible");
            if ($("#eosContentResults_ddlTipoActividadPax").val() != 8) {
                $('#eosContentResults_btnGuardarDatosUsuarioConEura').show();
                $('#eosContentResults_btnGuardarDatosUsuario').hide();
            }
            else {
                $('#eosContentResults_btnGuardarDatosUsuarioConEura').hide();
                $('#eosContentResults_btnGuardarDatosUsuario').show();
            }

        }
    });

    $("#eosContentFilter_ddlTipoActividadPax").change(function (event) {
        if ($("#eosContentResults_ddlTipoActividadPax").val() == 8) {
            $('#eosContentResults_btnGuardarDatosUsuarioConEura').hide();
            $('#eosContentResults_btnGuardarDatosUsuario').show();
        }
        else {
            if ($("#eosContentResults_ddlTipoAsistente").val() != 4) {
                $('#eosContentResults_btnGuardarDatosUsuarioConEura').show();
                $('#eosContentResults_btnGuardarDatosUsuario').hide();
            }
            else {
                $('#eosContentResults_btnGuardarDatosUsuarioConEura').hide();
                $('#eosContentResults_btnGuardarDatosUsuario').show();
            }
        }
    });

    $(':radio[id="eosContentResults_rblpagodirecto_1"]').click(function () {
        $("#idtdpagofundacionotros1").attr("style", "visibility: hidden");
        $("#idtdpagofundacionotros2").attr("style", "visibility: hidden");
    });
    $(':radio[id="eosContentResults_rblpagodirecto_0"]').click(function () {
        $("#idtdpagofundacionotros1").attr("style", "visibility: visible");
        $("#idtdpagofundacionotros2").attr("style", "visibility: visible");
    });

    $("#eosContentResults_txtMSDID").focusout(function () {
      alert("El MSDID confirma que el HCP ha sido dado de alta previamente en APPIAN. Antes de proceder a este alta en EOS busca en la " +
                "Base de datos de EOS al HCP para el que requieres los servicios.  " +
                "Si posteriormente conoces el código MSDI puedes enviarlo por correo a la agencia de viajes. " +
                "\nPeriódicamente los HCPs con código incorrecto serán reportados a tu división.");
    });

});
