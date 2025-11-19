namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Response from <see cref="ListModel"/> containing a paginated list of Models.
	/// </summary>
	public partial class ListModelsResponse
	{
		/// <summary>
		/// The returned Models.
		/// </summary>
		public List<Model>? Models { get; set; }
		/// <summary>
		/// A token, which can be sent as <see cref="page_token"/> to retrieve the next page. If this field is omitted, there are no more pages.
		/// </summary>
		public string? NextPageToken { get; set; }
    }
}
