#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

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