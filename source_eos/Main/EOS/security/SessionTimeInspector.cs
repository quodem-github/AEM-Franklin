using System;
using System.Linq;
using EOS.ServiceLogic.BLL.Token;

namespace EOS.security
{
    public class SessionTimeInspector : ISessionInspector
    {
        private readonly IServiceSuscriptorService _serviceSuscriptorService = new ServiceSubscriptionServiceImpl();

        public bool IsValid(System.Net.Http.HttpRequestMessage request, out string errorMessage)
        {
            var isValid = false;
            errorMessage = string.Empty;

            const string tokenName = "X-Token";
            if (request.Headers.Contains(tokenName))
            {
                var token = request.Headers.GetValues(tokenName).First();
                try
                {
                    isValid = _serviceSuscriptorService.IsValidHashedKey(token);
                    if (!isValid)
                    {
                        errorMessage = "Token de autorización no válido";
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }
            }
            else
            {
                errorMessage = "La solicitud web no contiene de un token de autorización en su cabecera";
            }

            if (!string.IsNullOrWhiteSpace(errorMessage) || !isValid)
            {
                Quodem.Monitor.Alerta.WriteLog("Time inspector validation failed: " + errorMessage);
            }

            return isValid;
        }
    }
}