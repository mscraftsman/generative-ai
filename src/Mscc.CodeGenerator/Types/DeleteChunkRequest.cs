namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Request to delete a <see cref="Chunk"/>.
	/// </summary>
	public partial class DeleteChunkRequest
	{
		/// <summary>
		/// Required. The resource name of the <see cref="Chunk"/> to delete. Example: <see cref="corpora/my-corpus-123/documents/the-doc-abc/chunks/some-chunk"/>
		/// </summary>
		public string? Name { get; set; }
    }
}
