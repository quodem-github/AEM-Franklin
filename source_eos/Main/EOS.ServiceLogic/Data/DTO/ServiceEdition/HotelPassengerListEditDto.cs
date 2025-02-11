

using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class HotelPassengerListEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idhotelpassengerlist no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idhotelpassengerlist es obligatorio")]
        [DataMember]
        public int? Idhotelpassengerlist { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idserviciohotel no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idserviciohotel es obligatorio")]
        [DataMember]
        public int? Idserviciohotel { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idpassengerlist no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idpassengerlist es obligatorio")]
        [DataMember]
        public int? Idpassengerlist { get; set; }
        [Range(0, 1, ErrorMessage = "locked no puede ser inferior a 0 ni superior a 1")]
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }

        public PassengersList PassengersList
        {
            get;
            set;
        }


        public Serviciosreservashotel Serviciosreservashotel
        {
            get;
            set;
        }
    }
}
