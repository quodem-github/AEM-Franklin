using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class Estados_ReservasResponseDto : ResponseBase
    {
        public List<Estados_ReservasDto> EstadosReservasList { get; set; }
        public int IdRegistre { get; set; }
        public string IdEstadoInicial { get; set; }
        public string IdEstadoFinal { get; set; }
        public int IdReserva { get; set; }
        public int IdExpediente { get; set; }
        public int Sync { get; set; }
        public string TransactionKey { get; set; }
        public int IdServicio { get; set; }
        public string FechacambioestadoInicial { get; set; }
        public string FechacambioestadoFinal { get; set; }
    }
}
