
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class PoblacionDto
    {
        [DataMember(Name = "IdPoblacion")]
        public int Idpoblacion { get; set; }
        [DataMember]
        public string Poblacion { get; set; }
        [DataMember(Name = "IdProvincia")]
        public string Idprovincia { get; set; }
        [DataMember(Name = "IdPais")]
        public string Idpais { get; set; }
        [DataMember(Name = "CodPostal")]
        public string Codpostal { get; set; }
        [DataMember]
        public int? Locked { get; set; }
        [DataMember (Name = "IdPaisABC")]
        public int? Idpaisabc { get; set; }
        [DataMember(Name = "inactivo")]
        public int? Inactivo { get; set; }
        public System.Nullable<int> Idconfempresa { get; set; }
    }
}