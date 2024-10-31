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

        public GeminiEmbeddingGenerator(string apiKey, string model = "")
        {
            var genAi = new GoogleAI(apiKey);
            _client = genAi.GenerativeModel(model);
            Metadata = new(providerName, null, model);
        }

        /// <inheritdoc/>
        public EmbeddingGeneratorMetadata Metadata { get; }

        /// <inheritdoc/>
        public TService? GetService<TService>(object? key) 
            where TService : class
            => key is null ? this as TService : null;

        /// <inheritdoc/>
        public async Task<GeneratedEmbeddings<Embedding<float>>> GenerateAsync(IEnumerable<string> values, 
            EmbeddingGenerationOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));

            var request = MicrosoftAi.AbstractionMapper.ToGeminiEmbedContentRequest(values, options);
            var response = await _client.EmbedContent(request);
            return MicrosoftAi.AbstractionMapper.ToGeneratedEmbeddings(request, response);
        }
        
        /// <inheritdoc/>
        public void Dispose()
        {
        }
    }
}