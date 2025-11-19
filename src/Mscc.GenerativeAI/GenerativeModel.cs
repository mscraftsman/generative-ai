using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Mscc.GenerativeAI
{
    public class GenerativeModel : BaseModel
    {
        private const string UrlGoogleAi = "{BaseUrlGoogleAi}/{model}:{method}";
        private const string UrlVertexAi = "{BaseUrlVertexAi}/publishers/{publisher}/{model}:{method}";
        private const string UrlVertexAiExpress = "https://aiplatform.googleapis.com/{version}/publishers/{publisher}/{model}:{method}";

        private readonly bool _useVertexAi;
        private readonly bool _useVertexAiExpress;
        private readonly CachedContent? _cachedContent;
        private readonly TuningJob? _tuningJob;
        private readonly Tools defaultGoogleSearchRetrieval = [new Tool { GoogleSearchRetrieval = new() }];
        private readonly Tools defaultGoogleSearch = [new Tool { GoogleSearch = new() }];
        private readonly Tools defaultCodeExecution = [new Tool { CodeExecution = new() }];

        private List<SafetySetting>? _safetySettings;
        private GenerationConfig? _generationConfig;
        private Tools? _tools;
        private ToolConfig? _toolConfig;
        private Content? _systemInstruction;

        private string Url
        {
            get
            {
                var url = UrlGoogleAi;
                if (_useVertexAi)
                {
                    url = UrlVertexAi;
                    if (!string.IsNullOrEmpty(_endpointId))
                    {
                        url = "{BaseUrlVertexAi}/{endpointId}";
                    }

                    if (_useVertexAiExpress)
                    {
                        url = UrlVertexAiExpress;
                    }
                }

                if (!string.IsNullOrEmpty(RequestOptions?.BaseUrl))
                {
                    url = RequestOptions.BaseUrl;
                }

                return url;
            }
        }

        /// <inheritdoc />
        internal override string Version
        {
            get
            {
                if (_useVertexAi)
                {
                    return ApiVersion.V1;
                }

                return _apiVersion;
            }
            set
            {
                if (!_useVertexAi)
                {
                    _apiVersion = value;
                }
            }
        }

        private string Method
        {
            get
            {
                var model = _model.SanitizeModelName().Split(['/'], 2, StringSplitOptions.RemoveEmptyEntries)[1];
#if NET472_OR_GREATER || NETSTANDARD2_0
                switch (model)
                {
                    case GenerativeAI.Model.BisonChat:
                        return GenerativeAI.Method.GenerateMessage;
                    case GenerativeAI.Model.BisonText:
                        return GenerativeAI.Method.GenerateText;
                    case GenerativeAI.Model.GeckoEmbedding:
                        return GenerativeAI.Method.EmbedText;
                    case GenerativeAI.Model.Embedding:
                        return GenerativeAI.Method.EmbedContent;
                    case GenerativeAI.Model.GeminiEmbedding:
                        return GenerativeAI.Method.EmbedContent;
                    case GenerativeAI.Model.TextEmbedding:
                        return GenerativeAI.Method.EmbedContent;
                    case GenerativeAI.Model.Imagen3:
                        return GenerativeAI.Method.Predict;
                    case GenerativeAI.Model.Imagen3Fast:
                        return GenerativeAI.Method.Predict;
                    case GenerativeAI.Model.Veo3:
                        return GenerativeAI.Method.PredictLongRunning;
                    case GenerativeAI.Model.AttributedQuestionAnswering:
                        return GenerativeAI.Method.GenerateAnswer;
                    case GenerativeAI.Model.Gemini20Flash:
                        return UseRealtime
                            ? GenerativeAI.Method.BidirectionalGenerateContent
                            : GenerativeAI.Method.GenerateContent;
                }
                if (_useVertexAi)
                {
                    if (!string.IsNullOrEmpty(_endpointId))
                        return GenerativeAI.Method.GenerateContent;
                    if (_useVertexAiExpress)
                        return GenerativeAI.Method.GenerateContent;
                    
                    return GenerativeAI.Method.StreamGenerateContent;
                }

                return GenerativeAI.Method.GenerateContent;
#else
                return model switch
                {
                    GenerativeAI.Model.BisonChat => GenerativeAI.Method.GenerateMessage,
                    GenerativeAI.Model.BisonText => GenerativeAI.Method.GenerateText,
                    GenerativeAI.Model.GeckoEmbedding => GenerativeAI.Method.EmbedText,
                    GenerativeAI.Model.Embedding => GenerativeAI.Method.EmbedContent,
                    GenerativeAI.Model.GeminiEmbedding => GenerativeAI.Method.EmbedContent,
                    GenerativeAI.Model.TextEmbedding => GenerativeAI.Method.EmbedContent,
                    GenerativeAI.Model.Imagen3 => GenerativeAI.Method.Predict,
                    GenerativeAI.Model.Imagen3Fast => GenerativeAI.Method.Predict,
                    GenerativeAI.Model.Veo3 => GenerativeAI.Method.PredictLongRunning,
                    GenerativeAI.Model.AttributedQuestionAnswering => GenerativeAI.Method.GenerateAnswer,
                    GenerativeAI.Model.Gemini20Flash => UseRealtime
                        ? GenerativeAI.Method.BidirectionalGenerateContent
                        : GenerativeAI.Method.GenerateContent,
                    _ => _useVertexAi
                        ? (!string.IsNullOrEmpty(_endpointId)
                            ? GenerativeAI.Method.GenerateContent
                            : (_useVertexAiExpress)
                                ? GenerativeAI.Method.GenerateContent
                                : GenerativeAI.Method.StreamGenerateContent)
                        : GenerativeAI.Method.GenerateContent
                };
#endif
            }
        }

        internal bool IsVertexAI => _useVertexAi;

        /// <summary>
        /// You can enable Server Sent Events (SSE) for gemini-1.0-pro
        /// </summary>
        /// <remarks>
        /// See <a href="https://developer.mozilla.org/en-US/docs/Web/API/Server-sent_events">Server-sent Events</a>
        /// </remarks>
        public bool UseServerSentEventsFormat { get; set; } = false;

        /// <summary>
        /// Activate JSON Mode (default = no)
        /// </summary>
        public bool UseJsonMode { get; set; } = false;

        /// <summary>
        /// Activate Grounding with Google Search (default = no)
        /// </summary>
        public bool UseGrounding { get; set; } = false;

        /// <summary>
        /// Activate Google Search (default = no)
        /// </summary>
        public bool UseGoogleSearch { get; set; } = false;

        /// <summary>
        /// Activate automatic code execution (default = no)
        /// </summary>
        public bool UseCodeExecution { get; set; } = false;

        /// <summary>
        /// Enable realtime stream using Multimodal Live API
        /// </summary>
        public bool UseRealtime { get; set; } = false;

        /// <inheritdoc/>
        protected override void ThrowIfUnsupportedRequest<T>(T request)
        {
            if (request is CopyModelRequest && !_useVertexAi)
                throw new NotSupportedException("Copying a model is not supported with Google AI");
            if (_cachedContent is not null && (UseGrounding || UseGoogleSearch))
                throw new NotSupportedException("Google Search or Grounding is not supported with CachedContent.");
            if (UseJsonMode && (UseGrounding || UseGoogleSearch))
                throw new NotSupportedException("Google Search or Grounding is not supported with JSON mode.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerativeModel"/> class.
        /// </summary>
        public GenerativeModel() : this(httpClientFactory: null, logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerativeModel"/> class.
        /// The default constructor attempts to read <c>.env</c> file and environment variables.
        /// Sets default values, if available.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public GenerativeModel(IHttpClientFactory? httpClientFactory = null, ILogger? logger = null) : base(httpClientFactory, logger)
        {
            _apiVersion = ApiVersion.V1Beta;
            Logger.LogGenerativeModelInvoking();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerativeModel"/> class with access to Google AI Gemini API.
        /// </summary>
        /// <param name="apiKey">API key provided by Google AI Studio</param>
        /// <param name="model">Model to use</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <param name="systemInstruction">Optional. </param>
        /// <param name="toolConfig">Optional. Configuration of tools.</param>
        /// <param name="vertexAi">Optional. Flag to indicate use of Vertex AI in express mode.</param>
        /// <param name="httpClientFactory">Optional. The IHttpClientFactory to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        internal GenerativeModel(string? apiKey = null,
            string? model = null,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            Tools? tools = null,
            Content? systemInstruction = null,
            ToolConfig? toolConfig = null,
            bool vertexAi = false,
            IHttpClientFactory? httpClientFactory = null, 
            ILogger? logger = null) : this(httpClientFactory, logger)
        {
            Logger.LogGenerativeModelInvoking();

            ApiKey = apiKey ?? _apiKey;
            Model = model ?? _model;
            _generationConfig ??= generationConfig;
            _safetySettings ??= safetySettings;
            _tools ??= tools;
            _toolConfig ??= toolConfig;
            _systemInstruction ??= systemInstruction;

            var useVertexAi = Environment.GetEnvironmentVariable("GOOGLE_GENAI_USE_VERTEXAI") ?? bool.FalseString;
            _useVertexAi = Convert.ToBoolean(useVertexAi) || vertexAi;
            _useVertexAiExpress = vertexAi;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerativeModel"/> class with access to Vertex AI Gemini API.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project</param>
        /// <param name="region">Region to use</param>
        /// <param name="model">Model to use</param>
        /// <param name="endpoint">Optional. Endpoint ID of the tuned model to use.</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <param name="systemInstruction">Optional. </param>
        /// <param name="toolConfig">Optional. Configuration of tools.</param>
        /// <param name="httpClientFactory">Optional. The IHttpClientFactory to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        internal GenerativeModel(string? projectId = null, string? region = null,
            string? model = null, string? endpoint = null,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            Tools? tools = null,
            Content? systemInstruction = null,
            ToolConfig? toolConfig = null,
            IHttpClientFactory? httpClientFactory = null, 
            ILogger? logger = null) : base(projectId, region, model, httpClientFactory, logger)
        {
            Logger.LogGenerativeModelInvoking();

            _useVertexAi = true;
            _endpointId = endpoint?.SanitizeEndpointName();
            _generationConfig = generationConfig;
            _safetySettings = safetySettings;
            _tools = tools;
            _toolConfig = toolConfig;
            _systemInstruction = systemInstruction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerativeModel"/> class given cached content.
        /// </summary>
        /// <param name="cachedContent">Content that has been preprocessed.</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="httpClientFactory">Optional. The IHttpClientFactory to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="cachedContent"/> is null.</exception>
        internal GenerativeModel(CachedContent cachedContent,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            IHttpClientFactory? httpClientFactory = null, 
            ILogger? logger = null) : this(httpClientFactory, logger)
        {
            _cachedContent = cachedContent ?? throw new ArgumentNullException(nameof(cachedContent));

            Model = cachedContent.Model;
            _tools = cachedContent.Tools;
            _toolConfig = cachedContent.ToolConfig;
            _systemInstruction = cachedContent.SystemInstruction;
            _generationConfig = generationConfig;
            _safetySettings = safetySettings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerativeModel"/> class given cached content.
        /// </summary>
        /// <param name="tuningJob">Tuning Job to use with the model.</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="httpClientFactory">Optional. The IHttpClientFactory to use for creating HttpClient instances.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="tuningJob"/> is null.</exception>
        internal GenerativeModel(TuningJob tuningJob,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            IHttpClientFactory? httpClientFactory = null,
            ILogger? logger = null) : this(httpClientFactory, logger)
        {
            _tuningJob = tuningJob ?? throw new ArgumentNullException(nameof(tuningJob));

            _model = _tuningJob.TunedModel!.Model;
            _endpointId = _tuningJob.TunedModel!.Endpoint.SanitizeEndpointName();
            _generationConfig = generationConfig;
            _safetySettings = safetySettings;
        }

        /// <summary>
        /// Internal constructor for testing purposes, allows injecting a custom HttpMessageHandler.
        /// </summary>
        internal GenerativeModel(HttpMessageHandler handler, ILogger? logger = null) : base(handler, logger)
        {
            _apiVersion = ApiVersion.V1Beta;
            Logger.LogGenerativeModelInvoking();
            // Default model for tests, can be overridden by test setup
            Model = GenerativeAI.Model.Gemini25Pro;
        }

        #region Undecided location of methods.Maybe IGenerativeAI might be better...

        /// <summary>
        /// Get a list of available tuned models and description.
        /// </summary>
        /// <returns>List of available tuned models.</returns>
        /// <param name="pageSize">The maximum number of Models to return (per page).</param>
        /// <param name="pageToken">A page token, received from a previous ListModels call. Provide the pageToken returned by one request as an argument to the next request to retrieve the next page.</param>
        /// <param name="filter">Optional. A filter is a full text search over the tuned model's description and display name. By default, results will not include tuned models shared with everyone. Additional operators: - owner:me - writers:me - readers:me - readers:everyone</param>
        /// <param name="returnPartialSuccess">When set to `true`, operations that are reachable are returned as normal, and those that are unreachable are returned in the [ListOperationsResponse.unreachable] field. This can only be `true` when reading across collections e.g. when `parent` is set to `"projects/example/locations/-"`. This field is not by default supported and will result in an `UNIMPLEMENTED` error if set unless explicitly documented otherwise in service or product specific documentation.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="NotSupportedException"></exception>
        private async Task<List<ModelResponse>> ListTunedModels(int? pageSize = null, 
            string? pageToken = null, 
            string? filter = null, 
            bool? returnPartialSuccess = false,
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (_useVertexAi)
            {
                throw new NotSupportedException();
            }

            if (!string.IsNullOrEmpty(_apiKey))
            {
                throw new NotSupportedException("Accessing tuned models via API key is not provided. Setup OAuth for your project.");
            }

            var url = "{BaseUrlGoogleAi}/tunedModels"; // v1beta3
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(pageSize)] = Convert.ToString(pageSize, CultureInfo.InvariantCulture),
                [nameof(pageToken)] = pageToken,
                [nameof(filter)] = filter,
                [nameof(returnPartialSuccess)] = Convert.ToString(returnPartialSuccess, CultureInfo.InvariantCulture)
            };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            var models = await Deserialize<ListTunedModelResponse>(response);
            return models?.TunedModels!;
        }

        /// <summary>
        /// Lists the [`Model`s](https://ai.google.dev/gemini-api/docs/models/gemini) available through the Gemini API.
        /// </summary>
        /// <returns>List of available models.</returns>
        /// <param name="tuned">Flag, whether models or tuned models shall be returned.</param>
        /// <param name="pageSize">The maximum number of `Models` to return (per page). If unspecified, 50 models will be returned per page. This method returns at most 1000 models per page, even if you pass a larger page_size.</param>
        /// <param name="pageToken">A page token, received from a previous ListModels call. Provide the pageToken returned by one request as an argument to the next request to retrieve the next page.</param>
        /// <param name="filter">Optional. A filter is a full text search over the tuned model's description and display name. By default, results will not include tuned models shared with everyone. Additional operators: - owner:me - writers:me - readers:me - readers:everyone</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<List<ModelResponse>> ListModels(bool tuned = false,
            int? pageSize = 50,
            string? pageToken = null,
            string? filter = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (tuned)
            {
                return await ListTunedModels(pageSize, pageToken, filter, false, requestOptions, cancellationToken);
            }

            var url = _useVertexAi ? "{BaseUrlVertexAi}/models" : "{BaseUrlGoogleAi}/models";
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(pageSize)] = Convert.ToString(pageSize),
                [nameof(pageToken)] = pageToken
            };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            var models = await Deserialize<ListModelsResponse>(response);
            return models?.Models!;
        }

        /// <summary>
        /// Gets information about a specific `Model` such as its version number, token limits, [parameters](https://ai.google.dev/gemini-api/docs/models/generative-models#model-parameters) and other metadata. Refer to the [Gemini models guide](https://ai.google.dev/gemini-api/docs/models/gemini) for detailed model information.
        /// </summary>
        /// <param name="model">Required. The resource name of the model. This name should match a model name returned by the ListModels method. Format: models/model-id or tunedModels/my-model-id</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<ModelResponse> GetModel(string? model = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            this.GuardSupported();

            model ??= _model;
            model = model.SanitizeModelName();
            if (!string.IsNullOrEmpty(_apiKey) && model.StartsWith("tunedModel", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException("Accessing tuned models via API key is not provided. Setup OAuth for your project.");
            }

            var url = $"{BaseUrlGoogleAi}/{model}";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<ModelResponse>(response);
        }

        // ToDo: Copy model on Vertex AI
        // Ref: https://cloud.google.com/vertex-ai/docs/model-registry/copy-model
        /// <summary>
        /// Copies a model in Vertex AI Model Registry.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        public async Task<CopyModelResponse> CopyModel(CopyModelRequest request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            ThrowIfUnsupportedRequest(request);

            var url = "{BaseUrlVertexAi}/models:{method}";
            return await PostAsync<CopyModelRequest, CopyModelResponse>(request,
                url,
                GenerativeAI.Method.Copy,
                requestOptions,
                HttpCompletionOption.ResponseContentRead,
                cancellationToken);
        }

        /// <summary>
        /// Creates a tuned model.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        public async Task<CreateTunedModelResponse> CreateTunedModel(CreateTunedModelRequest request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(_apiKey))
            {
                throw new NotSupportedException("Accessing tuned models via API key is not provided. Setup OAuth for your project.");
            }

            var method = GenerativeAI.Method.TunedModels;
            // var method = "createTunedModel";
            // if (_model is (string)Model.BisonText001)
            //     method = "createTunedTextModel";
            var url = "{BaseUrlGoogleAi}/{method}"; // v1beta3
            return await PostAsync<CreateTunedModelRequest, CreateTunedModelResponse>(request,
                url,
                method,
                requestOptions,
                HttpCompletionOption.ResponseContentRead,
                cancellationToken);
        }

        /// <summary>
        /// Deletes a tuned model.
        /// </summary>
        /// <param name="model">Required. The resource name of the model. Format: tunedModels/my-model-id</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="model"/> is null or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        public async Task<string> DeleteTunedModel(string model,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(model)) throw new ArgumentNullException(nameof(model));
            this.GuardSupported();

            model = model.SanitizeModelName();
            if (!string.IsNullOrEmpty(_apiKey) && model.StartsWith("tunedModel", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException("Accessing tuned models via API key is not provided. Setup OAuth for your project.");
            }

            var url = $"{BaseUrlGoogleAi}/{model}";   // v1beta3
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
#if NET472_OR_GREATER || NETSTANDARD2_0
            return await response.Content.ReadAsStringAsync();
#else
            return await response.Content.ReadAsStringAsync(cancellationToken);
#endif
        }

        /// <summary>
        /// Updates a tuned model.
        /// </summary>
        /// <param name="model">Required. The resource name of the model. Format: tunedModels/my-model-id</param>
        /// <param name="tunedModel">The tuned model to update.</param>
        /// <param name="updateMask">Optional. The list of fields to update. This is a comma-separated list of fully qualified names of fields. Example: "user.displayName,photo".</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="model"/> is null or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        public async Task<ModelResponse> UpdateTunedModel(string model,
            ModelResponse tunedModel,
            string? updateMask = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(model)) throw new ArgumentNullException(nameof(model));
            this.GuardSupported();

            model = model.SanitizeModelName();
            if (!string.IsNullOrEmpty(_apiKey) && model.StartsWith("tunedModel", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException("Accessing tuned models via API key is not provided. Setup OAuth for your project.");
            }

            var url = $"{BaseUrlGoogleAi}/{model}";   // v1beta3
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(updateMask)] = updateMask
            };
            
            url = ParseUrl(url).AddQueryString(queryStringParams);
            var json = Serialize(tunedModel);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            using var httpRequest = new HttpRequestMessage();
#if NET472_OR_GREATER || NETSTANDARD2_0
            httpRequest.Method = new HttpMethod("PATCH");
#else
            httpRequest.Method = HttpMethod.Patch;
#endif
            httpRequest.RequestUri = new Uri(url);
            httpRequest.Version = _httpVersion;
            httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<ModelResponse>(response);
        }

        /// <summary>
        /// Transfers ownership of the tuned model. This is the only way to change ownership of the tuned model. The current owner will be downgraded to writer role.
        /// </summary>
        /// <param name="model">Required. The resource name of the tuned model to transfer ownership. Format: tunedModels/my-model-id</param>
        /// <param name="emailAddress">Required. The email address of the user to whom the tuned model is being transferred to.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="model"/> or <paramref name="emailAddress"/> is null or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        public async Task<string> TransferOwnership(string model,
            string emailAddress,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(model)) throw new ArgumentNullException(nameof(model));
            if (string.IsNullOrEmpty(emailAddress)) throw new ArgumentNullException(nameof(emailAddress));
            this.GuardSupported();

            model = model.SanitizeModelName();
            if (!string.IsNullOrEmpty(_apiKey) && model.StartsWith("tunedModel", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new NotSupportedException("Accessing tuned models via API key is not provided. Setup OAuth for your project.");
            }

            var method = GenerativeAI.Method.TransferOwnership;
            var url = ParseUrl(Url, method);
            var json = Serialize(new { EmailAddress = emailAddress });   // TransferOwnershipRequest
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
#if NET472_OR_GREATER || NETSTANDARD2_0
            return await response.Content.ReadAsStringAsync();
#else
            return await response.Content.ReadAsStringAsync(cancellationToken);
#endif
        }

        #endregion

        #region Obsolete methods

        /// <summary>
        /// Uploads a file to the File API backend.
        /// </summary>
        /// <param name="uri">URI or path to the file to upload.</param>
        /// <param name="displayName">A name displayed for the uploaded file.</param>
        /// <param name="resumable">Flag indicating whether to use resumable upload.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the upload.</param>
        /// <returns>A URI of the uploaded file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="uri"/> is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the file <paramref name="uri"/> is not found.</exception>
        /// <exception cref="MaxUploadFileSizeException">Thrown when the file size exceeds the maximum allowed size.</exception>
        /// <exception cref="UploadFileException">Thrown when the file upload fails.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        [Obsolete("This method has been deprecated and will be removed soon. Use same method of class GoogleAI instead.")]
        public async Task<UploadMediaResponse> UploadFile(string uri,
            string? displayName = null,
            bool resumable = false,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));
            if (!File.Exists(uri)) throw new FileNotFoundException(nameof(uri));
            var fileInfo = new FileInfo(uri);
            if (fileInfo.Length > Constants.MaxUploadFileSize) throw new MaxUploadFileSizeException(nameof(uri));

            var mimeType = GenerativeAIExtensions.GetMimeType(uri);
            var totalBytes = new FileInfo(uri).Length;
            var request = new UploadMediaRequest()
            {
                File = new FileRequest()
                {
                    DisplayName = displayName ?? Path.GetFileNameWithoutExtension(uri),
                }
            };

            var baseUri = BaseUrlGoogleAi.ToLowerInvariant().Replace("/{version}", "");
            var url = $"{baseUri}/upload/{{Version}}/files"; // v1beta3 // ?key={apiKey}
            if (resumable)
            {
                url = $"{baseUri}/resumable/upload/{{Version}}/files"; // v1beta3 // ?key={apiKey}
            }
            url = ParseUrl(url).AddQueryString(new Dictionary<string, string?>()
            {
                ["alt"] = "json",
                ["uploadType"] = "multipart"
            });
            var json = Serialize(request);

            using var fs = new FileStream(uri, FileMode.Open);
            var multipartContent = new MultipartContent("related")
            {
                new StringContent(json, Encoding.UTF8, Constants.MediaType),
                new StreamContent(fs, (int)Constants.ChunkSize)
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue(mimeType), 
                        ContentLength = totalBytes
                    }
                }
            };
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = multipartContent;

            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<UploadMediaResponse>(response);
        }


        /// <summary>
        /// Uploads a stream to the File API backend.
        /// </summary>
        /// <param name="stream">Stream to upload.</param>
        /// <param name="displayName">A name displayed for the uploaded file.</param>
        /// <param name="mimeType">The MIME type of the stream content.</param>
        /// <param name="resumable">Flag indicating whether to use resumable upload.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the upload.</param>
        /// <returns>A URI of the uploaded file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="stream"/> is null or empty.</exception>
        /// <exception cref="MaxUploadFileSizeException">Thrown when the <paramref name="stream"/> size exceeds the maximum allowed size.</exception>
        /// <exception cref="UploadFileException">Thrown when the <paramref name="stream"/> upload fails.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        [Obsolete("This method has been deprecated and will be removed soon. Use same method of class GoogleAI instead.")]
        public async Task<UploadMediaResponse> UploadFile(Stream stream,
            string displayName,
            string mimeType,
            bool resumable = false,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (stream.Length > Constants.MaxUploadFileSize) throw new MaxUploadFileSizeException(nameof(stream));
            if (string.IsNullOrEmpty(mimeType)) throw new ArgumentException(nameof(mimeType));
            if (string.IsNullOrEmpty(displayName)) throw new ArgumentException(nameof(displayName));

            var totalBytes = stream.Length;
            var request = new UploadMediaRequest()
            {
                File = new FileRequest()
                {
                    DisplayName = displayName
                }
            };

            var baseUri = BaseUrlGoogleAi.ToLowerInvariant().Replace("/{version}", "");
            var url = $"{baseUri}/upload/{{Version}}/files"; // v1beta3 // ?key={apiKey}
            if (resumable)
            {
                url = $"{baseUri}/resumable/upload/{{Version}}/files"; // v1beta3 // ?key={apiKey}
            }
            url = ParseUrl(url).AddQueryString(new Dictionary<string, string?>()
            {
                ["alt"] = "json",
                ["uploadType"] = "multipart"
            });
            var json = Serialize(request);

            var multipartContent = new MultipartContent("related")
            {
                new StringContent(json, Encoding.UTF8, Constants.MediaType),
                new StreamContent(stream, (int)Constants.ChunkSize)
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue(mimeType), 
                        ContentLength = totalBytes
                    }
                }
            };

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = multipartContent;
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<UploadMediaResponse>(response);
        }

        /// <summary>
        /// Lists the metadata for Files owned by the requesting project.
        /// </summary>
        /// <param name="pageSize">The maximum number of Models to return (per page).</param>
        /// <param name="pageToken">A page token, received from a previous ListFiles call. Provide the pageToken returned by one request as an argument to the next request to retrieve the next page.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>List of files in File API.</returns>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        [Obsolete("This method has been deprecated and will be removed soon. Use same method of class GoogleAI instead.")]
        public async Task<ListFilesResponse> ListFiles(int? pageSize = 100, 
            string? pageToken = null, 
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            this.GuardSupported();

            var url = "{BaseUrlGoogleAi}/files";
            var queryStringParams = new Dictionary<string, string?>()
            {
                [nameof(pageSize)] = Convert.ToString(pageSize), 
                [nameof(pageToken)] = pageToken
            };

            url = ParseUrl(url).AddQueryString(queryStringParams);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<ListFilesResponse>(response);
        }

        /// <summary>
        /// Gets the metadata for the given File.
        /// </summary>
        /// <param name="file">Required. The resource name of the file to get. This name should match a file name returned by the ListFiles method. Format: files/file-id.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Metadata for the given file.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="file"/> is null or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        [Obsolete("This method has been deprecated and will be removed soon. Use same method of class GoogleAI instead.")]
        public async Task<FileResource> GetFile(string file, 
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(file)) throw new ArgumentNullException(nameof(file));
            this.GuardSupported();

            file = file.SanitizeFileName();

            var url = $"{BaseUrlGoogleAi}/{file}";
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<FileResource>(response);
        }

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="file">Required. The resource name of the file to get. This name should match a file name returned by the ListFiles method. Format: files/file-id.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>If successful, the response body is empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="file"/> is null or empty.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        [Obsolete("This method has been deprecated and will be removed soon. Use same method of class GoogleAI instead.")]
        public async Task<string> DeleteFile(string file, 
            RequestOptions? requestOptions = null, 
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(file)) throw new ArgumentNullException(nameof(file));
            this.GuardSupported();

            file = file.SanitizeFileName();

            var url = $"{BaseUrlGoogleAi}/{file}"; // v1beta3
            url = ParseUrl(url);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
#if NET472_OR_GREATER || NETSTANDARD2_0
            return await response.Content.ReadAsStringAsync();
#else
            return await response.Content.ReadAsStringAsync(cancellationToken);
#endif
        }

        #endregion

        /// <summary>
        /// Generates a model response given an input <see cref="GenerateContentRequest"/>.
        /// </summary>
        /// <remarks>
        /// Refer to the [text generation guide](https://ai.google.dev/gemini-api/docs/text-generation) for detailed usage information.
        /// Input capabilities differ between models, including tuned models.
        /// Refer to the [model guide](https://ai.google.dev/gemini-api/docs/models/gemini) and [tuning guide](https://ai.google.dev/gemini-api/docs/model-tuning) for details.
        /// </remarks>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model or combination of features.</exception>
        public async Task<GenerateContentResponse> GenerateContent(GenerateContentRequest? request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            ThrowIfUnsupportedRequest(request);

            request.Tools ??= _tools;
            request.ToolConfig ??= _toolConfig;
            request.SystemInstruction ??= _systemInstruction;

            if (_cachedContent is not null)
            {
                request.CachedContent = _cachedContent.Name;
                Model = _cachedContent.Model;
                if (_cachedContent.Contents != null)
                {
                    request.Contents.AddRange(_cachedContent.Contents);
                }

                // "CachedContent can not be used with GenerateContent request setting system_instruction, tools or tool_config."
                request.Tools = null;
                request.ToolConfig = null;
                request.SystemInstruction = null;
            }

            request.Model = !string.IsNullOrEmpty(request.Model) ? request.Model : _model;
            request.GenerationConfig ??= _generationConfig;
            request.SafetySettings ??= _safetySettings;

            if (UseJsonMode)
            {
                request.GenerationConfig ??= new GenerationConfig();
                request.GenerationConfig.ResponseMimeType ??= Constants.MediaType;
            }

            if (UseGoogleSearch)
            {
                request.Tools ??= defaultGoogleSearch;
                if (request.Tools != null && !request.Tools.Any(t => t.GoogleSearch is not null))
                {
                    request.Tools.AddRange(defaultGoogleSearch);
                }
            }

            if (UseGrounding)
            {
                request.Tools ??= defaultGoogleSearchRetrieval;
                if (request.Tools != null && !request.Tools.Any(t => t.GoogleSearchRetrieval is not null))
                {
                    request.Tools.AddRange(defaultGoogleSearchRetrieval);
                }
            }

            if (UseCodeExecution)
            {
                request.Tools ??= defaultCodeExecution;
                if (request.Tools != null && !request.Tools.Any(t => t.CodeExecution is not null))
                {
                    request.Tools.AddRange(defaultCodeExecution);
                }
            }
            
            var url = ParseUrl(Url, Method);
            var json = Serialize(request);

            Logger.LogMethodInvokingRequest(nameof(GenerateContent));

            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            // ToDo: Handle payload exception like this
            // except google.api_core.exceptions.InvalidArgument as e:
            // if e.message.startswith("Request payload size exceeds the limit:"):
            // e.message += (
            //     " Please upload your files with the File API instead."
            // "`f = genai.upload_file(path); m.generate_content(['tell me about this file:', f])`"
            //     )
            await response.EnsureSuccessAsync(cancellationToken);

            if (_useVertexAi && !_useVertexAiExpress)
            {
                var contents = await Deserialize<List<GenerateContentResponse>>(response);
                return contents.Merge();
            }

            return await Deserialize<GenerateContentResponse>(response);
        }

        /// <summary>
        /// Generates a response from the model given an input prompt and other parameters.
        /// </summary>
        /// <param name="prompt">Required. String to process.</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <param name="toolConfig">Optional. Configuration of tools.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="prompt"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<GenerateContentResponse> GenerateContent(string? prompt,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            Tools? tools = null,
            ToolConfig? toolConfig = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateContentRequest(prompt,
                generationConfig ?? _generationConfig,
                safetySettings ?? _safetySettings,
                tools ?? _tools,
                toolConfig: toolConfig ?? _toolConfig);
            return await GenerateContent(request, requestOptions, cancellationToken);
        }

        /// <remarks/>
        public async Task<GenerateContentResponse> GenerateContent(List<IPart>? parts,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            Tools? tools = null,
            ToolConfig? toolConfig = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            var request = new GenerateContentRequest(parts,
                generationConfig ?? _generationConfig,
                safetySettings ?? _safetySettings,
                tools ?? _tools,
                toolConfig: toolConfig ?? _toolConfig);
            request.Contents[0].Role = Role.User;
            return await GenerateContent(request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Generates a streamed response from the model given an input GenerateContentRequest.
        /// This method uses a MemoryStream and StreamContent to send a streaming request to the API.
        /// It runs asynchronously sending and receiving chunks to and from the API endpoint, which allows non-blocking code execution.
        /// </summary>
        /// <param name="request">The request to send to the API.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Stream of GenerateContentResponse with chunks asynchronously.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model or combination of features.</exception>
        public async IAsyncEnumerable<GenerateContentResponse> GenerateContentStream(GenerateContentRequest? request,
            RequestOptions? requestOptions = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            ThrowIfUnsupportedRequest(request);

            if (UseServerSentEventsFormat)
            {
                await foreach (var item in GenerateContentStreamSSE(request, requestOptions, cancellationToken))
                {
                    if (cancellationToken.IsCancellationRequested)
                        yield break;
                    yield return item;
                }

                yield break;
            }

            request.Tools ??= _tools;
            request.ToolConfig ??= _toolConfig;
            request.SystemInstruction ??= _systemInstruction;

            if (_cachedContent is not null)
            {
                request.CachedContent = _cachedContent.Name;
                Model = _cachedContent.Model;
                if (_cachedContent.Contents != null)
                {
                    request.Contents.AddRange(_cachedContent.Contents);
                }

                // "CachedContent can not be used with GenerateContent request setting system_instruction, tools or tool_config."
                request.Tools = null;
                request.ToolConfig = null;
                request.SystemInstruction = null;
            }

            request.Model = !string.IsNullOrEmpty(request.Model) ? request.Model : _model;
            request.GenerationConfig ??= _generationConfig;
            request.SafetySettings ??= _safetySettings;

            if (UseJsonMode)
            {
                request.GenerationConfig ??= new GenerationConfig();
                request.GenerationConfig.ResponseMimeType ??= Constants.MediaType;
            }

            if (UseGoogleSearch)
            {
                request.Tools ??= defaultGoogleSearch;
                if (request.Tools != null && !request.Tools.Any(t => t.GoogleSearch is not null))
                {
                    request.Tools.AddRange(defaultGoogleSearch);
                }
            }

            if (UseGrounding)
            {
                request.Tools ??= defaultGoogleSearchRetrieval;
                if (request.Tools != null && !request.Tools.Any(t => t.GoogleSearchRetrieval is not null))
                {
                    request.Tools.AddRange(defaultGoogleSearchRetrieval);
                }
            }

            if (UseCodeExecution)
            {
                request.Tools ??= defaultCodeExecution;
                if (request.Tools != null && !request.Tools.Any(t => t.CodeExecution is not null))
                {
                    request.Tools.AddRange(defaultCodeExecution);
                }
            }

            var method = "streamGenerateContent";
            var url = ParseUrl(Url, method);

            if (Logger.IsEnabled(LogLevel.Debug)) Logger.LogMethodInvokingRequest(nameof(GenerateContentStream));

            // Ref: https://code-maze.com/using-streams-with-httpclient-to-improve-performance-and-memory-usage/
            // Ref: https://www.stevejgordon.co.uk/using-httpcompletionoption-responseheadersread-to-improve-httpclient-performance-dotnet
            var ms = new MemoryStream();
            await JsonSerializer.SerializeAsync(ms, request, WriteOptions, cancellationToken);
            ms.Seek(0, SeekOrigin.Begin);

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Version = _httpVersion;
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

            using var payload = new StreamContent(ms);
            httpRequest.Content = payload;
            payload.Headers.ContentType = new MediaTypeHeaderValue(Constants.MediaType);
            using var response = await SendAsync(httpRequest, requestOptions, cancellationToken, HttpCompletionOption.ResponseHeadersRead);
            await response.EnsureSuccessAsync(cancellationToken);
            if (response.Content is not null)
            {
#if NET472_OR_GREATER || NETSTANDARD2_0
                using var stream = await response.Content.ReadAsStreamAsync();
#else
                using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
#endif
                // Ref: https://github.com/dotnet/runtime/issues/97128 - HttpIOException
                // https://github.com/grpc/grpc-dotnet/issues/2361#issuecomment-1895805167 
                await foreach (var item in JsonSerializer.DeserializeAsyncEnumerable<GenerateContentResponse>(
                                   stream, ReadOptions, cancellationToken))
                {
                    if (cancellationToken.IsCancellationRequested)
                        yield break;
                    yield return item;
                }
            }
        }

        /// <remarks/>
        public IAsyncEnumerable<GenerateContentResponse> GenerateContentStream(string? prompt,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            Tools? tools = null,
            ToolConfig? toolConfig = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateContentRequest(prompt,
                generationConfig ?? _generationConfig,
                safetySettings ?? _safetySettings,
                tools ?? _tools,
                toolConfig: toolConfig ?? _toolConfig);
            return GenerateContentStream(request, requestOptions, cancellationToken);
        }

        /// <remarks/>
        public IAsyncEnumerable<GenerateContentResponse> GenerateContentStream(List<IPart>? parts,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            Tools? tools = null,
            ToolConfig? toolConfig = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            var request = new GenerateContentRequest(parts,
                generationConfig ?? _generationConfig,
                safetySettings ?? _safetySettings,
                tools ?? _tools,
                toolConfig: toolConfig ?? _toolConfig);
            request.Contents[0].Role = Role.User;
            return GenerateContentStream(request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Generates a response from the model given an input GenerateContentRequest.
        /// </summary>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        internal async IAsyncEnumerable<GenerateContentResponse> GenerateContentStreamSSE(
            GenerateContentRequest? request,
            RequestOptions? requestOptions = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            request.Tools ??= _tools;
            request.ToolConfig ??= _toolConfig;
            request.SystemInstruction ??= _systemInstruction;

            if (_cachedContent is not null)
            {
                request.CachedContent = _cachedContent.Name;
                Model = _cachedContent.Model;
                if (_cachedContent.Contents != null)
                {
                    request.Contents.AddRange(_cachedContent.Contents);
                }

                // "CachedContent can not be used with GenerateContent request setting system_instruction, tools or tool_config."
                request.Tools = null;
                request.ToolConfig = null;
                request.SystemInstruction = null;
            }

            request.Model = !string.IsNullOrEmpty(request.Model) ? request.Model : _model;
            request.GenerationConfig ??= _generationConfig;
            request.SafetySettings ??= _safetySettings;

            var method = "streamGenerateContent";
            var url = ParseUrl(Url, method).AddQueryString(new Dictionary<string, string?>() { ["alt"] = "sse" });
            var json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Version = _httpVersion;
            httpRequest.Content = payload;
            // message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(Constants.MediaType));

            using var response = await SendAsync(httpRequest, requestOptions, cancellationToken, HttpCompletionOption.ResponseHeadersRead);
            await response.EnsureSuccessAsync(cancellationToken);
            if (response.Content is not null)
            {
#if NET472_OR_GREATER || NETSTANDARD2_0
                using var sr = new StreamReader(await response.Content.ReadAsStreamAsync());
#else
                using var sr = new StreamReader(await response.Content.ReadAsStreamAsync(cancellationToken));
#endif
                while (!sr.EndOfStream)
                {
#if NET472_OR_GREATER || NETSTANDARD2_0 || NET6_0
                    var data = await sr.ReadLineAsync();
#else
                    var data = await sr.ReadLineAsync(cancellationToken);
#endif
                    if (string.IsNullOrWhiteSpace(data))
                        continue;

                    var item = JsonSerializer.Deserialize<GenerateContentResponse>(
                        data.Substring("data:".Length).Trim(), ReadOptions);
                    if (cancellationToken.IsCancellationRequested)
                        yield break;
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Enqueues a batch of <see cref="EmbedContent"/> requests for batch processing.
        /// </summary>
        /// <remarks>
        /// We have a `BatchEmbedContents` handler in `GenerativeService`, but it was synchronized.
        /// So we name this one to be `Async` to avoid confusion.
        /// </remarks>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model or combination of features.</exception>
        public async Task<Operation> BatchEmbedContent(AsyncBatchEmbedContentRequest request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            ThrowIfUnsupportedRequest(request);

            var url = ParseUrl(Url, GenerativeAI.Method.AsyncBatchEmbedContent);
            return await PostAsync<AsyncBatchEmbedContentRequest, Operation>(request, url, GenerativeAI.Method.AsyncBatchEmbedContent, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Enqueues a batch of <see cref="GenerateContent"/> requests for batch processing.
        /// </summary>
        /// <remarks>
        /// Refer to the [text generation guide](https://ai.google.dev/gemini-api/docs/text-generation) for detailed usage information.
        /// Input capabilities differ between models, including tuned models.
        /// Refer to the [model guide](https://ai.google.dev/gemini-api/docs/models/gemini) and [tuning guide](https://ai.google.dev/gemini-api/docs/model-tuning) for details.
        /// </remarks>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model or combination of features.</exception>
        public async Task<Operation<GenerateContentBatchOutput>> BatchGenerateContent(BatchGenerateContentRequest request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            ThrowIfUnsupportedRequest(request);

            var url = ParseUrl(Url, GenerativeAI.Method.BatchGenerateContent);
            return await PostAsync<BatchGenerateContentRequest, Operation<GenerateContentBatchOutput>>(request, url, GenerativeAI.Method.BatchGenerateContent, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        // ToDo: Implement methode
        // Ref: https://ai.google.dev/gemini-api/docs/models/gemini-v2#live-api
        // Ref: https://ai.google.dev/api/multimodal-live
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GenerateContentResponse> BidiGenerateContent()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<GenerateImagesResponse> GenerateImages(GenerateImagesRequest request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = ParseUrl(Url, Method);
            return await PostAsync<GenerateImagesRequest, GenerateImagesResponse>(request, url, Method, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Generates images from text prompt.
        /// </summary>
        /// <param name="model">Required. Model to use.</param>
        /// <param name="prompt">Required. String to process.</param>
        /// <param name="config">Configuration of image generation.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="model"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="prompt"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<GenerateImagesResponse> GenerateImages(string model,
            string prompt, 
            GenerateImagesConfig? config = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateImagesRequest(prompt);
            request.Parameters = (ImageGenerationParameters)config ?? request.Parameters;

            return await GenerateImages(request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Generates images from text prompt.
        /// </summary>
        /// <param name="prompt">Required. String to process.</param>
        /// <param name="numberOfImages">Number of images to generate. Range: 1..8.</param>
        /// <param name="negativePrompt">A description of what you want to omit in the generated images.</param>
        /// <param name="aspectRatio">Aspect ratio for the image.</param>
        /// <param name="guidanceScale">Controls the strength of the prompt. Suggested values are - * 0-9 (low strength) * 10-20 (medium strength) * 21+ (high strength)</param>
        /// <param name="language">Language of the text prompt for the image.</param>
        /// <param name="safetyFilterLevel">Adds a filter level to Safety filtering.</param>
        /// <param name="personGeneration">Allow generation of people by the model.</param>
        /// <param name="enhancePrompt">Option to enhance your provided prompt.</param>
        /// <param name="addWatermark">Explicitly set the watermark</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="prompt"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<GenerateImagesResponse> GenerateImages(string prompt,
            int numberOfImages = 1, string? negativePrompt = null,
            ImageAspectRatio? aspectRatio = null, int? guidanceScale = null,
            ImagePromptLanguage? language = null, SafetyFilterLevel? safetyFilterLevel = null,
            PersonGeneration? personGeneration = null, bool? enhancePrompt = null,
            bool? addWatermark = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateImagesRequest(prompt, numberOfImages);
            request.Parameters.AspectRatio = aspectRatio ?? request.Parameters.AspectRatio;
            request.Parameters.NegativePrompt = negativePrompt ?? request.Parameters.NegativePrompt;
            request.Parameters.GuidanceScale = guidanceScale ?? request.Parameters.GuidanceScale;
            request.Parameters.Language = language ?? request.Parameters.Language;
            request.Parameters.SafetyFilterLevel = safetyFilterLevel ?? request.Parameters.SafetyFilterLevel;
            request.Parameters.PersonGeneration = personGeneration ?? request.Parameters.PersonGeneration;
            request.Parameters.EnhancePrompt = enhancePrompt ?? request.Parameters.EnhancePrompt;
            request.Parameters.AddWatermark = addWatermark ?? request.Parameters.AddWatermark;

            return await GenerateImages(request, requestOptions, cancellationToken);
        }

        // ToDo: https://googleapis.github.io/python-genai/genai.html#genai.models.AsyncModels.edit_image
        /// <summary>
        /// Edits a set of images specified in the request.
        /// </summary>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model or combination of features.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        private async Task<EditImageResponse> EditImage(EditImageRequest request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (!IsVertexAI) throw new NotSupportedException($"This method is only supported in the Vertex AI client.");

            return await PostAsync<EditImageRequest, EditImageResponse>(request, Url, GenerativeAI.Method.Predict, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Edits a set of images based on a text description and configuration.
        /// </summary>
        /// <param name="model">Required. Model to use.</param>
        /// <param name="prompt">Required. A text description of the edit to apply to the image.</param>
        /// <param name="images">List of references images for editing.</param>
        /// <param name="numberOfImages">Number of images to generate. Range: 1..8.</param>
        /// <param name="config">Configuration for image editing.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="model"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="prompt"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<EditImageResponse> EditImage(string? model,
            string prompt,
            List<ReferenceImage> images,
            int numberOfImages = 1, 
            EditImageConfig? config = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            Model = model ?? Model ?? throw new ArgumentNullException(nameof(model));
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new EditImageRequest(prompt, numberOfImages);
            request.Instances[0].ReferenceImages = images;
            request.Parameters = config ?? request.Parameters;

            return await EditImage(request, requestOptions, cancellationToken);
        }

        // ToDo: https://googleapis.github.io/python-genai/genai.html#genai.models.AsyncModels.upscale_image
        /// <summary>
        /// Makes an API request to upscale a provided image.
        /// </summary>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model or combination of features.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<UpscaleImageResponse> UpscaleImage(UpscaleImageRequest request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (!IsVertexAI) throw new NotSupportedException($"This method is only supported in the Vertex AI client.");
            
            return await PostAsync<UpscaleImageRequest, UpscaleImageResponse>(request, Url, GenerativeAI.Method.Predict, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Makes an API request to upscale a provided image.
        /// </summary>
        /// <param name="model">The model to use.</param>
        /// <param name="image">The input image for upscaling.</param>
        /// <param name="upscaleFactor">The factor to upscale the image (x2 or x4).</param>
        /// <param name="config">Configuration for upscaling.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="model"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="image"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<UpscaleImageResponse> UpscaleImage(string? model,
            Image? image,
            UpscaleFactor? upscaleFactor = UpscaleFactor.X2,
            UpscaleImageConfig? config = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            Model = model ?? Model ?? throw new ArgumentNullException(nameof(model));
            if (image == null) throw new ArgumentNullException(nameof(image));

            var request = new UpscaleImageRequest();
            request.Instances.Add(new Instance() { Image = image });
            request.Parameters.Mode = "upscale"; // UpscaleMode.Upscale;
            request.Parameters.NumberOfImages = 1;
            request.Parameters.UpscaleConfig ??= new UpscaleConfig();
            request.Parameters.UpscaleConfig.UpscaleFactor = upscaleFactor;
            request.Config = config;

            return await UpscaleImage(request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Generates a video response from the model given an input <see cref="GenerateVideosRequest"/>.
        /// </summary>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        public async Task<GenerateVideosResponse> GenerateVideos(GenerateVideosRequest request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = ParseUrl(Url, Method);
            return await PostAsync<GenerateVideosRequest, GenerateVideosResponse>(request, url, Method, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="prompt"></param>
        /// <param name="config"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<GenerateVideosResponse> GenerateVideos(string model,
            string prompt, GenerateVideosConfig? config = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateVideosRequest(prompt);
            request.Parameters = config ?? request.Parameters;

            return await GenerateVideos(request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Generates images from text prompt.
        /// </summary>
        /// <param name="prompt">Required. String to process.</param>
        /// <param name="numberOfImages">Number of images to generate. Range: 1..8.</param>
        /// <param name="negativePrompt">A description of what you want to omit in the generated images.</param>
        /// <param name="aspectRatio">Aspect ratio for the image.</param>
        /// <param name="guidanceScale">Controls the strength of the prompt. Suggested values are - * 0-9 (low strength) * 10-20 (medium strength) * 21+ (high strength)</param>
        /// <param name="language">Language of the text prompt for the image.</param>
        /// <param name="safetyFilterLevel">Adds a filter level to Safety filtering.</param>
        /// <param name="personGeneration">Allow generation of people by the model.</param>
        /// <param name="enhancePrompt">Option to enhance your provided prompt.</param>
        /// <param name="addWatermark">Explicitly set the watermark</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="prompt"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<GenerateVideosResponse> GenerateVideos(string prompt,
            int numberOfImages = 1, string? negativePrompt = null,
            ImageAspectRatio? aspectRatio = null, int? guidanceScale = null,
            ImagePromptLanguage? language = null, SafetyFilterLevel? safetyFilterLevel = null,
            PersonGeneration? personGeneration = null, bool? enhancePrompt = null,
            bool? addWatermark = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateVideosRequest(prompt, numberOfImages);
            request.Parameters.AspectRatio = aspectRatio;
//            request.Parameters.NegativePrompt ??= negativePrompt;
//            request.Parameters.GuidanceScale ??= guidanceScale;
//            request.Parameters.Language ??= language;
            //request.Parameters.SafetyFilterLevel = safetyFilterLevel ?? request.Parameters.SafetyFilterLevel;

            if (personGeneration is not null)
            {
                request.Parameters.PersonGeneration = personGeneration;
            }

            request.Parameters.EnhancePrompt = enhancePrompt;
//            request.Parameters.AddWatermark = addWatermark;

            return await GenerateVideos(request, requestOptions, cancellationToken);
        }

        //ToDo: Implement new endpoint method createCachedContent 
        //Models: gemini-1.5-pro-001 & gemini-1.5-flash-001 only

        /// <summary>
        /// Generates a grounded answer from the model given an input GenerateAnswerRequest.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Response from the model for a grounded answer.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<GenerateAnswerResponse> GenerateAnswer(GenerateAnswerRequest? request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = ParseUrl(Url, Method);
            return await PostAsync<GenerateAnswerRequest, GenerateAnswerResponse>(request, url, Method, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <remarks/>
        public async Task<GenerateAnswerResponse> GenerateAnswer(string? prompt,
            AnswerStyle? answerStyle = null,
            List<SafetySetting>? safetySettings = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateAnswerRequest(prompt,
                answerStyle,
                safetySettings ?? _safetySettings);
            return await GenerateAnswer(request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Generates a text embedding vector from the input `Content` using the specified [Gemini Embedding model](https://ai.google.dev/gemini-api/docs/models/gemini#text-embedding).
        /// </summary>
        /// <param name="request">Required. EmbedContentRequest to process. The content to embed. Only the parts.text fields will be counted.</param>
        /// <param name="model">Optional. The model used to generate embeddings. Defaults to models/embedding-001.</param>
        /// <param name="taskType">Optional. Optional task type for which the embeddings will be used. Can only be set for models/embedding-001.</param>
        /// <param name="title">Optional. An optional title for the text. Only applicable when TaskType is RETRIEVAL_DOCUMENT. Note: Specifying a title for RETRIEVAL_DOCUMENT provides better quality embeddings for retrieval.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>List containing the embedding (list of float values) for the input content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        public async Task<EmbedContentResponse> EmbedContent(EmbedContentRequest request,
            string? model = null,
            TaskType? taskType = null,
            string? title = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.Content == null) throw new ArgumentNullException(nameof(request.Content));
            if (string.IsNullOrEmpty(request.Model))
            {
                request.Model = model ?? _model;
            }

            request.TaskType ??= taskType;
            request.Title ??= title;

            if (!string.IsNullOrEmpty(request.Title) && request.TaskType != TaskType.RetrievalDocument) throw new NotSupportedException("If a title is specified, the task must be a retrieval document type task.");
            
            var url = ParseUrl(Url, Method);
            return await PostAsync<EmbedContentRequest, EmbedContentResponse>(request, url, Method, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Generates multiple embedding vectors from the input `Content` which consists of a batch of strings represented as `EmbedContentRequest` objects.
        /// </summary>
        /// <param name="requests">Required. Embed requests for the batch. The model in each of these requests must match the model specified BatchEmbedContentsRequest.model.</param>
        /// <param name="model">Optional. The model used to generate embeddings. Defaults to models/embedding-001.</param>
        /// <param name="taskType">Optional. Optional task type for which the embeddings will be used. Can only be set for models/embedding-001.</param>
        /// <param name="title">Optional. An optional title for the text. Only applicable when TaskType is RETRIEVAL_DOCUMENT. Note: Specifying a title for RETRIEVAL_DOCUMENT provides better quality embeddings for retrieval.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>List containing the embedding (list of float values) for the input content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="requests"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<EmbedContentResponse> EmbedContent(List<EmbedContentRequest> requests,
            string? model = null,
            TaskType? taskType = null,
            string? title = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (requests == null) throw new ArgumentNullException(nameof(requests));
            if (!string.IsNullOrEmpty(title) && taskType != TaskType.RetrievalDocument) throw new NotSupportedException("If a title is specified, the task must be a retrieval document type task.");

            var method = GenerativeAI.Method.BatchEmbedContents;
            var url = ParseUrl(Url, method);
            return await PostAsync<List<EmbedContentRequest>, EmbedContentResponse>(requests, url, method, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Generates an embedding from the model given an input Content.
        /// </summary>
        /// <param name="content">Required. String to process. The content to embed. Only the parts.text fields will be counted.</param>
        /// <param name="model">Optional. The model used to generate embeddings. Defaults to models/embedding-001.</param>
        /// <param name="taskType">Optional. Optional task type for which the embeddings will be used. Can only be set for models/embedding-001.</param>
        /// <param name="title">Optional. An optional title for the text. Only applicable when TaskType is RETRIEVAL_DOCUMENT. Note: Specifying a title for RETRIEVAL_DOCUMENT provides better quality embeddings for retrieval.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>List containing the embedding (list of float values) for the input content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="content"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        public async Task<EmbedContentResponse> EmbedContent(string content,
            string? model = null,
            TaskType? taskType = null,
            string? title = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            var request = new EmbedContentRequest(content)
            {
                Model = model,
                TaskType = taskType,
                Title = title
            };
            return await EmbedContent(request, requestOptions: requestOptions, cancellationToken: cancellationToken);
        }

        // Todo: Capture Python SDK for JSON structures.
        /// <summary>
        /// Generates an embedding from the model given an input Content.
        /// </summary>
        /// <param name="content">Required. List of strings to process. The content to embed. Only the parts.text fields will be counted.</param>
        /// <param name="model">Optional. The model used to generate embeddings. Defaults to models/embedding-001.</param>
        /// <param name="taskType">Optional. Optional task type for which the embeddings will be used. Can only be set for models/embedding-001.</param>
        /// <param name="title">Optional. An optional title for the text. Only applicable when TaskType is RETRIEVAL_DOCUMENT. Note: Specifying a title for RETRIEVAL_DOCUMENT provides better quality embeddings for retrieval.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>List containing the embedding (list of float values) for the input content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="content"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        public async Task<EmbedContentResponse> EmbedContent(IEnumerable<string> content,
            string? model = null,
            TaskType? taskType = null,
            string? title = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            // var requests = new List<EmbedContentRequest>();
            // foreach (string prompt in content)
            // {
            //     if (string.IsNullOrEmpty(prompt)) continue;
            //     var request = new EmbedContentRequest()
            //     {
            //         Model = model?.SanitizeModelName(),
            //         Content = new(prompt),
            //         TaskType = taskType,
            //         Title = title
            //     };
            //     requests.Add(request);
            // }
            // return await EmbedContent(requests);
            var request = new EmbedContentRequest()
            {
                Model = model?.SanitizeModelName(),
                Content = new(),
                TaskType = taskType,
                Title = title
            };
            foreach (var prompt in content)
            {
                if (string.IsNullOrEmpty(prompt)) continue;
                request.Content.Parts.Add(new() { Text = prompt });
            }

            return await EmbedContent(request, requestOptions: requestOptions, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Generates multiple embeddings from the model given input text in a synchronous call.
        /// </summary>
        /// <param name="content">Content to embed.</param>
        /// <param name="model">Optional. The model used to generate embeddings. Defaults to models/embedding-001.</param>
        /// <param name="taskType">Optional. Optional task type for which the embeddings will be used. Can only be set for models/embedding-001.</param>
        /// <param name="title">Optional. An optional title for the text. Only applicable when TaskType is RETRIEVAL_DOCUMENT. Note: Specifying a title for RETRIEVAL_DOCUMENT provides better quality embeddings for retrieval.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>List containing the embedding (list of float values) for the input content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="content"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">Thrown when the functionality is not supported by the model.</exception>
        public async Task<EmbedContentResponse> EmbedContent(ContentResponse content,
            string? model = null,
            TaskType? taskType = null,
            string? title = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            var request = new EmbedContentRequest()
            {
                Model = model,
                Content = content,
                TaskType = taskType,
                Title = title
            };
            return await EmbedContent(request, requestOptions: requestOptions, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Counts the number of tokens for the given content.
        /// </summary>
        /// <param name="model">The name of the GenAI model to use for token counting.</param>
        /// <param name="contents">A <see cref="List{Content}"/> to compute tokens for.</param>
        /// <param name="config">A <see cref="CountTokensConfig"/> instance that specifies the optional configurations.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task{CountTokensResponse}"/> that represents the asynchronous operation. The task result contains a <see
        /// cref="CountTokensResponse"/> instance with token information.</returns>
        public async Task<CountTokensResponse> CountTokens(string model,
            List<Content> contents,
            CountTokensConfig? config = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            var request = new CountTokensRequest()
                {
                    Model = model ?? _model, 
                    Contents = contents, 
                    Config = config
                };
            if (config?.HttpOptions != null)
            {
                requestOptions ??= new RequestOptions(config?.HttpOptions);
            }

            var method = GenerativeAI.Method.CountTokens;
            var url = ParseUrl(Url, method);
            return await PostAsync<CountTokensRequest, CountTokensResponse>(request, Url, method, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Counts the number of tokens for the given content.
        /// </summary>
        /// <remarks>
        /// Refer to the [tokens guide](https://ai.google.dev/gemini-api/docs/tokens) to learn more about tokens.
        /// </remarks>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task{CountTokensResponse}"/> that represents the asynchronous operation. The task result contains a <see
        /// cref="CountTokensResponse"/> instance with token information.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<CountTokensResponse> CountTokens(GenerateContentRequest request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var method = GenerativeAI.Method.CountTokens;
            var url = ParseUrl(Url, method);
            var json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<CountTokensResponse>(response);
        }

        /// <remarks/>
        public async Task<CountTokensResponse> CountTokens(string? prompt,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var model = _model.SanitizeModelName().Split(['/'], 2, StringSplitOptions.RemoveEmptyEntries).Last();
            switch (model)
            {
                case GenerativeAI.Model.BisonChat:
                    var chatRequest = new GenerateMessageRequest(prompt);
                    return await CountTokens(chatRequest, requestOptions, cancellationToken);
                case GenerativeAI.Model.BisonText:
                    var textRequest = new GenerateTextRequest(prompt);
                    return await CountTokens(textRequest, requestOptions, cancellationToken);
                case GenerativeAI.Model.GeckoEmbedding:
                    var embeddingRequest = new GenerateTextRequest(prompt);
                    return await CountTokens(embeddingRequest, requestOptions, cancellationToken);
                default:
                    var request = new GenerateContentRequest(prompt, _generationConfig, _safetySettings, _tools);
                    return await CountTokens(request, requestOptions, cancellationToken);
            }
        }

        /// <remarks/>
        public async Task<CountTokensResponse> CountTokens(List<IPart>? parts,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (parts == null) throw new ArgumentNullException(nameof(parts));

            var request = new GenerateContentRequest(parts, _generationConfig, _safetySettings, _tools);
            return await CountTokens(request, requestOptions, cancellationToken: cancellationToken);
        }

        public async Task<CountTokensResponse> CountTokens(FileResource file,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            var request = new GenerateContentRequest(file, _generationConfig, _safetySettings, _tools);
            return await CountTokens(request, requestOptions, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Computes the number of tokens for the given content.
        /// </summary>
        /// <param name="model">The name of the GenAI model to use for token computation.</param>
        /// <param name="contents">A <see cref="List{Content}"/> to compute tokens for.</param>
        /// <param name="config">A <see cref="ComputeTokensConfig"/> instance that specifies the optional configurations.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task{ComputeTokensResponse}"/> that represents the asynchronous operation. The task result contains a <see
        /// cref="ComputeTokensResponse"/> instance with token information.</returns>
        /// <exception cref="NotSupportedException">Thrown when called with a non-Vertex AI client.</exception>
        public async Task<ComputeTokensResponse> ComputeTokens(string model,
            List<Content> contents,
            ComputeTokensConfig? config = null,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (!_useVertexAi) throw new NotSupportedException("This method is only supported in the Vertex AI client.");
            
            var request = new ComputeTokensRequest()
            {
                Model = model ?? _model, 
                Contents = contents, 
                Config = config
            };
            if (config?.HttpOptions != null)
            {
                requestOptions ??= new RequestOptions(config?.HttpOptions);
            }

            var method = GenerativeAI.Method.ComputeTokens;
            var url = ParseUrl(Url, method);
            return await PostAsync<ComputeTokensRequest, ComputeTokensResponse>(request, Url, method, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Computes the number of tokens for the given content.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task{ComputeTokensResponse}"/> that represents the asynchronous operation. The task result contains a <see
        /// cref="ComputeTokensResponse"/> instance with token information.</returns>
        /// <exception cref="NotSupportedException">Thrown when called with a non-Vertex AI client.</exception>
        /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
        public async Task<ComputeTokensResponse> ComputeTokens(ComputeTokensRequest request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (!_useVertexAi) throw new NotSupportedException("This method is only supported in the Vertex AI client.");
            
            if (request == null) throw new ArgumentNullException(nameof(request));

            var method = GenerativeAI.Method.CountTokens;
            var url = ParseUrl(Url, method);
            var json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<ComputeTokensResponse>(response);
        }

        // Todo: Implementation missing
        /// <summary>
        /// Starts a chat session. 
        /// </summary>
        /// <param name="history">Optional. A collection of <see cref="ContentResponse"/> objects, or equivalents to initialize the session.</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <param name="enableAutomaticFunctionCalling"></param>
        /// <returns>Returns a <see cref="ChatSession"/> attached to this model.</returns>
        public ChatSession StartChat(List<ContentResponse>? history = null,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            Tools? tools = null,
            bool enableAutomaticFunctionCalling = false)
        {
            var config = generationConfig ?? _generationConfig;
            var safety = safetySettings ?? _safetySettings;
            var tool = tools ?? _tools;

            if (_cachedContent is not null)
            {
                history ??= _cachedContent.Contents?.Select(c =>
                    new ContentResponse { Role = c.Role, Parts = c.PartTypes }
                ).ToList();
            }

            return new ChatSession(this, history, config, safety, tool, enableAutomaticFunctionCalling);
        }

        /// <summary>
        /// Performs a prediction request.
        /// </summary>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Prediction response.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<PredictResponse> Predict(PredictRequest request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var method = GenerativeAI.Method.Predict;
            var url = ParseUrl(Url, method);
            return await PostAsync<PredictRequest, PredictResponse>(request, url, method, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Same as Predict but returns an LRO.
        /// </summary>
        /// <param name="request">Required. The request to send to the API.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Prediction response.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<Operation> PredictLongRunning(PredictLongRunningRequest request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var method = GenerativeAI.Method.PredictLongRunning;
            var url = ParseUrl(Url, method);
            return await PostAsync<PredictLongRunningRequest, Operation>(request, url, method, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        #region "PaLM 2" methods

        /// <summary>
        /// Generates a response from the model given an input message.
        /// </summary>
        /// <param name="request">The request to send to the API.</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<GenerateTextResponse> GenerateText(GenerateTextRequest? request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = ParseUrl(Url, Method);
            return await PostAsync<GenerateTextRequest, GenerateTextResponse>(request, url, Method, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <remarks/>
        public async Task<GenerateTextResponse> GenerateText(string prompt,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateTextRequest(prompt);
            return await GenerateText(request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Counts the number of tokens in the content. 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Number of tokens.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<CountTokensResponse> CountTokens(GenerateTextRequest? request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var method = GenerativeAI.Method.CountTextTokens;
            var url = ParseUrl(Url, method);
            var json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<CountTokensResponse>(response);
        }

        /// <summary>
        /// Generates a response from the model given an input prompt.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<GenerateMessageResponse> GenerateMessage(GenerateMessageRequest? request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = ParseUrl(Url, Method);
            return await PostAsync<GenerateMessageRequest, GenerateMessageResponse>(request, url, Method, requestOptions, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <remarks/>
        public async Task<GenerateMessageResponse> GenerateMessage(string prompt,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateMessageRequest(prompt);
            return await GenerateMessage(request, requestOptions, cancellationToken);
        }

        /// <summary>
        /// Runs a model's tokenizer on a string and returns the token count.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Number of tokens.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<CountTokensResponse> CountTokens(GenerateMessageRequest request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var method = GenerativeAI.Method.CountMessageTokens;
            var url = ParseUrl(Url, method);
            var json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, requestOptions, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<CountTokensResponse>(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<EmbedTextResponse> EmbedText(EmbedTextRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = ParseUrl(Url, Method);
            var json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, null, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<EmbedTextResponse>(response);
        }

        /// <remarks/>
        public async Task<EmbedTextResponse> EmbedText(string prompt,
            CancellationToken cancellationToken = default)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new EmbedTextRequest(prompt);
            return await EmbedText(request, cancellationToken);
        }

        /// <summary>
        /// Counts the number of tokens in the content. 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Number of tokens.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<CountTokensResponse> CountTokens(EmbedTextRequest request,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var method = GenerativeAI.Method.CountMessageTokens;
            var url = ParseUrl(Url, method);
            var json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);

            if (requestOptions != null)
            {
                //Client.Timeout = requestOptions.Timeout;
            }

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, null, cancellationToken);
            await response.EnsureSuccessAsync(cancellationToken);
            return await Deserialize<CountTokensResponse>(response);
        }

        /// <summary>
        /// Generates multiple embeddings from the model given input text in a synchronous call.
        /// </summary>
        /// <param name="request">Required. Embed requests for the batch. The model in each of these requests must match the model specified BatchEmbedContentsRequest.model.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>List of Embeddings of the content as a list of floating numbers.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        public async Task<EmbedTextResponse> BatchEmbedText(BatchEmbedTextRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = ParseUrl(Url, GenerativeAI.Method.BatchEmbedText);
            var json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, null, cancellationToken);
            return await Deserialize<EmbedTextResponse>(response);
        }

        #endregion
    }
}