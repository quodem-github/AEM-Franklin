using System;

namespace EOS.Entidades.Filtros
{
    public class FiltroAprovadorAmec
    {
        public int? IdAprobadorAmec { get; set; }
        public string IdAmecs { get; set; }
        public int? IdAprobador { get; set; }
        public int? IdCreador { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int? AprobacionJefe { get; set; }
    }
}
