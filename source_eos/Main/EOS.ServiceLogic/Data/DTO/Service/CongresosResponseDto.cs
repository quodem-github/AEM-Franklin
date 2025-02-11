using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class CongresosResponseDto : ResponseBase
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int? IdCongreso { get; set; }
        public string Congreso { get; set; }
        public List<CongresosDto> CongresoList { get; set; }
    }
}
