using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using EOS.security;
using EOS.ServiceLogic;
using EOS.ServiceLogic.BLL.Token;
using EOS.ServiceLogic.Data.ServiceResponse;
using EOS.ServiceLogic.Enums;
using EOS.ServiceModel;

namespace EOS.Api
{
    public class IsTokenManagementController : ApiController
    {
        private readonly IServiceSuscriptorService _serviceSuscriptor = new ServiceSubscriptionServiceImpl();

        // RequestToken
        public HttpResponseMessage Get(string param)
        {
            var result = new SrvResponse();
            try
            {
                QServiceQuery tokenObj = null;
                if (EOS.Api.WebApiApplication.LstTokens.Count > 0)
                {
                    var lastToken = EOS.Api.WebApiApplication.LstTokens.Last();
                    var tokenDateValid = lastToken.Createdate.Value.AddMinutes(5);
                    var isValid = DateTime.Compare(tokenDateValid, DateTime.Now) > 0;
                    if (isValid)
                    {
                        tokenObj = lastToken;
                    }
                }
                if (tokenObj == null)
                {
                    tokenObj = _serviceSuscriptor.GetAthorizationGuid(param);
                    EOS.Api.WebApiApplication.LstTokens.Add(tokenObj);
                    if (EOS.Api.WebApiApplication.LstTokens.Count > 50)
                    {
                        EOS.Api.WebApiApplication.LstTokens.RemoveAt(0);
                    }
                }
                result.Response = tokenObj.Randonguid;
            }
            catch (Exception ex)
            {
                result.AddError(ErrorCode.ErrorGettingAccessToken, Utility.GetDisplayName(typeof(ErrorCode), ErrorCode.ErrorGettingAccessToken.ToString()));
            }


            return Request.CreateResponse(result.IsError ? HttpStatusCode.BadRequest : HttpStatusCode.OK, result);
        }
    }
}