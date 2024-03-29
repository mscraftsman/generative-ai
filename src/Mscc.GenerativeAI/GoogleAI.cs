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
    public sealed class GoogleAI : IGenerativeAI
    {
        private readonly string? _apiKey;
        private readonly string? _accessToken;
        private GenerativeModel? _generativeModel;

        /// <summary>
        /// Default constructor attempts to read <c>.env</c> file and environment variables.
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
        /// Initialize access to Google AI Gemini API.
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
        /// <param name="model">Model to use (default: "gemini-1.0-pro")</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <exception cref="ArgumentNullException">Thrown when either API key or access token is null.</exception>
        /// <returns>Generative model instance.</returns>
        public GenerativeModel GenerativeModel(string model = Model.Gemini10Pro,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null)
        {
            if (_apiKey is null && _accessToken is null) 
                throw new ArgumentNullException("apiKey or accessToken", 
                    message: "Either API key or access token is required.");
            
            _generativeModel = new GenerativeModel(_apiKey, model, generationConfig, safetySettings);
            if (_apiKey is null)
            {
                _generativeModel.AccessToken = _accessToken;
            }

            return _generativeModel;
        }
    }
}
