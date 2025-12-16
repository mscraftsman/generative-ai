namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Vertex Vector Search instance.
    /// </summary>
    public partial class VertexVectorSearch
    {
        /// <summary>
        /// This is the resource name of the Vector Search index that's used with the RAG corpus.
        /// </summary>
        /// <remarks>
        /// Format: projects/{project}/locations/{location}/indexes/{index}
        /// </remarks>
        public string Index { get; set; }
        /// <summary>
        /// The resource name of the Vector Search Index Endpoint
        /// </summary>
        /// <remarks>
        /// Format: projects/{project}/locations/{location}/indexEndpoints/{index_endpoint}
        /// </remarks>
        public string IndexEndpoint { get; set; }
    }
}