namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The response to an <see cref="EmbedContentRequest"/>.
	/// </summary>
	public partial class EmbedContentResponse
	{
		/// <summary>
		/// Output only. The embedding generated from the input content.
		/// </summary>
		public ContentEmbedding? Embedding { get; set; }
    }
}
