namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The semantic retrieval resource to retrieve from.
    /// </summary>
    public class RetrievalResource
    {
        /// <summary>
        /// Required. The name of the semantic retrieval resource to retrieve from. Example: `ragStores/my-rag-store-123`
        /// </summary>
        public string RagStoreName { get; set; }
    }
}