#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Entry point to access Gemini API running in Vertex AI.
    /// </summary>
    public sealed class VertexAI : IGenerativeAI
    {
        private readonly string? _projectId;
        private readonly string _region = "us-central1";

        /// <summary>
        /// Default constructor attempts to read environment variables and
        /// sets default values, if available
        /// </summary>
        private VertexAI()
        {
            GenerativeModelExtensions.ReadDotEnv();

            _projectId = Environment.GetEnvironmentVariable("GOOGLE_PROJECT_ID");
            _region = Environment.GetEnvironmentVariable("GOOGLE_REGION") ?? _region;
        }
        
        /// <summary>
        /// Constructor to initialize access to Vertex AI Gemini API.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project</param>
        /// <param name="region">Region to use</param>
        public VertexAI(string projectId, string region) : this()
        {
            _projectId ??= projectId;
            _region ??= region;
        }

        /// <summary>
        /// Create a generative model on Vertex AI to use.
        /// </summary>
        /// <param name="model">Model to use (default: "gemini-1.0-pro")</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>Generative model instance.</returns>
        public GenerativeModel GenerativeModel(string model = Model.Gemini10Pro,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null)
        {
            if (_projectId is null) throw new ArgumentNullException("projectId");
            if (_region is null) throw new ArgumentNullException("region");

            return new GenerativeModel(_projectId, _region, model, generationConfig, safetySettings);
        }
    }
}
