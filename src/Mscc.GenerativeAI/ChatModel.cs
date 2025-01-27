#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;

namespace Mscc.GenerativeAI
{
    public class ChatModel : BaseModel
    {
        protected override string Version => ApiVersion.V1Beta;
        
        /// <inheritdoc cref="BaseModel"/>
        protected override void AddApiKeyHeader(HttpRequestMessage request)
        {
            if (!string.IsNullOrEmpty(_apiKey))
            {
                if (request.Headers.Contains("Authorization"))
                {
                    request.Headers.Remove("Authorization");
                }
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatModel"/> class.
        /// </summary>
        public ChatModel() : this(logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatModel"/> class.
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public ChatModel(ILogger? logger) : base(logger) { }

        /// <summary>
        /// Generates a set of responses from the model given a chat history input.
        /// </summary>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<ChatCompletionsResponse> Completions(ChatCompletionsRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = $"{BaseUrlGoogleAi}/chat/completions";
            url = ParseUrl(url);
            var json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, cancellationToken);
            await response.EnsureSuccessAsync();
            return await Deserialize<ChatCompletionsResponse>(response);
        }
    }
}