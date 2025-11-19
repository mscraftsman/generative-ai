namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Response from <see cref="BatchUpdateChunks"/> containing a list of updated <see cref="Chunk"/>s.
	/// </summary>
	public partial class BatchUpdateChunksResponse
	{
		/// <summary>
		/// <see cref="Chunk"/>s updated.
		/// </summary>
		public List<Chunk>? Chunks { get; set; }
    }
}
