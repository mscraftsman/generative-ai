#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace Mscc.GenerativeAI
{
    public class OpenAIModel : BaseModel
    {
        internal override string Version => ApiVersion.V1Beta;

        protected override void AddApiKeyHeader(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

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
        /// Initializes a new instance of the <see cref="OpenAIModel"/> class.
        /// </summary>
        public OpenAIModel() : this(httpClientFactory: null, logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenAIModel"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public OpenAIModel(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger) { }

        /// <summary>
        /// Lists the currently available models.
        /// </summary>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>List of available models.</returns>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<SdkListModelsResponse> ListModels(RequestOptions? requestOptions = null, CancellationToken cancellationToken = default)
        {
            var url = $"{BaseUrlGoogleAi}/openai/models";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            var models = await Deserialize<SdkListModelsResponse>(response);
            return models;
        }

        /// <summary>
        /// Gets a model instance.
        /// </summary>
        /// <param name="modelsId">Required. The resource name of the model. This name should match a model name returned by the ListModels method.</param>
        /// <param name="model">Required. The name of the model to get.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<SdkModel> GetModel(string? modelsId = null,
            string? model = null,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            modelsId ??= _model;
            modelsId = modelsId.SanitizeModelName();

            var url = $"{BaseUrlGoogleAi}/openai/{modelsId}";
            url = ParseUrl(url);
            if (!string.IsNullOrEmpty(model))
            {
                url = url.AddQueryString(new Dictionary<string, string?>()
                {
                    [nameof(model)] = model
                });
            }
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<SdkModel>(response);
        }
        
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

            var url = $"{BaseUrlGoogleAi}/openai/chat/completions";
            return await PostAsync<ChatCompletionsRequest, ChatCompletionsResponse>(request, url, string.Empty, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Generates embeddings from the model given an input.
        /// </summary>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<GenerateEmbeddingsResponse> Embeddings(GenerateEmbeddingsRequest request,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = $"{BaseUrlGoogleAi}/openai/embeddings";
            return await PostAsync<GenerateEmbeddingsRequest, GenerateEmbeddingsResponse>(request, url, string.Empty, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ImagesGenerationsResponse> Images(ImagesGenerationsRequest request,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = $"{BaseUrlGoogleAi}/openai/images/generations";
            return await PostAsync<ImagesGenerationsRequest, ImagesGenerationsResponse>(request, url, string.Empty, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }
    }
}