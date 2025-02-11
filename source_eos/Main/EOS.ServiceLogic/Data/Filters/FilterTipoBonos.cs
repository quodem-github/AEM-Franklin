using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterTipoBonos:FilterBase
    {
        [Required(ErrorMessage = "IdTipoBono es obligatorio")]
        public string IdTipoBono { get; set; }
        public string TipoBono { get; set; }
    }
}
