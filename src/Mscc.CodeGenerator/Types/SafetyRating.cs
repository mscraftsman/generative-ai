namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Safety rating for a piece of content. The safety rating contains the category of harm and the harm probability level in that category for a piece of content. Content is classified for safety across a number of harm categories and the probability of the harm classification is included here.
	/// </summary>
	public partial class SafetyRating
	{
		/// <summary>
		/// Was this content blocked because of this rating?
		/// </summary>
		public bool? Blocked { get; set; }
		/// <summary>
		/// Required. The category for this rating.
		/// </summary>
		public Category? Category { get; set; }
		/// <summary>
		/// Required. The probability of harm for this content.
		/// </summary>
		public Probability? Probability { get; set; }
    }
}
