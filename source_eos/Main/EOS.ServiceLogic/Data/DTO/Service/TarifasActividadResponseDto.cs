using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class TarifasActividadResponseDto : ResponseBase
    {
        public List<TarifasActividadDto> TarifasActividadList { get; set; }
        public int? IdTarifaActividad { get; set; }
        public int? IdCongreso { get; set; }
    }
}
