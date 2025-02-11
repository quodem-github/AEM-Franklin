using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class ReservasViajesEditDto
    {
     

        [Range(-1, int.MaxValue, ErrorMessage = "Idreserva no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idreserva es obligatorio")]
        [DataMember]
        public int Idreserva { get; set; }

        [Required(ErrorMessage = "Reserva es obligatorio")]
        [DataMember]
        public string Reserva { get; set; }

        [Required(ErrorMessage = "Fechapeticion es obligatorio")]
        [DataMember]
        public string Fechapeticion { get; set; }

        [Required(ErrorMessage = "Idestado es obligatorio")]
        [DataMember]
        public string Idestado { get; set; }

        [DataMember]
        public string Lastupd { get; set; }

        [DataMember]
        public string Lastlog { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Idpeticionario no puede ser inferior a 0")]
        [Required(ErrorMessage = "IdPeticionario es obligatorio")]
        [DataMember]
        public int Idpeticionario { get; set; }

        [DataMember]
        public string Observaciones { get; set; }

        [DataMember(Name = "observ_agencia")]
        public string ObservAgencia { get; set; }

        [DataMember]
        public string Mainreserva { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Fkidexpediente no puede ser inferior a 0")]
        [Required(ErrorMessage = "Fkidexpediente es obligatorio")]
        [DataMember]
        public int Fkidexpediente { get; set; }

        [Range(0, 1, ErrorMessage = "locked tiene que ser entre 0 y 1")]
        [DataMember]
        public int? Locked { get; set; }

        
     

        public Expediente Expediente
        {
            get;
            set;
        }

        public Peticionarios Peticionarios
        {
            get;
            set;
        }
       public Estadosreservas Estadosreservas
        {
            get;
            set;
        } 

        public List<Serviciosreservasviajes> Serviciosreservasviajes
        {
            get;
            set;
        }

        public List<EstadosReservas> EstadosReservas
        {
            get;
            set;
        } 
    }
}
