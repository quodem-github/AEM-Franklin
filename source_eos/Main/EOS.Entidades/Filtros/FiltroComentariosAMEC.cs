using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Filtros
{
   public class FiltroComentariosAMEC
    {
        public string IdAmec { get; set; }
        public int? IdUsuario { get; set; }
        public string Comentario { get; set; }
       
        public string SortParameter { get; set; }
        public int? StartRowIndex { get; set; }
        public int? MaximumRows { get; set; }
    }
}



