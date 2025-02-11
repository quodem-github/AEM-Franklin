
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class ActividadesPassengersListDto
    {
        [DataMember(Name = "idactividadpassengerlist")]
        public int Idactividadpassengerlist { get; set; }
        [DataMember(Name = "idservicioactividad")]
        public int Idservicioactividad { get; set; }
        [DataMember(Name = "idpassengerlist")]
        public int Idpassengerlist { get; set; }
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
    }
}