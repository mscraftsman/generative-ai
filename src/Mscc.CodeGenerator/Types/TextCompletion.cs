namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Output text returned from a model.
	/// </summary>
	public partial class TextCompletion
	{
		/// <summary>
		/// Output only. Citation information for model-generated <see cref="output"/> in this <see cref="TextCompletion"/>. This field may be populated with attribution information for any text included in the <see cref="output"/>.
		/// </summary>
		public CitationMetadata? CitationMetadata { get; set; }
		/// <summary>
		/// Output only. The generated text returned from the model.
		/// </summary>
		public string? Output { get; set; }
		/// <summary>
		/// Ratings for the safety of a response. There is at most one rating per category.
		/// </summary>
		public List<SafetyRating>? SafetyRatings { get; set; }
    }
}
