using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    class CriteriosSeleccionResponseDto : ResponseBase
    {
        public List<CriteriosSeleccionDto> CeriteriosSeleccionList { get; set; }
    }
}
