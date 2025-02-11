using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterInscripcionPassengersList : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdInsPassengerlist no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdInsPassengerlist es obligatorio")]
        public int? IdInsPassengerlist { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdServicioInscripcion no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdServicioInscripcion es obligatorio")]
        public int? IdServicioInscripcion { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdExpediente no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdExpediente es obligatorio")]
        public int? IdExpediente { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdPassengerlist no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdPassengerlist es obligatorio")]
        public int? IdPassengerlist { get; set; }
    }
}
