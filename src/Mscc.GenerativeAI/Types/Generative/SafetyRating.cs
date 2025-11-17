using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A safety rating for a piece of content. The safety rating contains the harm category and the
    /// harm probability level.
    /// Ref: https://ai.google.dev/api/rest/v1beta/SafetyRating
    /// </summary>
    [DebuggerDisplay("{Category}: {Probability} ({Blocked})")]
    public class SafetyRating
    {
        /// <summary>
        /// Output only. The harm category of this rating.
        /// </summary>
        public HarmCategory? Category { get; set; }
        /// <summary>
        /// Output only. The overwritten threshold for the safety category of Gemini 2.0 image out. If
        /// minors are detected in the output image, the threshold of each safety category will be
        /// overwritten if user sets a lower threshold. This field is not supported in Gemini API.
        /// </summary>
        public HarmBlockThreshold? OverwrittenThreshold { get; set; }
        /// <summary>
        /// Output only. The probability of harm for this category.
        /// </summary>
        public HarmProbability? Probability { get; set; }
        /// <summary>
        /// Output only. Indicates whether the content was blocked because of this rating.
        /// </summary>
        public bool? Blocked { get; set; }
        /// <summary>
        /// Output only. The probability score of harm for this category.
        /// </summary>
        /// <remarks>
        /// This field is not supported in Gemini API.
        /// </remarks>
        public float? ProbabilityScore { get; set; }
        /// <summary>
        /// Output only. The severity of harm for this category.
        /// </summary>
        /// <remarks>
        /// This field is not supported in Gemini API.
        /// </remarks>
        public HarmSeverity? Severity { get; set; }
        /// <summary>
        /// Output only. The severity score of harm for this category.
        /// </summary>
        /// <remarks>
        /// This field is not supported in Gemini API.
        /// </remarks>
        public float? SeverityScore { get; set; }
    }
}
