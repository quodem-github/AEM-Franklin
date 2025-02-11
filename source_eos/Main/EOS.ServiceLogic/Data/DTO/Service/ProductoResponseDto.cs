using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class ProductoResponseDto : ResponseBase
    {
        public List<ProductoDto> ProductosList { get; set; }
    }
}
