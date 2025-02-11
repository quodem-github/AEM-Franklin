using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class DatosAdicionalesReservasPassengerResponseDto : ResponseBase
    {
        public List<DatosAdicionalesReservasPassengerDto> DatosAdicionalesList { get; set; }
        public int IdExpediente { get; set; }
        public int IdPassengerList { get; set; }
    }
}
