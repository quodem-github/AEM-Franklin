using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class ConfEmpresaResponseDto : ResponseBase
    {
        public List<ConfEmpresaDto> ConfEmpresaList { get; set; }
    }
}
