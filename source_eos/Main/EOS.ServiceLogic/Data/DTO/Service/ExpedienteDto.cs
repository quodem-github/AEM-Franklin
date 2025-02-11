
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class ExpedienteDto
    {
        [DataMember]
        public int Idxpediente { get; set; }
        [DataMember]
        public int? Idregion { get; set; }
        [DataMember]
        public int Idamec { get; set; }
        [DataMember(Name = "Expediente")]
        public string Expediente1 { get; set; }
        [DataMember]
        public int? Idunidad { get; set; }
        [DataMember]
        public int? Idarea { get; set; }
        [DataMember(Name = "IdPeticionario")]
        public int Idpeticionario { get; set; }
        [DataMember(Name = "IdTiporeserva")]
        public int IdtipoReserva { get; set; }
        [DataMember(Name = "Idempresa")]
        public int Idempresa { get; set; }
        [DataMember]
        public string Idestado { get; set; }
        [DataMember]
        public int? Iddistrito { get; set; }
        [DataMember]
        public int? Locked { get; set; }
        [DataMember]
        public string Codexpediente { get; set; }
        [DataMember]
        public int? Urgente { get; set; }
        [DataMember(Name = "importetotal")]
        public double? Importetotal { get; set; }
        [DataMember(Name = "visible")]
        public int? Visible { get; set; }
        [DataMember(Name = "Fechacreacion")]
        public string Fechacreacion { get; set; }
        [DataMember(Name = "Iddepartament")]
        public int? Iddepartament { get; set; }
        [DataMember(Name = "Idsalesforce")]
        public int? Idsaleforce { get; set; }
        [DataMember(Name = "Iddistrict")]
        public int? Iddistrict { get; set; }
        [DataMember(Name = "Tipopagofee")]
        public int? Tipopagofee { get; set; }
    }
}
