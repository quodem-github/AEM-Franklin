using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class TransportePassengersListEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idtransportepassengerlist no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idtransportepassengerlist es obligatorio")]
        [DataMember(Name = "idtransportepassengerlist")]
        public int? Idtransportepassengerlist { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Idserviciotransporte no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idserviciotransporte es obligatorio")]
        [DataMember(Name = "idserviciotransporte")]
        public int? Idserviciotransporte { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Idpassengerlist no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idpassengerlist es obligatorio")]
        [DataMember(Name = "idpassengerlist")]
        public int? Idpassengerlist { get; set; }

        [Range(0, 1, ErrorMessage = "Locked ha de estar entre 0 y 1.")]
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }

        public  PassengersList PassengersList
        {
            get;
            set;
        }


        public  Serviciosreservastransporte Serviciosreservastransporte
        {
            get;
            set;
        }
    }
}
