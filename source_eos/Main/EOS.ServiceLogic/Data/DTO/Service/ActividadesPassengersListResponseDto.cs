using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class ActividadesPassengersListResponseDto : ResponseBase
    {
        public List<ActividadesPassengersListDto> ActividadesPassengerListList { get; set; }
        public int? IdExpediente { get; set; }
        public int? IdServicioActividad { get; set; }
        public int? IdActividadPassengerList { get; set; }
        public int? IdPassengerlist { get; set; }
    }
}
