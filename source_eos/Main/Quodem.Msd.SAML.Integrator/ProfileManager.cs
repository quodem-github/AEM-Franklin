using System;
using System.Dynamic;
using System.Runtime.Remoting.Messaging;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Quodem.Msd.SAML.Integrator.Dto;
using Quodem.Msd.SAML.Integrator.Encrypt;
using Quodem.Msd.SAML.Integrator.Service;
using Quodem.Msd.SAML.Integrator.Utilities;
using RestSharp;

namespace Quodem.Msd.SAML.Integrator
{
    public class ProfileManager : IDisposable
    {
        protected SamlConfigValues SamlConfigValues { get; set; }
        protected TokenValues TokenValues { get; set; }
        protected RestClient MiddlewareAuthority { get; set; }
        protected RestRequest ClaimsApi { get; set; }

        public void Dispose()
        {
            SamlConfigValues = null;
            TokenValues = null;
            MiddlewareAuthority = null;
            ClaimsApi = null;
        }

        protected bool IsTokenValid()
        {
            try
            {
                //If cookie not expired && Decrypts equals to SessionID + AppName
                //etq
                HttpContext.Current.Response.Write(DateTime.Now < DateTime.ParseExact(TokenValues.Expiration, "yyyy/MM/dd HH:mm", System.Globalization.CultureInfo.InvariantCulture) &&
                    RijndaelManagedEncryptor.DecryptRijndael(TokenValues.Token.Replace(" ", "+"),
                        SamlConfigValues.SamlKey, SamlConfigValues.SamlIv) ==
                    TokenValues.SessionId + SamlConfigValues.AppName);
                    if (DateTime.Now < DateTime.ParseExact(TokenValues.Expiration, "yyyy/MM/dd HH:mm", System.Globalization.CultureInfo.InvariantCulture) &&
                    RijndaelManagedEncryptor.DecryptRijndael(TokenValues.Token.Replace(" ", "+"),
                        SamlConfigValues.SamlKey, SamlConfigValues.SamlIv) ==
                    TokenValues.SessionId + SamlConfigValues.AppName)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        private dynamic DecryptUserProfile()
        {
            try
            {
                ClaimsApi.AddHeader("X-SessionID", TokenValues.SessionId);
                ClaimsApi.AddHeader("X-Token", TokenValues.Token.Replace(" ", "+"));
                ClaimsApi.AddHeader("X-SiteCode", SamlConfigValues.AppName);

                var response = MiddlewareAuthority.Execute(ClaimsApi);

                return RijndaelManagedEncryptor.DecryptRijndael(response.Content.TrimStart('"').TrimEnd('"'), SamlConfigValues.SamlClaimsKey, Utils.SecretGenerator(TokenValues.SessionId));
            }
            catch
            {
                return null;
                
            }
            
        }

        protected dynamic GetUserProfile()
        {
            try
            {
                if (TokenValues != null)
                {
                    if (IsTokenValid())
                    {
                        return JsonConvert.DeserializeObject<ExpandoObject[]>(DecryptUserProfile(),
                            new ExpandoObjectConverter());

                    }
                }
            }
            catch (Exception)
            {
                return null;
            }


            return null;
        }

        protected SignInHashKeyValues CreateSignInHashKey(string appName)
        {
            var instance = new ServiceRandomGenerator();

            var key = instance.GetRandomWord(8);
            return new SignInHashKeyValues()
            {
                Key = key,
                Hash = Sha1Generator.Minus3_GetHash(key + appName)
            };
        }

        protected void SignIn(string appName)
        {
            var signInHashKeys = CreateSignInHashKey(appName);
            HttpContext.Current.Response.Redirect(SamlConfigValues.MiddlewareAuthority + "/api/MsdSamlAuth/Login/?X-Token=" + signInHashKeys.Hash + "&X-Key=" + signInHashKeys.Key);
        
        }
        protected void SignOut(string returnUrl, bool forcePlattform = false)
        {
            if (forcePlattform)
            {
                //TODO implementar signOut de la plataforma Padre, por ejemplo SAML MSD
            }
            var cook = HttpContext.Current.Request.Cookies["profile_SAML_MSD"];
            if (cook == null) return;
            cook.Expires = DateTime.Now.AddDays(-1d);
            HttpContext.Current.Response.Cookies.Add(cook);
            HttpContext.Current.Response.Redirect(returnUrl);
        }

    }
}


