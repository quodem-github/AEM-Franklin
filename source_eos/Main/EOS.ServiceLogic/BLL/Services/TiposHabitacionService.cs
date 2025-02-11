using System;
using System.Linq;
using AutoMapper;
using EOS.ServiceLogic.Data.DTO.Service;
using EOS.ServiceModel;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Linq;

namespace EOS.ServiceLogic.BLL.Services
{
    public class TiposHabitacionService : IServiceBase
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
        private readonly string _token;
        private readonly string _agencyKey;
        #endregion

        public TiposHabitacionService(string agencyKey, string token)
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
            _token = token;
            _logService = new QuodemLogService(_session);
        }
        
        public ServiceError ValidFilter(QSuscriptor suscriptor, string data)
        {
            throw new NotImplementedException();
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var results = _session.Query<TiposHab>().Select(x => Mapper.Map<TiposHabitacionDto>(x)).ToList();

            var result = new TiposHabitacionResponseDto()
            {
                TiposHabitacionList = results,
                CurrentPageIndex = 1,
                PageSize = Variables.ServicePageSize,
                PageCount = (int)Math.Ceiling(results.Count * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("TiposHabitacion", string.Empty, results.Count.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }
    }
}
