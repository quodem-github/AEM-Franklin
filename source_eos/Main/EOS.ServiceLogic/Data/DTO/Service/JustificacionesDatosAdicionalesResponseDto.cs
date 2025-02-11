using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class JustificacionesDatosAdicionalesResponseDto:ResponseBase
    {
        public List<JustificacionesDatosAdicionalesDto> JustificacionesDatosAdicionalesList { get; set; }
    }
}
