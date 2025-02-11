using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class TiposBonosDto
    {
        [DataMember(Name = "IdTipoBono")]
        public string Idtipobono{get;set;}
        [DataMember(Name = "TipoBono")]
        public string Tipobono{get;set;}
        [DataMember(Name = "locked")]
        public int? Locked{get;set;}
    }
}
