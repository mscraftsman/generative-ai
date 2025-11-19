namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Semantic retrieval configuration.
    /// </summary>
    public class FileSearchRetrievalConfig
    {
        /// <summary>
        /// Optional. Metadata filter to apply to the semantic retrieval documents and chunks.
        /// </summary>
        public string? MetadataFilter { get; set; }
        /// <summary>
        /// Optional. The number of semantic retrieval chunks to retrieve.
        /// </summary>
        public int? TopK { get; set; }
    }
}