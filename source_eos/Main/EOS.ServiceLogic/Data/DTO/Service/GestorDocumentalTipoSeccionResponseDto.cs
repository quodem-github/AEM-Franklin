using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class GestorDocumentalTipoSeccionResponseDto : ResponseBase
    {
        public List<GestorDocumentalTipoSeccionDto> GestorDocumentalTipoSeccionList { get; set; }
    }
}
