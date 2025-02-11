using System;
using System.ComponentModel.DataAnnotations;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterGestorInvitados : FilterBase
    {
        public string CongresoDateFrom { get; set; }
        public string CongresoDateTo { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdGestorInvitados no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdGestorInvitados es obligatorio")]
        public int? IdGestorInvitados { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdCongreso no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdCongreso es obligatorio")]
        public int? IdCongreso { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "IdEventoFormulario no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdEventoFormulario es obligatorio")]
        public int? IdEventoFormulario { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
