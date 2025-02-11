using System;
using System.ComponentModel.DataAnnotations;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterEstructuraOrganizativa : FilterBase
    {
        public string UpdateDateFrom { get; set; }
        public string UpdateDateTo { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdPeticionario no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdPeticionario es obligatorio")]
        public int IdPeticionario { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdPeticionario no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdPeticionario es obligatorio")]
        public int IdPeticionarioManager { get; set; }
        public string Wein { get; set; }
        public string WeinManager { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
