using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class InscripcionPassengersListEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idinspassengerlist no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idinspassengerlist es obligatorio")]
        [DataMember(Name = "idinspassengerlist")]
        public int Idinspassengerlist { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idpassengerlist no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idpassengerlist es obligatorio")]
        [DataMember]
        public int Idpassengerlist { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idservicioinscripcion no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idservicioinscripcion es obligatorio")]
        [DataMember]
        public int Idservicioinscripcion { get; set; }
        [Range(0, 1, ErrorMessage = "locked no puede ser inferior a 0 ni superior a 1")]
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }

        
        public  PassengersList PassengersList
        {
            get;
            set;
        }

        public  Serviciosreservasinscripciones Serviciosreservasinscripciones
        {
            get;
            set;
        }
    }
}