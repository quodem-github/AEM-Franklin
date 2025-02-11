
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class TiposAlojamientoDto
    {
        [DataMember(Name = "IdTipoAloj")]
        public int Idtipoaloj { get; set; }
        [DataMember(Name = "TipoAloj")]
        public string Tipoaloj { get; set; }
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
    }
}
