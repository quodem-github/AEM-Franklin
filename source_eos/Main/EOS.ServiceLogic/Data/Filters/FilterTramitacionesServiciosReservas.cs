using System.ComponentModel.DataAnnotations;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterTramitacionesServiciosReservas : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdExpediente no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdExpediente es obligatorio")]
        public int? IdExpediente { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdServicio no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdServicio es obligatorio")]
        public int? IdServicio { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdReserva no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdReserva es obligatorio")]
        public int? IdReserva { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdTramitacion no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdTramitacion es obligatorio")]
        public int? IdTramitacion { get; set; }
    }
}
