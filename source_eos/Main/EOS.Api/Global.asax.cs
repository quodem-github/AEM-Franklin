using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EOS.ServiceLogic.BLL.AutoMapper;
using EOS.ServiceModel;
using NHibernate;
using NHibernate.Linq;
using System.Net;

namespace EOS.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static QSuscriptor _suscriptor;
        public static QSuscriptor Suscriptor
        {
            get
            {
                if (_suscriptor == null)
                {
                    ISession session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(ServiceLogic.Enums.EDbConnection.Default.GetHashCode());
                    _suscriptor = session.Query<QSuscriptor>().First();
                }
                return _suscriptor;
            }
            set { _suscriptor = value; }
        }

        private static List<QServiceQuery> _lstTokens;
        public static List<QServiceQuery> LstTokens
        {
            get
            {
                if (_lstTokens == null)
                {
                    _lstTokens =  new List<QServiceQuery>();
                }
                return _lstTokens;
            }
            set { _lstTokens = value; }
        }

        private static List<ServiceModel.Confempresa> _lstConfEmpresas;
        public static List<ServiceModel.Confempresa> LstConfEmpresas
        {
            get
            {
                if (_lstConfEmpresas == null)
                {
                    ISession session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(ServiceLogic.Enums.EDbConnection.Default.GetHashCode());
                    _lstConfEmpresas = session.Query<ServiceModel.Confempresa>().ToList();
                }
                return _lstConfEmpresas;
            }
            set { _lstConfEmpresas = value; }
        }
        protected void Application_Start()
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;

            LoadCache();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutomapperService.InitializeAutomapper();
        }

        public void LoadCache()
        {
            ISession session = Quodem.ORM.NHibernate.Helper.GetCurrentSession(ServiceLogic.Enums.EDbConnection.Default.GetHashCode());
            LstConfEmpresas = session.Query<ServiceModel.Confempresa>().ToList();
            Suscriptor = session.Query<QSuscriptor>().First();
            LstTokens = new List<QServiceQuery>();
        }
    }
}
