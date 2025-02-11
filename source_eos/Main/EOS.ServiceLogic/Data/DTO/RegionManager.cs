using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO
{
    public class RegionManager
    {
        public int iddirectorreg { get; set; }
        public int IdPeticionario { get; set; }
        public int idregion { get; set; }
        public bool locked { get; set; }
    }
}
