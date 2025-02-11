using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class JustificacionesDatosAdicionalesDto
    {
        [DataMember(Name = "idjustificaciones")]
        public int Idjustificaciones { get; set; }
        [DataMember(Name = "justificaciones")]
        public string Justificaciones { get; set; }
        [DataMember(Name = "inactivo")]
        public int Inactivo { get; set; }

    }
}
