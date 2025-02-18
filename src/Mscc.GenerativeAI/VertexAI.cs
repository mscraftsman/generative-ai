#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.Logging;

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
        private readonly string? _apiKey;
        private readonly bool _isExpressMode = false;

        private string _endpointId;
        private GenerativeModel? _generativeModel;
        private ImageGenerationModel? _imageGenerationModel;
        private ImageTextModel? _imageTextModel;
        private SupervisedTuningJobModel? _tuningJobModel;

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
        /// <remarks>The following environment variables are used:
        /// <list type="table">
        /// <item><term>GOOGLE_PROJECT_ID</term>
        /// <description>Identifier of the Google Cloud project.</description></item>
        /// <item><term>GOOGLE_REGION</term>
        /// <description>Identifier of the Google Cloud region to use (default: "us-central1").</description></item>
        /// </list>
        /// </remarks>
        private VertexAI(ILogger? logger = null) : base(logger)
        {
            GenerativeAIExtensions.ReadDotEnv();
            _projectId = Environment.GetEnvironmentVariable("GOOGLE_PROJECT_ID");
            _region = Environment.GetEnvironmentVariable("GOOGLE_REGION") ?? _region;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexAI"/> class with access to Vertex AI Gemini API.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project.</param>
        /// <param name="region">Optional. Region to use (default: "us-central1").</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="projectId"/> is <see langword="null"/>.</exception>
        public VertexAI(string? projectId, string? region = null, ILogger? logger = null) : this(logger)
        {
            _projectId ??= projectId ?? throw new ArgumentNullException(nameof(projectId));
            _region = region ?? _region;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexAI"/> class with access to Vertex AI Gemini API.
        /// </summary>
        /// <param name="apiKey">API key for Vertex AI in express mode.</param>
        /// <param name="logger">Optional. Logger instance used for logging.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="apiKey"/> is <see langword="null"/>.</exception>
        public VertexAI(string? apiKey, ILogger? logger = null) : this(logger)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
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
        /// <returns>Generative model instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when "projectId" or "region" is <see langword="null"/>.</exception>
        public GenerativeModel GenerativeModel(string model = Model.Gemini15Pro,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            Tools? tools = null,
            Content? systemInstruction = null)
        {
            if (_isExpressMode)
            {
                if (_apiKey is null) throw new ArgumentNullException(message: "API key has not been set", null);
                _generativeModel = new GenerativeModel(_apiKey,
                    model,
                    generationConfig,
                    safetySettings,
                    tools,
                    systemInstruction,
                    vertexAi: true);
                return _generativeModel;
            }
            
            if (_projectId is null) throw new ArgumentNullException(message: "ProjectId has not been set", null);
            if (_region is null) throw new ArgumentNullException(message: "Region has not been set", null);

            _generativeModel = new GenerativeModel(_projectId,
                _region,
                model,
                _endpointId,
                generationConfig,
                safetySettings,
                tools,
                systemInstruction);
            return _generativeModel;
        }

        /// <summary>
        /// Create a generative model on Vertex AI to use.
        /// </summary>
        /// <param name="cachedContent">Content that has been preprocessed.</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <returns>Generative model instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="cachedContent"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when "projectId" or "region" is <see langword="null"/>.</exception>
        public GenerativeModel GenerativeModel(CachedContent cachedContent,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null)
        {
            if (cachedContent == null) throw new ArgumentNullException(nameof(cachedContent));
            if (_projectId is null) throw new ArgumentNullException(message: "ProjectId has not been set", null);
            if (_region is null) throw new ArgumentNullException(message: "Region has not been set", null);

            _generativeModel = new GenerativeModel(cachedContent,
                generationConfig,
                safetySettings)
            {
                ProjectId = _projectId,
                Region = _region,
            };
            return _generativeModel;
        }

        /// <summary>
        /// Create a generative model on Vertex AI to use.
        /// </summary>
        /// <param name="tuningJob">Tuning Job to use with the model.</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <returns>Generative model instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="tuningJob"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when "projectId" or "region" is <see langword="null"/>.</exception>
        public GenerativeModel GenerativeModel(TuningJob tuningJob,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null)
        {
            if (tuningJob == null) throw new ArgumentNullException(nameof(tuningJob));
            if (_projectId is null) throw new ArgumentNullException(message: "ProjectId has not been set", null);
            if (_region is null) throw new ArgumentNullException(message: "Region has not been set", null);

            _generativeModel = new GenerativeModel(tuningJob,
                generationConfig,
                safetySettings)
            {
                ProjectId = _projectId, 
                Region = _region,
            };
            return _generativeModel;
        }

        /// <inheritdoc cref="IGenerativeAI"/>
        public Task<ModelResponse> GetModel(string model)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">Model to use.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when "projectId" or "region" is <see langword="null"/>.</exception>
        public SupervisedTuningJobModel SupervisedTuningJob(string model = Model.Gemini15Pro002)
        {
            if (_projectId is null) throw new ArgumentNullException(message: "ProjectId has not been set", null);
            if (_region is null) throw new ArgumentNullException(message: "Region has not been set", null);

            _tuningJobModel = new SupervisedTuningJobModel(_projectId, _region, model);
            return _tuningJobModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">Model to use (default: "imagegeneration")</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when "projectId" or "region" is <see langword="null"/>.</exception>
        public ImageGenerationModel ImageGenerationModel(string model = Model.ImageGeneration)
        {
            if (_projectId is null) throw new ArgumentNullException(message: "ProjectId has not been set", null);
            if (_region is null) throw new ArgumentNullException(message: "Region has not been set", null);

            _imageGenerationModel = new ImageGenerationModel(_projectId, _region, model);
            return _imageGenerationModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">Model to use (default: "imagetext")</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when "projectId" or "region" is <see langword="null"/>.</exception>
        public ImageTextModel ImageTextModel(string model = Model.ImageText)
        {
            if (_projectId is null) throw new ArgumentNullException(message: "ProjectId has not been set", null);
            if (_region is null) throw new ArgumentNullException(message: "Region has not been set", null);

            _imageTextModel = new ImageTextModel(_projectId, _region, model);
            return _imageTextModel;
        }
    }
}
