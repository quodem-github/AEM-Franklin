using Quodem.Msd.SAML.Integrator.Dto;

namespace Quodem.Msd.SAML.Integrator
{
    public interface ISamlConfigFactory
    {
        SamlConfigValues GetSamlConfiguration();

    }
}
