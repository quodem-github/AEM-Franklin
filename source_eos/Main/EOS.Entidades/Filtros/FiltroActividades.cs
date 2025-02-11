using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Filtros
{
    public class FiltroActividades
    {
        public int? IdCongreso { get; set; }
        public string NombreLike { get; set; }
        public int? IDPoblacion { get; set; }
        public string AmecLike { get; set; }
        public int? TipoActividad { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        public Boolean? Publicar { get; set; }
        public int? IdConfEmpresa { get; set; }
        public string SortParameter { get; set; }
        public int? StartRowIndex { get; set; }
        public int? MaximumRows { get; set; }
        public bool? isAdmin { get; set; }
        public bool? newco { get; set; }
        public bool isGestorInvitados { get; set; } = false;
    }
}
