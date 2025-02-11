using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.Entidades
{
    public class InformacionExpediente
    {
        public decimal importe { get; set; }
        public List<DetalleServicios> detalleservicios { get; set;}
        public string LugarRealizacion;
        public DateTime FechaComienzo;
        public DateTime FechaFin; 
    }
}
