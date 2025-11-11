using System.Collections.Generic;

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