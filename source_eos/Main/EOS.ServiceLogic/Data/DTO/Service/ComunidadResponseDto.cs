using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class ComunidadResponseDto : ResponseBase
    {
      public List<ComunidadDto> ComunidadList { get; set; }
    }
}
