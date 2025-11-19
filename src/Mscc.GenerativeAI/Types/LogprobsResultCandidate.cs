using System.Diagnostics.Contracts;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A single token and its associated log probability.
    /// </summary>
    public class LogprobsResultCandidate
    {
        /// <summary>
        /// The token's string representation.
        /// </summary>
        public string? Token { get; set; }
        /// <summary>
        /// The token's numerical ID. While the `token` field provides the string representation of the
        /// token, the `token_id` is the numerical representation that the model uses internally. This
        /// can be useful for developers who want to build custom logic based on the model's vocabulary.
        /// </summary>
        public int? TokenId { get; set; }
        /// <summary>
        /// The log probability of this token. A higher value indicates that the model was more
        /// confident in this token. The log probability can be used to assess the relative likelihood
        /// of different tokens and to identify when the model is uncertain.
        /// </summary>
        public float LogProbability { get; set; }
    }
}