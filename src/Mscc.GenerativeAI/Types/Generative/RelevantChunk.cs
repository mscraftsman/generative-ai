namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The information for a chunk relevant to a query.
    /// </summary>
    public class RelevantChunk
    {
        /// <summary>
        /// <see cref="Chunk"/> associated with the query.
        /// </summary>
        public Chunk Chunk { get; set; }
        /// <summary>
        /// <see cref="Chunk"/> relevance to the query.
        /// </summary>
        public float ChunkRelevanceScore { get; set; }
    }
}