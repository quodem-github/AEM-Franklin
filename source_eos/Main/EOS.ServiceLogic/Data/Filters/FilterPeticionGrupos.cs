using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterPeticionGrupos : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdPeticionGrupo no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdPeticionGrupo es obligatorio")]
        public int IdPeticionGrupo { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdEvento no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdEvento es obligatorio")]
        public int IdEvento { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdAsistente no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdAsistente es obligatorio")]
        public int IdAsistente { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdExpediente no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdExpediente es obligatorio")]
        public int IdExpediente { get; set; }
    }
}
