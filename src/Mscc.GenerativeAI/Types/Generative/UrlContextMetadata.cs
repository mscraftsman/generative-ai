using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Metadata related to url context retrieval tool.
    /// </summary>
    public class UrlContextMetadata
    {
        /// <summary>
        /// List of url context.
        /// </summary>
        public List<UrlMetadata> UrlMetadata { get; set; } 
    }
}