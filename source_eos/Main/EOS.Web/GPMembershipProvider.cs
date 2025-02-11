using System;
using System.Web.Security;
using EOS.Logica;
using EOS.Entidades.Datos;

namespace EOS.Web
{
    public class GPMembershipProvider : MembershipProvider
    {

        public ILogicaUsuarios MiLogicaUsuarios { get; set; }

        public string m_applicationName;

        public override string ApplicationName
        {
            get
            {
                return m_applicationName;
            }
            set
            {
                m_applicationName = value;
            }
        }

        public GPMembershipProvider()
        {
            MiLogicaUsuarios = new LogicaUsuarios();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            bool bCambiado = false;
            string sOldPasswordHashed = FormsAuthentication.HashPasswordForStoringInConfigFile(oldPassword, "sha1");
            bool bValidaUsuario = MiLogicaUsuarios.ValidarUsuario(username, oldPassword, sOldPasswordHashed);

            if (bValidaUsuario)
            {
                bCambiado = MiLogicaUsuarios.CambiarPassword(username, FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, "sha1"));
            }

            return bCambiado;            
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { return true; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            DDatosPersonalesUsuario datosPersonales = MiLogicaUsuarios.ObtenerDatosPersonalesPorLogin(username);
            MembershipUser usuario = null;

            if (datosPersonales != null)
            {
               usuario = new MembershipUser(
                    Name,
                    datosPersonales.login,
                    null,
                    datosPersonales.Email,
                    null,
                    null,
                    true, false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
            }

            return usuario;            
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Hashed; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return false; }
        }

        public override string ResetPassword(string username, string answer)
        {
            string sPasswordTemp = Membership.GeneratePassword(8, 0);

            MiLogicaUsuarios.CambiarPassword(username, FormsAuthentication.HashPasswordForStoringInConfigFile(sPasswordTemp, "sha1"));

            return sPasswordTemp;
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            string sPasswordHashed = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");

            bool bValidaUsuario = MiLogicaUsuarios.ValidarUsuario(username, password, sPasswordHashed);

            if (bValidaUsuario)
            {
                MiLogicaUsuarios.ObtenerDatosPersonalesPorLogin(username);
            }

            return bValidaUsuario;
        }
    }
}
