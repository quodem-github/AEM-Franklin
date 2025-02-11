using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterHotelPassengerList : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdHotelPassengerlist no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdHotelPassengerlist es obligatorio")]
        public int? IdHotelPassengerlist { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdServicioHotel no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdServicioHotel es obligatorio")]
        public int? IdServicioHotel { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdExpediente no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdExpediente es obligatorio")]
        public int? IdExpediente { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdPassengerlist no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdPassengerlist es obligatorio")]
        public int? IdPassengerlist { get; set; }
        public string IdEstado { get; set; }
    }
}
