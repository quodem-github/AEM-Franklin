using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class DFicheroFecha
    {
        public string NombreFichero { get; set; }
        public string NombreFicheroEncriptado { get; set; }
        public bool DocumentoNoValido { get; set; }
        public DateTime FechaFichero { get; set; }
    }
}
