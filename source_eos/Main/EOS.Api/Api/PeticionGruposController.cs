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
    public class PeticionGruposController : ApiController
    {


        private IServiceExtended _service;
        private readonly AgencyKeyService _agencyKeyService;

        public PeticionGruposController()
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
                //Compruebo que el filtro no sea nulo
                if (encryptedFilter == null || string.IsNullOrEmpty(encryptedFilter.Data))
                {
                    result.AddError(ErrorCode.NullFilter, Utility.GetDisplayName(typeof(ErrorCode),
                        ErrorCode.NullFilter.ToString()));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

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
                _service = new PeticionGruposService(token, agencyKey);

                var filterValidateResult = _service.ValidFilter(EOS.Api.WebApiApplication.Suscriptor, encryptedFilter.Data);
                if (filterValidateResult.IsError)
                {
                    foreach (var error in filterValidateResult.ErrorList)
                    {
                        result.AddError(error.Code, error.Description);
                    }

                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }
                result.Response = _service.GetList(EOS.Api.WebApiApplication.Suscriptor);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog("Error servicio PeticionGruposController.GetList " + ex.Message + " - " + ex.StackTrace);
                result.AddError(ErrorCode.DataNotValid,
                    Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.DataNotValid.ToString()));

                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpPost]
        [ActionName("Edit")]
        public HttpResponseMessage Edit([FromBody] ReceivedData encryptedFilter)
        {
            var result = new SrvResponse();

            try
            {
                //Compruebo que el filtro no sea nulo
                if (encryptedFilter == null || string.IsNullOrEmpty(encryptedFilter.Data))
                {
                    result.AddError(ErrorCode.NullFilter, Utility.GetDisplayName(typeof(ErrorCode),
                        ErrorCode.NullFilter.ToString()));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

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
                _service = new PeticionGruposService(token, agencyKey);

                var filterValidateResult = _service.ValidData(EOS.Api.WebApiApplication.Suscriptor, encryptedFilter.Data);
                if (filterValidateResult.IsError)
                {
                    foreach (var error in filterValidateResult.ErrorList)
                    {
                        result.AddError(error.Code, error.Description);
                    }
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                return Request.CreateResponse(HttpStatusCode.OK, _service.Edit(EOS.Api.WebApiApplication.Suscriptor));
            }
            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog("Error servicio PeticionGruposController.Edit " + ex.Message + " - " + ex.StackTrace);
                result.AddError(ErrorCode.DataNotValid,
                    Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.DataNotValid.ToString()));

                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public HttpResponseMessage Delete([FromBody] ReceivedData encryptedFilter)
        {
            var result = new SrvResponse();

            try
            {
                //Compruebo que el filtro no sea nulo
                if (encryptedFilter == null || string.IsNullOrEmpty(encryptedFilter.Data))
                {
                    result.AddError(ErrorCode.NullFilter, Utility.GetDisplayName(typeof(ErrorCode),
                        ErrorCode.NullFilter.ToString()));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

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
                _service = new PeticionGruposService(token, agencyKey);

                var filterValidateResult = _service.ValidDataDelete(EOS.Api.WebApiApplication.Suscriptor, encryptedFilter.Data);
                if (filterValidateResult.IsError)
                {
                    foreach (var error in filterValidateResult.ErrorList)
                    {
                        result.AddError(error.Code, error.Description);
                    }
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                return Request.CreateResponse(HttpStatusCode.OK, _service.Delete(EOS.Api.WebApiApplication.Suscriptor));
            }
            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog("Error servicio PeticionGruposController.Delete " + ex.Message + " - " + ex.StackTrace);
                result.AddError(ErrorCode.DataNotValid,
                    Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.DataNotValid.ToString()));

                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
        }
    }
}