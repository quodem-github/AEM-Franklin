using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class TransportePassengersListResponseDto : ResponseBase
    {
        public List<TransportePassengersListDto> TransportePassengersListList { get; set; }
        public int? IdTransportePassengersList { get; set; }
        public int? IdServicioTransporte { get; set; }
        public int? IdExpediente { get; set; }
        public int? IdPassengerlist { get; set; }
    }
}
