using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class TiposPRVResponseDto : ResponseBase
    {
        public List<TiposPRVDto> TiposPrvList { get; set; }
       
    }
}
