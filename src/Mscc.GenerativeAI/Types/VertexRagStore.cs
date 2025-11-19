using System;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Retrieve from Vertex RAG Store for grounding.
    /// </summary>
    public class VertexRagStore
    {
        /// <summary>
        /// 
        /// </summary>
        public string[] RagCorpora { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public RagResource[] RagResources { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public RagRetrievalConfig? RagRetrievalConfig { get; set; }
        /// <summary>
        /// The number of top contexts to retrieve.
        /// </summary>
        [Obsolete("This item is deprecated!")]
        public int? SimilarityTopK { get; set; }
        /// <summary>
        /// Only contexts with a vector distance smaller than the threshold are returned.
        /// </summary>
        [Obsolete("This item is deprecated!")]
        public float? VectorDistanceThreshold { get; set; }
    }
}