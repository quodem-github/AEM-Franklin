using Quodem.Utility;
using System.Configuration;

namespace EOS.ServiceLogic
{
    public static class Variables
    {
        public const string DATE_TIME_FORMAT = "yyyy-MM-ddTHH:mm:ss";
        public static string IdConnectionDatabase = ConfigurationManager.AppSettings["Eos.ConnectionStringName"];
        public static string CalculadoraIdPeticionActividadDefault = ConfigurationManager.AppSettings["Calculadora.IdPeticionActividadDefault"];
        public const string X_TOKEN = "X-Token";
        public const string AGENCY_KEY = "Agency-Key";
        public const string ENCRYPTION_ALGORITHM_NAME = "DESEDE";
        public const int NULL_INT = -1;
        public const int TRUE_VALUE = 1;
        public const int FALSE_VALUE = 0;

        public static string InteropServiceCodeMaxSessionTimeKey
        {
            get { return Quodem.Utility.Configuration.GetKey("Data.Interoperability.MaxSessionTime"); }
        }

        public static int PeticionariosListPageSize
        {
            get { return int.Parse(Quodem.Utility.Configuration.GetKey("API.PeticionariosList.PageSize")); }
        }

        public static int PassengersListPageSize
        {
            get { return int.Parse(Quodem.Utility.Configuration.GetKey("API.PassengersList.PageSize")); }
        }
        public static int ServicePageSize
        {
            get { return int.Parse(Quodem.Utility.Configuration.GetKey("API.Service.PageSize")); }
        }

        public static string QuodemIp
        {
            get { return Quodem.Utility.Configuration.GetKey("QuodemIp"); }
        }

    }
}
