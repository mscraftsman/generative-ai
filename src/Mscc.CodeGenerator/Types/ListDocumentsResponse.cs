namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Response from <see cref="ListDocuments"/> containing a paginated list of <see cref="Document"/>s. The <see cref="Document"/>s are sorted by ascending <see cref="document.create_time"/>.
	/// </summary>
	public partial class ListDocumentsResponse
	{
		/// <summary>
		/// The returned <see cref="Document"/>s.
		/// </summary>
		public List<Document>? Documents { get; set; }
		/// <summary>
		/// A token, which can be sent as <see cref="page_token"/> to retrieve the next page. If this field is omitted, there are no more pages.
		/// </summary>
		public string? NextPageToken { get; set; }
    }
}
