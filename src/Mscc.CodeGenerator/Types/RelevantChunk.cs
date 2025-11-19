namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The information for a chunk relevant to a query.
	/// </summary>
	public partial class RelevantChunk
	{
		/// <summary>
		/// <see cref="Chunk"/> associated with the query.
		/// </summary>
		public Chunk? Chunk { get; set; }
		/// <summary>
		/// <see cref="Chunk"/> relevance to the query.
		/// </summary>
		public double? ChunkRelevanceScore { get; set; }
		/// <summary>
		/// <see cref="Document"/> associated with the chunk.
		/// </summary>
		public Document? Document { get; set; }
    }
}
