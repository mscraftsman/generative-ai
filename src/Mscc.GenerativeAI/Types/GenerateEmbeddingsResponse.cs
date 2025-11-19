using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Response for embedding generation.
    /// </summary>
    public class GenerateEmbeddingsResponse
    {
        /// <summary>
        /// Output only. Model used to generate the embeddings.
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// Output only. Always \"embedding\", required by the SDK.
        /// </summary>
        public string Object { get; set; } = "embedding";
        /// <summary>
        /// Output only. A list of the requested embeddings.
        /// </summary>
        public List<GenerateEmbeddingsEmbedding> Data { get; set; }
    }
}