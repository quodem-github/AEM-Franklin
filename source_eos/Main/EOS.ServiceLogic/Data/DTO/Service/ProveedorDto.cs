
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class ProveedorDto
    {
        [DataMember (Name = "Idproveedor")]
        public int IdProveedor { get; set; }
        [DataMember (Name = "Idtipoprv")]
        public string IdTipoPrv { get; set; }
        [DataMember(Name = "Codcia")]
        public string CodCia { get; set; }
        [DataMember(Name = "Codamadeus")]
        public string CodAmadeus { get; set; }
        [DataMember]
        public string Enlace { get; set; }
        [DataMember]
        public string Proveedor { get; set; }
        [DataMember]
        public string Direccion { get; set; }
        [DataMember]
        public string Nro { get; set; }
        [DataMember]
        public string Piso { get; set; }
        [DataMember(Name = "IdPoblacion")]
        public int Idpoblacion { get; set; }
        [DataMember(Name = "CodPostal")]
        public string Codpostal { get; set; }
        [DataMember]
        public string Telefono { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember(Name = "http")]
        public string Http { get; set; }
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }

    }
}