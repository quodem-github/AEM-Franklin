using System.ComponentModel.DataAnnotations;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterPoblacion : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdPoblacion no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdPoblacion es obligatorio")]
        public int? IdPoblacion { get; set; }
        public string Poblacion { get; set; }
    }
}
