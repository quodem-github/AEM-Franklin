using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class TiposPatrocinioDto
    {
        public string Tiponump { get; set; }
        public string Tipopatrocinio { get; set; }
        public int? Locked { get; set; }
        public int Idtipopatrocinio { get; set; }

    }
}
