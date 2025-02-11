using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class EmpleadosGpResponseDto : ResponseBase
    {
        public List<EmpleadosGpDto> EmpleadosGpList { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Idempleadogp { get; set; }
        public string Nombre { get; set; }
    }
}
