using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Metadata returned when the model uses the `url_context` tool to get information from a
    /// user-provided URL.
    /// </summary>
    public class UrlContextMetadata
    {
        /// <summary>
        /// Output only. A list of URL metadata, with one entry for each URL retrieved by the tool.
        /// </summary>
        public List<UrlMetadata>? UrlMetadata { get; set; } 
    }
}