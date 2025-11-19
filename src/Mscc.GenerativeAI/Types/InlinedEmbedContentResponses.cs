namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The responses to the requests in the batch.
	/// </summary>
	public partial class InlinedEmbedContentResponses
	{
		/// <summary>
		/// Output only. The responses to the requests in the batch.
		/// </summary>
		public List<InlinedEmbedContentResponse>? InlinedResponses { get; set; }
    }
}
