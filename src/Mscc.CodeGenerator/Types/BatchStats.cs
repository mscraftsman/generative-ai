namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Stats about the batch.
	/// </summary>
	public partial class BatchStats
	{
		/// <summary>
		/// Output only. The number of requests that failed to be processed.
		/// </summary>
		public long? FailedRequestCount { get; set; }
		/// <summary>
		/// Output only. The number of requests that are still pending processing.
		/// </summary>
		public long? PendingRequestCount { get; set; }
		/// <summary>
		/// Output only. The number of requests in the batch.
		/// </summary>
		public long? RequestCount { get; set; }
		/// <summary>
		/// Output only. The number of requests that were successfully processed.
		/// </summary>
		public long? SuccessfulRequestCount { get; set; }
    }
}
