using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class ServiciosResponseDto : ResponseBase
    {
        public List<ServiciosDto> ServiciosList { get; set; }
       
    }
}
