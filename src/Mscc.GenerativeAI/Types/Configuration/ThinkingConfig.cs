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
        public int? ThinkingBudget { get; set; }
    }
}