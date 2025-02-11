using EOS.security;
using EOS.ServiceLogic;
using EOS.ServiceLogic.BLL.Services;
using EOS.ServiceLogic.Data;
using EOS.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using EOS.ServiceLogic.Data.ServiceResponse;
using EOS.ServiceLogic.Enums;

namespace EOS.Api
{
    public class PassengersListController : ApiController
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

                PassengersService service = new PassengersService();

                List<PassengersService.ResultType> validationResult = service.Validate(EOS.Api.WebApiApplication.Suscriptor, encryptedFilter, token);

                if (validationResult.Contains(PassengersService.ResultType.DateFormatError))
                {
                    result.AddError(ErrorCode.DateFormatError, Utility.GetDisplayName(typeof(ErrorCode),
                        ErrorCode.DateFormatError.ToString()));
                }
                if (validationResult.Contains(PassengersService.ResultType.PageError))
                {
                    result.AddError(ErrorCode.PageError, Utility.GetDisplayName(typeof(ErrorCode),
                            ErrorCode.PageError.ToString()));
                }
                if (validationResult.Contains(PassengersService.ResultType.UnexpectedError))
                {
                    result.AddError(ErrorCode.UnexpectedError, Utility.GetDisplayName(typeof(ErrorCode),
                            ErrorCode.UnexpectedError.ToString()));
                }
                if (validationResult.Contains(PassengersService.ResultType.DateFromBiggerThanDateTo)) 
                {
                    result.AddError(ErrorCode.DateFromBiggerThanDateTo, Utility.GetDisplayName(typeof(ErrorCode),
                            ErrorCode.DateFromBiggerThanDateTo.ToString()));
                }

                if (result.ErrorList.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, result);
                }

                result.Response = service.GetList(EOS.Api.WebApiApplication.Suscriptor, encryptedFilter, token);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Quodem.Monitor.Alerta.WriteLog("Error servicio PassengersListController.Post " + ex.Message + " - " + ex.StackTrace);
                result.AddError(ErrorCode.DataNotValid,
                    Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.DataNotValid.ToString()));

                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
        }
    }
}