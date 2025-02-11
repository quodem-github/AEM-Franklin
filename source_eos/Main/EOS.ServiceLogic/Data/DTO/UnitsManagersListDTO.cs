using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO
{
    public class UnitsManagersListDTO
    {
        public List<UnitManager> UnitsManagersList { get; set; }

        public UnitsManagersListDTO() 
        {
            UnitsManagersList = new List<UnitManager>();
        }
    }
}
