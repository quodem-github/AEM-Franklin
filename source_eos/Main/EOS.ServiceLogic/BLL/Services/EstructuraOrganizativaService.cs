using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using EOS.ServiceLogic.BLL.Validator;
using EOS.ServiceLogic.Data.DTO.Service;
using EOS.ServiceLogic.Data.Filters;
using EOS.ServiceLogic.Enums;
using EOS.ServiceModel;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Linq;


namespace EOS.ServiceLogic.BLL.Services
{
    public class EstructuraOrganizativaService : IServiceBase
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

        #region Properties
        private QuodemLogService _logService;
        private FilterEstructuraOrganizativa _filter;
        private readonly string _token;
        private readonly string _agencyKey;
        #endregion

        public EstructuraOrganizativaService(string token, string agencyKey)
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
            _token = token;
            _agencyKey = agencyKey;
            _logService = new QuodemLogService(_session);
        }

        public ServiceError ValidFilter(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<FilterEstructuraOrganizativa>
            {
                DateFromProp = "UpdateDateFrom",
                DateToProp = "UpdateDateTo",
                DateTimeFromProp = "DateFrom",
                DateTimeToProp = "DateTo"
            };
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query = _session.Query<AgencyUserApprovalStructure>();

            if (_filter.IdPeticionario != Variables.NULL_INT)
            {
                query = query.Where(x => x.Idpeticionario == _filter.IdPeticionario);
            }

            if (_filter.IdPeticionarioManager != Variables.NULL_INT)
            {
                query = query.Where(x => x.Idpeticionariomanager == _filter.IdPeticionarioManager);
            }

            if (!string.IsNullOrWhiteSpace(_filter.Wein))
            {
                query = query.Where(x => x.Wein == _filter.Wein);
            }

            if (!string.IsNullOrWhiteSpace(_filter.WeinManager))
            {
                query = query.Where(x => x.Weinmanager == _filter.WeinManager);
            }

            if (_filter.DateFrom != DateTime.MinValue)
            {
                query = query.Where(x => x.Lastupdatedate >= _filter.DateFrom);
            }

            if (_filter.DateTo != DateTime.MinValue)
            {
                query = query.Where(x => x.Lastupdatedate <= _filter.DateTo);
            }

            var results =
                query.Skip((_filter.CurrentPageIndex.Value - 1)*Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<EstructuraOrganizativaDto>(x))
                    .ToList();

            int rowCount = query.Count();

            var result = new EstructuraOrganizativaResponseDto()
            {
                EstructuraOrganizativaList = results,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                IdPeticionario = _filter.IdPeticionario,
                IdPeticionarioManager = _filter.IdPeticionarioManager,
                Wein = _filter.Wein,
                WeinManager = _filter.Wein,
                UpdateDateFrom = _filter.UpdateDateFrom,
                UpdateDateTo = _filter.UpdateDateTo,
                PageCount = (int) Math.Ceiling(rowCount*1.0/Variables.ServicePageSize)
            };

            _logService.AddLogEntry("EstructuraOrganizativa", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public ServiceError ValidData()
        {
            throw new NotImplementedException();
        }

        public bool Edit()
        {
            throw new NotImplementedException();
        }

    }
}
