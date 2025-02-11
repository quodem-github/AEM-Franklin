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
    public class InscripcionPassengersListService : IService
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

        private FilterInscripcionPassengersList _filter;
        private InscripcionPassengersListEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public InscripcionPassengersListService(string token, string agencyKey)
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


            var validateService = new ValidateService<FilterInscripcionPassengersList>();
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

            var validateService = new ValidateService<InscripcionPassengersListEditDto>();
            _data = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query = GetListByFiltersAndAgency();

            var inspass =
                query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<InscripcionPassengersListDto>(x)).ToList();

            int rowCount = query.Count();

            var result = new InscripcionPassengersListResponseDto()
            {
                InscripcionPassengersListList = inspass,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                IdExpediente = _filter.IdExpediente,
                IdPassengerlist = _filter.IdPassengerlist,
                IdInsPassengerlist = _filter.IdInsPassengerlist,
                IdServicioInscripcion = _filter.IdServicioInscripcion,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("InscripcionPassengersList", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

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
                    .Where(x => x.Idagencia == _agencyKey && x.serv.serv.serv.serv.Idservicioinscripcion == _data.Idservicioinscripcion);

                    var results = query.ToList();
                    bool isAgencyService = results.Count >= 1;

                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.InscripcionPassengersListInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.InscripcionPassengersListInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("ins_passengers_list", JsonConvert.SerializeObject(_data), "Error en InscripcionPassengersListService.Edit: El Idservicioinscripcion no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }

                    _session.Flush();


                    var editData = Mapper.Map<InsPassengersList>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idinspassengerlist == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<InsPassengersList>()
                            .Max(x => x.Idinspassengerlist);
                        editData.Idinspassengerlist = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("ins_passengers_list", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new InscripcionPassengersListEditResponseDto()
                    {
                        InscripcionPassengersList = _data,
                        InscripcionPassengersListResult = Mapper.Map<InscripcionPassengersListEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en InscripcionPassengersListService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.InscripcionPassengersListInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.InscripcionPassengersListInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("ins_passengers_list", JsonConvert.SerializeObject(_data),
                        "Error en InscripcionPassengersListService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }

        public List<InsPassengersList> GetListByFiltersAndAgency(bool edit = false)
        {
            var query =
                _session.Query<InsPassengersList>()
                    .Join(_session.Query<Serviciosreservasviajes>(),
                        ins => new { Idservicioinscripcion = ins.Idservicioinscripcion },
                        sres => new { Idservicioinscripcion = sres.Idservicioinscripcion ?? Variables.NULL_INT },
                        (ins, sres) => new { ins, sres.Idreserva })
                    .Join(_session.Query<Reservasviajes>(), acts => new { IdReserva = acts.Idreserva },
                        resv => new { IdReserva = resv.Idreserva }, (ins, resv) => new { ins.ins, resv.Fkidexpediente })
                    .Join(_session.Query<Expediente>(), act => new { IdExpediente = act.Fkidexpediente },
                        exp => new { IdExpediente = exp.Idxpediente },
                        (ins, exp) => new { ins.ins, exp.Idxpediente, exp.Idamec })
                    .Join(_session.Query<Amec>(), act => new { IdAmec = act.Idamec }, amc => new { IdAmec = amc.Idamec },
                        (ins, amc) => new { ins.ins, ins.Idxpediente, amc.Idconfempresa })
                    .Join(_session.Query<Confempresa>(),
                        ins => new { IdConfEmpresa = ins.Idconfempresa ?? Variables.NULL_INT },
                        conf => new { IdConfEmpresa = conf.Idconfempresa },
                        (ins, conf) => new { ins.ins, conf.Idagencia, ins.Idxpediente })
                    .Where(x => x.Idagencia == _agencyKey);

            if (edit)
                return query.Select(x => x.ins).Distinct().ToList();

            if (_filter.IdExpediente != Variables.NULL_INT)
            {
                query = query.Where(x => x.Idxpediente == _filter.IdExpediente.Value);
            }
            if (_filter.IdServicioInscripcion != Variables.NULL_INT)
            {
                query = query.Where(x => x.ins.Idservicioinscripcion == _filter.IdServicioInscripcion.Value);
            }
            if (_filter.IdInsPassengerlist != Variables.NULL_INT)
            {
                query = query.Where(x => x.ins.Idinspassengerlist == _filter.IdInsPassengerlist.Value);
            }
            if (_filter.IdPassengerlist != Variables.NULL_INT)
            {
                query = query.Where(x => x.ins.Idpassengerlist == _filter.IdPassengerlist.Value);
            }

            return query.Select(x => x.ins).Distinct().ToList();
        }
    }
}
