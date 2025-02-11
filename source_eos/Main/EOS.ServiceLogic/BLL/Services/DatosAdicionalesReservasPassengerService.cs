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
    public class DatosAdicionalesReservasPassengerService : IServiceBase
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
        private FilterDatosAdicionalesReservasPassenger _filter;
        private readonly string _token;
        private readonly string _agencyKey;
        #endregion

        public DatosAdicionalesReservasPassengerService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterDatosAdicionalesReservasPassenger>();
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query = _session.Query<DatosadicionalesReservasPassenger>()
                .Join(_session.Query<Expediente>(), da => new {IdExp = da.Idexpediente ?? Variables.NULL_INT},
                    ex => new {IdExp = ex.Idxpediente}, (da, ex) => new {da, ex.Idamec, ex.Idxpediente})
                .Join(_session.Query<Amec>(), exda => new {IdAmec = exda.Idamec}, am => new {IdAmec = am.Idamec},
                    (exda, am) => new {exda, am.Idconfempresa})
                .Join(_session.Query<Confempresa>(),
                    exdaam => new {IdConfEmpresa = exdaam.Idconfempresa ?? Variables.NULL_INT},
                    co => new {IdConfEmpresa = co.Idconfempresa}, (exdaam, co) => new {exdaam, co.Idagencia})
                .Where(x => x.Idagencia == _agencyKey);



            if (_filter.IdExpediente != Variables.NULL_INT)
            {
                query = query.Where(x => x.exdaam.exda.Idxpediente == _filter.IdExpediente);
            }
            if (_filter.IdPassengerList != Variables.NULL_INT)
            {
                query = query.Where(x => x.exdaam.exda.da.Idpassengerlist == _filter.IdPassengerList);
            }

            var results =
                query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<DatosAdicionalesReservasPassengerDto>(x.exdaam.exda.da)).ToList();

            int rowCount = query.Count();

            var result = new DatosAdicionalesReservasPassengerResponseDto()
            {
                DatosAdicionalesList = results,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                IdExpediente = _filter.IdExpediente,
                IdPassengerList = _filter.IdPassengerList,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("DatosAdicionalesReservasPassenger", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

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
