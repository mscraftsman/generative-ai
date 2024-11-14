namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Options for streaming requests.
    /// </summary>
    public class StreamOptions
    {
        /// <summary>
        /// Optional. If set, include usage statistics in the response.
        /// </summary>
        public bool IncludeUsage { get; set; } = false;
    }
}