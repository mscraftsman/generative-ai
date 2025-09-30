#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Response from `QueryDocument` containing a list of relevant chunks.
    /// </summary>
    public class QueryDocumentResponse
    {
        /// <summary>
        /// The returned relevant chunks.
        /// </summary>
        public List<RelevantChunk>? RelevantChunks { get; set; }
    }
}