using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class AmecsResponseDto:ResponseBase
    {
        public List<AmecsDto> AmecsList { get; set; }
        public string Idamecs { get; set; }
        public string LastUpdateDateFrom { get; set; }
        public string LastUpdateDateTo { get; set; }
    }
}
