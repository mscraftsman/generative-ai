using System;

namespace Mscc.GenerativeAI.Microsoft
{
    public class GeminiClient
    {
        private readonly IGenerativeAI _genAi;

        /// <summary>
        /// Creates an instance of the Gemini API client using automatic discovery of credentials.
        /// </summary>
        public GeminiClient()
        {
            _genAi = CreateGenerativeAI();
        }

        /// <summary>
        /// Creates an instance of the Gemini API client using Google AI.
        /// </summary>
        /// <param name="apiKey">API key provided by Google AI Studio</param>
        public GeminiClient(string apiKey)
        {
            _genAi = new GoogleAI(apiKey);
        }

        /// <summary>
        /// Creates an instance of the Gemini API client using Vertex AI.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project.</param>
        /// <param name="region">Optional. Region to use (default: "us-central1").</param>
        /// <param name="accessToken">Access token for the Google Cloud project.</param>
        public GeminiClient(string projectId,
            string? region = null,
            string? accessToken = null
        )
        {
            _genAi = new VertexAI(projectId: projectId, region: region, accessToken: accessToken);
        }

        /// <summary>
        /// Creates an instance of the Gemini API client using specified credentials.
        /// </summary>
        /// <param name="apiKey">Optional. API key for Google AI Studio.</param>
        /// <param name="projectId">Optional. Identifier of the Google Cloud project for Vertex AI.</param>
        /// <param name="region">Optional. Region to use (default: "us-central1").</param>
        /// <param name="accessToken">Optional. Access token for authentication.</param>
        public GeminiClient(string? apiKey = null,
            string? projectId = null,
            string? region = null,
            string? accessToken = null)
        {
            _genAi = CreateGenerativeAI(apiKey, projectId, region, accessToken);
        }

        private static IGenerativeAI CreateGenerativeAI(string? apiKey = null,
            string? projectId = null,
            string? region = null,
            string? accessToken = null)
        {
            if (!string.IsNullOrEmpty(projectId))
            {
                return new VertexAI(projectId: projectId, region: region, accessToken: accessToken);
            }

            if (!string.IsNullOrEmpty(apiKey) || !string.IsNullOrEmpty(accessToken))
            {
                return new GoogleAI(apiKey: apiKey, accessToken: accessToken);
            }

            // Fallback to automatic discovery
            apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY") ??
                     Environment.GetEnvironmentVariable("GEMINI_API_KEY");
            if (!string.IsNullOrEmpty(apiKey))
            {
                return new GoogleAI(apiKey: apiKey);
            }

            return new VertexAI(projectId: null, region: region, accessToken: accessToken);
        }

        /// <summary>
        /// Gets an instance of <see cref="GenerativeModel"/>.
        /// </summary>
        /// <param name="model">Model to use.</param>
        /// <returns>An instance of <see cref="GenerativeModel"/>.</returns>
        internal virtual GenerativeModel GetGenerativeModel(string? model)
        {
            return _genAi.GenerativeModel(model: model);
        }
    }
}