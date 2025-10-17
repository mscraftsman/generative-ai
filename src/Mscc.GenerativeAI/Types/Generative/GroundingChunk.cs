#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Grounding chunk.
    /// </summary>
    public sealed class GroundingChunk
    {
        /// <summary>
        /// Grounding chunk from the web.
        /// </summary>
        public WebChunk? Web { get; set; }
        /// <summary>
        /// Grounding chunk from Google Maps.
        /// </summary>
        public GoogleMapsChunk? Maps { get; set; }
        /// <summary>
        /// Optional. Grounding chunk from context retrieved by the file search tool.
        /// </summary>
        public RetrievedContext? RetrievedContext { get; set;}
    }
}