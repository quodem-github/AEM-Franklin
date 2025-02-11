using System;
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class TarifasActividadDto
    {
        [DataMember]
        public int Idtarifaactividad { get; set; }
        [DataMember]
        public int Fkidcongreso { get; set; }
        [DataMember]
        public int Idtipoactividadcongreso { get; set; }
        [DataMember]
        public int IdProveedor { get; set; }
        [DataMember]
        public string Idproducto { get; set; }
        [DataMember]
        public string Actividad { get; set; }
        [DataMember]
        public double? Pvp { get; set; }
        [DataMember]
        public string Notascli { get; set; }
        [DataMember]
        public int? Prepago { get; set; }
        [DataMember]
        public int? Socio { get; set; }
        [DataMember]
        public string Cancelacion { get; set; }
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
        [DataMember]
        public string Fechainicio { get; set; }
        [DataMember]
        public string Fechafin { get; set; }
        [DataMember(Name = "descripcion")]
        public string Descripcion { get; set; }
    }
}
