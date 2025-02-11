using EOS.ServiceModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    class ServiciosReservasViajesEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idservicio no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idservicio es obligatorio")]
        [DataMember(Name = "Idservicio")]
        public int Idservicio { get; set; }

        [Required(ErrorMessage = "Idreserva es obligatorio.")]
        [DataMember(Name = "idreserva")]
        public int Idreserva { get; set; }

        [Required(ErrorMessage = "Idtipobono es obligatorio.")]
        [DataMember(Name = "IdTipoBono")]
        public string Idtipobono { get; set; }

        [Required(ErrorMessage = "Fechapeticion es obligatorio.")]
        [DataMember(Name = "fechapeticion")]
        public string Fechapeticion { get; set; }

        [Required(ErrorMessage = "Resumenservicio es obligatorio.")]
        [DataMember(Name = "resumenservicio")]
        public string Resumenservicio { get; set; }

        [Range(0, 1, ErrorMessage = "Cotizado no puede ser inferior a 0 ni superior a 1")]
        [DataMember(Name = "Cotizado")]
        public int? Cotizado { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Idservicioinscripcion no puede ser inferior a 0")]
        [DataMember(Name = "Idservicioinscripcion")]
        public int? Idservicioinscripcion { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Idserviciohotel no puede ser inferior a 0")]
        [DataMember(Name = "idserviciohotel")]
        public int? Idserviciohotel { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Idservicioactividad no puede ser inferior a 0")]
        [DataMember(Name = "idservicioactividad")]
        public int? Idservicioactividad { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Idserviciotransporte no puede ser inferior a 0")]
        [DataMember(Name = "idserviciotransporte")]
        public int? Idserviciotransporte { get; set; }

        [Range(0, 1, ErrorMessage = "Locked no puede ser inferior a 0 ni superior a 1")]
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "IdtransporteServicioavion1 no puede ser inferior a 0")]
        [DataMember(Name = "idtransporte_servicioavion1")]
        public int? IdtransporteServicioavion1 { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "IdtransporteServiciotren1 no puede ser inferior a 0")]
        [DataMember(Name = "idtransporte_serviciotren1")]
        public int? IdtransporteServiciotren1 { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "IdtransporteServiciocar1 no puede ser inferior a 0")]
        [DataMember(Name = "idtransporte_serviciocar1")]
        public int? IdtransporteServiciocar1 { get; set; }

        [DataMember(Name = "importeReserva")]
        public double? Importereserva { get; set; }

        //--

        public Reservasviajes Reservasviajes
        {
            get { return new Reservasviajes(); }
            set { }
        }

        public Serviciosreservasactividades Serviciosreservasactividades
        {
            get { return new Serviciosreservasactividades(); }
            set { }
        }

        public Serviciosreservashotel Serviciosreservashotel
        {
            get { return new Serviciosreservashotel(); }
            set { }
        }

        public Serviciosreservasinscripciones Serviciosreservasinscripciones
        {
            get { return new Serviciosreservasinscripciones(); }
            set { }
        }

        public Serviciosreservastransporte Serviciosreservastransporte
        {
            get { return new Serviciosreservastransporte(); }
            set { }
        }

        public ISet<Tramitacionesserviciosreservas> Tramitacionesserviciosreservas
        {
            get { return new HashSet<Tramitacionesserviciosreservas>(); }
            set { }
        }

    }
}
