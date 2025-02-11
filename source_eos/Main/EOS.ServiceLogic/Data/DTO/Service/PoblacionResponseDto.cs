using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class PoblacionResponseDto : ResponseBase
    {
        public List<PoblacionDto> PoblacionList { get; set; }
        public int? IdPoblacion { get; set; }
        public string Poblacion { get; set; }
    }
}
