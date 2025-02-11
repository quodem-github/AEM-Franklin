$(function () {
    // eura plugin para mostrar la información cada vez que se selecciona un profesional.
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