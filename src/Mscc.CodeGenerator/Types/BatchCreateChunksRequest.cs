namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Request to batch create <see cref="Chunk"/>s.
	/// </summary>
	public partial class BatchCreateChunksRequest
	{
		/// <summary>
		/// Required. The request messages specifying the <see cref="Chunk"/>s to create. A maximum of 100 <see cref="Chunk"/>s can be created in a batch.
		/// </summary>
		public List<CreateChunkRequest>? Requests { get; set; }
    }
}
