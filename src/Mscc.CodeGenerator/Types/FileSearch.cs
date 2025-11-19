namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The FileSearch tool that retrieves knowledge from Semantic Retrieval corpora. Files are imported to Semantic Retrieval corpora using the ImportFile API.
	/// </summary>
	public partial class FileSearch
	{
		/// <summary>
		/// Required. The names of the file_search_stores to retrieve from. Example: <see cref="fileSearchStores/my-file-search-store-123"/>
		/// </summary>
		public List<string>? FileSearchStoreNames { get; set; }
		/// <summary>
		/// Optional. Metadata filter to apply to the semantic retrieval documents and chunks.
		/// </summary>
		public string? MetadataFilter { get; set; }
		/// <summary>
		/// Optional. The number of semantic retrieval chunks to retrieve.
		/// </summary>
		public int? TopK { get; set; }
    }
}
