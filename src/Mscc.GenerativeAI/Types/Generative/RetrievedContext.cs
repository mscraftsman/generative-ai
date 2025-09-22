namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Chunk from context retrieved by the retrieval tools.
    /// </summary>
    public class RetrievedContext
    {
        /// <summary>
        /// Output only. The full document name for the referenced Vertex AI Search document.
        /// </summary>
        public string? DocumentName { get; set; }
        /// <summary>
        /// Additional context for the RAG retrieval result. This is only populated when using the RAG retrieval tool.
        /// </summary>
        public RagChunk? RagChunk { get; set; }
        /// <summary>
        /// Text of the attribution.
        /// </summary>
        public string? Text { get; set; }
        /// <summary>
        /// Title of the attribution.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// URI reference of the attribution.
        /// </summary>
        public string? Uri { get; set; }
    }
}