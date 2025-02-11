using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Filtros
{
    public class FiltroUnidadesOrgAMEC
    {
        public string IdAmec { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdUnidad { get; set; }
        public int? IdArea { get; set; }
        public int? IdRegion { get; set; }
        public int? IdDistrito { get; set; }
                
        public string SortParameter { get; set; }
        public int? StartRowIndex { get; set; }
        public int? MaximumRows { get; set; }
    }


}
