using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class ProvinciasDto
    {
        public string Idprovincia { get; set; }
        public string Idpais { get; set; }
        public string Provincia { get; set; }
        public int? Idcomunidad { get; set; }
        public int? Locked { get; set; }
    }
}
