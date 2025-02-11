using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class InscripcionPassengersListDto
    {
        [DataMember(Name = "idinspassengerlist")]
        public int Idinspassengerlist { get; set; }
        [DataMember]
        public int Idpassengerlist { get; set; }
        [DataMember]
        public int Idservicioinscripcion { get; set; }
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
    }
}