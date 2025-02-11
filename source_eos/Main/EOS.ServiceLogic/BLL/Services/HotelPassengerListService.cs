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
    public class HotelPassengerListService : IService
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

        private FilterHotelPassengerList _filter;
        private HotelPassengerListEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public HotelPassengerListService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterHotelPassengerList>();
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

            var validateService = new ValidateService<HotelPassengerListEditDto>();
            _data = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query = _session.Query<Expediente>()
                .Join(_session.Query<Reservasviajes>(), exp => new { IdExpediente = exp.Idxpediente }, resv => new { IdExpediente = resv.Fkidexpediente }, (exp, resv) => new { resv.Idreserva, exp.Idxpediente, exp.Idamec, exp.Idestado })
                .Join(_session.Query<Serviciosreservasviajes>(), exres => new { IdReserva = exres.Idreserva }, sres => new { IdReserva = sres.Idreserva }, (exres, sres) => new { exres.Idestado, exres.Idxpediente, exres.Idamec, sres.Idserviciohotel })
                .Join(_session.Query<HotelPassengersList>(), ers => new { IdHotel = ers.Idserviciohotel ?? Variables.NULL_INT }, hot => new { IdHotel = hot.Idserviciohotel }, (ers, hot) => new { ers.Idestado, hot, ers.Idxpediente, ers.Idamec })
                .Join(_session.Query<Amec>(), exhot => new { IdAmec = exhot.Idamec }, am => new { IdAmec = am.Idamec }, (exhot, am) => new { exhot.Idestado, exhot.hot, exhot.Idxpediente, am.Idconfempresa })
                .Join(_session.Query<Confempresa>(), exhot => new { IdConf = exhot.Idconfempresa ?? Variables.NULL_INT }, conf => new { IdConf = conf.Idconfempresa },
                    (exhot, conf) => new { exhot.Idxpediente, exhot.hot, conf.Idagencia, exhot.Idestado })
                .Where(x => x.Idagencia == _agencyKey);


            if (_filter.IdPassengerlist != Variables.NULL_INT)
            {
                query = query.Where(x => x.hot.Idpassengerlist == _filter.IdPassengerlist);
            }

            if (_filter.IdHotelPassengerlist != Variables.NULL_INT)
            {
                query = query.Where(x => x.hot.Idhotelpassengerlist == _filter.IdHotelPassengerlist.Value);
            }
            if (_filter.IdServicioHotel != Variables.NULL_INT)
            {
                query = query.Where(x => x.hot.Idserviciohotel == _filter.IdServicioHotel.Value);
            }
            if (!string.IsNullOrEmpty(_filter.IdEstado))
            {
                query = query.Where(x => x.Idestado == _filter.IdEstado);
            }

            if (_filter.IdExpediente != Variables.NULL_INT)
            {
                query = query.Where(x => x.Idxpediente == _filter.IdExpediente.Value);
            }

            var results =
                query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<HotelPassengerListDto>(x.hot)).ToList();

            int rowCount = query.Count();

            var result = new HotelPassengerListResponseDto()
            {
                HotelPassengersListList = results,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                IdExpediente = _filter.IdExpediente,
                IdHotelPassengerlist = _filter.IdHotelPassengerlist,
                IdPassengerlist = _filter.IdPassengerlist,
                IdServicioHotel = _filter.IdServicioHotel,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("HotelPassengerList", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

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
                    .Where(x => x.Idagencia == _agencyKey && x.serv.serv.serv.serv.Idserviciohotel == _data.Idserviciohotel);

                    var results = query.ToList();
                    bool isAgencyService = results.Count >= 1;

                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.HotelPassengerListInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.HotelPassengerListInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("hotelpassengerlist", JsonConvert.SerializeObject(_data), "Error en HotelPassengerListService.Edit: El idhotelpassengerlist no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }
                    /*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();

                    var editData = Mapper.Map<HotelPassengersList>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idhotelpassengerlist == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<HotelPassengersList>()
                            .Max(x => x.Idhotelpassengerlist);
                        editData.Idhotelpassengerlist = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("hotelpassengerlist", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new HotelPassengerListEditResponseDto()
                    {
                        HotelPassengersList = _data,
                        HotelPassengersListResult = Mapper.Map<HotelPassengerListEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en HotelPassengerListService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.HotelPassengerListInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.HotelPassengerListInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("hotelpassengerlist", JsonConvert.SerializeObject(_data),
                        "Error en HotelPassengerListService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }
        }
    }
}
