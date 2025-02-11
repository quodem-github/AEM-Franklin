using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TramitacionesServiciosReservasService : IService
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
        private TramitacionesServiciosReservasEditDto _data;
        private FilterTramitacionesServiciosReservas _filter;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public TramitacionesServiciosReservasService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterTramitacionesServiciosReservas>();
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query = _session.Query<Confempresa>()
                .Join(_session.Query<Amec>(), conf => new { IdConf = conf.Idconfempresa },
                    am => new { IdConf = am.Idconfempresa != null ? am.Idconfempresa.Value : Variables.NULL_INT },
                    (conf, am) => new { conf.Idagencia, am.Idamec })
                .Join(_session.Query<Expediente>(), cam => new { IdAmec = cam.Idamec }, exp => new { IdAmec = exp.Idamec },
                    (cam, exp) => new { cam.Idagencia, exp.Idxpediente })
                .Join(_session.Query<Reservasviajes>(), cexp => new { IdExpediente = cexp.Idxpediente },
                    resv => new { IdExpediente = resv.Fkidexpediente },
                    (cexp, resv) => new { cexp.Idagencia, cexp.Idxpediente, resv.Idreserva })
                .Join(_session.Query<Serviciosreservasviajes>(), x => new { IdReserva = x.Idreserva },
                    sres => new { IdReserva = sres.Idreserva },
                    (x, sres) => new { x.Idxpediente, x.Idagencia, x.Idreserva, sres.Idservicio })
                .Join(_session.Query<Tramitacionesserviciosreservas>(), x => new { IdServicio = x.Idservicio },
                    tra => new { IdServicio = tra.Fkidservicio },
                    (x, tra) => new { x.Idxpediente, x.Idagencia, x.Idreserva, x.Idservicio, tra })
                .Where(x => x.Idagencia == _agencyKey);

            if (_filter.IdExpediente != Variables.NULL_INT)
            {
                query = query.Where(x => x.Idxpediente == _filter.IdExpediente);
            }
            if (_filter.IdReserva != Variables.NULL_INT)
            {
                query = query.Where(x => x.Idreserva == _filter.IdReserva);
            }
            if (_filter.IdServicio != Variables.NULL_INT)
            {
                query = query.Where(x => x.Idservicio == _filter.IdServicio);
            }
            if (_filter.IdTramitacion != Variables.NULL_INT)
            {
                query = query.Where(x => x.tra.Idtramitacion == _filter.IdTramitacion);
            }

            var results =
                query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<TramitacionesServiciosReservasDto>(x.tra)).ToList();

            int rowCount = query.Count();

            var result = new TramitacionesServiciosReservasResponseDto()
            {
                TramitacionesServiciosReservasList = results,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                IdExpediente = _filter.IdExpediente,
                IdReserva = _filter.IdReserva,
                IdServicio = _filter.IdServicio,
                IdTramitacion = _filter.IdTramitacion,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("TramitacionesServiciosReservas", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<TramitacionesServiciosReservasEditDto>();
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
                    var query = _session.Query<Confempresa>()
               .Join(_session.Query<Amec>(), conf => new { IdConf = conf.Idconfempresa },
                   am => new { IdConf = am.Idconfempresa != null ? am.Idconfempresa.Value : Variables.NULL_INT },
                   (conf, am) => new { conf.Idagencia, am.Idamec })
               .Join(_session.Query<Expediente>(), cam => new { IdAmec = cam.Idamec }, exp => new { IdAmec = exp.Idamec },
                   (cam, exp) => new { cam.Idagencia, exp.Idxpediente })
               .Join(_session.Query<Reservasviajes>(), cexp => new { IdExpediente = cexp.Idxpediente },
                   resv => new { IdExpediente = resv.Fkidexpediente },
                   (cexp, resv) => new { cexp.Idagencia, cexp.Idxpediente, resv.Idreserva })
               .Join(_session.Query<Serviciosreservasviajes>(), x => new { IdReserva = x.Idreserva },
                   sres => new { IdReserva = sres.Idreserva },
                   (x, sres) => new { x.Idxpediente, x.Idagencia, x.Idreserva, sres.Idservicio })
               .Where(x => x.Idagencia == _agencyKey && x.Idservicio == _data.Fkidservicio);

                    var results = query.ToList();
                    bool isAgencyService = results.Count >= 1;

                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.TramitacionesServiciosReservasInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.TramitacionesServiciosReservasInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("TramitacionesServiciosReservas", JsonConvert.SerializeObject(_data), "Error en TramitacionesServiciosReservasService.Edit: El Idservicio no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }

                    //*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();


                    var editData = Mapper.Map<Tramitacionesserviciosreservas>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idtramitacion == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<Tramitacionesserviciosreservas>()
                            .Max(x => x.Idtramitacion);
                        editData.Idtramitacion = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new TramitacionesServicioReservasEditResponseDto()
                    {
                        TramitacionServicioReservas = _data,
                        TramitacionServicioReservasResult = Mapper.Map<TramitacionesServiciosReservasEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("tramitacionesserviciosreservas", JsonConvert.SerializeObject(_data), JsonConvert.SerializeObject(editData), _agencyKey);

                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en TramitacionesServiciosReservasService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.TramitacionesServiciosReservasInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.TramitacionesServiciosReservasInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("tramitacionesserviciosreservas", JsonConvert.SerializeObject(_data),
                        "Error en TramitacionesServiciosReservasService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }
        }
    }
}
