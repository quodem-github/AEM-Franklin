using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class TipoCongresoResponseDto : ResponseBase
    {
        public List<TipoCongresoDto> TipoCongresoList { get; set; }
    }
}
