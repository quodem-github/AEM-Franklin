
using System.Collections.Generic;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class GestorInvitadosResponseDto : ResponseBase
    {
        public List<GestorInvitadosDto> GestorInvitadosList { get; set; }
        public string GestorDateFrom { get; set; }
        public string GestorDateTo { get; set; }
        public int? IdGestorInvitados { get; set; }
        public int? IdEventoFormulario { get; set; }
        public int? IdCongreso { get; set; }
    }
}
