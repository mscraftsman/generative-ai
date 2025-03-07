#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Response from `QueryCorpus` containing a list of relevant chunks.
    /// </summary>
    public class QueryCorpusResponse
    {
        /// <summary>
        /// The relevant chunks.
        /// </summary>
        public List<RelevantChunk> RelevantChunks { get; set; }
    }
}