using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class ProveedorResponseDto :ResponseBase
    {
        public List<ProveedorDto> ProveedorList { get; set; }
        public int? IdProveedor { get; set; }
        public string Proveedor { get; set; }
    }
}
