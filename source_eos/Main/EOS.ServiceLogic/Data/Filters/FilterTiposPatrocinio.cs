using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterTiposPatrocinio : FilterBase
    {
        public string Tiponump { get; set; }
        public string Tipopatrocinio { get; set; }
        [Range(-1, int.MaxValue, ErrorMessage = "Idtipopatrocinio no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idtipopatrocinio es obligatorio")]
        public int Idtipopatrocinio { get; set; }

    }
}
