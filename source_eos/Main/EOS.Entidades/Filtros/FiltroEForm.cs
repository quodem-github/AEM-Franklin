using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Filtros
{
    public class FiltroEForm
    {
        public int? IdCongreso { get; set; }

        public Boolean? Publicar { get; set; }
        public string SortParameter { get; set; }
        public int? StartRowIndex { get; set; }
        public int? MaximumRows { get; set; }
    }
}
