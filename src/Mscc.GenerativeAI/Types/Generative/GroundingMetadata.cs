#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Metadata returned to client when grounding is enabled.
    /// </summary>
    public class GroundingMetadata
    {
        /// <summary>
        /// Optional. Google search entry for the following-up web searches.
        /// </summary>
        public SearchEntryPoint? SearchEntryPoint { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<GroundingAttribution>? GroundingAttributions { get; set; }
        /// <summary>
        /// Web search queries for the following-up web search.
        /// </summary>
        public List<string>? WebSearchQueries { get; set; }
        /// <summary>
        /// List of grounding support.
        /// </summary>
        public List<GroundingSupport>? GroundingSupports { get; set; }
        /// <summary>
        /// Metadata related to retrieval in the grounding flow.
        /// </summary>
        public RetrievalMetadata? RetrievalMetadata { get; set; }
        /// <summary>
        /// List of supporting references retrieved from specified grounding source.
        /// </summary>
        public List<GroundingChunk>? GroundingChunks { get; set; }
    }
}