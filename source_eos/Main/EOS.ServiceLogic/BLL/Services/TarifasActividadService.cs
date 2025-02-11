using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TarifasActividadService : IService
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
        private TarifasActividadEditDto _data;
        private FilterTarifasActividad _filterTarifasActividad;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public TarifasActividadService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterTarifasActividad>();
            _filterTarifasActividad = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query = _session.Query<Tarifasactividad>()
                .Join(_session.Query<Congresos>(), tar => new { IdCongreso = tar.Fkidcongreso },
                    am => new { IdCongreso = am.Idcongreso },
                    (tar, am) => new { tar, am })
                .Join(_session.Query<Confempresa>(), tam => new { IdConfEmpresa = tam.am.Idconfempresa },
                    conf => new { IdConfEmpresa = conf.Idconfempresa }, (tam, conf) => new { tam, conf })
                .Where(x => x.conf.Idagencia == _agencyKey);

            if (_filterTarifasActividad.IdCongreso != Variables.NULL_INT)
            {
                query = query.Where(x => x.tam.tar.Fkidcongreso == _filterTarifasActividad.IdCongreso.Value);
            }

            if (_filterTarifasActividad.IdTarifaActividad != Variables.NULL_INT)
            {
                query = query.Where(x => x.tam.tar.Idtarifaactividad == _filterTarifasActividad.IdTarifaActividad.Value);
            }

            var tarifas =
                query.Skip((_filterTarifasActividad.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<TarifasActividadDto>(x.tam.tar)).ToList();

            int rowCount = query.Count();

            var result = new TarifasActividadResponseDto()
            {
                TarifasActividadList = tarifas,
                CurrentPageIndex = _filterTarifasActividad.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize),
                IdCongreso = _filterTarifasActividad.IdCongreso,
                IdTarifaActividad = _filterTarifasActividad.IdTarifaActividad
            };

            _logService.AddLogEntry("TarifasActividad", JsonConvert.SerializeObject(_filterTarifasActividad), rowCount.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<TarifasActividadEditDto>();
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
                    //*ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/
                    var query = _session.Query<Congresos>()
                .Join(_session.Query<Confempresa>(), tam => new { IdConfEmpresa = tam.Idconfempresa },
                    conf => new { IdConfEmpresa = conf.Idconfempresa }, (tam, conf) => new { tam, conf })
                .Where(x => x.conf.Idagencia == _agencyKey && x.tam.Idcongreso == _data.Fkidcongreso);

                    var results = query.ToList();
                    bool isAgencyService = results.Count >= 1;

                    List<Tarifasactividad> tarifasList = null;
                    if (isAgencyService && _data.Idtarifaactividad != Variables.NULL_INT)
                    {
                        tarifasList = _session.Query<Tarifasactividad>().Where(x => x.Idtarifaactividad == _data.Idtarifaactividad).ToList();
                        isAgencyService = tarifasList.Count >= 1;
                    }

                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.TarifasActividadInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.TarifasActividadInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("TarifasActividad", JsonConvert.SerializeObject(_data), "Error en TarifasActividadService.Edit: El Idtarifaactividad no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }

                    //*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();
                    if (tarifasList != null && tarifasList.Count > 0)
                    {
                        _session.Evict(tarifasList[0]);
                        _session.Flush();
                    }

                    var editData = Mapper.Map<Tarifasactividad>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idtarifaactividad == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<Tarifasactividad>()
                            .Max(x => x.Idtarifaactividad);
                        editData.Idtarifaactividad = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("TarifasActividadService", JsonConvert.SerializeObject(_data), JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new TarifasActividadEditResponseDto()
                    {
                        TarifasActividad = _data,
                        TarifasActividadResult = Mapper.Map<TarifasActividadEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en TarifasActividadService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.TarifasActividadInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.TarifasActividadInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("TarifasActividad", JsonConvert.SerializeObject(_data),
                        "Error en TarifasActividadService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }
        }
    }
}
