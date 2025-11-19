namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The response to a EmbedTextRequest.
	/// </summary>
	public partial class BatchEmbedTextResponse
	{
		/// <summary>
		/// Output only. The embeddings generated from the input text.
		/// </summary>
		public List<Embedding>? Embeddings { get; set; }
    }
}
