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
        /// Output only. The 0-based index of this candidate in the list of generated responses. This is
        /// useful for distinguishing between multiple candidates when `candidate_count` > 1.
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        /// Output only. A list of ratings for the safety of a response candidate. 
        /// There is at most one rating per category.
        /// </summary>
        public List<SafetyRating>? SafetyRatings { get; set; }

        /// <summary>
        /// Output only. Source attribution of the generated content.
        /// </summary>
        public CitationMetadata? CitationMetadata { get; set; }

        /// <summary>
        /// Output only. Metadata returned when grounding is enabled. It contains the sources used to
        /// ground the generated content.
        /// </summary>
        public GroundingMetadata? GroundingMetadata { get; set; }

        /// <summary>
        /// Output only. Token count for this candidate.
        /// </summary>
        public int? TokenCount { get; set; }

        /// <summary>
        /// Output only. Attribution information for sources that contributed to a grounded answer.
        /// This field is populated for GenerateAnswer calls.
        /// </summary>
        public List<GroundingAttribution>? GroundingAttributions { get; set; }

        /// <summary>
        /// Output only. The average log probability of the tokens in this candidate. This is a
        /// length-normalized score that can be used to compare the quality of candidates of different
        /// lengths. A higher average log probability suggests a more confident and coherent response.
        /// </summary>
        public float? AvgLogprobs { get; set; }

        /// <summary>
        /// Output only. The detailed log probability information for the tokens in this candidate. This
        /// is useful for debugging, understanding model uncertainty, and identifying potential
        /// </summary>
        public LogprobsResult? LogprobsResult { get; set; }

        /// <summary>
        /// Output only. Metadata related to url context retrieval tool.
        /// </summary>
        public UrlRetrievalMetadata? UrlRetrievalMetadata { get; set; }

        /// <summary>
        /// Output only. Metadata returned when the model uses the `url_context` tool to get information
        /// from a user-provided URL.
        /// </summary>
        public UrlContextMetadata? UrlContextMetadata { get; set; }
    }
}