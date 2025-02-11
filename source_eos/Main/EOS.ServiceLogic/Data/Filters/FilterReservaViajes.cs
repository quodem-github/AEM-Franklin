using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterReservaViajes : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdExpediente no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdExpediente es obligatorio")]
        public int? IdExpediente { get; set; }

        [Range(-1, int.MaxValue, ErrorMessage = "IdReserva no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdReserva es obligatorio")]
        public int? IdReserva { get; set; }
        public string IdEstado { get; set; }
        public string LastUpdateDateFrom { get; set; }
        public string LastUpdateDateTo { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
