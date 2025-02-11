using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using EOS.security;
using EOS.ServiceLogic.BLL.Token;
using EOS.Web.Extensions;
using EOS.ServiceLogic.Data;
using EOS.ServiceLogic;
using EOS.ServiceLogic.BLL.Services;
using System.Collections.Generic;
using EOS.ServiceLogic.Data.ServiceResponse;
using EOS.ServiceLogic.Enums;

namespace EOS.Api
{

    public class PeticionariosListController : ApiController 
    {
        [HttpPost]
        public HttpResponseMessage Post([FromBody] ReceivedData encryptedFilter)
        {
            var result = new SrvResponse();

            try
            {
                if (encryptedFilter == null || string.IsNullOrEmpty(encryptedFilter.Data))
                {
                    result.AddError(ErrorCode.NullFilter, Utility.GetDisplayName(typeof(ErrorCode),
                        ErrorCode.NullFilter.ToString()));
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                var token = Request.Headers.GetValues(Variables.X_TOKEN).First();

                PeticionariosService service = new PeticionariosService();

                List<PeticionariosService.ResultType> validationResult = service.Validate(EOS.Api.WebApiApplication.Suscriptor, encryptedFilter, token);

                if (validationResult.Contains(PeticionariosService.ResultType.DateFormatError)) 
                {
                    result.AddError(ErrorCode.DateFormatError, Utility.GetDisplayName(typeof(ErrorCode),
                        ErrorCode.DateFormatError.ToString()));
                }
                if (validationResult.Contains(PeticionariosService.ResultType.PageError))
                {
                    result.AddError(ErrorCode.PageError, Utility.GetDisplayName(typeof(ErrorCode),
                            ErrorCode.PageError.ToString()));
                }
                if (validationResult.Contains(PeticionariosService.ResultType.UnexpectedError))
                {
                    result.AddError(ErrorCode.UnexpectedError, Utility.GetDisplayName(typeof(ErrorCode),
                            ErrorCode.UnexpectedError.ToString()));
                }
                if (validationResult.Contains(PeticionariosService.ResultType.DateFromBiggerThanDateTo))
                {
                    result.AddError(ErrorCode.DateFromBiggerThanDateTo, Utility.GetDisplayName(typeof(ErrorCode),
                            ErrorCode.DateFromBiggerThanDateTo.ToString()));
                }

                if(result.ErrorList.Count > 0)
                {       
                   return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                result.Response = service.GetList(EOS.Api.WebApiApplication.Suscriptor, encryptedFilter, token);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog("Error servicio PeticionariosListController.Post " + ex.Message + " - " + ex.StackTrace);
                result.AddError(ErrorCode.DataNotValid,
                    Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.DataNotValid.ToString()));

                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
        }
    }
}