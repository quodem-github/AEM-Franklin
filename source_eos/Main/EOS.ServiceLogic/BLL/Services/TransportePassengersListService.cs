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
    public class TransportePassengersListService : IService
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

        private FilterTransportePassengersList _filter;
        private TransportePassengersListEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public TransportePassengersListService(string token, string agencyKey)
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


            var validateService = new ValidateService<FilterTransportePassengersList>();
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

            var validateService = new ValidateService<TransportePassengersListEditDto>();
            _data = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query = GetListByFiltersAndAgency();
            var transList =
                query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<TransportePassengersListDto>(x)).ToList();

            int rowCount = query.Count();

            var result = new TransportePassengersListResponseDto()
            {
                TransportePassengersListList = transList,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                IdExpediente = _filter.IdExpediente,
                IdPassengerlist = _filter.IdPassengerlist,
                IdTransportePassengersList = _filter.IdTransportePassengersList,
                IdServicioTransporte = _filter.IdServicioTransporte,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("TransportePassengersList", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

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
                    .Where(x => x.Idagencia == _agencyKey && x.serv.serv.serv.serv.Idserviciotransporte == _data.Idserviciotransporte);

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
                        _logService.AddLogEntry("transportepassengerslist", JsonConvert.SerializeObject(_data), "Error en TransportePassengerListService.Edit: El idtransportepassengerlist no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }
                    /*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();

                    var editData = Mapper.Map<Transportepassengerslist>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idtransportepassengerlist == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<Transportepassengerslist>()
                            .Max(x => x.Idtransportepassengerlist);
                        editData.Idtransportepassengerlist = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("transportepassengerslist", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new TransportePassengersListEditResponseDto()
                    {
                        TransportePassengersList = _data,
                        TransportePassengersListResult = Mapper.Map<TransportePassengersListEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en TransportePassengersListService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.TransportepassengerslistInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.TransportepassengerslistInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("transportepassengerslist", JsonConvert.SerializeObject(_data),
                        "Error en TransportePassengersListService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }

        public List<Transportepassengerslist> GetListByFiltersAndAgency(bool edit = false)
        {
            var query = _session.Query<Transportepassengerslist>()
                .Join(_session.Query<Serviciosreservastransporte>(),
                    trans => new { IdService = trans.Idserviciotransporte },
                    sertrans => new { IdService = sertrans.Idserviciotransporte },
                    (trans, sertrans) => new { trans, sertrans.Idserviciotransporte })
                .Join(_session.Query<Serviciosreservasviajes>(),
                    trans => new { IdServicioTransporte = trans.Idserviciotransporte },
                    serv => new { IdServicioTransporte = serv.Idserviciotransporte ?? Variables.NULL_INT },
                    (trans, serv) => new { trans.trans, serv.Idreserva })
                .Join(_session.Query<Reservasviajes>(), trans => new { IdReserva = trans.Idreserva },
                    resv => new { IdReserva = resv.Idreserva },
                    (trans, resv) => new { trans.trans, resv.Fkidexpediente })
                .Join(_session.Query<Expediente>(), trans => new { IdExpediente = trans.Fkidexpediente }, exp => new { IdExpediente = exp.Idxpediente }, (trans, exp) => new { trans.trans, exp.Idamec, exp.Idxpediente })
                .Join(_session.Query<Amec>(), trans => new { IdAmec = trans.Idamec },
                    am => new { IdAmec = am.Idamec },
                    (trans, am) => new { trans.trans, am.Idconfempresa, trans.Idxpediente })
                .Join(_session.Query<Confempresa>(),
                    trans => new { IdConfEmpresa = trans.Idconfempresa ?? Variables.NULL_INT },
                    conf => new { IdConfEmpresa = conf.Idconfempresa },
                    (trans, conf) => new { trans.trans, trans.Idxpediente, conf.Idagencia })
                .Where(x => x.Idagencia == _agencyKey);

            if (edit)
                return query.Select(x => x.trans).Distinct().ToList();

            if (_filter.IdExpediente != Variables.NULL_INT)
            {
                query = query.Where(x => x.Idxpediente == _filter.IdExpediente.Value);
            }
            if (_filter.IdServicioTransporte != Variables.NULL_INT)
            {
                query = query.Where(x => x.trans.Idserviciotransporte == _filter.IdServicioTransporte.Value);
            }
            if (_filter.IdPassengerlist != Variables.NULL_INT)
            {
                query = query.Where(x => x.trans.Idpassengerlist == _filter.IdPassengerlist.Value);
            }
            if (_filter.IdTransportePassengersList != Variables.NULL_INT)
            {
                query = query.Where(x => x.trans.Idtransportepassengerlist == _filter.IdTransportePassengersList.Value);
            }

            return query.Select(x => x.trans).Distinct().ToList();
        }
    }
}
