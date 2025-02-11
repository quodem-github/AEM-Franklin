using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO
{
    public class District
    {
        public int iddistrito { get; set; }
        public int IdEmpresa { get; set; }
        public int idregion { get; set; }
        public string distrito { get; set; }
        public bool locked { get; set; }
        public string coddistrito { get; set; }
        public int idarea { get; set; }
        public bool inactivo { get; set; }
    }
}
