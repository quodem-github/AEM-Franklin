using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO
{
    [DataContract]
    public class TratamientosDto
    {
        [DataMember]
        public int Idtratamiento{get;set;}
        [DataMember]
        public string Ttocarta{get;set;}
        [DataMember]
        public string Ttocertificado{get;set;}
        [DataMember]
        public string Ttomsd{get;set;}
        [DataMember(Name = "locked")]
        public int? Locked{get;set;}

    }
}
