namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Encapsulates a snippet of a user review that answers a question about the features of a specific place in Google Maps.
	/// </summary>
	public partial class ReviewSnippet
	{
		/// <summary>
		/// A link that corresponds to the user review on Google Maps.
		/// </summary>
		public string? GoogleMapsUri { get; set; }
		/// <summary>
		/// The ID of the review snippet.
		/// </summary>
		public string? ReviewId { get; set; }
		/// <summary>
		/// Title of the review.
		/// </summary>
		public string? Title { get; set; }
    }
}
