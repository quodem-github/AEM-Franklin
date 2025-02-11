
using System.ComponentModel.DataAnnotations;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterPeticionariosEmpleadosGP : FilterBase
    {
        [Range(0, int.MaxValue, ErrorMessage = "id no puede ser inferior a 0")]
        public int? id { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "idpeticionario no puede ser inferior a 0")]
        public int? idpeticionario { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "idempleadogp no puede ser inferior a 0")]
        public int? idempleadogp { get; set; }
        [Range(0, 1, ErrorMessage = "locked tiene que valer 0, 1 o null")]
        public int? locked { get; set; }
    }
}
