using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterPeticionarriosList :FilterPaginated
    {
        public int? IdPeticionario { get; set; }
        public string Login { get; set; }
        public string Wein { get; set; }
        public string WeinManager { get; set; }
    }
}
