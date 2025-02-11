using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using EOS.ServiceLogic.BLL.Validator;
using EOS.ServiceLogic.Data;
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
using Quodem.ContentManager.Events;

namespace EOS.ServiceLogic.BLL.Services
{
    public class ExpedienteService : IService
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

        private FilterExpediente _filter;
        private ExpedienteEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public ExpedienteService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterExpediente>
            {
                DateFromProp = "CreationDateFrom",
                DateToProp = "CreationDateTo",
                DateTimeFromProp = "DateFrom",
                DateTimeToProp = "DateTo"
            };
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }



        public string GetList(QSuscriptor suscriptor)
        {

            var query = _session.Query<Expediente>()
                .Join(_session.Query<Amec>(), exp => new { IdAmec = exp.Idamec },
                    amc => new { IdAmec = amc.Idamec }, (exp, amc) => new { exp, amc })
                .Join(_session.Query<Confempresa>(), aex => new { IdConf = aex.amc.Idconfempresa ?? Variables.NULL_INT },
                    conf => new { IdConf = conf.Idconfempresa }, (aex, conf) => new { aex, conf })
                .Where(x => x.conf.Idagencia == _agencyKey);

            if (_filter.DateFrom != DateTime.MinValue)
            {
                query = query.Where(x => x.aex.exp.Fechacreacion >= _filter.DateFrom);
            }

            if (_filter.DateTo != DateTime.MinValue)
            {
                query = query.Where(x => x.aex.exp.Fechacreacion <= _filter.DateTo);
            }

            if (!string.IsNullOrEmpty(_filter.Amec))
            {
                query = query.Where(x => x.aex.amc.Amec1 == _filter.Amec);
            }

            if (_filter.IdExpediente != Variables.NULL_INT)
            {
                query = query.Where(x => x.aex.exp.Idxpediente == _filter.IdExpediente);
            }

            var expedientes =
                query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<ExpedienteDto>(x.aex.exp)).ToList();

            int rowCount = query.Count();

            var result = new ExpedienteResponseDto()
            {
                ExpedienteList = expedientes,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                CreationDateFrom = _filter.CreationDateFrom,
                CreationDateTo = _filter.CreationDateTo,
                Amec = _filter.Amec,
                IdExpediente = _filter.IdExpediente,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("Expediente", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<ExpedienteEditDto>();
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
                    /*ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/
                    var query = _session.Query<Expediente>()
               .Join(_session.Query<Amec>(), exp => new { IdAmec = exp.Idamec },
                   amc => new { IdAmec = amc.Idamec }, (exp, amc) => new { exp, amc })
               .Join(_session.Query<Confempresa>(), aex => new { IdConf = aex.amc.Idconfempresa ?? Variables.NULL_INT },
                   conf => new { IdConf = conf.Idconfempresa }, (aex, conf) => new { aex, conf })
               .Where(x => x.conf.Idagencia == _agencyKey && x.aex.amc.Idamec == _data.Idamec);

                    if (_data.Idxpediente != Variables.NULL_INT)
                    {
                        query = query.Where(x => x.aex.exp.Idxpediente == _data.Idxpediente);
                    }

                    var results = query.ToList();
                    bool isAgencyService = results.Count >= 1;


                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.ExpedienteInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.ExpedienteInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("expediente", JsonConvert.SerializeObject(_data), "Error en ExpedienteService.Edit: El Idxpediente no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }
                    /*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();

                    int? idempleadogp = null;
                    if (_data.Idempleadogp.HasValue)
                    {
                        var resultList = _session.Query<Empleadosgp>().Where(x =>x.Idempleadogp == _data.Idempleadogp.Value.ToString() && x.Idconfempresa == results[0].conf.Idconfempresa).ToList();
                        if (resultList.Count > 0)
                        {
                            idempleadogp = resultList[0].Id;
                        }
                    }

                    var editData = Mapper.Map<Expediente>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idxpediente == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<Expediente>()
                            .Max(x => x.Idxpediente);
                        editData.Idxpediente = lastId + 1;
                        editData.Idempleadogp = idempleadogp;
                        _session.Save(editData);
                    }
                    else
                    {
                        _session.Evict(results[0].aex.exp);
                        _session.Flush();
                        //Se actualiza con el objeto que se recibe.
                        editData.Idunidad = results.First().aex.exp.Idunidad;
                        editData.Idarea = results.First().aex.exp.Idarea;
                        editData.Idregion = results.First().aex.exp.Idregion;
                        editData.Iddistrito = results.First().aex.exp.Iddistrito;
                        editData.Idempleadogp = idempleadogp;
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("expediente", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new ExpedienteEditResponseDto()
                    {
                        Expediente = _data,
                        ExpedienteResult = Mapper.Map<ExpedienteEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en ExpedienteService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.ExpedienteInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.ExpedienteInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("expediente", JsonConvert.SerializeObject(_data),
                        "Error en ExpedienteService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }


    }
}
