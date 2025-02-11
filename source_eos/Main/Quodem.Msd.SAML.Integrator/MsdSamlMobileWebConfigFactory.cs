using Quodem.Msd.SAML.Integrator.Dto;
using Quodem.Msd.SAML.Integrator.Utilities;

namespace Quodem.Msd.SAML.Integrator
{
    class MsdSamlMobileWebConfigFactory : ISamlConfigFactory
    {
        public SamlConfigValues GetSamlConfiguration()
        {
            return GlobalVariables.MsdSamlMobileWebConfigSettings;

        }
    }
}
