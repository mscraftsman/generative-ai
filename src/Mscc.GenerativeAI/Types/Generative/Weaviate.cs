namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Specifies your Weaviate instance.
    /// </summary>
    public class Weaviate
    {
        /// <summary>
        /// The Weaviate instance's HTTP endpoint.
        /// </summary>
        public string HttpEndpoint { get; set; }
        /// <summary>
        /// The Weaviate collection that the RAG corpus maps to.
        /// </summary>
        public string CollectionName { get; set; }
    }
}