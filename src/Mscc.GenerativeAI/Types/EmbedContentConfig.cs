namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Optional parameters for the embed_content method.
    /// </summary>
    public partial class EmbedContentConfig : BaseConfig
    {
        /// <summary>
        /// Vertex API only. Whether to silently truncate inputs longer than the max sequence length. If this option is set to false, oversized inputs will lead to an INVALID_ARGUMENT error, similar to other text APIs.
        /// </summary>
        public bool? AutoTruncate { get; set; }
        /// <summary>
        /// Vertex API only. The MIME type of the input.
        /// </summary>
        public string? MimeType { get; set; }
        /// <summary>
        /// Reduced dimension for the output embedding. If set, excessive values in the output embedding are truncated from the end. Supported by newer models since 2024 only. You cannot set this value if using the earlier model (models/embedding-001).
        /// </summary>
        public int? OutputDimensionality { get; set; }
        /// <summary>
        /// Type of task for which the embedding will be used.
        /// </summary>
        public string? TaskType { get; set; }
        /// <summary>
        /// Title for the text. Only applicable when TaskType is RETRIEVAL_DOCUMENT.
        /// </summary>
        public string? Title { get; set; }
    }
}