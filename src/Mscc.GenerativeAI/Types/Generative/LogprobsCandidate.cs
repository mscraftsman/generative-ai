using System.Diagnostics.Contracts;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Candidate for the logprobs token and score.
    /// </summary>
    public class LogprobsCandidate
    {
        /// <summary>
        /// The candidate’s token string value.
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// The candidate’s token id value.
        /// </summary>
        public int TokenId { get; set; }
        /// <summary>
        /// The candidate's log probability.
        /// </summary>
        public float LogProbability { get; set; }
    }
}