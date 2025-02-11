using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class TarifasAlojResponseDto : ResponseBase
    {
        public int? IdTarifaAloj { get; set; }
        public int? IdCongreso { get; set; }
        public List<TarifasAlojDto> TarifasAlojList { get; set; }
    }
}
