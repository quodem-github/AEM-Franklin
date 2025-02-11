using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterAprobadorAmec : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "IdPeticionario no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idaprobador es obligatorio")]
        public int IdAprobador { get; set; }
        public string IdAmecs { get; set; }
    }
}
