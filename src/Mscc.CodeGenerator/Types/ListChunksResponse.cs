namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Response from <see cref="ListChunks"/> containing a paginated list of <see cref="Chunk"/>s. The <see cref="Chunk"/>s are sorted by ascending <see cref="chunk.create_time"/>.
	/// </summary>
	public partial class ListChunksResponse
	{
		/// <summary>
		/// The returned <see cref="Chunk"/>s.
		/// </summary>
		public List<Chunk>? Chunks { get; set; }
		/// <summary>
		/// A token, which can be sent as <see cref="page_token"/> to retrieve the next page. If this field is omitted, there are no more pages.
		/// </summary>
		public string? NextPageToken { get; set; }
    }
}
