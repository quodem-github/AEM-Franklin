using System.Collections.Generic;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class ReservaPassengersResponseDto : ResponseBase
    {
        public List<ReservaPassengersDto> ReservaPassengersListList { get; set; }
        public int? IdExpediente { get; set; }
        public int? IdReservaPassengersList { get; set; }
        public int? IdPassengerlist { get; set; }
    }
}
