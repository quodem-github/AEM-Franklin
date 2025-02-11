using System.Configuration;
using System;
using NHibernate;


namespace EOS.ServiceLogic
{
    public sealed class HelperNHibernateMySql : IDisposable
    {
        #region Const
        /// <summary>
        /// Connection 
        /// </summary>
        public const string IdKeyHibernateConnectionString = "connection.connection_string";
        #endregion

        #region Definitions
        /// <summary>
        /// Sessión factory
        /// </summary>
        private static ISessionFactory _sessionFactory;
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Method for dispose objects
        /// </summary>
        public void Dispose()
        {
            CloseSessionFactory();
        }
        #endregion

        #region Methods Statics


        private static ISession GetCurrentSession(string idConnectionString)
        {
            if (_sessionFactory == null)
            {
                var connectionString =ConfigurationManager.ConnectionStrings[idConnectionString].ConnectionString;
                var configFactory = new NHibernate.Cfg.Configuration();
                configFactory.SetProperty(IdKeyHibernateConnectionString, connectionString);
                configFactory.Configure();  
                _sessionFactory = configFactory.BuildSessionFactory();
            }
            return _sessionFactory.OpenSession();
        }
        
        /// <summary>
        /// Get Session For IdConnection DataBase
        /// </summary>
        /// <returns></returns>
        public static ISession GetCurrentSession()
        {
            return GetCurrentSession(Variables.IdConnectionDatabase);
        }

        /// <summary>
        /// Close Session Factory
        /// </summary>
        private static void CloseSessionFactory()
        {
            if (_sessionFactory != null && !_sessionFactory.IsClosed)
            {
                _sessionFactory.Close();
                _sessionFactory.Dispose();
            }
        }
        #endregion
    }
}
