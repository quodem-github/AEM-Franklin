using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.ServiceLogic.Data.DTO.Service;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    class ServiciosEditResponseDto
    {
        public ServiciosEditDto ServiciosListResult { get; set; }
        public ServiciosEditDto ServiciosList { get; set; }
    }
}
