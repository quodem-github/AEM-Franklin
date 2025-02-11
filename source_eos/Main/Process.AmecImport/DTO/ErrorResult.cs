using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmecImport.DTO
{
    public class ErrorResult
    {
        public List<ErrorDto> ErrorList { get; set; }
        public bool Status { get; set; }
    }
}
