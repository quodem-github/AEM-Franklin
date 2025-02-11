using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class NivelRiesgoHCPDto
    {
        [DataMember(Name = "idnivelriesgo")]
        public long Idnivelriesgo
        {
            get;
            set;
        }
        [DataMember(Name = "nivelriesgo")]
        public string Nivelriesgo
        {
            get;
            set;
        }
    }
}
