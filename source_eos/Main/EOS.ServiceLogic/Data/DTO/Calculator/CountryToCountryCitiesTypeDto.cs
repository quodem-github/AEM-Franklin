using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Calculator
{
    [Serializable]
    [DataContract]
    public class CountryToCountryCitiesTypeDto
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Text { get; set; }
    }
}
