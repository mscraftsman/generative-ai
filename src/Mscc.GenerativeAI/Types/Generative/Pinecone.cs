namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Specifies your Pinecone instance.
    /// </summary>
    public class Pinecone
    {
        /// <summary>
        /// This is the name used to create the Pinecone index that's used with the RAG corpus.
        /// </summary>
        public string IndexName { get; set; }
    }
}