using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class CriteriosSeleccionDto
    {
        [DataMember]
        public string Criterioseleccion { get; set; }
        [DataMember]
        public int? Locked { get; set; }
        [DataMember]
        public int Idcriterio { get; set; }
        [DataMember(Name = "idcriterioABC")]
        public long Idcriterioabc { get; set; }
        [DataMember(Name = "inactivo")]
        public short Inactivo { get; set; }


    }
}
