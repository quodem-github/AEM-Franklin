using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class TipoActividadResponseDto : ResponseBase
    {
        public List<TipoActividadDto> TipoActividadList { get; set; }
       
    }
}
