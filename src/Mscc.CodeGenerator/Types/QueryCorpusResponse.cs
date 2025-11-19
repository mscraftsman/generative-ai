namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Response from <see cref="QueryCorpus"/> containing a list of relevant chunks.
	/// </summary>
	public partial class QueryCorpusResponse
	{
		/// <summary>
		/// The relevant chunks.
		/// </summary>
		public List<RelevantChunk>? RelevantChunks { get; set; }
    }
}
