using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A response candidate generated from the model.
    /// Ref: https://ai.google.dev/api/rest/v1beta/Candidate
    /// </summary>
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
        public FinishReason? FinishReason { get; set; }
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
        /// <summary>
        /// 
        /// </summary>
        public FunctionCall? FunctionCall { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public GroundingMetadata? GroundingMetadata {  get; set; }
        /// <summary>
        /// Output only. Token count for this candidate.
        /// </summary>
        public int? TokenCount { get; set; }
        /// <summary>
        /// Output only. Attribution information for sources that contributed to a grounded answer.
        /// This field is populated for GenerateAnswer calls.
        /// </summary>
        public List<GroundingAttribution>? GroundingAttributions { get; set; }
    }
}
