using System;
using System.ComponentModel.DataAnnotations;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterComunidad : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdComunidad no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdComunidad es obligatorio")]
        public int? IdComunidad { get; set; }
        public string Comunidad { get; set; }
    }
}
