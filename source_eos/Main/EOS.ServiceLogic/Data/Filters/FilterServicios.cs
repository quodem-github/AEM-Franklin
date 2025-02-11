using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterServicios : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idnumerico no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idnumerico es obligatorio")]
        public int Idnumerico { get; set; }
        public string Servicio { get; set; }
        public string Idservicio { get; set; }
    }
}
