using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO
{
    public class UnitManager
    {
        public int iddirectorunidad { get; set; }
        public int IdPeticionario { get; set; }
        public int idunidad { get; set; }
        public bool locked { get; set; }
    }
}
