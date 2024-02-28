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
        public string Category { get; set; } = default;
        /// <summary>
        /// Output only. Harm probability levels in the content.
        /// </summary>
        //public HarmProbability Probability { get; internal set; }
        public string Probability { get; set; } = default;
        /// <summary>
        /// Output only. Indicates whether the content was filtered out because of this rating.
        /// </summary>
        public bool Blocked { get; set; }
    }
}
