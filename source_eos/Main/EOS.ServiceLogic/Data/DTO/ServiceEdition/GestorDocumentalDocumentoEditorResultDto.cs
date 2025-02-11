using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    public class GestorDocumentalDocumentoEditorResultDto
    {
        public int Id { get; set; }
        public string IdAmec { get; set; }
        public int? IdExpediente { get; set; }
        public int IdTipodoc { get; set; }
        public int? IdSubTipodoc { get; set; }
        public int? IdPassengerList { get; set; }
        public string CampoLibre { get; set; }
        public string NombreOriginal { get; set; }
        public bool NuevaVersion { get; set; }
        public int? IdDocumentoVersion { get; set; }
        public string FechaCreacionDocumento { get; set; }
        public string FechaModificacionDocumento { get; set; }
        public string FechaCreacionDocumentoVersion { get; set; }
        public string FechaModificacionDocumentoVersion { get; set; }
        public bool DocumentoNoValido { get; set; }
        public int version { get; set; }
    }
}
