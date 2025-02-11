using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class ComunidadDto
    {
        [DataMember]
        public int IdComunidad { get; set; }
        [DataMember(Name = "Comunidad")]
        public string Comunidad1 { get; set; }
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
    }
}
