namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Batch request to get a text embedding from the model.
	/// </summary>
	public partial class BatchEmbedTextRequest
	{
		/// <summary>
		/// Optional. Embed requests for the batch. Only one of <see cref="texts"/> or <see cref="requests"/> can be set.
		/// </summary>
		public List<EmbedTextRequest>? Requests { get; set; }
		/// <summary>
		/// Optional. The free-form input texts that the model will turn into an embedding. The current limit is 100 texts, over which an error will be thrown.
		/// </summary>
		public List<string>? Texts { get; set; }
    }
}
