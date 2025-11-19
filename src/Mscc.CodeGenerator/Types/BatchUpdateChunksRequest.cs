namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Request to batch update <see cref="Chunk"/>s.
	/// </summary>
	public partial class BatchUpdateChunksRequest
	{
		/// <summary>
		/// Required. The request messages specifying the <see cref="Chunk"/>s to update. A maximum of 100 <see cref="Chunk"/>s can be updated in a batch.
		/// </summary>
		public List<UpdateChunkRequest>? Requests { get; set; }
    }
}
