using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using EOS.ServiceLogic.BLL.Validator;
using EOS.ServiceLogic.Data.DTO.Service;
using EOS.ServiceLogic.Data.Filters;
using EOS.ServiceLogic.Data.ServiceResponse;
using EOS.ServiceLogic.Enums;
using EOS.ServiceModel;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Linq;
using EOS.ServiceLogic.Data.DTO.ServiceEdition;

namespace EOS.ServiceLogic.BLL.Services
{
    public class TiposPRVService : IService
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
        private TiposPRVEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public TiposPRVService(string agencyKey, string token)
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
            var query = _session.Query<TiposPrv>();

            var results = query.Select(x => Mapper.Map<TiposPRVDto>(x)).ToList();

            var result = new TiposPRVResponseDto()
            {
                TiposPrvList = results,
            };

            _logService.AddLogEntry("TiposPRV", string.Empty, results.Count.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<TiposPRVEditDto>();
            _data = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public SrvResponse Edit(QSuscriptor suscriptor)
        {
            var result = new SrvResponse()
            {
                ErrorList = new List<ErrorItem>()
            };

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    var editData = Mapper.Map<TiposPrv>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.

                    //Se actualiza con el objeto que se recibe.
                    _session.SaveOrUpdate(editData);

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("tipos_prv", JsonConvert.SerializeObject(_data), JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new TiposPRVEditResponseDto()
                    {
                        TiposPRV = _data,
                        TiposPRVResult = Mapper.Map<TiposPRVEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en TiposPRVService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.TiposPRVInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.TiposPRVInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("TiposPRV", JsonConvert.SerializeObject(_data),
                        "Error en TiposPRVService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }
        }
    }
}
