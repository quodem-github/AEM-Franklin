using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class ServiciosReservasActividadesDto
    {
        [DataMember]
        public int Idservicioactividad { get; set; }
        [DataMember]
        public int? Idtarifaactividad { get; set; }
        [DataMember]
        public string Observaciones { get; set; }
        [DataMember]
        public double? Pvp { get; set; }
        [DataMember]
        public int? Locked { get; set; }
        [DataMember]
        public string Sede { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public string Tipo { get; set; }
        [DataMember]
        public string Fechainicio { get; set; }
        [DataMember]
        public string Horainicio { get; set; }
        [DataMember]
        public string Minutosinicio { get; set; }
        [DataMember]
        public string Fechafin { get; set; }
        [DataMember]
        public string Horafin { get; set; }
        [DataMember(Name = "minutosfin")]
        public string Minutosfin { get; set; }
        [DataMember(Name = "pax")]
        public int? Pax { get; set; }


    }
}
