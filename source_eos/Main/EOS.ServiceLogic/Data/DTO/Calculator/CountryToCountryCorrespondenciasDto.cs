
using System;
using System.Runtime.Serialization;

namespace EOS.ServiceLogic.Data.DTO.Calculator
{
    [Serializable]
    [DataContract]
    public class CountryToCountryCorrespondenciasDto
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long? IdType { get; set; }
        [DataMember]
        public long? IdGlobalType { get; set; }
        [DataMember]
        public long? IdNumSpeaksType { get; set; }
        [DataMember]
        public long? IdCitiesType { get; set; }
        [DataMember]
        public long? IdNumSpeaksModeratorsType { get; set; }
        [DataMember]
        public string TableText { get; set; }
        [DataMember]
        public long? IdStandarValue { get; set; }
        [DataMember]
        public string IdStandarValueAppend { get; set; }
        [DataMember]
        public long? IdMaxValue { get; set; }
        [DataMember]
        public string IdMaxValueAppend { get; set; }
        [DataMember]
        public long? IdDaysNoSpeak { get; set; }
        [DataMember]
        public long? Aditional { get; set; }
        [DataMember]
        public string AditionalAppend { get; set; }
    }
}
