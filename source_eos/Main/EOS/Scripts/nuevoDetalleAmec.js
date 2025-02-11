
$(function () {
    // Mostrar mensaje cuando se escriba un valor en el campo NumPonentes.
    var txt = $('#eosContentFilter_txtNumPonentes');

    if (txt.length) {
        txt.blur(function () {
            txt.val() && alert('El programa definitivo (en el que se indica: título de la reunión, fecha , lugar, nombre de ponentes con afiliaciones, ponencias y el logo de MSD) debe estar colgado en la documentación del AMEC');
        });
    }

    // eura plugin para mostrar la información a aceptar.
    $('#eosContentFilter_btnSometer').eura({
        //text: 'Certifico la revisión del nivel de riesgo de los profesionales sanitarios incluidos en esta actividad, con respecto a la misma según lo establecido en Pol. Corp. Nº 20/FCPA',
        text: 'Certifico la revisión del nivel de riesgo de los profesionales sanitarios incluidos en esta actividad, con respecto a la misma según lo establecido en C.Pol 5/FCPA',
        
        action: function (e) {
           
            // Llamar al método __doPostBack "fantasma" de asp.net para realizar la acción del control, si existe.
            if (__doPostBack) {
                __doPostBack(e.currentTarget.attributes.name.nodeValue, '');
            }
        }
    });
});