namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A RagChunk includes the content of a chunk of a RagFile, and associated metadata.
    /// </summary>
    public class RagChunk
    {
        /// <summary>
        /// If populated, represents where the chunk starts and ends in the document.
        /// </summary>
        public RagChunkPageSpan? PageSpan { get; set; }
        /// <summary>
        /// The content of the chunk.
        /// </summary>
        public string? Text { get; set; }
    }
}