using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterTarifasActividad : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdTarifaActividad no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdTarifaActividad es obligatorio")]
        public int? IdTarifaActividad { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdCongreso no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdCongreso es obligatorio")]
        public int? IdCongreso { get; set; }

    }
}
