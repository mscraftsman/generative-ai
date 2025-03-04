using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Candidates with top log probabilities at each decoding step.
    /// </summary>
    public class TopCandidates
    {
        /// <summary>
        /// Sorted by log probability in descending order.
        /// </summary>
        public List<LogprobsResultCandidate> Candidates { get; set; }
    }
}