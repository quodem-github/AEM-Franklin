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
    public class PeticionGruposService : IServiceExtended
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

        private FilterPeticionGrupos _filter;
        private PeticionGruposEditDto _data;
        private PeticionGruposDeleteDto _dataDelete;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;

        #endregion

        public PeticionGruposService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterPeticionGrupos>();
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {

            var query = _session.Query<PeticionGrupos>()
                .Join(_session.Query<Expediente>(), pet => new {IdExpediente = pet.Idexpediente.Value},
                    ex => new {IdExpediente = ex.Idxpediente},
                    (pet, ex) => new {pet.Idpeticiongrupo, ex.Idxpediente, ex.Idamec, pet})
                .Join(_session.Query<Amec>(), amex => new {IdAmec = amex.Idamec}, am => new {IdAmec = am.Idamec},
                    (amex, am) => new {amex.Idxpediente, am.Idamec, am.Idconfempresa, amex.pet})
                .Join(_session.Query<Confempresa>(), amexco => new {IdConf = amexco.Idconfempresa.Value},
                    co => new {IdConf = co.Idconfempresa},
                    (amexco, co) => new {amexco.Idxpediente, co.Idconfempresa, co.Idagencia, amexco.pet})
                .Where(x => x.Idagencia == _agencyKey);

            if (_filter.IdAsistente != Variables.NULL_INT)
            {
                query = query.Where(x => x.pet.Idasistente == _filter.IdAsistente);
            }
            if (_filter.IdEvento != Variables.NULL_INT)
            {
                query = query.Where(x => x.pet.Idevento == _filter.IdEvento);
            }
            if (_filter.IdExpediente != Variables.NULL_INT)
            {
                query = query.Where(x => x.pet.Idexpediente == _filter.IdExpediente);
            }
            if (_filter.IdPeticionGrupo != Variables.NULL_INT)
            {
                query = query.Where(x => x.pet.Idpeticiongrupo == _filter.IdPeticionGrupo);
            }

            var results =
                query.Skip((_filter.CurrentPageIndex.Value - 1)*Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<PeticionGruposDto>(x.pet)).ToList();

            int rowCount = query.Count();

            var result = new PeticionGruposResponseDto()
            {
                PeticionGruposList = results,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                IdExpediente = _filter.IdExpediente,
                IdAsistente = _filter.IdAsistente,
                IdPeticionGrupo = _filter.IdPeticionGrupo,
                IdEvento = _filter.IdEvento,
                PageCount = (int) Math.Ceiling(rowCount*1.0/Variables.ServicePageSize)
            };

            _logService.AddLogEntry("PeticionGrupos", JsonConvert.SerializeObject(_filter), rowCount.ToString(),
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

            var validateService = new ValidateService<PeticionGruposEditDto>();
            _data = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public ServiceError ValidDataDelete(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<PeticionGruposDeleteDto>();
            _dataDelete = validateService.Validate(suscriptor, data, ref result, _token);

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
                    int results = 0;
                    if (_data.Idpeticiongrupo != Variables.NULL_INT)
                    {
                        results = _session.Query<PeticionGrupos>()
                            .Join(_session.Query<Expediente>(), pet => new {IdExpediente = pet.Idexpediente.Value},
                                ex => new {IdExpediente = ex.Idxpediente},
                                (pet, ex) => new {pet.Idpeticiongrupo, ex.Idxpediente, ex.Idamec, pet})
                            .Join(_session.Query<Amec>(), amex => new {IdAmec = amex.Idamec},
                                am => new {IdAmec = am.Idamec},
                                (amex, am) => new {amex.Idxpediente, am.Idamec, am.Idconfempresa, amex.pet})
                            .Join(_session.Query<Confempresa>(), amexco => new {IdConf = amexco.Idconfempresa.Value},
                                co => new {IdConf = co.Idconfempresa},
                                (amexco, co) => new {amexco.Idxpediente, co.Idconfempresa, co.Idagencia, amexco.pet})
                            .Where(x => x.Idagencia == _agencyKey && x.pet.Idpeticiongrupo == _data.Idpeticiongrupo && x.Idxpediente == _data.Idexpediente).Count();
                    }
                    else
                    {
                        results = _session.Query<Expediente>()
                            .Join(_session.Query<Amec>(), amex => new { IdAmec = amex.Idamec },
                                am => new { IdAmec = am.Idamec },
                                (amex, am) => new { amex.Idxpediente, am.Idamec, am.Idconfempresa, amex })
                            .Join(_session.Query<Confempresa>(), amexco => new { IdConf = amexco.Idconfempresa.Value },
                                co => new { IdConf = co.Idconfempresa },
                                (amexco, co) => new { amexco.Idxpediente, co.Idconfempresa, co.Idagencia, amexco })
                            .Where(x => x.Idagencia == _agencyKey && x.Idxpediente == _data.Idexpediente).Count();
                    }

                    bool isAgencyService = ((results == 1 && _data.Idpeticiongrupo != Variables.NULL_INT) ||
                                            (results >= 1 && _data.Idpeticiongrupo == Variables.NULL_INT));

                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.PeticionGruposInsertUpdateError.GetHashCode(),
                            Description =
                                Utility.GetDisplayName(typeof(ErrorCode),
                                    ErrorCode.PeticionGruposInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("PeticionGrupos", JsonConvert.SerializeObject(_data),
                            "Error en PeticionGrupos.Edit: El idpeticiongrupo no pertenece a la agencia que está usando el servicio",
                            _agencyKey);
                        transaction.Commit();
                        return result;
                    }
                    /*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();
                    if (string.IsNullOrWhiteSpace(_data.FechaInicioEvento))
                    {
                        _data.FechaInicioEvento = null;
                    }

                    if (string.IsNullOrWhiteSpace(_data.FechaFinEvento))
                    {
                        _data.FechaFinEvento = null;
                    }
                    if (string.IsNullOrWhiteSpace(_data.FechaPeticion))
                    {
                        _data.FechaPeticion = null;
                    }

                    if (string.IsNullOrWhiteSpace(_data.FechaExpediente))
                    {
                        _data.FechaExpediente = null;
                    }
                    if (string.IsNullOrWhiteSpace(_data.UltimaActualizacion))
                    {
                        _data.UltimaActualizacion = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    }

                    if (string.IsNullOrWhiteSpace(_data.FechaInicioEvento))
                    {
                        _data.FechaInicioEvento = null;
                    }

                    if (string.IsNullOrWhiteSpace(_data.FechaFinEvento))
                    {
                        _data.FechaFinEvento = null;
                    }

                    var editData = Mapper.Map<PeticionGrupos>(_data);

                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idpeticiongrupo == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<PeticionGrupos>()
                            .Max(x => x.Idpeticiongrupo);
                        editData.Idpeticiongrupo = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        //_session.Evict(results[0].pet);
                        _session.Flush();
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("PeticionGrupos", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new PeticionGruposEditResponseDto()
                    {
                        PeticionGrupos = _data,
                        PeticionGruposResult = Mapper.Map<PeticionGruposEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en PeticionGruposService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.PeticionGruposInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.PeticionGruposInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("PeticionGrupos", JsonConvert.SerializeObject(_data),
                        "Error en PeticionGruposService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }
        }

        public SrvResponse Delete(QSuscriptor suscriptor)
        {
            var result = new SrvResponse()
            {
                ErrorList = new List<ErrorItem>()
            };

            /*ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

            if (_dataDelete.ListIdpeticiongrupo == null || _dataDelete.ListIdpeticiongrupo.Count == 0)
            {
                result.ErrorList.Add(new ErrorItem()
                {
                    Code = ErrorCode.PeticionGruposInsertUpdateError.GetHashCode(),
                    Description =
                        Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.PeticionGruposInsertUpdateError.ToString())
                });

                //Generar el log en la tabla de auditoria
                _logService.AddLogEntry("PeticionGrupos", JsonConvert.SerializeObject(_dataDelete),
                    "Error en PeticionGruposService.Delete: No se han recibido ListIdpeticiongrupo a borrar", _agencyKey);
                return result;
            }

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new PeticionGruposDeleteResponseDto()
                    {
                        PeticionGrupos = _dataDelete,
                        PeticionGruposResult = new PeticionGruposDeleteDto()
                        {
                            ListIdpeticiongrupo = new List<int>()
                        }
                    };

                    foreach (var peticiongrupo in _dataDelete.ListIdpeticiongrupo)
                    {

                        /*ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/
                        var query = _session.Query<PeticionGrupos>()
                            .Join(_session.Query<Expediente>(), pet => new {IdExpediente = pet.Idexpediente.Value},
                                ex => new {IdExpediente = ex.Idxpediente},
                                (pet, ex) => new {pet.Idpeticiongrupo, ex.Idxpediente, ex.Idamec, pet})
                            .Join(_session.Query<Amec>(), amex => new {IdAmec = amex.Idamec},
                                am => new {IdAmec = am.Idamec},
                                (amex, am) => new {amex.Idxpediente, am.Idamec, am.Idconfempresa, amex.pet})
                            .Join(_session.Query<Confempresa>(), amexco => new {IdConf = amexco.Idconfempresa.Value},
                                co => new {IdConf = co.Idconfempresa},
                                (amexco, co) => new {amexco.Idxpediente, co.Idconfempresa, co.Idagencia, amexco.pet})
                            .Where(x => x.Idagencia == _agencyKey && x.pet.Idpeticiongrupo == peticiongrupo);

                        var results = query.ToList();
                        bool isAgencyService = results.Count == 1;

                        _session.Flush();
                        if (!isAgencyService)
                        {
                            result.ErrorList.Add(new ErrorItem()
                            {
                                Code = ErrorCode.PeticionGruposInsertUpdateError.GetHashCode(),
                                Description =
                                    Utility.GetDisplayName(typeof(ErrorCode),
                                        ErrorCode.PeticionGruposInsertUpdateError.ToString()) +
                                    " idpeticiongrupo no borrado: " + peticiongrupo.ToString()
                            });

                            //Generar el log en la tabla de auditoria
                            _logService.AddLogEntry("PeticionGrupos_Delete", JsonConvert.SerializeObject(_dataDelete),
                                "Error en PeticionGrupos.Delete: El idpeticiongrupo no pertenece a la agencia que está usando el servicio: " + peticiongrupo,
                                _agencyKey);
                        }
                        else
                        {
                            var editData = results[0].pet;
                            _session.Evict(results[0].pet);
                            _session.Flush();
                            _session.Delete(editData);

                            _logService.AddLogEntry("PeticionGrupos_Delete", JsonConvert.SerializeObject(_dataDelete),
                                JsonConvert.SerializeObject(peticiongrupo), _agencyKey);
                            response.PeticionGruposResult.ListIdpeticiongrupo.Add(peticiongrupo);
                        }
                    }

                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en PeticionGruposService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.PeticionGruposInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.PeticionGruposInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("PeticionGrupos", JsonConvert.SerializeObject(_dataDelete),
                        "Error en PeticionGruposService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }
        }
    }
}
