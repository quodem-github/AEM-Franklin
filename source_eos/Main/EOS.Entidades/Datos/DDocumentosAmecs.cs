using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DDocumentosAmecs
    {
        public string Ruta { get; set; }
        public List<DFicherosAmecs> Ficheros { get; set; }
    }
}
