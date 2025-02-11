using System.Configuration;
using Quodem.Msd.SAML.Integrator.Dto;

namespace Quodem.Msd.SAML.Integrator.Utilities
{
    public static class GlobalVariables
    {
        public static SamlConfigValues MsdSamlWebSiteWebConfigSettings
        {
            get
            {
                return new SamlConfigValues()
                {
                    MiddlewareAuthority = ConfigurationManager.AppSettings["Quodem.MiddlewareAuthority"],
                    AppName = ConfigurationManager.AppSettings["Quodem.AppName"],
                    SamlKey = Utils.ConvertFromBase64(ConfigurationManager.AppSettings["Quodem.SamlKey"]),
                    SamlClaimsKey = Utils.ConvertFromBase64(ConfigurationManager.AppSettings["Quodem.SamlClaimsKey"]),
                    SamlIv = Utils.ConvertFromBase64(ConfigurationManager.AppSettings["Quodem.SamlIV"])
                };
            }
        }

        public static SamlConfigValues MsdSamlMobileWebConfigSettings
        {
            get
            {
                return new SamlConfigValues()
                {
                    MiddlewareAuthority = ConfigurationManager.AppSettings["Quodem.MiddlewareAuthority"],
                    AppName = ConfigurationManager.AppSettings["Quodem.MobileAppName"],
                    SamlKey = Utils.ConvertFromBase64(ConfigurationManager.AppSettings["Quodem.SamlKey"]),
                    SamlClaimsKey = Utils.ConvertFromBase64(ConfigurationManager.AppSettings["Quodem.SamlClaimsKey"]),
                    SamlIv = Utils.ConvertFromBase64(ConfigurationManager.AppSettings["Quodem.SamlIV"])
                };
            }
        }
    }
}
