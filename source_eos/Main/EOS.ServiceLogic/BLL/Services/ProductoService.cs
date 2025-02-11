using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using EOS.ServiceLogic.BLL.Validator;
using EOS.ServiceLogic.Data.DTO.Service;
using EOS.ServiceLogic.Data.DTO.ServiceEdition;
using EOS.ServiceLogic.Data.ServiceResponse;
using EOS.ServiceLogic.Enums;
using EOS.ServiceModel;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Linq;

namespace EOS.ServiceLogic.BLL.Services
{
    public class ProductoService : IService
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
        private readonly string _token;
        private ProductoEditDto _data;
        private readonly string _agencyKey;
        private QuodemLogService _logService;
        #endregion

        public ProductoService(string token, string agencyKey)
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
            _token = token;
            _agencyKey = agencyKey;
            _logService = new QuodemLogService(_session);
        }

        public ServiceError ValidFilter(QSuscriptor suscriptor, string data)
        {
            throw new NotImplementedException();
        }

        public string GetList(QSuscriptor suscriptor)
        {
            var results = _session.Query<Productos>().Select(x => Mapper.Map<ProductoDto>(x)).ToList();
            var result = new ProductoResponseDto()
            {
                ProductosList = results
            };

            _logService.AddLogEntry("Producto", JsonConvert.SerializeObject(_data), results.Count.ToString(), _agencyKey, false);

            return Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(result), _token);
        }

        public ServiceError ValidData(QSuscriptor suscriptor, string data)
        {
            var result = new ServiceError()
            {
                IsError = false,
                ErrorList = new List<ServiceErrorItem>()
            };

            var validateService = new ValidateService<ProductoEditDto>();
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

                    var editData = Mapper.Map<Productos>(_data);
                    //Si el Id es null o -1 entonces se inserta, si no, se actualiza.
                    
                        //Se actualiza con el objeto que se recibe.
                        _session.SaveOrUpdate(editData);
                    

                    //Generar el log en la tabla de auditoria
                    _logService.AddLogEntry("Productos", JsonConvert.SerializeObject(_data),
                    JsonConvert.SerializeObject(editData), _agencyKey);

                    //Generar la respuesta cuando se ha actualizado o insertado correctamente.
                    var response = new ProductoEditResponseDto()
                    {
                        Producto = _data,
                        ProductoResult = Mapper.Map<ProductoEditDto>(editData)
                    };
                    //Encriptar el resultado y añadirlo a la respuesta.
                    result.Response = Utility.Encrypt3DES(suscriptor, JsonConvert.SerializeObject(response), _token);

                    transaction.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    //Si se produce algun error durante al inserción o actualziación se genera un email de error y se devuelve el error.
                    Quodem.Monitor.Alerta.SendNotificacion("Error en ProductoService.Edit: " + e.Message);
                    result.ErrorList.Add(new ErrorItem()
                    {
                        Code = ErrorCode.ProductosInsertUpdateError.GetHashCode(),
                        Description =
                            Utility.GetDisplayName(typeof(ErrorCode),
                                ErrorCode.ProductosInsertUpdateError.ToString())
                    });
                    transaction.Rollback();
                    _logService.AddLogEntry("Productos", JsonConvert.SerializeObject(_data),
                        "Error en ProductoService.Edit: " + e.Message, _agencyKey);

                    return result;
                }
            }

        }
    }
}
