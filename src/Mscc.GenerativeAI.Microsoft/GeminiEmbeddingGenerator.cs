#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#endif
using Microsoft.Extensions.AI;

namespace Mscc.GenerativeAI.Microsoft
{
    public class GeminiEmbeddingGenerator : IEmbeddingGenerator<string, Embedding<float>>
    {
        private const string providerName = "gemini";
    
        /// <summary>
        /// Gets the Gemini model that is used to communicate with.
        /// </summary>
        private readonly GenerativeModel _client;

        /// <inheritdoc/>
        public EmbeddingGeneratorMetadata Metadata { get; }

        /// <summary>
        /// Creates an instance of the Gemini API client using Google AI.
        /// </summary>
        /// <param name="apiKey">API key provided by Google AI Studio</param>
        /// <param name="model">Model to use.</param>
        public GeminiEmbeddingGenerator(string apiKey, string model = Model.Embedding)
        {
            var genAi = new GoogleAI(apiKey);
            _client = genAi.GenerativeModel(model);
            Metadata = new(providerName, null, model);
        }

        /// <summary>
        /// Creates an instance of the Gemini API client using Vertex AI.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project.</param>
        /// <param name="region">Optional. Region to use (default: "us-central1").</param>
        /// <param name="model">Model to use.</param>
        public GeminiEmbeddingGenerator(string projectId, string? region = null, string model = Model.Embedding)
        {
            var genAi = new VertexAI(projectId: projectId, region: region);
            _client = genAi.GenerativeModel(model);
            Metadata = new(providerName, null, model);
        }

        /// <inheritdoc/>
        public async Task<GeneratedEmbeddings<Embedding<float>>> GenerateAsync(
            IEnumerable<string> values, 
            EmbeddingGenerationOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            var request = MicrosoftAi.AbstractionMapper.ToGeminiEmbedContentRequest(values, options);
            var response = await _client.EmbedContent(request);
            return MicrosoftAi.AbstractionMapper.ToGeneratedEmbeddings(request, response);
        }

        /// <inheritdoc/>
        public object? GetService(Type serviceType, object? key)
            => key is null && serviceType?.IsInstanceOfType(this) is true ? this : null;

        /// <inheritdoc/>
        public void Dispose() { }
    }
}