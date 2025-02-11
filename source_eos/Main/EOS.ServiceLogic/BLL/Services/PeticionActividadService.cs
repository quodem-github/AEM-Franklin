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
    public class PeticionActividadService : IService
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

        private FilterPeticionActividad _filter;
        private PeticionActividadEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public PeticionActividadService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterPeticionActividad>()
            {
                DateFromProp = "PeticionActividadDateFrom",
                DateToProp = "PeticionActividadDateTo",
                DateTimeFromProp = "DateFrom",
                DateTimeToProp = "DateTo",
                DateFromProp2 = "PeticionActividadCreationDateFrom",
                DateToProp2 = "PeticionActividadCreationDateTo",
                DateTimeFromProp2 = "CreationDateFrom",
                DateTimeToProp2 = "CreationDateTo"
            };
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query =
                _session.Query<PeticionesActividad>()
                    .Join(_session.Query<Confempresa>(),
                        pet => new { IdConf = pet.Idconfempresa },
                        conf => new { IdConf = conf.Idconfempresa }, (pet, conf) => new { pet, conf.Idagencia })
                    .Where(x => x.Idagencia == _agencyKey);

            if (_filter.DateFrom != DateTime.MinValue)
            {
                query = query.Where(x => x.pet.Desde >= _filter.DateFrom);
            }
            if (_filter.DateTo != DateTime.MinValue)
            {
                query = query.Where(x => x.pet.Hasta <= _filter.DateTo);
            }
            if (_filter.CreationDateFrom != DateTime.MinValue)
            {
                query = query.Where(x => x.pet.Fechacreacion.Value >= _filter.CreationDateFrom);
            }
            if (_filter.CreationDateTo != DateTime.MinValue)
            {
                query = query.Where(x => x.pet.Fechacreacion.Value <= _filter.CreationDateTo);
            }
            if (_filter.IdPeticionActividad != Variables.NULL_INT)
            {
                query = query.Where(x => x.pet.Idpeticionactividad == _filter.IdPeticionActividad.Value);
            }
            if (!string.IsNullOrEmpty(_filter.PeticionActividad))
            {
                query = query.Where(x => x.pet.Nombre.ToLower().Contains(_filter.PeticionActividad.ToLower()));
            }

            var petAct =
                query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<PeticionActividadDto>(x.pet)).ToList();

            int rowCount = query.Count();

            var result = new PeticionActividadResponseDto()
            {
                PeticionActividadList = petAct,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize),
                PeticionActividad = _filter.PeticionActividad,
                IdPeticionActividad = _filter.IdPeticionActividad,
                PeticionActividadDateFrom = _filter.PeticionActividadDateFrom,
                PeticionActividadDateTo = _filter.PeticionActividadDateTo,
                PeticionActividadCreateDateFrom = _filter.PeticionActividadCreationDateFrom,
                PeticionActividadCreateDateTo = _filter.PeticionActividadCreationDateTo
            };

            _logService.AddLogEntry("PeticionActividad", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<PeticionActividadEditDto>();
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
                    var query = _session.Query<PeticionesActividad>()
                     .Join(_session.Query<Confempresa>(),
                         pet => new { IdConf = pet.Idconfempresa },
                         conf => new { IdConf = conf.Idconfempresa },
                         (pet, conf) => new { conf.Idconfempresa, conf.Idagencia, pet })

                     .Where(x => x.Idagencia == _agencyKey);

                    if (_data.Idpeticionactividad != Variables.NULL_INT)
                    {
                        query = query.Where(x => x.pet.Idpeticionactividad == _data.Idpeticionactividad);
                    }

                    var results = query.ToList();
                    bool isAgencyService = results.Count >= 1;

                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.PeticionActividadInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.PeticionActividadInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("PeticionActividad", JsonConvert.SerializeObject(_data), "Error en PeticionActividad.Edit: El Idconfempresa no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }

                    var editData = Mapper.Map<PeticionesActividad>(_data);
                    editData.Idconfempresa = results.First().Idconfempresa;
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idpeticionactividad == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<PeticionesActividad>()
                            .Max(x => x.Idpeticionactividad);
                        editData.Idpeticionactividad = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        _session.Evict(results[0].pet);
                        _session.Flush();
                        editData.Idconfempresa = _session.Query<Confempresa>().First(x => x.Idagencia == _agencyKey).Idconfempresa;
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("PeticionActividad", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new PeticionActividadEditResponseDto()
                    {
                        PeticionActividad = _data,
                        PeticionActividadResult = Mapper.Map<PeticionActividadEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en PeticionActividadService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.PeticionActividadInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.PeticionActividadInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("PeticionActividad", JsonConvert.SerializeObject(_data),
                        "Error en PeticionActividadService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }
    }
}
