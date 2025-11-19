namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Response from <see cref="ListFileSearchStores"/> containing a paginated list of <see cref="FileSearchStores"/>. The results are sorted by ascending <see cref="file_search_store.create_time"/>.
	/// </summary>
	public partial class ListFileSearchStoresResponse
	{
		/// <summary>
		/// The returned rag_stores.
		/// </summary>
		public List<FileSearchStore>? FileSearchStores { get; set; }
		/// <summary>
		/// A token, which can be sent as <see cref="page_token"/> to retrieve the next page. If this field is omitted, there are no more pages.
		/// </summary>
		public string? NextPageToken { get; set; }
    }
}
