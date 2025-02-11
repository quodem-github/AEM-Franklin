using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.ServiceLogic.Data.DTO.Service;

namespace EOS.ServiceLogic.Data.DTO.Calculator
{
    public class NivelRiesgoHCPDatosAdicionalesResponseDto : ResponseBase
    {
        public List<NivelRiesgoHCPDatosAdicionalesDto> NivelriesgoHCPDatosAdicionalesList { get; set; }
    }
}
