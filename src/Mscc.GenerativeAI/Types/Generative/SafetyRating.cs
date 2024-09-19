using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Safety rating for a piece of content.
    /// Ref: https://ai.google.dev/api/rest/v1beta/SafetyRating
    /// </summary>
    [DebuggerDisplay("{Category}: {Probability} ({Blocked})")]
    public class SafetyRating
    {
        /// <summary>
        /// Output only. Required. The category for this rating.
        /// </summary>
        //public HarmCategory Category { get; internal set; }
        public HarmCategory Category { get; set; } = default;
        /// <summary>
        /// Output only. Required. The probability of harm for this content.
        /// </summary>
        //public HarmProbability Probability { get; internal set; }
        public HarmProbability Probability { get; set; } = default;
        /// <summary>
        /// Output only. Indicates whether the content was filtered out because of this rating.
        /// </summary>
        public bool? Blocked { get; set; }
        /// <summary>
        /// Output only. Harm probability scoring in the content.
        /// Vertex AI only
        /// </summary>
        public float? ProbabilityScore { get; set; }
        /// <summary>
        /// Output only. Harm severity levels in the content.
        /// Vertex AI only
        /// </summary>
        public HarmSeverity? Severity { get; set; }
        //public string? Severty {  get; set; }
        /// <summary>
        /// Output only. Harm severity scoring in the content.
        /// Vertex AI only
        /// </summary>
        public float? SeverityScore { get; set; }
    }
}
