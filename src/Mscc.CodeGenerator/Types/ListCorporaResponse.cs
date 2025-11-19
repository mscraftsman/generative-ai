namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Response from <see cref="ListCorpora"/> containing a paginated list of <see cref="Corpora"/>. The results are sorted by ascending <see cref="corpus.create_time"/>.
	/// </summary>
	public partial class ListCorporaResponse
	{
		/// <summary>
		/// The returned corpora.
		/// </summary>
		public List<Corpus>? Corpora { get; set; }
		/// <summary>
		/// A token, which can be sent as <see cref="page_token"/> to retrieve the next page. If this field is omitted, there are no more pages.
		/// </summary>
		public string? NextPageToken { get; set; }
    }
}
