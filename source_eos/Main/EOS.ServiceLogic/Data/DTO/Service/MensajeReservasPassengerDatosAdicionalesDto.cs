using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class MensajeReservasPassengerDatosAdicionalesDto
    {
        [DataMember]
        public long Idmensaje { get; set; }
        [DataMember]
        public long Idnivelriesgo { get; set; }
        [DataMember]
        public long Idtipoactividadpax { get; set; }
        [DataMember]
        public long Idtipoasistente { get; set; }
        [DataMember(Name = "mensaje")]
        public string Mensaje { get; set; }
    }
}
