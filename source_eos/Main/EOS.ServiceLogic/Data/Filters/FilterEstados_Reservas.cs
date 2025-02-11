using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterEstados_Reservas : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdRegistre no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdRegistre es obligatorio")]
        public int IdRegistre { get; set; }
        public string IdEstadoInicial { get; set; }
        public string IdEstadoFinal { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdReserva no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdReserva es obligatorio")]
        public int IdReserva { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdExpediente no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdExpediente es obligatorio")]
        public int IdExpediente { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "Sync no puede ser inferior a -1")]
        [Required(ErrorMessage = "Sync es obligatorio")]
        public int Sync { get; set; }
        public string TransactionKey { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdServicio no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdServicio es obligatorio")]
        public int IdServicio { get; set; }
        public string FechacambioestadoInicial { get; set; }
        public string FechacambioestadoFinal { get; set; }
    }
}
