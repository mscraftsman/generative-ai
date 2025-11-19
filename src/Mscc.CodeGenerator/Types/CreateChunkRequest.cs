namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Request to create a <see cref="Chunk"/>.
	/// </summary>
	public partial class CreateChunkRequest
	{
		/// <summary>
		/// Required. The <see cref="Chunk"/> to create.
		/// </summary>
		public Chunk? Chunk { get; set; }
		/// <summary>
		/// Required. The name of the <see cref="Document"/> where this <see cref="Chunk"/> will be created. Example: <see cref="corpora/my-corpus-123/documents/the-doc-abc"/>
		/// </summary>
		public string? Parent { get; set; }
    }
}
