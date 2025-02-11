using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class IatasReponseDto : ResponseBase
    {
        public List<IatasDto> IatasList { get; set; }
        public string Iata { get; set; }
        public int? Idpoblacion { get; set; }
        public string Idiata { get; set; }
    }
}
