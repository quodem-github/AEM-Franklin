using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class EstadosReservasDto
    {
        [DataMember(Name = "idestado")]
        public string Idestado { get; set; }
        [DataMember(Name = "estado")]
        public string Estado { get; set; }
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
    }
}
