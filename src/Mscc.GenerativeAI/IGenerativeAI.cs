using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI.Types;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The interface shall be used to write generic implementations using either
    /// Google AI Gemini API or Vertex AI Gemini API as backends.
    /// </summary>
    public interface IGenerativeAI
    {
        /// <summary>
        /// Create an instance of a generative model to use.
        /// </summary>
        /// <param name="model">Model to use (default: "gemini-1.5-pro")</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="tools">Optional. A list of Tools the model may use to generate the next response.</param>
        /// <param name="systemInstruction">Optional. </param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        /// <exception cref="ArgumentNullException">Thrown when required parameters are null.</exception>
        /// <returns>Generative model instance.</returns>
        GenerativeModel GenerativeModel(string model = Model.Gemini25Pro,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            Tools? tools = null,
            Content? systemInstruction = null,
            ILogger? logger = null);

        /// <summary>
        /// Create an instance of a generative model to use.
        /// </summary>
        /// <param name="cachedContent">Content that has been preprocessed.</param>
        /// <param name="generationConfig">Optional. Configuration options for model generation and outputs.</param>
        /// <param name="safetySettings">Optional. A list of unique SafetySetting instances for blocking unsafe content.</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        /// <returns>Generative model instance.</returns>
        GenerativeModel GenerativeModel(CachedContent cachedContent,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null,
            ILogger? logger = null);
        
        /// <summary>
        /// Gets information about a specific Model.
        /// </summary>
        /// <param name="model">Required. The resource name of the model. This name should match a model name returned by the models.list method. Format: models/model-id or tunedModels/my-model-id</param>
        /// <param name="requestOptions">Options for the request.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when model parameter is null.</exception>
        /// <exception cref="NotSupportedException">Thrown when the backend does not support this method or the model.</exception>
        Task<ModelResponse> GetModel(string model, 
            RequestOptions? requestOptions = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns an instance of an image generation model.
        /// </summary>
        /// <param name="model">Model to use (default: "imagegeneration")</param>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        ImageGenerationModel ImageGenerationModel(string model,
            ILogger? logger = null);

        /// <summary>
        /// Returns an instance of <see cref="FileSearchStoresModel"/>.
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        FileSearchStoresModel FileSearchStoresModel();
        
        /// <summary>
        /// Returns an instance of <see cref="OperationsModel"/>.
        /// </summary>
        /// <param name="logger">Optional. Logger instance used for logging</param>
        OperationsModel OperationsModel();
    }
}