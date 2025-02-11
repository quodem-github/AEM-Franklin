using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmecImport.DTO
{
    public class ErrorDto
    {
        public string EventName { get; set; }
        public string EmEventId { get; set; }
        public string RecordTypeDescription { get; set; }
        public string EventTypeDescription { get; set; }
        public string StatusDescription { get; set; }
        public string OwnerWein { get; set; }
        public string OwnerName { get; set; }
        public string Canceled { get; set; }
        public string CommittedCost { get; set; }
        public string ActualCost { get; set; }
        public DateTime fechaComienzo { get; set; }
        public DateTime fechaFinalizacion { get; set; }
        public DateTime RecordCreateDate { get; set; }
    }
}
