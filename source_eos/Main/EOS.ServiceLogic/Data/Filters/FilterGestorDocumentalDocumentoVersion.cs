using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterGestorDocumentalDocumentoVersion : FilterBase
    {
        public int? Id { get; set; }
        public int? IdDocumento { get; set; }
        public string IdAmec { get; set; }
        public int? IdExpediente { get; set; }
        public DateTime? CreationDateFrom { get; set; }
        public DateTime? CreationDateTo { get; set; }
        public DateTime? UpdateDateFrom { get; set; }
        public DateTime? UpdateDateTo { get; set; }
    }
}
