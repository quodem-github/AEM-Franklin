using System;
using System.ComponentModel.DataAnnotations;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterCongresos : FilterBase
    {
        public string CongresoDateFrom { get; set; }
        public string CongresoDateTo { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdCongreso no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdCongreso es obligatorio")]
        public int? IdCongreso { get; set; }
        public string Congreso { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
