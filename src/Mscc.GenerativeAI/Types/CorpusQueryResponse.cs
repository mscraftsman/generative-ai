using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Response from `QueryCorpus` containing a list of relevant chunks.
    /// </summary>
    public class CorpusQueryResponse
    {
        /// <summary>
        /// The relevant chunks.
        /// </summary>
        public List<RelevantChunk> RelevantChunks { get; set; }
    }
}