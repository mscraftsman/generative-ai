namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class RagVectorDbConfig
    {
        public string RagManagedDb { get; set; }
        /// <summary>
        /// Specifies your Weaviate instance.
        /// </summary>
        public Weaviate Weaviate { get; set; }
        /// <summary>
        /// Specifies your Pinecone instance.
        /// </summary>
        public Pinecone Pinecone { get; set; }
        /// <summary>
        /// Specifies your Vertex AI Feature Store instance.
        /// </summary>
        public VertexFeatureStore VertexFeatureStore { get; set; }
        /// <summary>
        /// Specifies your Vertex Vector Search instance.
        /// </summary>
        public VertexVectorSearch VertexVectorSearch { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ApiAuth ApiAuth { get; set; }
        /// <summary>
        /// The embedding model to use for the RAG corpus.
        /// </summary>
        public RagEmbeddingModelConfig RagEmbeddingModelConfig { get; set; }

        public RagVectorDbConfig(RagEmbeddingModelConfig ragRagEmbeddingModelConfig)
        {
            RagEmbeddingModelConfig = ragRagEmbeddingModelConfig;
        }
    }
}