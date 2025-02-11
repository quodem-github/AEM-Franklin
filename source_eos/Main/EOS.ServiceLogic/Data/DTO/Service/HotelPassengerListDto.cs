

using System.Runtime.Serialization;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class HotelPassengerListDto
    {
        [DataMember]
        public int Idhotelpassengerlist { get; set; }
        [DataMember]
        public int Idserviciohotel { get; set; }
        [DataMember]
        public int Idpassengerlist { get; set; }
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
        public PassengersList PassengersList { get; set; }
    }
}
