using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DGestorDocumentos
    {
        public string Ruta { get; set; }
        public List<DGestorFicheros> Ficheros { get; set; }
        public List<DGestorDocumentos> SubTipo { get; set; }
    }
}
