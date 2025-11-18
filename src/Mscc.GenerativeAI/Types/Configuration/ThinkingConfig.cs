namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Config for thinking features.
    /// </summary>
    public class ThinkingConfig
    {
        /// <summary>
        /// Indicates whether to include thoughts in the response.
        /// If true, thoughts are returned only when available.
        /// </summary>
        public bool? IncludeThoughts { get; set; }
        /// <summary>
        /// The number of thoughts tokens that the model should generate.
        /// Value range: 0 to 24576
        /// </summary>
        /// <remarks>
        /// Gemini 2.5 Pro: The thinking budget 1 is invalid. Please choose a value between 128 and 32768.
        /// </remarks>
        public int? ThinkingBudget { get; set; }
        /// <summary>
        /// Optional. The level of thoughts tokens that the model should generate.
        /// </summary>
        public ThinkingLevel? ThinkingLevel { get; set; }
    }
}