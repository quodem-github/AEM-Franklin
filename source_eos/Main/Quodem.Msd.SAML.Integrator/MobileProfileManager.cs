using Quodem.Msd.SAML.Integrator.Dto;
using RestSharp;

namespace Quodem.Msd.SAML.Integrator
{
    public class MobileProfileManager : ProfileManager, IProfileManager
    {
        public MobileProfileManager(ITokenReader tokenReader = null, ISamlConfigFactory configBuilder = null, RestClient middlewareAuthority = null, RestRequest claimsApi = null)
        {
            TokenValues = tokenReader != null ? tokenReader.GetTokenValues() : new CookieTokenReader().GetTokenValues();
            SamlConfigValues = configBuilder != null ? configBuilder.GetSamlConfiguration() : new MsdSamlMobileWebConfigFactory().GetSamlConfiguration();
            MiddlewareAuthority = middlewareAuthority ?? new RestClient(SamlConfigValues.MiddlewareAuthority);
            ClaimsApi = claimsApi ?? new RestRequest("/api/MsdSamlAuth/Token", Method.GET);

        }
        public new dynamic GetUserProfile()
        {
            return base.GetUserProfile();
        }

        public new bool IsTokenValid()
        {
            return base.IsTokenValid();
        }
        public void SignIn()
        {
            SignIn(SamlConfigValues.AppName);
        }
        public new void SignOut(string returnUrl, bool forcePlattform = false)
        {
            base.SignOut(returnUrl, forcePlattform);
        }
        public SignInHashKeyValues CreateSignInHashKey()
        {
            return CreateSignInHashKey(SamlConfigValues.AppName);
        }
    }
}
