using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class ReservaViajesResponseDto : ResponseBase
    {
        public List<ReservaViajesDto> ReservasViajesList { get; set; }
        public int? IdExpediente { get; set; }
        public int? IdReserva { get; set; }
        public string IdEstado { get; set; }
        public string LastUpdateDateFrom { get; set; }
        public string LastUpdateDateTo { get; set; }
}
}
