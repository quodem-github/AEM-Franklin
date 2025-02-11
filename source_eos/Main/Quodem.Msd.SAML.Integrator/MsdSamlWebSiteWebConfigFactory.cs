using Quodem.Msd.SAML.Integrator.Dto;
using Quodem.Msd.SAML.Integrator.Utilities;

namespace Quodem.Msd.SAML.Integrator
{
    class MsdSamlWebSiteWebConfigFactory : ISamlConfigFactory
    {
        public SamlConfigValues GetSamlConfiguration()
        {
            return GlobalVariables.MsdSamlWebSiteWebConfigSettings;

        }
    }
}
