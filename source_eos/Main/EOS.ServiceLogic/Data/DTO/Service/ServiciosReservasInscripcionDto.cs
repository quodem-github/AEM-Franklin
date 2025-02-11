
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class ServiciosReservasInscripcionDto
    {
        [DataMember]
        public int Idservicioinscripcion { get; set; }
        [DataMember]
        public string Inscripcion { get; set; }
        [DataMember]
        public string Envioboletin { get; set; }
        [DataMember]
        public string Tipoinscripcion { get; set; }
        [DataMember]
        public string Otros { get; set; }
        [DataMember]
        public string Observaciones { get; set; }
        [DataMember(Name = "observ_agencia")]
        public string ObservAgencia { get; set; }
        [DataMember]
        public int? Iddatosentrega { get; set; }
        [DataMember(Name = "idtarifainscripcion")]
        public int? Idtarifainscripcion { get; set; }
        [DataMember]
        public double? Pvp { get; set; }
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
    }
}
