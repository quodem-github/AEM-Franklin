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
    public class ServiciosReservasTransporteService : IService
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

        private FilterServiciosReservasTransporte _filter;
        private ServiciosReservasTransporteEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public ServiciosReservasTransporteService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterServiciosReservasTransporte>();
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
                    (re, srv) => new { srv.Idservicio, re.Idreserva, re.Idxpediente, re.Idagencia, srv.Idserviciotransporte })
                .Join(_session.Query<Serviciosreservastransporte>(), srv => new { IdServicioTransporte = srv.Idserviciotransporte ?? Variables.NULL_INT },
                    srh => new { IdServicioTransporte = srh.Idserviciotransporte },
                    (srv, srh) => new { srv.Idxpediente, srv.Idreserva, srv.Idservicio, srv.Idagencia, srh })
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
            if (_filter.IdServicioTransporte != Variables.NULL_INT)
            {
                query = query.Where(x => x.srh.Idserviciotransporte == _filter.IdServicioTransporte.Value);
            }

            var list =
                query.Skip((_filter.CurrentPageIndex - 1 ?? 0) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<ServiciosReservasTransporteDto>(x.srh)).ToList();

            int rowCount = query.Count();

            var result = new ServiciosReservasTransporteResponseDto()
            {
                ServiciosReservasTransporteList = list,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                IdExpediente = _filter.IdExpediente,
                IdReserva = _filter.IdReserva,
                IdServicio = _filter.IdServicio,
                IdServicioTransporte = _filter.IdServicioTransporte,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("ServiciosReservasTransporte", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<ServiciosReservasTransporteEditDto>();
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
                    var query = _session.Query<Serviciosreservastransporte>()
                .Join(_session.Query<Confempresa>(),
                    srt => new { IdConf = srt.Idconfempresa },
                    co => new { IdConf = co.Idconfempresa },
                    (srt, co) => new { srt, co.Idagencia, co.Idconfempresa })

                .Where(x => x.Idagencia == _agencyKey);

                    if (_data.Idserviciotransporte != Variables.NULL_INT)
                    {
                        query = query.Where(x => x.srt.Idserviciotransporte == _data.Idserviciotransporte);
                    }

                    var results = query.ToList();
                    bool isAgencyService = results.Count >= 1;

                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.ServiciosReservasTransporteInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.ServiciosReservasTransporteInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("Serviciosreservastransporte", JsonConvert.SerializeObject(_data), "Error en ServiciosReservasTransporteService.Edit: El Idconfempresa no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }
                    /*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();

                    var editData = Mapper.Map<Serviciosreservastransporte>(_data);
                    editData.Idconfempresa = results.First().Idconfempresa;
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idserviciotransporte == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<Serviciosreservastransporte>()
                            .Max(x => x.Idserviciotransporte);
                        editData.Idserviciotransporte = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new ServiciosReservasTransporteEditResponseDto()
                    {
                        ServiciosReservasTransporte = _data,
                        ServiciosReservasTransporteResult = Mapper.Map<ServiciosReservasTransporteEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("Serviciosreservastransporte", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en ServiciosReservasTransporteService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.ServiciosReservasTransporteInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.ServiciosReservasTransporteInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("Serviciosreservastransporte", JsonConvert.SerializeObject(_data),
                        "Error en ServiciosReservasTransporteService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }
    }
}
