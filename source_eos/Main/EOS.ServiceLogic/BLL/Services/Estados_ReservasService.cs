using System;
using System.CodeDom;
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
    public class Estados_ReservasService : IService
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

        private FilterEstados_Reservas _filter;
        private Estados_ReservasEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;

        #endregion

        public Estados_ReservasService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterEstados_Reservas>();
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query = _session.Query<EstadosReservas>()
                .Join(_session.Query<Expediente>(), er => new {IdEx = er.Idexpediente},
                    ex => new {IdEx = ex.Idxpediente}, (er, ex) => new {er, ex.Idamec})
                .Join(_session.Query<Amec>(), erex => new {IdAmec = erex.Idamec}, am => new {IdAmec = am.Idamec},
                    (erex, am) => new {erex.er, am.Idconfempresa})
                .Join(_session.Query<Confempresa>(),
                    erexam => new {IdConfEmpresa = erexam.Idconfempresa ?? Variables.NULL_INT},
                    co => new {IdConfEmpresa = co.Idconfempresa}, (erexam, co) => new {erexam.er, co.Idagencia})
                .Where(x => x.Idagencia == _agencyKey);

            if (_filter.IdReserva != Variables.NULL_INT)
            {
                query = query.Where(x => x.er.Idreserva == _filter.IdReserva);
            }
            if (_filter.IdExpediente != Variables.NULL_INT)
            {
                query = query.Where(x => x.er.Idexpediente == _filter.IdExpediente);
            }
            if (_filter.IdRegistre != Variables.NULL_INT)
            {
                query = query.Where(x => x.er.Idregistre == _filter.IdRegistre);
            }
            if (_filter.IdServicio != Variables.NULL_INT)
            {
                query = query.Where(x => x.er.Idservicio == _filter.IdServicio);
            }
            if (_filter.Sync != Variables.NULL_INT)
            {
                query = query.Where(x => x.er.Sync == _filter.Sync);
            }
            if (!string.IsNullOrEmpty(_filter.IdEstadoFinal))
            {
                query = query.Where(x => x.er.Idestadofinal == _filter.IdEstadoFinal);
            }
            if (!string.IsNullOrEmpty(_filter.IdEstadoInicial))
            {
                query = query.Where(x => x.er.Idestadoinicial == _filter.IdEstadoInicial);
            }
            if (!string.IsNullOrEmpty(_filter.TransactionKey))
            {
                query = query.Where(x => x.er.TransactionKey == _filter.TransactionKey);
            }

            if (!string.IsNullOrWhiteSpace(_filter.FechacambioestadoInicial))
            {
                query =
                    query.Where(
                        x =>
                            x.er.Fechacambioestado >=
                            DateTime.ParseExact(_filter.FechacambioestadoInicial, Variables.DATE_TIME_FORMAT, null));
            }

            if (!string.IsNullOrWhiteSpace(_filter.FechacambioestadoFinal))
            {
                query =
                    query.Where(
                        x =>
                            x.er.Fechacambioestado <=
                            DateTime.ParseExact(_filter.FechacambioestadoFinal, Variables.DATE_TIME_FORMAT, null));
            }

            var results =
                query.Skip((_filter.CurrentPageIndex.Value - 1)*Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<Estados_ReservasDto>(x.er)).ToList();

            int rowCount = query.Count();

            var result = new Estados_ReservasResponseDto()
            {
                EstadosReservasList = results,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                IdReserva = _filter.IdReserva,
                IdRegistre = _filter.IdRegistre,
                IdServicio = _filter.IdServicio,
                Sync = _filter.Sync,
                IdEstadoInicial = _filter.IdEstadoInicial,
                IdEstadoFinal = _filter.IdEstadoFinal,
                TransactionKey = _filter.TransactionKey,
                PageCount = (int) Math.Ceiling(rowCount*1.0/Variables.ServicePageSize),
                FechacambioestadoInicial = _filter.FechacambioestadoInicial,
                FechacambioestadoFinal = _filter.FechacambioestadoFinal
            };

            _logService.AddLogEntry("Estados_Reservas", JsonConvert.SerializeObject(_filter), rowCount.ToString(),
                _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<Estados_ReservasEditDto>();
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
                    var query =
                        _session.Query<Expediente>()
                            .Join(_session.Query<Amec>(), exp => new {IdAmec = exp.Idamec},
                                am => new {IdAmec = am.Idamec}, (erex, am) => new {erex.Idxpediente, am.Idconfempresa})
                            .Join(_session.Query<Confempresa>(),
                                erexam => new {IdConfEmpresa = erexam.Idconfempresa ?? Variables.NULL_INT},
                                co => new {IdConfEmpresa = co.Idconfempresa},
                                (erexam, co) => new {erexam.Idxpediente, co})
                            .Where(x => x.co.Idagencia == _agencyKey && x.Idxpediente == _data.Idexpediente);

                    var results = query.ToList();
                    bool isAgencyService = results.Count == 1;


                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.Estados_ReservasInsertUpdateError.GetHashCode(),
                            Description =
                                Utility.GetDisplayName(typeof(ErrorCode),
                                    ErrorCode.Estados_ReservasInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("Estados_reservas", JsonConvert.SerializeObject(_data),
                            "Error en Estados_ReservasService.Edit: El Idexpediente no pertenece a la agencia que está usando el servicio",
                            _agencyKey);
                        transaction.Commit();
                        return result;
                    }
                    /*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();



                    var editData = Mapper.Map<EstadosReservas>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idregistre == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<EstadosReservas>()
                            .Max(x => x.Idregistre);
                        editData.Idregistre = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("Estados_Reservas", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new Estados_ReservasEditResponseDto()
                    {
                        Estados_Reservas = _data,
                        Estados_ReservasResult = Mapper.Map<Estados_ReservasEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en Estados_ReservasService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.Estados_ReservasInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.Estados_ReservasInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("Estados_Reservas", JsonConvert.SerializeObject(_data),
                        "Error en Estados_ReservasService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }

    }
}
