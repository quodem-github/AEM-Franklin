using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
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
    public class ActividadesPassengersListService : IService
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

        private FilterActividadesPassengersList _filter;
        private ActividadesPassengersListEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public ActividadesPassengersListService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterActividadesPassengersList>();
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

            var validateService = new ValidateService<ActividadesPassengersListEditDto>();
            _data = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query =
                _session.Query<ActividadesPassengersList>()
                    .Join(_session.Query<Serviciosreservasviajes>(),
                        ac => new { IdServicioActividad = ac.Idservicioactividad },
                        sres => new { IdServicioActividad = sres.Idservicioactividad ?? Variables.NULL_INT },
                        (ac, sres) => new { ac, sres.Idreserva })
                    .Join(_session.Query<Reservasviajes>(), acts => new { IdReserva = acts.Idreserva },
                        resv => new { IdReserva = resv.Idreserva }, (acts, resv) => new { acts.ac, resv.Fkidexpediente })
                    .Join(_session.Query<Expediente>(), act => new { IdExpediente = act.Fkidexpediente },
                        exp => new { IdExpediente = exp.Idxpediente },
                        (act, exp) => new { act.ac, exp.Idxpediente, exp.Idamec })
                    .Join(_session.Query<Amec>(), act => new { IdAmec = act.Idamec }, amc => new { IdAmec = amc.Idamec },
                        (act, amc) => new { act.ac, act.Idxpediente, amc.Idconfempresa })
                    .Join(_session.Query<Confempresa>(), ac => new { IdConf = ac.Idconfempresa ?? Variables.NULL_INT },
                        conf => new { IdConf = conf.Idconfempresa },
                        (ac, conf) => new { ac.ac, ac.Idxpediente, conf.Idagencia })
                    .Where(x => x.Idagencia == _agencyKey);

            if (_filter.IdActividadPassengerList != Variables.NULL_INT)
            {
                query = query.Where(x => x.ac.Idactividadpassengerlist == _filter.IdActividadPassengerList);
            }

            if (_filter.IdExpediente != Variables.NULL_INT)
            {
                query = query.Where(x => x.Idxpediente == _filter.IdExpediente);
            }
            if (_filter.IdServicioActividad != Variables.NULL_INT)
            {
                query = query.Where(x => x.ac.Idservicioactividad == _filter.IdServicioActividad.Value);
            }
            if (_filter.IdPassengerlist != Variables.NULL_INT)
            {
                query = query.Where(x => x.ac.Idpassengerlist == _filter.IdPassengerlist);
            }

            var tarifas =
                query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<ActividadesPassengersListDto>(x.ac)).ToList();

            int rowCount = query.Count();

            var result = new ActividadesPassengersListResponseDto()
            {
                ActividadesPassengerListList = tarifas,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize),
                IdActividadPassengerList = _filter.IdActividadPassengerList,
                IdPassengerlist = _filter.IdPassengerlist,
                IdServicioActividad = _filter.IdServicioActividad,
                IdExpediente = _filter.IdExpediente
            };

            _logService.AddLogEntry("ActividadesPassengersList", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);


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
                    var query =
                    _session.Query<Serviciosreservasviajes>()
                    .Join(_session.Query<Reservasviajes>(),
                        serv => new { IdReserva = serv.Idreserva },
                        resv => new { IdReserva = resv.Idreserva }, (serv, resv) => new { serv, resv.Fkidexpediente })
                    .Join(_session.Query<Expediente>(), serv => new { IdExpediente = serv.Fkidexpediente },
                        exp => new { IdExpediente = exp.Idxpediente },
                        (serv, exp) => new { serv, exp.Idxpediente, exp.Idamec })
                    .Join(_session.Query<Amec>(), serv => new { IdAmec = serv.Idamec }, amc => new { IdAmec = amc.Idamec },
                        (serv, amc) => new { serv, serv.Idxpediente, amc.Idconfempresa })
                    .Join(_session.Query<Confempresa>(), ac => new { IdConf = ac.Idconfempresa ?? Variables.NULL_INT },
                        conf => new { IdConf = conf.Idconfempresa },
                        (serv, conf) => new { serv, serv.Idxpediente, conf.Idagencia })
                    .Where(x => x.Idagencia == _agencyKey && x.serv.serv.serv.serv.Idservicioactividad == _data.Idservicioactividad.Value);

                    var results = query.ToList();
                    bool isAgencyService = results.Count >= 1;


                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.ActividadesPassengerListInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.ActividadesPassengerListInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("actividades_passengers_list", JsonConvert.SerializeObject(_data), "Error en ActividadPassengerListService.Edit: El Idservicioactividad no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }
                    /*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();

                    var editData = Mapper.Map<ActividadesPassengersList>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idactividadpassengerlist == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<ActividadesPassengersList>()
                            .Max(x => x.Idactividadpassengerlist);
                        editData.Idactividadpassengerlist = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("actividades_passengers_list", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new ActividadesPassengersListEditResponseDto()
                    {
                        ActividadesPassengersList = _data,
                        ActividadesPassengersListResult = Mapper.Map<ActividadesPassengersListEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en ActividadesPassengerListService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.ActividadesPassengerListInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.ActividadesPassengerListInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("actividades_passengers_list", JsonConvert.SerializeObject(_data),
                        "Error en ActividadesPassengerListService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }
    }
}
