using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class IatasDto
    {
        public int Id { get; set; }
        public int Idconfempresa { get; set; }
        [DataMember]
        public string Iata { get; set; }
        [DataMember(Name = "Idpoblacion")]
        public int? IdPoblacion { get; set; }
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
        [DataMember]
        public string Idiata { get; set; }

    }
}
