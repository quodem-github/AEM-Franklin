using System;
using System.Linq;
using EOS.ServiceLogic.DLL;
using EOS.ServiceLogic.Enums;
using EOS.ServiceModel;
using NHibernate;
using NHibernate.Linq;

namespace EOS.ServiceLogic.BLL.Token
{
    /// <summary>
    /// The service subscription service implementation.
    /// </summary>
    public class ServiceSubscriptionServiceImpl : IServiceSuscriptorService
    {
        #region Attributes

        /// <summary>NHibernate session object</summary>
        private ISession _sessVariable;
        public ISession _session
        {
            get
            {
                if (!_sessVariable.IsOpen)
                {
                    _sessVariable = Quodem.ORM.NHibernate.Helper.GetCurrentSession(ServiceLogic.Enums.EDbConnection.Default.GetHashCode());
                }
                return _sessVariable;
            }
            set { _sessVariable = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceSubscriptionServiceImpl"/> class.
        /// </summary>
        public ServiceSubscriptionServiceImpl()
        {
            _session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(Enums.EDbConnection.Default.GetHashCode());
        }

        #endregion

        #region Methods

        #region Publics

        /// <summary>
        /// Checks if a hashed key exist in the token database, if it is valid, then it updates the token update date.
        /// </summary>
        /// <param name="hashedKey">
        /// The hashed key.
        /// </param>
        /// <returns>
        /// True if the key exists and has a valid date (checking out expiry from configuration)
        /// </returns>
        public bool IsValidHashedKey(string hashedKey)
        {
            if (string.IsNullOrEmpty(hashedKey))
            {
                throw new Exception("The hashed key can´t be null or empty");
            }
            var serviceQuery = _session.Query<QServiceQuery>().FirstOrDefault(x => x.Hashedkey == hashedKey.Trim().ToUpper());

            // No token was found
            if (serviceQuery == null)
            {
                return false;
            }

            var tokenUpdateDate = serviceQuery.Updatedate.GetValueOrDefault().AddSeconds(int.Parse(Variables.InteropServiceCodeMaxSessionTimeKey));

            var isValid = DateTime.Compare(tokenUpdateDate, DateTime.Now) > 0;
            if (isValid)
            {
                //UpdateServiceQuery(serviceQuery);
            }

            return isValid;
        }

        public ServiceOrigin GetServiceQuery(string hashedKey)
        {
            if (string.IsNullOrEmpty(hashedKey))
            {
                throw new Exception("The hashed key can´t be null or empty");
            }

            QServiceQuery serviceQuery = _session.Query<QServiceQuery>()
                .FirstOrDefault(x => x.Hashedkey.Trim().ToUpper() == hashedKey.Trim().ToUpper());



            return _session.Query<QServiceSuscriptor>()
                .Where(x => x.Idservicesuscriptor == serviceQuery.Idservicesuscriptor).Select(x => new ServiceOrigin
                {
                    ServiceId = x.Idservice.GetValueOrDefault(),
                    SubscriptorId = x.Idsuscriptor.GetValueOrDefault()
                }).FirstOrDefault();
        }

        /// <summary>
        /// Creates an authorization GUID if the subscriber is valid. Stores the token and the GUID on the database.
        /// </summary>
        /// <param name="publicKey">
        /// The public key.
        /// </param>
        /// <returns>
        /// New GUID associated to the subscriber.
        /// </returns>
        public QServiceQuery GetAthorizationGuid(string publicKey)
        {
            if (string.IsNullOrEmpty(publicKey))
            {
                throw new Exception("The public key can´t be null or empty");
            }
            return CreateServiceQuery(publicKey);
        }

        #endregion

        #region Privates

        /// <summary>
        /// Generates a new security info object for the subscriber with the specified public key
        /// </summary>
        /// <param name="suscriptorPublickKey">Subscriber public key</param>
        /// <returns>
        /// A security info object with the Random GUID user to generate the hashed key
        /// </returns>
        private ServiceQuerySecurityInfo GenerateNewHashedKey(string suscriptorPublickKey)
        {
            if (string.IsNullOrEmpty(suscriptorPublickKey))
            {
                throw new Exception("La llave pública no puede ser null");
            }

            var privateKey =
                _session.Query<QSuscriptor>()
                    .Where(
                        x =>
                            x.Publickey == suscriptorPublickKey &&
                            x.Status != null && x.Status.Value == 1)
                    .Select(x => x.Privatekey)
                    .FirstOrDefault();

            if (string.IsNullOrEmpty(privateKey))
            {
                throw new Exception("No se encontró un servicio con la llave pública especificada");
            }

            var securityInfo = new ServiceQuerySecurityInfo { RandomGuid = Guid.NewGuid() };
            securityInfo.HashedKey = Quodem.ContentManager.Utils.HashPassword(Quodem.ContentManager.Variables.PasswordMethodType.SHA1, securityInfo.RandomGuid + privateKey);
            return securityInfo;
        }

        /// <summary>
        /// Creates a new service query storing the token and GUID on database
        /// </summary>
        /// <param name="suscriptorPublicKey">
        /// The subscriber public key.
        /// </param>
        /// <returns>
        /// </returns>
        private QServiceQuery CreateServiceQuery(string suscriptorPublicKey)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;
                    var securityInfo = GenerateNewHashedKey(suscriptorPublicKey);
                    var serviceQuery = new QServiceQuery
                    {
                        Createdate = now,
                        Updatedate = now,
                        Hashedkey = securityInfo.HashedKey,
                        Randonguid = securityInfo.RandomGuid.ToString(),
                        Idservicesuscriptor = 1
                    };
                    _session.SaveOrUpdate(serviceQuery);
                    transaction.Commit();

                    return serviceQuery;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates a service query setting current date on updates date
        /// </summary>
        /// <param name="serviceQuery">
        /// The service query.
        /// </param>
        private void UpdateServiceQuery(QServiceQuery serviceQuery)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    serviceQuery.Updatedate = DateTime.Now;
                    _session.SaveOrUpdate(serviceQuery);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        #endregion

        #endregion
    }
}