namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Content filtering metadata associated with processing a single request.
    /// Ref: https://ai.google.dev/api/rest/v1beta/ContentFilter
    /// </summary>
    public class ContentFilter
    {
        /// <summary>
        /// Output only. The reason content was blocked during request processing.
        /// </summary>
        public BlockedReason BlockReason { get; set; }
        /// <summary>
        /// A string that describes the filtering behavior in more detail.
        /// </summary>
        public string Message { get; set; }
    }
}