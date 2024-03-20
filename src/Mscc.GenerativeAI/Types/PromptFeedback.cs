#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A set of the feedback metadata the prompt specified in GenerateContentRequest.content.
    /// </summary>
    public class PromptFeedback
    {
        /// <summary>
        /// Output only. Optional. If set, the prompt was blocked and no candidates are returned. Rephrase your prompt.
        /// </summary>
        public BlockedReason BlockReason { get; set; }
        /// <summary>
        /// Output only. Ratings for safety of the prompt. There is at most one rating per category.
        /// </summary>
        public List<SafetyRating>? SafetyRatings { get; set; }
        /// <summary>
        /// Output only. A readable block reason message.
        /// </summary>
        public string BlockReasonMessage { get; set; } = default;
    }
}
