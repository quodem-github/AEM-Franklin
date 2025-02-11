/*
 END USER REQUEST AGREEMENT window
*/

(function ($) {
    var that,
        settings,
        defaults = {
            title: 'ACEPTAR POLÍTICA'
        };
    
    //
    $.fn.eura = function (options) {
        that = this;
        settings = defaults;
        
        var acceptEura = function (e, callback, dlg) {
            var chk = dlg.find('input');

            // Ejecuto la acción predeterminada del control ssi se ha seleccionado el checkbox.
            if (chk.length && chk.is(':checked')) {
                // Settings contiene un callback con el handler del evento del control.
                if (callback) {
                    callback(e);
                }
            }
            
            $.modal.close();
        };

        return this.each(function () {
            // Opciones.
            $.extend(settings, options);
            // Dialogo a mostrar.
            var dlg = $('<div class="eura-container">' +
                    '<div class="eura-container-frame">' +
                        '<h3>' + settings.title + '<\/h3>' +
                        /*'<div><textarea cols="58" rows="4" readonly>' + settings.text + '<\/textarea><\/div>' +*/
                        '<div><textarea cols="58" rows="4" readonly>Verificar el riesgo cruzado y en caso de que el nivel de riesgo del profesional sanitario incluido en esta actividad sea medio o alto realizar, según lo establecido en Global Standard 5.2.1, el FCPA en Appian.<\/textarea><\/div>' +
                        '<div class="file-link-eura"><a target="_blank" class="document-link" href="https://collaboration.merck.com/sites/ECO/Policy Documents (ex US)/Interacting_with_Ex_US_HCPs_and_Other_Government_Officials/HCP_Due_Diligence/Direct Engagement Global Heat Map.pdf?cid=d012a282-c27c-4836-a859-f36acc9bb84e">Direct Engagement Global Heat Map.pdf</a><\/div>' +
                        '<div class="eura-container-chk-cnt"><input type="checkbox" \/><span>Acepto<\/span><\/div>' +
                    '<\/div>' +
                    '<div class="eura-container-btn-cnt"><button><span>Aceptar<\/span><\/button><\/div>' +
                '<\/div>');

            // Evento click del control.
            that.click(function (e) {
                e = e || window.event;
                // Handler del botón de Aceptar del dialogo.
                dlg.find('button').click(function () {
                    acceptEura(e, settings.action, dlg);
                });

                dlg.modal(); // Mostrar el dialogo con el texto a Aceptar.
                return false; // Impedir que se ejecuten las acciones del control.
            });
        });
    };
})(jQuery);