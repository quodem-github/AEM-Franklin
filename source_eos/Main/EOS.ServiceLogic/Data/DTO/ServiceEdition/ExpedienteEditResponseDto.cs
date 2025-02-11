using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EOS.ServiceLogic.Data.DTO.Service;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    class ExpedienteEditResponseDto
    {
        public ExpedienteEditDto ExpedienteResult { get; set; }
        public ExpedienteEditDto Expediente { get; set; }
    }
}
