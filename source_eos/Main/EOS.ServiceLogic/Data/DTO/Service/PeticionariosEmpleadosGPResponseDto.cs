
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class PeticionariosEmpleadosGPResponseDto : ResponseBase
    {
        [DataMember]
        public List<PeticionariosEmpleadosGPDto> PeticionariosEmpleadosGPList { get; set; }
        [DataMember]
        public int? id { get; set; }
        [DataMember]
        public int? idpeticionario { get; set; }
        [DataMember]
        public int? idempleadogp { get; set; }
        [DataMember]
        public int? locked { get; set; }
    }
}
