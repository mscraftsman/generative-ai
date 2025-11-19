namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Request for an <see cref="AsyncBatchEmbedContent"/> operation.
	/// </summary>
	public partial class AsyncBatchEmbedContentRequest
	{
		/// <summary>
		/// Required. The batch to create.
		/// </summary>
		public EmbedContentBatch? Batch { get; set; }
    }
}
