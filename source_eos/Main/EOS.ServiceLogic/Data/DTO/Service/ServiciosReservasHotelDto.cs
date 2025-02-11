using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class ServiciosReservasHotelDto
    {
        [DataMember]
        public int Idserviciohotel { get; set; }
        [DataMember]
        public string Fechahorallegada { get; set; }
        [DataMember]
        public string Fechahorasalida { get; set; }
        [DataMember(Name = "IdPais")]
        public string Idpais { get; set; }
        [DataMember]
        public string Pais { get; set; }
        [DataMember(Name = "IdProvincia")]
        public string Idprovincia { get; set; }
        [DataMember]
        public string Provincia { get; set; }
        [DataMember(Name = "IdPoblacion")]
        public int? Idpoblacion { get; set; }
        [DataMember]
        public string Poblacion { get; set; }
        [DataMember]
        public int? Idproveedor { get; set; }
        [DataMember]
        public string Hotel { get; set; }
        [DataMember(Name = "IdTipoAloj")]
        public int Idtipoaloj { get; set; }
        [DataMember]
        public string Categoria { get; set; }
        [DataMember]
        public string Observaciones { get; set; }
        [DataMember]
        public string Idtipohab { get; set; }
        [DataMember(Name= "num_habitaciones")]
        public int? NumHabitaciones { get; set; }
        [DataMember(Name = "desc_tipoalojamiento")]
        public string DescTipoalojamiento { get; set; }
        [DataMember(Name = "desc_idtipo_habitacion")]
        public string DescIdtipoHabitacion { get; set; }
        [DataMember]
        public int? Idtarifaaloj { get; set; }
        [DataMember]
        public double? Pvp { get; set; }
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
    }
}
