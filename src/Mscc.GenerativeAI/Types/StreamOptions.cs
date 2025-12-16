namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Options for streaming requests.
    /// </summary>
    public partial class StreamOptions
    {
        /// <summary>
        /// Optional. If set, include usage statistics in the response.
        /// </summary>
        public bool IncludeUsage { get; set; } = false;
    }
}