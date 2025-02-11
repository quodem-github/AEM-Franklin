using EOS.ServiceLogic.Data;
using EOS.ServiceLogic.Data.DTO;
using EOS.ServiceLogic.Data.DTO.Calculator;
using EOS.ServiceModel;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace EOS.ServiceLogic.BLL.Calculator
{
    public class CountryToCountryService
    {
        #region Definitions

        private ISession _sessVariable;
        public ISession _session
        {
            get
            {
                if (!_sessVariable.IsOpen)
                {
                    _sessVariable = Quodem.ORM.NHibernate.Helper.GetCurrentSession(ServiceLogic.Enums.EDbConnection.Default.GetHashCode());
                }
                return _sessVariable;
            }
            set { _sessVariable = value; }
        }

        #endregion

        #region Constructor

        public CountryToCountryService()
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
        }

        #endregion

        #region Methods

        public CountryToCountryDto Get()
        {
            var countryToCountry = new CountryToCountryDto();

            var citiesTypes = _session.Query<CalcCountryCitiesType>().ToList();
            countryToCountry.CountryToCountryCitiesTypeDtoList = ConvertCitiesTypesToDTO(citiesTypes);

            var correspondencias = _session.Query<CalcCountryCorrespondencias>().ToList();
            countryToCountry.CountryToCountryCorrespondenciasDtoList = ConvertToCorrespondenciasDTO(correspondencias);

            var globalTypes = _session.Query<CalcCountryGlobalType>().ToList();
            countryToCountry.CountryToCountryGlobalTypeDtoList = ConvertGlobalTypesToDTO(globalTypes);

            var numSpeaksTypes = _session.Query<CalcCountryNumSpeaksType>().ToList();
            countryToCountry.CountryToCountryNumSpeaksTypeDtoList = ConvertNumSpeaksTypesToDTO(numSpeaksTypes);

            var numSpeaksModerTypes = _session.Query<CalcCountryNumSpeaksModeratorsType>().ToList();
            countryToCountry.CountryToCountryNumSpeaksModerTypeDtoList = ConvertNumSpeaksModerTypesToDTO(numSpeaksModerTypes);

            var reasonsTypes = _session.Query<CalcCountryReasonsType>().ToList();
            countryToCountry.CountryToCountryReasonsTypeDtoList = ConvertReasonsTypesToDTO(reasonsTypes);

            var types = _session.Query<CalcCountryType>().ToList();
            countryToCountry.CountryToCountryTypeDtoList = ConvertTypesToDTO(types);

            return countryToCountry;
        }

        private List<CountryToCountryCitiesTypeDto> ConvertCitiesTypesToDTO(List<CalcCountryCitiesType> dbObjList)
        {
            var result = new List<CountryToCountryCitiesTypeDto>();
            
            foreach (var dbObj in dbObjList)
            {
                CountryToCountryCitiesTypeDto obj = new CountryToCountryCitiesTypeDto()
                {
                    Id = dbObj.Id,
                    Text = dbObj.Text
                };

                result.Add(obj);
            }

            return result;
        }

        private List<CountryToCountryCorrespondenciasDto> ConvertToCorrespondenciasDTO(List<CalcCountryCorrespondencias> dbObjList)
        {
            var result = new List<CountryToCountryCorrespondenciasDto>();

            foreach (var correspondencia in dbObjList)
            {
                CountryToCountryCorrespondenciasDto honorario = new CountryToCountryCorrespondenciasDto()
                {
                    Id = correspondencia.Id,
                    IdType = correspondencia.Idtype,
                    IdGlobalType = correspondencia.Idglobaltype,
                    IdNumSpeaksType = correspondencia.Idnumspeakstype,
                    IdCitiesType = correspondencia.Idcitiestype,
                    IdNumSpeaksModeratorsType = correspondencia.Idnumspeaksmoderatorstype,
                    TableText = correspondencia.Tabletext,
                    IdStandarValue = correspondencia.Idstandarvalue,
                    IdStandarValueAppend = correspondencia.Idstandarvalueappend,
                    IdMaxValue = correspondencia.Idmaxvalue,
                    IdMaxValueAppend = correspondencia.Idmaxvalueappend,
                    IdDaysNoSpeak = correspondencia.Iddaysnospeak,
                    Aditional = correspondencia.Aditional,
                    AditionalAppend = correspondencia.Aditionalappend
                };

                result.Add(honorario);
            }

            return result;
        }

        private List<CountryToCountryGlobalTypeDto> ConvertGlobalTypesToDTO(List<CalcCountryGlobalType> dbObjList)
        {
            var result = new List<CountryToCountryGlobalTypeDto>();

            foreach (var dbObj in dbObjList)
            {
                CountryToCountryGlobalTypeDto obj = new CountryToCountryGlobalTypeDto()
                {
                    Id = dbObj.Id,
                    Text = dbObj.Text
                };

                result.Add(obj);
            }

            return result;
        }

        private List<CountryToCountryNumSpeaksModerTypeDto> ConvertNumSpeaksModerTypesToDTO(List<CalcCountryNumSpeaksModeratorsType> dbObjList)
        {
            var result = new List<CountryToCountryNumSpeaksModerTypeDto>();

            foreach (var dbObj in dbObjList)
            {
                CountryToCountryNumSpeaksModerTypeDto obj = new CountryToCountryNumSpeaksModerTypeDto()
                {
                    Id = dbObj.Id,
                    Text = dbObj.Text,
                    Visible = dbObj.Visible
                };

                result.Add(obj);
            }

            return result;
        }

        private List<CountryToCountryNumSpeaksTypeDto> ConvertNumSpeaksTypesToDTO(List<CalcCountryNumSpeaksType> dbObjList)
        {
            var result = new List<CountryToCountryNumSpeaksTypeDto>();

            foreach (var dbObj in dbObjList)
            {
                CountryToCountryNumSpeaksTypeDto obj = new CountryToCountryNumSpeaksTypeDto()
                {
                    Id = dbObj.Id,
                    Text = dbObj.Text,
                    Visible = dbObj.Visible
                };

                result.Add(obj);
            }

            return result;
        }

        private List<CountryToCountryReasonsTypeDto> ConvertReasonsTypesToDTO(List<CalcCountryReasonsType> dbObjList)
        {
            var result = new List<CountryToCountryReasonsTypeDto>();

            foreach (var dbObj in dbObjList)
            {
                CountryToCountryReasonsTypeDto obj = new CountryToCountryReasonsTypeDto()
                {
                    Id = dbObj.Id,
                    Text = dbObj.Text
                };

                result.Add(obj);
            }

            return result;
        }


        private List<CountryToCountryTypeDto> ConvertTypesToDTO(List<CalcCountryType> dbObjList)
        {
            var result = new List<CountryToCountryTypeDto>();

            foreach (var dbObj in dbObjList)
            {
                CountryToCountryTypeDto obj = new CountryToCountryTypeDto()
                {
                    Id = dbObj.Id,
                    Text = dbObj.Text
                };

                result.Add(obj);
            }

            return result;
        }

        #endregion
    }
}
