using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Information about the sources that support the content of a response. When grounding is
    /// enabled, the model returns citations for claims in the response. This object contains the
    /// retrieved sources.
    /// </summary>
    public sealed class GroundingMetadata
    {
        /// <summary>
        /// Optional. A web search entry point that can be used to display search results. This field is
        /// populated only when the grounding source is Google Search.
        /// </summary>
        public SearchEntryPoint? SearchEntryPoint { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<GroundingAttribution>? GroundingAttributions { get; set; }
        /// <summary>
        /// Optional. Output only. A list of URIs that can be used to flag a place or review for
        /// inappropriate content. This field is populated only when the grounding source is Google
        /// Maps. This field is not supported in Gemini API.
        /// </summary>
        public List<GroundingMetadataSourceFlaggingUri>? SourceFlaggingUris { get; set; }
        /// <summary>
        /// Optional. The web search queries that were used to generate the content. This field is
        /// populated only when the grounding source is Google Search.
        /// </summary>
        public List<string>? WebSearchQueries { get; set; }
        /// <summary>
        /// Optional. A list of grounding supports that connect the generated content to the grounding
        /// chunks. This field is populated when the grounding source is Google Search or Vertex AI
        /// Search.
        /// </summary>
        public List<GroundingSupport>? GroundingSupports { get; set; }
        /// <summary>
        /// Optional. Output only. Metadata related to the retrieval grounding source.
        /// </summary>
        public RetrievalMetadata? RetrievalMetadata { get; set; }
        /// <summary>
        /// Optional. The queries that were executed by the retrieval tools. This field is populated
        /// only when the grounding source is a retrieval tool, such as Vertex AI Search. This field is
        /// not supported in Gemini API.
        /// </summary>
        public List<string>? RetrievalQueries { get; set; }
        /// <summary>
        /// A list of supporting references retrieved from the grounding source. This field is populated
        /// when the grounding source is Google Search, Vertex AI Search, or Google Maps.
        /// </summary>
        public List<GroundingChunk>? GroundingChunks { get; set; }
        /// <summary>
        /// Optional. Output only. A token that can be used to render a Google Maps widget with the
        /// contextual data. This field is populated only when the grounding source is Google Maps.
        /// </summary>
        public string? GoogleMapsWidgetContextToken { get; set; }
    }
}