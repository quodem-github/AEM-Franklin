using System;
using System.ComponentModel.DataAnnotations;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterExpediente : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdExpediente no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdExpediente es obligatorio")]
        public int? IdExpediente { get; set; }
        [RegularExpression("^[0-9]+$", ErrorMessage = "Amec must be numeric")]
        public string Amec { get; set; }
        public string CreationDateFrom { get; set; }
        public string CreationDateTo { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
