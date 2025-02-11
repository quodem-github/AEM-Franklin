using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class GestorDocumentalDocumentoResponseDto : ResponseBase
    {
        public List<GestorDocumentalDocumentoDto> GestorDocumentalDocumentoList { get; set; }
        public int? Id { get; set; }
        public string IdAmec { get; set; }
        public int? IdExpediente { get; set; }
        public int? IdTipoDoc { get; set; }
        public int? IdSubTipoDoc { get; set; }
        public DateTime? CreationDateFrom { get; set; }
        public DateTime? CreationDateTo { get; set; }
        public DateTime? UpdateDateFrom { get; set; }
        public DateTime? UpdateDateTo { get; set; }
    }
}
