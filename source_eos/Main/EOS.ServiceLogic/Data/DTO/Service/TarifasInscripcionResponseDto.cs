using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class TarifasInscripcionResponseDto : ResponseBase
    {
        public int? IdTarifaInscripcion { get; set; }
        public int? IdCongreso { get; set; }
        public List<TarifasInscripcionDto> TarifasInscripcionList { get; set; }
    }
}
