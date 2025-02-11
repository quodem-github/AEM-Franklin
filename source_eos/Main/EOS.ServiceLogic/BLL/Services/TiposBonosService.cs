using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using EOS.ServiceLogic.BLL.Validator;
using EOS.ServiceLogic.Data.DTO;
using EOS.ServiceLogic.Data.DTO.Service;
using EOS.ServiceLogic.Data.DTO.ServiceEdition;
using EOS.ServiceLogic.Data.Filters;
using EOS.ServiceLogic.Data.ServiceResponse;
using EOS.ServiceLogic.Enums;
using EOS.ServiceModel;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Linq;


namespace EOS.ServiceLogic.BLL.Services
{
    public class TiposBonosService : IService
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

        private TipoBonosEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public TiposBonosService(string token, string agencyKey)
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

        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<TipoBonosEditDto>();
            _data = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query = _session.Query<TiposBonos>();

            var results =
                query.Select(x => Mapper.Map<TiposBonosDto>(x)).ToList();

            var result = new TiposBonosResponseDto()
            {
                TiposBonosList = results
            };

            _logService.AddLogEntry("TiposBonos", string.Empty, results.Count.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
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

                    var editData = Mapper.Map<TiposBonos>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.

                    //Se actualiza con el objeto que se recibe.
                    _session.SaveOrUpdate(editData);

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("tipos_bonos", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new TipoBonosEditResponseDto()
                    {
                        TipoBonos = _data,
                        TipoBonosResult = Mapper.Map<TipoBonosEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en TiposBonosService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.TipoBonoInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.TipoBonoInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("tipos_bonos", JsonConvert.SerializeObject(_data),
                        "Error en TiposBonosService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }
    }
}
