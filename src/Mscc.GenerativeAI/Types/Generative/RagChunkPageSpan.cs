namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Represents where the chunk starts and ends in the document.
    /// </summary>
    public class RagChunkPageSpan
    {
        /// <summary>
        /// Page where chunk starts in the document. Inclusive. 1-indexed.
        /// </summary>
        public int? FirstPage { get; set; }
        /// <summary>
        /// Page where chunk ends in the document. Inclusive. 1-indexed.
        /// </summary>
        public int? LastPage { get; set; }
    }
}