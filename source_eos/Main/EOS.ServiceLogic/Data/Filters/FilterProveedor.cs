using System.ComponentModel.DataAnnotations;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterProveedor : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdProveedor no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdProveedor es obligatorio")]
        public int? IdProveedor { get; set; }
        public string Proveedor { get; set; }
    }
}
