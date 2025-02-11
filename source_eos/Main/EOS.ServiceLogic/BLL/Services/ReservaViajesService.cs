using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using EOS.ServiceLogic;
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
    public class ReservaViajesService : IService
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

        private FilterReservaViajes _filter;
        private ReservasViajesEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public ReservaViajesService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterReservaViajes>
            {
                DateFromProp = "LastUpdateDateFrom",
                DateToProp = "LastUpdateDateTo",
                DateTimeFromProp = "DateFrom",
                DateTimeToProp = "DateTo"
            };
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var quer = _session.Query<Reservasviajes>()
               .Join(_session.Query<Expediente>(), re => new { IdExpediente = re.Fkidexpediente },
                   ex => new { IdExpediente = ex.Idxpediente }, (re, ex) => new { re.Idreserva, ex.Idxpediente, ex.Idamec, re })
               .Join(_session.Query<Amec>(), amex => new { IdAmec = amex.Idamec }, am => new { IdAmec = am.Idamec },
                   (amex, am) => new { amex.Idxpediente, am.Idamec, am.Idconfempresa, amex.Idreserva, amex.re })
               .Join(_session.Query<Confempresa>(), amexco => new { IdConf = amexco.Idconfempresa ?? Variables.NULL_INT },
                   co => new { IdConf = co.Idconfempresa },
                   (amexco, co) => new { amexco.Idxpediente, amexco.Idreserva, co.Idconfempresa, co.Idagencia, amexco.re })
               .Where(x => x.Idagencia == _agencyKey);


            if (_filter.IdExpediente != Variables.NULL_INT)
            {
                quer = quer.Where(x => x.Idxpediente == _filter.IdExpediente.Value);
            }

            if (_filter.IdReserva != Variables.NULL_INT)
            {
                quer = quer.Where(x => x.Idreserva == _filter.IdReserva.Value);
            }

            if (!string.IsNullOrEmpty(_filter.IdEstado))
            {
                quer = quer.Where(x => x.re.Idestado == _filter.IdEstado);
            }

            if (_filter.DateFrom != DateTime.MinValue)
            {
                quer = quer.Where(x => x.re.Lastupd >= _filter.DateFrom);
            }

            if (_filter.DateTo != DateTime.MinValue)
            {
                quer = quer.Where(x => x.re.Lastupd <= _filter.DateTo);
            }

            var list =
                quer.Skip((_filter.CurrentPageIndex - 1 ?? 0) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<ReservaViajesDto>(x.re)).ToList();

            int rowCount = quer.Count();

            var result = new ReservaViajesResponseDto()
            {
                ReservasViajesList = list,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                IdExpediente = _filter.IdExpediente,
                IdReserva = _filter.IdReserva,
                IdEstado = _filter.IdEstado,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize),
                LastUpdateDateFrom = _filter.LastUpdateDateFrom,
                LastUpdateDateTo = _filter.LastUpdateDateTo
            };

            _logService.AddLogEntry("ReservaViajes", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }



        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<ReservasViajesEditDto>();
            _data = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
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
                    var query = _session.Query<Reservasviajes>()
               .Join(_session.Query<Expediente>(), re => new { IdExpediente = re.Fkidexpediente },
                   ex => new { IdExpediente = ex.Idxpediente }, (re, ex) => new { re.Idreserva, ex.Idxpediente, ex.Idamec, re })
               .Join(_session.Query<Amec>(), amex => new { IdAmec = amex.Idamec }, am => new { IdAmec = am.Idamec },
                   (amex, am) => new { amex.Idxpediente, am.Idamec, am.Idconfempresa, amex.Idreserva, amex.re })
               .Join(_session.Query<Confempresa>(), amexco => new { IdConf = amexco.Idconfempresa ?? Variables.NULL_INT },
                   co => new { IdConf = co.Idconfempresa },
                   (amexco, co) => new { amexco.Idxpediente, amexco.Idreserva, co.Idconfempresa, co.Idagencia, amexco.re })
               .Where(x => x.Idagencia == _agencyKey && x.Idxpediente == _data.Fkidexpediente);

                    if (_data.Idreserva != Variables.NULL_INT)
                    {
                        query = query.Where(x => x.Idreserva == _data.Idreserva);
                    }

                    var results = query.ToList();
                    bool isAgencyService = results.Count >= 1;

                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.ReservaViajesInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.ReservaViajesInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("Reservasviajes", JsonConvert.SerializeObject(_data), "Error en ReservaViajesService.Edit: El idreserva no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }
                    /*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();

                    var editData = Mapper.Map<Reservasviajes>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idreserva == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<Reservasviajes>()
                            .Max(x => x.Idreserva);
                        editData.Idreserva = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        _session.Evict(results[0].re);
                        _session.Flush();
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("Reservasviajes", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new ReservaViajesEditResponseDto()
                    {
                        ReservaViajes = _data,
                        ReservaViajesResult = Mapper.Map<ReservasViajesEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en ReservaViajesService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.ReservaViajesInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.ReservaViajesInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("Reservasviajes", JsonConvert.SerializeObject(_data),
                        "Error en ReservaViajesService.Edit: " + e.Message, _agencyKey);

                    return result;
                }

            }

        }


    }
}