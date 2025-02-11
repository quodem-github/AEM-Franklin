using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    public class GestorDocumentalTipoDocDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Inactivo { get; set; }
        public bool CampoLibre { get; set; }
        public int Idtipo { get; set; }
    }
}
