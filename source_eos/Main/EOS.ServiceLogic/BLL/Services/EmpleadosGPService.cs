using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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
    public class EmpleadosGPService : IService
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

        private EmpleadosGpEditDto _data;
        private QuodemLogService _logService;
        private readonly string _token;
        private readonly string _agencyKey;
        #endregion

        public EmpleadosGPService(string token, string agencyKey)
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

        private List<Empleadosgp> GetQueryByAgency()
        {
            return _session.Query<Empleadosgp>()
                .Join(_session.Query<Confempresa>(),
                    empgp => new { IdConf = empgp.Idconfempresa }, conf => new { IdConf = conf.Idconfempresa },
                    (empgp, conf) => new
                    {
                        empgp,
                        conf
                    })
                 .Where(x => x.conf.Idagencia == _agencyKey).Select(x => x.empgp).ToList();
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query = GetQueryByAgency();
            var results = query.Select(x => Mapper.Map<EmpleadosGpDto>(x)).ToList();
            var result = new EmpleadosGpResponseDto()
            {
                EmpleadosGpList = results
            };

            _logService.AddLogEntry("EmpleadosGP", JsonConvert.SerializeObject(_data), results.Count.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<EmpleadosGpEditDto>();
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
                    bool isAgencyService = false;
                    var query = GetQueryByAgency();
                    if (_data.Id != Variables.NULL_INT)
                    {
                        query = query.Where(x => x.Id == _data.Id.Value).ToList();
                    }
                    var results = query.ToList();
                    isAgencyService = results.Count == 1;

                    if (!isAgencyService && _data.Id != Variables.NULL_INT)
                    {

                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.EmpleadosGpInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.EmpleadosGpInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("empleadogp", JsonConvert.SerializeObject(_data), "Error en EmpleadosGPService.Edit: El idEmpleadoGp no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }

                    _session.Flush();

                    var editData = Mapper.Map<Empleadosgp>(_data);
                    editData.Idconfempresa = results.First().Idconfempresa;
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Id == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<Empleadosgp>()
                            .Max(x => x.Id);
                        editData.Id = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        //Se actualiza con el objeto que se recibe.
                        _session.Flush();
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("empleadosgp", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new EmpleadosGpEditResponseDto()
                    {
                        EmpleadosGp = _data,
                        EmpleadosGpResult = Mapper.Map<EmpleadosGpEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en EmpleadosGPService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.EmpleadosGpInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.EmpleadosGpInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("empleadosgp", JsonConvert.SerializeObject(_data),
                        "Error en EmpleadosGPService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }
    }
}
