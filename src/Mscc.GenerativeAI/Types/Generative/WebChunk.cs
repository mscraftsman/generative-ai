namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Chunk from the web.
    /// </summary>
    public sealed class WebChunk
    {
        /// <summary>
        /// URI reference of the chunk.
        /// </summary>
        public string? Uri { get; set; }
        /// <summary>
        /// Title of the chunk.
        /// </summary>
        public string? Title { get; set; }
    }
}