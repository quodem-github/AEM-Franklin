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
    public class ProveedorService : IService
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

        private FilterProveedor _filter;
        private ProveedorEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public ProveedorService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterProveedor>();
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }


        public string GetList(QSuscriptor suscriptor)
        {
            var query = _session.Query<Proveedores>().Join(_session.Query<Confempresa>(), prov => new { IdConf = prov.Idconfempresa ?? Variables.NULL_INT },
                        conf => new { IdConf = conf.Idconfempresa }, (prov, conf) => new { prov, conf.Idagencia })
                    .Where(x => x.Idagencia == _agencyKey);

            if (_filter.IdProveedor != Variables.NULL_INT)
            {
                query = query.Where(x => x.prov.Idproveedor == _filter.IdProveedor.Value);
            }
            if (!string.IsNullOrEmpty(_filter.Proveedor))
            {
                query = query.Where(x => x.prov.Proveedor.ToLower().Contains(_filter.Proveedor.ToLower()));
            }

            var proveedores =
                query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<ProveedorDto>(x.prov)).ToList();

            int rowCount = query.Count();

            var result = new ProveedorResponseDto()
            {
                ProveedorList = proveedores,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize),
                IdProveedor = _filter.IdProveedor,
                Proveedor = _filter.Proveedor
            };

            _logService.AddLogEntry("Proveedor", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<ProveedorEditDto>();
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
                    var query = _session.Query<Proveedores>()
                    .Join(_session.Query<Confempresa>(), prov => new { IdConf = prov.Idconfempresa ?? Variables.NULL_INT },
                        conf => new { IdConf = conf.Idconfempresa }, (prov, conf) => new { prov, conf.Idagencia })
                    .Where(x => x.Idagencia == _agencyKey);

                    if (_data.IdProveedor != Variables.NULL_INT)
                    {
                        query = query.Where(x => x.prov.Idproveedor == _data.IdProveedor);
                    }

                    var results = query.ToList();
                    bool isAgencyService = results.Count == 1;

                    if (!isAgencyService && _data.IdProveedor != Variables.NULL_INT)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.ProveedorInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.ProveedorInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("Proveedores", JsonConvert.SerializeObject(_data), "Error en ProveedorService.Edit: El IdProveedor no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }
                    /*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    _session.Flush();

                    var editData = Mapper.Map<Proveedores>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idproveedor == Variables.NULL_INT)
                    {
                        editData.Idconfempresa = _session.Query<Confempresa>().First(x=>x.Idagencia == _agencyKey).Idconfempresa;
                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<Proveedores>()
                            .Max(x => x.Idproveedor);
                        editData.Idproveedor = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        _session.Evict(results[0].prov);
                        _session.Flush();
                        editData.Idconfempresa = _session.Query<Confempresa>().First(x => x.Idagencia == _agencyKey).Idconfempresa;
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("Proveedores", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new ProveedorEditResponseDto()
                    {
                        Proveedor = _data,
                        ProveedorResult = Mapper.Map<ProveedorEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en ProveedorService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.ProveedorInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.ProveedorInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("Proveedores", JsonConvert.SerializeObject(_data),
                        "Error en ProveedorService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }
    }
}
