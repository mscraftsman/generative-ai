namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Candidates with top log probabilities at each decoding step.
	/// </summary>
	public partial class TopCandidates
	{
		/// <summary>
		/// Sorted by log probability in descending order.
		/// </summary>
		public List<LogprobsResultCandidate>? Candidates { get; set; }
    }
}
