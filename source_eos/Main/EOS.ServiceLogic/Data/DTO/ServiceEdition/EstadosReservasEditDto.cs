using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class EstadosReservasEditDto
    {


        [Required(ErrorMessage = "idestado es obligatorio")]
        [DataMember]
        public string Idestado { get; set; }
        [Required(ErrorMessage = "estado es obligatorio")]
        [DataMember]
        public string Estado { get; set; }
        [Range(0, 1, ErrorMessage = "Locked no puede ser inferior a 0 ni superior a 1")]
        [DataMember]
        public int? Locked { get; set; }


        public List<Expediente> Expedientes
        {
            get;
            set;
        }


        public List<Reservasviajes> Reservasviajes
        {
            get;
            set;
        }

        public List<Tramitacionesserviciosreservas> Tramitacionesserviciosreservas
        {
            get;
            set;
        }

        public List<EstadosReservas> EstadosReservas_idestadofinal
        {
            get;
            set;
        }

        public List<EstadosReservas> EstadosReservas_idestadoinicial
        {
            get;
            set;
        }
    }
}
