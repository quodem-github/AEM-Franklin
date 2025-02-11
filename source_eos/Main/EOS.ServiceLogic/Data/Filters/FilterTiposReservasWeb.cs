using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterTiposReservasWeb : FilterBase
    {
        public string Descripcion { get; set; }
        public int Idtiporeserva { get; set; }
    }
}
