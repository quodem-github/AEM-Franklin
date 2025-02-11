using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterPassengerList :FilterPaginated
    {
        public int? IdPassengerList { get; set; }
        public string Msdid { get; set; }
        public int? Internacional { get; set; }
        public string GoldenId { get; set; }
        public string GenesysCode { get; set; }
        public int? Peticionario { get; set; }
    }
}
