using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class GestorInvitadosDto
    {
        [DataMember(Name = "IdGestorInvitados")]
        public long IdGestorInvitados { get; set; }
        [DataMember(Name = "IdEventoFormulario")]
        public long Ideventoformulario { get; set; }
        [DataMember(Name = "IdCongreso")]
        public int? Idcongreso { get; set; }
        [DataMember(Name = "DescripcionGestor")]
        public string Descripciongestor { get; set; }
        [DataMember(Name = "FechaInicio")]
        public string Fechainicio { get; set; }
        [DataMember(Name = "FechaFin")]
        public string Fechafin { get; set; }
        [DataMember]
        public string Poblacion { get; set; }
        [DataMember(Name = "LinkGestorInvitados")]
        public string Linkgestorinvitados { get; set; }
        [DataMember(Name = "Linkprograma")]
        public string Linkprograma { get; set; }
        [DataMember]
        public string Amec { get; set; }
        [DataMember(Name = "TipoGestorInvitados")]
        public string Tipogestorinvitados { get; set; }
        [DataMember]
        public int? Inactivo { get; set; }
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
        public int? Idconfempresa { get; set; }
    }
}
