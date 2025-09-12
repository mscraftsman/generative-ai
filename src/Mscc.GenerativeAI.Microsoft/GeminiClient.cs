namespace Mscc.GenerativeAI.Microsoft
{
    public class GeminiClient
    {
        private readonly IGenerativeAI _genAi; 

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
        public GeminiClient(string projectId, string? region = null)
        {
            _genAi = new VertexAI(projectId: projectId, region: region);
        }

        /// <summary>
        /// Gets an instance of <see cref="GenerativeModel"/>.
        /// </summary>
        /// <param name="model">Model to use.</param>
        /// <returns>An instance of <see cref="GenerativeModel"/>.</returns>
        public virtual GenerativeModel GetChatClient(string model)
        {
            return _genAi.GenerativeModel(model);
        }

        /// <summary>
        /// Gets an instance of <see cref="GenerativeModel"/>.
        /// </summary>
        /// <param name="model">Model to use.</param>
        /// <returns>An instance of <see cref="GenerativeModel"/>.</returns>
        public virtual GenerativeModel GetEmbeddingGenerator(string model)
        {
            return _genAi.GenerativeModel(model);
        }

        /// <summary>
        /// Gets an instance of <see cref="GenerativeModel"/>/
        /// </summary>
        /// <param name="model">Model to use.</param>
        /// <returns>An instance of <see cref="GenerativeModel"/>.</returns>
        public virtual GenerativeModel GetSpeechToTextClient(string model)
        {
            return _genAi.GenerativeModel(model);
        }
    }
}