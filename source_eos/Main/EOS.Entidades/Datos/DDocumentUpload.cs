using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DDocumentUpload
    {
        public object Fichero { get; set; }
        public int Tipo { get; set; }
        public int? SubTipo { get; set; }
        public int? Peticionario { get; set; }
        public string NombreAdicional { get; set; }
        public bool NuevaVersion { get; set; }
        public string DocumentoOriginal { get; set; }
        public string Asistente { get; set; }
        public string Amec { get; set; }
        public string Expediente { get; set; }
        public int Indexhelper { get; set; }
        public int? IdAmecDocCategory { get; set; }
    }

    public class DDocumentUploadList
    {
        public List<DDocumentUpload> List { get; set; }

        public DDocumentUploadList()
        {
            //List = new List<DDocumentUpload>() {new DDocumentUpload()};
        }
    }
}
