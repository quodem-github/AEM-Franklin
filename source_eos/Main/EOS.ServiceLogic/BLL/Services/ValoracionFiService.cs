using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using EOS.ServiceLogic.Data.DTO.Service;
using EOS.ServiceModel;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Linq;

namespace EOS.ServiceLogic.BLL.Services
{
    public class ValoracionFiService : IServiceBase
    {
        #region Definitions
        private readonly string _agencyKey;
        private QuodemLogService _logService;
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
        private readonly string _token;
        #endregion

        public ValoracionFiService(string token, string agencyKey)
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
            _token = token;
            _agencyKey = agencyKey;
            _logService = new QuodemLogService(_session);
        }


        public ServiceError ValidFilter(QSuscriptor suscriptor, string data)
        {
            throw new NotImplementedException();
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var results = _session.Query<ValoracionFi>().Select(x => Mapper.Map<ValoracionFiDto>(x)).ToList();

            var result = new ValoracionFiResponseDto()
            {
                ValoracionFiList = results,
                CurrentPageIndex = 1,
                PageSize = Variables.ServicePageSize,
                PageCount = (int)Math.Ceiling(results.Count * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("ValoracionFi", string.Empty, results.Count.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }
    }
}
