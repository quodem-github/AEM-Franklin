using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO
{
    public class AreaManager
    {
        public int idgerentearea { get; set; }
        public int IdPeticionario { get; set; }
        public int idarea { get; set; }
        public bool locked { get; set; }
    }
}
