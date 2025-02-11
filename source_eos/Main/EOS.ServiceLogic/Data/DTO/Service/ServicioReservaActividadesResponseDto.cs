using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    class ServicioReservaActividadesResponseDto : ResponseBase
    {
        public List<ServiciosReservasActividadesDto> ServiciosReservasActividadesList { get; set; }
        public int? IdExpediente { get; set; }
        public int? IdReserva { get; set; }
        public int? IdServicio { get; set; }
        public int? IdServicioActividad { get; set; }
    }
}
