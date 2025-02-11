using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterProductosPRV : FilterBase
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Fkidproveedor no puede ser inferior a -1")]
        [Required(ErrorMessage = "Fkidproveedor es obligatorio")]
        public int Fkidproveedor { get; set; }
        
        [Range(-1, int.MaxValue, ErrorMessage = "Idproductoprv no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idproductoprv es obligatorio")]
        public int Idproductoprv { get; set; }

        public string Desproducto { get; set; }
        public string Idproducto { get; set; }
    }
}
