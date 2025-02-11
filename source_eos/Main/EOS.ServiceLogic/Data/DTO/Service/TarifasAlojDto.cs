using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class TarifasAlojDto
    {
        [DataMember(Name = "Idtarifaaloj")]
        public int IdTarifaAloj { get; set; }
        [DataMember(Name = "FKIdCongreso")]
        public int Fkidcongreso { get; set; }
        [DataMember(Name = "IdTipoAloj")]
        public int Idtipoaloj { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember(Name = "IdProveedor")]
        public int Idproveedor { get; set; }
        [DataMember]
        public string Idproducto { get; set; }
        [DataMember(Name = "IdTipoHab")]
        public string Idtipohab { get; set; }
        [DataMember(Name = "IdServicio")]
        public string Idservicio { get; set; }
        [DataMember(Name = "PrecioNoche")]
        public double Precionoche { get; set; }
        [DataMember]
        public int? Prepago { get; set; }
        [DataMember]
        public int? Socio { get; set; }
        [DataMember]
        public string Fechainicio { get; set; }
        [DataMember]
        public string Fechafin { get; set; }
        [DataMember]
        public string Cancelacion { get; set; }
        [DataMember]
        public int? Locked { get; set; }
        [DataMember(Name = "visible")]
        public int? Visible { get; set; }


    }
}
