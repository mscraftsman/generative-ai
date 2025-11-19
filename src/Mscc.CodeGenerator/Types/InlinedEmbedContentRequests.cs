namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The requests to be processed in the batch if provided as part of the batch creation request.
	/// </summary>
	public partial class InlinedEmbedContentRequests
	{
		/// <summary>
		/// Required. The requests to be processed in the batch.
		/// </summary>
		public List<InlinedEmbedContentRequest>? Requests { get; set; }
    }
}
