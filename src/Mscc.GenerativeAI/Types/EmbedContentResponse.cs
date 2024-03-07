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
        /// 
        /// </summary>
        public Embedding? Embedding { get; set; }
        public List<Embedding>? Embeddings { get; set; }
    }
}
