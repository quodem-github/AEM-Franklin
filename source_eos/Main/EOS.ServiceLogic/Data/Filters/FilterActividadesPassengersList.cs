using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterActividadesPassengersList : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdActividadPassengerList no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdActividadPassengerList es obligatorio")]
        public int? IdActividadPassengerList { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdServicioActividad no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdServicioActividad es obligatorio")]
        public int? IdServicioActividad { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdExpediente no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdExpediente es obligatorio")]
        public int? IdExpediente { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdPassengerlist no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdPassengerlist es obligatorio")]
        public int? IdPassengerlist { get; set; }
    }
}
