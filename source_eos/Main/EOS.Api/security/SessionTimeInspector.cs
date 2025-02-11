using System;
using System.Linq;
using System.Threading;
using EOS.Api.security;
using EOS.ServiceLogic.BLL.Token;
using System.Collections.Generic;
using EOS.ServiceModel;

namespace EOS.security
{
    public class SessionTimeInspector : ISessionInspector
    {
        private const string PUBLIC_KEY = "EosPubkey";
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
                    try
                    {
                        isValid = EOS.Api.WebApiApplication.LstTokens != null && EOS.Api.WebApiApplication.LstTokens.Find(x => x.Hashedkey == token) != null;
                    }
                    catch(Exception e)
                    {
                        EOS.Api.WebApiApplication.LstTokens = new List<QServiceQuery>();
                        Quodem.Monitor.Alerta.WriteLog("Error Validando token e: " + e.StackTrace);
                        isValid = false;
                    }
                    if (!isValid)
                    {
                        try
                        {
                            isValid = _serviceSuscriptorService.IsValidHashedKey(token);
                            if (isValid)
                            {
                                var tokenObj = _serviceSuscriptorService.GetAthorizationGuid(PUBLIC_KEY);
                                EOS.Api.WebApiApplication.LstTokens.Add(tokenObj);
                            }
                        }
                        catch (Exception ex1)
                        {
                            Quodem.Monitor.Alerta.WriteLog("Error Validando token ex1: " + ex1.StackTrace);
                            Thread.Sleep(250);
                            try
                            {
                                isValid = _serviceSuscriptorService.IsValidHashedKey(token);
                                if (isValid)
                                {
                                    var tokenObj = _serviceSuscriptorService.GetAthorizationGuid(PUBLIC_KEY);
                                    EOS.Api.WebApiApplication.LstTokens.Add(tokenObj);
                                }
                            }
                            catch (Exception ex2)
                            {
                                Quodem.Monitor.Alerta.WriteLog("Error Validando token ex2: " + ex2.StackTrace);
                                Thread.Sleep(350);
                                try
                                {
                                    isValid = _serviceSuscriptorService.IsValidHashedKey(token);
                                    if (isValid)
                                    {
                                        var tokenObj = _serviceSuscriptorService.GetAthorizationGuid(PUBLIC_KEY);
                                        EOS.Api.WebApiApplication.LstTokens.Add(tokenObj);
                                    }
                                }
                                catch (Exception ex3)
                                {
                                    Quodem.Monitor.Alerta.WriteLog("Error Validando token ex3: " + ex3.StackTrace);
                                    Thread.Sleep(500);
                                    try
                                    {
                                        isValid = _serviceSuscriptorService.IsValidHashedKey(token);
                                        if (isValid)
                                        {
                                            var tokenObj = _serviceSuscriptorService.GetAthorizationGuid(PUBLIC_KEY);
                                            EOS.Api.WebApiApplication.LstTokens.Add(tokenObj);
                                        }
                                    }
                                    catch (Exception ex4)
                                    {
                                        Quodem.Monitor.Alerta.WriteLog("Error Validando token ex4: " + ex4.StackTrace);
                                        Thread.Sleep(750);
                                        isValid = _serviceSuscriptorService.IsValidHashedKey(token);
                                    }
                                }
                            }
                        }
                        if (!isValid)
                        {
                            errorMessage = "Token de autorización no válido";
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message + " " + ex.StackTrace;
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