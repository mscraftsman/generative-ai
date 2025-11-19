namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Logprobs Result
	/// </summary>
	public partial class LogprobsResult
	{
		/// <summary>
		/// Length = total number of decoding steps. The chosen candidates may or may not be in top_candidates.
		/// </summary>
		public List<LogprobsResultCandidate>? ChosenCandidates { get; set; }
		/// <summary>
		/// Sum of log probabilities for all tokens.
		/// </summary>
		public double? LogProbabilitySum { get; set; }
		/// <summary>
		/// Length = total number of decoding steps.
		/// </summary>
		public List<TopCandidates>? TopCandidates { get; set; }
    }
}
