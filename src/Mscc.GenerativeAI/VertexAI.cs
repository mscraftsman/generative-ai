using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    public class VertexAI
    {
        private readonly string projectId;
        private readonly string region;

        /// <summary>
        /// Constructor to initialize access to Vertex AI Gemini API.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project</param>
        /// <param name="region">Region to use</param>
        public VertexAI(string projectId, string region)
        {
            this.projectId = projectId;
            this.region = region;
        }

        /// <summary>
        /// Create a generative model on Vertex AI to use.
        /// </summary>
        /// <param name="model">Model to use (default: "gemini-1.0-pro")</param>
        /// <param name="generationConfig"></param>
        /// <param name="safetySettings"></param>
        /// <returns></returns>
        public GenerativeModel GenerativeModel(string model = Model.Gemini10Pro,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null)
        {
            return new GenerativeModel(projectId, region, model, generationConfig, safetySettings);
        }
    }
}
