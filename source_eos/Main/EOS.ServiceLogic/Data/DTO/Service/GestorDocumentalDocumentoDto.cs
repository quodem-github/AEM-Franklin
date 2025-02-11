using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class GestorDocumentalDocumentoDto : GestorDocumentalBase
    {
        public string IdAmec { get; set; }
        public int? IdExpediente { get; set; }
        public int IdTipodoc { get; set; }
        public int? IdSubTipodoc { get; set; }
        public int? IdPassengerList { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaModificacion { get; set; }
        public string CampoLibre { get; set; }

    }
}
