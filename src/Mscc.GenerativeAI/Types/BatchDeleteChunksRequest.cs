namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Request to batch delete <see cref="Chunk"/>s.
	/// </summary>
	public partial class BatchDeleteChunksRequest
	{
		/// <summary>
		/// Required. The request messages specifying the <see cref="Chunk"/>s to delete.
		/// </summary>
		public List<DeleteChunkRequest>? Requests { get; set; }
    }
}
