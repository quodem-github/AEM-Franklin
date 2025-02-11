using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Filtros
{
    public class FiltroExpedientes
    {
        public long? IdExpediente {get; set;}
        public string Actividad {get; set;}
        public string Asistente { get; set; }
        public string EstadoExpediente { get; set; }
        public string Amec { get; set; }
        public string EstadoReserva { get; set; }
        public int? Tipo { get; set; }
        public int? Año { get; set; }
        public int? Mes { get; set; }

        public string GroupParameter { get; set; }
        public string SortParameter { get; set; }
        public int? StartRowIndex { get; set; }
        public int? MaximumRows { get; set; }
    }
}
