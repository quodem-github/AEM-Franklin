
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class ReservaPassengersEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "idreservapassengerlist no puede ser inferior a -1")]
        [Required(ErrorMessage = "idreservapassengerlist es obligatorio")]
        [DataMember(Name = "idreservapassengerlist")]
        public int Idreservapassengerlist { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idpassengerlist no puede ser inferior a 0")]
        [Required(ErrorMessage = "idpassengerlist es obligatorio")]
        [DataMember(Name = "idpassengerlist")]
        public int Idpassengerlist { get; set; }
        [Range(0, 1, ErrorMessage = "locked tiene que ser un valor entre 0 y 1")]
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idxpediente no puede ser inferior a 0")]
        [Required(ErrorMessage = "idxpediente es obligatorio")]
        [DataMember(Name = "idxpediente")]
        public int Idxpediente { get; set; }


        public  Expediente Expediente
        {
            get;
            set;
        }

        public  PassengersList PassengersList
        {
            get;
            set;
        }
    }
}
