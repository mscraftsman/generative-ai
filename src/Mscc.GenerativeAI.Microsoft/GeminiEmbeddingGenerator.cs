using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;

namespace Mscc.GenerativeAI.Microsoft
{
    public sealed class GeminiEmbeddingGenerator : IEmbeddingGenerator<string, Embedding<float>>
    {
        private const string ProviderName = "gemini";
        private const string BaseUrl = "https://generativelanguage.googleapis.com/";
    
        /// <summary>
        /// Gets the Gemini model that is used to communicate with.
        /// </summary>
        private readonly GenerativeModel _client;
        /// <summary>Lazily-initialized metadata describing the implementation.</summary>
        public EmbeddingGeneratorMetadata? _metadata { get; }
        /// <summary>The default model that should be used when no override is specified.</summary>
        private readonly string? _model;

        /// <summary>
        /// Creates an instance of the <see cref="GeminiEmbeddingGenerator"/> class for the specified Gemini API client.
        /// </summary>
        /// <param name="client">The underlying client.</param>
        /// <param name="defaultModelDimensions"></param>
        /// <exception cref="ArgumentNullException">Thrown when the specified client is null.</exception>
        public GeminiEmbeddingGenerator(GenerativeModel client, int? defaultModelDimensions = null)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _model = client.Model;
        }

        /// <summary>
        /// Creates an instance of the Gemini API client using Google AI.
        /// </summary>
        /// <param name="apiKey">API key provided by Google AI Studio</param>
        /// <param name="model">Model to use.</param>
        public GeminiEmbeddingGenerator(string apiKey, string? model)
        {
            var genAi = new GoogleAI(apiKey);
            _client = genAi.GenerativeModel(model);
            _model = model ?? _client.Model;
        }

        /// <summary>
        /// Creates an instance of the Gemini API client using Vertex AI.
        /// </summary>
        /// <param name="projectId">Identifier of the Google Cloud project.</param>
        /// <param name="region">Optional. Region to use (default: "us-central1").</param>
        /// <param name="accessToken">Access token for the Google Cloud project.</param>
        /// <param name="model">Model to use.</param>
        public GeminiEmbeddingGenerator(string projectId, 
	        string? region = null, 
            string? accessToken = null,
	        string? model = null)
        {
            var genAi = new VertexAI(projectId: projectId, region: region, accessToken: accessToken);
            _client = genAi.GenerativeModel(model);
            _model = model ?? _client.Model;
        }

        /// <inheritdoc/>
        public async Task<GeneratedEmbeddings<Embedding<float>>> GenerateAsync(
            IEnumerable<string> values, 
            EmbeddingGenerationOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            var request = AbstractionMapper.ToGeminiEmbedContentRequest(values, options);
            var response = await _client.EmbedContent(request, cancellationToken: cancellationToken);
            return AbstractionMapper.ToGeneratedEmbeddings(request, response);
        }

        /// <inheritdoc/>
        public object? GetService(Type serviceType, object? serviceKey)
            => serviceKey is null && serviceType?.IsInstanceOfType(this) is true ? this : null;
        
        /// <inheritdoc />
        object? IEmbeddingGenerator.GetService(Type serviceType, object? serviceKey) =>
            serviceKey is not null ? null :
            serviceType == typeof(EmbeddingGeneratorMetadata) ? _metadata ?? new(ProviderName, new(BaseUrl), _model) :
            serviceType == typeof(GenerativeModel) ? _client :
            serviceType?.IsInstanceOfType(this) is true ? this :
            null;

        /// <inheritdoc/>
        public void Dispose() { }
    }
}