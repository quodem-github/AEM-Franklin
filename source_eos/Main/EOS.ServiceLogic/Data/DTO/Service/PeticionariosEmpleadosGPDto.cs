
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class PeticionariosEmpleadosGPDto
    {
        [DataMember(Name = "id")]
        public int id { get; set; }
        [DataMember(Name = "idpeticionario")]
        public int idpeticionario { get; set; }
        [DataMember(Name = "idempleadogp")]
        public int idempleadogp { get; set; }
        [DataMember(Name = "locked")]
        public int? locked { get; set; }
    }
}
