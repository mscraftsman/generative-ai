using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The response to a EmbedTextRequest.
    /// </summary>
    public class EmbedTextResponse
    {
        /// <summary>
        /// Output only. The embedding generated from the input text.
        /// </summary>
        public Embedding? Embedding { get; set; }
        /// <summary>
        /// Output only. The embeddings generated from the input text.
        /// </summary>
        public List<Embedding>? Embeddings { get; set; }
    }
}