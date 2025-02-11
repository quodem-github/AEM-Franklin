using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class EspecialidadesResponseDto : ResponseBase
    {
        public List<EspecialidadesDto> EspecialidadesList { get; set; }
    }
}
