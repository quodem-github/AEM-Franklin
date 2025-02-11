using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class ReservaViajesDto
    {
        [DataMember]
        public int Idreserva { get; set; }
        [DataMember]
        public string Reserva { get; set; }
        [DataMember]
        public string Fechapeticion { get; set; }
        [DataMember]
        public string Idestado { get; set; }
        [DataMember]
        public string LastUpd { get; set; }
        [DataMember]
        public string LastLog { get; set; }
        [DataMember]
        public int IdPeticionario { get; set; }
        [DataMember]
        public string Observaciones { get; set; }
        [DataMember(Name = "observ_agencia")]
        public string ObservAgencia { get; set; }
        [DataMember]
        public string Mainreserva { get; set; }
        [DataMember]
        public int Fkidexpediente { get; set; }
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }


    }
}
