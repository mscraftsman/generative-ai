namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Enables context window compression — a mechanism for managing the model's context window so that it does not exceed a given length.
    /// </summary>
    public partial class ContextWindowCompressionConfig
    {
        /// <summary>
        /// A sliding-window mechanism.
        /// </summary>
        public SlidingWindow? SlidingWindow { get; set; }
        /// <summary>
        /// The number of tokens (before running a turn) required to trigger a context window compression.
        /// This can be used to balance quality against latency as shorter context windows may result in
        /// faster model responses. However, any compression operation will cause a temporary latency increase,
        /// so they should not be triggered frequently. If not set, the default is 80% of the model's context
        /// window limit. This leaves 20% for the next user request/model response.
        /// </summary>
        public int? TriggerTokens { get; set; }
    }
}