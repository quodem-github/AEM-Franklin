using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class HotelPassengerListResponseDto : ResponseBase
    {
        public List<HotelPassengerListDto> HotelPassengersListList { get; set; }
        public int? IdHotelPassengerlist { get; set; }
        public int? IdServicioHotel { get; set; }
        public int? IdExpediente { get; set; }
        public int? IdPassengerlist { get; set; }
    }
}
