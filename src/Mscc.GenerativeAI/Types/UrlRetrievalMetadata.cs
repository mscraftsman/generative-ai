using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Metadata related to url context retrieval tool.
    /// </summary>
    public partial class UrlRetrievalMetadata
    {
        /// <summary>
        /// List of url retrieval contexts.
        /// </summary>
        public List<UrlRetrievalContext> UrlRetrievalContexts { get; set; }
    }
}