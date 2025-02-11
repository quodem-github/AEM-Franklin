using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class TipoActividadDto
    {
        [DataMember (Name = "tipoactividad")]
        public string Tipoactividad1 { get; set; }
        [DataMember (Name = "idtipoactividad")]
        public int Idtipoactividad { get; set; }
    }
}
