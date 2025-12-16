using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Types;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Entry point to access Gemini API running in Vertex AI.
    /// </summary>
    /// <remarks>
    /// See <a href="https://cloud.google.com/vertex-ai/generative-ai/docs/model-reference/overview">Model reference</a>.
    /// See also https://cloud.google.com/nodejs/docs/reference/vertexai/latest/vertexai/vertexinit
    /// </remarks>
    public sealed class VertexAI : BaseLogger, IGenerativeAI
    {
        private readonly string? _projectId;
        private readonly string _region = "us-central1";
        private readonly string _version;
        private readonly string? _apiKey;
        private readonly string? _accessToken;
        private readonly bool _isExpressMode;
        private readonly IHttpClientFactory? _httpClientFactory;
        private readonly RequestOptions? _requestOptions;
        private GenerativeModel? _generativeModel;
        private FileSearchStoresModel? _fileSearchStoresModel;
        private OperationsModel? _operationsModel;
        private ImageGenerationModel? _imageGenerationModel;
        private BatchesModel? _batchesModel;
        private SupervisedTuningJobModel? _supervisedTuningJobModel;

        private string _endpointId = string.Empty;

        public string EndpointId
        {
            get => _endpointId;
            set => _endpointId = value.SanitizeEndpointName();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexAI"/> class with access to Vertex AI Gemini API.
        /// The default constructor attempts to read <c>.env</c> file and environment variables.
        /// Sets default values, if available.
        /// </summary>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="requestOptions">Optional. Options for the request.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        /// <remarks>The following environment variables are used:
        /// <list type="table">
        /// <item><term>GOOGLE_PROJECT_ID</term>
        /// <description>Identifier of the Google Cloud project.</description></item>
        /// <item><term>GOOGLE_REGION</term>
        /// <description>Identifier of the Google Cloud region to use (default: "us-central1").</description></item>
        /// </list>
        /// </remarks>
        private VertexAI(IHttpClientFactory? httpClientFactory = null,
            RequestOptions? requestOptions = null,
            ILogger? logger = null) : base(logger)
        {
            GenerativeAIExtensions.ReadDotEnv();
            _projectId = Environment.GetEnvironmentVariable("GOOGLE_PROJECT_ID") ??
                         Environment.GetEnvironmentVariable("GOOGLE_CLOUD_PROJECT");
            _region = Environment.GetEnvironmentVariable("GOOGLE_REGION") ??
                      Environment.GetEnvironmentVariable("GOOGLE_CLOUD_LOCATION") ?? _region;
            _apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY") ??
                      Environment.GetEnvironmentVariable("GEMINI_API_KEY");
            _accessToken = Environment.GetEnvironmentVariable("GOOGLE_ACCESS_TOKEN");
            _version = ApiVersion.V1;
            _httpClientFactory = httpClientFactory;
            _requestOptions = requestOptions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexAI"/> class with access to Vertex AI Gemini API.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project.</param>
        /// <param name="region">Optional. Region to use (default: "us-central1").</param>
        /// <param name="accessToken">Access token for the Google Cloud project.</param>
        /// <param name="endpointId">Optional. Endpoint ID of the deployed model to use.</param>
        /// <param name="apiVersion">Version of the API.</param>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="requestOptions">Optional. Options for the request.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="projectId"/> is <see langword="null"/>.</exception>
        public VertexAI(string? projectId,
            string? region = null,
            string? accessToken = null,
            string? endpointId = null,
            string? apiVersion = null,
            IHttpClientFactory? httpClientFactory = null,
            RequestOptions? requestOptions = null,
            ILogger? logger = null) : this(httpClientFactory, requestOptions, logger)
        {
            _projectId = projectId ?? _projectId ?? throw new ArgumentNullException(nameof(projectId));
            _region = region ?? _region;
            _accessToken = accessToken ?? _accessToken;
            _endpointId = endpointId?.SanitizeEndpointName() ?? _endpointId;
            _version = apiVersion ?? _version;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexAI"/> class with access to Vertex AI Gemini API.
        /// </summary>
        /// <param name="apiKey">API key for Vertex AI in express mode.</param>
        /// <param name="apiVersion">Version of the API.</param>
        /// <param name="httpClientFactory">Optional. The <see cref="IHttpClientFactory"/> to use for creating HttpClient instances.</param>
        /// <param name="requestOptions">Optional. Options for the request.</param>
        /// <param name="logger">Optional. Logger instance used for logging.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="apiKey"/> is <see langword="null"/>.</exception>
        public VertexAI(string? apiKey,
            string? apiVersion = null,
            IHttpClientFactory? httpClientFactory = null,
            RequestOptions? requestOptions = null,
            ILogger? logger = null) : this(httpClientFactory, requestOptions, logger)
        {
            _apiKey = apiKey ?? _apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _version = apiVersion ?? _version;
            _isExpressMode = true;
        }

        /// <summary>
        /// Create a generative model on Vertex AI to use.
        /// </summary>
        /// <param name="model">Model to use (default: "gemini-1.5-pro")</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <param name="systemInstruction">Optional. </param>
        /// <param name="logger">Optional. Logger instance used for logging.</param>
        /// <returns>Generative model instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when "projectId" or "region" is <see langword="null"/>.</exception>
        public GenerativeModel GenerativeModel(string model = Model.Gemini25Pro,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            Tools? tools = null,
            Content? systemInstruction = null,
            ILogger? logger = null)
        {
            Guard();

            if (_isExpressMode)
            {
                _generativeModel ??= new GenerativeModel(_apiKey,
                    model,
                    generationConfig,
                    safetySettings,
                    tools,
                    systemInstruction,
                    vertexAi: true,
                    httpClientFactory: _httpClientFactory,
                    logger: logger ?? Logger)
                {
                    Version = _version,
                    RequestOptions = _requestOptions
                };
            }

            _generativeModel ??= new GenerativeModel(_projectId,
                _region,
                model,
                _accessToken,
                _endpointId,
                generationConfig,
                safetySettings,
                tools,
                systemInstruction,
                httpClientFactory: _httpClientFactory,
                logger: logger ?? Logger)
            {
	            // AccessToken = _apiKey is null ? _accessToken : null,
                Version = _version,
                RequestOptions = _requestOptions
            };
            return _generativeModel;
        }

        /// <summary>
        /// Create a generative model on Vertex AI to use.
        /// </summary>
        /// <param name="cachedContent">Content that has been preprocessed.</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="logger">Optional. Logger instance used for logging.</param>
        /// <returns>Generative model instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="cachedContent"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when "projectId" or "region" is <see langword="null"/>.</exception>
        public GenerativeModel GenerativeModel(CachedContent cachedContent,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            ILogger? logger = null)
        {
            if (cachedContent == null) throw new ArgumentNullException(nameof(cachedContent));
            Guard();

            _generativeModel ??= new GenerativeModel(cachedContent,
                generationConfig,
                safetySettings,
                httpClientFactory: _httpClientFactory,
                logger: logger ?? Logger)
            {
                ProjectId = _projectId,
                Region = _region,
	            // AccessToken = _apiKey is null ? _accessToken : null,
                Version = _version,
                RequestOptions = _requestOptions
            };
            return _generativeModel;
        }

        /// <summary>
        /// Create a generative model on Vertex AI to use.
        /// </summary>
        /// <param name="tuningJob">Tuning Job to use with the model.</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="logger">Optional. Logger instance used for logging.</param>
        /// <returns>Generative model instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="tuningJob"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when "projectId" or "region" is <see langword="null"/>.</exception>
        public GenerativeModel GenerativeModel(TuningJob tuningJob,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            ILogger? logger = null)
        {
            if (tuningJob == null) throw new ArgumentNullException(nameof(tuningJob));
            Guard();

            _generativeModel ??= new GenerativeModel(tuningJob,
                generationConfig,
                safetySettings,
                httpClientFactory: _httpClientFactory,
                logger: logger ?? Logger)
            {
                ProjectId = _projectId, 
                Region = _region,
	            // AccessToken = _apiKey is null ? _accessToken : null,
                Version = _version,
                RequestOptions = _requestOptions
            };
            return _generativeModel;
        }

        /// <inheritdoc cref="IGenerativeAI"/>
        public Task<ModelResponse> GetModel(string model,
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">Model to use.</param>
        /// <param name="logger">Optional. Logger instance used for logging.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when "projectId" or "region" is <see langword="null"/>.</exception>
        public SupervisedTuningJobModel SupervisedTuningJob(string model = Model.GeminiPro,
            ILogger? logger = null)
        {
            Guard();

            _supervisedTuningJobModel ??= new SupervisedTuningJobModel(_projectId,
	            _region,
	            _accessToken,
	            model,
	            _httpClientFactory,
	            logger: logger);
            return _supervisedTuningJobModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">Model to use (default: "imagegeneration")</param>
        /// <param name="logger">Optional. Logger instance used for logging.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when "projectId" or "region" is <see langword="null"/>.</exception>
        public ImageGenerationModel ImageGenerationModel(string model = Model.ImageGeneration,
            ILogger? logger = null)
        {
            Guard();
            _imageGenerationModel ??= new ImageGenerationModel(_projectId,
                _region,
                _accessToken,
                model,
                _httpClientFactory,
                logger: logger);
            return _imageGenerationModel;
        }

        public FileSearchStoresModel FileSearchStoresModel()
        {
            Guard();
            _fileSearchStoresModel ??= new FileSearchStoresModel(_httpClientFactory, Logger)
            {
                ProjectId = _projectId,
                Region = _region,
                RequestOptions = _requestOptions
            };
            return _fileSearchStoresModel;
        }

        public OperationsModel OperationsModel()
        {
            Guard();
            _operationsModel ??= new OperationsModel(_httpClientFactory, Logger)
            {
                ProjectId = _projectId,
                Region = _region,
                RequestOptions = _requestOptions
            };
            return _operationsModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">Model to use (default: "imagetext")</param>
        /// <param name="logger">Optional. Logger instance used for logging.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when "projectId" or "region" is <see langword="null"/>.</exception>
        public ImageTextModel ImageTextModel(string model = Model.ImageText,
            ILogger? logger = null)
        {
            Guard();

            return new ImageTextModel(_projectId,
                _region,
                _accessToken,
                model,
                _httpClientFactory,
                logger: logger);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when "projectId" or "region" is <see langword="null"/>.</exception>
        public RagEngineModel RagEngineModel(ILogger? logger = null)
        {
            Guard();

            return new RagEngineModel(_projectId,
                _region,
                httpClientFactory: _httpClientFactory,
                logger: logger)
            {
                Version = _version
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when "apiKey" is <see langword="null"/></exception>
        /// <exception cref="ArgumentNullException">Thrown when "projectId" or "region" is <see langword="null"/>.</exception>
        private void Guard()
        {
            if (_isExpressMode)
            {
                if (_apiKey is null) throw new ArgumentNullException(message: "API key has not been set", null);
                return;
            }

            if (_projectId is null) throw new ArgumentNullException(message: "ProjectId has not been set", null);
            if (_region is null) throw new ArgumentNullException(message: "Region has not been set", null);
        }
    }
}