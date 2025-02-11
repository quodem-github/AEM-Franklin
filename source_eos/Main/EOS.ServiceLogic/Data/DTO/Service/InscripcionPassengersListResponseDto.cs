using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class InscripcionPassengersListResponseDto : ResponseBase
    {
        public List<InscripcionPassengersListDto> InscripcionPassengersListList { get; set; }
        public int? IdInsPassengerlist { get; set; }
        public int? IdServicioInscripcion { get; set; }
        public int? IdExpediente { get; set; }
        public int? IdPassengerlist { get; set; }
    }
}
