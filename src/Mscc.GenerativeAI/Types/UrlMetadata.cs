namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The metadata for a single URL retrieval.
    /// </summary>
    public class UrlMetadata
    {
        /// <summary>
        /// The URL retrieved by the tool.
        /// </summary>
        public string? RetrievedUrl { get; set; }
        /// <summary>
        /// The status of the URL retrieval.
        /// </summary>
        public UrlRetrievalStatus? UrlRetrievalStatus { get; set; }
    }
}