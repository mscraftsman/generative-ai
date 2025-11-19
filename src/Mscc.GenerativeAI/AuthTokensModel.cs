#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Net.Http;
using System.Threading.Tasks;
#endif

namespace Mscc.GenerativeAI
{
    public class AuthTokensModel(IHttpClientFactory apiClient)
    {
        public async Task<AuthToken> Create(AuthToken authToken)
        {
            
        }
    }
}