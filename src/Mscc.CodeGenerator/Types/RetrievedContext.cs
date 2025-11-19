namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Chunk from context retrieved by the file search tool.
	/// </summary>
	public partial class RetrievedContext
	{
		/// <summary>
		/// Optional. Name of the <see cref="FileSearchStore"/> containing the document. Example: <see cref="fileSearchStores/123"/>
		/// </summary>
		public string? FileSearchStore { get; set; }
		/// <summary>
		/// Optional. Text of the chunk.
		/// </summary>
		public string? Text { get; set; }
		/// <summary>
		/// Optional. Title of the document.
		/// </summary>
		public string? Title { get; set; }
		/// <summary>
		/// Optional. URI reference of the semantic retrieval document.
		/// </summary>
		public string? Uri { get; set; }
    }
}
