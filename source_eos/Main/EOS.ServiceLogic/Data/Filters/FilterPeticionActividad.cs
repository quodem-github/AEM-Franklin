using System;
using System.ComponentModel.DataAnnotations;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterPeticionActividad : FilterBase
    {
        public string PeticionActividadDateFrom { get; set; }
        public string PeticionActividadDateTo { get; set; }
        public string PeticionActividadCreationDateFrom { get; set; }
        public string PeticionActividadCreationDateTo { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdPeticionActividad no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdPeticionActividad es obligatorio")]
        public int? IdPeticionActividad { get; set; }
        public string PeticionActividad { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime CreationDateFrom { get; set; }
        public DateTime CreationDateTo { get; set; }
    }
}
