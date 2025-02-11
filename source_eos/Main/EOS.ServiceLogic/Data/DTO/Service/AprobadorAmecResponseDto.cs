using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class AprobadorAmecResponseDto : ResponseBase
    {
        public List<AprobadorAmecDto> AprobadorAmecList { get; set; }
        public string IdAmecs { get; set; }
        public int IdAprobador { get; set; }
    }
}
