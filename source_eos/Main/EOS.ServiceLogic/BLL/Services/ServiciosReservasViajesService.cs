using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EOS.ServiceLogic.BLL.Validator;
using EOS.ServiceLogic.Data.DTO.Service;
using EOS.ServiceLogic.Data.Filters;
using EOS.ServiceModel;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Linq;
using EOS.ServiceLogic.Data.DTO.ServiceEdition;
using EOS.ServiceLogic.Data.ServiceResponse;
using EOS.ServiceLogic.Enums;

namespace EOS.ServiceLogic.BLL.Services
{
    public class ServiciosReservasViajesService : IService
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
        private ServiciosReservasViajesEditDto _data;
        private FilterServiciosReservasViajes _filter;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public ServiciosReservasViajesService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterServiciosReservasViajes>
            {
                DateFromProp = "",
                DateToProp = "",
                DateTimeFromProp = "",
                DateTimeToProp = ""
            };
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query = _session.Query<Expediente>()
                .Join(_session.Query<Amec>(), ex => new { IdAmec = ex.Idamec }, am => new { IdAmec = am.Idamec },
                    (ex, am) => new { am.Idamec, ex.Idxpediente, am.Idconfempresa })
                .Join(_session.Query<Confempresa>(), am => new { IdConf = am.Idconfempresa ?? Variables.NULL_INT },
                    co => new { IdConf = co.Idconfempresa }, (am, co) => new { am.Idamec, am.Idxpediente, co.Idagencia })
                .Join(_session.Query<Reservasviajes>(), am => new { IdExpediente = am.Idxpediente },
                    re => new { IdExpediente = re.Fkidexpediente },
                    (am, re) => new { re.Idreserva, am.Idxpediente, am.Idagencia })
                .Join(_session.Query<Serviciosreservasviajes>(), re => new { IdReserva = re.Idreserva },
                    srv => new { IdReserva = srv.Idreserva },
                    (re, srv) => new { srv, re.Idreserva, re.Idxpediente, re.Idagencia })
                .Where(x => x.Idagencia == _agencyKey);

            if (_filter.IdExpediente != Variables.NULL_INT)
            {
                query = query.Where(x => x.Idxpediente == _filter.IdExpediente.Value);
            }

            if (_filter.IdReserva != Variables.NULL_INT)
            {
                query = query.Where(x => x.Idreserva == _filter.IdReserva.Value);
            }

            if (_filter.IdServicio != Variables.NULL_INT)
            {
                query = query.Where(x => x.srv.Idservicio == _filter.IdServicio.Value);
            }

            var list =
                query.Skip((_filter.CurrentPageIndex - 1 ?? 0) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<ServiciosReservasViajesDto>(x.srv)).ToList();

            int rowCount = query.Count();

            var result = new ServiciosReservasViajesResponseDto()
            {
                ServiciosReservasViajesList = list,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                IdExpediente = _filter.IdExpediente,
                IdReserva = _filter.IdReserva,
                IdServicio = _filter.IdServicio,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("ServiciosReservasViajes", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }



        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<ServiciosReservasViajesEditDto>();
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

                    var query = _session.Query<Serviciosreservasviajes>()
                .Join(_session.Query<Reservasviajes>()
                    , srv => new { IdRes = srv.Idreserva }
                    , res => new { IdRes = res.Idreserva }
                    , (srv, res) => new { res.Fkidexpediente, res.Idreserva })
                .Join(_session.Query<Expediente>()
                    , res => new { IdExp = res.Fkidexpediente }
                    , exp => new { IdExp = exp.Idxpediente }
                    , (res, exp) => new { res.Idreserva, exp.Idamec })
                 .Join(_session.Query<Amec>()
                    , exp => new { IdAm = exp.Idamec }
                    , am => new { IdAm = am.Idamec }
                    , (exp, am) => new { am.Idconfempresa, exp.Idreserva })
                 .Join(_session.Query<Confempresa>()
                    , am => new { IdConf = am.Idconfempresa ?? Variables.NULL_INT }
                    , conf => new { IdConf = conf.Idconfempresa }
                    , (am, conf) => new { conf.Idagencia, conf.Idconfempresa, am.Idreserva })

                    // Se trata del Idreserva de la tabla Reservasviajes
                    .Where(x => x.Idagencia == _agencyKey && x.Idreserva == _data.Idreserva);


                    var results = query.ToList();
                    bool isAgencyService = results.Count >= 1;

                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.TarifasAlojInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.ServiciosReservasViajesInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("ServiciosReservaViajes", JsonConvert.SerializeObject(_data), "Error en ServiciosReservasViajesService.Edit: El Idreserva no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }

                    //*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();

                    var editData = Mapper.Map<Serviciosreservasviajes>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idservicio == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<Serviciosreservasviajes>()
                            .Max(x => x.Idservicio);
                        editData.Idservicio = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("ServiciosReservaViajes", JsonConvert.SerializeObject(_data), JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new ServiciosReservasViajesEditResponseDto()
                    {
                        ServiciosReservasViajes = _data,
                        ServiciosReservasViajesResult = Mapper.Map<ServiciosReservasViajesEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en ServiciosReservaViajesService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.ServiciosReservasViajesInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.ServiciosReservasViajesInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("ServiciosReservaViajes", JsonConvert.SerializeObject(_data),
                        "Error en ServiciosReservaViajesService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }
        }


    }
}
