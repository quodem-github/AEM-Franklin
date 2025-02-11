using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterTarifasAloj : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdTarifaAloj no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdTarifaAloj es obligatorio")]
        public int? IdTarifaAloj { get; set; }

        [Range(-1, int.MaxValue, ErrorMessage = "IdCongreso no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdCongreso es obligatorio")]
        public int? IdCongreso { get; set; }
    }
}
