using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Filtros
{
    public class FiltroFlujoAprobacion
    {
        public int? idTipoActividad { get; set; }
        public int? idTipoFlujo { get; set; }
        public int? orden { get; set; }
        public string fase { get; set; }
        public double? importe { get; set; }
        public int? idNivelInicial { get; set; }
        public int? idNivelSiguiente { get; set; }
        public string PreAprobado { get; set; }
        public int? idcreadopor { get; set; }
        public int? condicionado { get; set; }

        public string SortParameter { get; set; }
        public int? StartRowIndex { get; set; }
        public int? MaximumRows { get; set; }
    }
}
