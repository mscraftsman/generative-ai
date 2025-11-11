using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The `ChatModel` class provides methods for interacting with a chat-based generative model.
    /// </summary>
    public class ChatModel : BaseModel
    {
        internal override string Version => ApiVersion.V1Beta;
        
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
        public ChatModel() : this(httpClientFactory: null, logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatModel"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public ChatModel(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger) { }

        /// <summary>
        /// Generates a set of responses from the model given a chat history input.
        /// </summary>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<ChatCompletionsResponse> Completions(ChatCompletionsRequest request,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = $"{BaseUrlGoogleAi}/chat/completions";
            return await PostAsync<ChatCompletionsRequest, ChatCompletionsResponse>(request, url, string.Empty, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }
    }
}