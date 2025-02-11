using System.Collections.Generic;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class PeticionActividadResponseDto : ResponseBase
    {
        public List<PeticionActividadDto> PeticionActividadList { get; set; }
        public string PeticionActividadDateFrom { get; set; }
        public string PeticionActividadDateTo { get; set; }
        public string PeticionActividadCreateDateFrom { get; set; }
        public string PeticionActividadCreateDateTo { get; set; }
        public int? IdPeticionActividad { get; set; }
        public string PeticionActividad { get; set; }
    }
}
