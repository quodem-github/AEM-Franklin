using System.ComponentModel.DataAnnotations;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterReservasPassengerList : FilterBase
    {
        [Range(-1,int.MaxValue,ErrorMessage = "IdExpediente no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdExpediente es obligatorio")]
        public int? IdExpediente { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdReservaPassengersList no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdReservaPassengersList es obligatorio")]
        public int? IdReservaPassengersList { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdPassengerlist no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdPassengerlist es obligatorio")]
        public int? IdPassengerlist { get; set; }
    }
}
