using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Service
{[DataContract    ]
    public class TiposHabitacionDto
    {
        [DataMember(Name = "Idtipohab")]
        public string IdTipoHab { get; set; }
        [DataMember(Name = "Tipohab")]
        public string TipoHab { get; set; }
        [DataMember]
        public int? Ocup { get; set; }
        [DataMember]
        public int? Locked { get; set; }
        
    }
}
