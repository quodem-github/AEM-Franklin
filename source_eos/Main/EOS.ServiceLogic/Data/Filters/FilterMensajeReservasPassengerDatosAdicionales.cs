using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterMensajeReservasPassengerDatosAdicionales:FilterBase
    {
        public string Mensaje { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdMensaje no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdMensaje es obligatorio")]
        public int IdMensaje { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdTipoActividadPax no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdTipoActividadPax es obligatorio")]
        public int IdTipoActividadPax { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdTipoAsistene no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdTipoAsistene es obligatorio")]
        public int IdTipoAsistene { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdNivelRiesgo no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdNivelRiesgo es obligatorio")]
        public int IdNivelRiesgo { get; set; }
    }
}
