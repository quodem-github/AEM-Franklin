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
    public class PeticionariosEmpleadosGPService : IService
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

        private FilterPeticionariosEmpleadosGP _filter;
        private PeticionariosEmpleadosGPEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public PeticionariosEmpleadosGPService(string token, string agencyKey)
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


            var validateService = new ValidateService<FilterPeticionariosEmpleadosGP>();
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

            var validateService = new ValidateService<PeticionariosEmpleadosGPEditDto>();
            _data = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query = GetListByFiltersAndAgency();
            var petEmpleadosList =
                query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<PeticionariosEmpleadosGPDto>(x)).ToList();

            int rowCount = query.Count();

            var result = new PeticionariosEmpleadosGPResponseDto()
            {
                PeticionariosEmpleadosGPList = petEmpleadosList,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                id = _filter.id,
                idpeticionario = _filter.idpeticionario,
                idempleadogp = _filter.idempleadogp,
                locked = _filter.locked,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("PeticionariosEmpleadosGPList", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

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
                    var query = _session.Query<Empleadosgp>()
                        .Join(_session.Query<Confempresa>(),
                            emp => new { IdConfEmpresa = emp.Idconfempresa },
                            conf => new { IdConfEmpresa = conf.Idconfempresa },
                            (emp, conf) => new { emp, conf.Idagencia })
                        .Where(x => x.Idagencia == _agencyKey && x.emp.Id == _data.idempleadogp);

                    var results = query.ToList();
                    bool isAgencyService = results.Count >= 1;


                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.PeticionariosEmpleadosGpInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.PeticionariosEmpleadosGpInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("PeticionariosEmpleadosGP", JsonConvert.SerializeObject(_data), "Error en PeticionariosEmpleadosGP.Edit: El idempleado no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }
                    /*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();

                    var editData = Mapper.Map<PeticionariosEmpleadosgp>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Id == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<PeticionariosEmpleadosgp>()
                            .Max(x => x.Id);
                        editData.Id = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("PeticionariosEmpleadosGP", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new PeticionariosEmpleadosGPEditResponseDto()
                    {
                        PeticionariosEmpleadosGP = _data,
                        PeticionariosEmpleadosGPResult = Mapper.Map<PeticionariosEmpleadosGPEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en PeticionariosEmpleadosGP.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.PeticionariosEmpleadosGpInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.PeticionariosEmpleadosGpInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("PeticionariosEmpleadosGP", JsonConvert.SerializeObject(_data),
                        "Error en PeticionariosEmpleadosGP.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }

        public List<PeticionariosEmpleadosgp> GetListByFiltersAndAgency(bool edit = false)
        {
            var query = _session.Query<PeticionariosEmpleadosgp>()
                .Join(_session.Query<Empleadosgp>(),
                    pet => new { idempleadogp = pet.Idempleadogp },
                    emp => new { idempleadogp = emp.Id },
                    (pet, emp) => new { pet, emp.Idconfempresa })
                .Join(_session.Query<Confempresa>(),
                    empPet => new { IdConfEmpresa = empPet.Idconfempresa },
                    conf => new { IdConfEmpresa = conf.Idconfempresa },
                    (empPet, conf) => new { empPet.pet, conf.Idagencia })
                .Where(x => x.Idagencia == _agencyKey);

            if (edit)
                return query.Select(x => x.pet).Distinct().ToList();

            if (_filter.id.HasValue  && _filter.id != Variables.NULL_INT)
            {
                query = query.Where(x => x.pet.Id == _filter.id.Value);
            }
            if (_filter.idpeticionario.HasValue && _filter.idpeticionario != Variables.NULL_INT)
            {
                query = query.Where(x => x.pet.Idpeticionario == _filter.idpeticionario.Value);
            }
            if (_filter.idempleadogp.HasValue && _filter.idempleadogp != Variables.NULL_INT)
            {
                query = query.Where(x => x.pet.Idempleadogp == _filter.idempleadogp.Value);
            }
            if (_filter.locked.HasValue && _filter.locked != Variables.NULL_INT)
            {
                query = query.Where(x => x.pet.Locked == _filter.locked.Value);
            }

            return query.Select(x => x.pet).Distinct().ToList();
        }
    }
}
