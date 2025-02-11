using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using EOS.security;
using EOS.ServiceLogic;
using EOS.ServiceLogic.BLL;
using EOS.ServiceLogic.BLL.Services;
using EOS.ServiceLogic.Data;
using EOS.ServiceLogic.Data.ServiceResponse;
using EOS.ServiceLogic.Enums;

namespace EOS.Api
{
    public class JustificacionesDatosAdicionalesController : ApiController
    {


        private IServiceBase _service;
        private readonly AgencyKeyService _agencyKeyService;

        public JustificacionesDatosAdicionalesController()
        {
            _agencyKeyService = new AgencyKeyService();
        }

        [HttpPost]
        [ActionName("List")]
        public HttpResponseMessage GetList([FromBody] ReceivedData encryptedFilter)
        {
            var result = new SrvResponse();

            try
            {
             
                //Obtengo el Token y la agencia.
                var token = Request.Headers.GetValues(Variables.X_TOKEN).First();

                var agencyKey = _agencyKeyService.GetAgencyKey(Request.Headers.GetValues(Variables.AGENCY_KEY).First(), token, EOS.Api.WebApiApplication.LstConfEmpresas, EOS.Api.WebApiApplication.Suscriptor);
                if (string.IsNullOrEmpty(agencyKey))
                {
                    result.AddError(ErrorCode.BadAgencyKey, Utility.GetDisplayName(typeof(ErrorCode),
                        ErrorCode.BadAgencyKey.ToString()));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                //Creo el servicio con los datos de las cabeceras
                _service = new JustificacionesDatosAdicionalesService(token, agencyKey);
              
                result.Response = _service.GetList(EOS.Api.WebApiApplication.Suscriptor);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog("Error servicio JustificacionesDatosAdicionalesController.GetList " + ex.Message + " - " + ex.StackTrace);
                result.AddError(ErrorCode.DataNotValid,
                    Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.DataNotValid.ToString()));

                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
        }
    }
}