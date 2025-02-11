using System.Collections.Generic;
using System.Linq;
using EOS.ServiceModel;
using NHibernate;
using NHibernate.Linq;

namespace EOS.ServiceLogic.BLL.Services
{
    public class AgencyKeyService
    {
        #region Definitions
        
        #endregion


        public AgencyKeyService()
        {
            
        }

        public string GetAgencyKey(string agencyKey, string token, List<ServiceModel.Confempresa> LstConfEmpresas, QSuscriptor suscriptor)
        {
            //Compruebo que el token no venga vacio
            if (string.IsNullOrEmpty(token)) return string.Empty;

            //Compruebo que la clave de agencia no venga vacia
            if (string.IsNullOrEmpty(agencyKey)) return string.Empty;

            var result =  Utility.Decrypt3DES(suscriptor, agencyKey, token);
            
            if (string.IsNullOrEmpty(result))
            {
                return string.Empty;
            }

            //Recupero la empresa con el id que recibo, si no existe devuelvo cadena vacia
            var confEmpresa = LstConfEmpresas.FirstOrDefault(x => x.Idagencia == result);
            if (confEmpresa == null)
            {
                return string.Empty;
            }

            return result;
        }
    }
}
