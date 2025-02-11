using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO
{
    public class Unit
    {
        public int idunidad { get; set; }
        public int IdEmpresa { get; set; }
        public string codigo { get; set; }
        public string unidad { get; set; }
        public bool locked { get; set; }
        public bool inactivo { get; set; }
    }
}
