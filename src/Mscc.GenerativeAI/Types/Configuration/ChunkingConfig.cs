namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Parameters for telling the service how to chunk the file.
    /// inspired by google3/cloud/ai/platform/extension/lib/retrieval/config/chunker_config.proto
    /// </summary>
    public class ChunkingConfig
    {
        /// <summary>
        /// White space chunking configuration. 
        /// </summary>
        public WhiteSpaceConfig? WhiteSpaceConfig { get; set; }
        /// <summary>
        /// Number of tokens each chunk should have.
        /// </summary>
        public int? ChunkSize { get; set; }
        /// <summary>
        /// Number of tokens overlap between chunks.
        /// </summary>
        public int? ChunkOverlap { get; set; }
    }
}