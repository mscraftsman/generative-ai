namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Batch request to get embeddings from the model for a list of prompts.
	/// </summary>
	public partial class BatchEmbedContentsRequest
	{
		/// <summary>
		/// Required. Embed requests for the batch. The model in each of these requests must match the model specified <see cref="BatchEmbedContentsRequest.model"/>.
		/// </summary>
		public List<EmbedContentRequest>? Requests { get; set; }
    }
}
