using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    [DebuggerDisplay("{Category}: {Probability} ({Blocked})")]
    public class SafetyRating
    {
        /// <summary>
        /// Output only. Harm category.
        /// </summary>
        //public HarmCategory Category { get; internal set; }
        public HarmCategory Category { get; set; } = default;
        /// <summary>
        /// Output only. Harm probability levels in the content.
        /// </summary>
        //public HarmProbability Probability { get; internal set; }
        public HarmProbability Probability { get; set; } = default;
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
        /// <summary>
        /// Output only. Indicates whether the content was filtered out because of this rating.
        /// Vertex AI only
        /// </summary>
        public bool? Blocked { get; set; }
    }
}
