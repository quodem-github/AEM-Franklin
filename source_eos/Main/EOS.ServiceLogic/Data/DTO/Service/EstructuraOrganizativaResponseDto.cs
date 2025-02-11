using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class EstructuraOrganizativaResponseDto : ResponseBase
    {
        public List<EstructuraOrganizativaDto> EstructuraOrganizativaList { get; set; }
        public int IdPeticionario { get; set; }
        public int IdPeticionarioManager { get; set; }
        public string Wein { get; set; }
        public string WeinManager { get; set; }
        public string UpdateDateFrom { get; set; }
        public string UpdateDateTo { get; set; }
    }
}
