namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The response to a EmbedTextRequest.
	/// </summary>
	public partial class EmbedTextResponse
	{
		/// <summary>
		/// Output only. The embedding generated from the input text.
		/// </summary>
		public Embedding? Embedding { get; set; }
    }
}
