using System.Collections.Generic;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class ExpedienteResponseDto : ResponseBase
    {
        public List<ExpedienteDto> ExpedienteList { get; set; }
        public int? IdExpediente { get; set; }
        public string Amec { get; set; }
        public string CreationDateFrom { get; set; }
        public string CreationDateTo { get; set; }
        
    }
}
