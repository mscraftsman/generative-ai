namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The request to be processed in the batch.
	/// </summary>
	public partial class InlinedRequest
	{
		/// <summary>
		/// Optional. The metadata to be associated with the request.
		/// </summary>
		public object? Metadata { get; set; }
		/// <summary>
		/// Required. The request to be processed in the batch.
		/// </summary>
		public GenerateContentRequest? Request { get; set; }
    }
}
