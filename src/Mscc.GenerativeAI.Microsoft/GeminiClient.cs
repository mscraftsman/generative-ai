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
        /// <param name="accessToken">Access token for the Google Cloud project.</param>
        public GeminiClient(string projectId, 
	        string? region = null,
            string? accessToken = null
	        )
        {
            _genAi = new VertexAI(projectId: projectId, region: region, accessToken: accessToken);
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