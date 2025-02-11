using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterServicioReservasHotel : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdExpediente no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdExpediente es obligatorio")]
        public int? IdExpediente { get; set; }

        [Range(-1, int.MaxValue, ErrorMessage = "IdReserva no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdReserva es obligatorio")]
        public int? IdReserva { get; set; }

        [Range(-1, int.MaxValue, ErrorMessage = "IdServicio no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdServicio es obligatorio")]
        public int? IdServicio { get; set; }

        [Range(-1, int.MaxValue, ErrorMessage = "IdServicioHotel no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdServicioHotel es obligatorio")]
        public int? IdServicioHotel { get; set; }
    }
}
