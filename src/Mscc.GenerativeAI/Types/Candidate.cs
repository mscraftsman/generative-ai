using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    public class Candidate
    {
        /// <summary>
        /// Output only. Content parts of the candidate.
        /// </summary>
        public ContentResponse? Content { get; set; }
        /// <summary>
        /// Output only. The reason why the model stopped generating tokens. 
        /// If empty, the model has not stopped generating the tokens.
        /// </summary>
        //public FinishReason FinishReason { get; internal set; }
        public string? FinishReason { get; set; }
        /// <summary>
        /// Output only. Describes the reason the mode stopped generating tokens 
        /// in more detail. This is only filled when `finish_reason` is set.
        /// </summary>
        public string? FinishMessage { get; set; }
        /// <summary>
        /// Output only. Index of the candidate.
        /// </summary>
        public int? Index { get; set; }
        /// <summary>
        /// Output only. List of ratings for the safety of a response candidate. 
        /// There is at most one rating per category.
        /// </summary>
        public List<SafetyRating>? SafetyRatings { get; set; }
        /// <summary>
        /// Output only. Source attribution of the generated content.
        /// </summary>
        public CitationMetadata? CitationMetadata { get; set; }

        public FunctionCall? FunctionCall { get; set; }

        public GroundingMetadata? GroundingMetadata {  get; set; }
}
}
