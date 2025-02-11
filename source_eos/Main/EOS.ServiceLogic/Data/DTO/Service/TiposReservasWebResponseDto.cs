using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public  class TiposReservasWebResponseDto : ResponseBase
    {
        public List<TiposReservasWebDto> TiposReservasWebList { get; set; }
       
    }
}
