using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace EOS.ServiceLogic.BLL.Services
{
    public class ReservaPassengersListService : IService
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

        private FilterReservasPassengerList _filter;
        private ReservaPassengersEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public ReservaPassengersListService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterReservasPassengerList>();
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

            var validateService = new ValidateService<ReservaPassengersEditDto>();
            _data = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {

            var query = _session.Query<Expediente>()
                .Join(_session.Query<Amec>(), exp => new { IdAmec = exp.Idamec },
                    amc => new { IdAmec = amc.Idamec }, (exp, amc) => new { exp, amc })
                .Join(_session.Query<Confempresa>(),
                    aex => new { IdConf = aex.amc.Idconfempresa ?? Variables.NULL_INT },
                    conf => new { IdConf = conf.Idconfempresa }, (aex, conf) => new { aex, conf })
                .Join(_session.Query<ReservasPassengersList>(), eac => new { IdExpediente = eac.aex.exp.Idxpediente },
                    res => new { IdExpediente = res.Idxpediente }, (eac, res) => new { res, eac })
                .Where(x => x.eac.conf.Idagencia == _agencyKey);


            if (_filter.IdExpediente != Variables.NULL_INT)
            {
                query = query.Where(x => x.res.Idxpediente == _filter.IdExpediente);
            }
            if (_filter.IdReservaPassengersList != Variables.NULL_INT)
            {
                query = query.Where(x => x.res.Idreservapassengerlist == _filter.IdReservaPassengersList);
            }
            if (_filter.IdPassengerlist != Variables.NULL_INT)
            {
                query = query.Where(x => x.res.Idpassengerlist == _filter.IdPassengerlist);
            }

            var reservas =
                query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<ReservaPassengersDto>(x.res)).ToList();

            int rowCount = query.Count();

            var result = new ReservaPassengersResponseDto()
            {
                ReservaPassengersListList = reservas,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize),
                IdExpediente = _filter.IdExpediente,
                IdReservaPassengersList = _filter.IdReservaPassengersList,
                IdPassengerlist = _filter.IdPassengerlist
            };

            _logService.AddLogEntry("ReservaPassengersList", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

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
                    var query = _session.Query<Expediente>()
                .Join(_session.Query<Amec>(), exp => new { IdAmec = exp.Idamec },
                    amc => new { IdAmec = amc.Idamec }, (exp, amc) => new { exp, amc })
                .Join(_session.Query<Confempresa>(),
                    aex => new { IdConf = aex.amc.Idconfempresa ?? Variables.NULL_INT },
                    conf => new { IdConf = conf.Idconfempresa }, (aex, conf) => new { aex, conf })
                .Where(x => x.conf.Idagencia == _agencyKey && x.aex.exp.Idxpediente == _data.Idxpediente);

                    var results = query.ToList();
                    bool isAgencyService = results.Count >= 1;

                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.ReservasPassengersListInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.ReservasPassengersListInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("ReservaPassengersList", JsonConvert.SerializeObject(_data), "Error en ReservaPassengersListService.Edit: El idreservapassengerlist no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }
                    /*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();

                    var editData = Mapper.Map<ReservasPassengersList>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idreservapassengerlist == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<ReservasPassengersList>()
                            .Max(x => x.Idreservapassengerlist);
                        editData.Idreservapassengerlist = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("ReservasPassengersList", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new ReservaPassengersEditResponseDto()
                    {
                        ReservaPassengers = _data,
                        ReservaPassengersResult = Mapper.Map<ReservaPassengersEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en ReservasPassengersListService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.ReservasPassengersListInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.ReservasPassengersListInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("ReservasPassengersList", JsonConvert.SerializeObject(_data),
                        "Error en ReservasPassengersListService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }
    }
}
