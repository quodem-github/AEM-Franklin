using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO
{
    public class UnitsListDTO
    {
        public List<Unit> UnitsList { get; set; }

        public UnitsListDTO() 
        {
            UnitsList = new List<Unit>();
        }
    }
}
