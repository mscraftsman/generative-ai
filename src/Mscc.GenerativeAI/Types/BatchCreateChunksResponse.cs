namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Response from <see cref="BatchCreateChunks"/> containing a list of created <see cref="Chunk"/>s.
	/// </summary>
	public partial class BatchCreateChunksResponse
	{
		/// <summary>
		/// <see cref="Chunk"/>s created.
		/// </summary>
		public List<Chunk>? Chunks { get; set; }
    }
}
