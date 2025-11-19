using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A list of the top candidate tokens and their log probabilities at each decoding step. This can
    /// be used to see what other tokens the model considered.
    /// </summary>
    public class LogprobsResultTopCandidates
    {
        /// <summary>
        /// The list of candidate tokens, sorted by log probability in descending order.
        /// </summary>
        public List<LogprobsResultCandidate>? Candidates { get; set; }
    }
}