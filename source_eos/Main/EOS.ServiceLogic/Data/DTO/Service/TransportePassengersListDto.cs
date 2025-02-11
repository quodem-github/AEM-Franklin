
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class TransportePassengersListDto
    {
        [DataMember(Name = "idtransportepassengerlist")]
        public int Idtransportepassengerlist { get; set; }
        [DataMember(Name = "idserviciotransporte")]
        public int Idserviciotransporte { get; set; }
        [DataMember(Name = "idpassengerlist")]
        public int Idpassengerlist { get; set; }
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
    }
}
