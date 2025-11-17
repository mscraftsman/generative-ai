using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Grounding chunk.
    /// </summary>
    public sealed class GroundingChunk
    {
        /// <summary>
        /// A grounding chunk from a web page, typically from Google Search. See the `Web` message for
        /// details.
        /// </summary>
        public WebChunk? Web { get; set; }
        /// <summary>
        /// A grounding chunk from Google Maps. See the `Maps` message for details.
        /// </summary>
        public GoogleMapsChunk? Maps { get; set; }
        /// <summary>
        /// Optional. A grounding chunk from a data source retrieved by a retrieval tool, such as Vertex AI
        /// Search. See the <see cref="RetrievedContext"/> message for details
        /// </summary>
        public RetrievedContext? RetrievedContext { get; set;}
    }
}