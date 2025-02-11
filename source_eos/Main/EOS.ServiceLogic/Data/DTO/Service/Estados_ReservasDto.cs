using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class Estados_ReservasDto
    {
        [DataMember(Name = "idregistre")]
        public int Idregistre { get; set; }
        [DataMember]
        public string Idestadoinicial { get; set; }
        [DataMember]
        public string Idestadofinal { get; set; }
        [DataMember]
        public int? Enviado { get; set; }
        [DataMember]
        public string Fechacambioestado { get; set; }
        [DataMember]
        public int? Locked { get; set; }
        [DataMember]
        public string Tipo { get; set; }
        [DataMember]
        public int Idreserva { get; set; }
        [DataMember]
        public int Idexpediente { get; set; }
        [DataMember(Name = "wait_ack")]
        public int? WaitAck { get; set; }
        [DataMember]
        public string Datasync { get; set; }
        [DataMember]
        public int Sync { get; set; }
        [DataMember(Name = "transaction_key")]
        public string TransactionKey { get; set; }
        [DataMember]
        public int? Idservicio { get; set; }
        [DataMember(Name = "xml_data")]
        public string XmlData { get; set; }
        [DataMember(Name = "enviado_agencia")]
        public int? EnviadoAgencia { get; set; }
        [DataMember(Name = "idtramitacion")]
        public int? Idtramitacion { get; set; }


    }
}
