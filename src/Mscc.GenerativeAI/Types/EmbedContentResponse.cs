#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif
using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The response to an EmbedContentRequest.
    /// </summary>
    public class EmbedContentResponse
    {
        /// <summary>
        /// Output only. Generated candidates.
        /// </summary>
        public List<Candidate>? Candidates { get; set; }
        /// <summary>
        /// Output only. The embedding generated from the input content.
        /// </summary>
        public ContentEmbedding? Embedding { get; set; }
        /// <summary>
        /// Output only. The embeddings for each request, in the same order as provided in the batch request.
        /// </summary>
        public List<ContentEmbedding>? Embeddings { get; set; }
    }
}
