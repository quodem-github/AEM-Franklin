using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EOS.ServiceLogic.DLL
{
    internal class ServiceQuerySecurityInfo
    {
        /// <summary>
        /// The Random Guid GUID to generate the hased key. This Random Guid is going to be sent to the client
        /// </summary>
        public Guid RandomGuid { get; set; }

        /// <summary>
        /// The Hashed key generated using the Random GUID and the suscriptor private key
        /// </summary>
        public string HashedKey { get; set; }
    }
}
