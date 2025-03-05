#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.Logging;
using System.Text;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Generates a video from the model given an input.
    /// </summary>
    public sealed class VideoGenerationModel : BaseModel
    {
        private const string UrlGoogleAi = "{BaseUrlGoogleAi}/{model}:{method}";
        private const string UrlVertexAi = "{BaseUrlVertexAi}/publishers/{publisher}/{model}:{method}";

        private readonly bool _useVertexAi;

        internal override string Version => _useVertexAi ? ApiVersion.V1 : ApiVersion.V1Beta;

        private string Url => _useVertexAi ? UrlVertexAi : UrlGoogleAi;
        
        private string Method => GenerativeAI.Method.Predict;
        
        internal bool IsVertexAI => _useVertexAi;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoGenerationModel"/> class.
        /// </summary>
        public VideoGenerationModel() : this(logger: null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoGenerationModel"/> class.
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public VideoGenerationModel(ILogger? logger) : base(logger) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoGenerationModel"/> class with access to Google AI Gemini API.
        /// </summary>
        /// <param name="apiKey">API key provided by Google AI Studio</param>
        /// <param name="model">Model to use</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        internal VideoGenerationModel(string? apiKey = null,
            string? model = null, 
            ILogger? logger = null) : this(logger)
        {
            ApiKey = apiKey ?? _apiKey;
            Model = model ?? _model;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoGenerationModel"/> class with access to Vertex AI Gemini API.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project</param>
        /// <param name="region">Region to use</param>
        /// <param name="model">Model to use</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        public VideoGenerationModel(string? projectId = null, string? region = null,
            string? model = null, ILogger? logger = null) : base(projectId, region, model, logger)
        {
            _useVertexAi = true;
        }

        /// <summary>
        /// Generates videos from the specified <see cref="VideoGenerationRequest"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<GenerateVideosResponse> GenerateVideos(GenerateVideosRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var url = ParseUrl(Url, Method);
            var json = Serialize(request);
            var payload = new StringContent(json, Encoding.UTF8, Constants.MediaType);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = payload;
            var response = await SendAsync(httpRequest, cancellationToken);
            await response.EnsureSuccessAsync();
            return await Deserialize<GenerateVideosResponse>(response);
        }

        /// <summary>
        /// Generates images from text prompt.
        /// </summary>
        /// <param name="prompt">Required. String to process.</param>
        /// <param name="numberOfVideos">Number of images to generate. Range: 1..8.</param>
        /// <param name="negativePrompt">A description of what you want to omit in the generated images.</param>
        /// <param name="aspectRatio">Aspect ratio for the image.</param>
        /// <param name="safetyFilterLevel">Adds a filter level to Safety filtering.</param>
        /// <param name="personGeneration">Allow generation of people by the model.</param>
        /// <param name="enhancePrompt">Option to enhance your provided prompt.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="prompt"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<GenerateVideosResponse> GenerateVideos(string prompt,
            int numberOfVideos = 1, string? negativePrompt = null, 
            string? aspectRatio = null, string? safetyFilterLevel = null,
            PersonGeneration? personGeneration = null, bool? enhancePrompt = null,
            CancellationToken cancellationToken = default)
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));

            var request = new GenerateVideosRequest(prompt, numberOfVideos);
            if (!string.IsNullOrEmpty(aspectRatio))
            {
//                if (!AspectRatio.Contains(aspectRatio)) 
//                    throw new ArgumentException("Not a valid aspect ratio", nameof(aspectRatio));
//                request.Parameters.AspectRatio = aspectRatio;
            }
            request.Parameters.NegativePrompt ??= negativePrompt;
            if (!string.IsNullOrEmpty(safetyFilterLevel))
            { 
//              if (!SafetyFilterLevel.Contains(safetyFilterLevel.ToUpperInvariant()))
//                    throw new ArgumentException("Not a valid safety filter level", nameof(safetyFilterLevel));
//                request.Parameters.SafetyFilterLevel = safetyFilterLevel.ToUpperInvariant();
            }
            if (personGeneration is not null)
            {
                request.Parameters.PersonGeneration = personGeneration;
            }
            request.Parameters.EnhancePrompt = enhancePrompt;
            
            return await GenerateVideos(request, cancellationToken);
        }

        /// <summary>
        /// Generates a response from the model given an input prompt and other parameters.
        /// </summary>
        /// <param name="prompt">Required. String to process.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Response from the model for generated content.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="prompt"/> is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request fails to execute.</exception>
        public async Task<GenerateVideosResponse> GenerateContent(string prompt,
            CancellationToken cancellationToken = default)
        {
            return await GenerateVideos(prompt, cancellationToken: cancellationToken);
        }
    }
}