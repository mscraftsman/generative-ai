namespace Mscc.GenerativeAI
{
    public class UsageMetadata
    {
        /// <summary>
        /// Number of tokens in the request.
        /// </summary>
        public int PromptTokenCount { get; set; } = default;
        /// <summary>
        /// Number of tokens in the response(s).
        /// </summary>
        public int CandidatesTokenCount { get; set; } = default;
        /// <summary>
        /// Number of tokens in the response(s).
        /// </summary>
        public int TotalTokenCount { get; set; } = default;
        /// <summary>
        /// Number of tokens in the cached content.
        /// </summary>
        public int CachedContentTokenCount { get; set; } = default;
    }
}