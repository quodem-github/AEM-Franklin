using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class TipoServiciosResponseDto : ResponseBase
    {
        public List<TipoServiciosDto> TipoServiciosList { get; set; }
      
    }
}
