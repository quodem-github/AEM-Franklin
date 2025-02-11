using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class ServiciosReservasHotelResponseDto : ResponseBase
    {
        public List<ServiciosReservasHotelDto> ServiciosReservasHotelList { get; set; }
        public int? IdExpediente { get; set; }
        public int? IdReserva { get; set; }
        public int? IdServicio { get; set; }
        public int? IdServicioHotel { get; set; }
    }
}
