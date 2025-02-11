
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class TiposActividadCongresoDto
    {
        [DataMember]
        public int Idtipoactividadcongreso { get; set; }
        [DataMember]
        public string Tipoactividadcongreso { get; set; }
        [DataMember]
        public int? Orden { get; set; }
        [DataMember]
        public int? Locked { get; set; }
        [DataMember(Name = "codtipoactividad")]
        public string Codtipoactividad { get; set; }
    }
}
