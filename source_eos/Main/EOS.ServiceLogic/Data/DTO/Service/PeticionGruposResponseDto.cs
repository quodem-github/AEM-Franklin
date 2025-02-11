using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class PeticionGruposResponseDto : ResponseBase
    {
        public List<PeticionGruposDto> PeticionGruposList { get; set; }
        public int IdPeticionGrupo { get; set; }
        public int IdEvento { get; set; }
        public int IdAsistente { get; set; }
        public int IdExpediente { get; set; }
    }
}
