using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.Filters
{
    public class FilterTipoServicio:FilterBase
    {
        public string Descripcion { get; set; }
        public int Idtiposervicio { get; set; }
    }
}
