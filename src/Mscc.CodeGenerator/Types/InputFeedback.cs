namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Feedback related to the input data used to answer the question, as opposed to the model-generated response to the question.
	/// </summary>
	public partial class InputFeedback
	{
		/// <summary>
		/// Optional. If set, the input was blocked and no candidates are returned. Rephrase the input.
		/// </summary>
		public BlockReason? BlockReason { get; set; }
		/// <summary>
		/// Ratings for safety of the input. There is at most one rating per category.
		/// </summary>
		public List<SafetyRating>? SafetyRatings { get; set; }
    }
}
