
using System.ComponentModel.DataAnnotations;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterTransportePassengersList : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdTransportePassengersList no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdTransportePassengersList es obligatorio")]
        public int? IdTransportePassengersList { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdServicioTransporte no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdServicioTransporte es obligatorio")]
        public int? IdServicioTransporte { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdExpediente no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdExpediente es obligatorio")]
        public int? IdExpediente { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdPassengerlist no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdPassengerlist es obligatorio")]
        public int? IdPassengerlist { get; set; }
    }
}
