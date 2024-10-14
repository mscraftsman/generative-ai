namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Metadata on the usage of the cached content.
    /// </summary>
    public class CachedContentUsageMetadata
    {
        /// <summary>
        /// Total number of tokens that the cached content consumes.
        /// </summary>
        public int TotalTokenCount { get; set; }
    }
}