using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.ServiceEdition
{
    [DataContract]
    public class ActividadesPassengersListEditDto
    {
        [Range(-1, int.MaxValue, ErrorMessage = "Idactividadpassengerlist no puede ser inferior a -1")]
        [Required(ErrorMessage = "Idactividadpassengerlist es obligatorio")]
        [DataMember(Name = "idactividadpassengerlist")]
        public int? Idactividadpassengerlist { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idservicioactividad no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idservicioactividad es obligatorio")]
        [DataMember(Name = "idservicioactividad")]
        public int? Idservicioactividad { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Idpassengerlist no puede ser inferior a 0")]
        [Required(ErrorMessage = "Idpassengerlist es obligatorio")]
        [DataMember(Name = "idpassengerlist")]
        public int? Idpassengerlist { get; set; }
        [Range(0, 1, ErrorMessage = "Locked no puede ser inferior a 0 ni superior a 1")]
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
        public PassengersList PassengersList { get; set; }
        public Serviciosreservasactividades Serviciosreservasactividades { get; set; }



    }
}