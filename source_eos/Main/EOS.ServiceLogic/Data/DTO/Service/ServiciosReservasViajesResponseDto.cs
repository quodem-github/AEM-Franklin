using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class ServiciosReservasViajesResponseDto : ResponseBase
    {
        public List<ServiciosReservasViajesDto> ServiciosReservasViajesList { get; set; }
        public int? IdExpediente { get; set; }
        public int? IdServicio { get; set; }
        public int? IdReserva { get; set; }
    }
}
