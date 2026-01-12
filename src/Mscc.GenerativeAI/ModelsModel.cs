using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Mscc.GenerativeAI
{
    public sealed class ModelsModel(IHttpClientFactory apiClient, ILogger logger) : GenerativeModel(apiClient, logger)
    {
    }
}