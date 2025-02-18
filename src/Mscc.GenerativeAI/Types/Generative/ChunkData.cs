namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Extracted data that represents the `Chunk` content.
    /// </summary>
    public class ChunkData
    {
        /// <summary>
        /// The `Chunk` content as a string. The maximum number of tokens per chunk is 2043.
        /// </summary>
        public string StringValue { get; set; }
    }
}