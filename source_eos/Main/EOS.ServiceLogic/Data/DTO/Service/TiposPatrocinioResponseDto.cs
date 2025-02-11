using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class TiposPatrocinioResponseDto : ResponseBase
    {
        public List<TiposPatrocinioDto> TiposPatrocionioList { get; set; }
    }
}
