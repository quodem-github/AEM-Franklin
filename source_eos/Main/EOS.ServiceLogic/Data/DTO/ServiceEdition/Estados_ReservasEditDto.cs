using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class Estados_ReservasEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idregistre no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idregistre es obligatorio")]
        [DataMember]
        public int Idregistre { get; set; }
        [Required(ErrorMessage = "Idestadoinicial es obligatorio")]
        [DataMember]
        public string Idestadoinicial { get; set; }
        [Required(ErrorMessage = "Idestadofinal es obligatorio")]
        [DataMember]
        public string Idestadofinal { get; set; }
        [Range(0, 1, ErrorMessage = "Enviado tiene que tener un valor entre 0 y 1")]
        [DataMember]
        public int? Enviado { get; set; }
        [Required(ErrorMessage = "Fechacambioestado es obligatorio")]
        [DataMember]
        public string Fechacambioestado { get; set; }
        [Range(0, 1, ErrorMessage = "Locked tiene que tener un valor entre 0 y 1")]
        [DataMember]
        public int? Locked { get; set; }
        [DataMember]
        public string Tipo { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idreserva no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idreserva es obligatorio")]
        [DataMember]
        public int Idreserva { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idexpediente no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idexpediente es obligatorio")]
        [DataMember]
        public int Idexpediente { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "WaitAck no puede ser inferior a 0")]
        [DataMember(Name = "wait_ack")]
        public int? WaitAck { get; set; }
        [DataMember]
        public string Datasync { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Sync no puede ser inferior a 0")]
        [Required(ErrorMessage = "Sync es obligatorio")]
        [DataMember]
        public int Sync { get; set; }
        [DataMember(Name = "transaction_key")]
        public string TransactionKey { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idservicio no puede ser inferior a 0")]
        [DataMember]
        public int? Idservicio { get; set; }
        [DataMember(Name = "xml_data")]
        public string XmlData { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "EnviadoAgencia no puede ser inferior a 0")]
        [DataMember(Name = "enviado_agencia")]
        public int? EnviadoAgencia { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idtramitacion no puede ser inferior a 0")]
        [DataMember(Name = "idtramitacion")]
        public int? Idtramitacion { get; set; }

        public  Reservasviajes Reservasviajes
        {
            get;
            set;
        }

        public  Estadosreservas Estadosreservas_idestadofinal
        {
            get;
            set;
        }

        public  Estadosreservas Estadosreservas_idestadoinicial
        {
            get;
            set;
        }
    }
}
