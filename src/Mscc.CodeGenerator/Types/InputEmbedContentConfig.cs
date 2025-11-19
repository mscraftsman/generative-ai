namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Configures the input to the batch request.
	/// </summary>
	public partial class InputEmbedContentConfig
	{
		/// <summary>
		/// The name of the <see cref="File"/> containing the input requests.
		/// </summary>
		public string? FileName { get; set; }
		/// <summary>
		/// The requests to be processed in the batch.
		/// </summary>
		public InlinedEmbedContentRequests? Requests { get; set; }
    }
}
