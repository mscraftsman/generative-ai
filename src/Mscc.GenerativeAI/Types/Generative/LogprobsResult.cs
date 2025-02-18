using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Logprobs Result
    /// </summary>
    public class LogprobsResult
    {
        /// <summary>
        /// Length = total number of decoding steps.
        /// </summary>
        public List<TopCandidates> TopCandidates { get; set; }
        /// <summary>
        /// Length = total number of decoding steps.
        /// The chosen candidates may or may not be in topCandidates.
        /// </summary>
        public List<LogprobsResultCandidate> ChosenCanditates { get; set; }
    }
}