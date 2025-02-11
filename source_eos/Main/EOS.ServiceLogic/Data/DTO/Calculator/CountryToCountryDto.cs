using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EOS.ServiceLogic.Data.DTO.Calculator
{
    [Serializable]
    [DataContract]
    public class CountryToCountryDto
    {
        public List<CountryToCountryCitiesTypeDto> CountryToCountryCitiesTypeDtoList { get; set; }
        public List<CountryToCountryCorrespondenciasDto> CountryToCountryCorrespondenciasDtoList { get; set; }
        public List<CountryToCountryGlobalTypeDto> CountryToCountryGlobalTypeDtoList { get; set; }
        public List<CountryToCountryNumSpeaksModerTypeDto> CountryToCountryNumSpeaksModerTypeDtoList { get; set; }
        public List<CountryToCountryNumSpeaksTypeDto> CountryToCountryNumSpeaksTypeDtoList { get; set; }
        public List<CountryToCountryReasonsTypeDto> CountryToCountryReasonsTypeDtoList { get; set; }
        public List<CountryToCountryTypeDto> CountryToCountryTypeDtoList { get; set; }
    }
}
