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
    public class OpenAIModel : BaseModel
    {
        protected override string Version => ApiVersion.V1Beta;
        
        /// <inheritdoc cref="BaseModel"/>
        public override string? ApiKey
        {
            set
            {
                _apiKey = value;
                if (value != null)
                    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenAIModel"/> class.
        /// </summary>
        public OpenAIModel() : this(logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenAIModel"/> class.
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public OpenAIModel(ILogger? logger) : base(logger) { }

        /// <summary>
        /// Lists the currently available models.
        /// </summary>
        /// <returns>List of available models.</returns>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<SdkListModelsResponse> ListModels()
        {
            var url = $"{BaseUrlGoogleAi}/openai/models";
            url = ParseUrl(url);
            var response = await Client.GetAsync(url);
            await response.EnsureSuccessAsync();
            var models = await Deserialize<SdkListModelsResponse>(response);
            return models;
        }

        /// <summary>
        /// Gets information about a specific `Model` such as its version number.
        /// </summary>
        /// <param name="model">Required. The resource name of the model. This name should match a model name returned by the ListModels method.</param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<SdkModel> GetModel(string? model = null)
        {
            model ??= _model;
            model = model.SanitizeModelName();

            var url = $"{BaseUrlGoogleAi}/openai/{model}";
            url = ParseUrl(url);
            var response = await Client.GetAsync(url);
            await response.EnsureSuccessAsync();
            return await Deserialize<SdkModel>(response);
        }
        
        /// <summary>
        /// Generates a set of responses from the model given a chat history input.
        /// </summary>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<ChatCompletionsResponse> Completions(ChatCompletionsRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = $"{BaseUrlGoogleAi}/openai/chat/completions";
            url = ParseUrl(url);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            var response = await Client.PostAsync(url, payload, cancellationToken);
            await response.EnsureSuccessAsync();
            return await Deserialize<ChatCompletionsResponse>(response);
        }

        /// <summary>
        /// Generates embeddings from the model given an input.
        /// </summary>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<GenerateEmbeddingsResponse> Embeddings(GenerateEmbeddingsRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = $"{BaseUrlGoogleAi}/openai/embeddings";
            url = ParseUrl(url);
            string json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            var response = await Client.PostAsync(url, payload, cancellationToken);
            await response.EnsureSuccessAsync();
            return await Deserialize<GenerateEmbeddingsResponse>(response);
        }
    }
}