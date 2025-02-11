namespace Quodem.Msd.SAML.Integrator
{
    using Dto;
    public interface IProfileManager
    {
        dynamic GetUserProfile();
        bool IsTokenValid();
        void SignIn();
        void SignOut(string returnUrl,  bool forcePlattform = false);
        SignInHashKeyValues CreateSignInHashKey();
    }
}
