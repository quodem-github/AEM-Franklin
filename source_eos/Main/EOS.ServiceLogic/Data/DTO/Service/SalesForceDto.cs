using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class SalesForceDto
    {
        [DataMember(Name = "Idsaleforce")]
        public int Idsaleforce { get; set; }
        [DataMember(Name = "iddepartament")]
        public int? Iddepartament { get; set; }
        [DataMember(Name = "IdEmpresa")]
        public int Idempresa { get; set; }
        [DataMember(Name = "saleforce")]
        public string Saleforce { get; set; }
        [DataMember(Name = "inactivo")]
        public int? Inactivo { get; set; }
    }
}
