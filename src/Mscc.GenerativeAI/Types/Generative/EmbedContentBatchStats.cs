namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Stats about the batch.
    /// </summary>
    public class EmbedContentBatchStats
    {
        /// <summary>
        /// Output only. The number of requests in the batch.
        /// </summary>
        public int? RequestCount { get; set; }
        /// <summary>
        /// Output only. The number of requests that are still pending processing.
        /// </summary>
        public int? PendingRequestCount { get; set; }
        /// <summary>
        /// Output only. The number of requests that were successfully processed.
        /// </summary>
        public int? SuccessfulRequestCount { get; set; }
        /// <summary>
        /// Output only. The number of requests that failed to be processed.
        /// </summary>
        public int? FailedRequestCount { get; set; }
    }
}