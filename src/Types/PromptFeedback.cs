using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    public class PromptFeedback
    {
        /// <summary>
        /// Output only. Blocked reason.
        /// </summary>
        public BlockedReason BlockReason { get; set; }
        /// <summary>
        /// Output only. Safety ratings.
        /// </summary>
        public List<SafetyRating>? SafetyRatings { get; set; }
        /// <summary>
        /// Output only. A readable block reason message.
        /// </summary>
        public string BlockReasonMessage { get; set; } = default;
    }
}
