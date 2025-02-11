using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO
{
    public class DistrictManager
    {
        public int idgerentedist { get; set; }
        public int IdPeticionario { get; set; }
        public int iddistrito { get; set; }
        public bool locked { get; set; }
    }
}
