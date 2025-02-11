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
using EOS.ServiceLogic.BLL.Token;
using EOS.ServiceLogic.Data;
using EOS.ServiceLogic.Data.ServiceResponse;
using EOS.ServiceLogic.Enums;
using EOS.ServiceModel;

namespace EOS.Api
{
    public class ServerDateTimeController : ApiController
    {
        // RequestToken
        public HttpResponseMessage Get(string param)
        {
            var result = new SrvResponse();
            try
            {
                result.Response = DateTime.Now.ToString(Variables.DATE_TIME_FORMAT);
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ErrorGettingDateTime, Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.ErrorGettingDateTime.ToString()));
            }

            return Request.CreateResponse(result.IsError ? HttpStatusCode.BadRequest : HttpStatusCode.OK, result);
        }
    }
}