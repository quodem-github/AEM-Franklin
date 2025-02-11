using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data
{
    public class FilterPaginated
    {
        public int CurrentPageIndex { get; set; }
        public string LastUpdateDateFrom { get; set; }
        public string LastUpdateDateTo { get; set; }
    }
}
