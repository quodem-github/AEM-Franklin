/* OGP "20/09/2012" EventosForms */

$(function () {
    // Ocultar el contenedor del Formulario.
    // Mostrarlo en un diálogo modal. 
    var eventosFormsCnt = $('#eosContentFilter_eventosFormsCnt');

    /* OGP "25/09/2012" */
    if (eventosFormsCnt.length > 0) {
        eventosFormsCnt.hide();
        eventosFormsCnt.modal();
    }

    // Obtener el ID del Evento.
    $('.goMSDBtnClass').click(function () {
        
        var params = $(this).attr('alt').split("|");
        var idEvento = params[0];
        var idconfempresa = params[1];
        var urlEvento = params[2];

        var url = "EventosForms.aspx?idEvento=" + idEvento + "&idConfEmpresa=" + idconfempresa + "&urlEvento=" + urlEvento;
        window.open(url);
    });
});