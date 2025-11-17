namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Context retrieved from a data source to ground the model's response. This is used when a
    /// retrieval tool fetches information from a user-provided corpus or a public dataset. This data
    /// type is not supported in Gemini API.
    /// </summary>
    public class RetrievedContext
    {
        /// <summary>
        /// Output only. The full document name for the referenced Vertex AI Search document. This is
        /// used to identify the specific document that was retrieved. The format is
        /// `projects/{project}/locations/{location}/collections/{collection}/dataStores/{data_store}/branches/{branch}/documents/{document}`.
        /// </summary>
        public string? DocumentName { get; set; }
        /// <summary>
        /// Additional context for a Retrieval-Augmented Generation (RAG) retrieval result. This is
        /// populated only when the RAG retrieval tool is used.
        /// </summary>
        public RagChunk? RagChunk { get; set; }
        /// <summary>
        /// The content of the retrieved data source.
        /// </summary>
        public string? Text { get; set; }
        /// <summary>
        /// The title of the retrieved data source.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// The URI of the retrieved data source.
        /// </summary>
        public string? Uri { get; set; }
        /// <summary>
        /// Optional. Name of the `RagStore` containing the document. Example: `ragStores/123`
        /// </summary>
        public string? RagStore { get; set; }
    }
}