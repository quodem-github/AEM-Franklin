using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades.Datos
{
    public class ServicioTransporte
    {
        public class Transporte
        {
            public string Tipo { get; set; }
            public DateTime? FechaSalida { get; set; }
            public string NumVueloTren { get; set; }
            public string Origen { get; set; }
            public string Destino { get; set; }
            public string HoraSalida { get; set; }
            public string HoraLlegada { get; set; }
        }

        public IEnumerable<Transporte> Ida { get; set; }
        public IEnumerable<Transporte> Regreso { get; set; }

        public int? PAX { get; set; }
        public float? Cotizado { get; set; }
    }
}
