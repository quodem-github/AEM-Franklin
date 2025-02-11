using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class PositionsDto
    {
        [DataMember(Name = "idposition")]
        public int Idposition { get; set; }
        [DataMember(Name = "position")]
        public string Position { get; set; }
        [DataMember(Name = "Inactivo")]
        public int? Inactivo { get; set; }
    }
}
