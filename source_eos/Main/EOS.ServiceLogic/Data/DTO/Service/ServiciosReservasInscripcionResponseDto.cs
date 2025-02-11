using System.Collections.Generic;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class ServiciosReservasInscripcionResponseDto : ResponseBase
    {
        public List<ServiciosReservasInscripcionDto> ServiciosReservasInscripcionList { get; set; }
        public int? IdExpediente { get; set; }
        public int? IdServicio { get; set; }
        public int? IdReserva { get; set; }
        public int? IdServicioInscripcion { get; set; }
    }
}
