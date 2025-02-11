
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class ProductoDto
    {
        [DataMember]
        public string Idproducto { get; set; }
        [DataMember]
        public string Producto { get; set; }
        [DataMember]
        public int? Inactivo { get; set; }
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
    }
}
