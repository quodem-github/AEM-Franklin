using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class TramitacionesServiciosReservasDto
    {
        [DataMember(Name = "IdTramitacion")]
        public int Idtramitacion { get; set; }
        [DataMember]
        public int Fkidservicio { get; set; }
        [DataMember]
        public int Linea { get; set; }
        [DataMember]
        public string Requerimientos { get; set; }
        [DataMember(Name = "alternativa1")]
        public string Alternativa1 { get; set; }
        [DataMember(Name = "okalternativa1")]
        public string Okalternativa1 { get; set; }
        [DataMember(Name = "alternativa2")]
        public string Alternativa2 { get; set; }
        [DataMember(Name = "okalternativa2")]
        public string Okalternativa2 { get; set; }
        [DataMember(Name = "alternativa3")]
        public string Alternativa3 { get; set; }
        [DataMember(Name = "okalternativa3")]
        public string Okalternativa3 { get; set; }
        [DataMember]
        public string Idestado { get; set; }
        [DataMember(Name = "pvp1")]
        public double? Pvp1 { get; set; }
        [DataMember(Name = "pvp2")]
        public double? Pvp2 { get; set; }
        [DataMember(Name = "pvp3")]
        public double? Pvp3 { get; set; }
        [DataMember(Name = "requerimientos2")]
        public string Requerimientos2 { get; set; }
        [DataMember(Name = "IdProveedor1")]
        public int? Idproveedor1 { get; set; }
        [DataMember(Name = "IdProveedor2")]
        public int? Idproveedor2 { get; set; }
        [DataMember(Name = "IdProveedor3")]
        public int? Idproveedor3 { get; set; }
        [DataMember(Name = "IdProductoPrv1")]
        public int? Idproductoprv1 { get; set; }
        [DataMember(Name = "IdProductoPrv2")]
        public int? Idproductoprv2 { get; set; }
        [DataMember(Name = "IdProductoPrv3")]
        public int? Idproductoprv3 { get; set; }
        [DataMember(Name = "requerimientos3")]
        public string Requerimientos3 { get; set; }
        [DataMember]
        public string Validez1 { get; set; }
        [DataMember]
        public string Validez2 { get; set; }
        [DataMember]
        public string Validez3 { get; set; }
        [DataMember(Name = "IdProveedor4")]
        public int? Idproveedor4 { get; set; }
        [DataMember(Name = "IdProveedor5")]
        public int? Idproveedor5 { get; set; }
        [DataMember(Name = "IdProveedor6")]
        public int? Idproveedor6 { get; set; }
        [DataMember(Name = "gastoscancelacion1")]
        public string Gastoscancelacion1 { get; set; }
        [DataMember(Name = "gastoscancelacion2")]
        public string Gastoscancelacion2 { get; set; }
        [DataMember(Name = "gastoscancelacion3")]
        public string Gastoscancelacion3 { get; set; }
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }

    }
}
