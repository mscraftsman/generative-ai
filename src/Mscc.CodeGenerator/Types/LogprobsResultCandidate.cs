namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Candidate for the logprobs token and score.
	/// </summary>
	public partial class LogprobsResultCandidate
	{
		/// <summary>
		/// The candidate's log probability.
		/// </summary>
		public double? LogProbability { get; set; }
		/// <summary>
		/// The candidate’s token string value.
		/// </summary>
		public string? Token { get; set; }
		/// <summary>
		/// The candidate’s token id value.
		/// </summary>
		public int? TokenId { get; set; }
    }
}
