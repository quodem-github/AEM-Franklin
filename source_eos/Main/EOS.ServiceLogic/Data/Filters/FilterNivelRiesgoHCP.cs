using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterNivelRiesgoHCP : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idnivelriesgo no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idnivelriesgo es obligatorio")]
        public int Idnivelriesgo{get;set;}
        public string Nivelriesgo{get;set;}
    }
}
