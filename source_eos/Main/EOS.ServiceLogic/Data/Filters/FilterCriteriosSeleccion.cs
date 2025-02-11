using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterCriteriosSeleccion : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdCriterio no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdCriterio es obligatorio")]
        public int? IdCriterio { get; set; }
        public string CriterioSeleccion { get; set; } 
    }
}
