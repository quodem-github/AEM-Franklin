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
    public class ServicioReservaActividadesService : IService
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

        private FilterServicioReservasActividades _filter;
        private ServiciosReservasActividadesEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public ServicioReservaActividadesService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterServicioReservasActividades>
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
                    (re, srv) => new { srv.Idservicio, re.Idreserva, re.Idxpediente, re.Idagencia, srv.Idservicioactividad })
                .Join(_session.Query<Serviciosreservasactividades>(), srv => new { IdServicioActividad = srv.Idservicioactividad ?? Variables.NULL_INT },
                    sra => new { IdServicioActividad = sra.Idservicioactividad },
                    (srv, sra) => new { srv.Idxpediente, srv.Idreserva, srv.Idservicio, srv.Idagencia, sra })
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
                query = query.Where(x => x.Idservicio == _filter.IdServicio.Value);
            }
            if (_filter.IdServicioActividad != Variables.NULL_INT)
            {
                query = query.Where(x => x.sra.Idservicioactividad == _filter.IdServicioActividad.Value);
            }

            var list =
                query.Skip((_filter.CurrentPageIndex - 1 ?? 0) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<ServiciosReservasActividadesDto>(x.sra)).ToList();

            int rowCount = query.Count();

            var result = new ServicioReservaActividadesResponseDto()
            {
                ServiciosReservasActividadesList = list,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                IdExpediente = _filter.IdExpediente,
                IdReserva = _filter.IdReserva,
                IdServicio = _filter.IdServicio,
                IdServicioActividad = _filter.IdServicioActividad,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("ServicioReservaActividades", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }



        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<ServiciosReservasActividadesEditDto>();
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
                    var query = _session.Query<Serviciosreservasactividades>()
                .Join(_session.Query<Confempresa>(),
                    sra => new { IdConf = sra.Idconfempresa },
                    conf => new { IdConf = conf.Idconfempresa },
                    (sra, conf) => new { conf.Idconfempresa, conf.Idagencia, sra.Idservicioactividad })

                .Where(x => x.Idagencia == _agencyKey);

                    if (_data.Idservicioactividad != Variables.NULL_INT)
                    {
                        query = query.Where(x => x.Idservicioactividad == _data.Idservicioactividad);
                    }

                    var results = query.ToList();
                    bool isAgencyService = results.Count >= 1;


                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.ServiciosReservasActividadesInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.ServiciosReservasActividadesInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("ServicioReservaActividades", JsonConvert.SerializeObject(_data), "Error en ServicioReservaActividadesService.Edit: El Idconfempresa no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }
                    /*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();

                    var editData = Mapper.Map<Serviciosreservasactividades>(_data);
                    editData.Idconfempresa = results.First().Idconfempresa;
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idservicioactividad == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<Serviciosreservasactividades>()
                            .Max(x => x.Idservicioactividad);
                        editData.Idservicioactividad = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("ServicioReservaActividades", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new ServiciosReservasActividadesEditResponseDto()
                    {
                        ServiciosReservasActividades = _data,
                        ServiciosReservasActividadesResult = Mapper.Map<ServiciosReservasActividadesEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en ServicioReservaActividadesService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.ServiciosReservasActividadesInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.ServiciosReservasActividadesInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("ServicioReservaActividades", JsonConvert.SerializeObject(_data),
                        "Error en ServicioReservaActividadesService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }
    }
}
