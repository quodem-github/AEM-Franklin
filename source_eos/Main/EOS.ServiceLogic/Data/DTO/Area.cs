using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO
{
    public class Area
    {
        public int Idarea { get; set; }
        public int FKIdEmpresa { get; set; }
        public int idunidad { get; set; }
        public string Codarea { get; set; }
        public string area { get; set; }
        public bool inactivo { get; set; }
        public bool locked { get; set; }
    }
}
