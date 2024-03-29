#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endif

namespace Mscc.GenerativeAI
{
    public interface IGenerativeAI
    {
        /// <summary>
        /// Create a generative model to use.
        /// </summary>
        /// <param name="model">Model to use (default: "gemini-1.0-pro")</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
        /// <returns>Generative model instance.</returns>
        /// <remarks>The interface shall be used to write generic implementations using either
        /// Google AI Gemini API or Vertex AI Gemini API as backends.</remarks>
        public GenerativeModel GenerativeModel(string model = Model.Gemini10Pro,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null);

        /// <summary>
        /// Gets information about a specific Model.
        /// </summary>
        /// <param name="model">Required. The resource name of the model. This name should match a model name returned by the models.list method. Format: models/model-id or tunedModels/my-model-id</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when model parameter is null.</exception>
        /// <exception cref="NotSupportedException">Thrown when the backend does not support this method or the model.</exception>
        public Task<ModelResponse> GetModel(string model);
    }
}