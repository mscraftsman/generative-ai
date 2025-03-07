namespace Mscc.GenerativeAI
{
    public class ChunkingConfig
    {
        /// <summary>
        /// Number of tokens each chunk should have.
        /// </summary>
        public int ChunkSize { get; set; }
        /// <summary>
        /// Number of tokens overlap between chunks.
        /// </summary>
        public int ChunkOverlap { get; set; }
    }
}