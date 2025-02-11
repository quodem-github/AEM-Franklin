using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterAmec : FilterBase
    {
        public string Amec { get; set; }
        public int? IdAmec { get; set; }
    }
}
