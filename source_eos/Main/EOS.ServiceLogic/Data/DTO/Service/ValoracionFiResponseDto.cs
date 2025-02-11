using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class ValoracionFiResponseDto : ResponseBase
    {
        public List<ValoracionFiDto> ValoracionFiList { get; set; }
    }
}
