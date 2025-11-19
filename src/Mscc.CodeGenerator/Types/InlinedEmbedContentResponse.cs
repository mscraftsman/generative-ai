namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The response to a single request in the batch.
	/// </summary>
	public partial class InlinedEmbedContentResponse
	{
		/// <summary>
		/// Output only. The error encountered while processing the request.
		/// </summary>
		public Status? Error { get; set; }
		/// <summary>
		/// Output only. The metadata associated with the request.
		/// </summary>
		public object? Metadata { get; set; }
		/// <summary>
		/// Output only. The response to the request.
		/// </summary>
		public EmbedContentResponse? Response { get; set; }
    }
}
