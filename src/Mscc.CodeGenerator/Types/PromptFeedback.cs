namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A set of the feedback metadata the prompt specified in <see cref="GenerateContentRequest.content"/>.
	/// </summary>
	public partial class PromptFeedback
	{
		/// <summary>
		/// Optional. If set, the prompt was blocked and no candidates are returned. Rephrase the prompt.
		/// </summary>
		public BlockReason? BlockReason { get; set; }
		/// <summary>
		/// Ratings for safety of the prompt. There is at most one rating per category.
		/// </summary>
		public List<SafetyRating>? SafetyRatings { get; set; }
    }
}
