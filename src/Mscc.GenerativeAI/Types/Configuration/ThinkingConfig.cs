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
        public bool IncludeThoughts { get; set; }
    }
}