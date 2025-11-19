namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The response to a <see cref="BatchEmbedContentsRequest"/>.
	/// </summary>
	public partial class BatchEmbedContentsResponse
	{
		/// <summary>
		/// Output only. The embeddings for each request, in the same order as provided in the batch request.
		/// </summary>
		public List<ContentEmbedding>? Embeddings { get; set; }
    }
}
