using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EOS.ServiceModel;

namespace EOS.ServiceLogic.BLL.Token
{
    public interface IServiceSuscriptorService
    {
        /// <summary>
        /// Checks if the given hashed key is valid.
        /// </summary>
        /// <param name="hashedKey"></param>
        /// <returns>False: In case no hashed key is found in the database.
        /// False: In case the hashed key has exipired
        /// True: Other wise
        /// </returns>
        bool IsValidHashedKey(string hashedKey);

        /// <summary>
        /// Get a new authorization guid. 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        QServiceQuery GetAthorizationGuid(string publicKey);


        ServiceOrigin GetServiceQuery(string hashedKey);

    }
}
