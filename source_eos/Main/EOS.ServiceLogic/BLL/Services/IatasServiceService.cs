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
    public class IatasService : IService
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

        private FilterIatas _filter;
        private IatasEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public IatasService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterIatas>();
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query = _session.Query<Iatas>().Join(_session.Query<Confempresa>(),
                    iata => new { IdConf = iata.Idconfempresa }, conf => new { IdConf = conf.Idconfempresa },
                    (iata, conf) => new
                    {
                        iata,
                        conf
                    })
                 .Where(x => x.conf.Idagencia == _agencyKey).Select(x => x.iata);


            if (!string.IsNullOrEmpty(_filter.IdIata))
            {
                query = query.Where(x => x.Idiata == _filter.IdIata);
            }

            if (!string.IsNullOrEmpty(_filter.Iata))
            {
                query = query.Where(x => x.Iata.ToLower().Contains(_filter.Iata.ToLower()));
            }

          var results =
                query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<IatasDto>(x)).ToList();

            int rowCount = query.Count();

            var result = new IatasReponseDto()
            {
                IatasList = results,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                Idiata = _filter.IdIata,
                Iata = _filter.Iata,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("IatasService", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<IatasEditDto>();
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
                    var results = _session.Query<Iatas>().Join(_session.Query<Confempresa>(),
                    iata => new { IdConf = iata.Idconfempresa }, conf => new { IdConf = conf.Idconfempresa },
                    (iata, conf) => new
                    {
                        iata,
                        conf
                    }).Where(x => x.conf.Idagencia == _agencyKey && x.iata.Idiata == _data.Idiata).Select(x => x.iata).ToList();

                    if (results.Count > 1)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.IatasInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.IatasInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("empleadogp", JsonConvert.SerializeObject(_data), "Error en IatasService.Edit: Más de un resultado por los filtros aplicados", _agencyKey);
                        transaction.Commit();
                        return result;
                    }

                    _session.Flush();

                    var editData = Mapper.Map<Iatas>(_data);
                    if (results.Count == 1)
                    {
                        _session.Evict(results[0]);
                        _session.Flush();
                        editData.Id = results.First().Id;
                        editData.Idconfempresa = results.First().Idconfempresa;
                        _session.SaveOrUpdate(editData);
                    }
                    else
                    {
                        var lastId = _session.Query<Iatas>()
                            .Max(x => x.Id);
                        editData.Id = lastId + 1;
                        editData.Idconfempresa = _session.Query<Confempresa>().First(x => x.Idagencia == _agencyKey).Idconfempresa;
                        _session.Save(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("Iatas", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new IatasEditResponseDto()
                    {
                        Iatas = _data,
                        IatasResult = Mapper.Map<IatasEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en IatasService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.IatasInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.IatasInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("Iatas", JsonConvert.SerializeObject(_data),
                        "Error en IatasService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }

    }
}
