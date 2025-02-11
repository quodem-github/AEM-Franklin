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
    public class AmecsService : IServiceBase
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
        private FilterAmecs _filter;
        private readonly string _token;
        private readonly string _agencyKey;
        #endregion

        public AmecsService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterAmecs>
            {
                DateFromProp = "LastUpdateDateFrom",
                DateToProp = "LastUpdateDateTo",
                DateTimeFromProp = "DateFrom",
                DateTimeToProp = "DateTo"
            };
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query = _session.Query<Amecs>()
                .Join(_session.Query<Confempresa>(),am => new {IdConfEmpresa = am.Idconfempresa ?? Variables.NULL_INT },co => new {IdConfEmpresa = co.Idconfempresa },(am,co)=>new {am,co})
                .Where(x=>x.co.Idagencia == _agencyKey);


            if (!String.IsNullOrWhiteSpace(_filter.Idamecs))
            {
                query = query.Where(x => x.am.Idamecs.ToString() == _filter.Idamecs);
            }

            if (_filter.DateFrom != DateTime.MinValue)
            {
                query = query.Where(x => x.am.Fechaultimaactualizacion >= _filter.DateFrom);
            }

            if (_filter.DateTo != DateTime.MinValue)
            {
                query = query.Where(x => x.am.Fechaultimaactualizacion <= _filter.DateTo);
            }

            var results =
                query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<AmecsDto>(x.am)).ToList();

            int rowCount = query.Count();

            var result = new AmecsResponseDto()
            {
                AmecsList = results,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                Idamecs= _filter.Idamecs,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize),
                LastUpdateDateFrom = _filter.LastUpdateDateFrom,
                LastUpdateDateTo = _filter.LastUpdateDateTo
            };

            _logService.AddLogEntry("Amecs", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

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
