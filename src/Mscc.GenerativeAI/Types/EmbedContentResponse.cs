#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif
using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class EmbedContentResponse
    {
        /// <summary>
        /// Output only. Generated candidates.
        /// </summary>
        public List<Candidate>? Candidates { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Embedding? Embedding { get; set; }
        public List<Embedding>? Embeddings { get; set; }
    }
}
