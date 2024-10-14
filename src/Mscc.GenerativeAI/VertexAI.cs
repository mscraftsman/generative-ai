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
    public sealed class VertexAI : GenerationBase, IGenerativeAI
    {
        private readonly string? _projectId;
        private readonly string _region = "us-central1";
        // private readonly string? _apiEndpoint = "us-central1-aiplatform.googleapis.com";
        // private readonly GoogleAuthOptions? _googleAuthOptions;
        private GenerativeModel? _generativeModel;
        private ImageGenerationModel? _imageGenerationModel;
        private ImageTextModel? _imageTextModel;

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
        public VertexAI(string projectId, string? region = null, ILogger? logger = null) : this(logger)
        {
            _projectId ??= projectId ?? throw new ArgumentNullException(nameof(projectId));
            _region = region ?? _region;
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
            List<Tool>? tools = null,
            Content? systemInstruction = null)
        {
            if (_projectId is null) throw new ArgumentNullException(message: "ProjectId has not been set", null);
            if (_region is null) throw new ArgumentNullException(message: "Region has not been set", null);

            _generativeModel = new GenerativeModel(_projectId,
                _region,
                model,
                generationConfig,
                safetySettings,
                tools,
                systemInstruction);
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
