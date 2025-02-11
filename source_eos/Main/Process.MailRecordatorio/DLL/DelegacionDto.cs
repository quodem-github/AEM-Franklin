using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace msd.MailRecordatorio.DLL
{
    public class DelegacionDto
    {
        public string NombreUsuario { get; set; }
        public string NombreDelegado { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
    }
}
