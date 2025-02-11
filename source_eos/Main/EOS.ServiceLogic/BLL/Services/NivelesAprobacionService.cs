using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using EOS.Entidades.Modelo;
using EOS.ServiceLogic.BLL.Validator;
using EOS.ServiceLogic.Data.DTO.Service;
using EOS.ServiceLogic.Data.DTO.ServiceEdition;
using EOS.ServiceLogic.Data.ServiceResponse;
using EOS.ServiceLogic.Enums;
using EOS.ServiceModel;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Linq;

namespace EOS.ServiceLogic.BLL.Services
{
    public class NivelesAprobacionService : IService
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
        private NivelesAprobacionEditDto _data;
        private readonly string _token;

        private QuodemLogService _logService;
        private readonly string _agencyKey;
        #endregion

        public NivelesAprobacionService(string token, string agencyKey)
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
            _token = token;
            _logService = new QuodemLogService(_session);
            _agencyKey = agencyKey;
        }

        public ServiceError ValidFilter(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<NivelesAprobacionEditDto>();
            _data = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var results = _session.Query<Nivelesaprobacion>().Select(x => Mapper.Map<NivelesAprobacionDto>(x)).ToList();
            var result = new NivelesAprobacionResponseDto()
            {
                NivelesAprobacionList = results,
            };

            _logService.AddLogEntry("NivelesAprobacion", string.Empty, results.Count.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<NivelesAprobacionEditDto>();
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
                    var editData = Mapper.Map<Nivelesaprobacion>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idnivelaprobacion == Variables.NULL_INT)
                    {
                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<Nivelesaprobacion>()
                            .Max(x => x.Idnivelaprobacion);
                        editData.Idnivelaprobacion = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("nivelesAprobacion", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new NivelesAprobacionEditResponseDto()
                    {
                        NivelesAprobacion = _data,
                        NivelesAprobacionResult = Mapper.Map<NivelesAprobacionEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en NivelesAprobacionService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.NivelesAprobacionUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.NivelesAprobacionUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("nivelesAprobacion", JsonConvert.SerializeObject(_data),
                        "Error en NivelesAprobacion.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }
        }
    }
}
