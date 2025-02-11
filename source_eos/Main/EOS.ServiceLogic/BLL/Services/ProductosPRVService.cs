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
    public class ProductosPRVService : IService
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

        private FilterProductosPRV _filter;
        private ProductosPRVEditDto _data;
        private readonly string _token;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public ProductosPRVService(string token, string agencyKey)
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

            var validateService = new ValidateService<FilterProductosPRV>();
            _filter = validateService.Validate(suscriptor, data, ref result, _token);

            return result;
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var query = _session.Query<ProductosPrv>()
               .Join(_session.Query<Proveedores>(), prod => new { IdProveedor = prod.Fkidproveedor }, prov => new { IdProveedor = prov.Idproveedor }, (prod, prov) => new {prod, prov})
               .Join(_session.Query<Confempresa>(), pr => new { IdConf = pr.prov.Idconfempresa ?? Variables.NULL_INT }, conf => new { IdConf = conf.Idconfempresa }, (pr, conf) => new { pr, conf })
               .Where(x => x.conf.Idagencia == _agencyKey);


            if (_filter.Idproductoprv != Variables.NULL_INT)
            {
                query = query.Where(x => x.pr.prod.Idproductoprv == _filter.Idproductoprv);
            }

            if (_filter.Fkidproveedor != Variables.NULL_INT)
            {
                query = query.Where(x => x.pr.prod.Fkidproveedor == _filter.Fkidproveedor);
            }

            if (!string.IsNullOrEmpty(_filter.Idproducto))
            {
                query = query.Where(x => x.pr.prod.Idproducto == _filter.Idproducto);
            }

            if (!string.IsNullOrEmpty(_filter.Desproducto))
            {
                query = query.Where(x => x.pr.prod.Desproducto.ToLower().Contains(_filter.Desproducto.ToLower()));
            }


            var results =
                query.Skip((_filter.CurrentPageIndex.Value - 1) * Variables.ServicePageSize)
                    .Take(Variables.ServicePageSize)
                    .Select(x => Mapper.Map<ProductosPRVDto>(x.pr.prod)).ToList();

            int rowCount = query.Count();

            var result = new ProductosPRVResponseDto()
            {
                ProductosPrvList = results,
                CurrentPageIndex = _filter.CurrentPageIndex,
                PageSize = Variables.ServicePageSize,
                Idproductoprv = _filter.Idproductoprv,
                Idproducto = _filter.Idproducto,
                Fkidproveedor = _filter.Fkidproveedor,
                Desproducto = _filter.Desproducto,
                PageCount = (int)Math.Ceiling(rowCount * 1.0 / Variables.ServicePageSize)
            };

            _logService.AddLogEntry("ProductosPRV", JsonConvert.SerializeObject(_filter), rowCount.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<ProductosPRVEditDto>();
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
                    var query = _session.Query<ProductosPrv>()
                       .Join(_session.Query<Proveedores>(), prod => new { IdProveedor = prod.Fkidproveedor },
                           prov => new { IdProveedor = prov.Idproveedor }, (prod, prov) => new { prod, prov })
                       .Join(_session.Query<Confempresa>(), aex => new { IdConf = aex.prov.Idconfempresa ?? Variables.NULL_INT },
                           conf => new { IdConf = conf.Idconfempresa }, (aex, conf) => new { aex, conf })
                       .Where(x => x.conf.Idagencia == _agencyKey);

                    if (_data.Idproductoprv != Variables.NULL_INT)
                    {
                        query = query.Where(x => x.aex.prod.Idproductoprv == _data.Idproductoprv);
                    }

                    var results = query.ToList();
                    bool isAgencyService = results.Count >= 1;


                    if (!isAgencyService)
                    {
                        result.ErrorList.Add(new ErrorItem()
                        {
                            Code = ErrorCode.ProductosPRVInsertUpdateError.GetHashCode(),
                            Description = Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.ProductosPRVInsertUpdateError.ToString())
                        });

                        //Generar el log en la tabla de auditoria
                        _logService.AddLogEntry("ProductosPRV", JsonConvert.SerializeObject(_data), "Error en ProductosPRVService.Edit: El Idproductoprv no pertenece a la agencia que está usando el servicio", _agencyKey);
                        transaction.Commit();
                        return result;
                    }
                    /*FIN DE ZONA DE VALIDACIÓN DE SERVICIO PERTENECIENTE A AGENCIA*/

                    var editData = Mapper.Map<ProductosPrv>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    if (editData.Idproductoprv == Variables.NULL_INT)
                    {

                        //Obtener el ultimo ID para realizar la inserción en los casos que no tenga autonumerico la tabla destino.
                        var lastId = _session.Query<ProductosPrv>()
                            .Max(x => x.Idproductoprv);
                        editData.Idproductoprv = lastId + 1;
                        _session.Save(editData);
                    }
                    else
                    {
                        _session.Evict(results[0].aex.prod);
                        _session.Flush();
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    }

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("ProductosPRV", JsonConvert.SerializeObject(_data),
                        JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new ProductosPRVEditResponseDto()
                    {
                        ProductosPrv = _data,
                        ProductosPrvResult = Mapper.Map<ProductosPRVEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en ProductosPRVService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.ProductosPRVInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.ProductosPRVInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("ProductosPRV", JsonConvert.SerializeObject(_data),
                        "Error en ProductosPRVService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }

    }
}
