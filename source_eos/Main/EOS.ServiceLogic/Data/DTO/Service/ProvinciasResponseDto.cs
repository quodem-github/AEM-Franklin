using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class ProvinciasResponseDto:ResponseBase
    {
        public List<ProvinciasDto> ProvinciasList { get; set; }
       
    }
}
