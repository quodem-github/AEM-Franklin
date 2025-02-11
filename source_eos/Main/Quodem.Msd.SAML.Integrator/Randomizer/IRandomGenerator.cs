using Quodem.Msd.SAML.Integrator.Enums;

namespace Quodem.Msd.SAML.Integrator.Randomizer
{
    interface IRandomGenerator
    {
        bool IsMatch(ERandomTypes randomType);
        char GenerateRandomElement();
    }
}
