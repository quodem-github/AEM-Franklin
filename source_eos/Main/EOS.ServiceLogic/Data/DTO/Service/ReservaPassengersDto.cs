
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class ReservaPassengersDto
    {
        [DataMember(Name = "idreservapassengerlist")]
        public int Idreservapassengerlist { get; set; }
        [DataMember(Name = "idpassengerlist")]
        public int Idpassengerlist { get; set; }
        [DataMember(Name = "locked")]
        public int? Locked { get; set; }
        [DataMember(Name = "idxpediente")]
        public int Idxpediente { get; set; }
    }
}
