using EOS.ServiceLogic.Data.ServiceResponse;

namespace EOS.security
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using EOS.Web.Extensions;
    using EOS.ServiceLogic.Enums;

    public class SessionHandler : DelegatingHandler
    {
        #region Attributes

        private readonly List<ISessionInspector> _sessionInspectorsList = null;

        #endregion

        #region Contructors

        public SessionHandler(List<ISessionInspector> sessionInspectorsList)
        {
            _sessionInspectorsList = sessionInspectorsList;
        }

        #endregion

        #region Methods


        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (this._sessionInspectorsList.IsNullOrEmptyCollection()) { return base.SendAsync(request, cancellationToken); }

            foreach (var sessionInspector in this._sessionInspectorsList)
            {
                string errorMessage;
                var idValid = sessionInspector.IsValid(request, out errorMessage);
                if (!idValid)
                {
                    var errorObject = new SrvResponse();
                    errorObject.AddError(ErrorCode.BadToken, errorMessage);

                    return Task.Factory.StartNew(
                            () =>
                            {
                                var response = request.CreateResponse(HttpStatusCode.Unauthorized, errorObject);
                                return response;
                            },
                            cancellationToken);
                }
            }

            return base.SendAsync(request, cancellationToken);
        }

        #endregion
    }
}