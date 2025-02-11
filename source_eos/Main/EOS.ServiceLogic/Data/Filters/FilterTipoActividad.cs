using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterTipoActividad : FilterBase
    {
        public string Tipoactividad1 { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "Idtipoactividad no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idtipoactividad es obligatorio")]
        public int Idtipoactividad { get; set; }
    }
}
