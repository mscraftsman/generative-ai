#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Entry point to access Gemini API running in Google AI.
    /// </summary>
    public sealed class GoogleAI
    {
        private readonly string? _apiKey;
        private readonly string? _accessToken;
        private readonly string? _projectId;

        /// <summary>
        /// Default constructor attempts to read environment variables and
        /// sets default values, if available
        /// </summary>
        private GoogleAI()
        {
            GenerativeModelExtensions.ReadDotEnv();

            _apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");
            _accessToken = Environment.GetEnvironmentVariable("GOOGLE_ACCESS_TOKEN");
        }

        /// <summary>
        /// Constructor to initialize access to Google AI Gemini API.
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
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public GenerativeModel GenerativeModel(string model = Model.Gemini10Pro,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null)
        {
            if (_apiKey is null && _accessToken is null) 
                throw new ArgumentNullException("apiKey or accessToken", 
                    message: "Either API key or access token is required.");
            
            var generativeModel = new GenerativeModel(_apiKey, model, generationConfig, safetySettings);
            if (_apiKey is null)
            {
                generativeModel.AccessToken = _accessToken;
            }

            return generativeModel;
        }
    }
}
