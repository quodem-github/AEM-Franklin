using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class TarifasInscripcionDto
    {
        [DataMember(Name = "IdTarifaInscripcion")]
        public int Idtarifainscripcion { get; set; }
        [DataMember(Name = "FKIdCongreso")]
        public int Fkidcongreso { get; set; }
        [DataMember(Name = "IdTipoInscripcion")]
        public int Idtipoinscripcion { get; set; }
        [DataMember(Name = "IdProveedor")]
        public int Idproveedor { get; set; }
        [DataMember(Name = "PVP")]
        public double Pvp { get; set; }
        [DataMember(Name = "NotasCli")]
        public string Notascli { get; set; }
        [DataMember]
        public string Idproducto { get; set; }
        [DataMember]
        public int? Prepago { get; set; }
        [DataMember]
        public int? Socio { get; set; }
        [DataMember]
        public string Cancelacion { get; set; }
        [DataMember]
        public int? Locked { get; set; }
        [DataMember(Name="fecha_validez")]
        public string FechaValidez { get; set; }
        [DataMember]
        public string Fechainicio { get; set; }
        [DataMember]
        public string Fechafin { get; set; }
        [DataMember(Name = "descripcion")]
        public string Descripcion { get; set; }


    }
}
