using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class GestorDocumentalDocumentoVersionDto : GestorDocumentalBase
    {
        public int Id { get; set; }
        public int IdDocumento { get; set; }
        public string NombreOriginal { get; set; }
        public int version { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaModificacion { get; set; }
        public bool DocumentoNoValido { get; set; }
    }
}
