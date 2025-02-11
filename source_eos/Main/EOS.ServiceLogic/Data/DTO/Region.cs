using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO
{
    public class Region
    {
        public int idregion { get; set; }
        public int FKIdEmpresa { get; set; }
        public string region { get; set; }
        public bool locked { get; set; }
        public string codregion { get; set; }
        public int idunidad { get; set; }
        public bool inactivo { get; set; }
    }
}
