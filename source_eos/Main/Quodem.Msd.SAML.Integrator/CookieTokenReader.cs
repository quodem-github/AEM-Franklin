using Newtonsoft.Json;
using Quodem.Msd.SAML.Integrator.Dto;
using System.Web;

namespace Quodem.Msd.SAML.Integrator
{
    public class CookieTokenReader : ITokenReader
    {
        public TokenValues GetTokenValues()
        {
            var cookToken = HttpContext.Current.Request.Cookies["profile_SAML_MSD"];

            if (cookToken == null) return null;

            var tokenValues =
                JsonConvert.DeserializeObject<TokenValues>(
                    System.Web.HttpContext.Current.Server.UrlDecode(cookToken.Value));
            tokenValues.Token = tokenValues.Token.Replace(" ", "+");
            return tokenValues;
        }
    }
}
