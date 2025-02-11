using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    class FilterTarifasInscripcion : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdTarifaInscripcion no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdTarifaInscripcion es obligatorio")]
        public int? IdTarifaInscripcion { get; set; }

        [Range(-1, int.MaxValue, ErrorMessage = "IdCongreso no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdCongreso es obligatorio")]
        public int? IdCongreso { get; set; }
    }
}
