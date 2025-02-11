using System.Collections.Generic;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class TramitacionesServiciosReservasResponseDto : ResponseBase
    {
        public List<TramitacionesServiciosReservasDto> TramitacionesServiciosReservasList { get; set; }
        public int? IdExpediente { get; set; }
        public int? IdServicio { get; set; }
        public int? IdReserva { get; set; }
        public int? IdTramitacion { get; set; }
    }
}
