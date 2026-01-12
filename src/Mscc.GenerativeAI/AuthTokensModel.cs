using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Types;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mscc.GenerativeAI
{
    public sealed class AuthTokensModel(IHttpClientFactory apiClient, ILogger logger)
    {
#if false
        public async Task<AuthToken> Create(AuthToken authToken)
        {
            
        }
#endif
    }
}