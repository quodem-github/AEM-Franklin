using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AmecService : IService
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

        private FilterAmec _filter;
        private AmecEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public AmecService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterAmec>();
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query =
                _session.Query<Amec>()
                    .Join(_session.Query<Confempresa>(), am => new { IdConf = am.Idconfempresa ?? Variables.NULL_INT },
                        conf => new { IdConf = conf.Idconfempresa }, (am, conf) => new { am, conf.Idagencia })
                    .Where(x => x.Idagencia == _agencyKey);

            if (!string.IsNullOrEmpty(_filter.Amec))
            {
                query = query.Where(x => x.am.Amec1 == _filter.Amec);
            }

            if (_filter.IdAmec != null)
            {
                query = query.Where(x => x.am.Idamec == _filter.IdAmec);
            }

            var amecs =
                query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<AmecDto>(x.am)).ToList();

            int rowCount = query.Count();

            var result = new AmecResponseDto()
            {
                AmecList = amecs,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize),
                Amec = _filter.Amec,
                IdAmec = _filter.IdAmec
            };

            _logService.AddLogEntry("Amec", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<AmecEditDto>();
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
                    /*ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/
                    var query =
                _session.Query<Amec>()
                    .Join(_session.Query<Confempresa>(), am => new { IdConf = am.Idconfempresa ?? Variables.NULL_INT },
                        conf => new { IdConf = conf.Idconfempresa }, (am, conf) => new { am, conf.Idagencia })
                    .Where(x => x.Idagencia == _agencyKey);

                    if (_data.Idamec != Variables.NULL_INT)
                    {
                        query = query.Where(x => x.am.Idamec == _data.Idamec);
                    }

                    var results = query.ToList();
                    bool isAgencyService = results.Count == 1;

                    if (!isAgencyService && _data.Idamec != Variables.NULL_INT)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.AmecInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.AmecInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("amec", JsonConvert.SerializeObject(_data), "Error en AmecService.Edit: El idamec no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }
                    /*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();

                    var editData = Mapper.Map<Amec>(_data);
                    editData.Idconfempresa = results.First().am.Idconfempresa;
                    editData.Newco = results.First().am.Newco;
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idamec == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<Amec>()
                            .Max(x => x.Idamec);
                        editData.Idamec = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        editData.Idamecorigin = results[0].am.Idamecorigin;
                        //Se actualiza con el objeto que se recibe.
                        _session.Evict(results[0].am);
                        _session.Flush();                        
                        editData.Idconfempresa = _session.Query<Confempresa>().First(x => x.Idagencia == _agencyKey).Idconfempresa;
                        editData.Newco = results[0].am.Newco;
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new AmecEditResponseDto()
                    {
                        Amec = _data,
                        AmecResult = Mapper.Map<AmecEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("amec", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en AmecService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.AmecInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.AmecInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("amec", JsonConvert.SerializeObject(_data),
                        "Error en AmecService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }

    }
}
