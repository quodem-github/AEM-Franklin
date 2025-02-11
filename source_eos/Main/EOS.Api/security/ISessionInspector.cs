using System.Net.Http;
using System.Threading.Tasks;

namespace EOS.Api.security
{
    public interface ISessionInspector
    {
        /// <summary>
        /// The Http Request Message
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="errorMessage">The error message to be send to the client if the request is invalid</param>
        /// <returns>True: If the request is valid, flase otherwise</returns>
        bool IsValid(HttpRequestMessage request, out string errorMessage);
    }
}
