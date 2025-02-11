using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Filtros
{
    public class FiltroAMECCongreso
    {
        public string IdAMEC { get; set; }
        public int? IdCongreso { get; set; }
        public int? IdPoblacion { get; set; }
        public int? TipoActividad { get; set; }
        public string AmecLike { get; set; }
        public string NombreEvento { get; set; }
        public string LugarEvento { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }


        public string SortParameter { get; set; }
        public int? StartRowIndex { get; set; }
        public int? MaximumRows { get; set; }
    }
}
