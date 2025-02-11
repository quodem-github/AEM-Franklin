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
    public class AprobadorAmecService : IServiceBase
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
        private FilterAprobadorAmec _filter;
        private readonly string _token;
        private readonly string _agencyKey;
        #endregion

        public AprobadorAmecService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterAprobadorAmec>();
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query = _session.Query<Aprobadoramec>();

            if (!String.IsNullOrWhiteSpace(_filter.IdAmecs))
            {
                query = query.Where(x => x.Idamecs == _filter.IdAmecs);
            }

            if (_filter.IdAprobador != Variables.NULL_INT)
            {
                query = query.Where(x => x.Idaprobador == _filter.IdAprobador);
            }
            
            var results = query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize).Take(Variables.ServicePageSize).Select(x => Mapper.Map<AprobadorAmecDto>(x)).ToList();

            int rowCount = query.Count();

            var result = new AprobadorAmecResponseDto()
            {
                AprobadorAmecList = results,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                IdAmecs= _filter.IdAmecs,
                IdAprobador = _filter.IdAprobador,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("AprobadorAmec", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

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
