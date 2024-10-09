#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Entry point to access Gemini API running in Google AI.
    /// </summary>
    /// <remarks>
    /// See <a href="https://ai.google.dev/api/rest">Model reference</a>.
    /// </remarks>
    public sealed class GoogleAI : IGenerativeAI
    {
        private readonly string? _apiKey;
        private readonly string? _accessToken;
        private GenerativeModel? _generativeModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleAI"/> class with access to Google AI Gemini API.
        /// The default constructor attempts to read <c>.env</c> file and environment variables.
        /// Sets default values, if available.
        /// </summary>
        /// <remarks>The following environment variables are used:
        /// <list type="table">
        /// <item><term>GOOGLE_API_KEY</term>
        /// <description>API key provided by Google AI Studio.</description></item>
        /// <item><term>GOOGLE_ACCESS_TOKEN</term>
        /// <description>Optional. Access token provided by OAuth 2.0 or Application Default Credentials (ADC).</description></item>
        /// </list>
        /// </remarks>
        private GoogleAI()
        {
            GenerativeAIExtensions.ReadDotEnv();
            _apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
            _accessToken = Environment.GetEnvironmentVariable("GOOGLE_ACCESS_TOKEN");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleAI"/> class with access to Google AI Gemini API.
        /// Either API key or access token is required.
        /// </summary>
        /// <param name="apiKey">Identifier of the Google Cloud project</param>
        /// <param name="accessToken">Access token for the Google Cloud project</param>
        public GoogleAI(string? apiKey = null, string? accessToken = null) : this()
        {
            _apiKey ??= apiKey;
            _accessToken ??= accessToken;
        }

        /// <summary>
        /// Create a generative model on Google AI to use.
        /// </summary>
        /// <param name="model">Model to use (default: "gemini-1.5-pro")</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <param name="systemInstruction">Optional. </param>
        /// <exception cref="ArgumentNullException">Thrown when both "apiKey" and "accessToken" are <see langword="null"/>.</exception>
        /// <returns>Generative model instance.</returns>
        public GenerativeModel GenerativeModel(string model = Model.Gemini15Pro,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            List<Tool>? tools = null,
            Content? systemInstruction = null)
        {
            if (_apiKey is null && _accessToken is null) 
                throw new ArgumentNullException(message: "Either API key or access token is required.", null);
            
            _generativeModel = new GenerativeModel(_apiKey,
                model,
                generationConfig,
                safetySettings,
                tools,
                systemInstruction);
            if (_apiKey is null)
            {
                _generativeModel.AccessToken = _accessToken;
            }

            return _generativeModel;
        }

        /// <inheritdoc cref="IGenerativeAI"/>
        public async Task<ModelResponse> GetModel(string model)
        {
            return await _generativeModel?.GetModel(model)!;
        }
    }
}
