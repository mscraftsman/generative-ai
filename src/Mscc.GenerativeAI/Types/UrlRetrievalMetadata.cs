using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Metadata related to url context retrieval tool.
    /// </summary>
    public class UrlRetrievalMetadata
    {
        /// <summary>
        /// List of url retrieval contexts.
        /// </summary>
        public List<UrlRetrievalContext> UrlRetrievalContexts { get; set; }
    }
}