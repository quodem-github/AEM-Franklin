
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class ServiciosReservasViajesDto
    {
        [DataMember]
        public int Idservicio { get; set; }
        [DataMember]
        public int Idreserva { get; set; }
        [DataMember(Name = "IdTipoBono")]
        public string Idtipobono { get; set; }
        [DataMember]
        public string Fechapeticion { get; set; }
        [DataMember]
        public string Resumenservicio { get; set; }
        [DataMember]
        public int? Cotizado { get; set; }
        [DataMember]
        public int? Idservicioinscripcion { get; set; }
        [DataMember]
        public int? Idserviciohotel { get; set; }
        [DataMember]
        public int? Idservicioactividad { get; set; }
        [DataMember(Name = "idserviciotransporte")]
        public int? Idserviciotransporte { get; set; }
        [DataMember]
        public int? Locked { get; set; }
        [DataMember(Name = "idtransporte_servicioavion1")]
        public int? IdtransporteServicioavion1 { get; set; }
        [DataMember(Name = "idtransporte_serviciotren1")]
        public int? IdtransporteServiciotren1 { get; set; }
        [DataMember(Name = "idtransporte_serviciocar1")]
        public int? IdtransporteServiciocar1 { get; set; }
        [DataMember(Name = "importeReserva")]
        public double? Importereserva { get; set; }

    }
}
