using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class DepartamentsDto
    {
        [DataMember]
        public int Iddepartament { get; set; }
        [DataMember(Name = "IdEmpresa")]
        public int Idempresa { get; set; }
        [DataMember(Name = "departament")]
        public string Departament { get; set; }
        [DataMember]
        public int? Inactivo { get; set; }
    }
}
