// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CorsCustomHandler.cs" company="Quodem consultores S.L.">
//   Copyright Quodem Consultores S.L.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------



namespace EOS.Api
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Pre-flight custom handler.
    /// </summary>
    public class CorsCustomHandler : DelegatingHandler
    {
        /// <summary>Origin private constant</summary>
        private const string Origin = "Origin";

        #region pre-flight security headers

        /// <summary>Access control request method header.</summary>
        private const string AccessControlRequestMethod = "Access-Control-Request-Method";

        /// <summary>The access control request headers.</summary>
        private const string AccessControlRequestHeaders = "Access-Control-Request-Headers";

        /// <summary>The access control allow origin header.</summary>
        private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";

        /// <summary>The access control allow methods header.</summary>
        private const string AccessControlAllowMethods = "Access-Control-Allow-Methods";

        /// <summary>The access control allow headers.</summary>
        private const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";

        #endregion

        /// <summary>The send async method.</summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="Task"/> list.</returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool isCorsRequest = request.Headers.Contains(Origin);
            bool isPreflightRequest = request.Method == HttpMethod.Options;
            if (isCorsRequest)
            {
                if (isPreflightRequest)
                {
                    return Task.Factory.StartNew(
                        () =>
                        {
                            var response = new HttpResponseMessage(HttpStatusCode.OK);
                            response.Headers.Add(
                                AccessControlAllowOrigin,
                                request.Headers.GetValues(Origin).First());

                            string accessControlRequestMethod =
                                request.Headers.GetValues(AccessControlRequestMethod).FirstOrDefault();
                            if (accessControlRequestMethod != null)
                            {
                                response.Headers.Add(AccessControlAllowMethods, accessControlRequestMethod);
                            }

                            string requestedHeaders = string.Join(
                                ", ",
                                request.Headers.GetValues(AccessControlRequestHeaders));
                            if (!string.IsNullOrEmpty(requestedHeaders))
                            {
                                response.Headers.Add(AccessControlAllowHeaders, requestedHeaders);
                            }

                            return response;
                        },
                        cancellationToken);
                }

                return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(t =>
                    {
                        HttpResponseMessage resp = t.Result;
                        /*if (Variables.AllowLocalCorsTesting)
                        {
                            resp.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
                        }*/
                        return resp;
                    });
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
