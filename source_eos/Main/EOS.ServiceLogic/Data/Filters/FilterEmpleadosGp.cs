using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterEmpleadosGp : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdEmpleadoG no puede ser inferior a -1")]
        [Required(ErrorMessage = "IdEmpleadoG es obligatorio")]
        public string IdEmpleadoG { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
    }
}
