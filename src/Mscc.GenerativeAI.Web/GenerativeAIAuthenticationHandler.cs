using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mscc.GenerativeAI.Web
{
    public class GenerativeAIAuthenticationHandler<T> : DelegatingHandler
        where T : class, IGenerativeAIOptions
    {
        private readonly T options;

        public GenerativeAIAuthenticationHandler(IOptions<T> options)
        {
            this.options = options.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiKey = options?.Credentials?.ApiKey;
            if (string.IsNullOrEmpty(apiKey))
            {
                apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
            }

            if (string.IsNullOrEmpty(apiKey)) throw new ArgumentNullException(nameof(apiKey));

            request.Headers.Add(name: options.Scheme, value: apiKey);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}