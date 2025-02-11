using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterProvincias: FilterBase
    {
        public string IdProvincia { get; set; }
        public string Provincia { get; set; }
        public string IdPais { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdComunidad no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdComunidad es obligatorio")]
        public int IdComunidad { get; set; }
    }
}
