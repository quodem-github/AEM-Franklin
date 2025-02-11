using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{
    [DataContract]
    public class AprobadorAmecDto
    {
        [DataMember(Name = "IdAprobadorAmec")]
        public int Idaprovadoramec { get; set; }
        [DataMember(Name = "IdAmecs")]
        public string Idamecs { get; set; }
        [DataMember(Name = "IdAprobador")]
        public int Idaprobador { get; set; }
        [DataMember(Name = "IdCreadoPor")]
        public int Idcreadopor { get; set; }
        [DataMember(Name = "FechaCreacion")]
        public string Fechacreacion { get; set; }
        [DataMember(Name = "AprobacionJefe")]
        public int? Aprobacionjefe { get; set; }
    }
}
