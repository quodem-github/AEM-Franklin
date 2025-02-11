using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class DistrictsDto
    {
        [DataMember(Name = "iddistrict")]
        public  int Iddistrict{ get; set; }
        [DataMember(Name = "idsaleforce")]
        public  int? Idsaleforce { get; set; }
        [DataMember(Name = "IdEmpresa")]
        public int Idempresa { get; set; }
        [DataMember(Name = "district")]
        public string District { get; set; }
        [DataMember(Name = "inactivo")]
        public int? Inactivo { get; set; }
    }
}
