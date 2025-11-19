namespace Mscc.GenerativeAI
{
    public class VertexPredictionEndpoint
    {
        /// <summary>
        /// The embedding model to use for the RAG corpus.
        /// </summary>
        /// <remarks>
        /// This value can't be changed after it's set.
        /// If you leave it empty, we use `text-embedding-004` as the embedding model.
        /// </remarks>
        public string Endpoint { get; set; }
    }
}