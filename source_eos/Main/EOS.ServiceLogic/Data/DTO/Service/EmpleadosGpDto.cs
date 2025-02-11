using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class EmpleadosGpDto
    {
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Idempleadogp { get; set; }
        public string Nombre { get; set; }
        public int? Locked { get; set; }
        public int Id { get; set; }
    }
}
