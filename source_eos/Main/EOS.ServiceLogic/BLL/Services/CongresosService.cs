using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using EOS.ServiceLogic.BLL.AutoMapper;
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
    public class CongresosService : IService
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

        private FilterCongresos _filter;
        private CongresosEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public CongresosService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterCongresos>
            {
                DateFromProp = "CongresoDateFrom",
                DateToProp = "CongresoDateTo",
                DateTimeFromProp = "DateFrom",
                DateTimeToProp = "DateTo"
            };
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query =
                _session.Query<Congresos>()
                    .Join(_session.Query<Confempresa>(),
                        cong => new { IdConf = cong.Idconfempresa },
                        conf => new { IdConf = conf.Idconfempresa }, (cong, conf) => new { cong, conf.Idagencia })
                    .Where(x => x.Idagencia == _agencyKey);

            if (_filter.IdCongreso != Variables.NULL_INT)
            {
                query = query.Where(x => x.cong.Idcongreso == _filter.IdCongreso.Value);
            }

            if (!string.IsNullOrEmpty(_filter.Congreso))
            {
                query = query.Where(x => x.cong.Congreso.ToUpper().Contains(_filter.Congreso.ToUpper()));
            }

            if (_filter.DateFrom != DateTime.MinValue)
            {
                query = query.Where(x => x.cong.Fechacreacion >= _filter.DateFrom);
            }

            if (_filter.DateTo != DateTime.MinValue)
            {
                query = query.Where(x => x.cong.Fechacreacion <= _filter.DateTo);
            }

            var congresos =
                query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<CongresosDto>(x.cong)).ToList();

            int rowCount = query.Count();

            var result = new CongresosResponseDto()
            {
                CongresoList = congresos,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                DateFrom = _filter.CongresoDateFrom,
                DateTo = _filter.CongresoDateTo,
                Congreso = _filter.Congreso,
                IdCongreso = _filter.IdCongreso,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("Congresos", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<CongresosEditDto>();
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
                    var query = _session.Query<Congresos>()

                     .Join(_session.Query<Confempresa>(),
                         cong => new { IdConf = cong.Idconfempresa },
                         conf => new { IdConf = conf.Idconfempresa },
                         (cong, conf) => new { conf.Idconfempresa, conf.Idagencia, cong })

                     .Where(x => x.Idagencia == _agencyKey);

                    if (_data.Idcongreso != Variables.NULL_INT)
                    {
                        query = query.Where(x => x.cong.Idcongreso == _data.Idcongreso);
                    }

                    var results = query.ToList();
                    bool isAgencyService = results.Count >= 1;

                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.CongresosInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.CongresosInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("Congresos", JsonConvert.SerializeObject(_data), "Error en CongresosService.Edit: El Idconfempresa no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }
                    /*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();

                    var editData = Mapper.Map<Congresos>(_data);
                    editData.Idconfempresa = results.First().Idconfempresa;
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idcongreso == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<Congresos>()
                            .Max(x => x.Idcongreso);
                        editData.Idcongreso = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        _session.Evict(results[0].cong);
                        _session.Flush();
                        editData.Idconfempresa = _session.Query<Confempresa>().First(x => x.Idagencia == _agencyKey).Idconfempresa;
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("Congresos", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new CongresosEditResponseDto()
                    {
                        Congresos = _data,
                        CongresosResult = Mapper.Map<CongresosEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en CongresosService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.CongresosInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.CongresosInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("Congresos", JsonConvert.SerializeObject(_data),
                        "Error en CongresosService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }


    }
}
