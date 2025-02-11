using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterGestorDocumentalDocumentoVersionBaja : FilterBase
    {
        [Range(1, int.MaxValue, ErrorMessage = "Id no puede ser inferior a 1")]
        [Required(ErrorMessage = "Id es obligatorio")]
        public int Id { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "IdDocumento no puede ser inferior a 1")]
        [Required(ErrorMessage = "IdDocumento es obligatorio")]
        public int IdDocumento { get; set; }
    }
}
