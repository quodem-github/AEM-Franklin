using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterIatas : FilterBase
    {
        public string IdIata { get; set; }
        public string Iata { get; set; }
    }
}
