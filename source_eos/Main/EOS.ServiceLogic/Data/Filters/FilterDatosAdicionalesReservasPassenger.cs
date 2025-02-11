using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterDatosAdicionalesReservasPassenger : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdPassengerList no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdPassengerList es obligatorio")]
        public int IdPassengerList { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdExpediente no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdExpediente es obligatorio")]
        public int IdExpediente { get; set; }
    }
}
