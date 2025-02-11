using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Filtros
{
    public class FiltroParticipantes
    {
        public int? IdParticipante { get; set; }
        public string Nombre { get; set; }
        public string Apel1 { get; set; }
        public string Apel2 { get; set; }
        public string Hospital { get; set; }
        public string MSDID { get; set; }
        public int? IdDistrito { get; set; }
        public int? IdRegion { get; set; }
        public int? IdEmpresa { get; set; }
        public int? IdExpediente { get; set; }
        public int? IdAmec { get; set; }
        public string Existe { get; set; }
        public string IdExpedienteConjunto { get; set; }

        public string SortParameter { get; set; }
        public int? StartRowIndex { get; set; }
        public int? MaximumRows { get; set; }
    }
}
