using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class TiposAlojamientoResponseDto : ResponseBase
    {
        public List<TiposAlojamientoDto> TiposAlojamientoList { get; set; }
    }
}
