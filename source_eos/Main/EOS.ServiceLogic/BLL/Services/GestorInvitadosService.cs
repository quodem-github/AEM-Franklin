using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GestorInvitadosService : IServiceExtended
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

        private FilterGestorInvitados _filter;
        private GestorInvitadosEditDto _data;
        private GestorInvitadosDeleteDto _dataDelete;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public GestorInvitadosService(string token, string agencyKey)
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


            var validateService = new ValidateService<FilterGestorInvitados>
            {
                DateFromProp = "CongresoDateFrom",
                DateToProp = "CongresoDateTo",
                DateTimeFromProp = "DateFrom",
                DateTimeToProp = "DateTo"
            };
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }
        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<GestorInvitadosEditDto>();
            _data = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public ServiceError ValidDataDelete(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<GestorInvitadosDeleteDto>();
            _dataDelete = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query =
                _session.Query<Gestorinvitados>()
                    .Join(_session.Query<Confempresa>(),
                        ges =>
                            new { IdConf = ges.Idconfempresa ?? Variables.NULL_INT },
                        conf => new { IdConf = conf.Idconfempresa }, (ges, conf) => new { ges, conf.Idagencia })
                    .Where(x => x.Idagencia == _agencyKey);

            if (_filter.IdCongreso != Variables.NULL_INT)
            {
                query = query.Where(x => x.ges.Idcongreso == _filter.IdCongreso);
            }
            if (_filter.IdEventoFormulario != Variables.NULL_INT)
            {
                query = query.Where(x => x.ges.Ideventoformulario == _filter.IdEventoFormulario);
            }
            if (_filter.IdGestorInvitados != Variables.NULL_INT)
            {
                query = query.Where(x => x.ges.Idgestorinvitados == _filter.IdGestorInvitados);
            }

            if (_filter.DateFrom != DateTime.MinValue)
            {
                query = query.Where(x => x.ges.Fechainicio >= _filter.DateFrom);
            }

            if (_filter.DateTo != DateTime.MinValue)
            {
                query = query.Where(x => x.ges.Fechafin <= _filter.DateTo);
            }

            var gestorInvitados =
                query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<GestorInvitadosDto>(x.ges)).ToList();

            int rowCount = query.Count();

            var result = new GestorInvitadosResponseDto()
            {
                GestorInvitadosList = gestorInvitados,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                GestorDateFrom = _filter.CongresoDateFrom,
                GestorDateTo = _filter.CongresoDateTo,
                IdGestorInvitados = _filter.IdGestorInvitados,
                IdEventoFormulario = _filter.IdEventoFormulario,
                IdCongreso = _filter.IdCongreso,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("GestorInvitados", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);



        }

        public SrvResponse Edit(QSuscriptor suscriptor)
        {
            var result = new SrvResponse()
            {
                ErrorList = new List<ErrorItem>()
            };

            /*ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

            using (var transaction = _session.BeginTransaction())
            {
                try
                {

                    var query =
               _session.Query<Gestorinvitados>()
                   .Join(_session.Query<Confempresa>(),
                       ges =>
                           new { IdConf = ges.Idconfempresa ?? Variables.NULL_INT },
                       conf => new { IdConf = conf.Idconfempresa }, (ges, conf) => new { ges, conf })
                   .Where(x => x.conf.Idagencia == _agencyKey);

                    if (_data.IdGestorInvitados != Variables.NULL_INT)
                    {
                        query = query.Where(x => x.ges.Idgestorinvitados == _data.IdGestorInvitados);
                    }

                    var results = query.ToList();
                    bool isAgencyService = ((results.Count == 1 && _data.IdGestorInvitados != Variables.NULL_INT) || (_data.IdGestorInvitados == Variables.NULL_INT));


                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.GestorinvitadosInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.GestorinvitadosInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("GestorInvitados", JsonConvert.SerializeObject(_data), "Error en GestorInvitadosService.Edit: El IdGestorInvitados no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }
                    /*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();


                    var editData = Mapper.Map<Gestorinvitados>(_data);
                    editData.Idconfempresa = results.First().conf.Idconfempresa;
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idgestorinvitados == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<Gestorinvitados>()
                            .Max(x => x.Idgestorinvitados);
                        editData.Idgestorinvitados = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        _session.Evict(results[0].ges);
                        _session.Flush();
                        editData.Idconfempresa = _session.Query<Confempresa>().First(x => x.Idagencia == _agencyKey).Idconfempresa;
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("GestorInvitados", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new GestorInvitadosEditResponseDto()
                    {
                        GestorInvitados = _data,
                        GestorInvitadosResult = Mapper.Map<GestorInvitadosEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en GestorInvitadosService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.GestorinvitadosInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.GestorinvitadosInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("GestorInvitados", JsonConvert.SerializeObject(_data),
                        "Error en GestorInvitadosService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }
        }

        public SrvResponse Delete(QSuscriptor suscriptor)
        {
            var result = new SrvResponse()
            {
                ErrorList = new List<ErrorItem>()
            };

            /*ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

            if (_dataDelete.ListIdGestorInvitados == null || _dataDelete.ListIdGestorInvitados.Count == 0)
            {
                result.ErrorList.Add(new ErrorItem()
                {
                    Code = ErrorCode.GestorinvitadosInsertUpdateError.GetHashCode(),
                    Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.GestorinvitadosInsertUpdateError.ToString())
                });

                //Generar el log en la tabla de auditoria
                _logService.AddLogEntry("GestorInvitados", JsonConvert.SerializeObject(_dataDelete), "Error en GestorInvitadosService.Delete: No se han recibido ListIdGestorInvitado a borrar", _agencyKey);
                return result;
            }

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new GestorInvitadosDeleteResponseDto()
                    {
                        GestorInvitados = _dataDelete,
                        GestorInvitadosResult = new GestorInvitadosDeleteDto()
                        {
                            ListIdGestorInvitados = new List<long>()
                        }
                    };

                    foreach (var gestorInvitado in _dataDelete.ListIdGestorInvitados)
                    {
                        var query = _session.Query<Gestorinvitados>()
                            .Join(_session.Query<Confempresa>(),
                                ges =>
                                    new {IdConf = ges.Idconfempresa ?? Variables.NULL_INT},
                                conf => new {IdConf = conf.Idconfempresa}, (ges, conf) => new {ges, conf})
                            .Where(x => x.conf.Idagencia == _agencyKey && x.ges.Idgestorinvitados == gestorInvitado);
                        
                        var results = query.ToList();
                        bool isAgencyService = results.Count == 1;

                        _session.Flush();
                        if (!isAgencyService)
                        {
                            result.ErrorList.Add(new ErrorItem()
                            {
                                Code = ErrorCode.GestorinvitadosInsertUpdateError.GetHashCode(),
                                Description =
                                    Utility.GetDisplayName(typeof(ErrorCode),
                                        ErrorCode.GestorinvitadosInsertUpdateError.ToString()) +
                                    " Idgestorinvitados no borrado: " + gestorInvitado.ToString()
                            });

                            //Generar el log en la tabla de auditoria
                            _logService.AddLogEntry("GestorInvitados_Delete", JsonConvert.SerializeObject(_dataDelete),
                                "Error en GestorInvitadosService.Delete: El IdGestorInvitados no pertenece a la agencia que está usando el servicio: " + gestorInvitado,
                                _agencyKey);
                        }
                        else
                        {
                            var editData = results[0].ges;
                            _session.Evict(results[0].ges);
                            _session.Flush();
                            _session.Delete(editData);

                            _logService.AddLogEntry("GestorInvitados_Delete", JsonConvert.SerializeObject(_dataDelete), JsonConvert.SerializeObject(gestorInvitado), _agencyKey);

                            response.GestorInvitadosResult.ListIdGestorInvitados.Add(gestorInvitado);
                        }
                    }

                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en GestorInvitadosService.Delete: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.GestorinvitadosInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.GestorinvitadosInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("GestorInvitados", JsonConvert.SerializeObject(_dataDelete),
                        "Error en GestorInvitadosService.Delete: " + e.Message, _agencyKey);

                    return result;
                }
            }
        }
    }
}
