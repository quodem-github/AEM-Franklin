using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterBase
    {
        [Range(1, int.MaxValue, ErrorMessage = "CurrentPageIndex no puede ser inferior a 0")]
        [Required(ErrorMessage = "CurrentPageIndex es obligatorio")]
        public int? CurrentPageIndex { get; set; }
    }
}
