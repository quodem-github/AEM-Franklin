using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EOS.ServiceLogic.BLL.Validator;
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
    public class ServiciosService : IService
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

        private ServiciosEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public ServiciosService(string token, string agencyKey)
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
            var query = _session.Query<Servicios>();

            var results = query.Select(x => Mapper.Map<ServiciosDto>(x)).ToList();

            var result = new ServiciosResponseDto()
            {
                ServiciosList = results,
            };

            _logService.AddLogEntry("Servicios", string.Empty, results.Count.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }


        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<ServiciosEditDto>();
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

                    var editData = Mapper.Map<Servicios>(_data);
                   

                    //Se actualiza con el objeto que se recibe.
                    _session.SaveOrUpdate(editData);
                    
                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("servicios", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new ServiciosEditResponseDto()
                    {
                        ServiciosList = _data,
                        ServiciosListResult = Mapper.Map<ServiciosEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en ServiciosService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.ServiciosInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.ServiciosInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("servicios", JsonConvert.SerializeObject(_data),
                        "Error en ServiciosService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }
    }
}