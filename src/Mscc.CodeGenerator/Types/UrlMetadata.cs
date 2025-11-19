namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Context of the a single url retrieval.
	/// </summary>
	public partial class UrlMetadata
	{
		/// <summary>
		/// Retrieved url by the tool.
		/// </summary>
		public string? RetrievedUrl { get; set; }
		/// <summary>
		/// Status of the url retrieval.
		/// </summary>
		public UrlRetrievalStatus? UrlRetrievalStatus { get; set; }
    }
}
