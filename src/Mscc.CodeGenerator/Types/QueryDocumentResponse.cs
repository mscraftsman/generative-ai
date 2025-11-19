namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Response from <see cref="QueryDocument"/> containing a list of relevant chunks.
	/// </summary>
	public partial class QueryDocumentResponse
	{
		/// <summary>
		/// The returned relevant chunks.
		/// </summary>
		public List<RelevantChunk>? RelevantChunks { get; set; }
    }
}
