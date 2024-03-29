#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
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
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>Generative model instance.</returns>
        public GenerativeModel GenerativeModel(string model = Model.Gemini10Pro,
            GenerationConfig? generationConfig = null,
            List<SafetySetting>? safetySettings = null);
    }
}