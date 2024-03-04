using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    public class GenerationConfig
    {
        /// <summary>
        /// Optional. Controls the randomness of predictions.
        /// </summary>
        public float? Temperature { get; set; } = default;
        /// <summary>
        /// Optional. If specified, nucleus sampling will be used.
        /// </summary>
        public float? TopP { get; set; } = default;
        /// <summary>
        /// Optional. If specified, top-k sampling will be used.
        /// </summary>
        public int? TopK { get; set; } = default;
        /// <summary>
        /// Optional. Number of candidates to generate.
        /// </summary>
        public int? CandidateCount { get; set; } = default;
        /// <summary>
        /// Optional. The maximum number of output tokens to generate per message.
        /// </summary>
        public int? MaxOutputTokens { get; set; } = default;
        /// <summary>
        /// Optional. Stop sequences.
        /// </summary>
        public List<string>? StopSequences { get; set; }
    }
}
