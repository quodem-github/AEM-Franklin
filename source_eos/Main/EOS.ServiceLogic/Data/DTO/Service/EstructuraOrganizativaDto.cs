using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class EstructuraOrganizativaDto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdPeticionario { get; set; }
        [DataMember]
        public int? IdPeticionarioManager { get; set; }
        [DataMember]
        public string Wein { get; set; }
        [DataMember]
        public string WeinManager { get; set; }
        [DataMember(Name = "UpdateDate")]
        public string Lastupdatedate { get; set; }
    }
}
