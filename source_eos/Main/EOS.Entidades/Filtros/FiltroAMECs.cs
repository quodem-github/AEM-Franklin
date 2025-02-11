using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Filtros
{
    public class FiltroAMECs
    {
        public string IdAMEC { get; set; }
        public int? IdCongreso { get; set; }
        public string AMECLike { get; set; }
        public string CongresoLike { get; set; }
        public string Solicitante { get; set; }
        public string Aprobado { get; set; }
        public string PendientesAprobar { get; set; }
        public string Roles { get; set; }
        public int? Año { get; set; }
        public int? Mes { get; set; }
        public decimal? Importe { get; set; }
        public string TipoFiltroImporte { get; set; }

        public string SortParameter { get; set; }
        public int? StartRowIndex { get; set; }
        public int? MaximumRows { get; set; }
    }
}
