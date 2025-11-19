namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Response with CachedContents list.
	/// </summary>
	public partial class ListCachedContentsResponse
	{
		/// <summary>
		/// List of cached contents.
		/// </summary>
		public List<CachedContent>? CachedContents { get; set; }
		/// <summary>
		/// A token, which can be sent as <see cref="page_token"/> to retrieve the next page. If this field is omitted, there are no subsequent pages.
		/// </summary>
		public string? NextPageToken { get; set; }
    }
}
