using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class ProductosPRVDto
    {
        public int Fkidproveedor { get; set; }
        public short Inactivo { get; set; }
        public string Desproducto { get; set; }
        public string Idproducto { get; set; }
        public int? Locked { get; set; }
        public int Idproductoprv { get; set; }
    }
}
