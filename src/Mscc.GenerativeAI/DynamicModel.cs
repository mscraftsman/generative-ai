#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Threading;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Mscc.GenerativeAI
{
    public class DynamicModel : BaseModel
    {
        private string Url => "{BaseUrlGoogleAi}/dynamic/{model}:{method}";
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicModel"/> class.
        /// </summary>
        public DynamicModel() : this(httpClientFactory: null, logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicModel"/> class.
        /// The default constructor attempts to read <c>.env</c> file and environment variables.
        /// Sets default values, if available.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public DynamicModel(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger)
        {
            _apiVersion = ApiVersion.V1Beta;
            Logger.LogDynamicModelInvoking();
        }

        /// <summary>
        /// Generates a model response given an input `GenerateContentRequest`.
        /// Refer to the [text generation guide](https://ai.google.dev/gemini-api/docs/text-generation) for detailed usage information. Input capabilities differ between models, including tuned models. Refer to the [model guide](https://ai.google.dev/gemini-api/docs/models/gemini) and [tuning guide](https://ai.google.dev/gemini-api/docs/model-tuning) for details.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<GenerateContentResponse> GenerateContent(GenerateContentRequest request,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            return await PostAsync<GenerateContentRequest, GenerateContentResponse>(request, Url, "generateContent", requestOptions,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Generates a [streamed response](https://ai.google.dev/gemini-api/docs/text-generation?lang=python#generate-a-text-stream) from the model given an input `GenerateContentRequest`.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<GenerateContentResponse> GenerateContentStream(GenerateContentRequest request,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            return await PostAsync<GenerateContentRequest, GenerateContentResponse>(request, Url, "streamGenerateContent", requestOptions,
                cancellationToken: cancellationToken);
        }
    }
}