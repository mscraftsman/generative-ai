namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A query to retrieve relevant contexts.
    /// </summary>
    public class RagQuery
    {
        /// <summary>
        /// The query in text format to get relevant contexts.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Optional. The retrieval configuration for the query.
        /// </summary>
        public RagRetrievalConfig? RagRetrievalConfig { get; set; }
        /// <summary>
        /// Optional. The number of top contexts to retrieve.
        /// </summary>
        public float? SimilarityTopK { get; set; }
    }
}