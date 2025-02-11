using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using EOS.ServiceLogic.BLL.Validator;
using EOS.ServiceLogic.Data.DTO.Service;
using EOS.ServiceLogic.Data.Filters;
using EOS.ServiceLogic.Data.ServiceResponse;
using EOS.ServiceModel;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Linq;
using EOS.ServiceLogic.Data.DTO.ServiceEdition;
using EOS.ServiceLogic.Enums;
using EOS.Entidades.Modelo;

namespace EOS.ServiceLogic.BLL.Services
{
    public class TarifaAlojService : IService
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
        private TarifasAlojEditDto _data;
        private FilterTarifasAloj _filter;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public TarifaAlojService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterTarifasAloj>
            {
                DateFromProp = "",
                DateToProp = "",
                DateTimeFromProp = "",
                DateTimeToProp = ""
            };
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query = _session.Query<Tarifasaloj>()
                .Join(_session.Query<Congresos>(), taa => new { IdCongreso = taa.Fkidcongreso },
                    am => new { IdCongreso = am.Idcongreso },
                    (taa, am) => new { taa, am })
                .Join(_session.Query<Confempresa>(), tam => new { IdConfEmpresa = tam.am.Idconfempresa },
                    conf => new { IdConfEmpresa = conf.Idconfempresa }, (tam, conf) => new { tam, conf })
                .Where(x => x.conf.Idagencia == _agencyKey);

            if (_filter.IdCongreso != Variables.NULL_INT)
            {
                query = query.Where(x => x.tam.taa.Fkidcongreso == _filter.IdCongreso.Value);
            }

            if (_filter.IdTarifaAloj != Variables.NULL_INT)
            {
                query = query.Where(x => x.tam.taa.Idtarifaaloj == _filter.IdTarifaAloj.Value);
            }

            var TarifasAloj =
                query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<TarifasAlojDto>(x.tam.taa)).ToList();

            int rowCount = query.Count();

            var result = new TarifasAlojResponseDto()
            {
                TarifasAlojList = TarifasAloj,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                IdCongreso = _filter.IdCongreso,
                IdTarifaAloj = _filter.IdTarifaAloj,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("TarifaAloj", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<TarifasAlojEditDto>();
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
                    var query = _session.Query<Congresos>()
                            .Join(_session.Query<Confempresa>(), tam => new { IdConfEmpresa = tam.Idconfempresa },
                                conf => new { IdConfEmpresa = conf.Idconfempresa }, (tam, conf) => new { tam, conf })
                            .Where(x => x.conf.Idagencia == _agencyKey && x.tam.Idcongreso == _data.Fkidcongreso);

                    var results = query.ToList();
                    bool isAgencyService = results.Count >= 1;

                    List<Tarifasaloj> tarifasList = null;
                    if (isAgencyService && _data.IdTarifaAloj != Variables.NULL_INT)
                    {
                        tarifasList = _session.Query<Tarifasaloj>().Where(x => x.Idtarifaaloj == _data.IdTarifaAloj).ToList();
                        isAgencyService= tarifasList.Count >= 1;
                    }

                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.TarifasAlojInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.TarifasAlojInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("Tarifasaloj", JsonConvert.SerializeObject(_data), "Error en TarifasAlojService.Edit: El Idtarifaactividad no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }

                    //*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();
                    if (tarifasList != null && tarifasList.Count > 0)
                    {
                        _session.Evict(tarifasList[0]);
                        _session.Flush();
                    }

                    var editData = Mapper.Map<Tarifasaloj>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.

                    if (editData.Idtarifaaloj == Variables.NULL_INT)
                    {
                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<Tarifasaloj>().Max(x => x.Idtarifaaloj);
                        editData.Idtarifaaloj = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("TarifasAloj", JsonConvert.SerializeObject(_data), JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new TarifasAlojEditResponseDto()
                    {
                        TarifasAloj = _data,
                        TarifasAlojResult = Mapper.Map<TarifasAlojEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en TarifaAlojService.Edit: " + e.Message);

                    result.ErrorList.Add(new ErrorItem() { Code = ErrorCode.TarifasAlojInsertUpdateError.GetHashCode(), Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.TarifasAlojInsertUpdateError.ToString()) });

                    transaction.Rollback();
                    _logService.AddLogEntry("TarifaAloj", JsonConvert.SerializeObject(_data),
                        "Error en TarifaAlojService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }
        }
    }
}
